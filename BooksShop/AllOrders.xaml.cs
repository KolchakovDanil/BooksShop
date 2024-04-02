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

namespace BooksShop
{
    /// <summary>
    /// Логика взаимодействия для AllOrders.xaml
    /// </summary>
    public partial class AllOrders : Window
    {
        public AllOrders()
        {
            InitializeComponent();
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
            DataTable DT = ExecuteSql("SELECT * FROM Orderr inner join Customer on Orderr.O_TELEPHONE = Customer.CU_TELEPHONE inner join Books on Orderr.B_ID  = Books.B_ID inner join TypeBooks on Orderr.T_ID  = TypeBooks.T_ID");
            listzakaz.ItemsSource = DT.DefaultView;
        }
    }
}

