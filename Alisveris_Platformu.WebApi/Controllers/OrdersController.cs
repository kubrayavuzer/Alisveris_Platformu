using Alisveris_Platformu.Business.Operations.Order;
using Alisveris_Platformu.Business.Operations.Order.Dtos;
using Alisveris_Platformu.WebApi.Filters;
using Alisveris_Platformu.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Alisveris_Platformu.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> GetOrder (int id)
        {
            var order = await _orderService.GetOrder(id);

            if (order is null)
                return NotFound();
            else
                return Ok(order);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _orderService.GetOrders();

            return Ok(orders);

        }


        [HttpPost("Add")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddOrder(AddOrderRequest request)
        {
            var addOrderDto = new AddOrderDto
            {
                OrderName = request.OrderName,
                OrderDate = request.OrderDate,
                TotalAmount = request.TotalAmount,
                ProductIds = request.ProductIds,
                CustomerId = request.CustomerId,
            };

            var result = await _orderService.AddOrder(addOrderDto);

            if(!result.IsSucceed)
            {
                return NotFound(result.Message);
            }
            else
            {
                return Ok();
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var result = await _orderService.DeleteOrder(id);

            if (!result.IsSucceed)
            {
                return NotFound(result.Message);
            }
            else
            {
                return Ok();
            }
        }


        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [TimeControlFilter]
        public async Task<IActionResult> UpdateOrder(int id, UpdateOrderRequest request)
        {
            var updateOrderDto = new UpdateOrderDto
            {
                Id = id,
                Name = request.Name,
                OrderDate = request.OrderDate,
                TotalAmount = request.TotalAmount,
                ProductIds = request.ProductIds,
                CustomerId = request.CustomerId,

            };
            var result = await _orderService.UpdateOrder(updateOrderDto);

            if (!result.IsSucceed) 
                return NotFound(result.Message);
            else
                return await GetOrder(id);

        }

        
    }
}
