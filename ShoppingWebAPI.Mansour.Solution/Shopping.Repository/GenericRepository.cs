using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shopping.Core;
using Shopping.Core.IRepository;
using Shopping.Core.Models;
using Shopping.Core.Specification;
using Shopping.Repository.Data;

namespace Shopping.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        #region Construactor
        private protected readonly StoreDbContext _dbContext;

        public GenericRepository(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
        } 
        #endregion
       
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>().AsNoTracking().ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int Id)
        {
            return await _dbContext.Set<T>().FindAsync(Id);
        }

        #region Generic with Spec
        public async Task<IEnumerable<T>> GetAllWithSpecAsync(ISpecifications<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }
        public async Task<T?> GetEntityWithSpec(ISpecifications<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }
        public async  Task<int> GetCountAsync(ISpecifications<T> spec)
        {
            return await _dbContext.Set<T>().Where(spec.Criteria ?? (x => true)).CountAsync();
        }
        
        private IQueryable<T> ApplySpecification(ISpecifications<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_dbContext.Set<T>().AsQueryable(), spec);
        }

        #endregion
        public void Add(T entity)
        {
            _dbContext.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            _dbContext.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
        }

        public async Task AddAsync(T entity)
        {
            await _dbContext.AddAsync(entity);
        }
    }
}
