using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shopping.Core.Models;
using Shopping.Core.Specification;

namespace Shopping.Repository.SpecificationDesignPattern
{
    public class ProductWithBrandAndCategory : BaseSpecification<Product>
    {
        public ProductWithBrandAndCategory()
        {
            AddIncludeExpression(p => p.Category!);
            AddIncludeExpression(p => p.Brand!);
        }
        public ProductWithBrandAndCategory(int Id) : base(p => p.Id == Id)
        {
            AddIncludeExpression(p => p.Category!);
            AddIncludeExpression(p => p.Brand!);
        }
        public ProductWithBrandAndCategory(SpecParams? specParams)
        {
            AddIncludeExpression(p => p.Category!);
            AddIncludeExpression(p => p.Brand!);

            if (specParams is not null)
            {
                if (specParams.sort is not null)
                {
                    if (specParams.sort.ToUpper() == "NAMEDESC")
                        AddOrderByDescExpression(p => p.Name);
                    else if (specParams.sort.ToUpper() == "PRICEASC")
                        AddOrderByExpression(p => p.Price);
                    else if (specParams.sort.ToUpper() == "PRICEDESC")
                        AddOrderByDescExpression(p => p.Price);
                    else
                        AddOrderByExpression(p => p.Name);
                }

                if (specParams.CategoryId.HasValue && specParams.brandId.HasValue)
                    AddCriteria(
                         p => p.CategoryId == specParams.CategoryId 
                         &&
                         p.BrandId == specParams.brandId);
                else if (specParams.CategoryId.HasValue)
                    AddCriteria(p => p.CategoryId == specParams.CategoryId);
                else if (specParams.brandId.HasValue)
                    AddCriteria(p => p.BrandId == specParams.brandId);

                if (specParams.pageSize.HasValue && specParams.pageIndex.HasValue)
                {
                    if (specParams.pageIndex.Value <= 0)
                        specParams.pageIndex = 1;
                    Take = specParams.pageSize;
                    Skip = specParams.pageSize * (specParams.pageIndex - 1);
                } 
            }

        }
       
    }
}
