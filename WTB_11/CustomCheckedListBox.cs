using System;
using System.Drawing;
using System.Windows.Forms;

namespace WPB_11
{
    public class CustomCheckedListBox : UserControl
    {
        private Label label;
        private ComboBox comboBox; // Используем ComboBox вместо CustomList

        public CustomCheckedListBox(string labelText)
        {
            // Инициализация элементов
            label = new Label
            {
                Text = labelText,
                AutoSize = true,
                ForeColor = Color.Black,
                BackColor = Color.Transparent, // Делаем фон метки прозрачным
                Font = FontManager.GetSemiBoldFont(10),
                TextAlign = ContentAlignment.MiddleCenter,
            };

            // Инициализация ComboBox
            comboBox = new ComboBox
            {
                Width = 165,
                Location = new Point(0, label.Height + 5),
                Font = FontManager.GetRegularFont(10),
                DropDownStyle = ComboBoxStyle.DropDownList // Устанавливаем стиль выпадающего списка
            };

            // Настройка внешнего вида
            this.BackColor = Color.White;

            // Добавление элементов управления
            this.Controls.Add(label);
            this.Controls.Add(comboBox);

            // Установка позиции элементов
            label.Location = new Point(0, 0);
        }

        // Метод для добавления элементов в ComboBox
        public void AddItem(string itemText)
        {
            comboBox.Items.Add(itemText);
        }

        // Метод для получения выбранного элемента
        public string SelectedItem
        {
            get { return comboBox.SelectedItem?.ToString(); }
        }
    }

}



