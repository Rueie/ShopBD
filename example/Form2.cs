using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace example
{
    public partial class Form2 : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename=|DataDirectory|\Database1.mdf;Integrated Security = True");
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_MouseDown(object sender, MouseEventArgs e)
        {
            Hide();
            Form1 form1 = new Form1();
            form1.Show();
            Close();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            con.Close();
            // TODO: данная строка кода позволяет загрузить данные в таблицу "database1DataSet.Товары". При необходимости она может быть перемещена или удалена.
            this.товарыTableAdapter.Fill(this.database1DataSet.Товары);
            string query = "select distinct Категория from Товары";
            SqlDataAdapter da = new SqlDataAdapter(query, con);
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.CommandText = query;
            con.Open();
            DataSet ds = new DataSet();
            da.Fill(ds, "Товары");
            comboBox1.DisplayMember = "Категория";
            comboBox1.DataSource = ds.Tables["Товары"];
            con.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            con.Close();
            con.Open();
            this.dataGridView1.Rows.Clear();
            string g = comboBox1.Text;
            string query = $"SELECT Id_product, Наименование, Фирма, [Страна-изготовитель],[Розничная цена] FROM Товары where Категория='{g}'";
            SqlCommand command = new SqlCommand(query, con);
            SqlDataReader reader = command.ExecuteReader();
            List<string[]> data = new List<string[]>();
            while (reader.Read())
            {
                data.Add(new string[5]);

                data[data.Count - 1][0] = reader[0].ToString();
                data[data.Count - 1][1] = reader[1].ToString();
                data[data.Count - 1][2] = reader[2].ToString();
                data[data.Count - 1][3] = reader[3].ToString();
                data[data.Count - 1][4] = reader[4].ToString();
            }
            reader.Close();
            foreach (string[] s in data)
                dataGridView1.Rows.Add(s);
            con.Close();
        }

        private void button2_MouseDown(object sender, MouseEventArgs e)
        {
            string number = textBox5.Text;
            if (number == "") MessageBox.Show("Невозможно перейти к оформлению заказа, т.к. не был выбран товар!");
            else
            {
                int x = Convert.ToInt32(textBox5.Text);
                int y = (int)numericUpDown1.Value;
                if (x < y) MessageBox.Show("Невозможно перейти к оформлению заказа, т.к. было выбрано товара больше, чем его есть на складе!");
                else if (y == 0) MessageBox.Show("Невозможно перейти к оформлению заказа, т.к. было выбрано невозможное число товаров!");
                else
                {
                    this.Hide();
                    string index = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                    int real_index = int.Parse(index);
                    Form3 form3 = new Form3(real_index, textBox1.Text,textBox2.Text, textBox3.Text, textBox4.Text, (int)numericUpDown1.Value);
                    form3.Show();
                    this.Close();
                }
            }
        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            con.Close();
            con.Open();
            textBox1.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            textBox2.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            textBox3.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            textBox4.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            string index = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            SqlCommand thisCommand = con.CreateCommand();
            thisCommand.CommandText = $"select [Кол-во в наличии] FROM Товары where Id_product = {index} ";
            SqlDataReader thisReader = thisCommand.ExecuteReader();
            string res = string.Empty;
            while (thisReader.Read())
            {
                res += thisReader["Кол-во в наличии"];
            }
            thisReader.Close();
            textBox5.Text = res;
            con.Close();
        }
    }
}
