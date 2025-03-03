using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPB_11
{
    class TabShortInform
    {
        public void ShowTabContent(Panel contentPanel, string[] TabNames)
        {
            contentPanel.Controls.Clear();

            // Создаем DataGridView для отображения данных
            DataGridView dataGridView = new DataGridView
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

            // Пример данных для таблицы
            for (int i = 1; i <= 10; i++)
            {
                dataGridView.Rows.Add(i, DateTime.Now.AddMinutes(-i).ToString("g"),
                    $"{i * 10}", // F1
                    $"{i * 15}", // F2
                    $"{i * 20}", // F3
                    $"{i * 5}Кг",  // Q1
                    $"{i * 7}Кг",  // Q2
                    $"{i * 9}Кг",  // Q3
                    $"{i * 100}%", // M1
                    $"{i * 120}%", // M2
                    $"{i * 140}%", // M3
                    $"{i * 2}m/s", // Ветер
                    $"{20 + i}°C", // Темп. ВПБ
                    "Работа" // Режим
                );
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

            roundedButton.LeftButtonClick += (s, e) => MessageBox.Show("Нажата кнопка 'Обновить'!");
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


    }
}
