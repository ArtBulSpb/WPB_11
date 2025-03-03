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
    class TabLongInform
    {
        public void ShowTabContent(Panel contentPanel, string[] TabNames)
        {
            contentPanel.Controls.Clear();

            var winchesList = new CustomCheckedListBox("Выбор лебедки") { Margin = new Padding(25, 0, 25, 10), AutoSize=true };
            for (int i = 0; i <= 3; i++)
            {
                winchesList.AddItem("Лебедка " + i);
            }

            var totalNumberCycles = new TextBoxWithButton("Суммарное число циклов") { PlaceholderText = "значение появляется при подключении прибора" };
            var characteristicNumber = new TextBoxWithButton("Характеристическое число") { PlaceholderText = "значение появляется при подключении прибора" };
            var totalCargoWeight = new TextBoxWithButton("Суммарная масса груза") { PlaceholderText = "значение появляется при подключении прибора" };
            var loadDistributionCoefficient = new TextBoxWithButton("Коэфф. распределения нагрузок") { PlaceholderText = "значение появляется при подключении прибора" };
            var craneTime = new DoubleLabel("Наработка крана: ") { Margin = new Padding(30, 0, 50, 10) };
            var roundedButton = new RoundedButton
            {
                LeftText = "Обновить",
                RightText = "Печать",
                Size = new Size(200, 50), // Устанавливаем размеры кнопки
                Location = new Point(50, 50), // Устанавливаем положение кнопки на форме
                Margin = new Padding(110, 0, 50, 0)
            };

            // Подписка на события
            roundedButton.LeftButtonClick += (s, e) => MessageBox.Show("Нажата кнопка 'Обновить'!");
            roundedButton.RightButtonClick += (s, e) => MessageBox.Show("Нажата кнопка 'Печать'!");


            var histogramControl = new HistogramControl
            {
                Location = new Point(10, 10)
            };
            // Пример данных для гистограммы
            float[] histogramData = { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };
            histogramControl.SetData(histogramData);
            histogramControl.Text = "Гистограмма";
            histogramControl.Size = new Size(450, 400);

            Panel separator1 = new Panel // отделяет дату и время от остального
            {
                Height = 1, // Высота разделителя
                Dock = DockStyle.Top, // Расположение сверху
                BackColor = Color.FromArgb(224, 224, 224), // Цвет E0E0E0
                Margin = new Padding(0, 0, 0, 10)
            };

            TableLayoutPanel layoutPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3, // Увеличьте количество столбцов до 3
                RowCount = 3,
                AutoSize = true
            };

            // Установите ширину среднего столбца для отступа
            layoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize)); // Столбец с лебедками
            layoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 40)); // Пустой столбец для отступа
            layoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize)); // Столбец с гистограммой

            FlowLayoutPanel winchesPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true,
                WrapContents = false,
                Margin = new Padding(20, 0, 0, 0)
            };

            // Добавляем элементы в правую панель
            winchesPanel.Controls.Add(winchesList);
            winchesPanel.Controls.Add(totalNumberCycles);
            winchesPanel.Controls.Add(characteristicNumber);
            winchesPanel.Controls.Add(totalCargoWeight);
            winchesPanel.Controls.Add(loadDistributionCoefficient);

            FlowLayoutPanel histogramPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true,
                WrapContents = false,
                Margin = new Padding(0)
            };

            // Добавляем элементы в панель с гистограммой
            histogramPanel.Controls.Add(histogramControl);
            histogramPanel.Controls.Add(roundedButton);

            // Добавляем панели в layoutPanel
            layoutPanel.Controls.Add(craneTime, 0, 0);
            layoutPanel.Controls.Add(separator1, 0, 1);
            layoutPanel.SetColumnSpan(separator1, 3);
            layoutPanel.Controls.Add(winchesPanel, 0, 2);
            layoutPanel.Controls.Add(new Panel(), 1, 2); // Пустая панель для отступа
            layoutPanel.Controls.Add(histogramPanel, 2, 2);

            contentPanel.Controls.Add(layoutPanel);
        }
    }
}
