﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPaint.Shape
{
    class RightArrowShape : DrawingFrame
    {
        public RightArrowShape(Size surfaceSize, Point p)
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
                    int halfHorizontal = (rightBound - leftBound) / 4;
                    int halfVertical = (lowerBound - upperBound) / 4;


                    Point p1 = new Point(leftBound, halfVertical + upperBound);
                    Point p2 = new Point(leftBound, halfVertical * 3 + upperBound);
                    Point p3 = new Point(halfHorizontal * 2 + leftBound, halfVertical * 3 + upperBound);
                    Point p4 = new Point(halfHorizontal * 2 + leftBound, lowerBound);
                    Point p5 = new Point(rightBound, halfVertical * 2 + upperBound);
                    Point p6 = new Point(halfHorizontal * 2 + leftBound, upperBound);
                    Point p7 = new Point(halfHorizontal * 2 + leftBound, upperBound + halfVertical);

                    Point[] pArray = { p1, p2, p3, p4, p5, p6, p7 };
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
