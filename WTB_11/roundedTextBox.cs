using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Text;

namespace WPB_11
{
    public class roundedTextBox : UserControl
    {
        public Label label;
        public TextBox textBox;
        private string placeholderText;

        public roundedTextBox(string labelText)
        {

            // Инициализация элементов
            label = new Label
            {
                Text = labelText,
                AutoSize = true,
                Location = new Point(10, 5),
                ForeColor = Color.Black,
                BackColor = Color.Transparent, // Делаем фон метки прозрачным
                Font = FontManager.GetSemiBoldFont(10), // Устанавливаем 
            };

            textBox = new TransparentTextBox
            {
                Width = 120,
                Multiline = true, // Позволяем многострочный ввод
                BorderStyle = BorderStyle.None, // Убираем стандартную рамку
                Height = 50, // Устанавливаем высоту текстового поля
                TextAlign = HorizontalAlignment.Left,
                Location = new Point(10, label.Height + 5),
                MaxLength = 100, // Ограничиваем длину текста
                BackColor = Color.Transparent,
                Font = FontManager.GetRegularFont(10),
            };

            // Настройка внешнего вида
            this.BackColor = Color.White; // Цвет фона
            this.Height = 90;
            this.Width = 200;
            this.Controls.Add(label);
            this.Controls.Add(textBox);


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
            int padding = 3; // Отступ для границы

            // Рисуем фон
            e.Graphics.FillRectangle(new SolidBrush(Color.White), this.ClientRectangle);

            // Рисуем скругленные края с отступами
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddArc(padding, padding, 10, 10, 180, 90); // Левый верхний угол
            path.AddArc(this.Width - 10 - padding, padding, 10, 10, 270, 90); // Правый верхний угол
            path.AddArc(this.Width - 10 - padding, this.Height - 10 - padding, 10, 10, 0, 90); // Правый нижний угол
            path.AddArc(padding, this.Height - 10 - padding, 10, 10, 90, 90); // Левый нижний угол
            path.CloseFigure();

            e.Graphics.DrawPath(new Pen(Color.Gray), path); // Рисуем границу с отступами
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
