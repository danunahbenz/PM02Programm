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
    /// Логика взаимодействия для biomaterial.xaml
    /// </summary>
    public partial class biomaterial : Window
    {
        private string connectionString = "Server = HOME-PC\\SQLEXPRESS01; DataBase = PM02ZhuykovA; Trusted_Connection = True";
        public biomaterial()
        {
            InitializeComponent();
            LoadData();
        }
        private void LoadData()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT * FROM Биоматериал", connection);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);
                    dataGrid.ItemsSource = dataTable.DefaultView;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                }
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new PM02ZhuykovAEntities())
            {
                var biomaterial = new Биоматериал();
                biomaterial.Штрих_код = txt_barCode.Text;
                biomaterial.Дата_принятия = Convert.ToDateTime(dpic_receptionDate.SelectedDate);
                biomaterial.id_пользователя = Convert.ToInt32(labo.id);
                biomaterial.id_пациента = Convert.ToInt32(txt_patientID.Text);
                try
                {
                    db.Биоматериал.Add(biomaterial);
                    db.SaveChanges();
                    LoadData();
                    MessageBox.Show("Вы успешно добавили");

                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }

        }

        private void btn_Nazad_Click(object sender, RoutedEventArgs e)
        {
            laborant a = new laborant();
            a.Show();
            this.Close();
        }
    }
}
