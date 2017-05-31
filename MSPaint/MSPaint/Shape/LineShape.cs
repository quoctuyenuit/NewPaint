using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MSPaint.Shape
{
    class LineShape : Shape
    {
        private const int HANDLE_POINT_RADIUS = 2;

        private Point pointAhead;

        private Point pointTail;

        private bool doneStatus;
        private Point pivotMove;

        public LineShape(Size surfaceSize, Point _pointStart)
            : base(surfaceSize)
        {
            this.pointAhead.X = _pointStart.X;
            this.pointAhead.Y = _pointStart.Y;

            this.pointTail.X = _pointStart.X;
            this.pointTail.Y = _pointStart.Y;
            this.doneStatus = false;
            this.drawingStatus = DrawingSetting.DrawingStatus.PreDraw;
        }

        public override void updateShape(System.Drawing.Point _curPoint, Tools.DrawingProperties _properties, DrawingSetting.MoseStatus _mouseStatus)
        {
            this.drawingProperties = _properties;
            if (_mouseStatus == DrawingSetting.MoseStatus.Down)
            {
                switch (drawingStatus)
                {
                    case DrawingSetting.DrawingStatus.PreDraw:
                        {
                            this.drawingStatus = DrawingSetting.DrawingStatus.Draw;
                            this.drawingMode = DrawingSetting.DrawingMode.VerDown;
                            break;
                        }
                    case DrawingSetting.DrawingStatus.Free:
                        {
                            this.drawingStatus = DrawingSetting.DrawingStatus.Adjust;
                            this.drawingMode = checkDrawingMode(_curPoint);
                            this.pivotMove = _curPoint;
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
                            break;
                        }
                }
            }

            if (_mouseStatus == DrawingSetting.MoseStatus.Move)
            {
                #region Switch DrawingMode
                if (drawingStatus == DrawingSetting.DrawingStatus.Draw || drawingStatus == DrawingSetting.DrawingStatus.Adjust)
                {
                    switch (drawingMode)
                    {

                        case DrawingSetting.DrawingMode.VerUp:
                            {
                                this.pointTail.X = _curPoint.X;
                                this.pointTail.Y = _curPoint.Y;
                                break;
                            }
                        case DrawingSetting.DrawingMode.VerDown:
                            {
                                this.pointAhead.X = _curPoint.X;
                                this.pointAhead.Y = _curPoint.Y;

                                break;
                            }
                        case DrawingSetting.DrawingMode.Move:
                            {
                                this.pointTail.X += _curPoint.X - this.pivotMove.X;
                                this.pointTail.Y += _curPoint.Y - this.pivotMove.Y;

                                this.pointAhead.X += _curPoint.X - this.pivotMove.X;
                                this.pointAhead.Y += _curPoint.Y - this.pivotMove.Y;

                                this.pivotMove.X = _curPoint.X;
                                this.pivotMove.Y = _curPoint.Y;
                                break;
                            }
                    }
                }
                #endregion
            }

            if (_mouseStatus == DrawingSetting.MoseStatus.Up)
            {
                #region Set DrawingStatus After MouseUp

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

        private DrawingSetting.DrawingMode checkDrawingMode(Point _curPoint)
        {
            int X = _curPoint.X;
            int Y = _curPoint.Y;
            if (((X > pointAhead.X - pointRadius) && (X < pointAhead.X + pointRadius) && (Y > pointAhead.Y - pointRadius) && (Y < pointAhead.Y + pointRadius)))
                return DrawingSetting.DrawingMode.VerDown;

            if ((X > pointTail.X - pointRadius) && (X < pointTail.X + pointRadius) && (Y > pointTail.Y - pointRadius) && (Y < pointTail.Y + pointRadius))
                return DrawingSetting.DrawingMode.VerUp;

            int a = pointTail.Y - pointAhead.Y;
            int b = pointAhead.X - pointTail.X;
            //Tìm vector pháp tuyến của đường thẳng
            int c = -a * pointAhead.X - b * pointAhead.Y;

            double d = Math.Abs(a * X + b * Y + c) / Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2));

            if (d < pointRadius)
                return DrawingSetting.DrawingMode.Move;

            return DrawingSetting.DrawingMode.Normal;
        }

        public override void setDoneStatus()
        {
            doneStatus = true;
        }

        public override System.Windows.Forms.Cursor checkCursor(System.Drawing.Point _curPoint)
        {
            switch (checkDrawingMode(_curPoint))
            {
                case DrawingSetting.DrawingMode.Move:
                    return Cursors.SizeAll;
                case DrawingSetting.DrawingMode.VerDown:
                case DrawingSetting.DrawingMode.VerUp:
                    return Cursors.SizeNS;
                default:
                    return Cursors.Cross;
            }
        }

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
                    gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    Pen pen = genratePen(this.drawingProperties);
                    gr.DrawLine(pen, pointAhead, pointTail);
                }
            }

            if (!doneStatus)
            {
                using (Graphics gr = Graphics.FromImage(bmp))
                {
                    drawEditPoint(pointAhead.X, pointAhead.Y, gr);
                    drawEditPoint(pointTail.X, pointTail.Y, gr);
                }
            }

            return bmp;
        }

        public override bool DoneStatus
        {
            get { return doneStatus; }
        }

        private void drawEditPoint(int x, int y, Graphics gr)
        {
            int left = x - HANDLE_POINT_RADIUS;
            int upper = y - HANDLE_POINT_RADIUS;
            gr.FillRectangle(Brushes.White, left, upper, 2 * HANDLE_POINT_RADIUS, 2 * HANDLE_POINT_RADIUS);
            gr.DrawRectangle(Pens.Black, left, upper, 2 * HANDLE_POINT_RADIUS, 2 * HANDLE_POINT_RADIUS);

        }

    }
}
