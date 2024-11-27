using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Shopping.Core.Models;

namespace Shopping.Core.Specification
{
    public interface ISpecifications<T> where T : BaseEntity
    {
        Expression<Func<T, bool>>? Criteria { get; set; }
        List<Expression<Func<T, object>>> Includes { get; }
        Expression<Func<T, object>>? OrderBy { get; set; }
        Expression<Func<T, object>>? OrderByDesc { get; set; }
        int ?Take { get; set; }
        int ?Skip { get; set; }


    }
}

//_dbcontext.Set<T>().Where("Criteria").Include("Expression")
//Criteria where ( p => p.X == Y)

// To implement pagination
//_dbcontext.Set<T>()
//.Where("Criteria")
//.Include("Expression")
//.Skip(PageSize * (PageIndex - 1))
//.Take(PageSize)