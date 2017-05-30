using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPaint.Drawing
{
    class DrawingPanel
    {
        public delegate void MainPaneMouseEventHandler(object sender, Tools.MainPaneMouseEventArgs e);
        public event MainPaneMouseEventHandler MouseDown;
        public event MainPaneMouseEventHandler MouseMove;
        public event MainPaneMouseEventHandler MouseUp;

        private MainPanel mainPanel;

        private Shape.Shape activeShape;

        internal Shape.Shape ActiveShape
        {
            get { return activeShape; }
            set { activeShape = value; }
        }

        private Bitmap content;

        public Bitmap Content
        {
            get { return content; }
            set { content = value; }
        }

      
        private Size paneSize;

        public Size PaneSize
        {
            get { return paneSize; }
            set { paneSize = value; }
        }

        public DrawingPanel(MainPanel mainPanel, Size size)
        {
            this.mainPanel = mainPanel;
            this.paneSize = size;
            this.content = new Bitmap(this.paneSize.Width, this.paneSize.Height);
            this.content.MakeTransparent();
            this.MouseDown += DrawingPanel_MouseDown;
            this.MouseMove += DrawingPanel_MouseMove;
            this.MouseUp += DrawingPanel_MouseUp;
        }

        public void updateContent()
        {
            content = activeShape.CurrentShape;
            if (activeShape.DoneStatus)
            {
                mainPanel.embedDrawing2Content();
                activeShape = null;
            }
            mainPanel.refreshPanel();
        }

        private Tools.DrawingProperties getDrawingProperties()
        {
            Tools.DrawingProperties props = new Tools.DrawingProperties();
            props.PenWidth = Tools.PaintTools.PenWidth;
            props.ActiveColor = Tools.PaintTools.DrawingColor;
            props.ActiveBrush = Tools.PaintTools.DrawingBrush;
            return props;
        }

        public void embedImage()
        {
            if (activeShape != null)
            {
                activeShape.setDoneStatus();
                updateContent();
            }
        }

        #region Event

        void DrawingPanel_MouseDown(object sender, Tools.MainPaneMouseEventArgs e)
        {
            Tools.DrawingProperties props;
            if(activeShape != null)
            {
                props = getDrawingProperties();
                activeShape.updateShape(e.Location, props, Shape.DrawingSetting.MoseStatus.Down);
                this.updateContent();
            }

            if (activeShape == null)
            {
                switch (Tools.PaintTools.DrawingTool)
                {
                    case Tools.PaintTools.EnumDrawingTool.Rectangle:
                        {
                            activeShape = new Shape.RectangleShape(this.paneSize, e.Location);
                            break;
                        }
                    case Tools.PaintTools.EnumDrawingTool.FreePen:
                        {
                            activeShape = new Shape.FreePen(this.paneSize, e.Location);
                            break;
                        }
                    case Tools.PaintTools.EnumDrawingTool.Ellipse:
                        {
                            activeShape = new Shape.EllipseShape(this.paneSize, e.Location);
                            break;
                        }
                    case Tools.PaintTools.EnumDrawingTool.Line:
                        {
                            activeShape = new Shape.LineShape(this.paneSize, e.Location);
                            break;
                        }
                    case Tools.PaintTools.EnumDrawingTool.Triangle:
                        {
                            activeShape = new Shape.TriangleShape(this.paneSize, e.Location);
                            break;
                        }
                    case Tools.PaintTools.EnumDrawingTool.SquareTriangle:
                        {
                            activeShape = new Shape.SquareTriangleShape(this.paneSize, e.Location);
                            break;
                        }
                    case Tools.PaintTools.EnumDrawingTool.Diamond:
                        {
                            activeShape = new Shape.DiamondShape(this.paneSize, e.Location);
                            break;
                        }
                    case Tools.PaintTools.EnumDrawingTool.Pentagon:
                        {
                            activeShape = new Shape.PentagonShape(this.paneSize, e.Location);
                            break;
                        }
                    case Tools.PaintTools.EnumDrawingTool.DownArrow:
                        {
                            activeShape = new Shape.DownArowShape(this.paneSize, e.Location);
                            break;
                        }
                    case Tools.PaintTools.EnumDrawingTool.UpArrow:
                        {
                            activeShape = new Shape.UpArrowShape(this.paneSize, e.Location);
                            break;
                        }
                    case Tools.PaintTools.EnumDrawingTool.RightArrow:
                        {
                            activeShape = new Shape.RightArrowShape(this.paneSize, e.Location);
                            break;
                        }
                    case Tools.PaintTools.EnumDrawingTool.LeftArrow:
                        {
                            activeShape = new Shape.LeftArrowShape(this.paneSize, e.Location);
                            break;
                        }
                    case Tools.PaintTools.EnumDrawingTool.Eraser:
                        {
                            activeShape = new Shape.Eraser(this.paneSize, e.Location);
                            break;
                        }
                    case Tools.PaintTools.EnumDrawingTool.Select:
                        {
                            activeShape = new Shape.SelectShape(this.paneSize, e.Location, this.mainPanel);
                            break;
                        }
                    case Tools.PaintTools.EnumDrawingTool.Polygon:
                        {
                            activeShape = new Shape.PolygonShape(this.paneSize, e.Location);
                            break;
                        }
                    case Tools.PaintTools.EnumDrawingTool.Bezier:
                        {
                            activeShape = new Shape.BezierShape(this.paneSize, e.Location);
                            break;
                        }
                }
            }
            props = getDrawingProperties();
            activeShape.updateShape(e.Location, props, Shape.DrawingSetting.MoseStatus.Down);
            this.updateContent();
            if (activeShape == null)
                mainPanel.Cursor = System.Windows.Forms.Cursors.Cross;
            else
                mainPanel.Cursor = activeShape.checkCursor(e.Location);
        }

        void DrawingPanel_MouseUp(object sender, Tools.MainPaneMouseEventArgs e)
        {
            if (activeShape != null)
            {
                Tools.DrawingProperties props = getDrawingProperties();
                activeShape.updateShape(e.Location, props, Shape.DrawingSetting.MoseStatus.Up);
                updateContent();
                if (activeShape == null)
                    mainPanel.Cursor = System.Windows.Forms.Cursors.Cross;
                else
                    mainPanel.Cursor = activeShape.checkCursor(e.Location);
            }
        }

        void DrawingPanel_MouseMove(object sender, Tools.MainPaneMouseEventArgs e)
        {
            if (activeShape != null)
            {
                Tools.DrawingProperties props = getDrawingProperties();
                activeShape.updateShape(e.Location, props, Shape.DrawingSetting.MoseStatus.Move);
                updateContent();
                if (activeShape == null)
                    mainPanel.Cursor = System.Windows.Forms.Cursors.Cross;
                else
                     mainPanel.Cursor = activeShape.checkCursor(e.Location);
            }
        }

        public void doMouseUp(object sender, Tools.MainPaneMouseEventArgs e)
        {
            this.MouseUp(sender, e);
        }

        public void doMouseDown(object sender, Tools.MainPaneMouseEventArgs e)
        {
            this.MouseDown(sender, e);
        }

        public void doMouseMove(object sender, Tools.MainPaneMouseEventArgs e)
        {
            this.MouseMove(sender, e);
        }

        #endregion
    }
}
