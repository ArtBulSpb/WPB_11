using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace WPB_11
{
    class tabControl1 : UserControl
    {
        private Panel tabPanel;
        private Panel contentPanel;
        private Button selectedButton; // Для отслеживания выбранной кнопки
        private Panel separator; // Панель-разделитель

        // Названия вкладок
        public string[] TabNames { get; set; }

        public tabControl1(string[] tabNames)
        {
            TabNames = tabNames;
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            // Устанавливаем цвет панели
            Color tabColor = Color.White; // Цвет панели вкладок

            // Создаем панель для вкладок
            tabPanel = new Panel
            {
                Dock = DockStyle.Left,
                Width = 250, // Ширина панели вкладок
                BackColor = tabColor // Цвет панели вкладок
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
                Width = 1, // Ширина разделителя
                BackColor = Color.FromArgb(224, 224, 224) // Цвет E0E0E0
            };

            // Кнопки для вкладок 
            for (int i = TabNames.Length - 1; i >= 0; i--)
            {
                string tabName = TabNames[i];
                Button button = new Button
                {
                    Text = tabName,
                    Dock = DockStyle.Top,
                    Tag = i + 1, // Сохраняем индекс вкладки в тег кнопки
                    BackColor = tabColor, // Устанавливаем цвет кнопки равным цвету панели вкладок
                    FlatStyle = FlatStyle.Flat, // Устанавливаем стиль кнопки на Flat
                    Padding = new Padding(10, 0, 10, 0), // Устанавливаем отступы слева и справа
                    AutoSize = true, // Устанавливаем AutoSize для кнопки
                    TextAlign = ContentAlignment.MiddleLeft // Выравнивание текста слева
                };
                button.FlatAppearance.BorderSize = 0; // Убираем обводку

                // Скругляем края кнопки
                button.Paint += (s, e) =>
                {
                    var buttonControl = (Button)s;
                    var path = new System.Drawing.Drawing2D.GraphicsPath();
                    path.AddArc(0, 0, 20, 20, 180, 90); // Скругление верхнего левого угла
                    path.AddArc(buttonControl.Width - 20, 0, 20, 20, 270, 90); // Скругление верхнего правого угла
                    path.AddArc(buttonControl.Width - 20, buttonControl.Height - 20, 20, 20, 0, 90); // Скругление нижнего правого угла
                    path.AddArc(0, buttonControl.Height - 20, 20, 20, 90, 90); // Скругление нижнего левого угла
                    path.CloseAllFigures();
                    buttonControl.Region = new Region(path);
                };

                button.Click += TabButton_Click; // Подписываемся на событие клика
                tabPanel.Controls.Add(button);
            }

            // Добавляем панели на контрол
            this.Controls.Add(contentPanel);
            this.Controls.Add(separator); // Добавляем разделитель
            this.Controls.Add(tabPanel);

            // Инициализируем содержимое для первой вкладки
            ShowTabContent(1); // Показать содержимое последней добавленной вкладки (первая по порядку)
        }

        private void TabButton_Click(object sender, EventArgs e)
        {
            selectedButton = (Button)sender;
            ArrowButton arrowButton = new ArrowButton();
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
                selectedButton.BackColor = Color.FromArgb(247, 247, 247); // Устанавливаем новый цвет для выбранной кнопки (F7F7F7)
            }
            
        }


        private void ShowTabContent(int index)
        {
            tabDevice tabd = new tabDevice();
            tabd.ShowTabContent(contentPanel, index, TabNames);
        }
    }
}
