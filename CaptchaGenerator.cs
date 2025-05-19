using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

public class CaptchaGenerator
{
    private const string Chars = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnpqrstuvwxyz23456789";
    private static readonly Random Random = new Random();

    public string CurrentCaptchaText { get; private set; }

    public void GenerateCaptcha(Canvas canvas)
    {
        // Очищаем canvas
        canvas.Children.Clear();

        // Генерируем случайный текст
        CurrentCaptchaText = GenerateRandomText(6);

        // Рисуем текст с искажениями
        DrawText(canvas, CurrentCaptchaText);

        // Добавляем шум
        AddNoise(canvas, (int)canvas.ActualWidth, (int)canvas.ActualHeight);
    }

    private string GenerateRandomText(int length)
    {
        var chars = new char[length];
        for (int i = 0; i < length; i++)
        {
            chars[i] = Chars[Random.Next(Chars.Length)];
        }
        return new string(chars);
    }

    private void DrawText(Canvas canvas, string text)
    {
        for (int i = 0; i < text.Length; i++)
        {
            var textBlock = new TextBlock
            {
                Text = text[i].ToString(),
                FontSize = Random.Next(20, 30),
                RenderTransform = new TransformGroup
                {
                    Children = new TransformCollection
                    {
                        new RotateTransform(Random.Next(-15, 15)),
                        new TranslateTransform(Random.Next(-2, 2), Random.Next(-2, 2))
                    }
                },
                Foreground = new SolidColorBrush(Color.FromRgb(
                    (byte)Random.Next(100, 200),
                    (byte)Random.Next(100, 200),
                    (byte)Random.Next(100, 200)))
            };

            Canvas.SetLeft(textBlock, 10 + i * 25 + Random.Next(-5, 5));
            Canvas.SetTop(textBlock, 5 + Random.Next(-5, 5));

            canvas.Children.Add(textBlock);
        }
    }

    private void AddNoise(Canvas canvas, int width, int height)
    {
        // Линии
        for (int i = 0; i < 5; i++)
        {
            var line = new Line
            {
                X1 = Random.Next(0, width),
                Y1 = Random.Next(0, height),
                X2 = Random.Next(0, width),
                Y2 = Random.Next(0, height),
                StrokeThickness = Random.Next(1, 3),
                Stroke = new SolidColorBrush(Color.FromRgb(
                    (byte)Random.Next(100, 200),
                    (byte)Random.Next(100, 200),
                    (byte)Random.Next(100, 200)))
            };
            canvas.Children.Add(line);
        }

        // Точки
        for (int i = 0; i < 30; i++)
        {
            var ellipse = new Ellipse
            {
                Width = Random.Next(1, 3),
                Height = Random.Next(1, 3),
                Fill = new SolidColorBrush(Color.FromRgb(
                    (byte)Random.Next(100, 200),
                    (byte)Random.Next(100, 200),
                    (byte)Random.Next(100, 200)))
            };

            Canvas.SetLeft(ellipse, Random.Next(0, width));
            Canvas.SetTop(ellipse, Random.Next(0, height));

            canvas.Children.Add(ellipse);
        }
    }
}