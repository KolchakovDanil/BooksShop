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
using System.Data.SqlClient;
using System.Data;
using System.Windows.Navigation;
using System.Diagnostics;

namespace BooksShop
{
    /// <summary>
    /// Логика взаимодействия для AllOrders.xaml
    /// </summary>
    public partial class AllOrders : Window
    {
        private DataTable ordersTable;
        private DataView ordersView;
        public AllOrders()
        {
            InitializeComponent();
            tableZak();
        }

        static DataTable ExecuteSql(string sql)
        {
            DataTable DT = new DataTable();
            string connectionString = ClassSql.GetConnSQL();
            SqlConnection sqlcon = new SqlConnection(connectionString);
            using (sqlcon)
            {
                sqlcon.Open();
                SqlCommand sqlcd = new SqlCommand(sql, sqlcon);
                SqlDataReader sqlread = sqlcd.ExecuteReader();
                using (sqlread)
                {
                    DT.Load(sqlread);
                }
            }
            return DT;
        }
        public void tableZak()
        {
            ordersTable = ExecuteSql("SELECT * FROM Orderr inner join Customer on Orderr.O_TELEPHONE = Customer.CU_TELEPHONE inner join Books on Orderr.B_ID  = Books.B_ID inner join TypeBooks on Orderr.T_ID  = TypeBooks.T_ID");
            ordersView = new DataView(ordersTable);
            listzakaz.ItemsSource = ordersView;
        }
        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ordersView != null)
            {
                string searchText = txtSearch.Text.ToLower();
                if (!string.IsNullOrEmpty(searchText))
                {
                    StringBuilder filterBuilder = new StringBuilder();

                    string[] searchWords = searchText.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string word in searchWords)
                    {
                        if (filterBuilder.Length > 0)
                        {
                            filterBuilder.Append(" AND ");
                        }
                        filterBuilder.Append($"CU_SURNAME LIKE '%{word}%'");
                    }
                    ordersView.RowFilter = filterBuilder.ToString();
                }
                else
                {
                    ordersView.RowFilter = string.Empty;
                }
            }
        }

        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Важно знать, что контекстный поиск осуществляется по фамилии клиента!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);

        }
        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBlock textBlock)
            {
                string email = textBlock.Text;
                try
                {
                    string url = $"https://e.mail.ru/compose/?to={email}";

                    Process.Start(new ProcessStartInfo(url));
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при открытии браузера: " + ex.Message);
                }
            }
        }
    }
}

