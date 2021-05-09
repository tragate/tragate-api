using System;
using System.Collections.Generic;
using System.Data;
using Serilog;
using Tragate.Common.Library.Enum;
using Tragate.Domain.Core.Bus;
using Tragate.Domain.Core.Notifications;
using Tragate.Domain.Interfaces;
using Tragate.Domain.Models;
using Tragate.Infra.Data.Context;
using Tragate.Infra.Data.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Tragate.Common.Library;
using Tragate.Common.Library.Dto;

namespace Tragate.Infra.Data
{
    public class ProductImageRepository : Repository<ProductImage>, IProductImageRepository
    {
        private readonly IMediatorHandler _bus;

        public ProductImageRepository(TragateContext context,
            IMediatorHandler bus) : base(context){
            _bus = bus;
        }

        //TODO:optimistic concurency için load test yapılmalı,async veya sync olarak method test edilmeli.
        public bool AddList(int productId, List<ImageFileDto> files){
            var transaction = Db.Database.BeginTransaction(IsolationLevel.ReadUncommitted);
            try{
                foreach (var file in files){
                    var entity = new ProductImage
                    {
                        ProductId = productId,
                        FileSize = file.File.Length,
                        StatusId = (byte) StatusType.Active,
                        BigImagePath = file.Key,
                        SmallImagePath = file.Key,
                        CreatedDate = DateTime.Now
                    };
                    Db.ProductImage.Add(entity);
                }

                Db.SaveChanges();
                transaction.Commit();
                return true;
            }
            catch (System.Exception ex){
                _bus.RaiseEvent(new DomainNotification("Product.Image.Add",
                    "We had a problem during saving your data."));
                Log.Error(ex, "An error occured when user add transaction !");
                transaction.Rollback();
                return false;
            }
        }
    }
}