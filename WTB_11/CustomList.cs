using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WPB_11
{
    class CustomList : CheckedListBox
    {
        public CustomList()
        {
            Width = 400;
            Height = 100;
            BorderStyle = BorderStyle.None;
            BackColor = Color.White;
            DrawMode = DrawMode.OwnerDrawFixed;
            ItemHeight = 20;

            // Обработка событий
            this.ItemCheck += CheckedListBox_ItemCheck; // Добавляем обработчик
            this.DrawItem += CheckedListBox_DrawItem;
        }
        private void CheckedListBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            // Убираем стандартную подсветку
            e.DrawBackground();

            // Получаем текст элемента
            string itemText = this.Items[e.Index].ToString();

            // Проверяем, выбран ли элемент
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                // Рисуем фон выбранного элемента
                using (Brush brush = new SolidBrush(Color.FromArgb(224, 224, 224))) // Цвет E0E0E0
                {
                    e.Graphics.FillRectangle(brush, e.Bounds);
                }
            }

            // Рисуем текст без подсветки
            e.Graphics.DrawString(itemText, e.Font, Brushes.Black, e.Bounds.Left, e.Bounds.Top);

            // Рисуем чекбокс
            int textWidth = (int)e.Graphics.MeasureString(itemText, e.Font).Width;
            int checkboxX = e.Bounds.Left + textWidth + 10;
            CheckBoxRenderer.DrawCheckBox(e.Graphics, new Point(checkboxX, e.Bounds.Top),
                this.GetItemChecked(e.Index) ?
                System.Windows.Forms.VisualStyles.CheckBoxState.CheckedNormal :
                System.Windows.Forms.VisualStyles.CheckBoxState.UncheckedNormal);
        }



        private void CheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            // Если элемент снимается с отметки, просто возвращаем
            if (e.NewValue == CheckState.Unchecked)
                return;

            // Снимаем отметки со всех элементов, кроме выбранного
            for (int i = 0; i < this.Items.Count; i++)
            {
                if (i != e.Index)
                {
                    this.SetItemChecked(i, false);
                }
            }
        }

        public void AddItem(string itemText)
        {
            this.Items.Add(itemText);
        }

    }
}
