using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace example
{
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
        }

        private void Form5_Load(object sender, EventArgs e)
        {

        }

        private void button1_MouseDown(object sender, MouseEventArgs e)
        {
            Hide();
            Form1 form1 = new Form1();
            form1.Show();
            Close();
        }

        private void button5_MouseDown(object sender, MouseEventArgs e)
        {
            this.Hide();
            Form6 form6 = new Form6();
            form6.Show();
            this.Close();
        }

        private void button6_MouseDown(object sender, MouseEventArgs e)
        {
            this.Hide();
            Form7 form7 = new Form7();
            form7.Show();
            this.Close();
        }

        private void button4_MouseDown(object sender, MouseEventArgs e)
        {
            this.Hide();
            Form8 form8 = new Form8();
            form8.Show();
            this.Close();
        }

        private void button2_MouseDown(object sender, MouseEventArgs e)
        {
            if (Convert.ToString(dateTimePicker1.Value) == Convert.ToString(dateTimePicker2.Value))
            {
                MessageBox.Show("Даты не могут быть одинаковыми!");
            }
            else if (Convert.ToDateTime(dateTimePicker1.Value) > Convert.ToDateTime(dateTimePicker2.Value))
            {
                MessageBox.Show("Начала отчета не может быть позже конца!");
            }
            else
            {
                this.Hide();
                Form9 form9 = new Form9(Convert.ToString(dateTimePicker1.Value), Convert.ToString(dateTimePicker2.Value));
                form9.Show();
                this.Close();
            }
        }

        private void button3_MouseDown(object sender, MouseEventArgs e)
        {
            if (Convert.ToString(dateTimePicker1.Value) == Convert.ToString(dateTimePicker2.Value))
            {
                MessageBox.Show("Даты не могут быть одинаковыми!");
            }
            else if (Convert.ToDateTime(dateTimePicker1.Value) > Convert.ToDateTime(dateTimePicker2.Value))
            {
                MessageBox.Show("Начала отчета не может быть позже конца!");
            }
            else
            {
                this.Hide();
                Form10 form10 = new Form10(Convert.ToString(dateTimePicker1.Value), Convert.ToString(dateTimePicker2.Value));
                form10.Show();
                this.Close();
            }
        }
    }
}
