using BUTV.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BUTV.Data
{
    public interface IRepository<T> where T : BaseEntity
    {
        IQueryable<T> Table { get; }
        /// <summary>
        /// current Entity Framework core does not have Lazy Loading feature, so we do eager loading
        /// </summary>       
        IQueryable<T> TableWithInclude(params Expression<Func<T, object>>[] navigationProperties);
        IQueryable<T> TableNoTracking { get; }
        IQueryable<T> ExecuteStoredProcedureList(string commandText, params object[] parameters);

       T Get(long id);
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
