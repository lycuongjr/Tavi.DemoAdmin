using System;
using System.Collections.Generic;
using System.Linq;

using System.Data.Entity;
using System.Linq.Expressions;
using PagedList;
namespace Tavi.Database
{
    public class EntityHelper<TEntity, TContext> : IDisposable
        where TEntity : class
        where TContext : DbContext
    {
        private TContext context;
        // static BaseService() { }

        //public BaseService(TContext context)
        //{
        //    this.context = context;
        //}

        public TContext dbContext
        {
            get
            {
                if (this.context == null)
                    this.context = Activator.CreateInstance<TContext>();

                return this.context;
            }
        }

        internal string tableName
        {
            get
            {

                return (dbContext as System.Data.Entity.Infrastructure.IObjectContextAdapter)
                     .ObjectContext.CreateObjectSet<TEntity>().EntitySet.Name;
            }
        }

        internal DbSet<TEntity> dbSet
        {
            get
            {
                return dbContext.Set<TEntity>();
            }
        }

        public virtual void Delete(object id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (dbContext.Entry(entityToDelete).State == EntityState.Detached)
                dbSet.Attach(entityToDelete);
            dbSet.Remove(entityToDelete);
            SaveChange();
        }

        public virtual void Delete(IList<TEntity> entityToDelete)
        {
            dbSet.RemoveRange(entityToDelete);
            SaveChange();
        }

        /// <summary>
        /// Not want to save context, default save it
        /// </summary>
        public virtual void DeleteAll()
        {
            foreach (TEntity entity in dbSet)
                Delete(entity);
            SaveChange();

        }

        /// <summary>
        /// Not want to save context, default save it
        /// </summary>
        /// <param name="tEntitys"></param>
        public virtual void DeleteObject(IList<TEntity> tEntitys)
        {
            foreach (TEntity entityToDelete in tEntitys)
            {
                Delete(entityToDelete);
                SaveChange();
            }
        }

        /// <summary>
        /// Not want to save context, default save it
        /// </summary>
        /// <param name="entityToDelete"></param>
        public virtual void DeleteObject(HashSet<int> ids)
        {
            foreach (int id in ids)
            {
                Delete(id);
                SaveChange();
            }
        }
        public virtual TEntity FirstOrDefault(
         Expression<Func<TEntity, bool>> filter = null, string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
                query = query.Where(filter);

            if (includeProperties != "" && includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split
                     (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    query = query.Include(includeProperty);
            }

            if (query != null)
                return query.FirstOrDefault();
            return null;
        }

        /// <summary>
        /// Example: Get(
        ///        filter: d => value || d.ID == value,
        ///        orderBy: q => q.OrderBy(d => d.ID),
        ///        includeProperties: "TEntity"));
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> Get(
         Expression<Func<TEntity, bool>> filter = null,
         Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
         string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
                query = query.Where(filter);

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                query = query.Include(includeProperty);

            if (orderBy != null)
                return orderBy(query).ToList();
            else
                return query.ToList();
        }


        public virtual TEntity FindByKey(object id)
        {
            return dbSet.Find(id);
        }

        public virtual IEnumerable<TEntity> GetWithRawSql(string query, params object[] parameters)
        {
            return dbSet.SqlQuery(query, parameters).ToList();
        }
        public virtual void RunSql(string sql)
        {
            dbContext.Database.ExecuteSqlCommand(sql);
        }
        public virtual void RunSql(string sql, params object[] parameters)
        {
            dbContext.Database.ExecuteSqlCommand(sql, parameters);
        }
        /// <summary>
        /// GetDynamic("Name={0}","Giorgi")
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        protected virtual IEnumerable<TEntity> GetDynamic(string query, params object[] parameters)
        {
            return dbSet.SqlQuery(string.Format("SELECT * FROM {0} WHERE {1}", tableName, query), parameters).ToList();
        }

        public virtual IEnumerable<TEntity> FindList()
        {
            return dbSet.AsQueryable();
        }
       
        public virtual IQueryable<TEntity> GetList()
        {
            return dbSet.AsQueryable();
        }
        public virtual IPagedList<TEntity> ListByPage(int? pageNumber)
        {
            return ListByPage(pageNumber, 10);
        }
        public virtual IPagedList<TEntity> ListByPage(int? pageNumber, int pageSize)
        {
            return dbSet.ToPagedList(pageNumber.HasValue ? pageNumber.Value : 1, pageSize);
        }
        public virtual void SaveChanges(TEntity entityToUpdate)
        {
            if (dbContext.Entry(entityToUpdate).State == EntityState.Detached)
                dbSet.Attach(entityToUpdate);

            dbContext.Entry(entityToUpdate).State = EntityState.Modified;
            SaveChange();
        }

        public virtual void Insert(TEntity entity)
        {
            dbSet.Add(entity);
            SaveChange();
        }

        public virtual void Insert(IList<TEntity> entity)
        {
            dbSet.AddRange(entity);
            SaveChange();
        }

        public virtual void Update(TEntity entity)
        {
            dbSet.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
            SaveChange();
        }

        public void SaveChanges()
        {
            this.SaveChange();
        }

        private void SaveChange()
        {
            dbContext.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
                if (disposing)
                    context.Dispose();

            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
