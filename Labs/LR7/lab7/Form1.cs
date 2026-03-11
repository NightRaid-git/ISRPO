using System;
using System.Windows.Forms;

namespace lab7
{
    public partial class Form1 : Form
    {
        private DatabaseHelper dbHelper;
        private Book selectedBook;

        public Form1()
        {
            InitializeComponent();
            InitializeDatabaseConnection();
            SetupEventHandlers();
            LoadBooksToDataGrid();
        }

        private void InitializeDatabaseConnection()
        {
            dbHelper = new DatabaseHelper();
        }

        private void SetupEventHandlers()
        {
            dataGridView.SelectionChanged += DataGridView_SelectionChanged;

            if (order != null)
                order.Click += Order_Click;

            if (Exit != null)
                Exit.Click += Exit_Click;
        }

        private void LoadBooksToDataGrid()
        {
            try
            {
                dataGridView.Rows.Clear();
                dataGridView.Columns.Clear();

                dataGridView.Columns.Add("Id", "ID");
                dataGridView.Columns.Add("Name", "Название");
                dataGridView.Columns.Add("Author", "Автор");
                dataGridView.Columns.Add("Year", "Год");
                dataGridView.Columns.Add("Pages", "Страниц");
                dataGridView.Columns.Add("Circulation", "Тираж");

                dataGridView.Columns["Id"].Visible = false;

                dataGridView.ReadOnly = true;
                dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridView.RowHeadersVisible = false;
                dataGridView.AllowUserToAddRows = false;
                dataGridView.AllowUserToDeleteRows = false;

                var books = dbHelper.GetPublications();

                if (books.Count == 0)
                {
                    MessageBox.Show("В базе данных нет книг!", "Информация",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                foreach (var book in books)
                {
                    dataGridView.Rows.Add(
                        book.Id,
                        book.Name,
                        book.AuthorName,
                        book.ReleaseYear,
                        book.VolumeOfSheets,
                        book.Circulation
                    );
                }

                dataGridView.Columns["Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView.Columns["Author"].Width = 150;
                dataGridView.Columns["Year"].Width = 60;
                dataGridView.Columns["Pages"].Width = 70;
                dataGridView.Columns["Circulation"].Width = 70;

                if (dataGridView.Rows.Count > 0)
                {
                    dataGridView.Rows[0].Selected = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке книг: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count > 0)
            {
                try
                {
                    var selectedRow = dataGridView.SelectedRows[0];

                    int bookId = Convert.ToInt32(selectedRow.Cells["Id"].Value);
                    string bookName = selectedRow.Cells["Name"].Value?.ToString() ?? "";
                    string author = selectedRow.Cells["Author"].Value?.ToString() ?? "";
                    int year = Convert.ToInt32(selectedRow.Cells["Year"].Value);
                    int pages = Convert.ToInt32(selectedRow.Cells["Pages"].Value);
                    int circulation = Convert.ToInt32(selectedRow.Cells["Circulation"].Value);

                    if (name != null)
                        name.Text = bookName;

                    if (avtor != null)
                        avtor.Text = author;

                    if (yearTextBox != null)
                        yearTextBox.Text = year.ToString();

                    selectedBook = new Book
                    {
                        Id = bookId,
                        Name = bookName,
                        AuthorName = author,
                        ReleaseYear = year,
                        VolumeOfSheets = pages,
                        Circulation = circulation
                    };


                    if (order != null)
                        order.Enabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при выборе книги: {ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    if (order != null)
                        order.Enabled = false;
                }
            }
            else
            {
                if (order != null)
                    order.Enabled = false;
            }
        }

        private void Order_Click(object sender, EventArgs e)
        {
            if (selectedBook == null)
            {
                MessageBox.Show("Сначала выберите книгу из списка!", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                FormOrder formOrder = new FormOrder(selectedBook.Id, dbHelper);
                formOrder.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при открытии формы заказа: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Вы уверены, что хотите выйти?", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}