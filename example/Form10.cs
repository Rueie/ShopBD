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
    public partial class Form10 : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename=|DataDirectory|\Database1.mdf;Integrated Security = True");

        public Form10(string behind, string after)
        {
            InitializeComponent();
            con.Open();
            dateTimePicker1.Value = Convert.ToDateTime(behind);
            dateTimePicker2.Value = Convert.ToDateTime(after);
            SqlCommand command = new SqlCommand("select distinct Категория from Товары", con);
            SqlDataReader reader = command.ExecuteReader();
            List<string> kat = new List<string>();
            int k = 0;
            while (reader.Read())
            {
                k++;
                kat.Add(reader[0].ToString());
            }
            reader.Close();
            //получили категории
            List<List<int>> number_of_index = new List<List<int>>();
            for (int i = 0; i < k; i++) {
                List<int> index1 = new List<int>();
                SqlCommand command1 = new SqlCommand("select Id_product from Товары where Категория =@kat", con);
                command1.Parameters.AddWithValue("@kat",kat[i]);
                SqlDataReader reader1 = command1.ExecuteReader();
                while (reader1.Read())
                    index1.Add(Convert.ToInt32(reader1[0].ToString()));
                reader1.Close();
                number_of_index.Add(index1);
            }
            //получили индексы товаров для каждой категории
            List<string[]> data = new List<string[]>();
            for (int i = 0; i < k; i++)
            {
                List<int> index = new List<int>();
                //категория !есть!
                index = number_of_index[i];
                int number1 = 0;//общее число купленного товара
                decimal value1=0;//общая цена
                for (int j = 0; j < index.Count; j++)
                {
                    int number2 = 0;
                    SqlCommand command1 = new SqlCommand("select [Розничная цена] from Товары where Id_product=@id", con);
                    command1.Parameters.AddWithValue("@id", index[j]);
                    SqlDataReader reader1 = command1.ExecuteReader();
                    reader1.Read();
                    decimal value2 = Convert.ToDecimal(reader1[0].ToString());//цена товара
                    reader1.Close();
                    SqlCommand command2 = new SqlCommand("select Number from Продажи where (Id_product=@idd) and (Data BETWEEN @data1 AND @data2)", con);
                    command2.Parameters.AddWithValue("@data1", Convert.ToDateTime(behind));
                    command2.Parameters.AddWithValue("@data2", Convert.ToDateTime(after));
                    command2.Parameters.AddWithValue("@idd", index[j]);
                    SqlDataReader reader2 = command2.ExecuteReader();
                    while (reader2.Read())
                        number2 += Convert.ToInt32(reader2[0].ToString());
                    reader2.Close();
                    value2 *= number2;
                    number1 += number2;
                    value1 += value2;
                }
                //число проданного !есть!
                //сумма проданного !есть!
                int number3 = 0;//общее число купленного товара
                decimal value3 = 0;//общая цена
                for (int j = 0; j < index.Count; j++)
                {
                    int number4 = 0;
                    SqlCommand command1 = new SqlCommand("select [Оптовая цена] from Товары where Id_product=@id", con);
                    command1.Parameters.AddWithValue("@id", index[j]);
                    SqlDataReader reader1 = command1.ExecuteReader();
                    reader1.Read();
                    decimal value4 = Convert.ToDecimal(reader1[0].ToString());//цена товара
                    reader1.Close();
                    SqlCommand command2 = new SqlCommand("select Number from Покупки where (Id_product=@idd) and (Data BETWEEN @data1 AND @data2)", con);
                    command2.Parameters.AddWithValue("@data1", Convert.ToDateTime(behind));
                    command2.Parameters.AddWithValue("@data2", Convert.ToDateTime(after));
                    command2.Parameters.AddWithValue("@idd", index[j]);
                    SqlDataReader reader2 = command2.ExecuteReader();
                    while (reader2.Read())
                        number4 += Convert.ToInt32(reader2[0].ToString());
                    reader2.Close();
                    value4 *= number4;
                    number3 += number4;
                    value3 += value4;
                }
                //число купленного !есть!
                //сумма купленного !есть!
                decimal value5 = value1 - value3;
                //доход !есть!
                data.Add(new string[6]);
                data[data.Count - 1][0] = kat[i];
                data[data.Count - 1][1] = Convert.ToString(number1);
                data[data.Count - 1][2] = Convert.ToString(value1);
                data[data.Count - 1][3] = Convert.ToString(number3);
                data[data.Count - 1][4] = Convert.ToString(value3);
                data[data.Count - 1][5] = Convert.ToString(value5);
            }
            foreach (string[] s in data)
                dataGridView1.Rows.Add(s);
            con.Close();
        }

        private void button1_MouseDown(object sender, MouseEventArgs e)
        {
            this.Hide();
            Form5 form5 = new Form5();
            form5.Show();
            this.Close();
        }

        private void Form10_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            con.Open();
            this.dataGridView2.Rows.Clear();
            this.dataGridView3.Rows.Clear();
            string kat = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            List<int> index = new List<int>();
            SqlCommand command = new SqlCommand("select Id_product from Товары where Категория=@kat", con);
            command.Parameters.AddWithValue("@kat", kat);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
                index.Add(Convert.ToInt32(reader[0].ToString()));
            reader.Close();
            List<string[]> data = new List<string[]>();
            for(int i = 0; i < index.Count; i++)
            {
                SqlCommand command1 = new SqlCommand("select [Розничная цена], Наименование from Товары where Id_product=@id", con);
                command1.Parameters.AddWithValue("@id", index[i]);
                SqlDataReader reader1 = command1.ExecuteReader();
                reader1.Read();
                decimal value = Convert.ToDecimal(reader1[0].ToString());
                string name = reader1[1].ToString();
                reader1.Close();
                SqlCommand command2 = new SqlCommand("select Number,Data from Продажи where (Id_product=@id) and (Data BETWEEN @data1 AND @data2)", con);
                command2.Parameters.AddWithValue("@data1", dateTimePicker1.Value);
                command2.Parameters.AddWithValue("@data2", dateTimePicker2.Value);
                command2.Parameters.AddWithValue("@id", index[i]);
                SqlDataReader reader2 = command2.ExecuteReader();
                int l = 0;
                while (reader2.Read())
                {
                    int number = Convert.ToInt32(reader2[0].ToString());
                    string date = reader2[1].ToString();
                    data.Add(new string[5]);

                    data[data.Count - 1][0] = name;
                    data[data.Count - 1][1] = Convert.ToString(value);
                    data[data.Count - 1][2] = Convert.ToString(number);
                    data[data.Count - 1][3] = Convert.ToString(value * number);
                    data[data.Count - 1][4] = date;
                    l++;
                }
                reader2.Close();
            }


            List<int> index1 = new List<int>();
            SqlCommand command3 = new SqlCommand("select Id_product from Товары where Категория=@kat", con);
            command3.Parameters.AddWithValue("@kat", kat);
            SqlDataReader reader3 = command3.ExecuteReader();
            while (reader3.Read())
                index1.Add(Convert.ToInt32(reader3[0].ToString()));
            reader3.Close();
            List<string[]> data1 = new List<string[]>();
            for (int i = 0; i < index1.Count; i++)
            {
                SqlCommand command4 = new SqlCommand("select [Оптовая цена], Наименование from Товары where Id_product=@id", con);
                command4.Parameters.AddWithValue("@id", index1[i]);
                SqlDataReader reader4 = command4.ExecuteReader();
                reader4.Read();
                decimal value = Convert.ToDecimal(reader4[0].ToString());
                string name = reader4[1].ToString();
                reader4.Close();
                SqlCommand command5 = new SqlCommand("select Number,Data from Покупки where (Id_product=@id) and (Data BETWEEN @data1 AND @data2)", con);
                command5.Parameters.AddWithValue("@data1", dateTimePicker1.Value);
                command5.Parameters.AddWithValue("@data2", dateTimePicker2.Value);
                command5.Parameters.AddWithValue("@id", index1[i]);
                SqlDataReader reader5 = command5.ExecuteReader();
                int l = 0;
                while (reader5.Read())
                {
                    int number = Convert.ToInt32(reader5[0].ToString());
                    string date = reader5[1].ToString();
                    data1.Add(new string[5]);

                    data1[data1.Count - 1][0] = name;
                    data1[data1.Count - 1][1] = Convert.ToString(value);
                    data1[data1.Count - 1][2] = Convert.ToString(number);
                    data1[data1.Count - 1][3] = Convert.ToString(value * number);
                    data1[data1.Count - 1][4] = date;
                    l++;
                }
                reader5.Close();
            }
            foreach (string[] s in data)
                dataGridView2.Rows.Add(s);
            foreach (string[] s in data1)
                dataGridView3.Rows.Add(s);

            con.Close();
        }
    }
}
