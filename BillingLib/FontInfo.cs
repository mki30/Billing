using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingLib
{
    public class FontInfo
    {
        public float Height;
        public float Width;
        public float Ascent;
        public float Descent;
        public static string FontName = "Helvetica";
        //public static string FontName = "Tw Cen MT Condensed";
        public static float FontSize = 9;

        public static FontInfo GetFontSize(Graphics g, string txt, Boolean IsBold)
        {
            SizeF s = new SizeF(100, 1000);
            return GetFontSize(g, txt, IsBold, s);
        }

        public static FontInfo GetFontSize(Graphics g, string txt)
        {
            SizeF s = new SizeF(100, 1000);
            return GetFontSize(g, txt, false, s);
        }

        public static FontInfo GetFontSize(Graphics g, string txt, Boolean IsBold, SizeF LayoutArea)
        {
            FontInfo FI = new FontInfo();
            Font fn = new Font(FontName, FontSize, IsBold ? FontStyle.Bold : FontStyle.Regular);
            SizeF s = g.MeasureString(txt, fn, LayoutArea);
            FI.Height = s.Height;
            FI.Width = s.Width;
            FI.Descent = s.Height * fn.FontFamily.GetCellDescent(fn.Style) / fn.FontFamily.GetEmHeight(fn.Style);
            FI.Ascent = s.Height * fn.FontFamily.GetCellAscent(fn.Style) / fn.FontFamily.GetEmHeight(fn.Style);
            fn.Dispose();
            return FI;
        }
    }
}
