using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WPB_11
{
    class tabCrane
    {
        public void ShowTabContent(Panel contentPanel, string[] TabNames)
        {
            contentPanel.Controls.Clear();

            
            var craneInfoMark = new TextBoxWithButton("Техника") { PlaceholderText = "Марка \n Модель" };
            var craneInfoNumber = new TextBoxWithButton("Зав. номер") { PlaceholderText = "значение появляется при подключении прибора" };
            var permissibleWindSpeed = new TextBoxWithButton("Допустимая скор. ветра") { PlaceholderText = "значение появляется при подключении прибора" };

            var loadingMode = new CustomCheckedListBox("Режим нагружения") { Margin = new Padding(25, 0, 25, 0) };
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
            var maxQField1 = new TextBoxWithButton("MaxQ") { PlaceholderText = "значение появляется при подключении прибора" };

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
            var maxQField2 = new TextBoxWithButton("MaxQ") { PlaceholderText = "значение появляется при подключении прибора" };

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
    }
}
