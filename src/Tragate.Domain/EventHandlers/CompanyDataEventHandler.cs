using AutoMapper;
using MediatR;
using Nest;
using Tragate.Common.Library.Constants;
using Tragate.Common.Library.Dto;
using Tragate.Domain.Events.CompanyData;
using Tragate.Domain.Interfaces;

namespace Tragate.Domain.EventHandlers
{
    public class CompanyDataEventHandler : INotificationHandler<CompanyDataReferenceUpdatedEvent>
    {
        private readonly ElasticClient _elasticClient;
        private readonly ICompanyDataRepository _companyDataRepository;
        private readonly IMapper _mapper;

        public CompanyDataEventHandler(ElasticClient elasticClient, ICompanyDataRepository companyDataRepository,
            IMapper mapper){
            _elasticClient = elasticClient;
            _companyDataRepository = companyDataRepository;
            _mapper = mapper;
        }

        public void Handle(CompanyDataReferenceUpdatedEvent message){
            var result = _companyDataRepository.GetById(message.Id);
            var companyData = _mapper.Map<CompanyDataSearchDto>(result);
            _elasticClient.Index(companyData, x => x.Index(TragateConstants.COMPANY_DATA));
        }
    }
}