using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPB_11
{
    public class SingleRoundedButton : Control
    {
        private string buttonText = "Нажми меня";
        private Color defaultColor = Color.White; // Стандартный цвет фона
        private Color pressedColor = Color.FromArgb(224, 224, 224); // Цвет фона при нажатии
        private Color currentColor; // Текущий цвет кнопки

        public event EventHandler ButtonClick;

        public string ButtonText
        {
            get => buttonText;
            set { buttonText = value; Invalidate(); }
        }

        // Используем кастомные шрифты из FontManager
        public Font ButtonFont { get; set; } = FontManager.GetMediumFont(12); // Замените на нужный вам шрифт

        public SingleRoundedButton()
        {
            currentColor = defaultColor; // Устанавливаем начальный цвет
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

            // Заливаем кнопку текущим цветом
            g.FillPath(new SolidBrush(currentColor), path);

            // Рисуем обводку
            g.DrawPath(new Pen(Color.Black, 2), path); // Обводка кнопки

            // Рисуем текст
            SizeF textSize = g.MeasureString(buttonText, ButtonFont);
            float textX = (this.Width - textSize.Width) / 2; // Центрируем текст
            float textY = (this.Height - textSize.Height) / 2;

            // Рисуем текст
            g.DrawString(buttonText, ButtonFont, Brushes.Black, textX, textY);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                currentColor = pressedColor; // Меняем цвет при нажатии
                ButtonClick?.Invoke(this, EventArgs.Empty); // Вызываем событие нажатия кнопки
                Invalidate(); // Перерисовываем кнопку
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                currentColor = defaultColor; // Возвращаем цвет к стандартному
                Invalidate(); // Перерисовываем кнопку
            }
        }
    }

}
