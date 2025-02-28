using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPB_11
{
    public class DoubleLabel : UserControl
    {
        private Label mainLabel;
        private Label timeLabel;

        public DoubleLabel(string text)
        {
            // Инициализация лейблов
            mainLabel = new Label
            {
                Text = text,
                Font = FontManager.GetSemiBoldFont(10),
                AutoSize = true,
                Location = new Point(0, 0)
            };

            timeLabel = new Label
            {
                Text = DateTime.Now.ToString("HH:mm:ss"), // Отображаем текущее время
                Font = FontManager.GetRegularFont(10),
                AutoSize = true,
                Location = new Point(0, 25) // Смещение вниз для второго лейбла
            };

            // Добавляем лейблы в UserControl
            Controls.Add(mainLabel);
            Controls.Add(timeLabel);

            // Устанавливаем размеры UserControl
            this.AutoSize = true;
        }

        // Метод для обновления времени
        public void UpdateTime()
        {
            timeLabel.Text = DateTime.Now.ToString("HH:mm:ss");
        }
    }
}
