using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shopping.Core.Models;
using Shopping.Core.Specification;

namespace Shopping.Core.IRepository
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int Id);
        Task<T?> GetEntityWithSpec(ISpecifications<T> spec);
        Task<IEnumerable<T>> GetAllWithSpecAsync(ISpecifications<T> spec);
        Task AddAsync(T entity);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);

     
        Task<int> GetCountAsync(ISpecifications<T> spec);
    }
}
