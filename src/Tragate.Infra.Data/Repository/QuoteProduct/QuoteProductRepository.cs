using System.Collections.Generic;
using System.Data;
using Dapper;
using Tragate.Common.Library.Dto;
using Tragate.Common.Library.Helpers;
using Tragate.Domain.Interfaces;
using Tragate.Domain.Models;
using Tragate.Infra.Data.Context;

namespace Tragate.Infra.Data.Repository
{
    public class QuoteProductRepository : Repository<QuoteProduct>, IQuoteProductRepository
    {
        public readonly IDbConnection _db;

        public QuoteProductRepository(TragateContext context, IDbConnection db) : base(context){
            _db = db;
        }

        public IEnumerable<QuoteProductDto> GetQuoteProductsById(int id){
            var query = $@"SELECT
                            qp.*,
                            p.*,  
                            u.*,
                            pr.Id,
                            pr.ParameterValue1 as Value
                         FROM QuoteProduct qp
                            INNER JOIN Product p ON p.Id = qp.ProductId
                            INNER JOIN [User] u ON u.Id = qp.CreatedUserId
                            LEFT JOIN Parameter pr ON pr.ParameterType = 'UnitTypeId' AND pr.ParameterCode = qp.UnitTypeId
                        WHERE qp.QuoteId = {id}";

            return _db.Query<QuoteProductDto, ProductDto, User, ParameterDto, QuoteProductDto>(query,
                (qp, p, u, pr) =>
                {
                    qp.Product = p.Title;
                    qp.ListImagePath = p.ListImagePath.CheckProductProfileImage();
                    qp.CreatedUser = u.FullName;
                    qp.UnitType = pr?.Value;
                    return qp;
                });
        }
    }
}