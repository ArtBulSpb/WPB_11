using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPB_11
{
    public class TextBoxWithButton : UserControl
    {
        private roundedTextBox _roundedTextBox;
        private ArrowButton _arrowButton;
        private string buttonImagePath = "G:\\VisualStudio\\repos\\WTB_11\\WTB_11\\Img\\saveButton.JPG";
        private int buttonHeight = 30; // Высота кнопки
        private int buttonWidth = 140; // Ширина кнопки

        public TextBoxWithButton(string labelText)
        {
            // Инициализация элементов
            _roundedTextBox = new roundedTextBox(labelText)
            {
                Width = 200, // Устанавливаем ширину текстового поля
                Height = 90  // Устанавливаем высоту текстового поля
            };

            _arrowButton = new ArrowButton(buttonImagePath, buttonHeight, buttonWidth)
            {
                Width = buttonWidth, // Устанавливаем ширину кнопки
                Height = buttonHeight // Устанавливаем высоту кнопки
            };

            // Настройка внешнего вида
            this.BackColor = Color.Transparent; // Прозрачный фон для родительского элемента
            this.Width = 200; // Устанавливаем ширину
            this.Height = _roundedTextBox.Height + _arrowButton.Height; // Устанавливаем высоту

            // Устанавливаем позицию элементов
            _roundedTextBox.Location = new Point(0, 0); // Текстовое поле вверху

            // Центрируем кнопку под текстовым полем
            _arrowButton.Location = new Point((this.Width - buttonWidth) / 2, _roundedTextBox.Height); // Кнопка под текстовым полем по центру

            // Добавляем элементы управления
            this.Controls.Add(_roundedTextBox);
            this.Controls.Add(_arrowButton);
        }

        // Свойства для доступа к тексту и подсказке
        public string PlaceholderText
        {
            get => _roundedTextBox.PlaceholderText;
            set => _roundedTextBox.PlaceholderText = value;
        }

        public string Text
        {
            get => _roundedTextBox.Text;
            set => _roundedTextBox.Text = value;
        }

        // Событие нажатия кнопки
        public event EventHandler ButtonClick
        {
            add => _arrowButton.Click += value;
            remove => _arrowButton.Click -= value;
        }
    }

}
