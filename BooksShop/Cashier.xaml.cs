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
    public partial class Cashier : Window
    {
        public Cashier()
        {
            InitializeComponent();
            vivodID();
            VobrPokaz();
        }

        private void zak_Click(object sender, RoutedEventArgs e)
        {
            AllOrders allor = new AllOrders();
            allor.tableZak();
            allor.Show();
        }

        private void CSzak_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string connectionString = ClassSql.GetConnSQL();
            SqlConnection conn = new SqlConnection(connectionString);

            conn.Open();
            string sql = "select CU_SURNAME from Customer inner join Orderr on O_CU = CU_ID where O_ID  = '" + CSzak.SelectedItem + "'";
            SqlCommand Sqlcmd = new SqlCommand(sql, conn);
            Sur.Text = Convert.ToString(Sqlcmd.ExecuteScalar());

            string sql10 = "select CU_NAME from Customer inner join Orderr on O_CU = CU_ID where O_ID  = '" + CSzak.SelectedItem + "'";
            SqlCommand Sqlcmd10 = new SqlCommand(sql10, conn);
            Name.Text = Convert.ToString(Sqlcmd10.ExecuteScalar());

            string sql11 = "select CU_PARTONYMIC from Customer inner join Orderr on O_CU = CU_ID where O_ID  = '" + CSzak.SelectedItem + "'";
            SqlCommand Sqlcmd11 = new SqlCommand(sql11, conn);
            FIO.Text = Convert.ToString(Sqlcmd11.ExecuteScalar());

            string sql2 = "select CU_TELEPHONE from Customer inner join Orderr on O_CU = CU_ID where O_ID  ='" + CSzak.SelectedItem + "'";
            SqlCommand Sqlcmd2 = new SqlCommand(sql2, conn);
            phone.Text = Convert.ToString(Sqlcmd2.ExecuteScalar());

            string sql22 = "select CU_EMAIL from Customer inner join Orderr on O_CU = CU_ID where O_ID  ='" + CSzak.SelectedItem + "'";
            SqlCommand Sqlcmd22 = new SqlCommand(sql22, conn);
            email.Text = Convert.ToString(Sqlcmd22.ExecuteScalar());

            string sql3 = "select T_TITLE from Orderr inner join TypeBooks on Orderr.T_ID = TypeBooks.T_ID where O_ID ='" + CSzak.SelectedItem + "'";
            SqlCommand Sqlcmd3 = new SqlCommand(sql3, conn);
            Vobr.SelectedItem = Convert.ToString(Sqlcmd3.ExecuteScalar());

            string sql4 = "select B_NAME from Orderr inner join Books on Orderr.B_ID = Books.B_ID where O_ID ='" + CSzak.SelectedItem + "'";
            SqlCommand Sqlcmd4 = new SqlCommand(sql4, conn);
            Vode.SelectedItem = Convert.ToString(Sqlcmd4.ExecuteScalar());

            string sql44 = "select B_INFO from Orderr inner join Books on Orderr.B_ID = Books.B_ID where O_ID ='" + CSzak.SelectedItem + "'";
            SqlCommand Sqlcmd44 = new SqlCommand(sql44, conn);
            info.Text = Convert.ToString(Sqlcmd44.ExecuteScalar());

            string sql5 = "select O_DATE from Orderr where O_ID  ='" + CSzak.SelectedItem + "'";
            SqlCommand Sqlcmd5 = new SqlCommand(sql5, conn);
            date.Text = Convert.ToString(Sqlcmd5.ExecuteScalar());

            string sql6 = "select O_SUM from Orderr where O_ID  ='" + CSzak.SelectedItem + "'";
            SqlCommand Sqlcmd6 = new SqlCommand(sql6, conn);
            sum.Text = Convert.ToString(Sqlcmd6.ExecuteScalar());

            string sql7 = "select O_COUNT from Orderr where O_ID  ='" + CSzak.SelectedItem + "'";
            SqlCommand Sqlcmd7 = new SqlCommand(sql7, conn);
            count.Text = Convert.ToString(Sqlcmd7.ExecuteScalar());

            conn.Close();
        }

        public void vivodID()
        {
            CSzak.Items.Clear();
            string connectionString = ClassSql.GetConnSQL();
            string query = "SELECT O_ID FROM Orderr";

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
        public void VobrPokaz()
        {
            string connectionString = ClassSql.GetConnSQL();
            SqlConnection conn3 = new SqlConnection(connectionString);

            Vobr.Items.Clear();
            conn3.Open();
            for (int i = 0; i < 100; i++)
            {
                string id = "select T_TITLE from TypeBooks where T_ID =" + i;
                SqlCommand id2 = new SqlCommand(id, conn3);
                string Vobr13 = Convert.ToString(id2.ExecuteScalar());
                if (Vobr13 == "") { }
                else
                {
                    if (Vobr.Items.Contains(Vobr13))
                    {

                    }
                    else
                    {
                        Vobr.Items.Add(Vobr13);
                    }
                }
            }
            conn3.Close();
        }

        private void Ofzakaz_Click(object sender, RoutedEventArgs e)
        {
            if (Sur.Text == "" || Name.Text == "" || FIO.Text == "" || phone.Text == "" || date.SelectedDate == null || Vobr.SelectedIndex == -1 || Vode.SelectedIndex == -1 || count.Text == null || sum.Text == "" || info.Text == "" || email.Text == "")
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
                    string cmdit = "Insert into Customer (CU_SURNAME,CU_NAME,CU_PARTONYMIC,CU_TELEPHONE,CU_EMAIL) values ('" + Sur.Text + "','" + Name.Text + "','" + FIO.Text + "','" + phone.Text + "','" + email.Text + "')";
                    SqlCommand cmd = new SqlCommand(cmdit, conn6); cmd.ExecuteNonQuery();

                    string std = "select Top 1 CU_ID from Customer order by CU_ID desc";
                    SqlCommand cmdCu = new SqlCommand(std, conn6);
                    int cust = Convert.ToInt32(cmdCu.ExecuteScalar());

                    string Vobr1 = "SELECT T_ID FROM TypeBooks WHERE T_TITLE = '" + Vobr.SelectedItem + "'";
                    SqlCommand cmd2 = new SqlCommand(Vobr1, conn6);
                    int Vobr12 = Convert.ToInt32(cmd2.ExecuteScalar());

                    string Vode1 = "select B_ID from Books where B_NAME = '" + Vode.SelectedItem + "'";
                    SqlCommand cmd3 = new SqlCommand(Vode1, conn6);
                    int Vode12 = Convert.ToInt32(cmd3.ExecuteScalar());

                    string all = "Insert into Orderr (O_CU,O_TELEPHONE,T_ID,B_ID,O_COUNT,O_DATE,O_SUM) values ('" + cust + "','" + phone.Text + "','" + Vobr12 + "','" + Vode12 + "','" + count.Text + "','" + date.SelectedDate + "','" + sum.Text + "')";
                    SqlCommand alll = new SqlCommand(all, conn6); alll.ExecuteNonQuery();
                    conn6.Close();

                    MessageBox.Show("Заказ успешно оформлен");

                    Sur.Clear();
                    Name.Clear();
                    FIO.Clear();
                    phone.Clear();
                    date.SelectedDate = null;
                    Vobr.SelectedIndex = -1;
                    Vode.SelectedIndex = -1;
                    count.Clear();
                    sum.Clear();
                    email.Clear();
                    info.Clear();
                    imageBook.Source = null;
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

        private void Cina_Click(object sender, RoutedEventArgs e)
        {
            if (Vode.SelectedIndex == -1 || count.Text == "")
            {
                MessageBox.Show("Для результата нужно ввести данные: название книги, количество книг!");
            }
            else
            {
                string connectionString = ClassSql.GetConnSQL();
                SqlConnection conn4 = new SqlConnection(connectionString);

                try
                {
                    int count1 = Convert.ToInt32(count.Text);
                    conn4.Open();
                    string Vode1 = "SELECT B_PRICE FROM Books WHERE B_NAME = @did ";

                    SqlCommand sqlcm2 = new SqlCommand(Vode1, conn4);
                    sqlcm2.CommandType = CommandType.Text;
                    sqlcm2.Parameters.AddWithValue("@did", Convert.ToString(Vode.Text));
                    int cinaide = Convert.ToInt32(sqlcm2.ExecuteScalar());
                    
                    int x = 0;
                    x = cinaide * count1;
                    sum.Text = Convert.ToString(x);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    throw;
                }
                finally
                {
                    conn4.Close();
                }
            }
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
        private void Rzak_Click(object sender, RoutedEventArgs e)
        {
            imageBook.Source = null;
            Rzak.Visibility = Visibility.Hidden;
            Ofzakaz.Visibility = Visibility.Hidden;
            TBsz.Visibility = Visibility.Hidden;
            Ozak.Visibility = Visibility.Visible;
            CSzak.Visibility = Visibility.Visible;
            TIDzak.Visibility = Visibility.Visible;
            Szak.Visibility = Visibility.Hidden;
            T.Visibility = Visibility.Visible;
            Bsohr.Visibility = Visibility.Visible;
            Bdel.Visibility = Visibility.Visible;

            Sur.Clear();
            Name.Clear();
            FIO.Clear();
            phone.Clear();
            date.SelectedDate = null;
            Vobr.SelectedIndex = -1;
            Vode.SelectedIndex = -1;
            count.Clear();
            sum.Clear();
            email.Clear();
            info.Clear();
            imageBook.Source = null;
            vivodID();
            VobrPokaz();
        }

        private void Ozak_Click(object sender, RoutedEventArgs e)
        {
            imageBook.Source = null;
            Rzak.Visibility = Visibility.Visible;
            Ofzakaz.Visibility = Visibility.Visible;
            TBsz.Visibility = Visibility.Visible;
            Ozak.Visibility = Visibility.Hidden;
            CSzak.Visibility = Visibility.Hidden;
            TIDzak.Visibility = Visibility.Hidden;
            Szak.Visibility = Visibility.Hidden;
            T.Visibility = Visibility.Hidden;
            Bsohr.Visibility = Visibility.Hidden;
            Bdel.Visibility = Visibility.Hidden;

            Sur.Clear();
            Name.Clear();
            FIO.Clear();
            phone.Clear();
            date.SelectedDate = null;
            Vobr.SelectedIndex = -1;
            Vode.SelectedIndex = -1;
            count.Clear();
            sum.Clear();
            email.Clear();
            info.Clear();
            imageBook.Source = null;
            vivodID();
            VobrPokaz();
        }

        private void Bsohr_Click(object sender, RoutedEventArgs e)
        {
            if (CSzak.SelectedIndex == -1)
            {
                MessageBox.Show("Вы не выбрали код заказа!");
            }
            else
            {
                if (Sur.Text == "" || Name.Text == "" || FIO.Text == "" || phone.Text == "" || date.SelectedDate == null || Vobr.SelectedIndex == -1 || Vode.SelectedIndex == -1 || count.Text == null || sum.Text == "" || info.Text == "" || email.Text == "")
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
                        string saveVobr = Convert.ToString(save1.ExecuteScalar());

                        string save2 = "SELECT B_ID FROM Books WHERE B_NAME ='" + Vode.SelectedItem + "'";
                        SqlCommand save3 = new SqlCommand(save2, savezak);
                        string saveVode = Convert.ToString(save3.ExecuteScalar());

                        string save4 = "select CU_ID from Customer inner join Orderr on O_CU = CU_ID where O_ID ='" + CSzak.SelectedItem + "'";
                        SqlCommand saveklient = new SqlCommand(save4, savezak);
                        string save5 = Convert.ToString(saveklient.ExecuteScalar());

                        string savezakaz = "Update Orderr set T_ID = '" + saveVobr + "', B_ID = '" + saveVode + "', O_TELEPHONE = '" + phone.Text + "', O_COUNT = '" + count.Text + "', O_DATE = '" + date.SelectedDate + "', O_SUM = '" + sum.Text + "' where O_ID =" + CSzak.SelectedItem;
                        SqlCommand sz = new SqlCommand(savezakaz, savezak); sz.ExecuteNonQuery();

                        string saveklient2 = "Update Customer set CU_Surname = '" + Sur.Text + "', CU_Name = '" + Name.Text + "', CU_Partonymic = '" + FIO.Text + "', CU_TELEPHONE = '" + phone.Text + "', CU_EMAIL = '" + email.Text + "' where CU_ID =" + save5;
                        SqlCommand sk = new SqlCommand(saveklient2, savezak); sk.ExecuteNonQuery();

                        savezak.Close();
                        CSzak.SelectedIndex = -1;
                        Vobr.SelectedIndex = -1;
                        Vode.SelectedIndex = -1;
                        imageBook.Source = null;
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
                MessageBox.Show("Вы не выбрали код заказа!");
            }
            else
            {
                string connectionString = ClassSql.GetConnSQL();
                string query = "DELETE FROM Orderr WHERE O_ID = @OrderId";

                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@OrderId", Convert.ToString(CSzak.SelectedItem));

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        CSzak.SelectedIndex = -1;
                        Vobr.SelectedIndex = -1;
                        Vode.SelectedIndex = -1;
                        imageBook.Source = null;
                        vivodID();
                        MessageBox.Show("Заказ удален");
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("SQL Error: " + ex.Message);
                    }
                }
            }
        }

        private void Vobr_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Vode.IsEnabled = true;
            Vode.Items.Clear();
            string connectionString = ClassSql.GetConnSQL();
            string query = "SELECT B_NAME FROM TypeBooks INNER JOIN Books ON Books.B_TITLE = TypeBooks.T_ID WHERE T_TITLE = @Title";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Title", Convert.ToString(Vobr.SelectedItem));

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string bookName = reader.GetString(0);
                        if (!Vode.Items.Contains(bookName))
                        {
                            Vode.Items.Add(bookName);
                        }
                    }
                }
            }
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

        private void Vode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Vode.SelectedValue != null)
            {
                string selectedBookName = Vode.SelectedValue.ToString();
                int selectedBookId = GetBookIdByName(selectedBookName);
                LoadBookImage(selectedBookId);
                LoadBookInfo(selectedBookId);
                sum.Clear();
            }
        }
        private void LoadBookImage(int bookId)
        {
            string connectionString = ClassSql.GetConnSQL();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("SELECT B_IMAGE FROM Books WHERE B_ID = @BookId", connection);
                    command.Parameters.AddWithValue("@BookId", bookId);
                    string imageUrl = command.ExecuteScalar()?.ToString();

                    if (!string.IsNullOrEmpty(imageUrl))
                    {
                        string basePath = AppDomain.CurrentDomain.BaseDirectory;
                        string imagePath = System.IO.Path.Combine(basePath, imageUrl);
                        imageBook.Source = new BitmapImage(new Uri(imagePath));
                    }
                    else
                    {
                        MessageBox.Show("Изображение не найдено для выбранной книги.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка загрузки изображения: " + ex.Message);
                }
            }
        }
        private void LoadBookInfo(int bookId)
        {
            string connectionString = ClassSql.GetConnSQL();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("SELECT B_INFO FROM Books WHERE B_ID = @BookId", connection);
                    command.Parameters.AddWithValue("@BookId", bookId);
                    string bookInfo = command.ExecuteScalar()?.ToString();

                    if (!string.IsNullOrEmpty(bookInfo))
                    {
                        info.Text = bookInfo;
                    }
                    else
                    {
                        MessageBox.Show("Описание не найдено для выбранной книги.");
                        info.Clear();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка загрузки описания: " + ex.Message);
                }
            }
        }
        private int GetBookIdByName(string bookName)
        {
            int bookId = 0;
            string connectionString = ClassSql.GetConnSQL();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("SELECT B_ID FROM Books WHERE B_NAME = @BookName", connection);
                    command.Parameters.AddWithValue("@BookName", bookName);
                    object result = command.ExecuteScalar();
                    if (result != null)
                    {
                        bookId = Convert.ToInt32(result);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка получения ID книги: " + ex.Message);
                }
            }
            return bookId;
        }

        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result =  MessageBox.Show("Это окно Кассира! Здесь вы можете оформлять заказы для клиентов. Так же вы имеете доступ к просмотру всех созданных заказов" +
                " и редактировать их. Будте внимательны к заполнению всех данных!", "О программе", MessageBoxButton.OK, MessageBoxImage.Information);
            if (result == MessageBoxResult.OK)
            {
                MessageBox.Show("Не забудьте удостовериться в корректности данных клиента! (номер телефона, адрес электронной почты).", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
