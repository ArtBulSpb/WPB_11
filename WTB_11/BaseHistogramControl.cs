using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPB_11
{
    public class BaseHistogramControl : UserControl
    {
        protected float[] data; // Массив данных для гистограммы
        protected int maxCycles = 100; // Максимальное количество циклов для нормализации

        public BaseHistogramControl()
        {
            this.BackColor = Color.White;
            this.Size = new Size(400, 300);
            this.BorderStyle = BorderStyle.FixedSingle; // Обводка по контуру
        }

        // Метод для установки данных гистограммы
        public void SetData(float[] histogramData)
        {
            data = histogramData;
            this.Invalidate(); // Перерисовываем элемент управления
        }

        // Переопределяем метод OnPaint для рисования гистограммы
        protected virtual void OnDrawHistogram(PaintEventArgs e)
        {
            if (data == null || data.Length == 0) return;

            float barWidth = (this.Width - 20) / data.Length; // Ширина столбца
            float scaleFactor = (this.Height - 50) / maxCycles; // Масштаб для высоты

            for (int i = 0; i < data.Length; i++)
            {
                float barHeight = data[i] * scaleFactor; // Высота столбца
                e.Graphics.FillRectangle(Brushes.Blue, 10 + i * barWidth, this.Height - 40 - barHeight, barWidth - 2, barHeight);
            }
        }

        // Переопределяем метод OnPaint для рисования рамки
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            OnDrawHistogram(e);

            // Рисуем рамку
            e.Graphics.DrawRectangle(Pens.Black, 0, 0, this.Width - 1, this.Height - 1);
        }
    }
}
