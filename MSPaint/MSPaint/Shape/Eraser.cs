using MyPaint.Tools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPaint.Shape
{
    class Eraser : FreePen
    {
        public Eraser(Size surfaceSize, Point p)
            : base(surfaceSize, p)
        { }

        public override System.Drawing.Bitmap CurrentShape
        {
            get { return generateImage(); }
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
                    Pen pen = new Pen(Color.White);
                    pen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
                    pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                    pen.Width = Tools.PaintTools.EraserWidth;

                    gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                    if (doneStatus)
                    {
                        List<Point> tempList = new List<Point>();
                        for (int i = 0; i < listPrePoint.Count; i++)
                            if (i % 2 == 0)
                                tempList.Add(listPrePoint[i]);

                        if (tempList.Count <= 1)
                            tempList.Add(new Point() { X = listPrePoint.First().X + 1, Y = listPrePoint.First().Y });

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

    }
}
