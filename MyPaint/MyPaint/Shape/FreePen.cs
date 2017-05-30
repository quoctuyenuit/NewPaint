using MyPaint.Tools;
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

        protected List<Point> listPrePoint;

        protected bool doneStatus;

        public FreePen(Size surfaceSize, Point p)
            : base(surfaceSize)
        {
            this.currentShape = new Bitmap(surfaceSize.Width, surfaceSize.Height);
            this.doneStatus = false;
            this.listPrePoint = new List<Point>();
            this.prePoint = p;
            this.listPrePoint.Add(this.prePoint);
        }

        public override void updateShape(System.Drawing.Point _curPoint, Tools.DrawingProperties _properties, DrawingSetting.MoseStatus _mouseStatus)
        {
            drawingProperties = _properties;
            switch (_mouseStatus)
            {
                case DrawingSetting.MoseStatus.Down:
                    {
                        drawingStatus = DrawingSetting.DrawingStatus.Draw;
                        break;
                    }
                case DrawingSetting.MoseStatus.Move:
                    {
                        this.listPrePoint.Add(_curPoint);
                        break;
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

        private Bitmap generateImage()
        {
            Bitmap bmp = new Bitmap(surfaceSize.Width, surfaceSize.Height);

            if (drawingStatus == DrawingSetting.DrawingStatus.PreDraw)
                return bmp;
            else
            {
                using (Graphics gr = Graphics.FromImage(bmp))
                {
                    Pen pen = genratePen(this.drawingProperties);
                    pen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
                    pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                    gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                    if (doneStatus)
                    {
                        List<Point> tempList = new List<Point>();
                        for (int i = 0; i < listPrePoint.Count; i++)
                            if (i % 2 == 0)
                                tempList.Add(listPrePoint[i]);

                        if (tempList.Count > 1)
                            gr.DrawCurve(pen, tempList.ToArray(), 0.5f);
                    }
                    else
                    {
                        if (listPrePoint.Count > 1)
                            gr.DrawLines(pen, listPrePoint.ToArray());
                    }
                }
            }

            return bmp;
        }

        public override System.Drawing.Bitmap CurrentShape
        {
            get { return generateImage(); }
        }

        public override bool DoneStatus
        {
            get { return doneStatus; }
        }

        public override void RotateShape(double angleInDegrees)
        {
            throw new NotImplementedException();
        }
    }
}
