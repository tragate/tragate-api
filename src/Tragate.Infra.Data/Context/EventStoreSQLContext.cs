using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Tragate.Domain.Core.Events;
using Tragate.Infra.Data.Mappings;

namespace Tragate.Infra.Data.Context
{
    public class EventStoreSQLContext : DbContext
    {
        public DbSet<StoredEvent> StoredEvent { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder){
            modelBuilder.ApplyConfiguration(new StoredEventMap());

            base.OnModelCreating(modelBuilder);
        }

        public EventStoreSQLContext(DbContextOptions<EventStoreSQLContext> options) : base(options){
        }
    }
}