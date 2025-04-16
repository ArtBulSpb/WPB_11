using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
        private bool ascending = true;
        private DataTable dataTable = new DataTable();


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
            dataGridView.ColumnHeaderMouseClick += DataGridView_ColumnHeaderMouseClick;
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
            roundedButton.RightButtonClick += TestButtonClick;

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
            HashSet<string> processedTimestamps = new HashSet<string>();

            for (int i = 0; i < Math.Min(30, TPCHR.Length); i++)
            {
                var sensorData = TPCHR[i];
                string timestamp = $"{sensorData.DTT.Date}.{sensorData.DTT.Month}.{sensorData.DTT.Year} {sensorData.DTT.Hour}:{sensorData.DTT.Minute}:{sensorData.DTT.Second}";

                // Проверяем, был ли этот временной штамп уже обработан
                if (!processedTimestamps.Contains(timestamp))
                {
                    processedTimestamps.Add(timestamp);
                    Debug.WriteLine($"Index: {i}, DateTime: {timestamp}, " +
                                    $"CurrForce1: {sensorData.CurrForce1}, CurrForce2: {sensorData.CurrForce2}, " +
                                    $"CurrQ1: {sensorData.CurrQ1}, CurrQ2: {sensorData.CurrQ2}, " +
                                    $"CurrPercent1: {sensorData.CurrPercent1}, CurrPercent2: {sensorData.CurrPercent2}, " +
                                    $"CurrWind: {sensorData.CurrWind}, Temperature: {sensorData.Temperature}, SetupMode: {sensorData.SetupMode}");
                }
            }

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
                int startIndex = currentPage * pageSize; // Начальный индекс
                int endIndex = Math.Min(startIndex + pageSize, allTPCHR.Length); // Конечный индекс

                HashSet<string> processedTimestamps = new HashSet<string>();

                for (int i = startIndex; i < endIndex; i++)
                {
                    var sensorData = allTPCHR[i];
                    string timestamp = $"{sensorData.DTT.Date}.{sensorData.DTT.Month}.{sensorData.DTT.Year} {sensorData.DTT.Hour}:{sensorData.DTT.Minute}:{sensorData.DTT.Second}";

                    if (!processedTimestamps.Contains(timestamp))
                    {
                        processedTimestamps.Add(timestamp);

                        
                        int uniqueIndex = i + 1; 

                        var rowData = new SensorDataRow
                        {
                            Index = uniqueIndex.ToString(),
                            DateTime = timestamp,
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
                dataGridView.DataSource = null;
                dataGridView.DataSource = displayedRows;
                dataGridView.Columns["Index"].HeaderText = "№";
                dataGridView.Columns["DateTime"].HeaderText = "Дата/время";
                dataGridView.Columns["Wind"].HeaderText = "Ветер";
                dataGridView.Columns["TempVPB"].HeaderText = "Температура";
                dataGridView.Columns["Mode"].HeaderText = "Режим";
                dataGridView.Refresh();
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

        private void DataGridView_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string columnName = dataGridView.Columns[e.ColumnIndex].Name;
            SortData(columnName);
        }

        private void SortData(string columnName)
        {
            List<SensorDataRow> sortedRows;

            switch (columnName)
            {
                case "F1":
                    sortedRows = ascending
                        ? displayedRows.OrderBy(row => Convert.ToDouble(row.F1)).ToList()
                        : displayedRows.OrderByDescending(row => Convert.ToDouble(row.F1)).ToList();
                    break;
                case "F2":
                    sortedRows = ascending
                        ? displayedRows.OrderBy(row => Convert.ToDouble(row.F2)).ToList()
                        : displayedRows.OrderByDescending(row => Convert.ToDouble(row.F2)).ToList();
                    break;
                case "Q1":
                    sortedRows = ascending
                        ? displayedRows.OrderBy(row => Convert.ToDouble(row.Q1)).ToList()
                        : displayedRows.OrderByDescending(row => Convert.ToDouble(row.Q1)).ToList();
                    break;
                case "Q2":
                    sortedRows = ascending
                        ? displayedRows.OrderBy(row => Convert.ToDouble(row.Q2)).ToList()
                        : displayedRows.OrderByDescending(row => Convert.ToDouble(row.Q2)).ToList();
                    break;
                case "M1":
                    sortedRows = ascending
                        ? displayedRows.OrderBy(row => Convert.ToDouble(row.M1)).ToList()
                        : displayedRows.OrderByDescending(row => Convert.ToDouble(row.M1)).ToList();
                    break;
                case "M2":
                    sortedRows = ascending
                        ? displayedRows.OrderBy(row => Convert.ToDouble(row.M2)).ToList()
                        : displayedRows.OrderByDescending(row => Convert.ToDouble(row.M2)).ToList();
                    break;
                case "Wind":
                    sortedRows = ascending
                        ? displayedRows.OrderBy(row => Convert.ToDouble(row.Wind)).ToList()
                        : displayedRows.OrderByDescending(row => Convert.ToDouble(row.Wind)).ToList();
                    break;
                case "Mode":
                    sortedRows = ascending
                        ? displayedRows.OrderBy(row => row.Mode).ToList()
                        : displayedRows.OrderByDescending(row => row.Mode).ToList();
                    break;
                default:
                    return; // Если столбец не поддерживается, выходим
            }

            // Обновляем направление сортировки
            ascending = !ascending;

            // Обновляем отображаемые строки
            displayedRows.Clear();
            foreach (var row in sortedRows)
            {
                displayedRows.Add(row);
            }
            dataGridView.Refresh();
            UpdateDataGridView(); // Обновляем DataGridView
        }

        private void LoadTestData()
        {
            // Создаем тестовые данные с уникальными временными метками
            var testData = new VPBCurrType.VPBCurrTypeStruct[]
            {
        new VPBCurrType.VPBCurrTypeStruct { DTT = new VPBDateTimeTemp.VPBDateTimeTempStruct
                {
                    Hour = DateTime.Now.Hour,  // Час
                    Minute = DateTime.Now.Minute,  // Минуты
                    Second = DateTime.Now.Second,    // Секунды
                    Date = DateTime.Now.Year,  // День
                    Month = DateTime.Now.Month,  // Месяц
                    Year = DateTime.Now.Year,  // Год  
                }, CurrForce1 = 10, CurrForce2 = 5, CurrQ1 = 1, CurrQ2 = 2, CurrPercent1 = 50, CurrPercent2 = 75, CurrWind = 10, SetupMode = false },
        new VPBCurrType.VPBCurrTypeStruct { DTT = new VPBDateTimeTemp.VPBDateTimeTempStruct
                {
                    Hour = DateTime.Now.Hour,  // Час
                    Minute = DateTime.Now.Minute-2,  // Минуты
                    Second = DateTime.Now.Second,    // Секунды
                    Date = DateTime.Now.Year,  // День
                    Month = DateTime.Now.Month,  // Месяц
                    Year = DateTime.Now.Year,  // Год  
                }, CurrForce1 = 15, CurrForce2 = 10, CurrQ1 = 2, CurrQ2 = 1, CurrPercent1 = 60, CurrPercent2 = 80, CurrWind = 15, SetupMode = false },
        new VPBCurrType.VPBCurrTypeStruct { DTT = new VPBDateTimeTemp.VPBDateTimeTempStruct
                {
                    Hour = DateTime.Now.Hour,  // Час
                    Minute = DateTime.Now.Minute-15,  // Минуты
                    Second = DateTime.Now.Second,    // Секунды
                    Date = DateTime.Now.Year,  // День
                    Month = DateTime.Now.Month,  // Месяц
                    Year = DateTime.Now.Year,  // Год  
                }, CurrForce1 = 5, CurrForce2 = 2, CurrQ1 = 3, CurrQ2 = 3, CurrPercent1 = 40, CurrPercent2 = 70, CurrWind = 5, SetupMode = true },
                // Добавьте больше данных по необходимости
            };

            // Вызываем метод обработки с тестовыми данными
            HandleTPCHRProcessed(testData);
        }


        private void TestButtonClick(object sender, EventArgs e)
        {
            LoadTestData(); // Загружаем тестовые данные для проверки сортировки
        }

    }
}
