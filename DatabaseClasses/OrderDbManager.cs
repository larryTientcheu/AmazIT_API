using SampleRESTAPI.DatabaseClasses;
using SampleRESTAPI.Models;
using System.Data.SQLite;
using System.Diagnostics.Eventing.Reader;

namespace AmazIT_API.DatabaseClasses
{
    public class OrderDbManager : DbManager
    {
        public OrderDbManager(){ }
        CustomerDbManager customerDb = new CustomerDbManager();

        private Order CreateOrderObject(SQLiteDataReader reader)
        {
            return new Order
            {
                OrderId = Convert.ToInt32(reader["id"]),
                CustomerId = Convert.ToInt32(reader["customer_id"]),
                Customer = customerDb.GetCustomerById(Convert.ToInt32(reader["customer_id"])),
                OrderDate = Convert.ToString(reader["order_date"]),
                Total = Convert.ToDouble(reader["total"])
            };
        }
        public List<Order> GetOrders()
        {
            List<Order> orders = new List<Order>();
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM Orders", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            orders.Add(CreateOrderObject(reader));
                        }
                    }
                }
            }
            return orders;
        }

        public Order? GetOrder(int id)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand("SELECT * FROM Orders WHERE id=@id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())                        
                            return CreateOrderObject(reader);                        
                    }
                }
            }
            return null;
        }


        public List<Order>? GetOrderByCustomer(int customerId)
        {
            List<Order> orders = new List<Order>();
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM Orders WHERE customer_id = @customerId;", conn))
                {
                    command.Parameters.AddWithValue("@customerId", customerId);                    
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            orders.Add(CreateOrderObject(reader));
                        }
                    }
                }
            }
            return orders;
        }

        public List<Order>? GetOrderGreaterThanTotal(int total)
        {
            List<Order> orders = new List<Order>();
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM Orders WHERE total > @total;", conn))
                {
                    command.Parameters.AddWithValue("@total", total); ;
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            orders.Add(CreateOrderObject(reader));
                        }
                    }
                }
            }
            return orders;
        }



        public int AddOrder(Order order)
        {
            using(var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand("INSERT INTO Orders"+
                    "(customer_id, order_date, total)"+
                    "VALUES (@customerId, @orderDate, @total); SELECT last_insert_rowid();", connection))
                {
                    command.Parameters.AddWithValue("@customerId", order.CustomerId);
                    command.Parameters.AddWithValue("@orderDate", order.OrderDate);
                    command.Parameters.AddWithValue("@total", order.Total);

                    var result = command.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out int newId))
                        return newId;
                    else
                        throw new Exception("Failed to retrieve the new Order ID.");
                }
            }
        }

        public bool UpdateOrder(Order order)
        {
            using (var  connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand("UPDATE Orders SET customer_id=@customerId, order_date=@orderDate, total=@total WHERE id=@id", connection))
                {
                    command.Parameters.AddWithValue("@id", order.OrderId);
                    command.Parameters.AddWithValue("@customerId", order.CustomerId);
                    command.Parameters.AddWithValue("@orderDate", order.OrderDate);
                    command.Parameters.AddWithValue("@total", order.Total);

                    if (command.ExecuteNonQuery() >= 1) return true;
                    else return false;
                }
            }
        }

        public bool DeleteOrder(int id)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand("DELETE FROM Orders WHERE id=@id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    if (command.ExecuteNonQuery() >= 1) return true;
                    else return false;
                }
            }
        }


        







    }
}
