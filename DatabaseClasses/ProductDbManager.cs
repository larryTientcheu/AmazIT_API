﻿using SampleRESTAPI.Models;
using System.Data.SQLite;
using System.Data;
using SampleRESTAPI.DatabaseClasses;

namespace AmazIT_API.DatabaseClasses
{
    public class ProductDbManager : DbManager
    {
        public ProductDbManager(){ }

        #region PRODUCTS
        // Product CRUD operations
        public List<Product> GetProducts()
        {
            List<Product> products = new List<Product>();

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM Products", conn))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            products.Add(CreateProductObject(reader));
                        }
                    }
                }
            }
            return products;
        }

        public Product? GetProductById(int productId)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM Products WHERE id = @ProductId", conn))
                {
                    command.Parameters.AddWithValue("@ProductId", productId);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            return CreateProductObject(reader);
                        }
                    }
                }
            }
            return null;
        }

        public List<Product>? GetProductsByName(string productName)
        {
            List<Product> products = new List<Product>();
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM Products WHERE name LIKE '%' || @ProductName || '%';", conn))
                {
                    command.Parameters.AddWithValue("@ProductName", productName);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            products.Add(CreateProductObject(reader));
                        }
                    }
                }
            }
            return products;
        }

        public List<Product>? GetProductsByCategory(string productCategory)
        {
            List<Product> products = new List<Product>();
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM Products WHERE category COLLATE NOCASE = @ProductCategory;", conn))
                {
                    command.Parameters.AddWithValue("@ProductCategory", productCategory);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            products.Add(CreateProductObject(reader));
                        }
                    }
                }
            }
            return products;
        }

        public List<Product>? GetProductsByPrice(double minPrice, double maxPrice)
        {
            List<Product> products = new List<Product>();
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM Products WHERE price >= @minPrice AND price <= @maxPrice;", conn))
                {
                    command.Parameters.AddWithValue("@minPrice", minPrice);
                    command.Parameters.AddWithValue("@maxPrice", maxPrice);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            products.Add(CreateProductObject(reader));
                        }
                    }
                }
            }
            return products;
        }

        public int AddProduct(Product product)
        {
            // Une autre façon d'utiliser les usings de manière plus courte, sans les accolades 
            using var connection = new SQLiteConnection(connectionString);
            using var command = new SQLiteCommand("INSERT INTO Products (Name, Price, Stock, Category) VALUES (@Name, @Price, @Stock, @Category); SELECT last_insert_rowid();", connection);
            command.Parameters.AddWithValue("@Name", product.Name);
            command.Parameters.AddWithValue("@Price", product.Price);
            command.Parameters.AddWithValue("@Stock", product.Stock);
            command.Parameters.AddWithValue("@Category", product.Category);


            connection.Open();

            var result = command.ExecuteScalar();



            if (result != null && int.TryParse(result.ToString(), out int newId))
            {
                return newId;
            }
            else
            {
                throw new Exception("Failed to retrieve the new product ID.");
            }
        }

        public void UpdateProduct(Product product)
        {
            // Une autre façon d'utiliser les usings de manière plus courte, sans les accolades 
            using var connection = new SQLiteConnection(connectionString);
            using var command = new SQLiteCommand("UPDATE Products SET Name = @Name, Price = @Price, Stock = @Stock, Category = @Category WHERE id = @ProductId", connection);
            command.Parameters.AddWithValue("@Name", product.Name);
            command.Parameters.AddWithValue("@Price", product.Price);
            command.Parameters.AddWithValue("@Stock", product.Stock);
            command.Parameters.AddWithValue("@Category", product.Category);

            command.Parameters.AddWithValue("@ProductId", product.ProductID);


            connection.Open();
            command.ExecuteNonQuery();
        }

        public bool DeleteProduct(int productId)
        {
            using var connection = new SQLiteConnection(connectionString);
            using var command = new SQLiteCommand("DELETE FROM Products WHERE id = @ProductId", connection);
            command.Parameters.AddWithValue("@ProductId", productId);
            connection.Open();
            if (command.ExecuteNonQuery() >= 1) return true;
            else return false;
        }


        private Product CreateProductObject(SQLiteDataReader reader)
        {
            return new Product
            {
                ProductID = Convert.ToInt32(reader["id"]),
                Name = Convert.ToString(reader["name"]),
                Price = Convert.ToDecimal(reader["price"]),
                Stock = Convert.ToInt32(reader["stock"]),
                Category = Convert.ToString(reader["category"])
            };
        }
        #endregion
    }
}
