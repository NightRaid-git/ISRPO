using System;
using System.Drawing;
using System.Windows.Forms;
namespace lab3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            int[] ticket = new int[6];
            Random random = new Random();
            for (int i = 0; i < ticket.Length; i++)
            {
                ticket[i] = random.Next(0, 9);
            }
            label4.Text = string.Join("", ticket);
            if (ticket[0] + ticket[1] + ticket[2] == ticket[3] + ticket[4] + ticket[5])
            {
                label4.ForeColor = Color.Green;
                label3.Text = "Счастливый билет";
                label3.ForeColor = Color.Green;
            }
            else
            {
                label4.ForeColor = Color.Red;
                label3.Text = "Обычный билет";
                label3.ForeColor = Color.Red;
            }
        }
    }
}
