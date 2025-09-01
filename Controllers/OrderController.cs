using ERPDemo_Sujith.Models.DTOs;
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
        private readonly string _connection;
        public OrderController(IConfiguration configuration)
        {
            _connection = configuration.GetConnectionString("DBConnection");
        }

        [HttpPost("create-customer")]
        public IActionResult CreateCustomer([FromBody] CreateCustomerDto customerDto)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connection))
                using (SqlCommand cmd = new SqlCommand("SP_CreateCustomer", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Name", customerDto.Name);
                    cmd.Parameters.AddWithValue("@Address", customerDto.Address);

                    conn.Open();
                    var newCustomerId = cmd.ExecuteScalar();
                    return Ok(new { CustomerId = newCustomerId });
                }
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
                using (SqlConnection conn = new SqlConnection(_connection))
                using (SqlCommand cmd = new SqlCommand("SP_CreateProduct", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ProductName", productDto.ProductName);
                    cmd.Parameters.AddWithValue("@Price", productDto.Price);

                    conn.Open();
                    var newProductId = cmd.ExecuteScalar();
                    return Ok(new { ProductId = newProductId });
                }
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
                    using (SqlConnection conn = new SqlConnection(_connection))
                    using (SqlCommand cmd = new SqlCommand("SP_CreateOrder", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@OrderNumber", orderDto.OrderNumber);
                        cmd.Parameters.AddWithValue("@CustomerID", orderDto.CustomerID);
                        cmd.Parameters.AddWithValue("@CustomerAddress", orderDto.CustomerAddress);

                        // Table-Valued Parameter
                        DataTable orderItemsTable = new DataTable();
                        orderItemsTable.Columns.Add("ProductID", typeof(int));
                        orderItemsTable.Columns.Add("Quantity", typeof(int));
                        orderItemsTable.Columns.Add("Price", typeof(decimal));

                        foreach (var item in orderDto.Items)
                        {
                            orderItemsTable.Rows.Add(item.ProductId, item.Quantity, item.Price);
                        }

                        SqlParameter tvpParam = cmd.Parameters.AddWithValue("@OrderItems", orderItemsTable);
                        tvpParam.SqlDbType = SqlDbType.Structured;
                        tvpParam.TypeName = "dbo.OrderItemsType";

                        conn.Open();
                        var newOrderId = cmd.ExecuteScalar();
                        return Ok(new { OrderId = newOrderId });
                    }
                }
                catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }
        [HttpGet("GetProductsSold")]
        public IActionResult GetProductsSold(DateTime startDate, DateTime endDate)
        {
            var productsSold = new List<ProductSoldDto>();

            using (SqlConnection con = new SqlConnection(_connection))
            {
                using (SqlCommand cmd = new SqlCommand("SP_GetProductsWithinDateRange", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@StartDate", startDate);
                    cmd.Parameters.AddWithValue("@EndDate", endDate);

                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            productsSold.Add(new ProductSoldDto
                            {
                                ProductID = Convert.ToInt32(reader["ProductID"]),
                                ProductName = reader["ProductName"].ToString(),
                                CustomerID = Convert.ToInt32(reader["CustomerID"]),
                                CustomerName = reader["CustomerName"].ToString(),
                                TotalQuantity = Convert.ToInt32(reader["TotalQuantityBought"]),
                                TotalAmount = Convert.ToDecimal(reader["TotalAmountSpent"]),
                                TotalOrders = Convert.ToInt32(reader["TotalOrders"])
                            });
                        }
                    }
                }
            }

            return Ok(productsSold);
        }
}
}
