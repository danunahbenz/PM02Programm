using CsvHelper;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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
using Microsoft.Win32;
using CsvHelper.Configuration;
using System.Windows.Markup;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Windows;
using Microsoft.Win32;
namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для buh.xaml
    /// </summary>
    public partial class buh : Window
    {
        public buh()
        {
            InitializeComponent();
            Title = "Бухгалтер" + " " + labo.name;
            
        }

        public void ExportTextBoxesSimple(TextBox textBox1, TextBox textBox2, TextBox textBox3, TextBox textBox4, TextBox textBox5)
        {

            var records = new List<dynamic>
    {
        new {
            Название = textBox1.Text,
            ФИО = textBox2.Text,
            Услуга = textBox3.Text,
            СтоимостьУслуги = textBox4.Text,
            ИтоговаяСтоимость = textBox5.Text,
            
        }
    };

            
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv",
                DefaultExt = "csv",
                Title = "Export TextBox values to CSV"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    BaseFont baseFont = BaseFont.CreateFont(FONT_PATH, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                    Font font = new Font(baseFont, 12, Font.NORMAL);
                    var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                    {
                        Delimiter = ";", // Используйте "," если нужно
                        Encoding = Encoding.UTF8,
                        HasHeaderRecord = true
                    };

                    using (var writer = new StreamWriter(saveFileDialog.FileName, false, Encoding.UTF8))
                    using (var csv = new CsvWriter(writer, config))
                    {
                        
                        csv.WriteRecords(records);
                    }

                    MessageBox.Show("Data exported successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error exporting data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            using (var writer = new StreamWriter(saveFileDialog.FileName))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(records);
            }
        }
        private static readonly string FONT_PATH = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf");
        public void ExportTextBoxesToPdf(TextBox textBox1, TextBox textBox2, TextBox textBox3, TextBox textBox4, TextBox textBox5)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "PDF files (*.pdf)|*.pdf",
                DefaultExt = "pdf",
                Title = "Export to PDF"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                
                try
                {
                    BaseFont baseFont = BaseFont.CreateFont(FONT_PATH, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                    Font font = new Font(baseFont, 12, Font.NORMAL);
                    // Создаем документ
                    Document document = new Document();
                    PdfWriter.GetInstance(document, new FileStream(saveFileDialog.FileName, FileMode.Create));

                    document.Open();

                    // Добавляем содержимое
                    document.Add(new iTextSharp.text.Paragraph("", font));
                    document.Add(new iTextSharp.text.Paragraph(" ")); // Пустая строка

                    // Добавляем значения из TextBox
                    document.Add(new iTextSharp.text.Paragraph($"Название страховой компании: {textBox1.Text}", font));
                    document.Add(new iTextSharp.text.Paragraph($"ФИО пациента: {textBox2.Text}", font));
                    document.Add(new iTextSharp.text.Paragraph($"Услуга: {textBox3.Text}", font));
                    document.Add(new iTextSharp.text.Paragraph($"Стоимость услуги: {textBox4.Text}", font));
                    document.Add(new iTextSharp.text.Paragraph($"Итоговая стоимость: {textBox5.Text}", font));
                    

                    document.Close();

                    MessageBox.Show("Данные успешно экспортированы в PDF!", "Успех",
                                  MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при экспорте: {ex.Message}", "Ошибка",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }



        private void txt_exportCSV_Click(object sender, RoutedEventArgs e)
        {
            ExportTextBoxesSimple(txt_Nazvanie, txt_FIO, txt_Usluga, txt_stoimost, txt_itogStoimost);
        }

        private void btn_exportPDF_Click(object sender, RoutedEventArgs e)
        {
            ExportTextBoxesToPdf(txt_Nazvanie, txt_FIO, txt_Usluga, txt_stoimost, txt_itogStoimost);
        }
    }
}
