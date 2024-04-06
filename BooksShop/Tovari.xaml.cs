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
using System.IO;

namespace BooksShop
{
    /// <summary>
    /// Логика взаимодействия для Tovari.xaml
    /// </summary>
    public partial class Tovari : Window
    {
        public Tovari()
        {
            InitializeComponent();
            vivodID();
            inTypeBooks();
        }
        private void inRus(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^а-яА-Я]");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void inNumber(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void inNoRus(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^а-яА-Я]");
            e.Handled = !regex.IsMatch(e.Text);
        }
        private void inNoEng(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^a-zA-Z]");
            e.Handled = !regex.IsMatch(e.Text);
        }
        public void vivodID()
        {
            CSzak.Items.Clear();
            string connectionString = ClassSql.GetConnSQL();

            string query = "SELECT B_ID FROM Books";

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
        public void inTypeBooks()
        {
            string connectionString = ClassSql.GetConnSQL();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                Vobr.Items.Clear();
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT T_TITLE FROM TypeBooks", conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string title = reader.GetString(0);
                            if (!Vobr.Items.Contains(title))
                            {
                                Vobr.Items.Add(title);
                            }
                        }
                    }
                }
            }
        }
        public void inImage(string Path)
        {
            if (CSzak.SelectedIndex != -1)
            {
                string basePath = AppDomain.CurrentDomain.BaseDirectory;
                string imagePath = System.IO.Path.Combine(basePath, Path);
                imageBook.Source = new BitmapImage(new Uri(imagePath));
            }
            else
            {
                imageBook.Source = null;
            }
        }
        private void CSzak_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string connectionString = ClassSql.GetConnSQL();
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            string sql = "select B_PRICE from Books where B_ID  = '" + CSzak.SelectedItem + "'";
            SqlCommand Sqlcmd = new SqlCommand(sql, conn);
            sum.Text = Convert.ToString(Sqlcmd.ExecuteScalar());

            string sql10 = "select B_IMAGE from Books where B_ID  = '" + CSzak.SelectedItem + "'";
            SqlCommand Sqlcmd10 = new SqlCommand(sql10, conn);
            img.Text = Convert.ToString(Sqlcmd10.ExecuteScalar());

            string sql11 = "select B_INFO from Books where B_ID  = '" + CSzak.SelectedItem + "'";
            SqlCommand Sqlcmd11 = new SqlCommand(sql11, conn);
            info.Text = Convert.ToString(Sqlcmd11.ExecuteScalar());

            string sql3 = "select T_TITLE from Books inner join TypeBooks on Books.B_TITLE = TypeBooks.T_ID where B_ID ='" + CSzak.SelectedItem + "'";
            SqlCommand Sqlcmd3 = new SqlCommand(sql3, conn);
            Vobr.SelectedItem = Convert.ToString(Sqlcmd3.ExecuteScalar());

            string sql6 = "select B_NAME from Books where B_ID  ='" + CSzak.SelectedItem + "'";
            SqlCommand Sqlcmd6 = new SqlCommand(sql6, conn);
            Vode.Text = Convert.ToString(Sqlcmd6.ExecuteScalar());

            inImage(img.Text);

            conn.Close();
        }
        
        private void btnImg_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            dlg.InitialDirectory = @"C:\Users\d4six\source\repos\BooksShop\BooksShop\bin\Debug\ImageBooks";

            dlg.Filter = "Изображения (*.jpg, *.png)|*.jpg;*.png|Все файлы (*.*)|*.*";

            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                string selectedFile = dlg.FileName;

                if (!selectedFile.Contains("ImageBooks"))
                {
                    MessageBox.Show("Пожалуйста, выберите файл из папки ImageBooks", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                string fileName = System.IO.Path.GetFileName(selectedFile);

                string relativePath = System.IO.Path.Combine("ImageBooks", fileName);

                img.Text = relativePath;

                string basePath = AppDomain.CurrentDomain.BaseDirectory;
                string imagePath = System.IO.Path.Combine(basePath, relativePath);
                imageBook.Source = new BitmapImage(new Uri(imagePath));
            }
        }

        private void Ofzakaz_Click(object sender, RoutedEventArgs e)
        {
            if (sum.Text == "" || info.Text == "" || Vode.Text == "" || img.Text == "" || Vobr.SelectedIndex == -1)
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
                    string Vobr1 = "SELECT T_ID FROM TypeBooks WHERE T_TITLE = '" + Vobr.SelectedItem + "'";
                    SqlCommand cmd2 = new SqlCommand(Vobr1, conn6);
                    int Vobr12 = Convert.ToInt32(cmd2.ExecuteScalar());

                    string all = "Insert into Books (B_NAME, B_PRICE, B_IMAGE, B_TITLE, B_INFO) values ('" + Vode.Text + "','" + sum.Text + "','" + img.Text + "','" + Vobr12 + "','" + info.Text + "')";
                    SqlCommand alll = new SqlCommand(all, conn6); alll.ExecuteNonQuery();

                    conn6.Close();
                    MessageBox.Show("Книга успешно добавлена");

                    Ozak_Click(null, null);
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
        private void Rzak_Click(object sender, RoutedEventArgs e)
        {
            Rzak.Visibility = Visibility.Hidden;
            Ofzakaz.Visibility = Visibility.Hidden;
            Ozak.Visibility = Visibility.Visible;
            CSzak.Visibility = Visibility.Visible;
            TIDzak.Visibility = Visibility.Visible;
            Bsohr.Visibility = Visibility.Visible;
            Bdel.Visibility = Visibility.Visible;
            btnNewType.Visibility = Visibility.Hidden;

            imageBook.Source = null;
            sum.Clear();
            info.Clear();
            img.Clear();
            Vode.Clear();
            Vobr.Items.Clear();
            CSzak.Items.Clear();
            vivodID();
            inTypeBooks();
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
            btnNewType.Visibility = Visibility.Visible;

            imageBook.Source = null;
            sum.Clear();
            info.Clear();
            img.Clear();
            Vode.Clear();
            Vobr.Items.Clear();
            CSzak.Items.Clear();
            vivodID();
            inTypeBooks();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Admin admin = new Admin();
            admin.Show();
            this.Close();
        }

        private void Bsohr_Click(object sender, RoutedEventArgs e)
        {
            if (CSzak.SelectedIndex == -1)
            {
                MessageBox.Show("Вы не выбрали код книги!");
            }
            else
            {
                if (sum.Text == "" || info.Text == "" || Vode.Text == "" || img.Text == "" || Vobr.SelectedIndex == -1)
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
                        string save = "SELECT T_ID FROM TypeBooks WHERE T_TITLE ='" + Vobr.SelectedItem + "'";
                        SqlCommand save1 = new SqlCommand(save, savezak);
                        string saveType = Convert.ToString(save1.ExecuteScalar());

                        string saveBook = "Update Books set B_TITLE = '" + saveType + "', B_NAME = '" + Vode.Text + "', B_PRICE = '" + sum.Text + "', B_IMAGE = '" + img.Text + "', B_INFO = '" + info.Text + "' where B_ID =" + CSzak.SelectedItem;
                        SqlCommand sz = new SqlCommand(saveBook, savezak); sz.ExecuteNonQuery();

                        savezak.Close();
                        CSzak.SelectedIndex = -1;
                        Vobr.SelectedIndex = -1;
                        vivodID();
                        inTypeBooks();
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
                MessageBox.Show("Вы не выбрали код книги!");
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Вы уверены что хотите удалить данную книгу?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    string connectionString = ClassSql.GetConnSQL();
                    string query = "DELETE FROM Books WHERE B_ID = @BookId";

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@BookId", Convert.ToString(CSzak.SelectedItem));

                        try
                        {
                            connection.Open();
                            command.ExecuteNonQuery();
                            connection.Close();
                            CSzak.SelectedIndex = -1;
                            Vobr.SelectedIndex = -1;
                            vivodID();
                            inTypeBooks();
                            MessageBox.Show("Книга удалена");
                        }
                        catch (SqlException ex)
                        {
                            MessageBox.Show("SQL Error: " + ex.Message);
                        }
                    }
                }
                else if (result == MessageBoxResult.No)
                {

                }
            }
        }

        private void btnNewType_Click(object sender, RoutedEventArgs e)
        {
            addTypeBooks addTB = new addTypeBooks();
            addTB.Show();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            Vobr.SelectedIndex = -1;
            CSzak.SelectedIndex = -1;
            vivodID();
            inTypeBooks();
        }

        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("В данном окне происходит добавление, изменение и удаление данных о книгах. " +
                "Если вам нужна новая категория для книги нажмите 'Добавить тип книги'. Чтобы поменять или добавить новое изображение " +
                "для соответствующей книги нажмите 'Выбрать изображение'. Важно! При выборе изображения поместите файл в корневую папку \"ImageBooks\"!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
