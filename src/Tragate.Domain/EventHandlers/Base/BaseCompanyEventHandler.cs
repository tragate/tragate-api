using Nest;
using Tragate.Common.Library.Constants;
using Tragate.Common.Library.Dto;
using Tragate.Domain.Interfaces;

namespace Tragate.Domain.EventHandlers.Base
{
    public class BaseCompanyEventHandler
    {
        private readonly ElasticClient _elasticClient;
        private readonly ICompanyRepository _companyRepository;

        protected BaseCompanyEventHandler(ElasticClient elasticClient, ICompanyRepository companyRepository){
            _elasticClient = elasticClient;
            _companyRepository = companyRepository;
        }

        protected virtual void IndexDocument(int id){
            var company = _companyRepository.GetCompanyDetailById(id);
            company.JoinField = JoinField.Root<CompanyDto>();
            _elasticClient.Index<Root>(company, x => x.Index(TragateConstants.ALIAS));
        }
    }
}