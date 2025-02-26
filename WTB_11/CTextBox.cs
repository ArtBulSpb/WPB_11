using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPB_11
{
    public class CTextBox : UserControl // Наследуем от UserControl
    {
        private Label label;
        private TextBox textBox;
        private Button increaseButton;
        private Button decreaseButton;
        private string placeholderText;

        public CTextBox(string labelText)
        {
            // Инициализация компонентов
            label = new Label
            {
                Text = labelText,
                AutoSize = true,
                Location = new Point(5, 5),
                Font = FontManager.GetSemiBoldFont(10), // Устанавливаем // Положение лейбла
                BackColor = Color.Transparent // Прозрачный фон для лейбла
            };

            textBox = new TextBox
            {
                Width = 120,
                Multiline = true, // Позволяем многострочный ввод
                BorderStyle = BorderStyle.None, // Убираем стандартную рамку
                Height = 50, // Устанавливаем высоту текстового поля
                TextAlign = HorizontalAlignment.Left,
                Location = new Point(10, label.Height + 5),
                MaxLength = 100, // Ограничиваем длину текста
                Font = FontManager.GetRegularFont(10),
            };

            increaseButton = new Button
            {
                Text = "+",
                Size = new Size(20, 20), // Размер кнопки для удобства
                Location = new Point(textBox.Right + 25, textBox.Top) // Положение кнопки увеличения
            };
            increaseButton.FlatStyle = FlatStyle.Flat; // Убираем стандартное выделение

            increaseButton.Click += IncreaseButton_Click;

            decreaseButton = new Button
            {
                Text = "-",
                Size = new Size(20, 20), // Размер кнопки для удобства
                Location = new Point(textBox.Right + 25, increaseButton.Bottom + 5) // Положение кнопки уменьшения под кнопкой увеличения
            };
            decreaseButton.FlatStyle = FlatStyle.Flat; // Убираем стандартное выделение

            decreaseButton.Click += DecreaseButton_Click;

            // Устанавливаем размер UserControl с учетом обводки
            this.Size = new Size(textBox.Width + increaseButton.Width + 60, decreaseButton.Bottom + 25); // Увеличиваем размер контрола

            // Добавляем компоненты в UserControl
            this.Controls.Add(textBox);
            this.Controls.Add(increaseButton);
            this.Controls.Add(decreaseButton);
            // Добавляем лейбл последним, чтобы он был поверх других элементов
            this.Controls.Add(label);

            // Устанавливаем стиль скругленных краев
            this.Paint += CTextBox_Paint;

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

        private void CTextBox_Paint(object sender, PaintEventArgs e)
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


        private void IncreaseButton_Click(object sender, EventArgs e)
        {
            if (double.TryParse(textBox.Text, out double coefficient))
            {
                coefficient += 1; // Увеличиваем коэффициент на 1
                textBox.Text = coefficient.ToString(); // Обновляем текстовое поле
            }
        }

        private void DecreaseButton_Click(object sender, EventArgs e)
        {
            if (double.TryParse(textBox.Text, out double coefficient))
            {
                coefficient -= 1; // Уменьшаем коэффициент на 1
                textBox.Text = coefficient.ToString(); // Обновляем текстовое поле
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