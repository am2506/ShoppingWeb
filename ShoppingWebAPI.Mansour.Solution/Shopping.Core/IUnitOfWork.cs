using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shopping.Core.IRepository;
using Shopping.Core.Models;
using Shopping.Core.Models.OrderComponents;

namespace Shopping.Core
{
    public interface IUnitOfWork : IAsyncDisposable
    {
      
        //Method Will Create Repository per Demand
        IGenericRepository<T> Repository<T>() where T : BaseEntity;
      
        //Methods
        Task<int> CompleteAsync();
        //void Dispose();



    }
}
