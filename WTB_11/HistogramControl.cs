using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPB_11
{
    public class HistogramControl : BaseHistogramControl
    {
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Рисуем метку "Количество циклов" слева, перевернутую
            using (Font font = new Font("Arial", 10))
            {
                e.Graphics.TranslateTransform(20, this.Height / 2); // Перемещение для вращения
                e.Graphics.RotateTransform(-90); // Поворот на 90 градусов
                e.Graphics.DrawString("Количество циклов", font, Brushes.Black, new Point(0, 0));
                e.Graphics.ResetTransform(); // Сброс трансформации
            }

            // Рисуем метку "Загрузка лебедки %" снизу, снаружи рамки
            using (Font font = new Font("Arial", 10))
            {
                e.Graphics.DrawString("Загрузка лебедки %", font, Brushes.Black, new Point(10, this.Height - 30));
            }
        }

        // Переопределяем метод OnPaintBackground для рисования обводки
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            // Рисуем обводку
            e.Graphics.DrawRectangle(Pens.Black, 0, 0, this.Width - 1, this.Height - 1);
        }
    }

}
