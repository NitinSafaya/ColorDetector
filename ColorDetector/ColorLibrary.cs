using System;
using System.Drawing;
using System.IO;
using System.Threading;

namespace ColorDetector
{
    public class ColorLibrary
    {
        public enum DominantColor
        {
            White,
            Black,
            Gray,
            Red,
            Orange,
            Yellow,
            Green,
            Cyan,
            Blue,
            Magenta,
            Pink,
            Brown
        }

        public Image ReadImage(string filepath)
        {
            if(File.Exists(filepath)==true)
            {
                    Image img = Image.FromFile(@filepath);
                    return img;                
            }
            else
            {
                throw new Exception("File not forund");
            }
        }

        public Color GetColorAt(int x, int y)
        {
            Bitmap bmp = new Bitmap(1, 1);
            Rectangle bounds = new Rectangle(x, y, 1, 1);
            using (Graphics g = Graphics.FromImage(bmp))
                g.CopyFromScreen(bounds.Location, Point.Empty, bounds.Size);
            return bmp.GetPixel(0, 0);
        }
        public int GetDistance(Color current, Color known)
        {
            int redDifference;
            int greenDifference;
            int blueDifference;

            redDifference = current.R - known.R;
            greenDifference = current.G - known.G;
            blueDifference = current.B - known.B;

            int eucidValue = redDifference * redDifference + greenDifference * greenDifference +
                                   blueDifference * blueDifference;
            eucidValue = Convert.ToInt32(Math.Sqrt(eucidValue));
            return eucidValue;
        }

        public void Highlight(int x, int y)
        {
            Graphics g;
            using (g = Graphics.FromHwnd(IntPtr.Zero))
            {
                Pen yellowPen = new Pen(Brushes.Khaki);
                g.DrawEllipse(yellowPen, x - 4, y - 4, 10, 10);
                Thread.Sleep(1000);
                yellowPen.Width = 2.0F;
                g.DrawEllipse(yellowPen, x - 2, y - 2, 4, 4);
                Thread.Sleep(500);
                g.DrawEllipse(yellowPen, x, y, 1, 1);

            }
        }
        public string GetPrimaryColor(Color pixel)
        {
            string name = "Unknown";
            int lowestDist = 10000;
            foreach (DominantColor kc in Enum.GetValues(typeof(DominantColor)))
            {
                Color known = Color.FromName(kc.ToString());
                string tempName = Convert.ToString(known.Name).ToLower();
                if (!known.IsSystemColor)
                {
                    int distance = GetDistance(pixel, known);
                    if (distance < lowestDist)
                    {
                        lowestDist = distance;
                        name = known.Name;
                    }
                }
            }
            return name;
        }

        public string GetKnownColor(Color pixel)
        {
            string name = "Unknown";
            int lowestDist = 10000;
            foreach (KnownColor kc in Enum.GetValues(typeof(KnownColor)))
            {
                Color known = Color.FromKnownColor(kc);
                string tempName = Convert.ToString(known.Name).ToLower();
                if (!known.IsSystemColor)
                {
                    int distance = GetDistance(pixel, known);
                    if (distance < lowestDist)
                    {
                        lowestDist = distance;
                        name = known.Name;
                    }
                }
            }
            return name;
        }
    }
}
