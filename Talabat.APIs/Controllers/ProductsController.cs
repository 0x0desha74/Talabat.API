using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Talabat.APIs.DTOs;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Core.Specifications;

namespace Talabat.APIs.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IMapper _mapper;


        public ProductsController(IGenericRepository<Product> productRepository, IMapper mapper)
        {
            _productRepo = productRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductToReturnDTO>>> GetProducts()
        {
            var spec = new ProductWithBrandAndTypeSpecifications();
            var products = await _productRepo.GetAllWithSpecAsync(spec);

            return Ok(_mapper.Map<IEnumerable<Product>, IEnumerable<ProductToReturnDTO>>(products));
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDTO>> GetProduct(int id) 
        {
            var spec = new ProductWithBrandAndTypeSpecifications(id);
            var product = await _productRepo.GetByIdWithSpecAsync(spec);

            return Ok(_mapper.Map<Product,ProductToReturnDTO>(product));
        }

    }
}
