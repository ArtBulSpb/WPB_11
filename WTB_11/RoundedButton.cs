using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPB_11
{
    public class RoundedButton : Control
    {
        private string leftText = "Обновить";
        private string rightText = "Печать";
        private int dividerWidth = 2;

        private Color defaultColor = Color.White; // Стандартный цвет фона
        private Color pressedColor = Color.FromArgb(224, 224, 224); // Цвет фона при нажатии
        private Color leftColor; // Цвет левой части
        private Color rightColor; // Цвет правой части

        public event EventHandler LeftButtonClick;
        public event EventHandler RightButtonClick;

        public string LeftText
        {
            get => leftText;
            set { leftText = value; Invalidate(); }
        }

        public string RightText
        {
            get => rightText;
            set { rightText = value; Invalidate(); }
        }

        // Используем кастомные шрифты из FontManager
        public Font LeftButtonFont { get; set; } = FontManager.GetMediumFont(12); // Замените "LeftFont" на нужный вам шрифт
        public Font RightButtonFont { get; set; } = FontManager.GetMediumFont(12); // Замените "RightFont" на нужный вам шрифт

        public RoundedButton()
        {
            leftColor = defaultColor; // Устанавливаем начальный цвет для левой части
            rightColor = defaultColor; // Устанавливаем начальный цвет для правой части
            this.Size = new Size(200, 50); // Задаем размеры по умолчанию
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);
            Graphics g = pevent.Graphics;

            // Создаем округлую форму
            GraphicsPath path = new GraphicsPath();
            path.AddArc(0, 0, 20, 20, 180, 90); // Левый верхний угол
            path.AddArc(this.Width - 20, 0, 20, 20, 270, 90); // Правый верхний угол
            path.AddArc(this.Width - 20, this.Height - 20, 20, 20, 0, 90); // Правый нижний угол
            path.AddArc(0, this.Height - 20, 20, 20, 90, 90); // Левый нижний угол
            path.CloseFigure();

            this.Region = new Region(path);

            // Заливаем каждую часть кнопки своим цветом
            g.FillRectangle(new SolidBrush(leftColor), 0, 0, this.Width / 2, this.Height);
            g.FillRectangle(new SolidBrush(rightColor), this.Width / 2, 0, this.Width / 2, this.Height);

            // Рисуем разделитель
            g.FillRectangle(Brushes.Gray, this.Width / 2 - dividerWidth / 2, 0, dividerWidth, this.Height);

            // Рисуем обводку
            g.DrawPath(new Pen(Color.Black, 2), path); // Обводка кнопки

            // Рисуем текст
            SizeF leftTextSize = g.MeasureString(leftText, LeftButtonFont);
            SizeF rightTextSize = g.MeasureString(rightText, RightButtonFont);

            // Устанавливаем координаты для текста
            float leftTextX = (this.Width / 2 - dividerWidth) / 2 - leftTextSize.Width / 2; // Центрируем текст
            float leftTextY = (this.Height - leftTextSize.Height) / 2;

            float rightTextX = this.Width / 2 + (this.Width / 2 - dividerWidth) / 2 - rightTextSize.Width / 2; // Центрируем текст
            float rightTextY = (this.Height - rightTextSize.Height) / 2;

            // Рисуем текст
            g.DrawString(leftText, LeftButtonFont, Brushes.Black, leftTextX, leftTextY);
            g.DrawString(rightText, RightButtonFont, Brushes.Black, rightTextX, rightTextY);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // Проверяем, на какую часть кнопки нажали
                if (e.X < this.Width / 2)
                {
                    rightColor = defaultColor; // Возвращаем цвет правой части к стандартному
                    leftColor = pressedColor; // Меняем цвет при нажатии на левую часть
                    LeftButtonClick?.Invoke(this, EventArgs.Empty); // Нажали на "Обновить"
                }
                else
                {
                    leftColor = defaultColor;
                    rightColor = pressedColor; // Меняем цвет при нажатии на правую часть
                    RightButtonClick?.Invoke(this, EventArgs.Empty); // Нажали на "Печать"
                }
            }
            Invalidate(); // Перерисовываем кнопку
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // Возвращаем цвета к стандартным
                leftColor = defaultColor;
                rightColor = defaultColor;
                Invalidate(); // Перерисовываем кнопку
            }
        }
    }
}

    
