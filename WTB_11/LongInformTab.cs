using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace WPB_11
{
    class LongInformTab
    {
        public void ShowTabContent(Panel contentPanel, string[] TabNames)
        {
            contentPanel.Controls.Clear();

            //Лебедка 1
            var label1 = new Label()
            {
                Text = "Лебедка 1",
                AutoSize = true,
                Location = new Point(10, 5),
                ForeColor = Color.Black,
                BackColor = Color.Transparent, // Делаем фон метки прозрачным
                Font = FontManager.GetSemiBoldFont(12),
            };
            var totalNumberCycles1 = new TextBoxWithButton("MaxQ") { PlaceholderText = "значение появляется при подключении прибора" };
            var characteristicNumber1 = new TextBoxWithButton("MaxQ") { PlaceholderText = "значение появляется при подключении прибора" };
            var totalCargoWeight1 = new TextBoxWithButton("MaxQ") { PlaceholderText = "значение появляется при подключении прибора" };
            var loadDistributionCoefficient1 = new TextBoxWithButton("MaxQ") { PlaceholderText = "значение появляется при подключении прибора" };

            //Лебедка 2
            var label2 = new Label()
            {
                Text = "Лебедка 2",
                AutoSize = true,
                Location = new Point(10, 5),
                ForeColor = Color.Black,
                BackColor = Color.Transparent, // Делаем фон метки прозрачным
                Font = FontManager.GetSemiBoldFont(12),
            };
            var totalNumberCycles2 = new TextBoxWithButton("MaxQ") { PlaceholderText = "значение появляется при подключении прибора" };
            var characteristicNumber2 = new TextBoxWithButton("MaxQ") { PlaceholderText = "значение появляется при подключении прибора" };
            var totalCargoWeight2 = new TextBoxWithButton("MaxQ") { PlaceholderText = "значение появляется при подключении прибора" };
            var loadDistributionCoefficient2 = new TextBoxWithButton("MaxQ") { PlaceholderText = "значение появляется при подключении прибора" };

            //Лебедка 3
            var label3 = new Label()
            {
                Text = "Лебедка 3",
                AutoSize = true,
                Location = new Point(10, 5),
                ForeColor = Color.Black,
                BackColor = Color.Transparent, // Делаем фон метки прозрачным
                Font = FontManager.GetSemiBoldFont(12),
            };
            var totalNumberCycles3 = new TextBoxWithButton("MaxQ") { PlaceholderText = "значение появляется при подключении прибора" };
            var characteristicNumber3 = new TextBoxWithButton("MaxQ") { PlaceholderText = "значение появляется при подключении прибора" };
            var totalCargoWeight3 = new TextBoxWithButton("MaxQ") { PlaceholderText = "значение появляется при подключении прибора" };
            var loadDistributionCoefficient3 = new TextBoxWithButton("MaxQ") { PlaceholderText = "значение появляется при подключении прибора" };

            var histogramControl = new HistogramControl
            {
                Location = new Point(10, 10)
            };

            // Пример данных для гистограммы
            float[] histogramData = { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };
            histogramControl.SetData(histogramData);
            histogramControl.Text = "Гистограмма";
            histogramControl.Size = new Size(450, 400);
            contentPanel.Controls.Add(histogramControl);
        }
    }
}
