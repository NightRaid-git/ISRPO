using System;
using System.Linq;
using System.Windows.Forms;

namespace Shifr
{
    public partial class Shifr : Form
    {
        private readonly string rusLower = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";
        private readonly string rusUpper = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";

        private readonly string engLower = "abcdefghijklmnopqrstuvwxyz";
        private readonly string engUpper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public Shifr()
        {
            InitializeComponent();
            InitUI();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void InitUI()
        {
            comboLanguage.Items.Add("Русский");
            comboLanguage.Items.Add("English");
            comboLanguage.SelectedIndex = 0;

            for (int i = 1; i <= 10; i++)
                comboShift.Items.Add(i);
            comboShift.SelectedItem = 3;
        }

        private void ProcessText(bool encrypt)
        {
            if (string.IsNullOrWhiteSpace(textInput.Text))
            {
                MessageBox.Show("Введите текст!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (comboShift.SelectedItem == null)
            {
                MessageBox.Show("Выберите сдвиг!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int shift = (int)comboShift.SelectedItem;
            if (!encrypt) shift = -shift;

            string result = Caesar(textInput.Text, shift, comboLanguage.SelectedIndex == 0);
            textOutput.Text = result;
        }

        private string Caesar(string text, int shift, bool russian)
        {
            string lower = russian ? rusLower : engLower;
            string upper = russian ? rusUpper : engUpper;

            int lowerLen = lower.Length;
            int upperLen = upper.Length;

            string result = "";

            foreach (char c in text)
            {
                if (lower.Contains(c))
                {
                    int idx = (lower.IndexOf(c) + shift) % lowerLen;
                    if (idx < 0) idx += lowerLen;
                    result += lower[idx];
                }
                else if (upper.Contains(c))
                {
                    int idx = (upper.IndexOf(c) + shift) % upperLen;
                    if (idx < 0) idx += upperLen;
                    result += upper[idx];
                }
                else
                {
                    result += c;
                }
            }

            return result;
        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            ProcessText(encrypt: true);
        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            ProcessText(encrypt: false);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            textInput.Clear();
            textOutput.Clear();
        }
    }
}
