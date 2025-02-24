using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPB_11
{
    public class roundedTextBox : UserControl
    {
        private Label label;
        private TextBox textBox;
        private string placeholderText;

        public roundedTextBox(string labelText)
        {
            // Инициализация элементов
            label = new Label
            {
                Text = labelText,
                AutoSize = true,
                ForeColor = Color.Black,
                BackColor = Color.Transparent // Делаем фон метки прозрачным
            };

            textBox = new TransparentTextBox
            {
                Width = 140,
                Multiline = true, // Позволяем многострочный ввод
                BorderStyle = BorderStyle.None, // Убираем стандартную рамку
                Height = 50, // Устанавливаем высоту текстового поля
                TextAlign = HorizontalAlignment.Left,
                MaxLength = 100, // Ограничиваем длину текста
                BackColor = Color.Transparent
            };

            // Настройка внешнего вида
            this.BackColor = Color.White; // Цвет фона
            this.Padding = new Padding(5);
            this.Margin = new Padding(15);
            this.Controls.Add(label);
            this.Controls.Add(textBox);

            // Установка позиции текстового поля под меткой
            textBox.Location = new Point(0, label.Height + 5);

            // Установка обработчика для рисования обводки
            this.Paint += RoundedTextBox_Paint;

            // Установка обработчиков событий для управления подсказкой
            textBox.Enter += (s, e) =>
            {
                if (textBox.Text == placeholderText)
                {
                    textBox.Text = "";
                    textBox.ForeColor = Color.Black; // Возвращаем черный цвет при вводе
                }
            };

            textBox.Leave += (s, e) =>
            {
                if (string.IsNullOrEmpty(textBox.Text))
                {
                    textBox.Text = placeholderText;
                    textBox.ForeColor = Color.Gray; // Устанавливаем цвет подсказки
                }
            };
        }

        private void RoundedTextBox_Paint(object sender, PaintEventArgs e)
        {
            // Уменьшаем размеры рамки
            int padding = 0; // Отступ для рамки
            Rectangle textBoxBounds = new Rectangle(
                padding, // Сдвигаем вправо
                label.Height + 5 + padding, // Сдвигаем вниз
                this.Width - 2 * padding, // Уменьшаем ширину
                textBox.Height + 10 // Высота остается прежней
            );

            // Рисуем закругленные углы
            int radius = 10; // Радиус закругления
            using (GraphicsPath path = new GraphicsPath())
            {
                path.StartFigure();
                path.AddArc(textBoxBounds.X, textBoxBounds.Y, radius+100, radius, 180, 90);
                path.AddArc(textBoxBounds.Right - radius, textBoxBounds.Y, radius+ 100, radius, 270, 90);
                path.AddArc(textBoxBounds.Right - radius, textBoxBounds.Bottom - radius, radius, radius, 0, 90);
                path.AddArc(textBoxBounds.X, textBoxBounds.Bottom - radius, radius, radius, 90, 90);
                path.CloseFigure();

                e.Graphics.DrawPath(new Pen(Color.FromArgb(217, 217, 217), 2), path); // Рисуем обводку вокруг текстового поля
            }
        }


        public string PlaceholderText
        {
            get => placeholderText;
            set
            {
                placeholderText = value;
                textBox.Text = placeholderText;
                textBox.ForeColor = Color.Gray; // Устанавливаем цвет подсказки
            }
        }

        public string Text
        {
            get => textBox.Text == placeholderText ? string.Empty : textBox.Text;
            set
            {
                textBox.Text = value;
                if (string.IsNullOrEmpty(value))
                {
                    textBox.Text = placeholderText;
                    textBox.ForeColor = Color.Gray; // Устанавливаем цвет подсказки
                }
                else
                {
                    textBox.ForeColor = Color.Black; // Устанавливаем цвет текста
                }
            }
        }
    }
}
