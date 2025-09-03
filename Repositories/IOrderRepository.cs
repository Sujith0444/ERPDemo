using ERPDemo_Sujith.Models.DTOs;
using Microsoft.Data.SqlClient;
using System.Data;
namespace ERPDemo_Sujith.Repositories
{
    public interface IOrderRepository
    {
        int CreateOrder(CreateOrderDto orderDto);
        IEnumerable<ProductSoldDto> GetProductsWithinDateRange(DateTime startDate, DateTime endDate);

        int CreateCustomer(CreateCustomerDto customerDto);
        int CreateProduct(CreateProductDto productDto);
    }
    public class OrderRepository : IOrderRepository
    {
        private readonly string _connection;

        public OrderRepository(IConfiguration configuration)
        {
            _connection = configuration.GetConnectionString("DBConnection");
        }
        public int CreateCustomer(CreateCustomerDto customerDto)
        {
            using var conn = new SqlConnection(_connection);
            using var cmd = new SqlCommand("SP_AddCustomer", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.Add(new SqlParameter("@CustomerName", SqlDbType.NVarChar, 200) { Value = customerDto.Name });
            cmd.Parameters.Add(new SqlParameter("@CustomerAddress", SqlDbType.NVarChar, 500) { Value = (object?)customerDto.Address ?? DBNull.Value });

            conn.Open();
            var result = cmd.ExecuteScalar();
            return Convert.ToInt32(result);
        }
        public int CreateProduct(CreateProductDto productDto)
        {
            using var conn = new SqlConnection(_connection);
            using var cmd = new SqlCommand("SP_AddProduct", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.Add(new SqlParameter("@ProductName", SqlDbType.NVarChar, 200) { Value = productDto.ProductName });
            cmd.Parameters.Add(new SqlParameter("@Price", SqlDbType.Decimal) { Precision = 18, Scale = 2, Value = productDto.Price });

            conn.Open();
            var result = cmd.ExecuteScalar();
            return Convert.ToInt32(result);
        }

        public int CreateOrder(CreateOrderDto orderDto)
        {
            using (SqlConnection conn = new SqlConnection(_connection))
            using (SqlCommand cmd = new SqlCommand("SP_CreateOrder", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@OrderNumber", orderDto.OrderNumber);
                cmd.Parameters.AddWithValue("@CustomerID", orderDto.CustomerID);
                cmd.Parameters.AddWithValue("@CustomerAddress", orderDto.CustomerAddress);

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
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public IEnumerable<ProductSoldDto> GetProductsWithinDateRange(DateTime startDate, DateTime endDate)
        {
            List<ProductSoldDto> products = new List<ProductSoldDto>();

            using (SqlConnection conn = new SqlConnection(_connection))
            using (SqlCommand cmd = new SqlCommand("SP_GetProductsWithinDateRange", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartDate", startDate);
                cmd.Parameters.AddWithValue("@EndDate", endDate);

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        products.Add(new ProductSoldDto
                        {
                            ProductID = reader.GetInt32(0),
                            ProductName = reader.GetString(1),
                            CustomerID = reader.GetInt32(2),
                            CustomerName = reader.GetString(3),
                            TotalQuantity = reader.GetInt32(4),
                            TotalAmount = reader.GetDecimal(5),
                            TotalOrders = reader.GetInt32(6)
                        });
                    }
                }
            }

            return products;
        }
    }
}
