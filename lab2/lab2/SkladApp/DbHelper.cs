using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SkladApp;

public static class DbHelper
{
    private static string ConnString = @"Data Source=Home_Dima\SQLEXPRESS;Initial Catalog=Sklad;Integrated Security=True";
    public static List<Product> GetAllProducts()
    {
        var list = new List<Product>();
        using (var cn = new SqlConnection(ConnString))
        using (var cmd = new SqlCommand("SELECT id, name, stillage, cell, quantity FROM products ORDER BY id", cn))
        {
            cn.Open();
            using (var r = cmd.ExecuteReader())
            {
                while (r.Read())
                {
                    list.Add(new Product
                    {
                        Id = r.GetInt32(0),
                        Name = r.GetString(1),
                        Stillage = r.GetInt32(2),
                        Cell = r.GetInt32(3),
                        Quantity = r.GetInt32(4)
                    });
                }
            }
        }
        return list;
    }

    // Возвращает сгенерированный id
    public static int InsertProduct(Product p)
    {
        using (var cn = new SqlConnection(ConnString))
        using (var cmd = new SqlCommand(
            "INSERT INTO products (name, stillage, cell, quantity) OUTPUT INSERTED.id VALUES (@name,@stillage,@cell,@quantity)", cn))
        {
            cmd.Parameters.AddWithValue("@name", p.Name);
            cmd.Parameters.AddWithValue("@stillage", p.Stillage);
            cmd.Parameters.AddWithValue("@cell", p.Cell);
            cmd.Parameters.AddWithValue("@quantity", p.Quantity);
            cn.Open();
            return (int)cmd.ExecuteScalar();
        }
    }

    public static void UpdateProduct(Product p)
    {
        using (var cn = new SqlConnection(ConnString))
        using (var cmd = new SqlCommand(
            "UPDATE products SET name=@name, stillage=@stillage, cell=@cell, quantity=@quantity WHERE id=@id", cn))
        {
            cmd.Parameters.AddWithValue("@id", p.Id);
            cmd.Parameters.AddWithValue("@name", p.Name);
            cmd.Parameters.AddWithValue("@stillage", p.Stillage);
            cmd.Parameters.AddWithValue("@cell", p.Cell);
            cmd.Parameters.AddWithValue("@quantity", p.Quantity);
            cn.Open();
            cmd.ExecuteNonQuery();
        }
    }

    public static void DeleteProduct(int id)
    {
        using (var cn = new SqlConnection(ConnString))
        using (var cmd = new SqlCommand("DELETE FROM products WHERE id=@id", cn))
        {
            cmd.Parameters.AddWithValue("@id", id);
            cn.Open();
            cmd.ExecuteNonQuery();
        }
    }

    public static List<Product> SearchByName(string namePart)
    {
        var list = new List<Product>();
        using (var cn = new SqlConnection(ConnString))
        using (var cmd = new SqlCommand("SELECT id, name, stillage, cell, quantity FROM products WHERE name LIKE @p ORDER BY id", cn))
        {
            cmd.Parameters.AddWithValue("@p", "%" + namePart + "%");
            cn.Open();
            using (var r = cmd.ExecuteReader())
            {
                while (r.Read())
                {
                    list.Add(new Product
                    {
                        Id = r.GetInt32(0),
                        Name = r.GetString(1),
                        Stillage = r.GetInt32(2),
                        Cell = r.GetInt32(3),
                        Quantity = r.GetInt32(4)
                    });
                }
            }
        }
        return list;
    }

    public static List<Product> SearchByCoords(int stillage, int cell)
    {
        var list = new List<Product>();
        using (var cn = new SqlConnection(ConnString))
        using (var cmd = new SqlCommand("SELECT id, name, stillage, cell, quantity FROM products WHERE stillage=@s AND cell=@c ORDER BY id", cn))
        {
            cmd.Parameters.AddWithValue("@s", stillage);
            cmd.Parameters.AddWithValue("@c", cell);
            cn.Open();
            using (var r = cmd.ExecuteReader())
            {
                while (r.Read())
                {
                    list.Add(new Product
                    {
                        Id = r.GetInt32(0),
                        Name = r.GetString(1),
                        Stillage = r.GetInt32(2),
                        Cell = r.GetInt32(3),
                        Quantity = r.GetInt32(4)
                    });
                }
            }
        }
        return list;
    }
}
