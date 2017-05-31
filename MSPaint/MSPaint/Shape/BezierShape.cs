using MSPaint.Tools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSPaint.Shape
{
    class BezierShape : DrawingFrame
    {
        private Point p1;
        private Point p2;
        private Point p3;
        private Point p4;

        //Tỷ lệ xác định vị trí các điểm trong khung hình, dùng để xác định vị trí 4 điểm sau khi thay đổi kích cỡ hình
        struct TyLe
        {
            public float X { get; set; }
            public float Y { get; set; }
        }

        private TyLe tlp1;
        private TyLe tlp2;
        private TyLe tlp3;
        private TyLe tlp4;

        private int countClick;


        public BezierShape(Size surfaceSize, Point p)
            : base(surfaceSize, p)
        {
            p1 = p2 = p3 = p4 = p;
            countClick = 0;
        }

        public override void updateShape(Point _curPoint, DrawingProperties _properties, DrawingSetting.MoseStatus _mouseStatus)
        {
            this.drawingProperties = _properties;

            if (_mouseStatus == DrawingSetting.MoseStatus.Down)
            {
                #region MouseDown
                this.countClick++;

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

                            this.pivotMove = _curPoint;

                            if (countClick <= 3)
                            {
                                this.drawingStatus = DrawingSetting.DrawingStatus.Draw;
                                this.drawingMode = DrawingSetting.DrawingMode.DownRight;

                                switch (countClick)
                                {
                                    case 2:
                                        {
                                            p2 = _curPoint;
                                            p3 = _curPoint;
                                            break;
                                        }
                                    case 3:
                                        {
                                            p3 = _curPoint;
                                            break;
                                        }
                                }
                                break;
                            }
                            else
                                this.drawingMode = checkDrawingMode(_curPoint);


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
                #endregion
            }

            if (_mouseStatus == DrawingSetting.MoseStatus.Move)
            {
                #region MouseMove
                if (drawingStatus == DrawingSetting.DrawingStatus.Draw || drawingStatus == DrawingSetting.DrawingStatus.Adjust)
                {
                    if (countClick <= 3)
                    {
                        this.leftBound = Math.Min(p1.X, Math.Min(p2.X, Math.Min(p3.X, p4.X)));
                        this.rightBound = Math.Max(p1.X, Math.Max(p2.X, Math.Max(p3.X, p4.X)));

                        this.upperBound = Math.Min(p1.Y, Math.Min(p2.Y, Math.Min(p3.Y, p4.Y)));
                        this.lowerBound = Math.Max(p1.Y, Math.Max(p2.Y, Math.Max(p3.Y, p4.Y)));
                    }

                    switch (countClick)
                    {
                        case 1: p4 = _curPoint; break;
                        case 2: p2 = p3 = _curPoint; break;
                        case 3: p3 = _curPoint; break;
                        default:
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

                                            p1.X += _curPoint.X - pivotMove.X;
                                            p1.Y += _curPoint.Y - pivotMove.Y;
                                            p2.X += _curPoint.X - pivotMove.X;
                                            p2.Y += _curPoint.Y - pivotMove.Y;
                                            p3.X += _curPoint.X - pivotMove.X;
                                            p3.Y += _curPoint.Y - pivotMove.Y;
                                            p4.X += _curPoint.X - pivotMove.X;
                                            p4.Y += _curPoint.Y - pivotMove.Y;

                                            this.pivotMove.X = _curPoint.X;
                                            this.pivotMove.Y = _curPoint.Y;
                                            break;
                                        }
                                }
                                #endregion

                                if (drawingMode != DrawingSetting.DrawingMode.Move && drawingMode != DrawingSetting.DrawingMode.Normal)
                                {
                                    p1.X = (int)(leftBound + tlp1.X * (rightBound - leftBound));
                                    p1.Y = (int)(upperBound + tlp1.Y * (lowerBound - upperBound));

                                    p2.X = (int)(leftBound + tlp2.X * (rightBound - leftBound));
                                    p2.Y = (int)(upperBound + tlp2.Y * (lowerBound - upperBound));

                                    p3.X = (int)(leftBound + tlp3.X * (rightBound - leftBound));
                                    p3.Y = (int)(upperBound + tlp3.Y * (lowerBound - upperBound));

                                    p4.X = (int)(leftBound + tlp4.X * (rightBound - leftBound));
                                    p4.Y = (int)(upperBound + tlp4.Y * (lowerBound - upperBound));
                                }
                                break;
                            }
                    }
                }
                #endregion
            }

            if (_mouseStatus == DrawingSetting.MoseStatus.Up)
            {
                //Sau khi vẽ điểm cuối cùng trong hình thì lưu lại tỷ lệ của các điểm
                if (this.countClick == 3)
                {
                    tlp1.X = xacDinhTyLe(p1, true);
                    tlp1.Y = xacDinhTyLe(p1, false);

                    tlp2.X = xacDinhTyLe(p2, true);
                    tlp2.Y = xacDinhTyLe(p2, false);

                    tlp3.X = xacDinhTyLe(p3, true);
                    tlp3.Y = xacDinhTyLe(p3, false);

                    tlp4.X = xacDinhTyLe(p4, true);
                    tlp4.Y = xacDinhTyLe(p4, false);
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

            //base.updateShape(_curPoint, _properties, _mouseStatus);
        }

        //loaiTyLe = true => tỷ lệ X ngược lại là tỷ lệ Y
        private float xacDinhTyLe(Point p, bool loaiTyLe)
        {
            return (loaiTyLe) ? ((float)(p.X - leftBound) / (rightBound - leftBound)) : ((float)(p.Y - upperBound) / (lowerBound - upperBound));
        }

        protected override System.Drawing.Bitmap generateImage()
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
                    Pen pen = genratePen(this.drawingProperties);
                    pen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
                    pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                    gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    gr.DrawBezier(pen, p1, p2, p3, p4);
                }
            }

            if (this.countClick > 3 || (this.countClick == 3 && drawingStatus != DrawingSetting.DrawingStatus.Draw))
                drawFrame(bmp);

            return bmp;
        }
        public override System.Windows.Forms.Cursor checkCursor(Point _curPoint)
        {
            if (this.countClick <= 2 || (this.countClick == 3 && drawingStatus == DrawingSetting.DrawingStatus.Draw))
                return System.Windows.Forms.Cursors.Cross;
            return base.checkCursor(_curPoint);
        }
    }
}
