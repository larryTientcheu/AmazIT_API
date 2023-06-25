using System.Data.SQLite;
using SampleRESTAPI.Models;
using System.Diagnostics.Eventing.Reader;
using SampleRESTAPI.DatabaseClasses;
using AmazIT_API.Models;

namespace AmazIT_API.DatabaseClasses
{
    public class OrderItemDbManager: DbManager
    {
        public OrderItemDbManager() { }

        OrderDbManager orderDb = new OrderDbManager();
        ProductDbManager productDb = new ProductDbManager();

        private OrderItem CreateOrderItemObject(SQLiteDataReader reader)
        {
            return new OrderItem
            {
                OrderItemId = Convert.ToInt32(reader["id"]),
                OrderId = Convert.ToInt32(reader["order_id"]),
                Order = orderDb.GetOrder(Convert.ToInt32(reader["order_id"])),
                Product = productDb.GetProductById(Convert.ToInt32(reader["product_id"])),
                ProductId = Convert.ToInt32(reader["product_id"]),
                Quantity = Convert.ToInt32(reader["quantity"]),
                Price = Convert.ToDouble(reader["price"])
            };
        }

        private BestSelling CreateBestSellingObject(SQLiteDataReader reader)
        {
            return new BestSelling
            {
                ProductId = Convert.ToInt32(reader["product_id"]),
                ProductName = Convert.ToString(reader["product_name"]),
                TotalQuantity = Convert.ToInt32(reader["total_quantity"])
            };
        }


        public List<OrderItem> GetOrderItems()
        {
            List<OrderItem> orderItems = new List<OrderItem>();
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM order_items", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            orderItems.Add(CreateOrderItemObject(reader));
                        }
                    }
                }
            }
            return orderItems;
        }

        public OrderItem? GetOrderItem(int id)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand("SELECT * FROM order_items WHERE id=@id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                            return CreateOrderItemObject(reader);
                    }
                }
            }
            return null;
        }

        public BestSelling? GetBestSelling()
        {
            BestSelling bestSelling = new BestSelling();           
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                string query = @"
        SELECT
            oi.product_id, p.name AS product_name,
            SUM(oi.quantity) AS total_quantity FROM order_items AS oi
        JOIN
            products AS p ON oi.product_id = p.id
        GROUP BY
            oi.product_id,
            p.name
        ORDER BY
            total_quantity DESC
        LIMIT 1";

                using (SQLiteCommand command = new SQLiteCommand(query, conn))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {                           
                                return CreateBestSellingObject(reader);
                        }
                    }
                }
            }
            return null;
        }


        public int AddOrderItem(OrderItem orderItem)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand("INSERT INTO order_items" +
                    "(order_id, product_id, quantity, price)" +
                    "VALUES (@orderId, @productId, @quantity, @price); SELECT last_insert_rowid();", connection))
                {
                    command.Parameters.AddWithValue("@orderId", orderItem.OrderId);
                    command.Parameters.AddWithValue("@productId", orderItem.ProductId);
                    command.Parameters.AddWithValue("@quantity", orderItem.Quantity);
                    command.Parameters.AddWithValue("@price", orderItem.Price);

                    var result = command.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out int newId))
                        return newId;
                    else
                        throw new Exception("Failed to retrieve the new Order ID.");
                }
            }
        }

        public bool UpdateOrderItem(OrderItem orderItem)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand("UPDATE order_items SET order_id=@orderId, product_id=@productId, quantity=@quantity, price=@price WHERE id=@id", connection))
                {
                    command.Parameters.AddWithValue("@id", orderItem.OrderItemId);
                    command.Parameters.AddWithValue("@productId", orderItem.ProductId);
                    command.Parameters.AddWithValue("@quantity", orderItem.Quantity);
                    command.Parameters.AddWithValue("@price", orderItem.Price);

                    if (command.ExecuteNonQuery() >= 1) return true;
                    else return false;
                }
            }
        }

        public bool DeleteOrderItem(int id)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand("DELETE FROM order_items WHERE id=@id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    if (command.ExecuteNonQuery() >= 1) return true;
                    else return false;
                }
            }
        }










    }
}
