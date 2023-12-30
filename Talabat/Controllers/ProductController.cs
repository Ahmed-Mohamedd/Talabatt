using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Api.DTOs;
using Talabat.Api.Helpers;
using Talabat.API.Errors;
using Talabat.Core;
using Talabat.Core.Repositories;
using Talabat.Core.specifications;
using Talabat.DAL.Entities;

namespace Talabat.Api.Controllers
{
    
    public class ProductController : APIBaseController
    {
        //private readonly IGenericRepository<Product> _productRepo;
        //private readonly IGenericRepository<ProductBrand> _brandRepo;
        //private readonly IGenericRepository<ProductType> _typeRepo;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ProductController(IMapper mapper, IUnitOfWork unitOfWork
            //IGenericRepository<Product> productRepo,  
            //IGenericRepository<ProductBrand> brandRepo, 
            //IGenericRepository<ProductType> typeRepo
            )
        {
            //_productRepo=productRepo;
            //_brandRepo=brandRepo;
            //_typeRepo=typeRepo;
            _mapper=mapper;
            _unitOfWork=unitOfWork;
        }
        
        [CachedAttribute(300)]
        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetAll([FromQuery]ProductParams productParams)
        {
            var spec = new ProductWithBrandAndTypeSpecification(productParams);
            var products = await _unitOfWork.Repository<Product>().GetAllWithSpec(spec);
            var MappedProducts = _mapper.Map<IEnumerable<Product> , IEnumerable<ProductToReturnDto>>(products);

            var countSpec = new ProductWithFilterForCountSpecification(productParams);
            var count = await _unitOfWork.Repository<Product>().GetCountAsync(countSpec);

            return Ok(new Pagination<ProductToReturnDto>(productParams.PageIndex, productParams.PageSize, count, MappedProducts));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductToReturnDto>> GetById(int id)
        {
            var spec = new ProductWithBrandAndTypeSpecification(id);
            var product = await _unitOfWork.Repository<Product>().GetByIdWithSpec(spec);
            if (product == null) return NotFound(new ApiResponse(404));

            return Ok(_mapper.Map<Product, ProductToReturnDto>(product));
        }
        
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            var brands = await _unitOfWork.Repository<ProductBrand>().GetAll();
            return Ok(brands);
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetTypes()
        {
            var Types = await _unitOfWork.Repository<ProductType>().GetAll();
            return Ok(Types);
        }
    }
}
