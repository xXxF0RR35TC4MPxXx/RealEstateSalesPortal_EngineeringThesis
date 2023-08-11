using Inżynierka.Shared.IRepositories;
using Microsoft.Extensions.Logging;

namespace Inżynierka.Shared.Repositories
{
    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class, new()
    {
        #region Private Members

        private readonly DataContext _dataContext;
        private readonly ILogger<BaseRepository<TEntity>> _logger;

        #endregion Private Members

        #region Constructors

        protected BaseRepository(DataContext trackerDb)
        {
            _dataContext = trackerDb;
        }

        #endregion Constructors

        #region Properties

        protected DataContext DataContext
        {
            get
            {
                return _dataContext;
            }
        }

        #endregion Properties

        #region Repository<TInterface> Members

        public virtual TEntity Add(TEntity entity)
        {
            _dataContext.AddEntity(entity);

            return entity;
        }

        public virtual TEntity AddAndSaveChanges(TEntity entity)
        {
            _dataContext.AddEntityAndSaveChanges(entity);

            return entity;
        }

        public virtual void AddRange(IEnumerable<TEntity> entity)
        {
            _dataContext.AddEntitiesRange(entity);
        }

        public virtual void AddRangeAndSaveChanges(IEnumerable<TEntity> entity)
        {
            _dataContext.AddEntitiesRangeAndSaveChanges(entity);
        }

        public virtual void Attach(TEntity entity)
        {
            _dataContext.Set<TEntity>().Attach(entity);
        }

        public virtual void Update(TEntity entity)
        {
            _dataContext.UpdateEntity(entity);
        }

        public virtual void UpdateAndSaveChanges(TEntity entity)
        {
            Update(entity);
            _dataContext.SaveChanges();
        }

        public virtual void UpdateRangeAndSaveChanges(IEnumerable<TEntity> entities)
        {
            _dataContext.UpdateEntitiesRangeAndSaveChanges(entities);
        }

        public virtual void Remove(TEntity entity)
        {
            _dataContext.RemoveEntity(entity);
        }

        public virtual void RemoveById(int id)
        {
            var entity = GetById(id);
            if (entity != null)
            {
                Remove(entity);
            }
        }

        public virtual void RemoveByIdAndSaveChanges(int id)
        {
            RemoveById(id);
            SaveChanges();
        }

        public virtual void RemoveRange(IEnumerable<TEntity> entity)
        {
            _dataContext.RemoveEntitiesRange(entity);
        }

        public virtual void RemoveRangeAndSaveChanges(IEnumerable<TEntity> entity)
        {
            _dataContext.RemoveEntitiesRangeAndSaveChanges(entity);
        }

        public virtual void DetectChanges()
        {
            _dataContext.ChangeTracker.DetectChanges();
        }

        public virtual void SaveChanges()
        {
            _dataContext.SaveChanges();
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            return DataContext.Set<TEntity>();
        }

        #endregion Repository<TInterface> Members

        #region IDisposable Members

        public void Dispose()
        {
            if (_dataContext != null)
            {
                _dataContext.Dispose();
            }
        }

        #endregion IDisposable Members

        #region Public Methods

        public virtual TEntity GetById(int id)
        {
            return DataContext.Set<TEntity>().Find(id);
        }

        #endregion Public Methods
    }
}