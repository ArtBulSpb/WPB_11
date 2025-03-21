using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WPB_11.DataStructures;
using WPB_11.Device;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WPB_11
{
    class TabDevice
    {
        private DeviceConnector _deviceConnector;
        private DevicePackets devicePackets;

        private TextBoxWithButton serialNumber;
        private TextBoxWithButton windAveraging;
        private TextBoxWithButton coeffField1;
        private TextBoxWithButton additivField1;
        private TextBoxWithButton integrationField1;
        private TextBoxWithButton coeffField2;
        private TextBoxWithButton additivField2;
        private TextBoxWithButton integrationField2;
        private DataGridView parametersGridView;
        private TextBoxWithButton windSpeedCoefficient;

        private System.Windows.Forms.Timer _updateTimer; // Таймер для периодического обновления

        public void ShowTabContent(Panel contentPanel, string[] TabNames)
        {
            contentPanel.Controls.Clear();

            devicePackets = DevicePackets.Instance();
            _deviceConnector = DeviceConnector.Instance("COM3");
            // Инициализация таймера
            _updateTimer = new System.Windows.Forms.Timer();
            _updateTimer.Interval = 1000; // Обновление каждую секунду
            _updateTimer.Tick += UpdateDeviceTime; // Подписка на событие
            _updateTimer.Start(); // Запуск таймера

            devicePackets.VPBCraneProcessed += HandleVPBCraneProcessed;

            // Создаем DataGridView для отображения параметров устройства
            parametersGridView = new DataGridView
            {
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells,
                AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells,
                ReadOnly = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ColumnHeadersVisible = true,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = Color.White,
                AllowUserToResizeColumns = false,
                AllowUserToResizeRows = false,
                BorderStyle = BorderStyle.None,
                Dock = DockStyle.Fill,
                Margin = new Padding(50, 5, 0, 0)

            };

            // Убираем стандартное выделение
            parametersGridView.DefaultCellStyle.SelectionBackColor = Color.White;
            parametersGridView.DefaultCellStyle.SelectionForeColor = Color.Black;
            parametersGridView.DefaultCellStyle.BackColor = Color.White;


            // Настраиваем столбцы
            parametersGridView.Columns.Add("Index", "Номер датчика");
            parametersGridView.Columns.Add(new DataGridViewComboBoxColumn
            {
                HeaderText = "Датчик (название)",
                Name = "SensorName",
                DataSource = new string[] { "Отключен", "Усилие. Лебедка 1", "Усилие. Лебедка 2", "Панель 1", "Панель 2" },
                FlatStyle = FlatStyle.Flat
            });
            parametersGridView.Columns.Add(new DataGridViewComboBoxColumn
            {
                HeaderText = "Запрос",
                Name = "Request",
                DataSource = new byte[] { 96, 225, 162, 48, 49 },
                FlatStyle = FlatStyle.Flat
            });
            parametersGridView.Columns.Add("loadPercentage", "Значение 1");
            parametersGridView.Columns.Add("force", "Значение 2");


            ArrowButton emptyButton1 = new ArrowButton("G:\\VisualStudio\\repos\\WTB_11\\WTB_11\\Img\\saveEmpty.JPG", 50,200);
            ArrowButton emptyButton2 = new ArrowButton("G:\\VisualStudio\\repos\\WTB_11\\WTB_11\\Img\\saveEmpty.JPG", 50, 200);
            ArrowButton emptyButton3 = new ArrowButton("G:\\VisualStudio\\repos\\WTB_11\\WTB_11\\Img\\saveEmpty.JPG", 50, 200);
            ArrowButton cargoEmptyImg = new ArrowButton("G:\\VisualStudio\\repos\\WTB_11\\WTB_11\\Img\\cargoEmpty.JPG", 50, 200);


            serialNumber = new TextBoxWithButton("Зав №") { PlaceholderText = "" };
            windAveraging = new TextBoxWithButton("Усреднение ветра") { PlaceholderText = "" };
            var labelSum = new Label()
            {
                Text = "Ветер",
                AutoSize = true,
                Location = new Point(10, 5),
                ForeColor = Color.Black,
                BackColor = Color.Transparent, // Делаем фон метки прозрачным
                Font = FontManager.GetSemiBoldFont(12),
            };
            var totalEffort = new TextBoxWithButton("Суммарное усилие") { PlaceholderText = "" };
            var cargoMassField = new TextBoxWithButton("Текущая масса груза") { PlaceholderText = "" };
            windSpeedCoefficient = new TextBoxWithButton("Коэфф. скорости ветра") { PlaceholderText = "Скорость ветра (коэфф.)" };

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
            coeffField1 = new TextBoxWithButton("Coeff") { PlaceholderText = "значение появляется при подключении прибора" };
            additivField1 = new TextBoxWithButton("Additiv") { PlaceholderText = "значение появляется при подключении прибора" };
            integrationField1 = new TextBoxWithButton("Интегрирование 1") { PlaceholderText = "значение появляется при подключении прибора" };
            var maxQField1 = new TextBoxWithButton("MaxQ") { PlaceholderText = "значение появляется при подключении прибора" };
            var cargoMassField1 = new TextBoxWithButton("Масса груза:") { PlaceholderText = "значение появляется при подключении прибора" };

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
            coeffField2 = new TextBoxWithButton("Coeff") { PlaceholderText = "значение появляется при подключении прибора" };
            additivField2 = new TextBoxWithButton("Additiv") { PlaceholderText = "значение появляется при подключении прибора" };
            integrationField2 = new TextBoxWithButton("Интегрирование") { PlaceholderText = "значение появляется при подключении прибора" };
            var maxQField2 = new TextBoxWithButton("MaxQ") { PlaceholderText = "значение появляется при подключении прибора" };
            var cargoMassField2 = new TextBoxWithButton("Масса груза:") { PlaceholderText = "значение появляется при подключении прибора" };

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
            var coeffField3 = new TextBoxWithButton("Coeff") { PlaceholderText = "значение появляется при подключении прибора" };
            var additivField3 = new TextBoxWithButton("Additiv") { PlaceholderText = "значение появляется при подключении прибора" };
            var integrationField3 = new TextBoxWithButton("Интегрирование 1") { PlaceholderText = "значение появляется при подключении прибора" };
            var maxQField3 = new TextBoxWithButton("MaxQ") { PlaceholderText = "значение появляется при подключении прибора" };
            var cargoMassField3 = new TextBoxWithButton("Масса груза:") { PlaceholderText = "значение появляется при подключении прибора" };

            // Создаем TableLayoutPanel для текстовых полей и таблицы
            TableLayoutPanel layoutPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 5, // Один столбец для каждого текстового поля и один для таблицы
                RowCount = 3,
                AutoSize = true
            };

            Panel separator1 = new Panel // отделяет дату и время от остального
            {
                Height = 1, // Высота разделителя
                Dock = DockStyle.Top, // Расположение сверху
                BackColor = Color.FromArgb(224, 224, 224), // Цвет E0E0E0
                Margin = new Padding(0, 0, 0, 10)
            };
            layoutPanel.Controls.Add(separator1, 0, 1);
            layoutPanel.SetColumnSpan(separator1, 5);

            Panel separator2 = new Panel // отделяет дату и время от остального
            {
                Width = 1, // Ширина разделителя
                Dock = DockStyle.Left, // Расположение слева
                BackColor = Color.FromArgb(224, 224, 224), // Цвет E0E0E0
                Margin = new Padding(10, 0, 10, 0) // Отступы по вертикали
            };
            layoutPanel.Controls.Add(separator2, 3, 2);
            layoutPanel.SetRowSpan(separator2, 1);

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
            winche1ParamPanel.Controls.Add(coeffField1);
            winche1ParamPanel.Controls.Add(additivField1);
            winche1ParamPanel.Controls.Add(integrationField1);
            //winche1ParamPanel.Controls.Add(maxQField1);
            //winche1ParamPanel.Controls.Add(cargoMassField1);
            //winche1ParamPanel.Controls.Add(emptyButton1);
            layoutPanel.Controls.Add(winche1ParamPanel, 0, 2);

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
            winche2ParamPanel.Controls.Add(coeffField2);
            winche2ParamPanel.Controls.Add(additivField2);
            winche2ParamPanel.Controls.Add(integrationField2);
            //winche2ParamPanel.Controls.Add(maxQField2);
            //winche2ParamPanel.Controls.Add(cargoMassField2);
            //winche2ParamPanel.Controls.Add(emptyButton2);
            // Добавляем правую панель в основное расположение (вторая строка, вторая колонка)
            layoutPanel.Controls.Add(winche2ParamPanel, 1, 2);

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
            winche3ParamPanel.Controls.Add(coeffField3);
            winche3ParamPanel.Controls.Add(additivField3);
            winche3ParamPanel.Controls.Add(integrationField3);
            //winche3ParamPanel.Controls.Add(maxQField3);
            //winche3ParamPanel.Controls.Add(cargoMassField3);
            //winche3ParamPanel.Controls.Add(emptyButton3);
            // Добавляем правую панель в основное расположение (вторая строка, вторая колонка)
            layoutPanel.Controls.Add(winche3ParamPanel, 2, 2);

            FlowLayoutPanel sumPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Left, // Расположение слева
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true,
                WrapContents = false,
                Margin = new Padding(0)
            };
            sumPanel.Controls.Add(labelSum);
            //sumPanel.Controls.Add(totalEffort);
            //sumPanel.Controls.Add(cargoMassField);
            sumPanel.Controls.Add(windSpeedCoefficient);
            //sumPanel.Controls.Add(serialNumber);
            sumPanel.Controls.Add(windAveraging);
            //sumPanel.Controls.Add(cargoEmptyImg);
            layoutPanel.Controls.Add(sumPanel, 4, 2);
            layoutPanel.Controls.Add(serialNumber, 4, 0);


            // Добавляем текстовые поля в TableLayoutPanel
            layoutPanel.Controls.Add(parametersGridView, 0, 0); // Первое текстовое поле
            layoutPanel.SetColumnSpan(parametersGridView, 3);

            // Устанавливаем размеры столбцов
            layoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize)); // Для первого текстового поля
            layoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize)); // Для второго текстового поля
            layoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize)); // Для DataGridView, который будет занимать оставшееся пространство
            layoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            layoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Для первого текстового поля

            // Добавляем элементы в contentPanel
            contentPanel.Controls.Add(layoutPanel); // Затем панель с текстовыми полями и таблицей
        }

        private void UpdateDeviceTime(object sender, EventArgs e)
        {
            Debug.WriteLine("Пишу TabDevice");
            if (DeviceConnector.Instance().IsConnected)
            {
                DeviceConnector.Instance().Request(DeviceCommands.RequestVPBCrane);

            }
        }

        private void HandleVPBCraneProcessed(VPBCrane.VPBCraneStruct vpbcCrane)
        {
            Debug.WriteLine("HandleVPBCraneProcessed вызван"); // Отладочное сообщение

            if (serialNumber.InvokeRequired)
            {
                serialNumber.Invoke(new Action<VPBCrane.VPBCraneStruct>(HandleVPBCraneProcessed), vpbcCrane);
            }
            else
            {
                // Проверяем, есть ли данные
                if (vpbcCrane.VPBNumber != null) 
                {
                    // Обновляем текстовые поля на основе данных
                    serialNumber.Text = new string(vpbcCrane.VPBNumber);
                    coeffField1.Text = vpbcCrane.CoeffQ1.ToString(); //заменить на currQ1
                    additivField1.Text = vpbcCrane.AdditivQ1.ToString();
                    integrationField1.Text = vpbcCrane.Integral1.ToString();
                    coeffField2.Text = vpbcCrane.CoeffQ2.ToString(); //заменить на currQ2
                    additivField2.Text = vpbcCrane.AdditivQ2.ToString();
                    integrationField2.Text = vpbcCrane.Integral2.ToString();
                    windSpeedCoefficient.Text = vpbcCrane.MaxV.ToString();
                    windAveraging.Text = vpbcCrane.IntegralV.ToString();


                    UpdateParametersGridView(vpbcCrane.Sensors);
                }
                else
                {
                    // Обработка случая, когда данные отсутствуют
                    Debug.WriteLine("Нет данных для обновления интерфейса device");
                }
            }
        }

        private void UpdateParametersGridView(VPBSensors.VPBSensorsStruct[] sensors)
        {
            parametersGridView.Rows.Clear();

            for (int i = 0; i < sensors.Length; i++)
            {
                var sensor = sensors[i];

                // Получаем название сенсора из словаря
                string sensorName = sensorTypes.ContainsKey(sensor.SensorType)
                    ? sensorTypes[sensor.SensorType]
                    : "Неизвестный тип"; // Обработка случая, если тип не найден

                Debug.WriteLine($"Попытка добавить данные для датчика {i + 1}: " +
                                $"SensorType: {sensorName.GetType()}, Query: {sensor.Query.GetType()}, " +
                                $"Data0Low: {sensor.Data0Low}, Data0High: {sensor.Data0High}");

                parametersGridView.Rows.Add(
                    i + 1,
                    sensorName, // Используем название сенсора
                    sensor.Query,
                    sensor.Data0Low + sensor.Data0High * 256
                );
            }
        }


        private Dictionary<byte, string> sensorTypes = new Dictionary<byte, string>
        {
            { 0, "Отключен" },
            { 1, "Усилие. Лебедка 1" },
            { 2, "Усилие. Лебедка 2" },
            { 3, "Панель 1" },
            { 4, "Панель 2" }
        };
    }
}
