using BarcodeStandard;
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

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void HoldButton_PreviewMouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            txtPassword.Text = pboxPassword.Password;
            pboxPassword.Visibility = Visibility.Collapsed;
        }
        private void HoldButton_PreviewMouseLeftButtonUp(object sender, RoutedEventArgs e)
        {
            txtPassword.Text = null;
            pboxPassword.Visibility = Visibility.Visible;
        }
        public void btnAuth_Click(object sender, RoutedEventArgs e)
        {
           
            
            using (var db = new PM02ZhuykovAEntities())
            {
                var usern = db.Пользователи.FirstOrDefault(uch => uch.Логин == txtLogin.Text && uch.Пароль == pboxPassword.Password);
                if (usern == null) MessageBox.Show("Неверно введен логин или пароль");
                else
                {
                    MessageBox.Show("Добро пожаловать" + ", " + usern.Имя + "!");
                    labo.id = usern.ID_Роли;
                    labo.name = usern.Имя;
                    labo.login = usern.Логин;
                    labo.IP_Adress = usern.IP_Адрес;
                    labo.lastEnter = Convert.ToDateTime(usern.Последний_вход);
                    labo.services = usern.Услуги;
                    labo.type = usern.Тип;
                    labo.ID_Role = usern.ID_Роли;
                    
                    captchaWindow asd = new captchaWindow();
                    asd.Show();
                    this.Close();

                }



            }

        }
    }
}
