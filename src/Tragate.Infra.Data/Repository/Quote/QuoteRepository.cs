using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Tragate.Common.Library.Dto;
using Tragate.Common.Library.Enum;
using Tragate.Common.Library.Helpers;
using Tragate.Domain.Core.Bus;
using Tragate.Domain.Core.Notifications;
using Tragate.Domain.Interfaces;
using Tragate.Domain.Models;
using Tragate.Infra.Data.Context;
using Tragate.Infra.Data.Repository;

namespace Tragate.Infra.Data
{
    public class QuoteRepository : Repository<Quote>, IQuoteRepository
    {
        private readonly IMediatorHandler _bus;
        private readonly IUserRepository _userRepository;
        private readonly ICompanyRepository _companyRepository;
        public readonly IDbConnection _db;

        public QuoteRepository(TragateContext context, IMediatorHandler bus,
            IUserRepository userRepository,
            ICompanyRepository companyRepository,
            IDbConnection db) : base(context){
            _bus = bus;
            _userRepository = userRepository;
            _companyRepository = companyRepository;
            _db = db;
        }

        public IEnumerable<QuoteListDto> GetQuotes(int page, int pageSize, QuoteStatusType quoteStatus,
            OrderStatusType orderStatus,
            int? sellerUserId = null, int? sellerCompanyId = null, int? buyerUserId = null, int? buyerCompanyId = null){
            page -= 1;
            var sb = new StringBuilder();
            sb.AppendLine(@"SELECT
                              q.*,
                              bu.*,
                              su.*,
                              sc.*,
                              buc.*,
                              suc.*,
                              os.Id,
                              os.ParameterValue1  AS Value,
                              bcs.Id,
                              bcs.ParameterValue1 AS Value,
                              scs.Id,
                              scs.ParameterValue1 AS Value
                        FROM Quote q
                          INNER JOIN [User] bu ON bu.Id = q.BuyerUserId
                          INNER JOIN [User] su ON su.Id = q.SellerUserId
                          INNER JOIN [User] sc ON sc.Id = q.SellerCompanyId
                          INNER JOIN Location buc ON buc.Id = bu.CountryId
                          INNER JOIN Location suc ON suc.Id = su.CountryId
                          INNER JOIN Parameter os ON os.ParameterType = 'OrderStatusId' AND os.ParameterCode = q.OrderStatusId
                          INNER JOIN Parameter bcs ON bcs.ParameterType = 'QuoteContactStatusId' AND bcs.ParameterCode = q.BuyerContactStatusId
                          INNER JOIN Parameter scs ON scs.ParameterType = 'QuoteContactStatusId' AND scs.ParameterCode = q.SellerContactStatusId
                        WHERE 1 = 1");

            if (quoteStatus != QuoteStatusType.All)
                sb.AppendLine($"and q.QuoteStatusId={(int) quoteStatus}");

            if (orderStatus != OrderStatusType.All)
                sb.AppendLine($"and q.OrderStatusId={(int) orderStatus}");

            if (sellerUserId.HasValue)
                sb.AppendLine($"and q.SellerUserId={sellerUserId}");

            if (sellerCompanyId.HasValue)
                sb.AppendLine($"and q.SellerCompanyId={sellerCompanyId}");

            if (buyerUserId.HasValue)
                sb.AppendLine($"and q.BuyerUserId={buyerUserId}");

            if (buyerCompanyId.HasValue)
                sb.AppendLine($"and q.BuyerCompanyId={buyerCompanyId}");


            sb.AppendLine($"ORDER BY q.UpdatedDate desc OFFSET {page * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY");
            return _db.Query(sb.ToString(), new[]
            {
                typeof(QuoteListDto), typeof(QuoteUserDto),
                typeof(QuoteUserDto), typeof(QuoteCompanyDto),
                typeof(LocationDto), typeof(LocationDto),
                typeof(ParameterDto), typeof(ParameterDto), typeof(ParameterDto),
            }, (objects) =>
            {
                var q = (QuoteListDto) objects[0];
                q.BuyerUser = (QuoteUserDto) objects[1];
                q.SellerUser = (QuoteUserDto) objects[2];
                q.SellerCompany = (QuoteCompanyDto) objects[3];
                q.BuyerUser.Country = ((LocationDto) objects[4]).Name;
                q.SellerUser.Country = ((LocationDto) objects[5]).Name;
                q.OrderStatus = ((ParameterDto) objects[6]).Value;
                q.BuyerContactStatus = ((ParameterDto) objects[7]).Value;
                q.SellerContactStatus = ((ParameterDto) objects[8]).Value;
                q.BuyerUser.ProfileImagePath = q.BuyerUser.ProfileImagePath.CheckUserProfileImage();
                q.SellerUser.ProfileImagePath = q.SellerUser.ProfileImagePath.CheckUserProfileImage();
                q.SellerCompany.ProfileImagePath = q.SellerCompany.ProfileImagePath.CheckCompanyProfileImage();

                return q;
            });
        }

        public int CountQuotes(QuoteStatusType quoteStatus, OrderStatusType orderStatus, int? sellerUserId = null,
            int? sellerCompanyId = null, int? buyerUserId = null, int? buyerCompanyId = null){
            var sb = new StringBuilder();
            sb.AppendLine("SELECT count(*) FROM Quote q where 1=1");

            if (quoteStatus != QuoteStatusType.All)
                sb.AppendLine($"and q.QuoteStatusId={(int) quoteStatus}");

            if (orderStatus != OrderStatusType.All)
                sb.AppendLine($"and q.OrderStatusId={(int) orderStatus}");

            if (sellerUserId.HasValue)
                sb.AppendLine($"and q.SellerUserId={sellerUserId}");

            if (sellerCompanyId.HasValue)
                sb.AppendLine($"and q.SellerCompanyId={sellerCompanyId}");

            if (buyerUserId.HasValue)
                sb.AppendLine($"and q.BuyerUserId={buyerUserId}");

            if (buyerCompanyId.HasValue)
                sb.AppendLine($"and q.BuyerCompanyId={buyerCompanyId}");

            return _db.ExecuteScalar<int>(sb.ToString());
        }

        public QuoteDto GetQuoteById(int id){
            var sb = new StringBuilder();
            sb.AppendLine($@"SELECT
                                  q.*,
                                  bu.*,
                                  su.*,
                                  sc.*,
                                  bc.*,
                                  sua.*,
                                  iua.*,
                                  os.Id,
                                  os.ParameterValue1     AS Value,
                                  bcs.Id,
                                  bcs.ParameterValue1    AS Value,
                                  scs.Id,
                                  scs.ParameterValue1    AS Value,
                                  crncy.Id,
                                  crncy.ParameterValue1  AS Value,
                                  trterm.Id,
                                  trterm.ParameterValue1 AS Value,
                                  shp.Id,
                                  shp.ParameterValue1    AS Value
                                FROM Quote q
                                  INNER JOIN [User] bu ON bu.Id = q.BuyerUserId
                                  INNER JOIN [User] su ON su.Id = q.SellerUserId
                                  INNER JOIN [User] sc ON sc.Id = q.SellerCompanyId
                                  INNER JOIN Parameter os ON os.ParameterType = 'OrderStatusId' AND os.ParameterCode = q.OrderStatusId
                                  INNER JOIN Parameter bcs ON bcs.ParameterType = 'QuoteContactStatusId' AND bcs.ParameterCode = q.BuyerContactStatusId
                                  INNER JOIN Parameter scs ON scs.ParameterType = 'QuoteContactStatusId' AND scs.ParameterCode = q.SellerContactStatusId
                                  LEFT JOIN [User] bc ON bc.Id = q.BuyerCompanyId
                                  LEFT JOIN UserAddress sua ON sua.Id = q.ShipmentUserAddressId
                                  LEFT JOIN UserAddress iua ON iua.Id = q.InvoiceUserAddressId
                                  LEFT JOIN Parameter crncy ON crncy.ParameterType = 'CurrencyId' AND crncy.ParameterCode = q.CurrencyId
                                  LEFT JOIN Parameter trterm ON trterm.ParameterType = 'TradeTermId' AND trterm.ParameterCode = q.TradeTermId
                                  LEFT JOIN Parameter shp ON shp.ParameterType = 'ShippingMethodId' AND shp.ParameterCode = q.ShippingMethodId
                                WHERE q.Id = {id}");

            return _db.Query(sb.ToString(), new[]
            {
                typeof(QuoteDto), typeof(QuoteUserDto), typeof(QuoteUserDto), typeof(QuoteCompanyDto),
                typeof(QuoteCompanyDto), typeof(UserAddressDto), typeof(UserAddressDto), typeof(ParameterDto),
                typeof(ParameterDto), typeof(ParameterDto), typeof(ParameterDto), typeof(ParameterDto),
                typeof(ParameterDto)
            }, (objects) =>
            {
                var q = (QuoteDto) objects[0];
                q.BuyerUser = (QuoteUserDto) objects[1];
                q.SellerUser = (QuoteUserDto) objects[2];
                q.SellerCompany = (QuoteCompanyDto) objects[3];
                q.BuyerCompany = objects[4] as QuoteCompanyDto;
                q.ShipmentUserAddress = (objects[5] as UserAddressDto)?.Address;
                q.InvoiceUserAddress = (objects[6] as UserAddressDto)?.Address;
                q.OrderStatus = ((ParameterDto) objects[7]).Value;
                q.BuyerContactStatus = ((ParameterDto) objects[8]).Value;
                q.SellerContactStatus = ((ParameterDto) objects[9]).Value;
                q.Currency = (objects[10] as ParameterDto)?.Value;
                q.TradeTerm = (objects[11] as ParameterDto)?.Value;
                q.ShippingMethod = (objects[12] as ParameterDto)?.Value;
                q.BuyerUser.ProfileImagePath = q.BuyerUser.ProfileImagePath.CheckUserProfileImage();
                q.SellerUser.ProfileImagePath = q.SellerUser.ProfileImagePath.CheckUserProfileImage();
                q.SellerCompany.ProfileImagePath = q.SellerCompany.ProfileImagePath.CheckCompanyProfileImage();
                if (q.BuyerCompany != null)
                    q.BuyerCompany.ProfileImagePath = q.BuyerCompany.ProfileImagePath.CheckCompanyProfileImage();

                return q;
            }).SingleOrDefault();
        }

        public QuoteCountDto GetNotificationCountByUserId(int id){
            var count = new QuoteCountDto()
            {
                WaitingBuyerCount =
                    _db.ExecuteScalar<int>(
                        $@"select count(*) from Quote where BuyerUserId={id} and 
                           BuyerContactStatusId={(int) QuoteContactStatusType.Waiting_Buyer_Response}"),
                WaitingSellerCount = _db.ExecuteScalar<int>(
                    $@"select count(*) from Quote where SellerUserId={id} and 
                           SellerContactStatusId={(int) QuoteContactStatusType.Waiting_Seller_Response}"),
            };

            return count;
        }


        public Quote Add(User user, Quote quote, QuoteProduct quoteProduct){
            var transaction = Db.Database.BeginTransaction(IsolationLevel.ReadUncommitted);
            try{
                quote = FindBuyerAndCreatedUser(quote, user);
                if (quote == null) return null;
                quote = SetSellerUserId(quote);
                Db.Quote.Add(quote);
                Db.SaveChanges();
                if (quoteProduct.ProductId.HasValue || !string.IsNullOrEmpty(quoteProduct.Note)){
                    quoteProduct.QuoteId = quote.Id;
                    quoteProduct.CreatedUserId = quote.CreatedUserId;
                    Db.QuoteProduct.Add(quoteProduct);
                    Db.SaveChanges();
                }

                transaction.Commit();
            }
            catch (Exception ex){
                _bus.RaiseEvent(new DomainNotification("Quote.Add",
                    "We had a problem during saving your data."));
                Log.Error(ex, "An error occured when user add transaction !");
                transaction.Rollback();
                return null;
            }

            return quote;
        }

        /// <summary>
        /// Aşagıdaki bütün mesele;Anonim bir kullanıcının buyerUserId ve CreatedUserId alanlarının oluşturulması içindir. 
        /// </summary>
        /// <param name="quote"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        private Quote FindBuyerAndCreatedUser(Quote quote, User user){
            if (!quote.BuyerUserId.HasValue){
                var buyerUser = _userRepository.GetByEmail(user.Email);
                if (buyerUser == null){
                    Db.User.Add(user);
                    Db.SaveChanges();
                    quote = SetBuyerUserIdAndCreatedUserId(quote, user.Id);
                }
                else{
                    if (buyerUser.RegisterType != RegisterType.Anonymous){
                        _bus.RaiseEvent(new DomainNotification("Quote.User.Get",
                            "You need to login to get this Quote"));
                        return null;
                    }

                    //buyer tekrar quote create etmek istediğinde hala anonim olarak anonim olan kendi user'ını
                    //tekrar set edecektir bu yuzden createdUserId hala nulldır.
                    if (buyerUser.RegisterType == RegisterType.Anonymous){
                        quote = SetBuyerUserIdAndCreatedUserId(quote, buyerUser.Id);
                    }
                }
            }

            return quote;
        }

        /// <summary>
        /// CreatedUserId'nin null olma ihtimali sadece buyer'ın quote oluşturma sürecinde geçerlidir,
        /// Oyle ki seller zaten createdUserId'si olan bir userdır.
        /// </summary>
        /// <param name="quote"></param>
        /// <param name="userId"></param>
        private Quote SetBuyerUserIdAndCreatedUserId(Quote quote, int userId){
            quote.BuyerUserId = userId;
            quote.CreatedUserId = quote.CreatedUserId == 0 ? userId : quote.CreatedUserId;

            return quote;
        }

        private Quote SetSellerUserId(Quote quote){
            if (!quote.SellerUserId.HasValue){
                quote.SellerUserId = _companyRepository.GetByUserId(quote.SellerCompanyId).ContactUserId;
            }

            return quote;
        }
    }
}