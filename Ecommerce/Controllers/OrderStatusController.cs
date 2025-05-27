
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;


namespace Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderStatusController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public OrderStatusController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("{orderId}")]
        public IActionResult GetOrderStatus(int orderId)
        {
            string? connStr = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection connection = new SqlConnection(connStr))
            {
                string query = @"
                    SELECT 
                        o.OrderID, o.CustomerID, o.ProductID, o.DealerID, o.Quantity, o.OrderDate, o.AddressId,
                        p.ProductName, p.Description, p.Price, p.ImagePath,
                        os.StatusID, os.Status, os.StatusTimestamp
                    FROM Orders o
                    JOIN Products p ON o.ProductID = p.ProductID
                    LEFT JOIN OrderStatus os ON o.OrderID = os.OrderID
                    WHERE o.OrderID = @OrderID
                    ORDER BY os.StatusTimestamp ASC, os.StatusID ASC";

                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@OrderID", orderId);

                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (!reader.HasRows)
                        return NotFound(new { Message = "Order not found." });

                    var result = new
                    {
                        OrderID = 0,
                        CustomerID = 0,
                        ProductID = 0,
                        DealerID = 0,
                        Quantity = 0,
                        OrderDate = DateTime.MinValue,
                        AddressId = 0,
                        Product = new { ProductName = "", Price = 0m, ImagePath = "" },
                        OrderStatuses = new List<object>()
                    };

                    bool firstRow = true;
                    while (reader.Read())
                    {
                        if (firstRow)
                        {
                            result = new
                            {
                                OrderID = (int)reader["OrderID"],
                                CustomerID = (int)reader["CustomerID"],
                                ProductID = (int)reader["ProductID"],
                                DealerID = (int)reader["DealerID"],
                                Quantity = (int)reader["Quantity"],
                                OrderDate = (DateTime)reader["OrderDate"],
                                AddressId = (int)reader["AddressId"],
                                Product = new
                                {
                                    ProductName = reader["ProductName"].ToString(),
                                    Price = (decimal)reader["Price"],
                                    ImagePath = reader["ImagePath"].ToString()
                                },
                                OrderStatuses = new List<object>()
                            };
                            firstRow = false;
                        }

                        if (reader["StatusID"] != DBNull.Value)
                        {
                            ((List<object>)result.OrderStatuses).Add(new
                            {
                                StatusID = (int)reader["StatusID"],
                                Status = reader["Status"].ToString(),
                                StatusTimestamp = reader["StatusTimestamp"] != DBNull.Value
                                    ? ((DateTime)reader["StatusTimestamp"]).ToString("o")
                                    : null
                            });
                        }
                    }

                    if (result.OrderStatuses.Count == 0)
                    {
                        ((List<object>)result.OrderStatuses).Add(new
                        {
                            StatusID = 0,
                            Status = "Order is placed",
                            StatusTimestamp = ((DateTime)result.OrderDate).ToString("o")
                        });
                    }

                    return Ok(result);
                }
            }
        }

        [HttpPost]
        public IActionResult AddOrderStatus([FromBody] OrderStatusRequest request)
        {
            if (request == null || request.OrderID <= 0 || string.IsNullOrEmpty(request.Status))
                return BadRequest(new { Message = "Invalid order status data." });

            try
            {
                // Parse statusTimestamp, fallback to server time if invalid or missing
                DateTime statusTimestamp;
                if (string.IsNullOrEmpty(request.StatusTimestamp) || !DateTime.TryParse(request.StatusTimestamp, out statusTimestamp))
                {
                    statusTimestamp = DateTime.UtcNow;
                }

                string? connStr = _configuration.GetConnectionString("DefaultConnection");

                using (SqlConnection connection = new SqlConnection(connStr))
                {
                    string query = @"
                        INSERT INTO OrderStatus (OrderID, Status, StatusTimestamp)
                        VALUES (@OrderID, @Status, @StatusTimestamp);
                        SELECT SCOPE_IDENTITY();";

                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@OrderID", request.OrderID);
                    cmd.Parameters.AddWithValue("@Status", request.Status);
                    cmd.Parameters.AddWithValue("@StatusTimestamp", statusTimestamp);

                    connection.Open();
                    var statusID = cmd.ExecuteScalar();

                    return Ok(new
                    {
                        StatusID = Convert.ToInt32(statusID),
                        OrderID = request.OrderID,
                        Status = request.Status,
                        StatusTimestamp = statusTimestamp.ToString("o")
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Failed to add order status.", Error = ex.Message });
            }
        }
    }

    public class OrderStatusRequest
    {
        public int OrderID { get; set; }
        public string? Status { get; set; }
        public string? StatusTimestamp { get; set; }
    }
}