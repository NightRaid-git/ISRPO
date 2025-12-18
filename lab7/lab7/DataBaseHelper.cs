using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Data;

namespace lab7
{
    public class DatabaseHelper : IDisposable
    {
        private string connectionString = @"Data Source=Home_Dima\SQLEXPRESS;Initial Catalog=Izdatelstvo;Integrated Security=True";

        public bool TestConnection()
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    return connection.State == ConnectionState.Open;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Connection test failed: {ex.Message}");
                return false;
            }
        }
        public List<Book> GetPublications()
        {
            var books = new List<Book>();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"
                    SELECT p.id_Publication, p.Name, p.AuthorID, 
                           a.Surname + ' ' + a.Name as AuthorName,
                           p.ReleaseYear, p.VolumeOfSheets, p.Circulation
                    FROM Publications p
                    LEFT JOIN Authors a ON p.AuthorID = a.id_Author
                    ORDER BY p.Name";

                using (var command = new SqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        books.Add(new Book
                        {
                            Id = (int)reader["id_Publication"],
                            Name = reader["Name"].ToString(),
                            AuthorID = reader["AuthorID"] != DBNull.Value ? (int)reader["AuthorID"] : 0,
                            AuthorName = reader["AuthorName"].ToString(),
                            ReleaseYear = (int)reader["ReleaseYear"],
                            VolumeOfSheets = (int)reader["VolumeOfSheets"],
                            Circulation = (int)reader["Circulation"]
                        });
                    }
                }
            }

            return books;
        }
        public List<Book> GetPublicationsWithPrice()
        {
            var books = new List<Book>();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"
                    SELECT 
                        p.id_Publication, 
                        p.Name, 
                        p.AuthorID, 
                        a.Surname + ' ' + a.Name as AuthorName,
                        p.ReleaseYear, 
                        p.VolumeOfSheets, 
                        p.Circulation,
                        COALESCE(
                            (SELECT TOP 1 Price FROM Orders 
                             WHERE PublicationID = p.id_Publication 
                             ORDER BY DateOfAdmission DESC),
                            p.VolumeOfSheets * 2.5
                        ) as Price
                    FROM Publications p
                    LEFT JOIN Authors a ON p.AuthorID = a.id_Author
                    ORDER BY p.Name";

                using (var command = new SqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        books.Add(new Book
                        {
                            Id = (int)reader["id_Publication"],
                            Name = reader["Name"].ToString(),
                            AuthorID = reader["AuthorID"] != DBNull.Value ? (int)reader["AuthorID"] : 0,
                            AuthorName = reader["AuthorName"].ToString(),
                            ReleaseYear = (int)reader["ReleaseYear"],
                            VolumeOfSheets = (int)reader["VolumeOfSheets"],
                            Circulation = (int)reader["Circulation"],
                            Price = reader["Price"] != DBNull.Value ? Convert.ToDecimal(reader["Price"]) : 500
                        });
                    }
                }
            }

            return books;
        }
        public List<Office> GetOffices()
        {
            var offices = new List<Office>();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT id_Office, OfficeName, Address, Phone FROM Offices ORDER BY OfficeName";

                using (var command = new SqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        offices.Add(new Office
                        {
                            Id = (int)reader["id_Office"],
                            OfficeName = reader["OfficeName"].ToString(),
                            Address = reader["Address"] != DBNull.Value ? reader["Address"].ToString() : "",
                            Phone = reader["Phone"] != DBNull.Value ? reader["Phone"].ToString() : ""
                        });
                    }
                }
            }

            return offices;
        }
        public Dictionary<int, string> GetCustomerTypes()
        {
            var types = new Dictionary<int, string>();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT id_TypeCustomer, TypeCustomer FROM TypesCustomers";

                using (var command = new SqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        types.Add((int)reader["id_TypeCustomer"], reader["TypeCustomer"].ToString());
                    }
                }
            }

            return types;
        }
        public int CreateCustomer(string customerName, int typeCustomerID, string address, string phone)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = @"
                    INSERT INTO Customers (Name, TypeCustomerID, Address, Phone)
                    VALUES (@Name, @TypeCustomerID, @Address, @Phone);
                    SELECT SCOPE_IDENTITY();";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", customerName);
                    command.Parameters.AddWithValue("@TypeCustomerID", typeCustomerID);
                    command.Parameters.AddWithValue("@Address",
                        string.IsNullOrEmpty(address) ? (object)DBNull.Value : address);
                    command.Parameters.AddWithValue("@Phone",
                        string.IsNullOrEmpty(phone) ? (object)DBNull.Value : phone);

                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }
        public int CreateOrder(Order order)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = @"
                    INSERT INTO Orders (OrderName, TypeProductID, PublicationID, OfficeID, CustomerID, 
                                        DateOfAdmission, DateOfCompletion, Price)
                    VALUES (@OrderName, @TypeProductID, @PublicationID, @OfficeID, @CustomerID, 
                            @DateOfAdmission, @DateOfCompletion, @Price);
                    SELECT SCOPE_IDENTITY();";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@OrderName",
                        string.IsNullOrEmpty(order.OrderName) ? "Новый заказ" : order.OrderName);
                    command.Parameters.AddWithValue("@TypeProductID", order.TypeProductID);
                    command.Parameters.AddWithValue("@PublicationID", order.PublicationID);
                    command.Parameters.AddWithValue("@OfficeID", order.OfficeID);
                    command.Parameters.AddWithValue("@CustomerID", order.CustomerID);
                    command.Parameters.AddWithValue("@DateOfAdmission", order.DateOfAdmission);

                    if (order.DateOfCompletion > DateTime.MinValue)
                        command.Parameters.AddWithValue("@DateOfCompletion", order.DateOfCompletion);
                    else
                        command.Parameters.AddWithValue("@DateOfCompletion", DBNull.Value);

                    command.Parameters.AddWithValue("@Price", order.Price);

                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }
        public int CreateFullOrder(string customerName, string address, string phone,
                                  int publicationId, int officeId, int quantity, decimal price)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    int customerId = 0;
                    string createCustomerQuery = @"
                        INSERT INTO Customers (Name, Address, Phone, TypeCustomerID)
                        VALUES (@Name, @Address, @Phone, 1);
                        SELECT SCOPE_IDENTITY();";

                    using (var cmdCustomer = new SqlCommand(createCustomerQuery, connection, transaction))
                    {
                        cmdCustomer.Parameters.AddWithValue("@Name", customerName);
                        cmdCustomer.Parameters.AddWithValue("@Address", address);
                        cmdCustomer.Parameters.AddWithValue("@Phone", phone);
                        customerId = Convert.ToInt32(cmdCustomer.ExecuteScalar());
                    }

                    decimal totalAmount = price * quantity;
                    string createOrderQuery = @"
                        INSERT INTO Orders (OrderName, TypeProductID, PublicationID, OfficeID, CustomerID, 
                                            DateOfAdmission, Price)
                        VALUES (@OrderName, 1, @PublicationID, @OfficeID, @CustomerID, 
                                @DateOfAdmission, @TotalAmount);
                        SELECT SCOPE_IDENTITY();";

                    using (var cmdOrder = new SqlCommand(createOrderQuery, connection, transaction))
                    {
                        cmdOrder.Parameters.AddWithValue("@OrderName", $"Заказ книги #{publicationId}");
                        cmdOrder.Parameters.AddWithValue("@PublicationID", publicationId);
                        cmdOrder.Parameters.AddWithValue("@OfficeID", officeId);
                        cmdOrder.Parameters.AddWithValue("@CustomerID", customerId);
                        cmdOrder.Parameters.AddWithValue("@DateOfAdmission", DateTime.Now);
                        cmdOrder.Parameters.AddWithValue("@TotalAmount", totalAmount);

                        int orderId = Convert.ToInt32(cmdOrder.ExecuteScalar());

                        transaction.Commit();
                        return orderId;
                    }
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
        public Order GetOrderDetails(int orderId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = @"
                    SELECT 
                        o.id_Order, 
                        o.OrderName, 
                        o.TypeProductID,
                        o.PublicationID,
                        o.OfficeID,
                        o.CustomerID,
                        o.DateOfAdmission, 
                        o.DateOfCompletion,
                        o.Price,
                        p.Name as BookName, 
                        c.Name as CustomerName,
                        c.Address as CustomerAddress,
                        c.Phone as CustomerPhone,
                        off.Office as OfficeName
                    FROM Orders o
                    LEFT JOIN Publications p ON o.PublicationID = p.id_Publication
                    LEFT JOIN Customers c ON o.CustomerID = c.id_Customer
                    LEFT JOIN Offices off ON o.OfficeID = off.id_Office
                    WHERE o.id_Order = @OrderId";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@OrderId", orderId);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var order = new Order
                            {
                                id_Order = (int)reader["id_Order"],
                                OrderName = reader["OrderName"].ToString(),
                                TypeProductID = reader["TypeProductID"] != DBNull.Value ? (int)reader["TypeProductID"] : 0,
                                PublicationID = reader["PublicationID"] != DBNull.Value ? (int)reader["PublicationID"] : 0,
                                OfficeID = reader["OfficeID"] != DBNull.Value ? (int)reader["OfficeID"] : 0,
                                CustomerID = reader["CustomerID"] != DBNull.Value ? (int)reader["CustomerID"] : 0,
                                DateOfAdmission = (DateTime)reader["DateOfAdmission"],
                                Price = (decimal)reader["Price"]
                            };

                            order.CustomerName = reader["CustomerName"]?.ToString();
                            order.Address = reader["CustomerAddress"]?.ToString();
                            order.Phone = reader["CustomerPhone"]?.ToString();

                            if (reader["DateOfCompletion"] != DBNull.Value)
                            {
                                order.DateOfCompletion = (DateTime)reader["DateOfCompletion"];
                            }

                            return order;
                        }
                    }
                }
            }

            return null;
        }
        public decimal GetBookPriceFromOrders(int publicationId)
        {
            decimal price = 0;

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = @"SELECT TOP 1 Price 
                                FROM Orders 
                                WHERE PublicationID = @PublicationId 
                                ORDER BY DateOfAdmission DESC";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PublicationId", publicationId);

                    object result = command.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        price = Convert.ToDecimal(result);
                    }
                    else
                    {
                        price = CalculateDefaultPrice(publicationId);
                    }
                }
            }

            return price;
        }
        private decimal CalculateDefaultPrice(int publicationId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT VolumeOfSheets FROM Publications WHERE id_Publication = @Id";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", publicationId);
                    object result = command.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        int pages = Convert.ToInt32(result);
                        return pages * 2.5m;
                    }
                }
            }

            return 500;
        }
        public Dictionary<int, string> GetProductTypes()
        {
            var types = new Dictionary<int, string>();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT id_Type, Type FROM TypesOfProducts";

                using (var command = new SqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        types.Add((int)reader["id_Type"], reader["Type"].ToString());
                    }
                }
            }

            return types;
        }

        public void Dispose()
        {

        }
    }
}