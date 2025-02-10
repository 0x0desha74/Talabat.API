using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Specifications;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public class ProductWithBrandAndTypeSpecifications : BaseSpecification<Product>
    {
        //This constructor is used for Get All Products
        public ProductWithBrandAndTypeSpecifications(ProductSpecParams specParams)
            : base(P =>
                        (!specParams.BrandId.HasValue || P.ProductBrandId == specParams.BrandId.Value)
                        && (!specParams.TypeId.HasValue || P.ProductTypeId == specParams.TypeId.Value)
                 )
        {
            Includes.Add(P => P.ProductBrand);
            Includes.Add(P => P.ProductType);

            AddOrderBy(P => P.Name);

            if (!string.IsNullOrEmpty(specParams.Sort))
            {
                switch (specParams.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(P => P.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDesc(P => P.Price);
                        break; 
                    default:
                        AddOrderBy(P => P.Name);
                        break;
                }
            }


            //skip = (size-1)*index
            //take = size

            ApplyPagination(specParams.PageIndex * (specParams.PageSize - 1), specParams.PageSize);

        }

        //This constructor id used for get a specific product using filtering
        public ProductWithBrandAndTypeSpecifications(int id) : base(P => P.Id == id)
        {
            Includes.Add(P => P.ProductBrand);
            Includes.Add(P => P.ProductType);
        }
    }
}
