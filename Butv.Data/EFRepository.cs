using BUTV.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace BUTV.Data
{
    public class EFRepository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly ApplicationDbContext _ctx;
        private  DbSet<T> entities;
        string errorMessage = string.Empty;

        public EFRepository(ApplicationDbContext context)
        {
            this._ctx = context;
            entities = context.Set<T>();
        }
     
        public virtual IQueryable<T> Table => this.entities;
        public IQueryable<T> TableWithInclude(params Expression<Func<T, object>>[] navigationProperties)
        {
            IQueryable<T> dbQuery = entities;
            foreach (var nav in navigationProperties)
                dbQuery =  this.entities.Include<T, object>(nav);
           
            return dbQuery;
        }
        public IQueryable<T> TableNoTracking => this.entities.AsNoTracking();

        public T Get(long id)
        {
            return entities.SingleOrDefault(s => s.Id == id);
        }
        public void Insert(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Add(entity);
            _ctx.SaveChanges();
        }

        public void Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            
            _ctx.SaveChanges();
        }

        public void Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
            _ctx.SaveChanges();
        }

        public IQueryable<T> ExecuteStoredProcedureList(string commandText, params object[] parameters)
        {           
            _ctx.Database.ExecuteSqlCommand(commandText, parameters);
            return entities.FromSql(commandText, parameters);
        }
    }
}
