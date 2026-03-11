using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SkladApp
{
    public partial class Sklad : Form
    {
        public Sklad()
        {
            InitializeComponent();
            txtId.ReadOnly = true;
            LoadProducts();
            if (dgvProducts.Columns["Name"] != null) dgvProducts.Columns["Name"].HeaderText = "Название";
            if (dgvProducts.Columns["Stillage"] != null) dgvProducts.Columns["Stillage"].HeaderText = "Стеллаж";
            if (dgvProducts.Columns["Cell"] != null) dgvProducts.Columns["Cell"].HeaderText = "Ячейка";
            if (dgvProducts.Columns["Quantity"] != null) dgvProducts.Columns["Quantity"].HeaderText = "Количество";
        }

        private void LoadProducts()
        {
            try
            {
                dgvProducts.DataSource = DbHelper.GetAllProducts();
                dgvProducts.Columns["Id"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message);
            }
        }

        // Кнопка Добавить (btnAdd) — использует поля Name и NumericUpDown'ы для чисел
        private void btnAdd_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Введите название продукта");
                return;
            }

            int s = Convert.ToInt32(numStillage.Value);
            int c = Convert.ToInt32(numCell.Value);
            int q = Convert.ToInt32(numQuantity.Value);

            var p = new Product
            {
                Name = name,
                Stillage = s,
                Cell = c,
                Quantity = q
            };

            try
            {
                int newId = DbHelper.InsertProduct(p);
                p.Id = newId;
                txtId.Text = newId.ToString();
                LoadProducts();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка добавления: " + ex.Message);
            }
        }

        // Кнопка Обновить (btnUpdate) — у вас в проекте метод назывался btnAdd_Click_1, 
        // сохраняю подписку на тот же обработчик, но логика — обновление.
        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            // Обработка обновления — если в дизайнере это кнопка обновления, оставьте имя события
            if (!int.TryParse(txtId.Text, out int id))
            {
                MessageBox.Show("Выберите продукт для обновления");
                return;
            }

            string name = txtName.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Введите название продукта");
                return;
            }

            int s = Convert.ToInt32(numStillage.Value);
            int c = Convert.ToInt32(numCell.Value);
            int q = Convert.ToInt32(numQuantity.Value);

            var p = new Product
            {
                Id = id,
                Name = name,
                Stillage = s,
                Cell = c,
                Quantity = q
            };

            try
            {
                DbHelper.UpdateProduct(p);
                LoadProducts();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка обновления: " + ex.Message);
            }
        }

        // Удаление
        private void btnDelete_Click_1(object sender, EventArgs e)
        {
            if (!int.TryParse(txtId.Text, out int id))
            {
                MessageBox.Show("Выберите продукт для удаления");
                return;
            }

            var confirm = MessageBox.Show($"Удалить продукто?", "Подтвердите", MessageBoxButtons.YesNo);
            if (confirm != DialogResult.Yes) return;

            try
            {
                DbHelper.DeleteProduct(id);
                ClearInputFields();
                LoadProducts();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка удаления: " + ex.Message);
            }
        }

        // При выборе строки в таблице заполняем поля
        private void dgvProducts_SelectionChanged_1(object sender, EventArgs e)
        {
            if (dgvProducts.CurrentRow == null) return;
            try
            {
                var row = dgvProducts.CurrentRow;
                // Некоторые источники данных могут использовать имена колонок с разным регистром — безопасно ищем по индексам, если нужно.
                if (row.Cells["Id"] != null)
                    txtId.Text = row.Cells["Id"].Value?.ToString() ?? "";
                else
                    txtId.Text = "";

                txtName.Text = row.Cells["Name"].Value?.ToString() ?? "";

                // Если в гриде числа представлены как объекты, безопасно конвертируем
                string sVal = row.Cells["Stillage"].Value?.ToString() ?? "0";
                string cVal = row.Cells["Cell"].Value?.ToString() ?? "0";
                string qVal = row.Cells["Quantity"].Value?.ToString() ?? "0";

                if (int.TryParse(sVal, out int s)) numStillage.Value = Math.Max(numStillage.Minimum, Math.Min(numStillage.Maximum, s));
                if (int.TryParse(cVal, out int c)) numCell.Value = Math.Max(numCell.Minimum, Math.Min(numCell.Maximum, c));
                if (int.TryParse(qVal, out int q)) numQuantity.Value = Math.Max(numQuantity.Minimum, Math.Min(numQuantity.Maximum, q));
            }
            catch
            {
                // Игнорируем ошибки привязки ячеек
            }
        }

        // Поиск по имени
        private void btnSearchName_Click(object sender, EventArgs e)
        {
            try
            {
                var res = DbHelper.SearchByName(txtSearchName.Text.Trim());
                dgvProducts.DataSource = res;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка поиска: " + ex.Message);
            }
        }

        // Поиск по координатам
        private void btnSearchCoords_Click(object sender, EventArgs e)
        {
            int s = Convert.ToInt32(numSearchStillage.Value);
            int c = Convert.ToInt32(numSearchCell.Value);

            try
            {
                var res = DbHelper.SearchByCoords(s, c);
                dgvProducts.DataSource = res;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка поиска: " + ex.Message);
            }
        }

        // Сохранение в CSV
        private void btnSaveToFile_Click(object sender, EventArgs e)
        {
            try
            {
                var list = DbHelper.GetAllProducts();
                var lines = new List<string> { "Id;Name;Stillage;Cell;Quantity" };
                lines.AddRange(list.ConvertAll(p => $"{p.Id};{p.Name};{p.Stillage};{p.Cell};{p.Quantity}"));
                using (var sfd = new SaveFileDialog { Filter = "CSV|*.csv", FileName = "products_export.csv" })
                {
                    if (sfd.ShowDialog() != DialogResult.OK) return;
                    System.IO.File.WriteAllLines(sfd.FileName, lines, System.Text.Encoding.UTF8);
                    MessageBox.Show("Сохранено в " + sfd.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка сохранения: " + ex.Message);
            }
        }

        private void ClearInputFields()
        {
            txtId.Text = "";
            txtName.Text = "";
            numStillage.Value = numStillage.Minimum;
            numCell.Value = numCell.Minimum;
            numQuantity.Value = numQuantity.Minimum;
        }

        private void Sklad_Load(object sender, EventArgs e)
        {

        }
    }
}
