using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public class ProductWithFilterationForCountSpecification : BaseSpecification<Product>
    {
        public ProductWithFilterationForCountSpecification(ProductSpecParams specParams)
             : base(P =>
                        (!specParams.BrandId.HasValue || P.ProductBrandId == specParams.BrandId.Value)
                        && (!specParams.TypeId.HasValue || P.ProductTypeId == specParams.TypeId.Value)
                 )
        {

        }
    }
}
