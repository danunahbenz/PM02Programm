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
    /// Логика взаимодействия для addPatient.xaml
    /// </summary>
    public partial class addPatient : Window
    {
        private string connectionString = "Server = HOME-PC\\SQLEXPRESS01; DataBase = PM02ZhuykovA; Trusted_Connection = True";
        public addPatient()
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
                    SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT * FROM Пациенты", connection);
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
                var patient = new Пациенты();
                patient.Логин = txt_Login.Text;
                patient.Пароль = txt_Password.Text;
                patient.ФИО = txt_FIO.Text;
                patient.ДатаРождения = Convert.ToDateTime(dpic_DataRojdeniya.Text);
                patient.СерияПаспорта = txt_Seriya.Text;
                patient.НомерПаспорта = txt_Nomer.Text;
                patient.Телефон = txt_Telefon.Text;
                patient.Email = txt_Email.Text;
                patient.НомерПолиса = txt_NomerPolisa.Text;
                patient.ТипПолиса = txt_TipPolisa.Text;
                try
                {
                    db.Пациенты.Add(patient);
                    db.SaveChanges();
                    LoadData();
                    MessageBox.Show("Вы успешно добавили");

                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
        }

        private void btn_nazad_Click(object sender, RoutedEventArgs e)
        {
            laborant a = new laborant();
            a.Show();
            this.Close();
        }
    }
}
