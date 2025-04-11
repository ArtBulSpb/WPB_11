using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using WPB_11.DataStructures;
using WPB_11.Device;

namespace WPB_11
{
    class TabShortInform
    {
        private DeviceConnector _deviceConnector;
        private DevicePackets devicePackets;

        private DataGridView dataGridView;


        public void ShowTabContent(Panel contentPanel, string[] TabNames)
        {
            contentPanel.Controls.Clear();

            devicePackets = DevicePackets.Instance();
            _deviceConnector = DeviceConnector.Instance("COM3");
            
            devicePackets.TPCHRProcessed += HandleTPCHRProcessed;

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

            // Настраиваем столбцы
            dataGridView.Columns.Add("Index", "№");
            dataGridView.Columns.Add("DateTime", "Дата/время");
            dataGridView.Columns.Add("F1", "F1");
            dataGridView.Columns.Add("F2", "F2");
            dataGridView.Columns.Add("F3", "F3");
            dataGridView.Columns.Add("Q1", "Q1");
            dataGridView.Columns.Add("Q2", "Q2");
            dataGridView.Columns.Add("Q3", "Q3");
            dataGridView.Columns.Add("M1", "M1");
            dataGridView.Columns.Add("M2", "M2");
            dataGridView.Columns.Add("M3", "M3");
            dataGridView.Columns.Add("Wind", "Ветер");
            dataGridView.Columns.Add("TempVPB", "Темп. ВПБ");
            dataGridView.Columns.Add("Mode", "Режим");

            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.Automatic; // Включаем автоматическую сортировку
            }

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



        private void HandleTPCHRProcessed(VPBCurrType.VPBCurrTypeStruct[] TPCHR)
        {
            Debug.WriteLine("HandleTPCHRProcessed tabShort вызван");

            if (dataGridView.InvokeRequired)
            {
                dataGridView.Invoke(new Action<VPBCurrType.VPBCurrTypeStruct[]>(HandleTPCHRProcessed), TPCHR);
                return;
            }

            if (TPCHR == null || TPCHR.Length == 0)
            {
                Debug.WriteLine("Массив TPCHR пуст или равен null.");
                return;
            }

            // Очищаем DataGridView
            dataGridView.Rows.Clear();
            int i = 1;

            // Добавляем строки по одной
            foreach (var sensorData in TPCHR)
            {
                dataGridView.Rows.Add(
                    // Заполните здесь ваши данные
                    i, // Например, индекс
                    $"{sensorData.DTT.Date}.{sensorData.DTT.Month}.{sensorData.DTT.Year} {sensorData.DTT.Hour}:{sensorData.DTT.Minute}:{sensorData.DTT.Second}",
                    sensorData.CurrForce1,
                    sensorData.CurrForce2,
                    "N/A",
                    sensorData.CurrQ1,
                    sensorData.CurrQ2,
                    "N/A",
                    sensorData.CurrPercent1,
                    sensorData.CurrPercent2,
                    "N/A",
                    sensorData.CurrWind,
                    sensorData.Temperature,
                    sensorData.SetupMode ? "Настройка" : "Работа"
                );
                i++;
                // Вызовите метод для обновления интерфейса, если необходимо
                Application.DoEvents(); // Это позволит обновить интерфейс
            }
        }


        private void ReadButtonClick(object sender, EventArgs e)
        {
            Debug.WriteLine("ReadButtonClick tabshort");
            if (DeviceConnector.Instance().IsConnected)
            {
                DeviceConnector.Instance().Request(DeviceCommands.CreateRequestTPCHR(0));
            }
        }
    }
}
