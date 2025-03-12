using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Data;

namespace WPB_11
{
    class tabControl1 : UserControl
    {
        private Panel tabPanel;
        private Panel contentPanel;
        private Button selectedButton; // Для отслеживания выбранной кнопки
        private Label statusLabel; // Панель-разделитель
        private Panel separator;
        private DeviceConnector deviceConnector;
        private System.Windows.Forms.Timer statusCheckTimer;

        public string[] TabNames { get; set; }

        public tabControl1(string[] tabNames)
        {
            TabNames = tabNames;
            InitializeComponents();
            InitializeDeviceConnector("COM3");
            //deviceConnector.Connect();

            // Настройка таймера
            statusCheckTimer = new System.Windows.Forms.Timer();
            statusCheckTimer.Interval = 2000; // Проверять состояние каждые 5 секунд
            statusCheckTimer.Tick += StatusCheckTimer_Tick; // Подписываемся на событие Tick
            statusCheckTimer.Start(); // Запускаем таймер
        }

        private void InitializeComponents()
        {
            Color tabColor = Color.White; // Цвет панели вкладок

            // Создаем панель для вкладок
            tabPanel = new Panel
            {
                Dock = DockStyle.Left,
                Width = 250,
                BackColor = tabColor
            };

            // Панель для содержимого
            contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White
            };

            // Панель-разделитель
            separator = new Panel
            {
                Dock = DockStyle.Left,
                Width = 1,
                BackColor = Color.FromArgb(224, 224, 224)
            };

            // Кнопки для вкладок 
            for (int i = TabNames.Length - 1; i >= 0; i--)
            {
                string tabName = TabNames[i];
                Button button = new Button
                {
                    Text = tabName,
                    Dock = DockStyle.Top,
                    Tag = i + 1,
                    BackColor = tabColor,
                    FlatStyle = FlatStyle.Flat,
                    Padding = new Padding(10, 0, 10, 0),
                    AutoSize = true,
                    TextAlign = ContentAlignment.MiddleLeft
                };
                button.FlatAppearance.BorderSize = 0;

                // Скругляем края кнопки
                button.Paint += (s, e) =>
                {
                    var buttonControl = (Button)s;
                    var path = new System.Drawing.Drawing2D.GraphicsPath();
                    path.AddArc(0, 0, 20, 20, 180, 90);
                    path.AddArc(buttonControl.Width - 20, 0, 20, 20, 270, 90);
                    path.AddArc(buttonControl.Width - 20, buttonControl.Height - 20, 20, 20, 0, 90);
                    path.AddArc(0, buttonControl.Height - 20, 20, 20, 90, 90);
                    path.CloseAllFigures();
                    buttonControl.Region = new Region(path);
                };

                button.Click += TabButton_Click;
                tabPanel.Controls.Add(button);
            }

            // Создаем поле для статуса подключения
            statusLabel = new Label
            {
                Dock = DockStyle.Fill, // Изменено на Bottom
                Height = 30, // Высота поля статуса
                BackColor = tabColor,
                TextAlign = ContentAlignment.MiddleCenter,
                Text = "Статус подключения: Определяется", // Начальный текст
                Font = new Font("Arial", 10, FontStyle.Bold)
            };

            // Добавляем панели на контрол
            this.Controls.Add(contentPanel);
            this.Controls.Add(separator);
            tabPanel.Controls.Add(statusLabel); // Добавляем поле статуса в tabPanel
            this.Controls.Add(tabPanel);

            // Инициализируем содержимое для первой вкладки
            ShowTabContent(1); // Показать содержимое последней добавленной вкладки (первая по порядку)
        }
        private void InitializeDeviceConnector(string portName)
        {
            deviceConnector = DeviceConnector.Instance();
            deviceConnector.OnDeviceConnected += UpdateStatus; // Подписываемся на событие
            if (!deviceConnector.IsConnected) // Добавьте проверку
            {
                deviceConnector.Connect();
            }
        }

        private void UpdateStatus(string status)
        {
            if (statusLabel.InvokeRequired)
            {
                // Если метод вызывается не из основного потока, используем Invoke
                statusLabel.Invoke(new Action<string>(UpdateStatus), status);
            }
            else
            {
                // Обновляем текст статуса
                statusLabel.Text = status;
            }

        }

        private void StatusCheckTimer_Tick(object sender, EventArgs e)
        {
            if (!deviceConnector.IsConnected)
            {
                deviceConnector.Connect();
            }
            // Проверяем состояние устройства и обновляем статус
            string currentStatus = deviceConnector.CheckDeviceStatus();
            UpdateStatus(currentStatus);
        }



        private void TabButton_Click(object sender, EventArgs e)
        {
            selectedButton = (Button)sender;
            ArrowButton arrowButton = new ArrowButton("G:\\VisualStudio\\repos\\WTB_11\\WTB_11\\Img\\arrow.PNG", 90, 200);
            if (selectedButton != arrowButton)
            {
                // Получаем индекс выбранной вкладки из тега кнопки
                int index = (int)((Button)sender).Tag;
                ShowTabContent(index); // Показываем содержимое соответствующей вкладки

                // Меняем цвет кнопок
                if (selectedButton != null)
                {
                    selectedButton.BackColor = tabPanel.BackColor; // Возвращаем предыдущей кнопке цвет панели вкладок
                }

                selectedButton = (Button)sender; // Запоминаем текущую выбранную кнопку
                selectedButton.BackColor = Color.FromArgb(224, 224, 224); // Устанавливаем новый цвет для выбранной кнопки (F7F7F7)
                
            }

        }


        private void ShowTabContent(int index)
        {
            switch (index)
            {
                case 1:
                    TabCurrentValue tabc = new TabCurrentValue();
                    tabc.ShowTabContent(contentPanel, TabNames);
                    break;

                case 2:
                    TabDevice tabd = new TabDevice();
                    tabd.ShowTabContent(contentPanel, TabNames);
                    break;

                case 3:
                    tabCrane tabcrane = new tabCrane();
                    tabcrane.ShowTabContent(contentPanel, TabNames);
                    break;

                case 4:
                    TabLongInform tablong = new TabLongInform();
                    tablong.ShowTabContent(contentPanel, TabNames);
                    break;
                case 5:
                    TabShortInform tabshort = new TabShortInform();
                    tabshort.ShowTabContent(contentPanel, TabNames);
                    break;
                case 6:
                    TabOverload tabover = new TabOverload();
                    tabover.ShowTabContent(contentPanel, TabNames);
                    break;
                case 7:
                    TabSettings tabs = new TabSettings();
                    tabs.ShowTabContent(contentPanel, TabNames);
                    break;
            }
        }
        
    }
}
