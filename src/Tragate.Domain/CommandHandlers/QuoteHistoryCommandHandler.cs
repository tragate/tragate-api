using AutoMapper;
using MediatR;
using Tragate.Common.Library.Dto;
using Tragate.Common.Library.Enum;
using Tragate.Domain.Commands;
using Tragate.Domain.Core.Bus;
using Tragate.Domain.Core.Notifications;
using Tragate.Domain.Events;
using Tragate.Domain.Interfaces;
using Tragate.Domain.Models;

namespace Tragate.Domain.CommandHandlers
{
    public class QuoteHistoryCommandHandler : CommandHandler,
        INotificationHandler<AddNewQuoteHistoryCommand>
    {
        private readonly IMapper _mapper;
        private readonly IQuoteHistoryRepository _quoteHistoryRepository;
        private readonly IUserRepository _userRepository;
        private readonly IQuoteRepository _quoteRepository;

        public QuoteHistoryCommandHandler(
            IMapper mapper,
            IUnitOfWork uow,
            IMediatorHandler bus,
            IQuoteRepository quoteRepository,
            IQuoteHistoryRepository quoteHistoryRepository,
            INotificationHandler<DomainNotification> notifications, IUserRepository userRepository) : base(uow, bus,
            notifications){
            _mapper = mapper;
            _quoteHistoryRepository = quoteHistoryRepository;
            _userRepository = userRepository;
            _quoteRepository = quoteRepository;
        }


        public void Handle(AddNewQuoteHistoryCommand message){
            base.Validate(message);
            if (message.IsValid()){
                var entity = _mapper.Map<QuoteHistory>(message);
                _quoteHistoryRepository.Add(entity);
                if (base.Commit()){
                    var quote = _quoteRepository.GetById(message.QuoteId);
                    this.SendUpdatedDateQuoteCommand(message.QuoteId);
                    this.SendUpdateQuoteContactStatusCommand(quote, message.CreatedUserId);
                    this.SendQuoteHisyoryCreatedEvent(quote, message.CreatedUserId);
                }
            }
        }

        private void SendUpdatedDateQuoteCommand(int quoteId){
            base.SendCommand(new UpdatedDateQuoteCommand()
            {
                Id = quoteId
            });
        }

        private void SendUpdateQuoteContactStatusCommand(Quote quote, int createdUserId){
            base.SendCommand(new UpdateQuoteContactStatusCommand
            {
                Id = quote.Id,
                SellerContactStatusId = quote.BuyerUserId == createdUserId
                    ? (int) QuoteContactStatusType.Waiting_Seller_Response
                    : 0,
                BuyerContactStatusId = quote.SellerUserId == createdUserId
                    ? (int) QuoteContactStatusType.Waiting_Buyer_Response
                    : 0
            });
        }

        /// <summary>
        /// eger buyerUserId create eden ise mail satıcıya gider yani receiverUserId satıcıdır.
        /// satıcı anonim olamaz.ilk if case'i mutlaka alıcıya ait olmaz zorundadır.
        /// ama hem seller hem buyer anonim olmayabilir. else'de ise artık mail , alıcı firmanın
        /// tüm adminlerine gidiyor. tabi receiverCompanyId'yi de bulmak mesele artık o yuzden
        /// receiverUserId gibi onu da ufak bir if case'ine tabi tutuyoruz . Not:Quotation konusu
        /// seller user-buyer user / seller company-buyer company ve hangisi receiver hangisi sender gibi
        /// problemlere gebe olan bir business oldugu için iyi anlaşılması lazım. 
        /// </summary>
        /// <param name="quote"></param>
        /// <param name="createdUserId"></param>
        private void SendQuoteHisyoryCreatedEvent(Quote quote, int createdUserId){
            int receiverUserId = quote.BuyerUserId == createdUserId
                ? quote.SellerUserId.Value
                : quote.BuyerUserId.Value;

            int receiverCompanyId = quote.BuyerUserId == createdUserId
                ? quote.SellerCompanyId
                : quote.BuyerCompanyId.Value;

            var receiveruser = _userRepository.GetById(receiverUserId);
            if (receiveruser.RegisterTypeId == (byte) RegisterType.Anonymous){
                base.RaiseEvent(new AnonymUserCreatedEvent()
                {
                    UserId = receiveruser.Id,
                    Subject = $"New Message : {quote.Title}",
                });
            }
            else{
                var senderUser = _userRepository.GetById(createdUserId);
                base.RaiseEvent(new QuoteHistoryCreatedEvent()
                {
                    QuoteId = quote.Id,
                    Title = quote.Title,
                    CreatedUserId = createdUserId,
                    CompanyId = receiverCompanyId,
                    ReceiverUser = _mapper.Map<QuoteUserDto>(receiveruser),
                    SenderUser = _mapper.Map<QuoteUserDto>(senderUser)
                });
            }
        }
    }
}