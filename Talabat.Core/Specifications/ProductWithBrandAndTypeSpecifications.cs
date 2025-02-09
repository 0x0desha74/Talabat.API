using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public class ProductWithBrandAndTypeSpecifications : BaseSpecification<Product>
    {
        //This constructor is used for Get All Products
        public ProductWithBrandAndTypeSpecifications(string? sort, int? brandId, int? typeId)
            : base(P =>
                        (!brandId.HasValue || P.ProductBrandId == brandId.Value)
                        && (!typeId.HasValue || P.ProductTypeId == typeId.Value)
                 )
        {
            Includes.Add(P => P.ProductBrand);
            Includes.Add(P => P.ProductType);

            AddOrderBy(P => P.Name);

            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort)
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
        }

        //This constructor id used for get a specific product using filtering
        public ProductWithBrandAndTypeSpecifications(int id) : base(P => P.Id == id)
        {
            Includes.Add(P => P.ProductBrand);
            Includes.Add(P => P.ProductType);
        }
    }
}
