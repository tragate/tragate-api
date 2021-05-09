using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Tragate.Domain.Interfaces;
using Tragate.Infra.Data.Context;

namespace Tragate.Infra.Data.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly TragateContext Db;
        private readonly DbSet<TEntity> DbSet;

        public Repository(TragateContext context){
            Db = context;
            DbSet = Db.Set<TEntity>();
        }

        public virtual void Add(TEntity obj){
            DbSet.Add(obj);
        }

        public virtual TEntity GetById(int id){
            return DbSet.Find(id);
        }

        public virtual IQueryable<TEntity> GetAll(){
            return DbSet;
        }

        public virtual void Update(TEntity obj){
            DbSet.Update(obj);
        }

        public virtual void Remove(int id){
            DbSet.Remove(DbSet.Find(id));
        }

        public int SaveChanges(){
            return Db.SaveChanges();
        }

        public void Dispose(){
            Db.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}