using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MSPaint.Drawing
{
    class MainPanel : System.Windows.Forms.Panel
    {
        private Stack<Bitmap> listBack;

        //Bán kính phát hiện sự kiện cho phép chỉnh sửa size bảng vẽ
        private const int pointRadius = 20;

        //Bán kính vẽ hình vuông nhỏ dấu hiệu chỉnh sửa size bảng vẽ
        public const int HANDLE_POINT_RADIUS = 2;

        private Shape.DrawingSetting.DrawingStatus drawingStatus;
        private Shape.DrawingSetting.DrawingMode drawingMode;
        private Tools.PaintTools.EnumDrawingTool tempTools;//Lưu lại loại vẽ trước
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
            this.MouseDown += MainPanel_MouseDown;
            this.MouseUp += MainPanel_MouseUp;
            this.MouseMove += MainPanel_MouseMove;
            this.Paint += MainPanel_Paint;
            this.Resize += MainPanel_Resize;
            this.drawingPanel = new DrawingPanel(this, new Size(this.Size.Width - 2 * HANDLE_POINT_RADIUS, this.Size.Height - 2 * HANDLE_POINT_RADIUS));
            this.contentPanel = new ContentPanel(this.drawingPanel.PaneSize);

            
            this.listBack = new Stack<Bitmap>();
            this.DoubleBuffered = true;
            this.Cursor = Cursors.Cross;
            this.drawingStatus = Shape.DrawingSetting.DrawingStatus.Free;
            this.tempTools = Tools.PaintTools.DrawingTool;
        }

        void MainPanel_DoubleClick(object sender, EventArgs e)
        {
            if (this.drawingPanel.ActiveShape is Shape.PolygonShape)
            {
                Shape.PolygonShape pl = (Shape.PolygonShape)this.drawingPanel.ActiveShape;
                pl.Finish = true;
                pl.getRateListPoint();
            }
        }

        private MSPaint.Shape.DrawingSetting.DrawingMode checkDrawingMode(Point _curPoint)
        {
            int X = _curPoint.X;
            int Y = _curPoint.Y;
            Point p = new Point(this.Size.Width, this.Size.Height);

            if ((X > p.X - pointRadius) && (X < p.X + pointRadius) && (Y > p.Y - pointRadius) && (Y < p.Y + pointRadius))
                return MSPaint.Shape.DrawingSetting.DrawingMode.DownRight;

            if (X >= this.Size.Width - pointRadius && X <= this.Size.Width + pointRadius && Y >= 0 && Y <= this.Size.Height)
                return MSPaint.Shape.DrawingSetting.DrawingMode.HonRight;

            if (X >= 0 && X <= this.Size.Width && Y >= this.Size.Height - pointRadius && Y < this.Size.Height + pointRadius)
                return Shape.DrawingSetting.DrawingMode.VerDown;

            return Shape.DrawingSetting.DrawingMode.Normal;
        }

        public System.Windows.Forms.Cursor checkCursor(Point _curPoint)
        {
            switch (checkDrawingMode(_curPoint))
            {
                case MSPaint.Shape.DrawingSetting.DrawingMode.DownRight:
                    return Cursors.SizeNWSE;
                case MSPaint.Shape.DrawingSetting.DrawingMode.HonRight:
                    return Cursors.SizeWE;
                case MSPaint.Shape.DrawingSetting.DrawingMode.VerDown:
                    return Cursors.SizeNS;
                default: return Cursors.Cross;
            }
        }

        #region Event
        public void MainPanel_Resize(object sender, EventArgs e)
        {
            int newWidth = (this.Size.Width > 2 * HANDLE_POINT_RADIUS) ? this.Size.Width - 2 * HANDLE_POINT_RADIUS : 1;
            int newHeight = (this.Size.Height > 2 * HANDLE_POINT_RADIUS) ? this.Size.Height - 2 * HANDLE_POINT_RADIUS : 1;
            this.drawingPanel.PaneSize = new Size(newWidth, newHeight);
            this.drawingPanel.Content = new Bitmap(this.drawingPanel.PaneSize.Width, this.drawingPanel.PaneSize.Height);
            this.contentPanel.resizePanel(this.drawingPanel.PaneSize.Width, this.drawingPanel.PaneSize.Height);
            this.Refresh();
        }

        void MainPanel_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawImage(contentPanel.Content, new Point(0, 0));
            g.DrawImage(drawingPanel.Content, new Point(0, 0));

        }

        void MainPanel_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            //Cho phép đổi Cursor trong khi không vẽ gì
            if(drawingPanel.ActiveShape == null)
                this.Cursor = checkCursor(e.Location);

            if (this.drawingStatus == Shape.DrawingSetting.DrawingStatus.Adjust)
            {
                switch (this.drawingMode)
                {
                    case Shape.DrawingSetting.DrawingMode.HonRight:
                        {
                            Size tempSize = new Size(e.Location.X, this.Size.Height);
                            this.Size = tempSize;
                            break;
                        }
                    case Shape.DrawingSetting.DrawingMode.DownRight:
                        {
                            Size tempSize = new Size(e.X, e.Y);
                            this.Size = tempSize;
                            break;
                        }
                    case Shape.DrawingSetting.DrawingMode.VerDown:
                        {
                            Size tempSize = new Size(this.Size.Width, e.Y);
                            this.Size = tempSize;
                            break;
                        }
                }
            }
            Tools.MainPaneMouseEventArgs _event = new Tools.MainPaneMouseEventArgs(e, contentPanel.Content);
            this.drawingPanel.doMouseMove(sender, _event);
        }

        void MainPanel_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Tools.PaintTools.DrawingTool = tempTools;
            this.drawingStatus = Shape.DrawingSetting.DrawingStatus.Free;
            Tools.MainPaneMouseEventArgs _event = new Tools.MainPaneMouseEventArgs(e, contentPanel.Content);
            this.drawingPanel.doMouseUp(sender, _event);
        }

        void MainPanel_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (checkCursor(e.Location) != Cursors.Cross && this.drawingPanel.ActiveShape == null)
            {
                this.drawingStatus = Shape.DrawingSetting.DrawingStatus.Adjust;
                this.drawingMode = checkDrawingMode(e.Location);
                this.tempTools = Tools.PaintTools.DrawingTool;
                Tools.PaintTools.DrawingTool = Tools.PaintTools.EnumDrawingTool.Free;
            }
            else
            {
                this.drawingStatus = Shape.DrawingSetting.DrawingStatus.Draw;
                this.tempTools = Tools.PaintTools.DrawingTool;
            }
            Tools.MainPaneMouseEventArgs _event = new Tools.MainPaneMouseEventArgs(e, contentPanel.Content);
            this.drawingPanel.doMouseDown(sender, _event);
        }
        #endregion

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
                using (Graphics gr = Graphics.FromImage(contentPanel.Content))
                {
                    gr.DrawImage(value, 0, 0);
                }
            }
        }
    }
}
