using BarcodeStandard;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Drawing;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using SkiaSharp;
using System.Data.SqlClient;
using System.Linq;
using System.Data.Entity.Infrastructure;
using SkiaSharp;
namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для laborant.xaml
    /// </summary>
    public partial class laborant : Window
    {
        private DispatcherTimer sessionTimer;
        private DispatcherTimer lockoutTimer;
        private TimeSpan sessionDuration = TimeSpan.FromMinutes(150); // 10 минут для тестирования (в реальности 2.5 часа)
        private TimeSpan warningTime = TimeSpan.FromMinutes(15); // Предупреждение за 5 минут (в реальности 15)
        private TimeSpan lockoutDuration = TimeSpan.FromMinutes(30); // Блокировка на 1 минуту (в реальности 30)
        private DateTime sessionStartTime;
        private bool isLocked = false;
        public laborant()
        {
            InitializeComponent();
            InitializeTimers();
            Title = "Лаборант" + " " + labo.name;
            LoadImage(1, img_Photo);
        }
        private void InitializeTimers()
        {

            // Таймер для отслеживания сеанса
            sessionTimer = new DispatcherTimer();
            sessionTimer.Interval = TimeSpan.FromSeconds(1);
            sessionTimer.Tick += SessionTimer_Tick;

            // Таймер для блокировки входа
            lockoutTimer = new DispatcherTimer();
            lockoutTimer.Interval = lockoutDuration;
            lockoutTimer.Tick += LockoutTimer_Tick;
            lockoutTimer.IsEnabled = false;
        }

        private void Window_Loaded(object sender, EventArgs e)
        {

            sessionStartTime = DateTime.Now;
            sessionTimer.Start();
            UpdateTimerDisplay();

        }
        private void SessionTimer_Tick(object sender, EventArgs e)
        {
            var elapsed = DateTime.Now - sessionStartTime;
            var remaining = sessionDuration - elapsed;

            UpdateTimerDisplay(remaining);

            // Проверка на предупреждение
            if (remaining <= warningTime && remaining > TimeSpan.Zero)
            {
                if (!WarningMessage.IsVisible)
                {
                    WarningMessage.Visibility = Visibility.Visible;
                    MessageBox.Show($"Внимание! До окончания сеанса осталось {warningTime.TotalMinutes} минут.");
                }
            }

            // Проверка на окончание сеанса
            if (remaining <= TimeSpan.Zero)
            {
                EndSession();
            }
        }
        private void UpdateTimerDisplay(TimeSpan? remaining = null)
        {
            if (remaining == null)
            {
                TimerDisplay.Text = sessionDuration.ToString(@"hh\:mm");
            }
            else
            {
                TimerDisplay.Text = remaining.Value.ToString(@"hh\:mm");
            }
        }
        private void EndSession()
        {
            sessionTimer.Stop();
            MessageBox.Show("Время сеанса истекло. Выполняется выход из системы.");

            // Блокировка входа
            isLocked = true;
            lockoutTimer.Start();

            // Выход из системы

            WarningMessage.Visibility = Visibility.Collapsed;
        }
        private void LockoutTimer_Tick(object sender, EventArgs e)
        {
            // Разблокировка входа после истечения времени блокировки
            isLocked = false;
            lockoutTimer.Stop();
        }

        public BitmapSource GenerateBarcode(string text)
        {
            var barcode = new Barcode
            {
                IncludeLabel = true,
                Width = 300,
                Height = 100
            };
            using (var image = barcode.Encode(BarcodeStandard.Type.Code128, text))
            using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
            using (var stream = new MemoryStream(data.ToArray()))
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.StreamSource = stream;
                bitmap.EndInit();
                return bitmap;
            }
        }

        public void btn_barcodeGenerate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string textToEndcode = txt_barcode.Text;
                if (!string.IsNullOrEmpty(textToEndcode))
                {
                    BarcodeImage.Source = GenerateBarcode(textToEndcode);
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void btn_biomaterial_Click(object sender, RoutedEventArgs e)
        {
            biomaterial biomaterial = new biomaterial();
            biomaterial.Show();
            this.Close();
        }
        public byte[] GetImageFromDatabase(int imageId)
        {
            byte[] imageData = null;
            string connectionString = "Server = home-pc\\sqlexpress01;database = PM02ZhuykovA; Trusted_Connection=true";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = $"SELECT Фото From Пользователи where ID_пользователя = {labo.id}";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID_пользователя", imageId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            imageData = (byte[])reader["Фото"];
                        }

                    }
                }
            }
            return imageData;
        }
        public void LoadImage(int imageId, Image imageControl)
        {
            byte[] imageData = GetImageFromDatabase(imageId);
            if (imageData != null)
            {
                BitmapImage bitmapImage = ConvertByteArrayToImage(imageData);
                imageControl.Source = bitmapImage;
            }
        }
        public BitmapImage ConvertByteArrayToImage(byte[] imageData)
        {
            using (MemoryStream ms = new MemoryStream(imageData))
            {
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = ms;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.EndInit();
                image.Freeze();
                return image;
            }

        }

        private void btn_patientAdd_Click(object sender, RoutedEventArgs e)
        {
            addPatient addPatient = new addPatient();
            addPatient.Show();
            this.Close();
        }
    }
}