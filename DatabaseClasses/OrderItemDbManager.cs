using System.Data.SQLite;
using SampleRESTAPI.Models;
using System.Diagnostics.Eventing.Reader;
using SampleRESTAPI.DatabaseClasses;

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
