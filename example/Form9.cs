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
    public partial class Form9 : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename=|DataDirectory|\Database1.mdf;Integrated Security = True");
        public Form9(string behind, string after)
        {
            InitializeComponent();
            dateTimePicker1.Value =Convert.ToDateTime(behind);
            dateTimePicker2.Value = Convert.ToDateTime(after);
            con.Open();
            SqlCommand command = new SqlCommand("select Id_product,Наименование,Фирма,[Страна-изготовитель],[Розничная цена],[Оптовая цена] from Товары", con);
            SqlDataReader reader = command.ExecuteReader();
            List<string[]> data = new List<string[]>();
            while (reader.Read())
            {
                data.Add(new string[11]);
                data[data.Count - 1][0] = reader[0].ToString();
                data[data.Count - 1][1] = reader[1].ToString();
                data[data.Count - 1][2] = reader[2].ToString();
                data[data.Count - 1][3] = reader[3].ToString();
                data[data.Count - 1][4] = reader[4].ToString();
                data[data.Count - 1][5] = null;
                data[data.Count - 1][6] = null;
                data[data.Count - 1][7] = reader[5].ToString();
                data[data.Count - 1][8] = null;
                data[data.Count - 1][9] = null;
                data[data.Count - 1][10] = null;
            }
            reader.Close();
            int j = data.Count;
            for (int i = 0; i < j; i++)
            {
                int number = 0;
                SqlCommand command1 = new SqlCommand("select Number from Продажи where (Id_product=@id) and (Data BETWEEN @data1 AND @data2)", con);
                command1.Parameters.AddWithValue("@data1", Convert.ToDateTime(behind));
                command1.Parameters.AddWithValue("@data2", Convert.ToDateTime(after));
                command1.Parameters.AddWithValue("@id", Convert.ToInt32(data[i][0]));
                SqlDataReader reader1 = command1.ExecuteReader();
                while (reader1.Read())
                    number += Convert.ToInt32(reader1[0].ToString());
                reader1.Close();
                data[i][5] = Convert.ToString(number);
                decimal price1 = Convert.ToDecimal(data[i][4]) * number;
                data[i][6] = Convert.ToString(price1);
            }
            for (int i = 0; i < j; i++)
            {
                int number1 = 0;
                SqlCommand command2 = new SqlCommand("select Number from Покупки where (Id_product=@id) and (Data BETWEEN @data1 AND @data2)", con);
                command2.Parameters.AddWithValue("@data1", Convert.ToDateTime(behind));
                command2.Parameters.AddWithValue("@data2", Convert.ToDateTime(after));
                command2.Parameters.AddWithValue("@id", Convert.ToInt32(data[i][0]));
                SqlDataReader reader2 = command2.ExecuteReader();
                while (reader2.Read())
                    number1 += Convert.ToInt32(reader2[0].ToString());
                reader2.Close();
                data[i][8] = Convert.ToString(number1);
                decimal price2 = Convert.ToDecimal(data[i][7]) * number1;
                data[i][9] = Convert.ToString(price2);
            }
            for (int i = 0; i < j; i++)
                data[i][10] = Convert.ToString(Convert.ToDecimal(data[i][6]) - Convert.ToDecimal(data[i][9]));
            foreach (string[] s in data)
                dataGridView1.Rows.Add(s);
            con.Close();
        }

        private void Form9_Load(object sender, EventArgs e)
        {

        }

        private void button1_MouseDown(object sender, MouseEventArgs e)
        {
            this.Hide();
            Form5 form5 = new Form5();
            form5.Show();
            this.Close();
        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            con.Open();
            this.dataGridView2.Rows.Clear();
            this.dataGridView3.Rows.Clear();
            int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value.ToString());
            SqlCommand command = new SqlCommand("select [Розничная цена] from Товары where Id_product=@id", con);
            command.Parameters.AddWithValue("@id", id);
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            decimal value = Convert.ToDecimal(reader[0].ToString());
            reader.Close();
            SqlCommand command1 = new SqlCommand("select Number,Data from Продажи where (Id_product=@id) and (Data BETWEEN @data1 AND @data2)", con);
            command1.Parameters.AddWithValue("@data1", Convert.ToDateTime(dateTimePicker1.Value));
            command1.Parameters.AddWithValue("@data2", Convert.ToDateTime(dateTimePicker2.Value));
            command1.Parameters.AddWithValue("@id", id);
            SqlDataReader reader1 = command1.ExecuteReader();
            List<string[]> data = new List<string[]>();
            while (reader1.Read())
            {
                int number = Convert.ToInt32(reader1[0].ToString());
                string date = reader1[1].ToString();
                data.Add(new string[3]);

                data[data.Count - 1][0] = Convert.ToString(number);
                data[data.Count - 1][1] = Convert.ToString(number * value);
                data[data.Count - 1][2] = date;
            }
            reader1.Close();
            foreach (string[] s in data)
                dataGridView2.Rows.Add(s);

            SqlCommand command2 = new SqlCommand("select [Оптовая цена] from Товары where Id_product=@id", con);
            command2.Parameters.AddWithValue("@id", id);
            SqlDataReader reader2 = command2.ExecuteReader();
            reader2.Read();
            decimal value2 = Convert.ToDecimal(reader2[0].ToString());
            reader2.Close();
            SqlCommand command3 = new SqlCommand("select Number,Data from Покупки where (Id_product=@id) and (Data BETWEEN @data1 AND @data2)", con);
            command3.Parameters.AddWithValue("@data1", Convert.ToDateTime(dateTimePicker1.Value));
            command3.Parameters.AddWithValue("@data2", Convert.ToDateTime(dateTimePicker2.Value));
            command3.Parameters.AddWithValue("@id", id);
            SqlDataReader reader3 = command3.ExecuteReader();
            List<string[]> data1 = new List<string[]>();
            while (reader3.Read())
            {
                int number = Convert.ToInt32(reader3[0].ToString());
                string date = reader3[1].ToString();
                data1.Add(new string[3]);

                data1[data1.Count - 1][0] = Convert.ToString(number);
                data1[data1.Count - 1][1] = Convert.ToString(number * value2);
                data1[data1.Count - 1][2] = date;
            }
            reader3.Close();
            foreach (string[] s in data1)
                dataGridView3.Rows.Add(s);

            con.Close();
        }
    }
}
