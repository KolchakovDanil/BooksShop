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
    /// Логика взаимодействия для addTypeBooks.xaml
    /// </summary>
    public partial class addTypeBooks : Window
    {
        public addTypeBooks()
        {
            InitializeComponent();
            vivodID();
        }
        private void inRus(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^а-яА-Я]");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("В этом окне осуществляется изменение, добавление и удаления категории книг. " +
                "Нажмите 'Добавить' для создания новой категории. Важно! Для отображения добавленой категории нажмите на кнопку 'Обновить' (на главном окне).", "О программе", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        public void vivodID()
        {
            CSzak.Items.Clear();
            string connectionString = ClassSql.GetConnSQL();

            string query = "SELECT T_ID FROM TypeBooks";

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
        private void CSzak_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string connectionString = ClassSql.GetConnSQL();
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            string sql = "select T_TITLE from TypeBooks where T_ID  = '" + CSzak.SelectedItem + "'";
            SqlCommand Sqlcmd = new SqlCommand(sql, conn);
            name.Text = Convert.ToString(Sqlcmd.ExecuteScalar());

            conn.Close();
        }
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (name.Text == "")
            {
                MessageBox.Show("Название категории книги не должно быть пустым!");
            }
            else
            {
                CSzak.SelectedIndex = -1;
                string connectionString = ClassSql.GetConnSQL();
                SqlConnection conn6 = new SqlConnection(connectionString);
                try
                {
                    conn6.Open();

                    string add = "Insert into TypeBooks (T_TITLE) values ('" + name.Text + "')";
                    SqlCommand addTB = new SqlCommand(add, conn6); addTB.ExecuteNonQuery();

                    conn6.Close();
                    MessageBox.Show("Категория успешно добавлена");
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
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            CSzak.SelectedIndex = -1;
            name.Text = "";
            btnAdd.Visibility = Visibility.Hidden;
            CSzak.Visibility = Visibility.Visible;
            TIDzak.Visibility = Visibility.Visible;
            btnSave.Visibility = Visibility.Visible;
            btnDel.Visibility = Visibility.Visible;
            btnNowAdd.Visibility = Visibility.Visible;
            vivodID();
        }
        private void btnNowAdd_Click(object sender, RoutedEventArgs e)
        {
            CSzak.SelectedIndex = -1;
            name.Text = "";
            btnAdd.Visibility = Visibility.Visible;
            CSzak.Visibility = Visibility.Hidden;
            TIDzak.Visibility = Visibility.Hidden;
            btnSave.Visibility = Visibility.Hidden;
            btnDel.Visibility = Visibility.Hidden;
            btnNowAdd.Visibility = Visibility.Hidden;
            vivodID();
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (CSzak.SelectedIndex == -1)
            {
                MessageBox.Show("Вы не выбрали код категории!");
            }
            else
            {
                if (name.Text == "")
                {
                    MessageBox.Show("Название категории книги не должно быть пустым!");
                }
                else
                {
                    string connectionString = ClassSql.GetConnSQL();
                    SqlConnection saveType = new SqlConnection(connectionString);
                    try
                    {
                        saveType.Open();
                        string savezakaz = "Update TypeBooks set T_TITLE = '" + name.Text + "' where T_ID =" + CSzak.SelectedItem;
                        SqlCommand sz = new SqlCommand(savezakaz, saveType); sz.ExecuteNonQuery();

                        saveType.Close();
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
        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            if (CSzak.SelectedIndex == -1)
            {
                MessageBox.Show("Вы не выбрали код категории!");
            }
            else
            {
                string connectionString = ClassSql.GetConnSQL();

                string checkBooksQuery = "SELECT COUNT(*) FROM Books WHERE B_TITLE = @TypeBooksId";

                string deleteCategoryQuery = "DELETE FROM TypeBooks WHERE T_ID = @TypeBooksId";

                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        using (SqlCommand command = new SqlCommand(checkBooksQuery, connection))
                        {
                            command.Parameters.AddWithValue("@TypeBooksId", Convert.ToString(CSzak.SelectedItem));

                            int booksCount = (int)command.ExecuteScalar();
                            if (booksCount > 0)
                            {
                                MessageBox.Show("Нельзя удалить категорию с заполненными данными!");
                            }
                            else
                            {
                                using (SqlCommand deleteCommand = new SqlCommand(deleteCategoryQuery, connection))
                                {
                                    deleteCommand.Parameters.AddWithValue("@TypeBooksId", Convert.ToString(CSzak.SelectedItem));

                                    int rowsAffected = deleteCommand.ExecuteNonQuery();
                                    if (rowsAffected > 0)
                                    {
                                        CSzak.SelectedIndex = -1;
                                        vivodID();
                                        MessageBox.Show("Категория успешно удалена");
                                    }
                                    else
                                    {
                                        MessageBox.Show("Категория не была удалена. Возможно, указанный идентификатор категории не существует.");
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("SQL Error: " + ex.Message);
                }
            }
        }
    }
}
