using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace example
{
    public partial class Form4 : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename=|DataDirectory|\Database1.mdf;Integrated Security = True");
        public Form4()
        {
            con.Open();
            InitializeComponent();
            string query = "SELECT Id_product,Категория,Наименование, Фирма, [Страна-изготовитель],[Оптовая цена],[Кол-во в наличии],[Минимальный пороговый запас] FROM Товары where [Кол-во в наличии]<=[Минимальный пороговый запас]";
            SqlCommand command = new SqlCommand(query, con);
            SqlDataReader reader = command.ExecuteReader();
            List<string[]> data = new List<string[]>();
            while (reader.Read())
            {
                data.Add(new string[8]);

                data[data.Count - 1][0] = reader[0].ToString();
                data[data.Count - 1][2] = reader[1].ToString();
                data[data.Count - 1][1] = reader[2].ToString();
                data[data.Count - 1][3] = reader[3].ToString();
                data[data.Count - 1][4] = reader[4].ToString();
                data[data.Count - 1][5] = reader[5].ToString();
                data[data.Count - 1][6] = reader[6].ToString();
                data[data.Count - 1][7] = reader[7].ToString();
            }
            reader.Close();
            foreach (string[] s in data)
                dataGridView1.Rows.Add(s);
            con.Close();
        }


        private void button1_MouseDown(object sender, MouseEventArgs e)
        {
            Hide();
            Form1 form1 = new Form1();
            form1.Show();
            Close();
        }

        private void Form4_Load(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (textBox3.Text != "")
            {
                int number = (int)numericUpDown1.Value;
                string price = textBox4.Text;
                decimal real_price = Convert.ToDecimal(price);
                real_price *= number;
                textBox2.Text = Convert.ToString(real_price);
            }
        }

        private void button2_MouseDown(object sender, MouseEventArgs e)
        {
            if (textBox1.Text == "") MessageBox.Show("Невозможно выполнить заказ, товар не был выбран!");
            else if (numericUpDown1.Value == 0) MessageBox.Show("Невозможно выполнить заказ, было выбрано невозможное число закупаемого товара!");
            else
            {
                SqlConnection con = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename=|DataDirectory|\Database1.mdf;Integrated Security = True");
                con.Open();
                SqlCommand command = new SqlCommand("insert Покупки (Id_product, Name, Number,Data) VALUES (@Id_product,@Name,@Number,@Data)", con);
                command.Parameters.AddWithValue("@Id_product", dataGridView1.CurrentRow.Cells[0].Value.ToString());
                command.Parameters.AddWithValue("@Name", dataGridView1.CurrentRow.Cells[1].Value.ToString());
                command.Parameters.AddWithValue("@Number", (int)numericUpDown1.Value);
                command.Parameters.AddWithValue("@Data", DateTime.Now);
                command.ExecuteNonQuery();
                int index = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value.ToString());
                SqlCommand command1 = new SqlCommand($"select [Кол-во в наличии], [Оптовая цена],Прибыль from Товары where Id_product={index}", con);
                SqlDataReader reader = command1.ExecuteReader();
                reader.Read();
                string number_on_storage = reader[0].ToString();
                string price = reader[1].ToString();
                string stonks = reader[2].ToString();
                reader.Close();
                int real_number_on_storage = Convert.ToInt32(number_on_storage);
                decimal real_price = Convert.ToDecimal(price);
                decimal real_stonks = Convert.ToDecimal(stonks);
                real_number_on_storage += (int)numericUpDown1.Value;
                real_stonks = real_stonks - (int)numericUpDown1.Value * real_price;
                SqlCommand command2 = new SqlCommand("update Товары set [Кол-во в наличии]=@Number, Прибыль=@stonks where Id_product=@index", con);
                command2.Parameters.AddWithValue("@Number", real_number_on_storage);
                command2.Parameters.AddWithValue("@stonks", real_stonks);
                command2.Parameters.AddWithValue("@index", index);
                command2.ExecuteNonQuery();

                this.dataGridView1.Rows.Clear();
                string query = "SELECT Id_product,Категория,Наименование, Фирма, [Страна-изготовитель],[Оптовая цена],[Кол-во в наличии],[Минимальный пороговый запас] FROM Товары where [Кол-во в наличии]<=[Минимальный пороговый запас]";
                SqlCommand command3 = new SqlCommand(query, con);
                SqlDataReader reader1 = command3.ExecuteReader();
                List<string[]> data = new List<string[]>();
                while (reader1.Read())
                {
                    data.Add(new string[8]);

                    data[data.Count - 1][0] = reader1[0].ToString();
                    data[data.Count - 1][2] = reader1[1].ToString();
                    data[data.Count - 1][1] = reader1[2].ToString();
                    data[data.Count - 1][3] = reader1[3].ToString();
                    data[data.Count - 1][4] = reader1[4].ToString();
                    data[data.Count - 1][5] = reader1[5].ToString();
                    data[data.Count - 1][6] = reader1[6].ToString();
                    data[data.Count - 1][7] = reader1[7].ToString();
                }
                reader1.Close();
                foreach (string[] s in data)
                    dataGridView1.Rows.Add(s);
                textBox1.Text = "";
                textBox1.Text = dataGridView1.CurrentRow.Cells[6].Value.ToString();
                con.Close();
            }
        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            con.Close();
            con.Open();
            textBox1.Text = dataGridView1.CurrentRow.Cells[6].Value.ToString();
            textBox3.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            textBox2.Text = "0";
            textBox4.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
            con.Close();
        }
    }
}
