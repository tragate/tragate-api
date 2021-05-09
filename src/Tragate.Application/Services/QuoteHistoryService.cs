using System;
using System.Collections.Generic;
using AutoMapper;
using Tragate.Application.ViewModels;
using Tragate.Common.Library;
using Tragate.Domain.Commands;
using Tragate.Domain.Core.Bus;
using Tragate.Domain.Interfaces;

namespace Tragate.Application
{
    public class QuoteHistoryService : IQuoteHistoryService
    {
        private readonly IMapper _mapper;
        private readonly IMediatorHandler _bus;
        private readonly IQuoteHistoryRepository _quoteHistoryRepository;

        public QuoteHistoryService(IMapper mapper, IMediatorHandler bus,
            IQuoteHistoryRepository quoteHistoryRepository){
            _mapper = mapper;
            _bus = bus;
            _quoteHistoryRepository = quoteHistoryRepository;
        }

        public IEnumerable<QuoteHistoryDto> GetQuoteHistoriesById(int id){
            return _quoteHistoryRepository.GetQuoteHistoriesById(id);
        }

        public void CreateQuoteHistory(CreateQuoteHistoryViewModel model){
            var createCommand = _mapper.Map<AddNewQuoteHistoryCommand>(model);
            _bus.SendCommand(createCommand);
        }

        public void Dispose(){
            GC.SuppressFinalize(this);
        }
    }
}