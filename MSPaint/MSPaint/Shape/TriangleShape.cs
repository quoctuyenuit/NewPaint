using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSPaint.Shape
{
    class TriangleShape : DrawingFrame
    {
        private Point p1;
        private Point p2;
        private Point p3;

        public TriangleShape(Size surfaceSize, Point p):base(surfaceSize, p)
        { }

        public override Bitmap CurrentShape
        {
            get { return this.generateImage(); }
        }

        public override void updateShape(Point _curPoint, Tools.DrawingProperties _properties, DrawingSetting.MoseStatus _mouseStatus)
        {
            base.updateShape(_curPoint, _properties, _mouseStatus);
            p1 = new Point(leftBound, lowerBound);
            p2 = new Point((rightBound - leftBound) / 2 + leftBound, upperBound);
            p3 = new Point(rightBound, lowerBound);
        }

        protected override Bitmap generateImage()
        {
            Bitmap bmp = new Bitmap(surfaceSize.Width, surfaceSize.Height);

            if (leftBound == rightBound && upperBound == lowerBound)
                return bmp;

            if (drawingStatus == DrawingSetting.DrawingStatus.PreDraw)
                return bmp;
            else
            {
                using (Graphics gr = Graphics.FromImage(bmp))
                {
                    gr.SmoothingMode = SmoothingMode.AntiAlias;

                    Pen pen = genratePen(this.drawingProperties);
                    pen.StartCap = LineCap.Round;
                    pen.EndCap = LineCap.Round;

                    

                    Point[] pArray = { p1, p2, p3 };
                    gr.DrawPolygon(pen, pArray);

                    if (Tools.PaintTools.BrushStatus == Tools.PaintTools.EnumBrushStatus.Fill)
                    {
                        gr.FillPolygon(drawingProperties.ActiveBrush, pArray);
                    }
                }
            }
            if (drawingStatus != DrawingSetting.DrawingStatus.Draw)
                drawFrame(bmp);

            return bmp;
        }
    }
}
