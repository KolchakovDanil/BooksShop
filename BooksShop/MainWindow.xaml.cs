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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Data;

namespace BooksShop
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void INspec_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = ClassSql.GetConnSQL();
            SqlConnection sqlcon = new SqlConnection(connectionString); 
            try
            {
                if (sqlcon.State == ConnectionState.Closed) sqlcon.Open();
                string query = "SELECT COUNT(1) FROM Employees WHERE E_LOGIN = @log AND E_PASSWORD = @pas AND P_ID = 2";
                SqlCommand sqlcd = new SqlCommand(query, sqlcon);
                sqlcd.CommandType = CommandType.Text;
                sqlcd.Parameters.AddWithValue("@log", Tlog.Text);
                sqlcd.Parameters.AddWithValue("@pas", Tpas.Password);
                int count = Convert.ToInt32(sqlcd.ExecuteScalar());

                if (count == 1)
                {
                    Admin window1 = new Admin();
                    window1.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Не правильно внесены данные");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
            finally
            {
                sqlcon.Close();
            }
        }

        private void INcass_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = ClassSql.GetConnSQL();
            SqlConnection sqlcon = new SqlConnection(connectionString);
            try
            {
                if (sqlcon.State == ConnectionState.Closed) sqlcon.Open();
                string query = "SELECT COUNT(1) FROM Employees WHERE E_LOGIN = @log AND E_PASSWORD = @pas AND P_ID = 1";
                SqlCommand sqlcd = new SqlCommand(query, sqlcon);
                sqlcd.CommandType = CommandType.Text;
                sqlcd.Parameters.AddWithValue("@log", Tlog.Text);
                sqlcd.Parameters.AddWithValue("@pas", Tpas.Password);
                int count = Convert.ToInt32(sqlcd.ExecuteScalar());

                if (count == 1)
                {
                    Cashier cashier = new Cashier();
                    cashier.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Не правильно внесены данные");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
            finally
            {
                sqlcon.Close();
            }
        }

        private void inlog(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void inpass(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void him_Click(object sender, RoutedEventArgs e)
        {
            him.Visibility = Visibility.Hidden;
            cass.Visibility = Visibility.Hidden;
            TBlog.Visibility = Visibility.Visible;
            TBpas.Visibility = Visibility.Visible;
            Tlog.Visibility = Visibility.Visible;
            Tpas.Visibility = Visibility.Visible;
            INspec.Visibility = Visibility.Visible;
            Exit.Visibility = Visibility.Visible;
        }

        private void cass_Click(object sender, RoutedEventArgs e)
        {
            him.Visibility = Visibility.Hidden;
            cass.Visibility = Visibility.Hidden;
            TBlog.Visibility = Visibility.Visible;
            TBpas.Visibility = Visibility.Visible;
            Tlog.Visibility = Visibility.Visible;
            Tpas.Visibility = Visibility.Visible;
            INcass.Visibility = Visibility.Visible;
            Exit.Visibility = Visibility.Visible;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            him.Visibility = Visibility.Visible;
            cass.Visibility = Visibility.Visible;
            TBlog.Visibility = Visibility.Hidden;
            TBpas.Visibility = Visibility.Hidden;
            Tlog.Visibility = Visibility.Hidden;
            Tpas.Visibility = Visibility.Hidden;
            INcass.Visibility = Visibility.Hidden;
            INspec.Visibility = Visibility.Hidden;
            Exit.Visibility = Visibility.Hidden;
        }
    }
}
