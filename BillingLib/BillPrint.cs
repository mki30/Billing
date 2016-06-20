using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Text.RegularExpressions;


namespace BillingLib
{
    public enum TextAlign
    {
        Left,
        Center,
        Right
    }

    public class BillPrint
    {

        public const int ScaleFactor = 1;

        public static void Box(Graphics g, float X, float Y, float Width, float Height)
        {
            SolidBrush blueBrush = new SolidBrush(Color.Yellow);
            g.FillRectangle(blueBrush, X, Y, Width, Height);
            blueBrush.Dispose();
        }

        public static void Line(Graphics g, float X, float Y, float Width)
        {
            // Create pen.
            Pen blackPen = new Pen(Color.Black, .3f);

            // Create points that define line.
            PointF point1 = new PointF(X, Y);
            PointF point2 = new PointF(X+Width, Y);

            // Draw line to screen.
            g.DrawLine(blackPen, point1, point2);
            blackPen.Dispose();

        }

        public static void PrintTxt(Graphics g, string txt, float x, float y, Boolean Rotate, Boolean IsBold)
        {
            Font Fn = new Font(FontInfo.FontName, FontInfo.FontSize, IsBold ? FontStyle.Bold : FontStyle.Regular);

            if (Rotate)
            {
                g.TranslateTransform(x, y);
                g.RotateTransform(-90);
            }

            if (txt != "")
            {
                string[] str = txt.Split('\n');
                y = 0;
                foreach (string s in str)
                {
                    g.DrawString(s, Fn, Brushes.Black, 0, y);
                    y += Fn.GetHeight();
                }
            }
            g.ResetTransform();
            g.ScaleTransform(ScaleFactor, ScaleFactor);
            Fn.Dispose();

        }

        public static void PrintTxt(Graphics g, string txt, float x, float y, float w, float h, ref float Maxheight)
        {
            Font Fn = new Font(FontInfo.FontName, FontInfo.FontSize, FontStyle.Regular);
            SizeF DrawArea = new SizeF(w, h);
            RectangleF R = new RectangleF(x, y, DrawArea.Width, DrawArea.Height);

            SizeF F = g.MeasureString(txt, Fn, DrawArea);
            g.DrawString(txt, Fn, Brushes.Black, R);
            if (F.Height > Maxheight)
                Maxheight = F.Height;
            Fn.Dispose();
        }

        public static SizeF PrintTxt(Graphics g, string txt, float x, float y, float w, float h, Brush brush, float backgroundheight, Boolean IsBold)
        {
            SizeF size = new SizeF();

            if (txt != "")
            {
                Font Fn = new Font(FontInfo.FontName, FontInfo.FontSize, IsBold ? FontStyle.Bold : FontStyle.Regular);
                g.DrawString("-", Fn, Brushes.White, x, y);

                if (brush != null)
                    g.FillRectangle(brush, x, y, w, backgroundheight);//horizontal background

                string[] str = txt.Split('\n');
                foreach (string s in str)
                {
                    if (w == 0 && h == 0)
                        g.DrawString(s, Fn, Brushes.Black, x, y);
                    else
                    {
                        StringFormat drawFormat = new StringFormat();
                        if (w != 0 && h != 0)
                        {
                            drawFormat.Alignment = StringAlignment.Center;
                            drawFormat.LineAlignment = StringAlignment.Center;
                            g.DrawString(s, Fn, Brushes.Black, new RectangleF(x - .5f, y, w + 1f, h), drawFormat);
                        }
                        else if (w != 0)
                        {
                            drawFormat.Alignment = StringAlignment.Center;
                            x -= w * .1f;
                            w += w * .2f;
                            //g.DrawString(s, Fn, Brushes.Black, new RectangleF(x - .5f, y, w + 1f, lineheight * 10), drawFormat);
                            g.DrawString(s, Fn, Brushes.Black, new RectangleF(x, y, w, Fn.GetHeight() * 10), drawFormat);
                        }
                        else
                        {
                            drawFormat.LineAlignment = StringAlignment.Center;
                            g.DrawString(s, Fn, Brushes.Black, new RectangleF(x, y, Fn.GetHeight() * 1000, h), drawFormat);
                            //g.DrawString(s, Fn, Brushes.Black, x, y);
                        }
                    }

                    y += Fn.GetHeight();
                }



                size = g.MeasureString(txt, Fn, new SizeF(w, 1000));

                Fn.Dispose();
            }

            return size;
        }

        public static SizeF TextSize(Graphics g, string txt, float w, Boolean IsBold = false)
        {
            Font fnB = new Font(FontInfo.FontName, FontInfo.FontSize, IsBold ? FontStyle.Bold : FontStyle.Regular);
            SizeF size = g.MeasureString(txt, fnB);

            if (w != 0)
            {
                SizeF DrawArea = new SizeF(w, 100);
                size = g.MeasureString(txt, fnB, DrawArea);
            }

            fnB.Dispose();
            return size;
        }

        public static SizeF PrintTxt(Graphics g, string txt, float x, float y, float w, TextAlign Align = TextAlign.Left, Boolean IsBold = false)
        {
            Font fnB = new Font(FontInfo.FontName, FontInfo.FontSize, IsBold ? FontStyle.Bold : FontStyle.Regular);
            SizeF size = g.MeasureString(txt, fnB);

            if (w != 0)
            {
                SizeF DrawArea = new SizeF(w, 100);
                RectangleF R = new RectangleF(x, y, DrawArea.Width, DrawArea.Height);

                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = Align == TextAlign.Right ? StringAlignment.Far : Align == TextAlign.Center ? StringAlignment.Center : StringAlignment.Near;
                g.DrawString(txt, fnB, Brushes.Black, R, stringFormat);
                size = g.MeasureString(txt, fnB, DrawArea);

            }
            else
                PrintTxt(g, txt, x, y, w, 0, null, 0, IsBold);

            fnB.Dispose();
            return size;
        }

        public static SizeF PrintTxt(Graphics g, string txt, float x, float y)
        {
            return PrintTxt(g, txt, x, y, 0, 0, null, 0, false);
        }

    }
}
