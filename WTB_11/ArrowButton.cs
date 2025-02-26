using System;
using System.Drawing;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ScrollBar;

namespace WPB_11;
public class ArrowButton : Button
{
    public ArrowButton()
    {
        InitializeArrowButton();
    }

    private void InitializeArrowButton() {
        this.Image = Image.FromFile("G:\\VisualStudio\\repos\\WTB_11\\WTB_11\\Img\\arrow.PNG");
        this.Height = 90;
        this.Width = 200;
        this.FlatStyle = FlatStyle.Flat;
        this.FlatAppearance.BorderSize = 0; // Убираем обводку
        this.FlatAppearance.MouseOverBackColor = Color.Transparent; // Убираем цвет фона при наведении
        this.FlatAppearance.MouseDownBackColor = Color.Transparent; // Убираем цвет фона при нажатии
    }
}


