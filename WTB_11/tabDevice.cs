using System;
using System.Linq;
using System.Windows.Forms;
using static WPB_11.roundedTextBox;

namespace WPB_11
{
    class tabDevice
    {
        public void ShowTabContent(Panel contentPanel, int index, string[] TabNames)
        {
            contentPanel.Controls.Clear();

            if (index == TabNames.Length - 1)
            {
                ArrowButton arrowButton = new ArrowButton {  Width = 100 };
                arrowButton.Image = Image.FromFile("G:\\VisualStudio\\repos\\WTB_11\\WTB_11\\arrow.PNG");
                arrowButton.Click += (s, e) =>
                {
                    MessageBox.Show("Установка времени...");
                };

                CustomCheckedListBox customCheckedListBoxWinches = new CustomCheckedListBox("Список лебедок:")
                {
                    Location = new Point(10, 10)
                };

                // Добавление элементов в список
                for (int i = 1; i <= 10; i++)
                {
                    customCheckedListBoxWinches.AddItem($"Элемент {i}");
                }
                CTextBox cTextBox = new CTextBox("Kar");

                var sensorsCountField = new roundedTextBox("Количество датчиков:") { PlaceholderText = "значение появляется при подключении прибора" };
                var effortOnSensorsField = new roundedTextBox("Усилие на датчиках:") { PlaceholderText = "значение появляется при подключении прибора" };
                var totalEffortField = new roundedTextBox("Суммарное усилие:") { PlaceholderText = "значение появляется при подключении прибора" };
                var cargoMassField = new roundedTextBox("Масса груза:") { PlaceholderText = "значение появляется при подключении прибора" };
                var loadPercentageField = new roundedTextBox("Процент загрузки:") { PlaceholderText = "значение появляется при подключении прибора" };
                var windSpeedField = new roundedTextBox("Скорость ветра:") { PlaceholderText = "значение появляется при подключении прибора" };
                var temperatureInBlockField = new roundedTextBox("Температура в блоке:") { PlaceholderText = "значение появляется при подключении прибора" };

                Label labelErrors = new Label { Text = "Ошибки:", AutoSize = true };
                TextBox textBoxErrors = new TextBox { Multiline = true, Width = 200, Height = 100, ReadOnly = true };

                FlowLayoutPanel flowLayoutPanel = new FlowLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    AutoScroll = true
                };

                // Добавляем элементы в flowLayoutPanel
                flowLayoutPanel.Controls.Add(sensorsCountField);
                flowLayoutPanel.Controls.Add(effortOnSensorsField);
                flowLayoutPanel.Controls.Add(totalEffortField);
                flowLayoutPanel.Controls.Add(cargoMassField);
                flowLayoutPanel.Controls.Add(loadPercentageField);
                flowLayoutPanel.Controls.Add(windSpeedField);
                flowLayoutPanel.Controls.Add(temperatureInBlockField);
                flowLayoutPanel.Controls.Add(customCheckedListBoxWinches);
                flowLayoutPanel.Controls.Add(arrowButton);
                flowLayoutPanel.Controls.Add(labelErrors);
                flowLayoutPanel.Controls.Add(textBoxErrors);
                flowLayoutPanel.Controls.Add(cTextBox);

                // Добавляем flowLayoutPanel в contentPanel
                contentPanel.Controls.Add(flowLayoutPanel);
            }
        }

    }
}
