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
    /// Логика взаимодействия для AdminEmployees.xaml
    /// </summary>
    public partial class AdminEmployees : Window
    {
        public AdminEmployees()
        {
            InitializeComponent();
            vivodID();
            PostPokaz();
        }

        private void CSzak_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SqlConnection conn = new SqlConnection(@"Data Source = DESKTOP-MJM6IQP\SQLEXPRESS; Initial Catalog = BookShop; Integrated Security = True");
            conn.Open();
            string sql = "select E_SURNAME from Employees where E_ID  = '" + CSzak.SelectedItem + "'";
            SqlCommand Sqlcmd = new SqlCommand(sql, conn);
            Sur.Text = Convert.ToString(Sqlcmd.ExecuteScalar());

            string sql10 = "select E_NAME from Employees where E_ID  = '" + CSzak.SelectedItem + "'";
            SqlCommand Sqlcmd10 = new SqlCommand(sql10, conn);
            Name.Text = Convert.ToString(Sqlcmd10.ExecuteScalar());

            string sql11 = "select E_PARTONYMIC from Employees where E_ID  = '" + CSzak.SelectedItem + "'";
            SqlCommand Sqlcmd11 = new SqlCommand(sql11, conn);
            FIO.Text = Convert.ToString(Sqlcmd11.ExecuteScalar());

            string sql2 = "select E_TELEPHONE from Employees where E_ID  ='" + CSzak.SelectedItem + "'";
            SqlCommand Sqlcmd2 = new SqlCommand(sql2, conn);
            phone.Text = Convert.ToString(Sqlcmd2.ExecuteScalar());

            string sql3 = "select P_TITLE from Employees inner join Post on Employees.P_ID = Post.P_ID where E_ID ='" + CSzak.SelectedItem + "'";
            SqlCommand Sqlcmd3 = new SqlCommand(sql3, conn);
            Post.SelectedItem = Convert.ToString(Sqlcmd3.ExecuteScalar());

            string sql6 = "select E_LOGIN from Employees where E_ID  ='" + CSzak.SelectedItem + "'";
            SqlCommand Sqlcmd6 = new SqlCommand(sql6, conn);
            Login.Text = Convert.ToString(Sqlcmd6.ExecuteScalar());

            string sql7 = "select E_PASSWORD from Employees where E_ID  ='" + CSzak.SelectedItem + "'";
            SqlCommand Sqlcmd7 = new SqlCommand(sql7, conn);
            Password.Text = Convert.ToString(Sqlcmd7.ExecuteScalar());

            conn.Close();
        }

        public void vivodID()
        {
            CSzak.Items.Clear();
            string connectionString = @"Data Source = DESKTOP-MJM6IQP\SQLEXPRESS; Initial Catalog = BookShop; Integrated Security = True";
            string query = "SELECT E_ID FROM Employees";

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
        public void PostPokaz()
        {
            string connectionString = @"Data Source = DESKTOP-MJM6IQP\SQLEXPRESS; Initial Catalog = BookShop; Integrated Security = True";
            string query = "SELECT P_TITLE FROM Employees INNER JOIN Post ON Post.P_ID = Employees.P_ID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string PostEmp = reader.GetString(0);
                        if (!Post.Items.Contains(PostEmp))
                        {
                            Post.Items.Add(PostEmp);
                        }
                    }
                }
            }
        }

        private void Ofzakaz_Click(object sender, RoutedEventArgs e)
        {
            if (Sur.Text == "" || Name.Text == "" || FIO.Text == "" || phone.Text == "" || Post.SelectedIndex == -1 || Login.Text == "" || Password.Text == "")
            {
                MessageBox.Show("Вы не заполнили все данные");
            }
            else
            {
                CSzak.SelectedIndex = -1;
                SqlConnection conn6 = new SqlConnection(@"Data Source = DESKTOP-MJM6IQP\SQLEXPRESS; Initial Catalog = BookShop; Integrated Security = True");
                try
                {
                    conn6.Open();
                    string Vobr1 = "SELECT P_ID FROM Post WHERE P_TITLE = '" + Post.SelectedItem + "'";
                    SqlCommand cmd2 = new SqlCommand(Vobr1, conn6);
                    int Vobr12 = Convert.ToInt32(cmd2.ExecuteScalar());

                    string all = "Insert into Employees (E_SURNAME, E_NAME, E_PARTONYMIC, E_TELEPHONE, P_ID, E_LOGIN, E_PASSWORD) values ('" + Sur.Text + "','" + Name.Text + "','" + FIO.Text + "','" + phone.Text + "','" + Vobr12 + "','" + Login.Text + "','" + Password.Text + "')";
                    SqlCommand alll = new SqlCommand(all, conn6); alll.ExecuteNonQuery();

                    conn6.Close();
                    MessageBox.Show("Сотрудник успешно добавлен");
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

        private void insur(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^а-яА-Я]");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void inname(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^а-яА-Я]");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void inpatr(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^а-яА-Я]");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void inphone(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Rzak_Click(object sender, RoutedEventArgs e)
        {
            Rzak.Visibility = Visibility.Hidden;
            Ofzakaz.Visibility = Visibility.Hidden;
            Ozak.Visibility = Visibility.Visible;
            CSzak.Visibility = Visibility.Visible;
            TIDzak.Visibility = Visibility.Visible;
            Bsohr.Visibility = Visibility.Visible;
            Bdel.Visibility = Visibility.Visible;

            Sur.Clear();
            Name.Clear();
            FIO.Clear();
            phone.Clear();
            Login.Clear();
            Password.Clear();
            CSzak.Items.Clear();
            Post.Items.Clear();
            vivodID();
            PostPokaz();
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
            Login.Clear();
            Password.Clear();
            CSzak.Items.Clear();
            Post.Items.Clear();
            vivodID();
            PostPokaz();
        }

        private void Bsohr_Click(object sender, RoutedEventArgs e)
        {
            if (CSzak.SelectedIndex == -1)
            {
                MessageBox.Show("Вы не выбрали код сотрудника!");
            }
            else
            {
                if (Sur.Text == "" || Name.Text == "" || FIO.Text == "" || phone.Text == "" || Post.SelectedIndex == -1 || Login.Text == "" || Password.Text == "")
                {
                    MessageBox.Show("Вы не заполнили все данные");
                }
                else
                {
                    SqlConnection savezak = new SqlConnection(@"Data Source = DESKTOP-MJM6IQP\SQLEXPRESS; Initial Catalog = BookShop; Integrated Security = True");
                    try
                    {
                        savezak.Open();
                        string save = "SELECT P_ID FROM Post WHERE P_TITLE ='" + Post.SelectedItem + "'";
                        SqlCommand save1 = new SqlCommand(save, savezak);
                        string savePost = Convert.ToString(save1.ExecuteScalar());

                        string savezakaz = "Update Employees set P_ID = '" + savePost + "', E_SURNAME = '" + Sur.Text + "', E_NAME = '" + Name.Text + "', E_PARTONYMIC = '" + FIO.Text + "', E_TELEPHONE = '" + phone.Text + "', E_LOGIN = '" + Login.Text + "', E_PASSWORD = '" + Password.Text + "' where E_ID =" + CSzak.SelectedItem;
                        SqlCommand sz = new SqlCommand(savezakaz, savezak); sz.ExecuteNonQuery();

                        savezak.Close();
                        CSzak.SelectedIndex = -1;
                        Post.SelectedIndex = -1;
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
                MessageBox.Show("Вы не выбрали код сотрудника!");
            }
            else
            {
                string connectionString = @"Data Source = DESKTOP-MJM6IQP\SQLEXPRESS; Initial Catalog = BookShop; Integrated Security = True";
                string query = "DELETE FROM Employees WHERE E_ID = @EmployeesId";

                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@EmployeesId", Convert.ToString(CSzak.SelectedItem));

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        CSzak.SelectedIndex = -1;
                        Post.SelectedIndex = -1;
                        vivodID();
                        MessageBox.Show("Сотрудник удален");
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
