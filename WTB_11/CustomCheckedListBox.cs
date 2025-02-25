using System;
using System.Drawing;
using System.Windows.Forms;

namespace WPB_11
{
    public class CustomCheckedListBox : UserControl
    {
        private Label label;
        private CustomList customList;

        public CustomCheckedListBox(string labelText)
        {
            // Инициализация элементов
            label = new Label
            {
                Text = labelText,
                AutoSize = true,
                ForeColor = Color.Black,
                BackColor = Color.Transparent // Делаем фон метки прозрачным
            };

            // Инициализация кастомного списка
            customList = new CustomList
            {
                Width = 400,
                Height = 100,
                Location = new Point(0, label.Height + 5)
            };

            // Настройка внешнего вида
            this.BackColor = Color.White;
            this.Padding = new Padding(5);
            this.Margin = new Padding(15);

            // Добавление элементов управления
            this.Controls.Add(label);
            this.Controls.Add(customList);

            // Установка позиции элементов
            label.Location = new Point(0, 0);
        }

        // Метод для добавления элементов в кастомный список
        public void AddItem(string itemText)
        {
            customList.AddItem(itemText);
        }
    }

}



