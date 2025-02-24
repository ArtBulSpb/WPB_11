using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPB_11
{
    public class TransparentTextBox : TextBox
    {
        public TransparentTextBox()
        {
            // Устанавливаем стиль для управления
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.BackColor = Color.Transparent; // Устанавливаем прозрачный цвет фона
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // Не рисуем фон
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
        }

        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
            // Не меняем цвет фона при выделении
            this.BackColor = Color.Transparent; // Убедитесь, что фон остается прозрачным
        }

        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            // Не меняем цвет фона при потере фокуса
            this.BackColor = Color.Transparent; // Убедитесь, что фон остается прозрачным
        }
    }

}
