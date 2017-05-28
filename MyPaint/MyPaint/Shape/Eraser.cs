using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPaint.Shape
{
    class Eraser:FreePen
    {
        public Eraser(Size surfaceSize, Point p)
            : base(surfaceSize, p)
        { }

        public override void updateShape(System.Drawing.Point _curPoint, Tools.DrawingProperties _properties, DrawingSetting.MoseStatus _mouseStatus)
        {
            switch (_mouseStatus)
            {
                case DrawingSetting.MoseStatus.Down:
                    {
                        this.prePoint.X = _curPoint.X;
                        this.prePoint.Y = _curPoint.Y;
                        break;
                    }
                case DrawingSetting.MoseStatus.Move:
                    {
                        using (Graphics g = Graphics.FromImage(currentShape))
                        {
                            Pen customPen = new Pen(Color.White);
                            customPen.Width = Tools.PaintTools.EraserWidth;
                            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                            g.DrawLine(customPen, prePoint, _curPoint);
                            prePoint = _curPoint;
                            break;
                        }
                    }
                case DrawingSetting.MoseStatus.Up:
                    {
                        this.doneStatus = true;
                        break;
                    }
            }
        }
    }
}
