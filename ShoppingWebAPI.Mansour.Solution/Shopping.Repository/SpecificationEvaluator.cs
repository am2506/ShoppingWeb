using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Shopping.Core.Models;

using Microsoft.EntityFrameworkCore;
using Shopping.Core.Specification;

namespace Shopping.Core
{
    public class SpecificationEvaluator<TEntity> where TEntity : BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecifications<TEntity> spec)
        {
            var query = inputQuery;
           // _dbcontext.Set<T>()
            if (spec.Criteria is not null)
            {
                query = query.Where(spec.Criteria);
                // _dbcontext.Set<T>().Where("Criteria")
            }

            query = spec.Includes.Aggregate(query, (current, expression) => current.Include(expression));
            // _dbcontext.Set<T>().Where("Criteria").Include("Expression")
            if (spec.OrderBy is not null)
            {
                query = query.OrderBy(spec.OrderBy);
                // _dbcontext.Set<T>().Where("Criteria").
                // Include("Expression").OrderBy("OrderBy")
            }
            else if (spec.OrderByDesc is not null)
            {
                query = query.OrderByDescending(spec.OrderByDesc);
                // _dbcontext.Set<T>().Where("Criteria").
                // Include("Expression").OrderByDesc("OrderByDesc")
            }

            if (spec.Take.HasValue)
                query = query.Take(spec.Take.Value);
            if (spec.Skip.HasValue)
                query = query.Skip(spec.Skip.Value);
            return query;
        }
    }
}
