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

                CustomCheckedListBox customCheckedListBoxWinches = new CustomCheckedListBox("Список лебедок(1-8):")
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
                var windSpeedField = new CTextBox("Скорость ветра:") { PlaceholderText = "Введите значение" };
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
                mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); 
                // Задаем размеры колонок
                mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
                mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize)); // Фиксированная ширина для правой колонки
                mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize)); // Фиксированная ширина для правой колонки


                // Создаем панель для даты и времени
                FlowLayoutPanel dateTimeDevice = new FlowLayoutPanel
                {
                    Dock = DockStyle.Top,
                    AutoSize = true,
                    WrapContents = false,
                    FlowDirection = FlowDirection.LeftToRight,
                    Margin = new Padding(0, 0, 0, 5)
                };
                // Добавляем элементы в dateTimePanel
                dateTimeDevice.Controls.Add(deviceTime);

                FlowLayoutPanel dateTimeButton = new FlowLayoutPanel
                {
                    Dock = DockStyle.Top,
                    AutoSize = true,
                    WrapContents = false,
                    FlowDirection = FlowDirection.LeftToRight,
                    Margin = new Padding(0, 0, 0, 5)
                };
                // Добавляем элементы в dateTimePanel
                dateTimeButton.Controls.Add(arrowButton);

                FlowLayoutPanel dateTimeSystem = new FlowLayoutPanel
                {
                    Dock = DockStyle.Top,
                    AutoSize = true,
                    WrapContents = false,
                    FlowDirection = FlowDirection.LeftToRight,
                    Margin = new Padding(0, 0, 0, 5)
                };
                dateTimeSystem.Controls.Add(systemTime);

                Panel separator = new Panel // отделяет дату и время от остального
                {
                    Height = 1, // Высота разделителя
                    Dock = DockStyle.Top, // Расположение сверху
                    BackColor = Color.FromArgb(224, 224, 224) // Цвет E0E0E0
                };

                // Добавляем dateTimePanel в основное расположение (первая строка, первая колонка)
                mainLayout.Controls.Add(dateTimeDevice, 0, 0);
                mainLayout.Controls.Add(dateTimeButton, 1, 0);
                mainLayout.Controls.Add(dateTimeSystem, 2, 0);

                mainLayout.Controls.Add(separator, 0, 1); // Вставляем разделитель в следующую строку
                mainLayout.SetColumnSpan(separator, 3); // Занимаем все три колонки

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
                mainLayout.Controls.Add(winchesFlowPanel, 1, 1);

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
                mainLayout.Controls.Add(winchesParamPanel, 2, 1);

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
                mainLayout.Controls.Add(deviceParamPanel, 3, 1);



                // Добавляем основной TableLayoutPanel в contentPanel
                contentPanel.Controls.Add(mainLayout);


            }
        }
    }
}