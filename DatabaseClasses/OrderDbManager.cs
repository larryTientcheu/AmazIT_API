using SampleRESTAPI.DatabaseClasses;
using SampleRESTAPI.Models;
using System.Data.SQLite;

namespace AmazIT_API.DatabaseClasses
{
    public class OrderDbManager : DbManager
    {
        public OrderDbManager(){ }
        CustomerDbManager customerDb = new CustomerDbManager();

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







    }
}
