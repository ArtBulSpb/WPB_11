using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using WPB_11.DataStructures;
using WPB_11.Device;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WPB_11
{
    class TabShortInform
    {
        private DeviceConnector _deviceConnector;
        private DevicePackets devicePackets;

        private DataGridView dataGridView;
        private BindingList<SensorDataRow> displayedRows = new BindingList<SensorDataRow>();
        private int currentPage = 0;
        private const int pageSize = 100;
        private VPBCurrType.VPBCurrTypeStruct[] allTPCHR; // Поле для хранения всех данных
        private SynchronizationContext syncContext;


        public void ShowTabContent(Panel contentPanel, string[] TabNames)
        {
            

            contentPanel.Controls.Clear();

            devicePackets = DevicePackets.Instance();
            _deviceConnector = DeviceConnector.Instance("COM3");
            syncContext = SynchronizationContext.Current;

            // Создаем DataGridView для отображения данных
            dataGridView = new DataGridView
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

            dataGridView.DefaultCellStyle.SelectionBackColor = Color.Transparent; // Цвет фона при выделении
            dataGridView.DefaultCellStyle.SelectionForeColor = Color.Black; // Цвет текста при выделении
            dataGridView.DefaultCellStyle.BackColor = Color.White; // Цвет фона ячеек
            dataGridView.DefaultCellStyle.ForeColor = Color.Black; // Цвет текста ячеек
            dataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.White; // Цвет фона заголовков
            dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black; // Цвет текста заголовков
            dataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.White; // Цвет фона при выделении заголовков
            dataGridView.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.Black; // Цвет текста при выделении заголовков
            dataGridView.DefaultCellStyle.Font = FontManager.GetRegularFont(10);
            dataGridView.ColumnHeadersDefaultCellStyle.Font = FontManager.GetSemiBoldFont(12);

            // Создаем кнопку
            var roundedButton = new DoubleRoundedButton
            {
                LeftText = "Считать",
                RightText = "Печать",
                Size = new Size(200, 50),
                Dock = DockStyle.None,
                Margin = new Padding(dataGridView.Width * 2-50, 10, 0, 0) // Установите верхний отступ на 10 пикселей
            };
            roundedButton.LeftButtonClick += ReadButtonClick;
            roundedButton.RightButtonClick += (s, e) => MessageBox.Show("Нажата кнопка 'Печать'!");

            // Создаем TableLayoutPanel
            TableLayoutPanel layoutPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1, // Один столбец для DataGridView
                RowCount = 2,    // Два ряда: один для таблицы, другой для кнопки
                Padding = new Padding(20),
                AutoSize = true
            };

            // Добавляем DataGridView и кнопку в TableLayoutPanel
            layoutPanel.Controls.Add(dataGridView, 0, 0); // DataGridView занимает первый ряд
            layoutPanel.Controls.Add(roundedButton, 0, 1); // Кнопка занимает второй ряд

            // Устанавливаем стиль для строк
            layoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 250)); // DataGridView занимает все доступное пространство
            layoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Кнопка занимает только необходимое пространство

            layoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            // Добавляем Panel в contentPanel
            contentPanel.Controls.Add(layoutPanel);
        }



        private async void HandleTPCHRProcessed(VPBCurrType.VPBCurrTypeStruct[] TPCHR)
        {
            Debug.WriteLine("HandleTPCHRProcessed tabShort вызван");

            // Сохраняем все данные для последующего использования
            allTPCHR = TPCHR;

            // Очищаем текущие отображаемые строки
            UpdateDataGridView();

            // Запускаем асинхронную загрузку данных
            await LoadDataAsync();
        }


        private async Task LoadDataAsync()
        {
            await Task.Run(() =>
            {
                int startIndex = currentPage * pageSize;
                int endIndex = Math.Min((currentPage + 1) * pageSize, allTPCHR.Length);

                for (int i = startIndex; i < endIndex; i++)
                {
                    var sensorData = allTPCHR[i];
                    var rowData = new SensorDataRow
                    {
                        Index = (i + 1).ToString(),
                        DateTime = $"{sensorData.DTT.Date}.{sensorData.DTT.Month}.{sensorData.DTT.Year} {sensorData.DTT.Hour}:{sensorData.DTT.Minute}:{sensorData.DTT.Second}",
                        F1 = sensorData.CurrForce1.ToString() ?? "N/A",
                        F2 = sensorData.CurrForce2.ToString() ?? "N/A",
                        F3 = "N/A",
                        Q1 = sensorData.CurrQ1.ToString() ?? "N/A",
                        Q2 = sensorData.CurrQ2.ToString() ?? "N/A",
                        Q3 = "N/A",
                        M1 = sensorData.CurrPercent1.ToString() ?? "N/A",
                        M2 = sensorData.CurrPercent2.ToString() ?? "N/A",
                        M3 = "N/A",
                        Wind = sensorData.CurrWind.ToString() ?? "N/A",
                        TempVPB = sensorData.Temperature ?? "N/A",
                        Mode = sensorData.SetupMode ? "Настройка" : "Работа"
                    };
                    AddToDisplayedRows(rowData);
                }
            });

            UpdateDataGridView();
        }

        private void dataGridView_Scroll(object sender, EventArgs e)
        {
            // Получаем доступ к вертикальной прокрутке через родительский контрол
            var scrollBar = (sender as DataGridView)?.Controls.OfType<VScrollBar>().FirstOrDefault();
            if (scrollBar != null && scrollBar.Value == scrollBar.Maximum)
            {
                currentPage++;
                LoadDataAsync(); // Загружаем следующую страницу данных
            }
        }

        private void UpdateDataGridView()
        {
            if (dataGridView.InvokeRequired)
            {
                dataGridView.Invoke(new Action(UpdateDataGridView));
            }
            else
            {
                dataGridView.DataSource = displayedRows;
                dataGridView.Columns["Index"].HeaderText = "№";
                dataGridView.Columns["DateTime"].HeaderText = "Дата/время";
                dataGridView.Columns["Wind"].HeaderText = "Ветер";
                dataGridView.Columns["TempVPB"].HeaderText = "Температура";
                dataGridView.Columns["Mode"].HeaderText = "Режим";
            }
        }

        private void AddToDisplayedRows(SensorDataRow rowData)
        {
            syncContext.Post(_ => displayedRows.Add(rowData), null);
        }

        private void ReadButtonClick(object sender, EventArgs e)
        {
            Debug.WriteLine("ReadButtonClick tabshort");
            if (DeviceConnector.Instance().IsConnected)
            {
                DeviceConnector.Instance().Request(DeviceCommands.CreateRequestTPCHR(0));
                devicePackets.TPCHRProcessed += HandleTPCHRProcessed;
            }
        }
    }
}
