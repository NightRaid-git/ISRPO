using System;
using System.Windows.Forms;

namespace caculator
{
    public partial class Form1 : Form
    {
        private double firstNumber = 0;
        private double secondNumber = 0;
        private string operation = "";
        private bool isNewOperation = true;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtDisplay.Text = "0";
        }
        private void btnNumber_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            if (txtDisplay.Text == "0" || isNewOperation)
            {
                txtDisplay.Text = button.Text;
                isNewOperation = false;
            }
            else
            {
                txtDisplay.Text += button.Text;
            }
        }
        private void btnDecimal_Click(object sender, EventArgs e)
        {
            if (!txtDisplay.Text.Contains(","))
            {
                txtDisplay.Text += ",";
                isNewOperation = false;
            }
        }
        private void btnOperation_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            firstNumber = Convert.ToDouble(txtDisplay.Text);
            operation = button.Text;
            isNewOperation = true;
        }
        private void btnEquals_Click(object sender, EventArgs e)
        {
            if (!isNewOperation)
            {
                secondNumber = Convert.ToDouble(txtDisplay.Text);

                double result = 0;
                switch (operation)
                {
                    case "+":
                        result = firstNumber + secondNumber;
                        break;
                    case "-":
                        result = firstNumber - secondNumber;
                        break;
                    case "×":
                        result = firstNumber * secondNumber;
                        break;
                    case "÷":
                        if (secondNumber != 0)
                            result = firstNumber / secondNumber;
                        else
                        {
                            MessageBox.Show("Деление на ноль невозможно!", "Ошибка",
                                          MessageBoxButtons.OK, MessageBoxIcon.Error);
                            ClearCalculator();
                            return;
                        }
                        break;
                }
                txtDisplay.Text = result.ToString();
                isNewOperation = true;
            }
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearCalculator();
        }
        private void btnBackspace_Click(object sender, EventArgs e)
        {
            if (txtDisplay.Text.Length > 1)
            {
                txtDisplay.Text = txtDisplay.Text.Substring(0, txtDisplay.Text.Length - 1);
            }
            else
            {
                txtDisplay.Text = "0";
                isNewOperation = true;
            }
        }
        private void btnPlusMinus_Click(object sender, EventArgs e)
        {
            if (txtDisplay.Text != "0")
            {
                if (txtDisplay.Text.StartsWith("-"))
                {
                    txtDisplay.Text = txtDisplay.Text.Substring(1);
                }
                else
                {
                    txtDisplay.Text = "-" + txtDisplay.Text;
                }
            }
        }
        private void ClearCalculator()
        {
            txtDisplay.Text = "0";
            firstNumber = 0;
            secondNumber = 0;
            operation = "";
            isNewOperation = true;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}