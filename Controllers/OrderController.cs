using ERPDemo_Sujith.Models.DTOs;
using ERPDemo_Sujith.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Security.AccessControl;

namespace ERPDemo_Sujith.Controllers
{
    [Route("api/OrderController")]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("create-customer")]
        public IActionResult CreateCustomer([FromBody] CreateCustomerDto customerDto)
        {
            try
            {
                var id = _orderService.CreateCustomer(customerDto);
                return Ok(new { CustomerId = id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpPost("create-product")]
        public IActionResult CreateProduct([FromBody] CreateProductDto productDto)
        {
            try
            {
                var id = _orderService.CreateProduct(productDto);
                return Ok(new { ProductId = id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpPost("create-order")]
        public IActionResult CreateOrder([FromBody] CreateOrderDto orderDto)
        {
            try
            {
                var newOrderId = _orderService.CreateOrder(orderDto);
                return Ok(new { OrderId = newOrderId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }
        [HttpGet("GetProductsSold")]
        public IActionResult GetProductsSold(DateTime startDate, DateTime endDate)
        {
            var products = _orderService.GetProductsWithinDateRange(startDate, endDate);
            return Ok(products);
        }
    }
}
