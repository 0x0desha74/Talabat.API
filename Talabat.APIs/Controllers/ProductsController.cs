using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.ComponentModel.DataAnnotations;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Core.Specifications;

namespace Talabat.APIs.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IGenericRepository<ProductBrand> _brandRepo;
        private readonly IGenericRepository<ProductType> _typeRepo;
        private readonly IMapper _mapper;

        
        public ProductsController(IGenericRepository<Product> productRepository, IMapper mapper, IGenericRepository<ProductBrand> brandRepo, IGenericRepository<ProductType> typeRepo)
        {
            _productRepo = productRepository;
            _mapper = mapper;
            _brandRepo = brandRepo;
            _typeRepo = typeRepo;
        }



        [HttpGet] //GET : api/products/{id}
        public async Task<ActionResult<IReadOnlyList<Pagination<ProductToReturnDTO>>>> GetProducts([FromQuery] ProductSpecParams specParams)
        {
            
            var spec = new ProductWithBrandAndTypeSpecifications(specParams);
            var countSpec = new ProductWithFilterationForCountSpecification(specParams);
            var products = await _productRepo.GetAllWithSpecAsync(spec);

            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDTO>>(products);


            var count = await _productRepo.GetCountWithSpecAsync(countSpec);
            return Ok(new Pagination<ProductToReturnDTO>(specParams.PageIndex,specParams.PageSize, count ,data));
        }





        [ProducesResponseType(typeof(ProductToReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")] //GET : api/products/{id}
        public async Task<ActionResult<ProductToReturnDTO>> GetProduct(int id)
        {
            var spec = new ProductWithBrandAndTypeSpecifications(id);
            var product = await _productRepo.GetByIdWithSpecAsync(spec);


            if (product is null) return NotFound(new ApiResponse(404));

            return Ok(_mapper.Map<Product, ProductToReturnDTO>(product));

        }






        [HttpGet("brands")] //GET : api/products/brands
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            var brands = await _brandRepo.GetAllAsync();

            return Ok(brands);
        }






        [HttpGet("types")] //GET : api/products/brands
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetTypes()
        {
            var types = await _typeRepo.GetAllAsync();

            return Ok(types);
        }

    }
}
