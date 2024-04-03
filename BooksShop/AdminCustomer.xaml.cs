using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Data;

namespace BooksShop
{
    /// <summary>
    /// Логика взаимодействия для AdminCustomer.xaml
    /// </summary>
    public partial class AdminCustomer : Window
    {
        public AdminCustomer()
        {
            InitializeComponent();
            vivodID();
            Rzak_Click(null, null);
        }
        private void CSzak_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string connectionString = ClassSql.GetConnSQL();
            SqlConnection conn = new SqlConnection(connectionString);

            conn.Open();
            string sql = "select CU_SURNAME from Customer where CU_ID  = '" + CSzak.SelectedItem + "'";
            SqlCommand Sqlcmd = new SqlCommand(sql, conn);
            Sur.Text = Convert.ToString(Sqlcmd.ExecuteScalar());

            string sql10 = "select CU_NAME from Customer where CU_ID  = '" + CSzak.SelectedItem + "'";
            SqlCommand Sqlcmd10 = new SqlCommand(sql10, conn);
            Name.Text = Convert.ToString(Sqlcmd10.ExecuteScalar());

            string sql11 = "select CU_PARTONYMIC from Customer where CU_ID  = '" + CSzak.SelectedItem + "'";
            SqlCommand Sqlcmd11 = new SqlCommand(sql11, conn);
            FIO.Text = Convert.ToString(Sqlcmd11.ExecuteScalar());

            string sql2 = "select CU_TELEPHONE from Customer where CU_ID  ='" + CSzak.SelectedItem + "'";
            SqlCommand Sqlcmd2 = new SqlCommand(sql2, conn);
            phone.Text = Convert.ToString(Sqlcmd2.ExecuteScalar());

            string sql3 = "select CU_EMAIL from Customer where CU_ID  ='" + CSzak.SelectedItem + "'";
            SqlCommand Sqlcmd3 = new SqlCommand(sql3, conn);
            email.Text = Convert.ToString(Sqlcmd3.ExecuteScalar());

            conn.Close();
        }

        public void vivodID()
        {
            CSzak.Items.Clear();
            string connectionString = ClassSql.GetConnSQL();
            string query = "SELECT CU_ID FROM Customer";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int orderId = reader.GetInt32(0);
                        CSzak.Items.Add(orderId);
                    }
                }
            }
        }
       
        private void Ofzakaz_Click(object sender, RoutedEventArgs e)
        {
            if (Sur.Text == "" || Name.Text == "" || FIO.Text == "" || phone.Text == "" || email.Text == "")
            {
                MessageBox.Show("Вы не заполнили все данные");
            }
            else
            {
                CSzak.SelectedIndex = -1;
                string connectionString = ClassSql.GetConnSQL();
                SqlConnection conn6 = new SqlConnection(connectionString);

                try
                {
                    conn6.Open();
                    string all = "Insert into Customer (CU_SURNAME, CU_NAME, CU_PARTONYMIC, CU_TELEPHONE, CU_EMAIL) values ('" + Sur.Text + "','" + Name.Text + "','" + FIO.Text + "','" + phone.Text + "','" + email.Text + "')";
                    SqlCommand alll = new SqlCommand(all, conn6); alll.ExecuteNonQuery();

                    conn6.Close();
                    MessageBox.Show("Клиент успешно добавлен");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    throw;
                }
                finally
                {

                }
            }
        }

        private void inRusString(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^а-яА-Я]");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void inNumber(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void inEngString(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^a-zA-Z]");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void inNoRusString(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^а-яА-Я]");
            e.Handled = !regex.IsMatch(e.Text);
        }

        private void Rzak_Click(object sender, RoutedEventArgs e)
        {
            Rzak.Visibility = Visibility.Hidden;
            Ofzakaz.Visibility = Visibility.Hidden;
            //Ozak.Visibility = Visibility.Visible;
            CSzak.Visibility = Visibility.Visible;
            TIDzak.Visibility = Visibility.Visible;
            Bsohr.Visibility = Visibility.Visible;
            Bdel.Visibility = Visibility.Visible;
            
            Sur.Clear();
            Name.Clear();
            FIO.Clear();
            phone.Clear();
            email.Clear();
            CSzak.Items.Clear();
            vivodID();
        }

        private void Ozak_Click(object sender, RoutedEventArgs e)
        {
            Rzak.Visibility = Visibility.Visible;
            Ofzakaz.Visibility = Visibility.Visible;
            Ozak.Visibility = Visibility.Hidden;
            CSzak.Visibility = Visibility.Hidden;
            TIDzak.Visibility = Visibility.Hidden;
            Bsohr.Visibility = Visibility.Hidden;
            Bdel.Visibility = Visibility.Hidden;
            Sur.Clear();
            Name.Clear();
            FIO.Clear();
            phone.Clear();
            email.Clear();
            CSzak.Items.Clear();;
            vivodID();
        }

        private void Bsohr_Click(object sender, RoutedEventArgs e)
        {
            if (CSzak.SelectedIndex == -1)
            {
                MessageBox.Show("Вы не выбрали код клиента!");
            }
            else
            {
                if (Sur.Text == "" || Name.Text == "" || FIO.Text == "" || phone.Text == "" || email.Text == "")
                {
                    MessageBox.Show("Вы не заполнили все данные");
                }
                else
                {
                    string connectionString = ClassSql.GetConnSQL();
                    SqlConnection savezak = new SqlConnection(connectionString);

                    try
                    {
                        savezak.Open();
                        string savezakaz = "Update Customer set CU_SURNAME = '" + Sur.Text + "', CU_NAME = '" + Name.Text + "', CU_PARTONYMIC = '" + FIO.Text + "', CU_TELEPHONE = '" + phone.Text + "', CU_EMAIL = '" + email.Text + "' where CU_ID =" + CSzak.SelectedItem;
                        SqlCommand sz = new SqlCommand(savezakaz, savezak); sz.ExecuteNonQuery();

                        savezak.Close();
                        CSzak.SelectedIndex = -1;
                        vivodID();
                        MessageBox.Show("Данные обновлены");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        throw;
                    }
                    finally
                    {

                    }
                }
            }
        }

        private void Bdel_Click(object sender, RoutedEventArgs e)
        {
            if (CSzak.SelectedIndex == -1)
            {
                MessageBox.Show("Вы не выбрали код клиента!");
            }
            else
            {
                string connectionString = ClassSql.GetConnSQL();
                string query = "DELETE FROM Customer WHERE CU_ID = @CustomerId";

                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CustomerId", Convert.ToString(CSzak.SelectedItem));

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        CSzak.SelectedIndex = -1;
                        vivodID();
                        connection.Close();
                        MessageBox.Show("Клиент удален");
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("SQL Error: " + ex.Message);
                    }
                }
            }
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Admin admin = new Admin();
            admin.Show();
            this.Close();
        }
    }
}
