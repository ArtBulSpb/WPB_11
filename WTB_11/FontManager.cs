using System.Drawing.Text;

namespace WPB_11
{
    public static class FontManager
    {
        private static PrivateFontCollection privateFonts = new PrivateFontCollection();

        static FontManager()
        {
            LoadCustomFonts();
        }

        private static void LoadCustomFonts()
        {
            privateFonts.AddFontFile("G:\\VisualStudio\\repos\\WTB_11\\WTB_11\\Inter-Medium.otf"); // Путь к файлу шрифта
            privateFonts.AddFontFile("G:\\VisualStudio\\repos\\WTB_11\\WTB_11\\Inter-Regular.otf"); // Путь к файлу полужирного шрифта
            privateFonts.AddFontFile("G:\\VisualStudio\\repos\\WTB_11\\WTB_11\\Inter-SemiBold.otf");
        }

        public static Font GetMediumFont(float size)
        {
            return new Font(privateFonts.Families[0], size, FontStyle.Regular);
        }

        public static Font GetRegularFont(float size)
        {
            return new Font(privateFonts.Families[1], size, FontStyle.Regular);
        }

        public static Font GetSemiBoldFont(float size)
        {
            return new Font(privateFonts.Families[2], size, FontStyle.Bold);
        }
    }
}
