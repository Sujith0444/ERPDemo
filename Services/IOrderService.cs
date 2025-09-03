using ERPDemo_Sujith.Models.DTOs;
using ERPDemo_Sujith.Repositories;

namespace ERPDemo_Sujith.Services
{
    public interface IOrderService
    {
        int CreateOrder(CreateOrderDto orderDto);
        IEnumerable<ProductSoldDto> GetProductsWithinDateRange(DateTime startDate, DateTime endDate);
        int CreateCustomer(CreateCustomerDto customerDto);
        int CreateProduct(CreateProductDto productDto);
    }
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public int CreateOrder(CreateOrderDto orderDto)
        {
            // Example: validation logic
            if (orderDto.Items == null || !orderDto.Items.Any())
                throw new Exception("Order must contain at least one product.");

            return _orderRepository.CreateOrder(orderDto);
        }

        public IEnumerable<ProductSoldDto> GetProductsWithinDateRange(DateTime startDate, DateTime endDate)
        {
            return _orderRepository.GetProductsWithinDateRange(startDate, endDate);
        }
        public int CreateCustomer(CreateCustomerDto customerDto)
        {
            return _orderRepository.CreateCustomer(customerDto);
        }

        public int CreateProduct(CreateProductDto productDto)
        {
            return _orderRepository.CreateProduct(productDto);
        }
    }
}
