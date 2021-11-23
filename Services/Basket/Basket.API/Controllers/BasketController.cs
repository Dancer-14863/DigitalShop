using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Basket.API.Dto;
using Basket.API.Filters;
using Basket.API.Models;
using Basket.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/api/baskets")]
    public class BasketController : Controller
    {
        private readonly BasketRepository _basketRepository;
        private readonly BasketProductRepository _basketProductRepository;
        private readonly ProductRepository _productRepository;
        private readonly AccountRepository _accountRepository;
        private readonly IMapper _mapper;
        private readonly IAuthorizationService _authorizationService;

        public BasketController(BasketRepository basketRepository, BasketProductRepository basketProductRepository, ProductRepository productRepository, AccountRepository accountRepository, IMapper mapper, IAuthorizationService authorizationService)
        {
            _basketRepository = basketRepository;
            _basketProductRepository = basketProductRepository;
            _productRepository = productRepository;
            _accountRepository = accountRepository;
            _mapper = mapper;
            _authorizationService = authorizationService;
        }

        [Authorize(Roles = "Admin, User")]
        [HttpPost]
        public async Task<ActionResult<Models.Basket>> CreateBasket(CreateBasketDto createBasketDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (await _accountRepository.Get(createBasketDto.OwnerId) == null) return NotFound("User not found");
            // loop through products in basket and check if they exist
            foreach (var productId in createBasketDto.ProductIds)
            {
                if (await _productRepository.Get(productId) == null) return NotFound("Product not found");
            }

            var basket = new Models.Basket{ OwnerId = createBasketDto.OwnerId };
            await _basketRepository.Add(basket);
            if (basket.Id == 0) return BadRequest("Could not create basket");
            var getBasketDto = _mapper.Map<GetBasketDto>(basket);
            foreach (var productId in createBasketDto.ProductIds)
            {
                await _basketProductRepository.Add(new BasketProduct { BasketId = basket.Id, ProductId = productId });
                getBasketDto.ProductIds.Add(productId);
            }
            return Ok(getBasketDto);
        }
        
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.Basket>>> GetBaskets([FromQuery] PaginationFilter paginationFilter,
            [FromQuery] BasketColumnFilter basketColumnFilter)
        {
            var filter = new PaginationFilter(paginationFilter.PageNumber, paginationFilter.PageSize);
            var baskets =
                await _basketRepository.GetAllFilteredPaginated(paginationFilter.PageNumber, paginationFilter.PageSize,
                    basketColumnFilter);
            List<GetBasketDto> getBasketDtos = new List<GetBasketDto>();
            foreach (var basket in baskets)
            {
                var getBasketDto = _mapper.Map<GetBasketDto>(basket);
                getBasketDto.ProductIds = (await _basketProductRepository.GetByBasketId(basket.Id))
                    .Select(basketProduct => basketProduct.ProductId).ToList();
                getBasketDtos.Add(getBasketDto);
            }

            var totalCount = await _basketRepository.Count();
            var totalPages = (int) Math.Ceiling((double) totalCount / filter.PageSize);
            return Ok(new PaginatedResponse<List<GetBasketDto>>(paginationFilter.PageNumber,
                paginationFilter.PageSize, totalPages, totalCount, getBasketDtos));
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<Models.Basket>> GetBasket(int id)
        {
            var basket = await _basketRepository.Get(id);
            if (basket == null) return NotFound();

            var result = await _authorizationService.AuthorizeAsync(User, basket, "BasketOwner");
            if (!result.Succeeded) return Forbid();
            
            var getBasketDto = _mapper.Map<GetBasketDto>(basket);
            getBasketDto.ProductIds = (await _basketProductRepository.GetByBasketId(basket.Id))
                .Select(basketProduct => basketProduct.ProductId).ToList();

            return Ok(getBasketDto);
        }
        
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Models.Basket>> UpdateBasket(int id, UpdateBasketDto updateBasketDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var basket = await _basketRepository.Get(id);
            if (basket == null) return NotFound();
            
            var result = await _authorizationService.AuthorizeAsync(User, basket, "BasketOwner");
            if (!result.Succeeded) return Forbid();
            
            var getBasketDto = _mapper.Map<GetBasketDto>(basket);
            var basketProducts = await _basketProductRepository.GetByBasketId(id);
            // delete products that are not in the update basket
            foreach (var basketProduct in basketProducts)
            {
                if (!updateBasketDto.ProductIds.Contains(basketProduct.ProductId))
                {
                    await _basketProductRepository.Delete(basketProduct);
                }
            }
            // add products from the update basket that are not in the current basket
            foreach (var productId in updateBasketDto.ProductIds)
            {
                if (!basketProducts.Any(basketProduct => basketProduct.ProductId == productId))
                {
                    await _basketProductRepository.Add(new BasketProduct { BasketId = id, ProductId = productId });
                }
            }
            getBasketDto.ProductIds = updateBasketDto.ProductIds;
            getBasketDto.UpdatedAt = DateTime.Now;
            return Ok(getBasketDto);
        }
        
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Models.Basket>> DeleteBasket(int id)
        {
            var basket = await _basketRepository.Get(id);
            if (basket == null) return NotFound();
            
            var result = await _authorizationService.AuthorizeAsync(User, basket, "BasketOwner");
            if (!result.Succeeded) return Forbid();

            await _basketRepository.Delete(basket);
            return Ok(_mapper.Map<GetBasketDto>(basket));
        }
    }
}