using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSPaint.Drawing
{
    class ContentPanel
    {
        private Bitmap content;
        private Bitmap backGround;

        public Bitmap Content
        {
            get { return content; }
            set { content = value; }
        }

        public ContentPanel(Size size)
        {
            this.content = new Bitmap(size.Width, size.Height);

            this.backGround = new Bitmap(2000, 2000);
            for (int x = 0; x < this.backGround.Size.Width; x++)
                for (int y = 0; y < this.backGround.Size.Height; y++)
                    this.backGround.SetPixel(x, y, Color.White);

            Graphics g = Graphics.FromImage(content);
            g.DrawImage(this.backGround, new Point(0, 0));
        }

        public void RefreshContent(int newWidth, int newHeight)
        {
            Bitmap newContent = new Bitmap(newWidth, newHeight);
            Graphics g = Graphics.FromImage(newContent);
            g.DrawImage(this.backGround, new Point(0, 0));
            this.content = newContent;
        }

        public void resizePanel(int newWidth, int newHeight)
        {
            try
            {
                Bitmap newContent = new Bitmap(newWidth, newHeight);
                Graphics g = Graphics.FromImage(newContent);
                g.DrawImage(this.backGround, new Point(0, 0));
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
                g.DrawImage(img, new Point(0, 0));
            }
        }

    }
}
