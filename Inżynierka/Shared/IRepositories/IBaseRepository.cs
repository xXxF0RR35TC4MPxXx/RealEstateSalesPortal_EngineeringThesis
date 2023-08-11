namespace Inżynierka.Shared.IRepositories
{
    public interface IBaseRepository<TEntity> where TEntity : class, new()
    {
        TEntity Add(TEntity entity);

        TEntity AddAndSaveChanges(TEntity entity);

        void AddRange(IEnumerable<TEntity> entity);

        void AddRangeAndSaveChanges(IEnumerable<TEntity> entity);

        void Attach(TEntity entity);

        void DetectChanges();

        void Dispose();

        IQueryable<TEntity> GetAll();

        TEntity? GetById(int id);

        void Remove(TEntity entity);

        void RemoveById(int id);

        void RemoveByIdAndSaveChanges(int id);

        void RemoveRange(IEnumerable<TEntity> entity);

        void RemoveRangeAndSaveChanges(IEnumerable<TEntity> entity);

        void SaveChanges();

        void Update(TEntity entity);

        void UpdateAndSaveChanges(TEntity entity);

        void UpdateRangeAndSaveChanges(IEnumerable<TEntity> entities);
    }
}