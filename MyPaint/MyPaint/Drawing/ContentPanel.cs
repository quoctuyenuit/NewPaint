using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPaint.Drawing
{
    class ContentPanel
    {
        private Bitmap content;

        public Bitmap Content
        {
            get { return content; }
            set { content = value; }
        }

        public ContentPanel(int width, int height)
        {
            this.content = new Bitmap(width, height);
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    this.content.SetPixel(x, y, Color.White);
        }

        public void resizePanel(int newWidth, int newHeight)
        {
            try
            {
                Bitmap newContent = new Bitmap(newWidth, newHeight);
                Graphics g = Graphics.FromImage(newContent);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.DrawImage(content, 0, 0);
                this.content = newContent;
            }
            catch (Exception ex)
            { }
        }

        public void embedImage(Bitmap img)
        {
            using(Graphics g = Graphics.FromImage(content))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.DrawImage(img, new Point(0, 0));
            }
        }

    }
}
