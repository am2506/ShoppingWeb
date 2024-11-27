using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shopping.Core;
using Shopping.Core.IRepository;
using Shopping.Core.Models;
using Shopping.Core.Models.OrderComponents;
using Shopping.Repository.Data;

namespace Shopping.Repository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreDbContext _dbContext;

        //Dictionary to Store Repositories
       // private Dictionary<string, IGenericRepository<BaseEntity>> _repositories;
        //Or Can Use HasTable {Key object , Value objec}
        private Hashtable _hasRepositories;
       
        public UnitOfWork(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
            _hasRepositories = new Hashtable();
        }

       //===================================================
        public IGenericRepository<T> Repository<T>() where T : BaseEntity
        {
            var key = typeof(T).Name;
            if (!_hasRepositories.ContainsKey(key))
            {
                var repo = new GenericRepository<T>(_dbContext);
                _hasRepositories.Add(key, repo);
            }
            return _hasRepositories[key] as IGenericRepository<T>;
        }

        public async Task<int> CompleteAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public async ValueTask DisposeAsync()
        {
             await _dbContext.DisposeAsync();
        }
    }
}
