using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyPaint.Shape
{
    class FreePen : Shape
    {
        protected Bitmap currentShape;

        protected Point prePoint;

        protected bool doneStatus;

        public FreePen(Size surfaceSize, Point p):base(surfaceSize)
        {
            this.currentShape = new Bitmap(surfaceSize.Width, surfaceSize.Height);
            this.doneStatus = false;
        }

        public override void updateShape(System.Drawing.Point _curPoint, Tools.DrawingProperties _properties, DrawingSetting.MoseStatus _mouseStatus)
        {
            switch(_mouseStatus)
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
                            Pen customPen = genratePen(_properties);
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

        public override void setDoneStatus()
        {
        }

        public override System.Windows.Forms.Cursor checkCursor(System.Drawing.Point _curPoint)
        {
            return Cursors.Cross;
        }

        public override System.Drawing.Bitmap CurrentShape
        {
            get { return currentShape; }
        }

        public override bool DoneStatus
        {
            get {return doneStatus; }
        }
    }
}
