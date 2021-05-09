using System;
using System.Collections.Generic;
using AutoMapper;
using Tragate.Application.ViewModels;
using Tragate.Common.Library;
using Tragate.Common.Library.Dto;
using Tragate.Common.Library.Enum;
using Tragate.Domain.Commands;
using Tragate.Domain.Core.Bus;
using Tragate.Domain.Interfaces;

namespace Tragate.Application
{
    public class QuoteService : IQuoteService
    {
        private readonly IMapper _mapper;
        private readonly IMediatorHandler _bus;
        private readonly IQuoteRepository _quoteRepository;

        public QuoteService(IMapper mapper, IMediatorHandler bus, IQuoteRepository quoteRepository){
            _mapper = mapper;
            _bus = bus;
            _quoteRepository = quoteRepository;
        }

        public IEnumerable<QuoteListDto> GetQuotes(int page, int pageSize, QuoteStatusType quoteStatus,
            OrderStatusType orderStatus,
            int? sellerUserId = null, int? sellerCompanyId = null, int? buyerUserId = null, int? buyerCompanyId = null){
            return _quoteRepository.GetQuotes(page, pageSize, quoteStatus, orderStatus, sellerUserId, sellerCompanyId,
                buyerUserId, buyerCompanyId);
        }

        public int CountQuotes(QuoteStatusType quoteStatus, OrderStatusType orderStatus, int? sellerUserId = null,
            int? sellerCompanyId = null, int? buyerUserId = null, int? buyerCompanyId = null){
            return _quoteRepository.CountQuotes(quoteStatus, orderStatus, sellerUserId, sellerCompanyId, buyerUserId,
                buyerCompanyId);
        }

        public IEnumerable<QuoteListDto> GetQuotesByUserId(int page, int pageSize, QuoteStatusType quoteStatus,
            OrderStatusType orderStatus, int? sellerUserId = null, int? buyerUserId = null){
            return _quoteRepository.GetQuotes(page, pageSize, quoteStatus, orderStatus, sellerUserId, null,
                buyerUserId);
        }

        public int CountQuotesByUserId(QuoteStatusType quoteStatus, OrderStatusType orderStatus,
            int? sellerUserId = null, int? buyerUserId = null){
            return _quoteRepository.CountQuotes(quoteStatus, orderStatus, sellerUserId, null, buyerUserId);
        }

        public IEnumerable<QuoteListDto> GetQuotesByCompanyId(int page, int pageSize, QuoteStatusType quoteStatus,
            OrderStatusType orderStatus,
            int? sellerCompanyId = null, int? buyerCompanyId = null){
            return _quoteRepository.GetQuotes(page, pageSize, quoteStatus, orderStatus, null, sellerCompanyId, null,
                buyerCompanyId);
        }

        public int CountQuotesByCompanyId(QuoteStatusType quoteStatus, OrderStatusType orderStatus,
            int? sellerCompanyId = null, int? buyerCompanyId = null){
            return _quoteRepository.CountQuotes(quoteStatus, orderStatus, null, sellerCompanyId, null,
                buyerCompanyId);
        }

        public QuoteDto GetQuoteById(int id){
            return _quoteRepository.GetQuoteById(id);
        }

        public QuoteCountDto GetNotificationCountByUserId(int id){
            return _quoteRepository.GetNotificationCountByUserId(id);
        }

        public void CreateQuote(CreateQuoteViewModel model){
            var createCommand = _mapper.Map<AddNewQuoteCommand>(model);
            _bus.SendCommand(createCommand);
        }

        public void UpdateQuoteContactStatus(int id, int buyerContactStatusId,
            int sellerContactStatusId){
            _bus.SendCommand(new UpdateQuoteContactStatusCommand()
            {
                Id = id,
                BuyerContactStatusId = buyerContactStatusId,
                SellerContactStatusId = sellerContactStatusId
            });
        }

        public void UpdateQuoteStatus(int id, int quoteStatusId, int orderStatusId){
            _bus.SendCommand(new UpdateQuoteStatusCommand()
            {
                Id = id,
                QuoteStatusId = quoteStatusId,
                OrderStatusId = orderStatusId
            });
        }

        public void Dispose(){
            GC.SuppressFinalize(this);
        }
    }
}