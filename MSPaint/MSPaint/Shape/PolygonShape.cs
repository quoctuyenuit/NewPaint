using MSPaint.Tools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSPaint.Shape
{
    class PolygonShape : DrawingFrame
    {
        private List<Point> listPoint;
        private Point startPoint;
        private Point tempPoint;
        private bool finish;

        public bool Finish
        {
            get { return finish; }
            set { finish = value; }
        }

        struct Rate
        {
            public float X { get; set; }
            public float Y { get; set; }
        }

        private List<Rate> listRate;

        //loaiTyLe = true => tỷ lệ X ngược lại là tỷ lệ Y
        private float xacDinhTyLe(Point p, bool loaiTyLe)
        {
            return (loaiTyLe) ? ((float)(p.X - leftBound) / (rightBound - leftBound)) : ((float)(p.Y - upperBound) / (lowerBound - upperBound));
        }

        public PolygonShape(Size surfaceSize, Point p)
            : base(surfaceSize, p)
        {
            this.listPoint = new List<Point>();
            this.listRate = new List<Rate>();
            this.startPoint = p;
            this.listPoint.Add(startPoint);
            this.finish = false;
        }

        private bool isFinish(Point _curPoint)
        {
            return ((_curPoint.X > startPoint.X - pointRadius && _curPoint.X < startPoint.X + pointRadius)
                                 && (_curPoint.Y > startPoint.Y - pointRadius && _curPoint.Y < startPoint.Y + pointRadius));
        }

        public override void updateShape(Point _curPoint, Tools.DrawingProperties _properties, MSPaint.Shape.DrawingSetting.MoseStatus _mouseStatus)
        {
            this.drawingProperties = _properties;
            if (_mouseStatus == MSPaint.Shape.DrawingSetting.MoseStatus.Down)
            {
                tempPoint = _curPoint;
                #region MouseStatus = Down

                switch (drawingStatus)
                {
                    case DrawingSetting.DrawingStatus.PreDraw:
                        {
                            this.drawingStatus = DrawingSetting.DrawingStatus.Draw;
                            this.drawingMode = DrawingSetting.DrawingMode.DownRight;
                            break;
                        }
                    case DrawingSetting.DrawingStatus.Free:
                        {
                            this.drawingStatus = DrawingSetting.DrawingStatus.Adjust;
                            this.drawingMode = checkDrawingMode(_curPoint);
                            this.pivotMove = _curPoint;

                            if (finish)
                            {
                                switch (drawingMode)
                                {
                                    case DrawingSetting.DrawingMode.Normal:
                                        {
                                            this.doneStatus = true;
                                            break;
                                        }
                                    case DrawingSetting.DrawingMode.UpLeft:
                                    case DrawingSetting.DrawingMode.UpRight:
                                    case DrawingSetting.DrawingMode.DownLeft:
                                    case DrawingSetting.DrawingMode.DownRight:
                                    case DrawingSetting.DrawingMode.VerUp:
                                    case DrawingSetting.DrawingMode.VerDown:
                                    case DrawingSetting.DrawingMode.HonRight:
                                    case DrawingSetting.DrawingMode.HonLeft:
                                        {
                                            this.drawingStatus = DrawingSetting.DrawingStatus.Adjust;
                                            break;
                                        }
                                }
                            }
                            else
                            {
                                this.drawingMode = DrawingSetting.DrawingMode.DownRight;
                                this.drawingStatus = DrawingSetting.DrawingStatus.Draw;
                            }

                            break;
                        }
                }
                #endregion
            }

            if (_mouseStatus == DrawingSetting.MoseStatus.Move)
            {
                #region MouseMove
                if (drawingStatus == DrawingSetting.DrawingStatus.Draw || drawingStatus == DrawingSetting.DrawingStatus.Adjust)
                {
                    if (drawingStatus == DrawingSetting.DrawingStatus.Draw)
                        tempPoint = _curPoint;
                    else
                    {
                        #region ChangeFrame
                        switch (drawingMode)
                        {
                            case DrawingSetting.DrawingMode.Drawing:
                            case DrawingSetting.DrawingMode.HonLeft:
                                {
                                    leftBound = Math.Min(_curPoint.X, rightBound - pointRadius);
                                    break;
                                }
                            case DrawingSetting.DrawingMode.HonRight:
                                {
                                    rightBound = Math.Max(_curPoint.X, leftBound + pointRadius);
                                    break;
                                }
                            case DrawingSetting.DrawingMode.VerUp:
                                {
                                    upperBound = Math.Min(_curPoint.Y, lowerBound - pointRadius);
                                    break;
                                }
                            case DrawingSetting.DrawingMode.VerDown:
                                {
                                    lowerBound = Math.Max(_curPoint.Y, upperBound + pointRadius);
                                    break;
                                }
                            case DrawingSetting.DrawingMode.UpLeft:
                                {
                                    upperBound = Math.Min(_curPoint.Y, lowerBound - pointRadius);
                                    leftBound = Math.Min(_curPoint.X, rightBound - pointRadius);
                                    break;
                                }
                            case DrawingSetting.DrawingMode.DownRight:
                                {
                                    lowerBound = Math.Max(_curPoint.Y, upperBound + pointRadius);
                                    rightBound = Math.Max(_curPoint.X, leftBound + pointRadius);
                                    break;
                                }
                            case DrawingSetting.DrawingMode.UpRight:
                                {
                                    upperBound = Math.Min(_curPoint.Y, lowerBound - pointRadius);
                                    rightBound = Math.Max(_curPoint.X, leftBound + pointRadius);
                                    break;
                                }
                            case DrawingSetting.DrawingMode.DownLeft:
                                {
                                    lowerBound = Math.Max(_curPoint.Y, upperBound + pointRadius);
                                    leftBound = Math.Min(_curPoint.X, rightBound - pointRadius);
                                    break;
                                }
                            case DrawingSetting.DrawingMode.Move:
                                {
                                    this.leftBound += _curPoint.X - pivotMove.X;
                                    this.rightBound += _curPoint.X - pivotMove.X;
                                    this.upperBound += _curPoint.Y - pivotMove.Y;
                                    this.lowerBound += _curPoint.Y - pivotMove.Y;

                                    for (int i = 0; i < listPoint.Count; i++)
                                    {
                                        Point p = listPoint[i];
                                        p.X += _curPoint.X - pivotMove.X;
                                        p.Y += _curPoint.Y - pivotMove.Y;

                                        listPoint[i] = p;
                                    }

                                    this.pivotMove.X = _curPoint.X;
                                    this.pivotMove.Y = _curPoint.Y;
                                    break;
                                }
                        }
                        #endregion

                        if (drawingMode != DrawingSetting.DrawingMode.Move && drawingMode != DrawingSetting.DrawingMode.Normal)
                        {
                            for (int i = 0; i < listRate.Count; i++)
                            {
                                Point p = listPoint[i];
                                p.X = (int)(leftBound + listRate[i].X * (rightBound - leftBound));
                                p.Y = (int)(upperBound + listRate[i].Y * (lowerBound - upperBound));

                                this.listPoint[i] = p;
                            }
                        }
                    }
                }
                #endregion
            }
            if (_mouseStatus == DrawingSetting.MoseStatus.Up)
            {

                if (!finish)
                {

                    listPoint.Add(new Point() { X = tempPoint.X, Y = tempPoint.Y });
                    int minX = startPoint.X, minY = startPoint.Y, maxX = startPoint.X, maxY = startPoint.Y;
                    foreach (Point p in listPoint)
                    {
                        minX = Math.Min(minX, p.X);
                        minY = Math.Min(minY, p.Y);
                        maxX = Math.Max(maxX, p.X);
                        maxY = Math.Max(maxY, p.Y);
                    }

                    leftBound = minX;
                    rightBound = maxX;
                    upperBound = minY;
                    lowerBound = maxY;

                    finish = isFinish(_curPoint);
                    if (finish)
                    {
                        getRateListPoint();
                    }
                }

                #region Set DrawingStatus After MouseUp

                if (leftBound == rightBound && upperBound == lowerBound)
                {
                    drawingStatus = DrawingSetting.DrawingStatus.PreDraw;
                    drawingMode = DrawingSetting.DrawingMode.Normal;
                    this.doneStatus = true;
                    return;
                }

                switch (drawingStatus)
                {
                    case DrawingSetting.DrawingStatus.Draw:
                        {
                            drawingStatus = DrawingSetting.DrawingStatus.Free;
                            break;
                        }
                    case DrawingSetting.DrawingStatus.Adjust:
                        {
                            drawingStatus = DrawingSetting.DrawingStatus.Free;
                            break;
                        }
                }
                #endregion
            }
        }

        public void getRateListPoint()
        {
            //Lưu lại tỷ lệ các điểm trong hình sau khi vừa vẽ xong
            foreach (Point p in listPoint)
                this.listRate.Add(new Rate() { X = xacDinhTyLe(p, true), Y = xacDinhTyLe(p, false) });
        }

        protected override System.Drawing.Bitmap generateImage()
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

                    if (finish)
                        gr.DrawPolygon(pen, listPoint.ToArray());
                    else
                    {
                        if (listPoint.Count > 1)
                            gr.DrawLines(pen, listPoint.ToArray());
                        gr.DrawLine(pen, listPoint.Last(), tempPoint);
                    }

                    if (Tools.PaintTools.BrushStatus == Tools.PaintTools.EnumBrushStatus.Fill)
                    {
                        gr.FillPolygon(drawingProperties.ActiveBrush, listPoint.ToArray());
                    }
                }
            }
            if (finish)
                drawFrame(bmp);
            return bmp;
        }

        public override System.Windows.Forms.Cursor checkCursor(Point _curPoint)
        {
            if (!finish)
                return System.Windows.Forms.Cursors.Cross;
            return base.checkCursor(_curPoint);
        }
    }
}
