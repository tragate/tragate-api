using System;
using AutoMapper;
using MediatR;
using Tragate.Common.Library.Enum;
using Tragate.Domain.Commands;
using Tragate.Domain.Core.Bus;
using Tragate.Domain.Core.Notifications;
using Tragate.Domain.Events;
using Tragate.Domain.Interfaces;
using Tragate.Domain.Models;

namespace Tragate.Domain.CommandHandlers
{
    public class QuoteCommandHandler : CommandHandler,
        INotificationHandler<AddNewQuoteCommand>,
        INotificationHandler<UpdateQuoteContactStatusCommand>,
        INotificationHandler<UpdatedDateQuoteCommand>,
        INotificationHandler<UpdateQuoteStatusCommand>
    {
        private readonly IQuoteRepository _quoteRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public QuoteCommandHandler(
            IQuoteRepository quoteRepository,
            IUnitOfWork uow,
            IMediatorHandler bus,
            INotificationHandler<DomainNotification> notifications, IMapper mapper, IUserRepository userRepository) :
            base(uow, bus, notifications){
            _quoteRepository = quoteRepository;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        /// <summary>
        /// user kontrolü ve anonim user creation
        ///  UserId ise direkt olarak devam edilir yoksa useremail alanı vardır.
        ///  useremail dbde yok ise anonim olarak create edilir
        ///  useremail db var ve anonim ise aynı anonim user üzerinden devam edilir.
        ///  useremail registerTypeId = Tragateuser ise login'e yonlendirilir -> error code doner ??
        /// quote creation
        /// quote product creation (productId veya product Note alanı var ise kayıt at ikisi de yok ise pas geç)
        /// quote history created mesajı ile creation(INotifyPropertyChanged ile ??)(event raise)
        /// sellerCompanyId'e ait userlar için bir event raise edilir ve quotation created maili atılır.(event raise)
        /// error code alanı domain notification alanına eklenerek responseda doner
        /// createdUserId null ise gelen userId yukardaki validasyonlardan bellidir.Değilse o dur :)
        /// </summary>
        /// <param name="message"></param>
        public void Handle(AddNewQuoteCommand message){
            base.Validate(message);
            if (message.IsValid()){
                var user = _mapper.Map<User>(message);
                var quote = _mapper.Map<Quote>(message);
                var quoteProduct = _mapper.Map<QuoteProduct>(message);
                var quoteResult = _quoteRepository.Add(user, quote, quoteProduct);
                if (quoteResult != null){
                    var quoteHistoryCommand = _mapper.Map<AddNewQuoteHistoryCommand>(quote);
                    base.SendCommand(quoteHistoryCommand);
                    if (_userRepository.GetById(quoteResult.BuyerUserId.Value).RegisterTypeId ==
                        (byte) RegisterType.Anonymous){
                        base.RaiseEvent(new AnonymUserCreatedEvent()
                        {
                            UserId = quoteResult.BuyerUserId.Value,
                        });
                    }
                }
            }
        }

        /// <summary>
        /// TODO:Refactor => mapper içinde yapılırsa daha temiz olur !
        /// </summary>
        /// <param name="message"></param>
        public void Handle(UpdateQuoteContactStatusCommand message){
            base.Validate(message);
            if (message.IsValid()){
                var entity = _quoteRepository.GetById(message.Id);
                if (message.BuyerContactStatusId != 0)
                    entity.BuyerContactStatusId = message.BuyerContactStatusId;

                if (message.SellerContactStatusId != 0)
                    entity.SellerContactStatusId = message.SellerContactStatusId;

                _quoteRepository.Update(entity);
                base.Commit();
            }
        }

        public void Handle(UpdatedDateQuoteCommand message){
            base.Validate(message);
            if (message.IsValid()){
                var quote = _quoteRepository.GetById(message.Id);
                quote.UpdatedDate = DateTime.Now;
                _quoteRepository.Update(quote);
                base.Commit();
            }
        }

        public void Handle(UpdateQuoteStatusCommand message){
            base.Validate(message);
            if (message.IsValid()){
                var entity = _quoteRepository.GetById(message.Id);
                if (message.QuoteStatusId != 0)
                    entity.QuoteStatusId = message.QuoteStatusId;

                if (message.OrderStatusId != 0)
                    entity.OrderStatusId = message.OrderStatusId;

                _quoteRepository.Update(entity);
                base.Commit();
            }
        }
    }
}