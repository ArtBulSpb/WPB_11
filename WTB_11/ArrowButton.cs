using System;
using System.Drawing;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ScrollBar;

namespace WPB_11;
public class ArrowButton : Button
{
    public ArrowButton(string path, int height, int width)
    {
        this.Image = Image.FromFile(path);
        this.Height = height;
        this.Width = width;
        this.FlatStyle = FlatStyle.Flat;
        this.FlatAppearance.BorderSize = 0; // Убираем обводку
        this.FlatAppearance.MouseOverBackColor = Color.Transparent; // Убираем цвет фона при наведении
        this.FlatAppearance.MouseDownBackColor = Color.Transparent; // Убираем цвет фона при нажатии
    }

}


