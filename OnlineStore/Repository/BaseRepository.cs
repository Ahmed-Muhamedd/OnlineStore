using Microsoft.EntityFrameworkCore;
using OnlineStore.Core.Interfaces;
using OnlineStore.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OnlineStore.Repository
{
    public class BaseRepository<T>: IBaseRepository<T> where T : class
    {
        protected AppDbContext _Context;
        public BaseRepository(AppDbContext context)
        {
            _Context = context;
        }

        public  async Task<T> AddAsync(T entity)
        {
            await _Context.Set<T>().AddAsync(entity);
            await _Context.SaveChangesAsync();
            return entity;
        }

        public  async  Task<bool>  Delete(T entity)
        {
            _Context.Set<T>().Remove(entity);
            return await  _Context.SaveChangesAsync() > 0;
        }

        public  async Task<bool> DeleteAsync(int id)
        {
            T? Entity = await GetByIDAsync(id);
            if (Entity is null) return false;

            _Context.Set<T>().Remove(Entity);
            
            return await _Context.SaveChangesAsync() > 0;
        }

        public async Task<T?> Find(Expression<Func<T, bool>> criteria)
        {
            return await _Context.Set<T>().AsNoTracking().SingleOrDefaultAsync(criteria);
        }

        public async Task<IEnumerable<T>> FindAll(Expression<Func<T, bool>> criteria)
        {
            return await _Context.Set<T>().Where(criteria).ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _Context.Set<T>().ToListAsync();
        }

        //public async Task<IEnumerable<T>> GetAllWithIncludeAsync(string[] includes)
        //{
        //    IQueryable<T> query = _Context.Set<T>();
        //    foreach (var include in includes)
        //    {
        //        query = query.Include(include);
        //    }
        //    return await query.ToListAsync();
        //}

        public async Task<T?> GetByIDAsync(int id)
        {
            T? Entity = await _Context.Set<T>().FindAsync(id);
            return Entity;
        }

        public async Task<bool> IsExist(Expression<Func<T, bool>> criteria)
        {
            T? Exist = await Find(criteria);

            return Exist != null ;
        }

        public  async Task<bool> UpdateAsync(T entity, int id)
        {
            T? Entity = await _Context.Set<T>().FindAsync(id);
            if (Entity is null) return false;

            if (!await UpdateAsync(Entity)) return false;
            
            return await _Context.SaveChangesAsync() > 0;
        }

        public  async Task<bool> UpdateAsync(T entity)
        {
            _Context.Set<T>().Update(entity);
            return await _Context.SaveChangesAsync() > 0;
        }
    }
}
