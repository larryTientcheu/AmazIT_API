using SampleRESTAPI.DatabaseClasses;
using SampleRESTAPI.Models;
using System.Data.SQLite;

namespace AmazIT_API.DatabaseClasses
{
    public class CustomerDbManager : DbManager
    {
        public CustomerDbManager(string connectionString) : base(connectionString){}


        #region CUSTOMERS

        public List<Customer> GetCustomers()
        {
            List<Customer> customers = new List<Customer>();

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM Customers", conn))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            customers.Add(CreateCustomerObject(reader));
                        }
                    }
                }
            }

            return customers;
        }

        public Customer GetCustomerById(int id)
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                using (var command = new SQLiteCommand("SELECT * FROM Customers WHERE id=@id", conn))
                {
                    command.Parameters.AddWithValue("@id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return CreateCustomerObject(reader);
                        }
                    }
                }
            }

            return null;
        }

        private Customer CreateCustomerObject(SQLiteDataReader reader)
        {
            return new Customer
            {
                CustomerID = Convert.ToInt32(reader["id"]),
                FirstName = Convert.ToString(reader["first_name"]),
                LastName = Convert.ToString(reader["last_name"]),
                Email = Convert.ToString(reader["email"]),
                Phone = Convert.ToString(reader["phone"])
            };
        }

        public int AddCustomer(Customer customer)
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                using (var command = new SQLiteCommand("INSERT INTO Customers (first_name, last_name, email, phone) "
                                                        + "VALUES (@firstName, @lastName, @email, @phone); SELECT last_insert_rowid();", conn))
                {
                    command.Parameters.AddWithValue("@firstName", customer.FirstName);
                    command.Parameters.AddWithValue("@lastName", customer.LastName);
                    command.Parameters.AddWithValue("@email", customer.Email);
                    command.Parameters.AddWithValue("@phone", customer.Phone);

                    var result = command.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out int newId))
                    {
                        return newId;
                    }
                    else
                    {
                        throw new Exception("Failed to retrieve the new customer ID.");
                    }
                }
            }
        }

        public bool UpdateCustomer(Customer customer)
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                using (var command = new SQLiteCommand("UPDATE Customers SET first_name=@firstName, last_name=@lastName, email=@email, phone=@phone WHERE id=@id", conn))
                {
                    command.Parameters.AddWithValue("@id", customer.CustomerID);
                    command.Parameters.AddWithValue("@firstName", customer.FirstName);
                    command.Parameters.AddWithValue("@lastName", customer.LastName);
                    command.Parameters.AddWithValue("@email", customer.Email);
                    command.Parameters.AddWithValue("@phone", customer.Phone);

                    if (command.ExecuteNonQuery() >= 1) return true;
                    else return false;
                }
            }
        }

        public bool DeleteCustomer(int id)
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                using (var command = new SQLiteCommand("DELETE FROM Customers WHERE id=@id", conn))
                {
                    command.Parameters.AddWithValue("@id", id);

                    if (command.ExecuteNonQuery() >= 1) return true;
                    else return false;
                }
            }
        }
        #endregion
    }
}
