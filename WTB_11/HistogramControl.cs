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
        protected int maxCycles = 140; // Максимальное количество циклов для нормализации
        protected float heightMultiplier = 3f; // Множитель для увеличения высоты столбцов

        public HistogramControl()
        {
            this.BackColor = Color.White;
            this.Size = new Size(500, 300);
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
            e.Graphics.DrawRectangle(Pens.Black, 20, 0, this.ClientSize.Width - 50, this.Height - 40);

            // Отрисовка гистограммы
            OnDrawHistogram(e);
        }

        // Метод для рисования гистограммы
        protected virtual void OnDrawHistogram(PaintEventArgs e)
        {
            if (data == null || data.Length == 0) return;

            float barWidth = (this.ClientSize.Width - 80) / (data.Length * 2.3f); // Ширина столбца
            float scaleFactor = (this.ClientSize.Height - 80) / maxCycles; // Масштаб для высоты

            // Укажите смещения для сдвига
            float xOffset = 30; // Сдвиг вправо
            float yOffset = 20; // Сдвиг вверх

            // Метки на оси Y
            int[] yLabels = { 10, 30, 50, 70, 90, 110, 130 };

            // Рисуем серые полоски и метки снизу
            for (int i = 0; i < yLabels.Length; i++)
            {
                int value = yLabels[i]; // Значение метки
                float yPosition = this.Height - 40 - value * scaleFactor; // Позиция по Y для линии
                float xPosition = xOffset + i * (barWidth * 2.3f); // Увеличиваем расстояние между метками

                // Рисуем серую линию
                e.Graphics.DrawLine(Pens.Gray, xPosition, 20, xPosition, this.Height - 60);

                // Рисуем метку
                e.Graphics.DrawString(value.ToString(), new Font("Arial", 8), Brushes.Black, xPosition - 5, this.Height - 55);
            }

            // Добавляем метку ">140"
            float lastYPosition = this.Height - 40 - 140 * scaleFactor; // Позиция для ">140"
            e.Graphics.DrawLine(Pens.Gray, xOffset + (yLabels.Length * (barWidth * 2.3f)), 20, xOffset + (yLabels.Length * (barWidth * 2.3f)), this.Height - 60);
            e.Graphics.DrawString(">140", new Font("Arial", 8), Brushes.Black, xOffset + (yLabels.Length * (barWidth * 2.3f)) - 10, this.Height - 55);

            // Рисуем столбцы
            for (int i = 0; i < data.Length; i++)
            {
                float barHeight = data[i] * scaleFactor * heightMultiplier; // Высота столбца
                float xPosition = xOffset + i * (barWidth * 2.3f); // Увеличиваем расстояние между столбцами

                // Рисуем столбец
                e.Graphics.FillRectangle(Brushes.Blue, xPosition - barWidth / 2, this.Height - 40 - barHeight - yOffset, barWidth, barHeight);

                // Рисуем значение над столбцом
                string valueText = ((int)data[i]).ToString(); // Преобразуем в целое число
                SizeF textSize = e.Graphics.MeasureString(valueText, new Font("Arial", 10)); // Измеряем размер текста
                e.Graphics.DrawString(valueText, new Font("Arial", 10), Brushes.Black,
                    xPosition - textSize.Width / 2,
                    this.Height - 40 - barHeight - yOffset - textSize.Height - 2); // Рисуем текст
            }
        }
    }
}

