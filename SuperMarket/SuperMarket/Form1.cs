using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Лабораторная_1
{
    public partial class Spisokproductov : Form
    {
        // строка подключения к БД
        string connectionString = "Data Source=Home_Dima;Initial Catalog=supermarket;Integrated Security=True";

        public Spisokproductov()
        {
            InitializeComponent();
        }

        // загрузка ассортимента при старте формы
        private void Spisokproductov_Load(object sender, EventArgs e)
        {
            comboBoxProducts.Items.Clear();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT name, price FROM products", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string name = reader["name"].ToString();
                    decimal price = (decimal)reader["price"];
                    comboBoxProducts.Items.Add($"{name} — {price} руб.");
                }
            }
        }

        // добавить выбранный продукт в корзину (ListBox)
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (comboBoxProducts.SelectedItem != null)
            {
                listBoxSelected.Items.Add(comboBoxProducts.SelectedItem.ToString());
            }
        }

        // очистить корзину
        private void buttonClear_Click(object sender, EventArgs e)
        {
            listBoxSelected.Items.Clear();
            textBoxTotal.Text = "";
        }

        // подсчитать итоговую сумму
        private void buttonTotal_Click(object sender, EventArgs e)
        {
            decimal total = 0;

            foreach (var item in listBoxSelected.Items)
            {
                string text = item.ToString(); // Пример: "Яблоко — 20.00 руб."
                string[] parts = text.Split('—');
                if (parts.Length == 2)
                {
                    string pricePart = parts[1].Replace("руб.", "").Trim();
                    if (decimal.TryParse(pricePart, out decimal price))
                    {
                        total += price;
                    }
                }
            }

            textBoxTotal.Text = total.ToString("C"); // формат валюты
        }

        private void Spisokproductov_Load_1(object sender, EventArgs e)
        {

        }
    }
}