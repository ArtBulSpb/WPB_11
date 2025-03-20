using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WPB_11.DataStructures;
using WPB_11.Device;

namespace WPB_11
{
    class tabCrane
    {
        private DeviceConnector _deviceConnector;
        private DevicePackets devicePackets;

        public TextBoxWithButton craneInfoMark;
        public TextBoxWithButton craneInfoNumber;
        private TextBoxWithButton permissibleWindSpeed;
        private CustomCheckedListBox loadingMode;
        private TextBoxWithButton maxQField1;
        private TextBoxWithButton maxQField2;

        private System.Windows.Forms.Timer _updateTimer;

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
            



            craneInfoMark = new TextBoxWithButton("Техника") { PlaceholderText = "Марка \n Модель" };
            craneInfoNumber = new TextBoxWithButton("Зав. номер") { PlaceholderText = "значение появляется при подключении прибора" };
            permissibleWindSpeed = new TextBoxWithButton("Допустимая скор. ветра") { PlaceholderText = "значение появляется при подключении прибора" };

            loadingMode = new CustomCheckedListBox("Режим нагружения") { Margin = new Padding(25, 0, 25, 0) };
            for(int i =0; i <= 8; i++)
            {
                loadingMode.AddItem("Режим нагружения " + i);
            }

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
            maxQField1 = new TextBoxWithButton("MaxQ") { PlaceholderText = "значение появляется при подключении прибора" };

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
            maxQField2 = new TextBoxWithButton("MaxQ") { PlaceholderText = "значение появляется при подключении прибора" };

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
            var maxQField3 = new TextBoxWithButton("MaxQ") { PlaceholderText = "значение появляется при подключении прибора" };

            craneInfoMark.ButtonClick += CraneInfoMark_ButtonClick;
            craneInfoNumber.ButtonClick += craneInfoNumber_ButtonClick;
            permissibleWindSpeed.ButtonClick += permissibleWindSpeed_ButtonClick;
            maxQField1.ButtonClick += maxQField1_ButtonClick;
            maxQField2.ButtonClick += maxQField2_ButtonClick;

            TableLayoutPanel layoutPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3, // Один столбец для каждого текстового поля и один для таблицы
                RowCount = 5,
                AutoSize = true
            };

            Panel separator1 = new Panel // отделяет дату и время от остального
            {
                Height = 1, // Высота разделителя
                Dock = DockStyle.Top, // Расположение сверху
                BackColor = Color.FromArgb(224, 224, 224), // Цвет E0E0E0
                Margin = new Padding(0, 0, 0, 10)
            };

            Panel separator2 = new Panel // отделяет дату и время от остального
            {
                Height = 1, // Высота разделителя
                Dock = DockStyle.Top, // Расположение сверху
                BackColor = Color.FromArgb(224, 224, 224), // Цвет E0E0E0
                Margin = new Padding(0, 0, 0, 10)
            };


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
            winche1ParamPanel.Controls.Add(maxQField1);

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
            winche2ParamPanel.Controls.Add(maxQField2);

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
            winche3ParamPanel.Controls.Add(maxQField3);

            layoutPanel.Controls.Add(craneInfoMark, 0, 0);
            layoutPanel.Controls.Add(craneInfoNumber, 1, 0);
            layoutPanel.Controls.Add(loadingMode, 2, 0);
            layoutPanel.Controls.Add(winche1ParamPanel, 0, 2);
            layoutPanel.Controls.Add(winche2ParamPanel, 1, 2);
            layoutPanel.Controls.Add(winche3ParamPanel, 2, 2);
            layoutPanel.Controls.Add(permissibleWindSpeed, 1, 4);
            layoutPanel.Controls.Add(separator1, 0, 1);
            layoutPanel.SetColumnSpan(separator1, 3);
            layoutPanel.Controls.Add(separator2, 0, 3);
            layoutPanel.SetColumnSpan(separator2, 3);


            contentPanel.Controls.Add(layoutPanel);

            // Устанавливаем размеры столбцов
            layoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize)); // Для первого текстового поля
            layoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize)); // Для второго текстового поля
            layoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize)); // Для DataGridView, который будет занимать оставшееся пространство
            layoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            layoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 140)); // Для первого текстового поля
        }

        private void UpdateDeviceTime(object sender, EventArgs e)
        {
            Debug.WriteLine("Пишу TabCrane");
            if (DeviceConnector.Instance().IsConnected)
            {
                DeviceConnector.Instance().Request(DeviceCommands.RequestVPBCrane);

            }
        }

        private void HandleVPBCraneProcessed(VPBCrane.VPBCraneStruct vpbcCrane)
        {
            Debug.WriteLine("HandleVPBCraneProcessed tabCrane вызван"); // Отладочное сообщение
            if (craneInfoMark.InvokeRequired)
            {
                craneInfoMark.Invoke(new Action<VPBCrane.VPBCraneStruct>(HandleVPBCraneProcessed), vpbcCrane);
            }
            else
            {
                // Проверяем, есть ли данные
                if (vpbcCrane.VPBNumber != null)
                {
                    Win1251ToCorrectText str = new Win1251ToCorrectText();
                    // Обновляем текстовые поля на основе данных
                    craneInfoMark.Text = str.GetNormalText(vpbcCrane.Crane);
                    craneInfoNumber.Text = new string(vpbcCrane.CraneNumber);
                    permissibleWindSpeed.Text = vpbcCrane.MaxV.ToString();
                    loadingMode.Text = vpbcCrane.LoadGroup.ToString();
                    maxQField1.Text = vpbcCrane.MaxQ1.ToString();
                    maxQField2.Text = vpbcCrane.MaxQ2.ToString();
                }
                else
                {
                    // Обработка случая, когда данные отсутствуют
                    Debug.WriteLine("Нет данных для обновления интерфейса device");
                }
            }
        }

        private void CraneInfoMark_ButtonClick(object sender, EventArgs e)
        {
            Debug.WriteLine("Пишу CraneInfoMark_ButtonClick");
            if (DeviceConnector.Instance().IsConnected)
            {
                tabCrane tabC = this;
                DeviceConnector.Instance().Request(DeviceCommands.CreateSetCraneMark(tabC));

            }
        }

        private void craneInfoNumber_ButtonClick(object sender, EventArgs e)
        {
            Debug.WriteLine("Пишу craneInfoNumber_ButtonClick");
            if (DeviceConnector.Instance().IsConnected)
            {
                //DeviceConnector.Instance().Request(DeviceCommands.SetCraneNumber);

            }
        }
        private void permissibleWindSpeed_ButtonClick(object sender, EventArgs e)
        {
            Debug.WriteLine("Пишу permissibleWindSpeed_ButtonClick");
            if (DeviceConnector.Instance().IsConnected)
            {
                //DeviceConnector.Instance().Request(DeviceCommands.SetCraneMark);

            }
        }
        private void maxQField1_ButtonClick(object sender, EventArgs e)
        {
            Debug.WriteLine("Пишу maxQField1_ButtonClick");
            if (DeviceConnector.Instance().IsConnected)
            {
                //DeviceConnector.Instance().Request(DeviceCommands.SetCraneMark);

            }
        }
        private void maxQField2_ButtonClick(object sender, EventArgs e)
        {
            Debug.WriteLine("Пишу maxQField2_ButtonClick");
            if (DeviceConnector.Instance().IsConnected)
            {
                //DeviceConnector.Instance().Request(DeviceCommands.SetCraneMark);

            }
        }
    }
}
