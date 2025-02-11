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
        public ProductWithFilterationForCountSpecification(ProductSpecParams ProductSpecParams)
             : base(P =>
                        (string.IsNullOrEmpty(ProductSpecParams.Search) || P.Name.ToLower().Contains(ProductSpecParams.Search)) &&

                        (!ProductSpecParams.BrandId.HasValue || P.ProductBrandId == ProductSpecParams.BrandId.Value)
                        && (!ProductSpecParams.TypeId.HasValue || P.ProductTypeId == ProductSpecParams.TypeId.Value)
                 )
        {

        }
    }
}
