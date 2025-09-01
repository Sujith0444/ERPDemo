namespace ERPDemo_Sujith.Models.DTOs
{
    public class ProductSoldDto
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public int TotalQuantity { get; set; }
        public decimal TotalAmount{ get; set; }
        public int TotalOrders { get; set; }
    }
}
