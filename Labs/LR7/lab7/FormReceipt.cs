using System;
using System.Drawing;
using System.Windows.Forms;

namespace lab7
{
    public partial class ReceiptForm : Form
    {
        private Order order;
        private OrderData orderData;
        public ReceiptForm(Order order)
        {
            this.order = order;
            InitializeForm();
        }
        public ReceiptForm()
        {
            InitializeComponent();
            this.order = new Order();
        }
        private void InitializeForm()
        {
            this.Text = "Чек предзаказа";
            this.Size = new Size(450, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            TextBox txtReceipt = new TextBox
            {
                Location = new Point(20, 20),
                Size = new Size(390, 400),
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                Font = new Font("Courier New", 10),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.WhiteSmoke,
                Text = GenerateReceipt()
            };


            Button btnClose = new Button
            {
                Text = "Закрыть",
                Location = new Point(170, 430),
                Size = new Size(100, 35),
                BackColor = Color.DodgerBlue,
                ForeColor = Color.White,
                Font = new Font("Arial", 10, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            btnClose.FlatAppearance.BorderSize = 0;


            Button btnPrint = new Button
            {
                Text = "Сохранить чек",
                Location = new Point(170, 470),
                Size = new Size(100, 35),
                BackColor = Color.LightGreen,
                ForeColor = Color.Black,
                Font = new Font("Arial", 10),
                FlatStyle = FlatStyle.Flat
            };
            btnPrint.FlatAppearance.BorderSize = 0;

            btnPrint.Click += (sender, e) =>
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "Текстовый файл (*.txt)|*.txt",
                    FileName = $"Чек_предзаказа_{orderData.OrderId}.txt"
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        System.IO.File.WriteAllText(saveFileDialog.FileName, txtReceipt.Text);
                        MessageBox.Show("Чек сохранен успешно!", "Успех",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            };

            btnClose.Click += (sender, e) => this.Close();

            this.Controls.Add(txtReceipt);
            this.Controls.Add(btnClose);
            this.Controls.Add(btnPrint);
        }

        private string GenerateReceipt()
        {
            label1.Text = "==========================================\n" +
                   "         ЧЕК ПРЕДЗАКАЗА КНИГИ          \n" +
                   "==========================================\n" +
                   $"Номер: {orderData.OrderId}\n" +
                   "------------------------------------------\n" +
                   "          ИНФОРМАЦИЯ О КНИГЕ            \n" +
                   "------------------------------------------\n" +
                   $"Название: {orderData.BookId}\n" +
                   "------------------------------------------\n" +
                   "          ИНФОРМАЦИЯ О КЛИЕНТЕ          \n" +
                   "------------------------------------------\n" +
                   $"Клиент: {orderData.CustomerName}\n" +
                   $"Офис получения: {order.OfficeID}\n" +
                   "------------------------------------------\n" +
                   "               ОПЛАТА                   \n" +
                   "------------------------------------------\n" +
                   $"Сумма: {order.Price:C}\n" +
                   $"Статус: ПРЕДЗАКАЗ\n" +
                   "------------------------------------------\n" +
                   "           ИНФОРМАЦИЯ                   \n" +
                   "------------------------------------------\n" +
                   "Книга будет доступна для получения      \n" +
                   "после её поступления в выбранный офис.  \n" +
                   "О дате готовности вы будете уведомлены  \n" +
                   "по указанному телефону.                 \n" +
                   "                                        \n" +
                   "Спасибо за ваш предзаказ!               \n" +
                   "==========================================\n";

            return "==========================================\n" +
                   "         ЧЕК ПРЕДЗАКАЗА КНИГИ          \n" +
                   "==========================================\n" +
                   $"Номер: {orderData.OrderId}\n" +
                   "------------------------------------------\n" +
                   "          ИНФОРМАЦИЯ О КНИГЕ            \n" +
                   "------------------------------------------\n" +
                   $"Название: {orderData.BookId}\n" +
                   "------------------------------------------\n" +
                   "          ИНФОРМАЦИЯ О КЛИЕНТЕ          \n" +
                   "------------------------------------------\n" +
                   $"Клиент: {orderData.CustomerName}\n" +
                   $"Офис получения: {order.OfficeID}\n" +
                   "------------------------------------------\n" +
                   "               ОПЛАТА                   \n" +
                   "------------------------------------------\n" +
                   $"Сумма: {order.Price:C}\n" +
                   $"Статус: ПРЕДЗАКАЗ\n" +
                   "------------------------------------------\n" +
                   "           ИНФОРМАЦИЯ                   \n" +
                   "------------------------------------------\n" +
                   "Книга будет доступна для получения      \n" +
                   "после её поступления в выбранный офис.  \n" +
                   "О дате готовности вы будете уведомлены  \n" +
                   "по указанному телефону.                 \n" +
                   "                                        \n" +
                   "Спасибо за ваш предзаказ!               \n" +
                   "==========================================\n";
        }

        private void ReceiptForm_Load(object sender, EventArgs e)
        {

        }
    }
}