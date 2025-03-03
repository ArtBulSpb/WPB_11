using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPB_11
{
    public class HistogramControl : UserControl
    {
        protected float[] data; // Массив данных для гистограммы
        protected int maxCycles = 100; // Максимальное количество циклов для нормализации

        public HistogramControl()
        {
            this.BackColor = Color.White;
            this.Size = new Size(400, 300);
            this.BorderStyle = BorderStyle.None;
        }

        // Метод для установки данных гистограммы
        public void SetData(float[] histogramData)
        {
            data = histogramData;
            this.Invalidate(); // Перерисовываем элемент управления
        }

        // Переопределяем метод OnPaint для рисования меток и гистограммы
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Рисуем метку "Количество циклов" слева, перевернутую
            using (Font font = new Font("Arial", 10))
            {
                e.Graphics.TranslateTransform(20, this.Height / 2); // Перемещение для вращения
                e.Graphics.RotateTransform(-90); // Поворот на 90 градусов
                e.Graphics.DrawString("Количество циклов", font, Brushes.Black, new Point(0, -20));
                e.Graphics.ResetTransform(); // Сброс трансформации
            }

            // Рисуем метку "Загрузка лебедки %" снизу, снаружи рамки
            using (Font font = new Font("Arial", 10))
            {
                e.Graphics.DrawString("Загрузка лебедки %", font, Brushes.Black, new Point(150, this.Height - 30)); // Размещаем метку ниже рамки
            }

            // Рисуем обводку
            e.Graphics.DrawRectangle(Pens.Black, 20, 0, this.Width - 50, this.Height - 40);

            // Отрисовка гистограммы
            OnDrawHistogram(e);
        }

        // Метод для рисования гистограммы
        // Метод для рисования гистограммы
        protected virtual void OnDrawHistogram(PaintEventArgs e)
        {
            if (data == null || data.Length == 0) return;

            float barWidth = (this.Width - 80) / data.Length; // Ширина столбца
            float scaleFactor = (this.Height - 80) / maxCycles; // Масштаб для высоты

            // Укажите смещения для сдвига
            float xOffset = 30; // Сдвиг вправо
            float yOffset = 20; // Сдвиг вверх

            for (int i = 0; i < data.Length; i++)
            {
                float barHeight = data[i] * scaleFactor; // Высота столбца
                e.Graphics.FillRectangle(Brushes.Blue, xOffset + i * barWidth, this.Height - 40 - barHeight - yOffset, barWidth - 2, barHeight);
            }
        }

    }


}
