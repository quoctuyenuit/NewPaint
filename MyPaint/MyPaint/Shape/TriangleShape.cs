using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPaint.Shape
{
    class TriangleShape : DrawingFrame
    {
        public TriangleShape(Size surfaceSize, Point p):base(surfaceSize, p)
        { }

        public override Bitmap CurrentShape
        {
            get { return this.generateImage(); }
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

                    Point p1 = new Point(leftBound, lowerBound);
                    Point p2 = new Point((rightBound - leftBound) / 2 + leftBound, upperBound);
                    Point p3 = new Point(rightBound, lowerBound);

                    Point[] pArray = { p1, p2, p3 };
                    gr.DrawPolygon(pen, pArray);

                    if (Tools.PaintTools.BrushStatus == Tools.PaintTools.EnumBrushStatus.Fill)
                    {
                        gr.FillPolygon(drawingProperties.ActiveBrush, pArray);
                    }
                }
            }
            drawFrame(bmp);

            return bmp;
        }
    }
}
