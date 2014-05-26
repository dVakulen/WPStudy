#region Using Directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#endregion
namespace ImagesGrid.Interfaces
{
  

    public interface IRepository<T>
    {
        #region Public Methods and Operators

        void Delete(T entity);

        void DeleteAll(IEnumerable<T> entities);

        T FirstOrDefault();

        T Get(Expression<Func<T, bool>> predicate);

        IQueryable<T> GetAll();

        void Insert(T entity);

        void InsertAll(IEnumerable<T> entities);

        void SubmitChanges();

        IQueryable<T> Where(Expression<Func<T, bool>> predicate);

        #endregion
    }
}