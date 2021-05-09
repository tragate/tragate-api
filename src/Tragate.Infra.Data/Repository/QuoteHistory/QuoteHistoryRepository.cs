using System.Collections.Generic;
using System.Data;
using Dapper;
using Tragate.Common.Library;
using Tragate.Common.Library.Dto;
using Tragate.Common.Library.Helpers;
using Tragate.Domain.Interfaces;
using Tragate.Domain.Models;
using Tragate.Infra.Data.Context;
using Tragate.Infra.Data.Repository;

namespace Tragate.Infra.Data
{
    public class QuoteHistoryRepository : Repository<QuoteHistory>, IQuoteHistoryRepository
    {
        private readonly IDbConnection _db;

        public QuoteHistoryRepository(TragateContext context, IDbConnection db) : base(context){
            _db = db;
        }

        public IEnumerable<QuoteHistoryDto> GetQuoteHistoriesById(int id){
            var query = $@"
                     SELECT 
                        qh.*,
                        u.*
                     FROM QuoteHistory qh
                        INNER JOIN [User] u ON u.Id = qh.CreatedUserId
                     WHERE qh.QuoteId = {id}";
            return _db.Query<QuoteHistoryDto, UserDto, QuoteHistoryDto>(query, (qu, u) =>
            {
                qu.CreatedUser = u.FullName;
                qu.ProfileImagePath = u.ProfileImagePath.CheckUserProfileImage();
                return qu;
            });
        }
    }
}