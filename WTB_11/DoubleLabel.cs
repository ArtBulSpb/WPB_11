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
                Font = FontManager.GetRegularFont(10),
                AutoSize = true,
                Location = new Point(mainLabel.Right + 30, 0) // Смещение вправо для второго лейбла
            };

            // Добавляем лейблы в UserControl
            Controls.Add(mainLabel);
            Controls.Add(timeLabel);

            // Устанавливаем размеры UserControl
            this.AutoSize = true;
        }

       
        public string TimeText
        {
            get => timeLabel.Text;
            set => timeLabel.Text = value;
        }
    }

}
