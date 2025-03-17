using System;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using WPB_11.DataStructures;
using WPB_11.Device;
using static WPB_11.RoundedTextBox;

namespace WPB_11
{
    class TabCurrentValue
    {
        private DeviceConnector _deviceConnector;
        private DevicePackets devicePackets;

        private RoundedTextBox systemTime;
        private RoundedTextBox deviceTime;
        private RoundedTextBox sensorsCountField1;
        private RoundedTextBox sensorsCountField2;
        private RoundedTextBox effortOnSensorsField1;
        private RoundedTextBox effortOnSensorsField2;
        private RoundedTextBox cargoMassField1;
        private RoundedTextBox cargoMassField2;
        private RoundedTextBox loadPercentageField1;
        private RoundedTextBox loadPercentageField2;

        private System.Windows.Forms.Timer _updateTimer; // Таймер для периодического обновления
        

        public void ShowTabContent(Panel contentPanel, string[] TabNames)
        {
            contentPanel.Controls.Clear();

            devicePackets = new DevicePackets();
            _deviceConnector = DeviceConnector.Instance("COM3", devicePackets);


            // Создаем элементы управления
            ArrowButton arrowButton = new ArrowButton("G:\\VisualStudio\\repos\\WTB_11\\WTB_11\\Img\\arrow.PNG", 90 , 200);
            arrowButton.Click += ArrowButton_Click;

            // Инициализация таймера
            _updateTimer = new System.Windows.Forms.Timer();
            _updateTimer.Interval = 1000; // Обновление каждую секунду
            _updateTimer.Tick += UpdateDeviceTime; // Подписка на событие
            _updateTimer.Start(); // Запуск таймера
            Debug.WriteLine("запуск");


            // Подписка на события
            devicePackets.DateTimeProcessed += HandleDateTimeProcessed;
            devicePackets.VPBCurrProcessed += HandleVPBCurrProcessed;


            CustomCheckedListBox customCheckedListBoxWinches = new CustomCheckedListBox("Список лебедок(1-8):")
            {
                Location = new Point(10, 10)
            };

            // Добавление элементов в список
            for (int i = 1; i <= 8; i++)
            {
                customCheckedListBoxWinches.AddItem($"Лебедка {i}");
            }

            //Лебедка 1
            var label1 = new Label() { 
                Text = "Лебедка 1",
                AutoSize = true,
                Location = new Point(10, 5),
                ForeColor = Color.Black,
                BackColor = Color.Transparent, // Делаем фон метки прозрачным
                Font = FontManager.GetSemiBoldFont(12),
            };
            sensorsCountField1 = new RoundedTextBox("Количество датчиков:") { PlaceholderText = "значение появляется при подключении прибора" };
            effortOnSensorsField1 = new RoundedTextBox("Усилие на датчиках:") { PlaceholderText = "значение появляется при подключении прибора" };
            var totalEffortField1 = new RoundedTextBox("Суммарное усилие:") { PlaceholderText = "значение появляется при подключении прибора" };
            cargoMassField1 = new RoundedTextBox("Масса груза:") { PlaceholderText = "значение появляется при подключении прибора" };
            loadPercentageField1 = new RoundedTextBox("Процент загрузки:") { PlaceholderText = "значение появляется при подключении прибора" };

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
            sensorsCountField2 = new RoundedTextBox("Количество датчиков:") { PlaceholderText = "значение появляется при подключении прибора" };
            effortOnSensorsField2 = new RoundedTextBox("Усилие на датчиках:") { PlaceholderText = "значение появляется при подключении прибора" };
            var totalEffortField2 = new RoundedTextBox("Суммарное усилие:") { PlaceholderText = "значение появляется при подключении прибора" };
            cargoMassField2 = new RoundedTextBox("Масса груза:") { PlaceholderText = "значение появляется при подключении прибора" };
            loadPercentageField2 = new RoundedTextBox("Процент загрузки:") { PlaceholderText = "значение появляется при подключении прибора" };

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
            var sensorsCountField3 = new RoundedTextBox("Количество датчиков:") { PlaceholderText = "значение появляется при подключении прибора" };
            var effortOnSensorsField3 = new RoundedTextBox("Усилие на датчиках:") { PlaceholderText = "значение появляется при подключении прибора" };
            var totalEffortField3 = new RoundedTextBox("Суммарное усилие:") { PlaceholderText = "значение появляется при подключении прибора" };
            var cargoMassField3 = new RoundedTextBox("Масса груза:") { PlaceholderText = "значение появляется при подключении прибора" };
            var loadPercentageField3 = new RoundedTextBox("Процент загрузки:") { PlaceholderText = "значение появляется при подключении прибора" };


            var windSpeedField = new CTextBox("Скорость ветра:") { PlaceholderText = "Скорость ветра (коэфф.)" };
            var temperatureInBlockField = new RoundedTextBox("Температура в блоке:") { PlaceholderText = "значение появляется при подключении прибора" };
            var errors = new RoundedTextBox("Ошибки:") { PlaceholderText = "значение появляется при подключении прибора" };

            systemTime = new RoundedTextBox("Дата и время в системе") { PlaceholderText = "" };
            deviceTime = new RoundedTextBox("Дата и время в приборе") { PlaceholderText = "значение появляется при подключении прибора" };


            

            // Создаем TableLayoutPanel
            TableLayoutPanel mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 5,
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
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));


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

            Panel separator1 = new Panel // отделяет дату и время от остального
            {
                Height = 1, // Высота разделителя
                Dock = DockStyle.Top, // Расположение сверху
                BackColor = Color.FromArgb(224, 224, 224), // Цвет E0E0E0
                Margin = new Padding(0, 10, 0, 10)
            };

            // Добавляем dateTimePanel в основное расположение (первая строка, первая колонка)
            mainLayout.Controls.Add(dateTimeDevice, 0, 0);
            mainLayout.Controls.Add(dateTimeButton, 1, 0);
            mainLayout.Controls.Add(dateTimeSystem, 2, 0);

            mainLayout.Controls.Add(separator1, 0, 1); // Разделитель даты времени
            mainLayout.SetColumnSpan(separator1, 3); // Занимаем все три колонки

            Panel separator3 = new Panel // отделяет дату и время от остального
            {
                Height = 1, // Высота разделителя
                Dock = DockStyle.Top, // Расположение сверху
                BackColor = Color.FromArgb(224, 224, 224), // Цвет E0E0E0
                Margin = new Padding(0, 20, 0, 10)
            };
            mainLayout.Controls.Add(separator3, 0, 3); // Разделитель лебедок и остального
            mainLayout.SetColumnSpan(separator3, 3); // Занимаем все три колонки

            // Лебедка 1
            FlowLayoutPanel winche1ParamPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true,
                WrapContents = false,
                Margin = new Padding(0)
            };
            // Добавляем элементы в правую панель
            winche1ParamPanel.Controls.Add(label1);
            winche1ParamPanel.Controls.Add(sensorsCountField1);
            winche1ParamPanel.Controls.Add(effortOnSensorsField1);
            winche1ParamPanel.Controls.Add(totalEffortField1);
            winche1ParamPanel.Controls.Add(cargoMassField1);
            winche1ParamPanel.Controls.Add(loadPercentageField1);
            // Добавляем правую панель в основное расположение (вторая строка, вторая колонка)
            mainLayout.Controls.Add(winche1ParamPanel, 0, 2);

            // Лебедка 2
            FlowLayoutPanel winche2ParamPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true,
                WrapContents = false,
                Margin = new Padding(0)
            };
            // Добавляем элементы в правую панель
            winche2ParamPanel.Controls.Add(label2);
            winche2ParamPanel.Controls.Add(sensorsCountField2);
            winche2ParamPanel.Controls.Add(effortOnSensorsField2);
            winche2ParamPanel.Controls.Add(totalEffortField2);
            winche2ParamPanel.Controls.Add(cargoMassField2);
            winche2ParamPanel.Controls.Add(loadPercentageField2);
            // Добавляем правую панель в основное расположение (вторая строка, вторая колонка)
            mainLayout.Controls.Add(winche2ParamPanel, 1, 2);

            // Лебедка 3
            FlowLayoutPanel winche3ParamPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true,
                WrapContents = false,
                Margin = new Padding(0)
            };
            // Добавляем элементы в правую панель
            winche3ParamPanel.Controls.Add(label3);
            winche3ParamPanel.Controls.Add(sensorsCountField3);
            winche3ParamPanel.Controls.Add(effortOnSensorsField3);
            winche3ParamPanel.Controls.Add(totalEffortField3);
            winche3ParamPanel.Controls.Add(cargoMassField3);
            winche3ParamPanel.Controls.Add(loadPercentageField3);
            // Добавляем правую панель в основное расположение (вторая строка, вторая колонка)
            mainLayout.Controls.Add(winche3ParamPanel, 2, 2);

            FlowLayoutPanel windSpeedPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true,
                Margin = new Padding(0)
            };

            FlowLayoutPanel temperatureInBlockPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true,
                Margin = new Padding(0)
            };

            FlowLayoutPanel errorsPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true,
                Margin = new Padding(0)
            };
            windSpeedPanel.Controls.Add(windSpeedField);
            temperatureInBlockPanel.Controls.Add(temperatureInBlockField);
            errorsPanel.Controls.Add(errors);
            // Добавляем правую панель в основное расположение (вторая строка, вторая колонка)
            mainLayout.Controls.Add(windSpeedPanel, 0, 4);
            mainLayout.Controls.Add(temperatureInBlockPanel, 1, 4);
            mainLayout.Controls.Add(errorsPanel, 2, 4);


            

            // Добавляем основной TableLayoutPanel в contentPanel
            contentPanel.Controls.Add(mainLayout);
        }
        private void ArrowButton_Click(object sender, EventArgs e)
        {
            if (DeviceConnector.Instance().IsConnected)
            {
                DeviceConnector.Instance().Request(DeviceCommands.SetDateTime);
            }
        }

        private void UpdateDeviceTime(object sender, EventArgs e)
        {
            Debug.WriteLine("Пишу что-то");
            if (DeviceConnector.Instance().IsConnected)
            {
                DeviceConnector.Instance().Request(DeviceCommands.RequestDateTime);
                DeviceConnector.Instance().Request(DeviceCommands.RequestTp);

            }
        }


        private void HandleDateTimeProcessed(string message)
        {
            Debug.WriteLine("HandleDateTimeProcessed вызван"); 
            if (deviceTime.InvokeRequired)
            {
                deviceTime.Invoke(new Action<string>(HandleDateTimeProcessed), message);
            }
            else
            {
                systemTime.Text = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
                deviceTime.Text = message; // Обновляем текст
            }
        }

        private void HandleVPBCurrProcessed(VPBCurrType.VPBCurrTypeStruct sensorData)
        {
            Debug.WriteLine("HandleSensorsCountProcessed вызван"); // Отладочное сообщение

            if (sensorsCountField1.InvokeRequired)
            {
                sensorsCountField1.Invoke(new Action<VPBCurrType.VPBCurrTypeStruct>(HandleVPBCurrProcessed), sensorData);
            }
            else
            {
                // Проверяем, есть ли данные
                if (sensorData.DTT.Year != 0) // Предположим, что 0 - это невалидное значение года
                {
                    // Обновляем текстовые поля на основе данных
                    deviceTime.Text = $"{sensorData.DTT.Year}-{sensorData.DTT.Month}-{sensorData.DTT.Date} {sensorData.DTT.Hour}:{sensorData.DTT.Minute}:{sensorData.DTT.Second}";
                    effortOnSensorsField1.Text = sensorData.CurrForce1.ToString();
                    effortOnSensorsField2.Text = sensorData.CurrForce2.ToString();
                    cargoMassField1.Text = $"{sensorData.CurrQ1} кг";
                    cargoMassField2.Text = $"{sensorData.CurrQ2} кг";
                    loadPercentageField1.Text = $"{sensorData.CurrPercent1}%";
                    loadPercentageField2.Text = $"{sensorData.CurrPercent2}%";
                }
                else
                {
                    // Обработка случая, когда данные отсутствуют
                    Debug.WriteLine("Нет данных для обновления интерфейса.");
                }
            }
        }

    }
}