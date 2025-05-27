using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace dealerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DealerOrderController : ControllerBase
    {
        private readonly string _connectionString;
        private readonly ILogger<DealerOrderController> _logger;

        public DealerOrderController(IConfiguration configuration, ILogger<DealerOrderController> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
        }

        // GET: api/DealerOrder/{dealerId}
        [HttpGet("{dealerId}")]
        public async Task<IActionResult> GetOrdersByDealer(int dealerId)
        {
            try
            {
                var orders = new List<OrderDto>();

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string query = @"
                        SELECT 
                            o.OrderId,
                            o.OrderDate,
                           
                            o.quantity,
                            o.ProductId,
                            ca.AddressId,
                            ca.Address,
                            c.CustomerName,
                            c.ContactNumber,
                            c.Email,
                            os.Status,
                            p.ProductName,
                            p.Price,
                            p.Description,
                            p.ImagePath
                        FROM Orders o
                        INNER JOIN CustomerAddress ca ON o.AddressId = ca.AddressId
                        INNER JOIN Customers c ON o.CustomerId = c.CustomerId
                        INNER JOIN OrderStatus os ON o.OrderId = os.OrderId
                        INNER JOIN Products p ON o.ProductId = p.ProductId
                        WHERE o.DealerId = @DealerId
                        ORDER BY o.OrderDate DESC;";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@DealerId", dealerId);

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            _logger.LogInformation($"Query returned {reader.FieldCount} columns.");

                            while (await reader.ReadAsync())
                            {
                                orders.Add(new OrderDto
                                {
                                    OrderId = reader.GetInt32(reader.GetOrdinal("OrderId")),
                                    OrderDate = reader.GetDateTime(reader.GetOrdinal("OrderDate")),
                                    quantity = reader.GetInt32(reader.GetOrdinal("quantity")),
                                    Status = reader.GetString(reader.GetOrdinal("Status")),
                                    Address = new AddressDto
                                    {
                                        AddressId = reader.GetInt32(reader.GetOrdinal("AddressId")),
                                        AddressLine = reader.GetString(reader.GetOrdinal("Address"))
                                    },
                                    Customer = new CustomerDto
                                    {
                                        Name = reader.GetString(reader.GetOrdinal("CustomerName")),
                                        ContactNumber = reader.IsDBNull(reader.GetOrdinal("ContactNumber")) ? null : reader.GetString(reader.GetOrdinal("ContactNumber")),
                                        EmailId = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString(reader.GetOrdinal("Email"))
                                    },
                                    Product = new ProductDto
                                    {
                                        ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                                        ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                                        Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                        Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                                        ImagePath = reader.IsDBNull(reader.GetOrdinal("ImagePath")) ? null : reader.GetString(reader.GetOrdinal("ImagePath"))
                                    }
                                });
                            }
                        }
                    }
                }

                return Ok(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving dealer orders for DealerId {DealerId}", dealerId);
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

       
        [HttpPut("status/{orderId}")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromBody] UpdateStatusDto updateStatusDto)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    
                    string updateQuery = @"
                        UPDATE OrderStatus
                        SET Status = @Status
                        WHERE OrderId = @OrderId;";

                    using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection))
                    {
                        updateCommand.Parameters.AddWithValue("@OrderId", orderId);
                        updateCommand.Parameters.AddWithValue("@Status", updateStatusDto.Status ?? string.Empty);

                        int rowsAffected = await updateCommand.ExecuteNonQueryAsync();
                        if (rowsAffected == 0)
                        {
                            _logger.LogWarning("No rows affected for OrderId {OrderId} in OrderStatus", orderId);
                            return NotFound($"Order with ID {orderId} not found in OrderStatus.");
                        }
                    }
                }

                _logger.LogInformation("Order {OrderId} status updated to {Status}", orderId, updateStatusDto.Status);
                return Ok(new { Message = "Order status updated successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating order status for OrderId {OrderId}", orderId);
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }

    public class OrderDto
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public int quantity { get; set; }
        public string? Status { get; set; }
        public AddressDto? Address { get; set; }
        public CustomerDto? Customer { get; set; }
        public ProductDto? Product { get; set; }
    }

    public class AddressDto
    {
        public int AddressId { get; set; }
        public string? AddressLine { get; set; }
    }

    public class CustomerDto
    {
        public string? Name { get; set; }
        public string? ContactNumber { get; set; }
        public string? EmailId { get; set; }
    }

    public class ProductDto
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public string? ImagePath { get; set; }
    }

    public class UpdateStatusDto
    {
        public string? Status { get; set; }
    }
}