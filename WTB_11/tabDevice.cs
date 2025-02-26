using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                Dock = DockStyle.None,
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
            parametersGridView.DefaultCellStyle.SelectionBackColor = Color.Transparent; // Убираем синий цвет выделения
            parametersGridView.DefaultCellStyle.SelectionForeColor = Color.Black;

            // Устанавливаем цвет фона для всех ячеек
            parametersGridView.DefaultCellStyle.BackColor = Color.White;

            // Настраиваем столбцы
            parametersGridView.Columns.Add("Index", "№");

            // Столбец для ввода данных с изменением цвета
            DataGridViewComboBoxColumn sensorColumn = new DataGridViewComboBoxColumn
            {
                HeaderText = "Датчик (название)",
                Name = "SensorName",
                DataSource = new string[] { "Датчик 1", "Датчик 2", "Датчик 3" },
                FlatStyle = FlatStyle.Flat
            };
            parametersGridView.Columns.Add(sensorColumn);

            // Столбец для выбора из списка
            DataGridViewComboBoxColumn requestColumn = new DataGridViewComboBoxColumn
            {
                HeaderText = "Запрос",
                Name = "Request",
                DataSource = new string[] { "Запрос 1", "Запрос 2", "Запрос 3" },
                FlatStyle = FlatStyle.Flat
            };
            parametersGridView.Columns.Add(requestColumn);

            // Пример данных для таблицы
            for (int i = 1; i <= 10; i++)
            {
                parametersGridView.Rows.Add(i, "Датчик 1", "Запрос 1");
            }

            // Переменная для хранения индекса последней редактируемой ячейки
            int lastEditedRowIndex = -1;

            // Обработчик события для изменения цвета ячейки при активации
            parametersGridView.CellBeginEdit += (sender, e) =>
            {
                // Если есть предыдущая редактируемая ячейка, возвращаем ей белый цвет
                if (lastEditedRowIndex != -1 && lastEditedRowIndex != e.RowIndex)
                {
                    parametersGridView.Rows[lastEditedRowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.White;
                }

                // Устанавливаем цвет E0E0E0 для текущей ячейки
                parametersGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.FromArgb(224, 224, 224);
                lastEditedRowIndex = e.RowIndex; // Сохраняем индекс текущей ячейки
            };

            // Обработчик события для возврата цвета ячейки при завершении редактирования
            parametersGridView.CellEndEdit += (sender, e) =>
            {
                parametersGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.White; // Возвращаем белый цвет
            };

            // Устанавливаем размеры DataGridView
            parametersGridView.AutoSize = true;
            parametersGridView.Height = parametersGridView.GetPreferredSize(parametersGridView.Size).Height;

            // Добавляем таблицу в contentPanel
            contentPanel.Controls.Add(parametersGridView);

            // Обработчик события для предотвращения стандартного выделения при клике
            parametersGridView.CellMouseClick += (sender, e) =>
            {
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                {
                    // Убираем выделение
                    parametersGridView.ClearSelection();
                }
            };

            // Обработчик события для предотвращения подсветки ячейки при наведении мыши
            parametersGridView.CellMouseEnter += (sender, e) =>
            {
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                {
                    // Убираем подсветку
                    parametersGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.White; // Возвращаем белый цвет
                }
            };

            // Обработчик события для предотвращения подсветки ячейки при выходе мыши
            parametersGridView.CellMouseLeave += (sender, e) =>
            {
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                {
                    // Убираем подсветку
                    parametersGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.White; // Возвращаем белый цвет
                }
            };

            // Убираем выделение всей строки при наведении мыши
            parametersGridView.RowsDefaultCellStyle.BackColor = Color.White; // Устанавливаем фон для всех ячеек
            parametersGridView.RowsDefaultCellStyle.SelectionBackColor = Color.Transparent; // Убираем цвет выделения для строк

            // Устанавливаем размеры DataGridView
            parametersGridView.AutoSize = true;
            parametersGridView.Height = parametersGridView.GetPreferredSize(parametersGridView.Size).Height;
            ArrowButton arrowButton = new ArrowButton("G:\\VisualStudio\\repos\\WTB_11\\WTB_11\\Img\\saveButton.JPG", 90, 200);

            FlowLayoutPanel devicePanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                WrapContents = false,
                FlowDirection = FlowDirection.LeftToRight,
                Margin = new Padding(0, 0, 0, 5)
            };
            devicePanel.Controls.Add(arrowButton);

            // Добавляем таблицу в contentPanel
            contentPanel.Controls.Add(parametersGridView);
            contentPanel.Controls.Add(devicePanel);
        }
    }
}
