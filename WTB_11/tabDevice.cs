using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WPB_11
{
    class TabDevice
    {
        public void ShowTabContent(Panel contentPanel, string[] TabNames)
        {
            contentPanel.Controls.Clear();

            // Создаем DataGridView для отображения параметров устройства
            DataGridView parametersGridView = new DataGridView
            {
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells,
                ReadOnly = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ColumnHeadersVisible = true,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = Color.White,
                AllowUserToResizeColumns = false,
                AllowUserToResizeRows = false
            };

            // Убираем стандартное выделение
            parametersGridView.DefaultCellStyle.SelectionBackColor = Color.Transparent;
            parametersGridView.DefaultCellStyle.SelectionForeColor = Color.Black;
            parametersGridView.DefaultCellStyle.BackColor = Color.White;

            // Настраиваем столбцы
            parametersGridView.Columns.Add("Index", "№");
            parametersGridView.Columns.Add(new DataGridViewComboBoxColumn
            {
                HeaderText = "Датчик (название)",
                Name = "SensorName",
                DataSource = new string[] { "Датчик 1", "Датчик 2", "Датчик 3" },
                FlatStyle = FlatStyle.Flat
            });
            parametersGridView.Columns.Add(new DataGridViewComboBoxColumn
            {
                HeaderText = "Запрос",
                Name = "Request",
                DataSource = new string[] { "Запрос 1", "Запрос 2", "Запрос 3" },
                FlatStyle = FlatStyle.Flat
            });
            parametersGridView.Columns.Add("loadPercentage", "Процент загрузки");
            parametersGridView.Columns.Add("force", "Усилие");

            // Пример данных для таблицы
            for (int i = 1; i <= 4; i++)
            {
                parametersGridView.Rows.Add(i, "Датчик 1", "Запрос 1", i + "%", i*i);
            }

            ArrowButton emptyButton1 = new ArrowButton("G:\\VisualStudio\\repos\\WTB_11\\WTB_11\\Img\\saveEmpty.JPG", 50,200);
            ArrowButton emptyButton2 = new ArrowButton("G:\\VisualStudio\\repos\\WTB_11\\WTB_11\\Img\\saveEmpty.JPG", 50, 200);
            ArrowButton emptyButton3 = new ArrowButton("G:\\VisualStudio\\repos\\WTB_11\\WTB_11\\Img\\saveEmpty.JPG", 50, 200);
            ArrowButton cargoEmptyImg = new ArrowButton("G:\\VisualStudio\\repos\\WTB_11\\WTB_11\\Img\\cargoEmpty.JPG", 50, 200);


            var serialNumber = new TextBoxWithButton("Зав №") { PlaceholderText = "Скорость ветра (коэфф.)" };
            var windAveraging = new TextBoxWithButton("Усреднение ветра") { PlaceholderText = "" };
            var labelSum = new Label()
            {
                Text = "Общие значения",
                AutoSize = true,
                Location = new Point(10, 5),
                ForeColor = Color.Black,
                BackColor = Color.Transparent, // Делаем фон метки прозрачным
                Font = FontManager.GetSemiBoldFont(12),
            };
            var totalEffort = new TextBoxWithButton("Суммарное усилие") { PlaceholderText = "" };
            var cargoMassField = new TextBoxWithButton("Текущая масса груза") { PlaceholderText = "" };
            var windSpeedCoefficient = new TextBoxWithButton("Коэфф. скорости ветра") { PlaceholderText = "Скорость ветра (коэфф.)" };

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
            var coeffField1 = new TextBoxWithButton("Coeff") { PlaceholderText = "значение появляется при подключении прибора" };
            var additivField1 = new TextBoxWithButton("Additiv") { PlaceholderText = "значение появляется при подключении прибора" };
            var integrationField1 = new TextBoxWithButton("Интегрирование 1") { PlaceholderText = "значение появляется при подключении прибора" };
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
            var coeffField2 = new TextBoxWithButton("Coeff") { PlaceholderText = "значение появляется при подключении прибора" };
            var additivField2 = new TextBoxWithButton("Additiv") { PlaceholderText = "значение появляется при подключении прибора" };
            var integrationField2 = new TextBoxWithButton("Интегрирование") { PlaceholderText = "значение появляется при подключении прибора" };
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
                Margin = new Padding(0, 10, 0, 10)
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
            winche1ParamPanel.Controls.Add(maxQField1);
            winche1ParamPanel.Controls.Add(cargoMassField1);
            winche1ParamPanel.Controls.Add(emptyButton1);
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
            winche2ParamPanel.Controls.Add(maxQField2);
            winche2ParamPanel.Controls.Add(cargoMassField2);
            winche2ParamPanel.Controls.Add(emptyButton2);
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
            winche3ParamPanel.Controls.Add(maxQField3);
            winche3ParamPanel.Controls.Add(cargoMassField3);
            winche3ParamPanel.Controls.Add(emptyButton3);
            // Добавляем правую панель в основное расположение (вторая строка, вторая колонка)
            layoutPanel.Controls.Add(winche3ParamPanel, 2, 2);

            FlowLayoutPanel sumPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true,
                WrapContents = false,
                Margin = new Padding(0)
            };
            sumPanel.Controls.Add(labelSum);
            sumPanel.Controls.Add(totalEffort);
            sumPanel.Controls.Add(cargoMassField);
            sumPanel.Controls.Add(windSpeedCoefficient);
            sumPanel.Controls.Add(cargoEmptyImg);
            layoutPanel.Controls.Add(sumPanel, 4, 2);

            // Добавляем текстовые поля в TableLayoutPanel
            layoutPanel.Controls.Add(parametersGridView, 0, 0); // Первое текстовое поле
            layoutPanel.Controls.Add(serialNumber, 1, 0); // Второе текстовое поле
            layoutPanel.Controls.Add(windAveraging, 2, 0); // DataGridView в третьем столбце

            // Устанавливаем размеры столбцов
            layoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize)); // Для первого текстового поля
            layoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize)); // Для второго текстового поля
            layoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize)); // Для DataGridView, который будет занимать оставшееся пространство

            layoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Для первого текстового поля

            // Добавляем элементы в contentPanel
            contentPanel.Controls.Add(layoutPanel); // Затем панель с текстовыми полями и таблицей
        }


    }
}
