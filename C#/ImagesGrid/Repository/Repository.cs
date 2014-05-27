#region Using Directives

using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Linq.Expressions;

using ImagesGrid.Context;
using ImagesGrid.Interfaces;

#endregion
namespace ImagesGrid.Repository
{
    using ImagesGrid.Models;

    public static class DatacontextWrapper
    {
        // public static CardsContext DataContext = new CardsContext();
    }

    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        #region Fields

        private readonly CardsContext context;

        private Table<TEntity> dataTable;

        #endregion

        #region Constructors and Destructors

        public Repository()
        {
            this.context = new CardsContext(); // dataContext;
            this.dataTable = this.context.GetTable<TEntity>();
        }

        #endregion

        #region Public Methods and Operators

        public void Delete(TEntity entity)
        {
        
                this.dataTable.DeleteOnSubmit(entity);
        }

        public void DeleteAll(IEnumerable<TEntity> entities)
        {
            this.dataTable.DeleteAllOnSubmit(entities);
        }

        public TEntity FirstOrDefault()
        {
            return Queryable.FirstOrDefault(this.dataTable);
        }

        public TEntity Get(Expression<Func<TEntity, bool>> predicate)
        {
            return Queryable.FirstOrDefault(this.dataTable, predicate);
        }

        public IQueryable<TEntity> GetAll()
        {
            return this.dataTable;
        }

        public void Insert(TEntity entity)
        {
            this.dataTable.InsertOnSubmit(entity);
        }

        public void InsertAll(IEnumerable<TEntity> entities)
        {
            this.dataTable.InsertAllOnSubmit(entities);
        }

        public void SubmitChanges()
        {
            this.context.SubmitChanges();
        }

        public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
        {
            return Queryable.Where(this.dataTable, predicate);
        }

        #endregion
    }
}