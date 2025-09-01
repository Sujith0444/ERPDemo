namespace ERPDemo_Sujith.Models.DTOs
{
    public class CreateOrderDto
    {
        public string OrderNumber { get; set; }
        public int CustomerID { get; set; }
        public string CustomerAddress { get; set; }
        public List<OrderItemDto> Items { get; set; }
    }
}
