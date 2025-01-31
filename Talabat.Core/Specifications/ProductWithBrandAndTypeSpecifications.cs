using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public class ProductWithBrandAndTypeSpecifications : BaseSpecification<Product>
    {
        //This constructor is used for Get All Products
        public ProductWithBrandAndTypeSpecifications()
        {
            Includes.Add(P => P.ProductBrand);
            Includes.Add(P => P.ProductType);
        }

        //This constructor id used for get a specific product using filtering
        public ProductWithBrandAndTypeSpecifications(int id) : base(P => P.Id == id)
        {
            Includes.Add(P => P.ProductBrand);
            Includes.Add(P => P.ProductType);
        }
    }
}
