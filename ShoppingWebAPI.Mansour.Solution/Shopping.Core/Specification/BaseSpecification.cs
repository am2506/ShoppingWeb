using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Shopping.Core.Models;
using Shopping.Core.Specification;

namespace Shopping.Repository.SpecificationDesignPattern
{
    public class BaseSpecification<T> : ISpecifications<T> where T : BaseEntity
    {
        /// Cases
        ///  1. Have Includes only
        ///  2. Have Include and where
        /// </summary>
        public Expression<Func<T, bool>>? Criteria { get; set; }

        public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();

        public Expression<Func<T, object>>? OrderBy { get; set; }

        public Expression<Func<T, object>>? OrderByDesc { get; set; }

        public int? Take { get; set; }

        public int? Skip { get; set; }

        //Use This Constructor If Expression Has Includes spec only
        public BaseSpecification()
        {
        }

        //Use This Constructor If Expression Has Includes spec and where Spec
        public BaseSpecification(Expression<Func<T, bool>> _criteria)
        {
            Criteria = _criteria;
        }
        
        public void AddIncludeExpression(Expression<Func<T, object>> expression)
        {
            Includes.Add(expression);
        }
        public void AddOrderByExpression(Expression<Func<T, object>> expression)
        {
            OrderBy = expression;
        }
        public void AddOrderByDescExpression(Expression<Func<T, object>> expression)
        {
            OrderByDesc = expression;
        }
        public void AddCriteria(Expression<Func<T, bool>> expression)
        {
            Criteria = expression;
        }
    }
}