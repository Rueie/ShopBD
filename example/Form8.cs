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
    public partial class Form8 : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename=|DataDirectory|\Database1.mdf;Integrated Security = True");
        public Form8()
        {
            InitializeComponent();
            con.Open();
            SqlCommand command = new SqlCommand("SELECT MIN([Общее кол-во продаж]) FROM Товары", con);
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            int number_of_sells = Convert.ToInt32(reader[0].ToString());
            reader.Close();
            SqlCommand command1 = new SqlCommand($"SELECT Id_product,Наименование,Категория, Фирма, [Страна-изготовитель],[Общее кол-во продаж],Прибыль FROM Товары where [Общее кол-во продаж]={number_of_sells}", con);
            SqlDataReader reader1 = command1.ExecuteReader();
            List<string[]> data = new List<string[]>();
            while (reader1.Read())
            {
                data.Add(new string[7]);
                
                data[data.Count - 1][0] = reader1[0].ToString();
                data[data.Count - 1][2] = reader1[1].ToString();
                data[data.Count - 1][1] = reader1[2].ToString();
                data[data.Count - 1][3] = reader1[3].ToString();
                data[data.Count - 1][4] = reader1[4].ToString();
                data[data.Count - 1][5] = reader1[5].ToString();
                data[data.Count - 1][6] = reader1[6].ToString();
            }
            reader1.Close();
            foreach (string[] s in data)
                dataGridView1.Rows.Add(s);

            SqlCommand command2 = new SqlCommand("SELECT MAX([Общее кол-во продаж]) FROM Товары", con);
            SqlDataReader reader2 = command2.ExecuteReader();
            reader2.Read();
            int number_of_sells1 = Convert.ToInt32(reader2[0].ToString());
            reader2.Close();
            SqlCommand command3 = new SqlCommand($"SELECT Id_product,Наименование,Категория, Фирма, [Страна-изготовитель],[Общее кол-во продаж],Прибыль FROM Товары where [Общее кол-во продаж]={number_of_sells1}", con);
            SqlDataReader reader3 = command3.ExecuteReader();
            List<string[]> data1 = new List<string[]>();
            while (reader3.Read())
            {
                data1.Add(new string[7]);

                data1[data1.Count - 1][0] = reader3[0].ToString();
                data1[data1.Count - 1][2] = reader3[1].ToString();
                data1[data1.Count - 1][1] = reader3[2].ToString();
                data1[data1.Count - 1][3] = reader3[3].ToString();
                data1[data1.Count - 1][4] = reader3[4].ToString();
                data1[data1.Count - 1][5] = reader3[5].ToString();
                data1[data1.Count - 1][6] = reader3[6].ToString();
            }
            reader3.Close();
            foreach (string[] s in data1)
                dataGridView2.Rows.Add(s);
            
            SqlCommand command4 = new SqlCommand("SELECT MIN(Прибыль) FROM Товары", con);
            SqlDataReader reader4 = command4.ExecuteReader();
            reader4.Read();
            decimal price_of_sells = Convert.ToDecimal(reader4[0].ToString());
            reader4.Close();
            SqlCommand command5 = new SqlCommand("SELECT Id_product,Наименование,Категория,Фирма,[Страна-изготовитель],[Общее кол-во продаж],Прибыль FROM Товары where Прибыль=@price", con);
            command5.Parameters.AddWithValue("@price", price_of_sells);
            SqlDataReader reader5 = command5.ExecuteReader();
            List<string[]> data2 = new List<string[]>();
            while (reader5.Read())
            {
                data2.Add(new string[7]);

                data2[data2.Count - 1][0] = reader5[0].ToString();
                data2[data2.Count - 1][2] = reader5[1].ToString();
                data2[data2.Count - 1][1] = reader5[2].ToString();
                data2[data2.Count - 1][3] = reader5[3].ToString();
                data2[data2.Count - 1][4] = reader5[4].ToString();
                data2[data2.Count - 1][5] = reader5[5].ToString();
                data2[data2.Count - 1][6] = reader5[6].ToString();
            }
            reader5.Close();
            foreach (string[] s in data2)
                dataGridView3.Rows.Add(s);

            SqlCommand command6 = new SqlCommand("SELECT MAX(Прибыль) FROM Товары", con);
            SqlDataReader reader6 = command6.ExecuteReader();
            reader6.Read();
            decimal price_of_sells1 = Convert.ToDecimal(reader6[0].ToString());
            reader6.Close();
            SqlCommand command7 = new SqlCommand($"SELECT Id_product,Наименование,Категория, Фирма, [Страна-изготовитель],[Общее кол-во продаж],Прибыль FROM Товары where Прибыль=@price", con);
            command7.Parameters.AddWithValue("@price", price_of_sells1);
            SqlDataReader reader7 = command7.ExecuteReader();
            List<string[]> data3 = new List<string[]>();
            while (reader7.Read())
            {
                data3.Add(new string[7]);

                data3[data3.Count - 1][0] = reader7[0].ToString();
                data3[data3.Count - 1][2] = reader7[1].ToString();
                data3[data3.Count - 1][1] = reader7[2].ToString();
                data3[data3.Count - 1][3] = reader7[3].ToString();
                data3[data3.Count - 1][4] = reader7[4].ToString();
                data3[data3.Count - 1][5] = reader7[5].ToString();
                data3[data3.Count - 1][6] = reader7[6].ToString();
            }
            reader7.Close();
            foreach (string[] s in data3)
                dataGridView4.Rows.Add(s);


            int counter = 0;
            SqlCommand com = new SqlCommand("select distinct Категория from Товары", con);
            SqlDataReader read = com.ExecuteReader();
            var string1 = new List<string>();
            while (read.Read())
            {
                string1.Add(read[0].ToString());
                counter++;
            }
            read.Close();
            var values = new List<int>();
            for (int i = 0; i < counter; i++)
            {
                values.Add(0);
                SqlCommand com1 = new SqlCommand("select [Общее кол-во продаж] from Товары where Категория=@kat", con);
                com1.Parameters.AddWithValue("@kat", string1[i]);
                SqlDataReader read1 = com1.ExecuteReader();
                while (read1.Read())
                    values[i] += Convert.ToInt32(read1[0].ToString());
                read1.Close();
            }
            int min = values[0];
            int max = min;
            for (int i = 1; i < counter; i++) {
                if (values[i] < min) min = values[i];
                else if (values[i] > max) max = values[i];
            }
            List<string[]> data4 = new List<string[]>();
            List<string[]> data5 = new List<string[]>();
            for(int i = 0; i < counter; i++)
            {
                if (values[i] == min)
                {
                    data4.Add(new string[2]);
                    data4[data4.Count - 1][0] = string1[i];
                    data4[data4.Count - 1][1] = Convert.ToString(min);
                }
                else if (values[i] == max)
                {
                    data5.Add(new string[2]);
                    data5[data5.Count - 1][0] = string1[i];
                    data5[data5.Count - 1][1] = Convert.ToString(max);
                }
            }
            foreach (string[] s in data4)
                dataGridView6.Rows.Add(s);
            foreach (string[] s in data5)
                dataGridView5.Rows.Add(s);

            var stonks = new List<decimal>();
            for (int i = 0; i < counter; i++)
            {
                stonks.Add(0);
                SqlCommand com1 = new SqlCommand("select Прибыль from Товары where Категория=@kat", con);
                com1.Parameters.AddWithValue("@kat", string1[i]);
                SqlDataReader read2 = com1.ExecuteReader();
                while (read2.Read())
                    stonks[i] += Convert.ToDecimal(read2[0].ToString());
                read2.Close();
            }
            decimal min1 = stonks[0];
            decimal max1 = min1;
            for (int i = 1; i < counter; i++)
            {
                if (stonks[i] < min1) min1 = stonks[i];
                else if (stonks[i] > max1) max1 = stonks[i];
            }
            List<string[]> data6 = new List<string[]>();
            List<string[]> data7 = new List<string[]>();
            for (int i = 0; i < counter; i++)
            {
                if (stonks[i] == min1)
                {
                    data6.Add(new string[2]);
                    data6[data6.Count - 1][0] = string1[i];
                    data6[data6.Count - 1][1] = Convert.ToString(min1);
                }
                else if (stonks[i] == max1)
                {
                    data7.Add(new string[2]);
                    data7[data7.Count - 1][0] = string1[i];
                    data7[data7.Count - 1][1] = Convert.ToString(max1);
                }
            }
            foreach (string[] s in data6)
                dataGridView7.Rows.Add(s);
            foreach (string[] s in data7)
                dataGridView8.Rows.Add(s);
            con.Close();
        }

        private void button1_MouseDown(object sender, MouseEventArgs e)
        {
            this.Hide();
            Form5 form5 = new Form5();
            form5.Show();
            this.Close();
        }

        private void Form8_Load(object sender, EventArgs e)
        {

        }
    }
}
