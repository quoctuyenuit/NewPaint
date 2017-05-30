using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyPaint.Drawing
{
    class MainPanel:System.Windows.Forms.Panel
    {
        private Stack<Bitmap> listBack;

        public Stack<Bitmap> ListBack
        {
            get { return listBack; }
            set { listBack = value; }
        }
        private DrawingPanel drawingPanel;

        internal DrawingPanel DrawingPanel
        {
            get { return drawingPanel; }
            set { drawingPanel = value; }
        }
        private ContentPanel contentPanel;

        internal ContentPanel ContentPanel
        {
            get { return contentPanel; }
            set { contentPanel = value; }
        }
        public MainPanel(Size _size)
        {
            this.Size = _size;

            this.DoubleClick += MainPanel_DoubleClick;
            this.MouseClick += MainPanel_MouseClick;
            this.MouseDown += MainPanel_MouseDown;
            this.MouseUp += MainPanel_MouseUp;
            this.MouseMove += MainPanel_MouseMove;
            this.Paint += MainPanel_Paint;
            this.Resize += MainPanel_Resize;
            this.drawingPanel = new DrawingPanel(this, _size);
            this.contentPanel = new ContentPanel(_size.Width, _size.Height);
            this.listBack = new Stack<Bitmap>();
            this.DoubleBuffered = true;
            this.Cursor = Cursors.Cross;
        }

        void MainPanel_DoubleClick(object sender, EventArgs e)
        {
            if(this.drawingPanel.ActiveShape is Shape.PolygonShape)
            {
                Shape.PolygonShape pl = (Shape.PolygonShape)this.drawingPanel.ActiveShape;
                pl.Finish = true;
            }
        }


        void MainPanel_Resize(object sender, EventArgs e)
        {
            this.drawingPanel.PaneSize = this.Size;
            this.contentPanel.resizePanel(this.Size.Width, this.Size.Height);
        }

        void MainPanel_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawImage(contentPanel.Content, new Point(0, 0));
            g.DrawImage(drawingPanel.Content, new Point(0, 0));
        }

        void MainPanel_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Tools.MainPaneMouseEventArgs _event = new Tools.MainPaneMouseEventArgs(e, contentPanel.Content);
            this.drawingPanel.doMouseMove(sender, _event);
        }

        void MainPanel_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Tools.MainPaneMouseEventArgs _event = new Tools.MainPaneMouseEventArgs(e, contentPanel.Content);
            this.drawingPanel.doMouseUp(sender, _event);
        }

        void MainPanel_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Tools.MainPaneMouseEventArgs _event = new Tools.MainPaneMouseEventArgs(e, contentPanel.Content);
            this.drawingPanel.doMouseDown(sender, _event);
        }

        void MainPanel_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
        }

        public void embedDrawing2Content()
        {
            Bitmap bmp = this.Image.Clone(new Rectangle(0, 0, this.Image.Width, this.Image.Height), this.Image.PixelFormat);
            
            this.listBack.Push(bmp);

            contentPanel.embedImage(drawingPanel.Content);
        }

        public void embed()
        {
            drawingPanel.embedImage();
        }

        public void refreshPanel()
        {
            this.Refresh();
        }

        public Bitmap Content
        {
            get { return contentPanel.Content; }
        }

        public Bitmap Image
        {
            get { return contentPanel.Content; }
            set
            {
                using(Graphics gr = Graphics.FromImage(contentPanel.Content))
                {
                    gr.DrawImage(value, 0, 0);
                }
            }
        }
    }
}
