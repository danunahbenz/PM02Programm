using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для admin.xaml
    /// </summary>
    public partial class admin : Window
    {
        private string connectionString = "Server = HOME-PC\\SQLEXPRESS01; DataBase = PM02ZhuykovA; Trusted_Connection = True";

        public admin()
        {
            InitializeComponent();
            LoadData();
        }
        private void UpdateRecord(int id, string newValue)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE Пациенты SET [Логин] = @newValue WHERE Id_пациента = @id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@newValue", newValue);
                command.Parameters.AddWithValue("@id", id);

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        LoadData();
                        MessageBox.Show("Запись успешно обновлена.");
                    }
                    else
                    {
                        MessageBox.Show("Запись не найдена.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                }
            }
        }
        private void LoadData()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT * FROM Пациенты", connection);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);
                    dataGrid.ItemsSource = dataTable.DefaultView;
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                }
            }
        }

        private void btn_refresh_Click(object sender, RoutedEventArgs e)
        {
            int id;
            if (int.TryParse(txt_ID.Text, out id))
            {
                string newValue = txt_Login.Text;
                UpdateRecord(id, newValue);
            }
            else
            {
                MessageBox.Show("Введите корректный ID.");
            }
        }
    }
}
