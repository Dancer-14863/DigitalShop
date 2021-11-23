using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Order.API.Dto;
using Order.API.Filters;
using Order.API.Repositories;

namespace Order.API.Controllers
{
    [Authorize(Roles = "Admin, User")]
    [ApiController]
    [Route("/api/orders")]
    public class OrderController : Controller
    {
        private readonly OrderRepository _orderRepository;
        private readonly ProductRepository _productRepository;
        private readonly BasketRepository _basketRepository;
        private readonly BasketProductRepository _basketProductRepository;
        private readonly IMapper _mapper;
        private readonly IAuthorizationService _authorizationService;


        public OrderController(OrderRepository orderRepository, ProductRepository productRepository, BasketRepository basketRepository, BasketProductRepository basketProductRepository, IMapper mapper, IAuthorizationService authorizationService)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _basketRepository = basketRepository;
            _basketProductRepository = basketProductRepository;
            _mapper = mapper;
            _authorizationService = authorizationService;
        }

        [HttpPost]
        public async Task<ActionResult<Models.Order>> CreateOrder(CreateOrderDto createOrderDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var basket = await _basketRepository.Get(createOrderDto.BasketId);
            if (basket == null) return NotFound("Basket not found");
            
            var result = await _authorizationService.AuthorizeAsync(User, basket, "BasketOwner");
            if (!result.Succeeded) return Forbid();
            
            var order = _mapper.Map<Models.Order>(createOrderDto);
            // sum of all products in basket
            var basketProducts = await _basketProductRepository.GetByBasketId(createOrderDto.BasketId);
            foreach (var basketProduct in basketProducts)
            {
                var product = await _productRepository.Get(basketProduct.Id);
                if (product != null)
                {
                    order.TotalPrice += product.ProductPrice;
                    if (product.ProductQuantity <= 0) return BadRequest("Product quantity is not enough");
                    product.ProductQuantity -= 1;
                    await _productRepository.Update(product);
                }
            }
            await _orderRepository.Add(order);
            return Ok(_mapper.Map<GetOrderDto>(order));
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.Order>>> GetOrders([FromQuery] PaginationFilter paginationFilter,
            [FromQuery] OrderColumnFilter orderColumnFilter)
        {
            var filter = new PaginationFilter(paginationFilter.PageNumber, paginationFilter.PageSize);
            var orders =
                await _orderRepository.GetAllFilteredPaginated(paginationFilter.PageNumber, paginationFilter.PageSize,
                    orderColumnFilter);

            var orderDtos = _mapper.Map<IEnumerable<GetOrderDto>>(orders);
            var totalCount = await _orderRepository.Count();
            var totalPages = (int) Math.Ceiling((double) totalCount / filter.PageSize);
            return Ok(new PaginatedResponse<IEnumerable<GetOrderDto>>(paginationFilter.PageNumber,
                paginationFilter.PageSize, totalPages, totalCount, orderDtos));
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<Models.Order>> GetOrder(int id)
        {
            var order = await _orderRepository.Get(id);
            if (order == null) return NotFound();

            var result = await _authorizationService.AuthorizeAsync(User, order, "OrderOwner");
            if (!result.Succeeded) return Forbid();
            
            return Ok(_mapper.Map<GetOrderDto>(order));
        }
        
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Models.Order>> DeleteOrder(int id)
        {
            var order = await _orderRepository.Get(id);
            if (order == null) return NotFound();

            var result = await _authorizationService.AuthorizeAsync(User, order, "OrderOwner");
            if (!result.Succeeded) return Forbid();

            await _orderRepository.Delete(order);
            return Ok(_mapper.Map<GetOrderDto>(order));
        }
    }
}