using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.ComponentModel.DataAnnotations;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Core.Specifications;

namespace Talabat.APIs.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public ProductsController(IGenericRepository<Product> productRepository, IMapper mapper, IGenericRepository<ProductBrand> brandRepo, IGenericRepository<ProductType> typeRepo, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }


        //[Authorize]
        [HttpGet] //GET : api/products/{id}
        public async Task<ActionResult<IReadOnlyList<Pagination<ProductToReturnDTO>>>> GetProducts([FromQuery] ProductSpecParams specParams)
        {
            
            var spec = new ProductWithBrandAndTypeSpecifications(specParams);
            var countSpec = new ProductWithFilterationForCountSpecification(specParams);
            var products = await _unitOfWork.Repository<Product>().GetAllWithSpecAsync(spec);

            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDTO>>(products);


            var count = await _unitOfWork.Repository<Product>().GetCountWithSpecAsync(countSpec);
            return Ok(new Pagination<ProductToReturnDTO>(specParams.PageIndex,specParams.PageSize, count ,data));
        }




        [ProducesResponseType(typeof(ProductToReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")] //GET : api/products/{id}
        public async Task<ActionResult<ProductToReturnDTO>> GetProduct(int id)
        {
            var spec = new ProductWithBrandAndTypeSpecifications(id);
            var product = await _unitOfWork.Repository<Product>().GetByIdWithSpecAsync(spec);


            if (product is null) return NotFound(new ApiResponse(404));

            return Ok(_mapper.Map<Product, ProductToReturnDTO>(product));

        }






        [HttpGet("brands")] //GET : api/products/brands
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            var brands = await _unitOfWork.Repository<ProductBrand>().GetAllAsync();

            return Ok(brands);
        }






        [HttpGet("types")] //GET : api/products/brands
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetTypes()
        {
            var types = await _unitOfWork.Repository<ProductType>().GetAllAsync();

            return Ok(types);
        }

    }
}
