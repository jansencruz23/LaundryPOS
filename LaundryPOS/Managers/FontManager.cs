using Guna.UI2.WinForms;
using System.Drawing.Text;

namespace LaundryPOS.Managers
{
    public class FontManager
    {
        private PrivateFontCollection fonts;
        private Dictionary<string, int> fontsDict;

        public FontManager()
        {
            InitializeFonts();
        }

        private void InitializeFonts()
        {
            fonts = new();
            fonts.AddFontFile("Fonts\\Helvetica.ttf");
            fonts.AddFontFile("Fonts\\FreeSansBold.ttf");
            fonts.AddFontFile("Fonts\\Montserrat-Bold.ttf");
            fonts.AddFontFile("Fonts\\fake receipt.otf");

            fontsDict = new()
            {
                { "HELVETICA", 2 },
                { "HELVETICA_BOLD", 1 },
                { "MONTSERRAT", 3 },
                { "FAKE_RECEIPT", 0 }
            };
        }

        public Font Helvetica(float size)
            => new(fonts.Families[fontsDict["HELVETICA"]], size);

        public Font HelveticaBold(float size)
            => new(fonts.Families[fontsDict["HELVETICA_BOLD"]], size);

        public Font Montserrat(float size)
           => new(fonts.Families[fontsDict["MONTSERRAT"]], size);

        public Font FakeReceipt(float size)
           => new(fonts.Families[fontsDict["FAKE_RECEIPT"]], size);

        public void StyleFontsButton(float size, params Guna2Button[] buttons)
        {
            Array.ForEach(buttons, btn
                => btn.Font = HelveticaBold(size));
        }

        public void StyleFontsTextBox(float size, params Guna2TextBox[] textbox)
        {
            Array.ForEach(textbox, txt
                => txt.Font = Helvetica(size));
        }

        public void StyleFontsLabel(float size, bool bold, params Label[] labels)
        {
            if (bold)
            {
                Array.ForEach(labels, lbl
                    => lbl.Font = HelveticaBold(size));
            }
            else
            {
                Array.ForEach(labels, lbl
                    => lbl.Font = Helvetica(size));
            }
        }
    }
}