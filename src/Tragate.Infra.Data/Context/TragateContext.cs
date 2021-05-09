using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Tragate.Domain.Models;

namespace Tragate.Infra.Data.Context
{
    public class TragateContext : DbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Parameter> Parameter { get; set; }
        public DbSet<Company> Company { get; set; }
        public DbSet<CompanyAdmin> CompanyAdmin { get; set; }
        public DbSet<CompanyCategory> CompanyCategory { get; set; }
        public DbSet<CompanyMembership> CompanyMembership { get; set; }
        public DbSet<CompanyNote> CompanyNote { get; set; }
        public DbSet<CompanyTask> CompanyTask { get; set; }
        public DbSet<Location> Location { get; set; }
        public DbSet<CompanyData> CompanyData { get; set; }
        public DbSet<Content> Content { get; set; }
        public DbSet<SystemAdmin> SystemAdmin { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<ProductImage> ProductImage { get; set; }
        public DbSet<ProductTag> ProductTag { get; set; }
        public DbSet<Tag> Tag { get; set; }
        public DbSet<Quote> Quote { get; set; }
        public DbSet<QuoteProduct> QuoteProduct { get; set; }
        public DbSet<QuoteHistory> QuoteHistory { get; set; }
        public DbSet<UserAddress> UserAddress { get; set; }
        public DbSet<Email> Email { get; set; }


        public TragateContext(DbContextOptions<TragateContext> options) : base(options){
        }
    }
}