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

namespace BooksShop
{
    /// <summary>
    /// Логика взаимодействия для Admin.xaml
    /// </summary>
    public partial class Admin : Window
    {
        public Admin()
        {
            InitializeComponent();
        }

        private void RedOrd_Click(object sender, RoutedEventArgs e)
        {
            AdminCustomer ac = new AdminCustomer();
            ac.Show();
            this.Close();
        }

        private void RedEmp_Click(object sender, RoutedEventArgs e)
        {
            AdminEmployees ae = new AdminEmployees();
            ae.Show();
            this.Close();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы уверены что хотите выйти из учетной записи?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                MainWindow main = new MainWindow();
                main.Show();
                this.Close();
            }
            else if (result == MessageBoxResult.No)
            {

            }
        }

        private void zak_Click(object sender, RoutedEventArgs e)
        {
            AllOrders allor = new AllOrders();
            allor.tableZak();
            allor.Show();
        }

        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Это окно Администратора! Здесь вы можете добавлять, изменять и удалять информацию в базе данных.", "О программе", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void RedTovar_Click(object sender, RoutedEventArgs e)
        {
            Tovari tov = new Tovari();
            tov.Show();
            this.Close();
        }
    }
}
