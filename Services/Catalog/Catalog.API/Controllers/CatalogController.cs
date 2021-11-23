using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Catalog.API.Dto;
using Catalog.API.Filters;
using Catalog.API.Models;
using Catalog.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/api/catalog")]
    public class CatalogController : Controller
    {
        private readonly ProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IAuthorizationService _authorizationService;

        public CatalogController(ProductRepository productRepository, IMapper mapper, IAuthorizationService authorizationService)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _authorizationService = authorizationService;
        }
        
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts([FromQuery] PaginationFilter paginationFilter,
            [FromQuery] ProductColumnFilter productColumnFilter)
        {
            var filter = new PaginationFilter(paginationFilter.PageNumber, paginationFilter.PageSize);
            var products =
                await _productRepository.GetAllFilteredPaginated(paginationFilter.PageNumber, paginationFilter.PageSize,
                    productColumnFilter);

            var productDtos = _mapper.Map<IEnumerable<GetProductDto>>(products);
            var totalCount = await _productRepository.Count();
            var totalPages = (int) Math.Ceiling((double) totalCount / filter.PageSize);
            return Ok(new PaginatedResponse<IEnumerable<GetProductDto>>(paginationFilter.PageNumber,
                paginationFilter.PageSize, totalPages, totalCount, productDtos));
        }
        
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _productRepository.Get(id);
            if (product == null) return NotFound();
            return Ok(_mapper.Map<GetProductDto>(product));
        }
        
        [Authorize(Roles = "Admin, Vendor")]
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(CreateProductDto createProductDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var product = _mapper.Map<Product>(createProductDto);
            await _productRepository.Add(product);
            return Ok(_mapper.Map<GetProductDto>(product));
        }
        
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Product>> UpdateProduct(int id, UpdateProductDto updateProductDto)
        {
            var product = await _productRepository.Get(id);
            if (product == null) return NotFound();

            var result = await _authorizationService.AuthorizeAsync(User, product, "ProductOwner");
            if (!result.Succeeded) return Forbid();

            if (!ModelState.IsValid) return BadRequest(ModelState);
            _mapper.Map(updateProductDto, product);
            product.UpdatedAt = DateTime.Now;
            await _productRepository.Update(product);
            return Ok(_mapper.Map<GetProductDto>(product));
            
        }
        
        [HttpPatch("{id:int}")]
        public async Task<ActionResult<Product>> PartialUpdateProduct(int id,
            [FromBody] JsonPatchDocument<UpdateProductDto> patchDoc)
        {
            var product = await _productRepository.Get(id);
            if (product == null) return NotFound();

            var result = await _authorizationService.AuthorizeAsync(User, product, "ProductOwner");
            if (!result.Succeeded) return Forbid();

            var updateProduct = _mapper.Map<UpdateProductDto>(product);
            patchDoc.ApplyTo(updateProduct, ModelState);
            if (!ModelState.IsValid) return BadRequest(ModelState);
            _mapper.Map(updateProduct, product);
            // id gets lost when mapping, so we need to set it manually
            product.Id = id;

            product.UpdatedAt = DateTime.Now;
            await _productRepository.Update(product);
            return Ok(_mapper.Map<GetProductDto>(product));
        }
        
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Product>> DeleteProduct(int id)
        {
            var product = await _productRepository.Get(id);
            if (product == null) return NotFound();

            var result = await _authorizationService.AuthorizeAsync(User, product, "ProductOwner");
            if (!result.Succeeded) return Forbid();

            await _productRepository.Delete(product);
            return Ok(_mapper.Map<GetProductDto>(product));
        }
    }
}