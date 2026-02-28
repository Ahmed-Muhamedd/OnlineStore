using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.Core.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T> AddAsync(T entity);
        Task<bool> UpdateAsync(T entity, int id);
        Task<bool> UpdateAsync(T entity);
        Task<bool> Delete(T entity);

        Task<bool> IsExist(Expression<Func<T, bool>> criteria);
        Task<bool> DeleteAsync(int id);
        Task<T?> GetByIDAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> Find(Expression<Func<T, bool>> criteria);
        Task<IEnumerable<T>> FindAll(Expression<Func<T, bool>> criteria);
    }
}
