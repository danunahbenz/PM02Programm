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

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для captchaWindow.xaml
    /// </summary>
    public partial class captchaWindow : Window
    {
        private readonly CaptchaGenerator _captchaGenerator = new CaptchaGenerator();

        public captchaWindow()
        {
            InitializeComponent();
            GenerateNewCaptcha();
        }
        private void GenerateNewCaptcha()
        {
            _captchaGenerator.GenerateCaptcha(CaptchaCanvas);
            CaptchaInput.Clear();
            ResultText.Text = string.Empty;
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            GenerateNewCaptcha();
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (CaptchaInput.Text.Equals(_captchaGenerator.CurrentCaptchaText, StringComparison.OrdinalIgnoreCase))
            {
                ResultText.Text = "CAPTCHA пройдена!";
                ResultText.Foreground = Brushes.Green;
                if (labo.ID_Role == 1)
                {
                    laborant laborant = new laborant();
                    laborant.Show();
                    this.Close();
                }
                if (labo.ID_Role == 2)
                {
                    labissledovatel labissledovatel = new labissledovatel();
                    labissledovatel.Show();
                    this.Close();
                }
                if (labo.ID_Role == 3)
                {
                    buh buh = new buh();
                    buh.Show();
                    this.Close();
                }
                if (labo.ID_Role == 4)
                {
                    admin admin = new admin();
                    admin.Show();
                    laborant laborant = new laborant();
                    laborant.Show();
                    buh buh = new buh();
                    buh.Show();
                    labissledovatel labissledovatel = new labissledovatel();
                    labissledovatel.Show();

                    this.Close();
                }
            }
            else
            {
                ResultText.Text = "Неверно, попробуйте снова";
                ResultText.Foreground = Brushes.Red;
                GenerateNewCaptcha();
            }

        }
    }
}
