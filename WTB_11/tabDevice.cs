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

            if (index == 1)
            {
                // Создаем элементы управления
                ArrowButton arrowButton = new ArrowButton { Width = 100 };
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
                for (int i = 1; i <= 8; i++)
                {
                    customCheckedListBoxWinches.AddItem($"Лебедка {i}");
                }

                CTextBox cTextBox = new CTextBox("Скорость ветра (коэфф.)");

                var sensorsCountField = new roundedTextBox("Количество датчиков:") { PlaceholderText = "значение появляется при подключении прибора" };
                var effortOnSensorsField = new roundedTextBox("Усилие на датчиках:") { PlaceholderText = "значение появляется при подключении прибора" };
                var totalEffortField = new roundedTextBox("Суммарное усилие:") { PlaceholderText = "значение появляется при подключении прибора" };
                var cargoMassField = new roundedTextBox("Масса груза:") { PlaceholderText = "значение появляется при подключении прибора" };
                var loadPercentageField = new roundedTextBox("Процент загрузки:") { PlaceholderText = "значение появляется при подключении прибора" };
                var windSpeedField = new roundedTextBox("Скорость ветра:") { PlaceholderText = "значение появляется при подключении прибора" };
                var temperatureInBlockField = new roundedTextBox("Температура в блоке:") { PlaceholderText = "значение появляется при подключении прибора" };
                var systemTime = new roundedTextBox("Дата и время в системе") { PlaceholderText = "" };
                var deviceTime = new roundedTextBox("Дата и время в приборе") { PlaceholderText = "значение появляется при подключении прибора" };
                var errors = new roundedTextBox("Ошибки:") { PlaceholderText = "значение появляется при подключении прибора" };

                // Создаем TableLayoutPanel
                TableLayoutPanel mainLayout = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    ColumnCount = 3,
                    RowCount = 3,
                    AutoSize = true,
                    CellBorderStyle = TableLayoutPanelCellBorderStyle.None
                };

                // Устанавливаем фиксированную высоту для строки с dateTimePanel
                mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Высота для dateTimePanel
                mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Для списка лебедок
                mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50)); 
                // Задаем размеры колонок
                mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
                mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200)); // Фиксированная ширина для правой колонки
                mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50)); // Фиксированная ширина для правой колонки


                // Создаем панель для даты и времени
                FlowLayoutPanel dateTimePanel = new FlowLayoutPanel
                {
                    Dock = DockStyle.Top,
                    AutoSize = true,
                    WrapContents = false,
                    FlowDirection = FlowDirection.LeftToRight,
                    Margin = new Padding(0, 0, 0, 5)
                };
                // Добавляем элементы в dateTimePanel
                dateTimePanel.Controls.Add(deviceTime);
                dateTimePanel.Controls.Add(arrowButton);
                dateTimePanel.Controls.Add(systemTime);
                // Добавляем dateTimePanel в основное расположение (первая строка, первая колонка)
                mainLayout.Controls.Add(dateTimePanel, 0, 0);


                FlowLayoutPanel winchesFlowPanel = new FlowLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    FlowDirection = FlowDirection.RightToLeft,
                    AutoSize = true,
                    WrapContents = false // Чтобы элементы не переносились на новую строку
                };

                // Добавляем список лебедок в панель
                winchesFlowPanel.Controls.Add(customCheckedListBoxWinches);

                // Добавляем winchesPanel в основное расположение (вторая строка, первая колонка)
                mainLayout.Controls.Add(winchesFlowPanel, 0, 1);

                // Создаем панель для правых элементов
                FlowLayoutPanel winchesParamPanel = new FlowLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    FlowDirection = FlowDirection.TopDown,
                    AutoSize = true,
                    Margin = new Padding(0)
                };
                // Добавляем элементы в правую панель
                winchesParamPanel.Controls.Add(sensorsCountField);
                winchesParamPanel.Controls.Add(effortOnSensorsField);
                winchesParamPanel.Controls.Add(totalEffortField);
                winchesParamPanel.Controls.Add(cargoMassField);
                winchesParamPanel.Controls.Add(loadPercentageField);
                // Добавляем правую панель в основное расположение (вторая строка, вторая колонка)
                mainLayout.Controls.Add(winchesParamPanel, 1, 1);

                FlowLayoutPanel deviceParamPanel = new FlowLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    FlowDirection = FlowDirection.TopDown,
                    AutoSize = true,
                    Margin = new Padding(0)
                };
                deviceParamPanel.Controls.Add(windSpeedField);
                deviceParamPanel.Controls.Add(temperatureInBlockField);
                deviceParamPanel.Controls.Add(errors);
                // Добавляем правую панель в основное расположение (вторая строка, вторая колонка)
                mainLayout.Controls.Add(deviceParamPanel, 2, 1);



                // Добавляем основной TableLayoutPanel в contentPanel
                contentPanel.Controls.Add(mainLayout);


            }
        }
    }
}