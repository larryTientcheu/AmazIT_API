using SampleRESTAPI.DatabaseClasses;
using SampleRESTAPI.Models;
using System.Data.SQLite;

namespace AmazIT_API.DatabaseClasses
{
    public class EmployeeDbManager: DbManager
    {
        public EmployeeDbManager() { }

        private Employee CreateEmployeeObject(SQLiteDataReader reader)
        {
            return new Employee
            {
                EmployeeId = Convert.ToInt32(reader["id"]),
                FirstName = Convert.ToString(reader["first_name"]),
                LastName = Convert.ToString(reader["last_name"]),
                Email = Convert.ToString(reader["email"]),
                Phone = Convert.ToString(reader["phone"]),
                HireDate = Convert.ToString(reader["hire_date"]),
                Salary = Convert.ToDouble(reader["salary"]),
                IsManager = Convert.ToBoolean(reader["is_manager"])

            };
        }
        public List<Employee> GetEmployees()
        {
            List<Employee> orders = new List<Employee>();
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM employees", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            orders.Add(CreateEmployeeObject(reader));
                        }
                    }
                }
            }
            return orders;
        }

        public Employee? GetEmployee(int id)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand("SELECT * FROM Employees WHERE id=@id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                            return CreateEmployeeObject(reader);
                    }
                }
            }
            return null;
        }

        public int AddEmployee(Employee employee)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand("INSERT INTO Employees" +
                    "(first_name, last_name, email, phone, hire_date, salary, is_manager )" +
                    "VALUES (@firstName, @lastName, @email, @phone, @hireDate, @salary , @isManager); SELECT last_insert_rowid();", connection))
                {                  
                    command.Parameters.AddWithValue("@firstName", employee.FirstName);
                    command.Parameters.AddWithValue("@lastName", employee.LastName);
                    command.Parameters.AddWithValue("@email", employee.Email);
                    command.Parameters.AddWithValue("@phone", employee.Phone);
                    command.Parameters.AddWithValue("@hireDate", employee.HireDate);
                    command.Parameters.AddWithValue("@salary", employee.Salary);
                    command.Parameters.AddWithValue("@isManager", employee.IsManager);
                   

                    var result = command.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out int newId))
                        return newId;
                    else
                        throw new Exception("Failed to retrieve the new Employee ID.");
                }
            }
        }

        public bool UpdateEmployee(Employee employee)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand("UPDATE Employees SET first_name=@firstName, last_name=@lastName, email=@email," +
                    "phone=@phone, hire_date=@hireDate, salary=@salary, is_manager=@isManager WHERE id=@id", connection))
                {
                    command.Parameters.AddWithValue("@id", employee.EmployeeId);                  
                    command.Parameters.AddWithValue("@firstName", employee.FirstName);
                    command.Parameters.AddWithValue("@lastName", employee.LastName);
                    command.Parameters.AddWithValue("@email", employee.Email);
                    command.Parameters.AddWithValue("@phone", employee.Phone);
                    command.Parameters.AddWithValue("@hireDate", employee.HireDate);
                    command.Parameters.AddWithValue("@salary", employee.Salary);
                    command.Parameters.AddWithValue("@isManager", employee.IsManager);

                    if (command.ExecuteNonQuery() >= 1) return true;
                    else return false;
                }
            }
        }

        public bool DeleteEmployee(int id)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand("DELETE FROM Employees WHERE id=@id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    if (command.ExecuteNonQuery() >= 1) return true;
                    else return false;
                }
            }
        }
    }
}
