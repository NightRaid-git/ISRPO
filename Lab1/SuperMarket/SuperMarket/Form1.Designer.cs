namespace Лабораторная_1
{
    partial class Spisokproductov
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.buttonTotal = new System.Windows.Forms.Button();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.comboBoxProducts = new System.Windows.Forms.ComboBox();
            this.listBoxSelected = new System.Windows.Forms.ListBox();
            this.buttonClear = new System.Windows.Forms.Button();
            this.textBoxTotal = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(125, 92);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(230, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Выберите продукты для покупки из списка:";
            // 
            // buttonTotal
            // 
            this.buttonTotal.Location = new System.Drawing.Point(32, 256);
            this.buttonTotal.Name = "buttonTotal";
            this.buttonTotal.Size = new System.Drawing.Size(114, 82);
            this.buttonTotal.TabIndex = 1;
            this.buttonTotal.Text = "Посчитать итог";
            this.buttonTotal.UseVisualStyleBackColor = true;
            // 
            // buttonAdd
            // 
            this.buttonAdd.Location = new System.Drawing.Point(152, 158);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(158, 62);
            this.buttonAdd.TabIndex = 2;
            this.buttonAdd.Text = "Добавить";
            this.buttonAdd.UseVisualStyleBackColor = false;
            // 
            // comboBoxProducts
            // 
            this.comboBoxProducts.FormattingEnabled = true;
            this.comboBoxProducts.Location = new System.Drawing.Point(152, 121);
            this.comboBoxProducts.Name = "comboBoxProducts";
            this.comboBoxProducts.Size = new System.Drawing.Size(158, 21);
            this.comboBoxProducts.TabIndex = 3;
            // 
            // listBoxSelected
            // 
            this.listBoxSelected.FormattingEnabled = true;
            this.listBoxSelected.Location = new System.Drawing.Point(152, 256);
            this.listBoxSelected.Name = "listBoxSelected";
            this.listBoxSelected.Size = new System.Drawing.Size(158, 82);
            this.listBoxSelected.TabIndex = 4;
            // 
            // buttonClear
            // 
            this.buttonClear.Location = new System.Drawing.Point(316, 256);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(115, 82);
            this.buttonClear.TabIndex = 5;
            this.buttonClear.Text = "Очистить";
            this.buttonClear.UseVisualStyleBackColor = true;
            // 
            // textBoxTotal
            // 
            this.textBoxTotal.Location = new System.Drawing.Point(258, 379);
            this.textBoxTotal.Name = "textBoxTotal";
            this.textBoxTotal.Size = new System.Drawing.Size(97, 20);
            this.textBoxTotal.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(100, 382);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(152, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Итоговая сумма за покупку:";
            // 
            // Spisokproductov
            // 
            this.ClientSize = new System.Drawing.Size(493, 453);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxTotal);
            this.Controls.Add(this.buttonClear);
            this.Controls.Add(this.listBoxSelected);
            this.Controls.Add(this.comboBoxProducts);
            this.Controls.Add(this.buttonAdd);
            this.Controls.Add(this.buttonTotal);
            this.Controls.Add(this.label1);
            this.Name = "Spisokproductov";
            this.Text = "SpisokProducts";
            this.Load += new System.EventHandler(this.Spisokproductov_Load_1);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonTotal;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.ComboBox comboBoxProducts;
        private System.Windows.Forms.ListBox listBoxSelected;
        private System.Windows.Forms.Button buttonClear;
        private System.Windows.Forms.TextBox textBoxTotal;
        private System.Windows.Forms.Label label2;
    }
}

