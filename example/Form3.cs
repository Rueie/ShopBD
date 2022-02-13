using System;
using System.Data.SqlClient;
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
    public partial class Form3 : Form
    {
        public Form3(int index, string name,string firm, string country, string price,int number)
        {
            InitializeComponent();
            textBox1.Text = name;
            textBox2.Text = firm;
            textBox3.Text = country;
            textBox5.Text = number.ToString();
            decimal value = Convert.ToDecimal(price);
            decimal all_price = Convert.ToInt32(value) *Convert.ToDecimal(number);
            textBox4.Text =all_price.ToString();
            textBox6.Text= DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
            textBox7.Text = index.ToString();
        }

        private void button1_MouseDown(object sender, MouseEventArgs e)
        {
            Hide();
            Form1 form1 = new Form1();
            form1.Show();
            Close();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
        }
        private void button2_MouseDown(object sender, MouseEventArgs e)
        {
            SqlConnection con = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename=|DataDirectory|\Database1.mdf;Integrated Security = True");
            con.Open();
            SqlCommand command = new SqlCommand("insert Продажи (Id_product, Name, Number,Data) VALUES (@Id_product,@Name,@Number,@Data)",con);
            command.Parameters.AddWithValue("@Id_product", textBox7.Text);
            command.Parameters.AddWithValue("@Name",textBox1.Text);
            command.Parameters.AddWithValue("@Number", textBox5.Text);
            command.Parameters.AddWithValue("@Data", DateTime.Now);
            command.ExecuteNonQuery();
            // закрываем подключение к БД
            int index = int.Parse(textBox7.Text);
            SqlCommand command1 = new SqlCommand($"select [Кол-во в наличии], [Общее кол-во продаж],[Розничная цена],Прибыль from Товары where Id_product={index}",con);
            SqlDataReader reader = command1.ExecuteReader();
            reader.Read();
            string number_on_storage = reader[0].ToString();
            string all_sells =reader[1].ToString();
            string price = reader[2].ToString();
            string stonks = reader[3].ToString();
            reader.Close();
            int real_number_on_storage = Convert.ToInt32(number_on_storage);
            int real_all_sells = Convert.ToInt32(all_sells);
            decimal real_price = Convert.ToDecimal(price);
            decimal real_stonks = Convert.ToDecimal(stonks);
            real_number_on_storage -= Convert.ToInt32(textBox5.Text);
            real_all_sells+= Convert.ToInt32(textBox5.Text);
            real_stonks+=real_stonks+real_price* Convert.ToDecimal(textBox5.Text);
            SqlCommand command2 = new SqlCommand("update Товары set [Кол-во в наличии]=@Number, [Общее кол-во продаж]=@Number_sells, Прибыль=@stonks where Id_product=@index",con);
            command2.Parameters.AddWithValue("@Number", real_number_on_storage);
            command2.Parameters.AddWithValue("@Number_sells", real_all_sells);
            command2.Parameters.AddWithValue("@stonks", real_stonks);
            command2.Parameters.AddWithValue("@index", index);
            command2.ExecuteNonQuery();
            con.Close();
            Hide();
            Form1 form1 = new Form1();
            form1.Show();
            Close();
        }
    }
}
