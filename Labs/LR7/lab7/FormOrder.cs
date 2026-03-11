using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace lab7
{
    public partial class FormOrder : Form
    {
        private DatabaseHelper dbHelper;
        private int bookId;
        private decimal bookPrice;

        public FormOrder(int bookId, DatabaseHelper dbHelper)
        {
            InitializeComponent();
            this.bookId = bookId;
            this.dbHelper = dbHelper;

            InitializeForm();
            LoadOffices();
        }


        private void InitializeForm()
        {
            try
            {
                var book = GetBookInfo(bookId);
                if (book != null)
                {
                    labelBookTitle.Text = book.Name;
                    labelAuthor.Text = book.AuthorName;
                    bookPrice = GetBookPrice(bookId);

             
                }

                labelTotal.Text = "Итого: 0 руб";
                numericUpDownQuantity.Minimum = 1;
                numericUpDownQuantity.Value = 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка инициализации: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Book GetBookInfo(int id)
        {
            try
            {
                var books = dbHelper.GetPublications();
                foreach (var book in books)
                {
                    if (book.Id == id)
                        return book;
                }
            }
            catch { }
            return null;
        }

        private decimal GetBookPrice(int id)
        {
            try
            {
                return dbHelper.GetBookPriceFromOrders(id);
            }
            catch
            {
                return 0;
            }
        }

        private void LoadOffices()
        {
            try
            {
                var offices = dbHelper.GetOffices();
                comboBoxOffice.DataSource = offices;
                comboBoxOffice.DisplayMember = "OfficeName";
                comboBoxOffice.ValueMember = "Id";

                if (comboBoxOffice.Items.Count > 0)
                    comboBoxOffice.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки офисов: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void numericUpDownQuantity_ValueChanged(object sender, EventArgs e)
        {
            decimal total = bookPrice * numericUpDownQuantity.Value;
            labelTotal.Text = $"Итого: {total} руб";
        }

        private void buttonConfirm_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxFIO.Text))
            {
                MessageBox.Show("Введите ФИО клиента!", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxFIO.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(textBoxAddress.Text))
            {
                MessageBox.Show("Введите адрес!", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxAddress.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(textBoxPhone.Text))
            {
                MessageBox.Show("Введите телефон!", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxPhone.Focus();
                return;
            }

            if (comboBoxOffice.SelectedItem == null)
            {
                MessageBox.Show("Выберите офис!", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SaveOrderToDatabase();
        }

        private void SaveOrderToDatabase()
        {
            try
            {
                decimal totalAmount = bookPrice * numericUpDownQuantity.Value;
                int officeId = ((Office)comboBoxOffice.SelectedItem).Id;

                Order order = new Order
                {
                    PublicationID = bookId,
                    CustomerName = textBoxFIO.Text,
                    Address = textBoxAddress.Text,
                    Phone = textBoxPhone.Text,
                    OfficeID = officeId,
                    Quantity = (int)numericUpDownQuantity.Value,
                    Price = totalAmount,
                    DateOfAdmission = DateTime.Now
                };

                int customerId = dbHelper.CreateCustomer(
                    order.CustomerName,
                    1,
                    order.Address,
                    order.Phone
                );

                order.CustomerID = customerId;
                order.OrderName = $"Заказ книги #{bookId}";
                order.TypeProductID = 1;

                int orderId = dbHelper.CreateOrder(order);

                MessageBox.Show($"Заказ успешно оформлен!\nНомер заказа: {orderId}", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения заказа: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void FormOrder_Load(object sender, EventArgs e)
        {
        }

        private void buttonConfirm_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxFIO.Text))
            {
                MessageBox.Show("Введите ФИО!", "Ошибка");
                return;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(textBoxAddress.Text))
                 {
                        MessageBox.Show("Введите Адресс!", "Ошибка");
                        return;
                 }
                 else
                 {
                    ReceiptForm receiptForm = new ReceiptForm();
                    receiptForm.Show();
                }
            }
        }
        private int GenerateOrderId()
        {
            return new Random().Next(1000, 9999);
        }

        private void SaveOrderToDatabase(OrderData orderData)
        {
            using (var connection = new SqlConnection("ваша_строка_подключения"))
            {
                connection.Open();
                string query = @"INSERT INTO Orders 
                        (OrderID, CustomerName, CustomerAddress, BookID, OrderDate) 
                        VALUES (@OrderID, @CustomerName, @CustomerAddress, @BookID, @OrderDate)";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@OrderID", orderData.OrderId);
                    command.Parameters.AddWithValue("@CustomerName", orderData.CustomerName);
                    command.Parameters.AddWithValue("@CustomerAddress", orderData.CustomerAddress);
                    command.Parameters.AddWithValue("@BookID", orderData.BookId);
                    command.Parameters.AddWithValue("@OrderDate", orderData.OrderDate);

                    command.ExecuteNonQuery();
                }
            }
        }

        private void FormOrder_Load_1(object sender, EventArgs e)
        {

        }
    } 
}