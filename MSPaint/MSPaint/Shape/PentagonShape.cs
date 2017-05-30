using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPaint.Shape
{
    class PentagonShape:DrawingFrame
    {
        public PentagonShape(Size surfaceSize, Point p)
            : base(surfaceSize, p)
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

                    Point p1 = new Point((rightBound - leftBound) / 2 + leftBound, upperBound);
                    Point p2 = new Point(rightBound, (int)((lowerBound - upperBound) * (0.39) + upperBound));
                    Point p3 = new Point((int)(rightBound - (rightBound - leftBound) * (0.19)), lowerBound);
                    Point p4 = new Point((int)(leftBound + (rightBound - leftBound) * (0.19)), lowerBound);
                    Point p5 = new Point(leftBound, (int)((lowerBound - upperBound) * (0.39) + upperBound));

                    Point[] pArray = { p1, p2, p3, p4, p5 };
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
