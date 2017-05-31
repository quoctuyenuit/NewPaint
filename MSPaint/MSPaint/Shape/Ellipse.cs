using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MSPaint.Shape
{
    class EllipseShape : DrawingFrame
    {

        public EllipseShape(Size size, Point p)
            : base(size, p)
        {}

        public override Bitmap CurrentShape
        {
            get { return generateImage(); }
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
                    gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    Pen pen = genratePen(this.drawingProperties);
                    gr.DrawEllipse(pen, new System.Drawing.Rectangle(leftBound, upperBound, rightBound - leftBound, lowerBound - upperBound));
                    if (Tools.PaintTools.BrushStatus == Tools.PaintTools.EnumBrushStatus.Fill)
                    {
                        gr.FillEllipse(drawingProperties.ActiveBrush, new System.Drawing.Rectangle(leftBound, upperBound, rightBound - leftBound, lowerBound - upperBound));
                    }
                }
            }

            if (drawingStatus != DrawingSetting.DrawingStatus.Draw)
                drawFrame(bmp);

            return bmp;
        }
    }
}
