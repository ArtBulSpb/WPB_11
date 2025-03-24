using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using WPB_11.DataStructures;
using WPB_11.Device;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace WPB_11
{
    class TabLongInform
    {
        private DeviceConnector _deviceConnector;
        private DevicePackets devicePackets;

        private DoubleLabel craneTime;
        private CustomCheckedListBox winchesList;
        private TextBoxWithButton totalNumberCycles;
        private TextBoxWithButton characteristicNumber;
        private TextBoxWithButton totalCargoWeight;
        private TextBoxWithButton loadDistributionCoefficient;
        private HistogramControl histogramControl;

        private System.Windows.Forms.Timer _updateTimer;

        public void ShowTabContent(Panel contentPanel, string[] TabNames)
        {
            contentPanel.Controls.Clear();

            devicePackets = DevicePackets.Instance();
            _deviceConnector = DeviceConnector.Instance("COM3");

            // Инициализация таймера
            _updateTimer = new System.Windows.Forms.Timer();
            _updateTimer.Interval = 1000; // Обновление каждую секунду
            _updateTimer.Tick += UpdateVPBCrane; // Подписка на событие
            _updateTimer.Start(); // Запуск таймера

            devicePackets.VPBCraneProcessed += HandleVPBCraneProcessed;
            devicePackets.DateTimeProcessed += HandleDateTimeProcessed;


            winchesList = new CustomCheckedListBox("Выбор лебедки") { Margin = new Padding(25, 0, 25, 10), AutoSize=true };
            for (int i = 1; i <= 3; i++)
            {
                winchesList.AddItem("Лебедка " + i);
            }

            winchesList.SelectDefaultItem();

            totalNumberCycles = new TextBoxWithButton("Суммарное число циклов") { PlaceholderText = "значение появляется при подключении прибора" };
            characteristicNumber = new TextBoxWithButton("Характеристическое число") { PlaceholderText = "значение появляется при подключении прибора" };
            totalCargoWeight = new TextBoxWithButton("Суммарная масса груза") { PlaceholderText = "значение появляется при подключении прибора" };
            loadDistributionCoefficient = new TextBoxWithButton("Коэфф. распределения нагрузок") { PlaceholderText = "значение появляется при подключении прибора" };
            craneTime = new DoubleLabel("Наработка крана: ") { Margin = new Padding(30, 0, 50, 10) };
            var roundedButton = new DoubleRoundedButton
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


            histogramControl = new HistogramControl
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
        private void UpdateVPBCrane(object sender, EventArgs e)
        {
            Debug.WriteLine("Пишу TabCrane");
            if (DeviceConnector.Instance().IsConnected)
            {
                DeviceConnector.Instance().Request(DeviceCommands.RequestVPBCrane);
                DeviceConnector.Instance().Request(DeviceCommands.RequestDateTime);
            }
        }

        private void HandleVPBCraneProcessed(VPBCrane.VPBCraneStruct vpbcCrane)
        {
            Debug.WriteLine("HandleVPBCraneProcessed tabLong вызван"); // Отладочное сообщение
            if (craneTime.InvokeRequired)
            {
                craneTime.Invoke(new Action<VPBCrane.VPBCraneStruct>(HandleVPBCraneProcessed), vpbcCrane);

            }
            else
            {
                // Проверяем, есть ли данные
                if (vpbcCrane.VPBNumber != null)
                {
                    Win1251ToCorrectText str = new Win1251ToCorrectText();
                    // Обновляем текстовые поля на основе данных
                    craneTime.TimeText = string.Format("{0} : {1} : {2}",
                        vpbcCrane.OperatingTime / 3600,
                        (vpbcCrane.OperatingTime % 3600) / 60,
                        (vpbcCrane.OperatingTime % 3600) % 60);
                    // Получаем выбранную лебедку
                    string selectedWinch = winchesList.SelectedItem?.ToString(); // Предполагается, что вы используете свойство SelectedItem

                    if (selectedWinch != null)
                    {
                        // Обновляем значения в зависимости от выбранной лебедки
                        switch (selectedWinch)
                        {
                            case "Лебедка 1":
                                totalNumberCycles.Text = vpbcCrane.SummCycles1.ToString();
                                characteristicNumber.Text = vpbcCrane.CharacteristicNumber1.ToString(); 
                                totalCargoWeight.Text = vpbcCrane.SummQ1.ToString();
                                histogramControl.SetData(DataForHistogram(vpbcCrane.Cycles1));
                                //loadDistributionCoefficient.Text = vpbcCrane.CoeffQ1.ToString(); // CoeffQ1 не currQ1
                                break;
                            case "Лебедка 2":
                                totalNumberCycles.Text = vpbcCrane.SummCycles2.ToString();
                                characteristicNumber.Text = vpbcCrane.CharacteristicNumber2.ToString();
                                totalCargoWeight.Text = vpbcCrane.SummQ2.ToString();
                                histogramControl.SetData(DataForHistogram(vpbcCrane.Cycles2));
                                //loadDistributionCoefficient.Text = vpbcCrane.CoeffQ2.ToString(); // CoeffQ2 не currQ2
                                break;
                            case "Лебедка 3":
                                break;
                        }
                    }
                }
                else
                {
                    // Обработка случая, когда данные отсутствуют
                    Debug.WriteLine("Нет данных для обновления интерфейса device");
                }
            }
        }

        private void HandleDateTimeProcessed(VPBCurrType.VPBCurrTypeStruct currSensorData)
        {
            Debug.WriteLine("HandleDateTimeProcessed tabLong вызван"); // Отладочное сообщение
            if (craneTime.InvokeRequired)
            {
                craneTime.Invoke(new Action<VPBCrane.VPBCraneStruct>(HandleVPBCraneProcessed), currSensorData);

            }
            else
            {
                // Получаем выбранную лебедку
                string selectedWinch = winchesList.SelectedItem?.ToString(); // Предполагается, что вы используете свойство SelectedItem

                if (selectedWinch != null)
                {
                    // Обновляем значения в зависимости от выбранной лебедки
                    switch (selectedWinch)
                    {
                        case "Лебедка 1":
                            loadDistributionCoefficient.Text = currSensorData.CurrQ1.ToString(); // CoeffQ1 не currQ1
                            break;
                        case "Лебедка 2":
                            loadDistributionCoefficient.Text = currSensorData.CurrQ2.ToString(); // CoeffQ2 не currQ2
                            break;
                        case "Лебедка 3":
                            break;
                    }
                }
            }
        }

        private float[] DataForHistogram(uint[] cyclesData1) //перевод массива циклов из uint в тип который принимает гистограмма (float)
        {
            float[] histogramData = new float[cyclesData1.Length];
            for (int i = 0; i < cyclesData1.Length; i++)
            {
                histogramData[i] = (float)cyclesData1[i]; // Преобразование типа
            }
            return histogramData;
        }
    }
}
