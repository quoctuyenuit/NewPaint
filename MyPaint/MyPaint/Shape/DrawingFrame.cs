using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyPaint.Shape
{
    abstract class DrawingFrame:Shape
    {
        private const int HANDLE_POINT_RADIUS = 2;
        protected int leftBound;
        protected int rightBound;
        protected int upperBound;
        protected int lowerBound;
        protected bool doneStatus;
        //Mốc bên trái cùng khi chỉnh sửa
        protected int pivotLeftBound;
        //Mốc bên phải cùng khi chỉnh sửa
        protected int pivotRightBound;
        //Mốc bên trên cùng khi chỉnh sửa
        protected int pivotUpperBound;
        //Mốc bên dưới cùng khi chỉnh sửa
        protected int pivotLowerBound;
        //Điểm mốc vị trí chuột khi chỉnh sửa hình
        protected Point pivotMove;

        public DrawingFrame(Size size, Point p)
            : base(size)
        {
            this.leftBound = this.rightBound = p.X;
            this.upperBound = this.lowerBound = p.Y;
            this.doneStatus = false;
            this.drawingStatus = DrawingSetting.DrawingStatus.PreDraw;
        }

        #region Override
        public override void updateShape(Point _curPoint, Tools.DrawingProperties _properties, DrawingSetting.MoseStatus _mouseStatus)
        {
            this.drawingProperties = _properties;
            if (_mouseStatus == DrawingSetting.MoseStatus.Down)
            {
                #region MouseStatus = Down

                switch (drawingStatus)
                {
                    case DrawingSetting.DrawingStatus.PreDraw:
                        {
                            this.drawingStatus = DrawingSetting.DrawingStatus.Draw;
                            this.drawingMode = DrawingSetting.DrawingMode.DownRight;
                            this.pivotLeftBound = leftBound;
                            this.pivotRightBound = rightBound;
                            this.pivotUpperBound = upperBound;
                            this.pivotLowerBound = lowerBound;
                            break;
                        }
                    case DrawingSetting.DrawingStatus.Free:
                        {
                            this.drawingStatus = DrawingSetting.DrawingStatus.Adjust;
                            this.drawingMode = checkDrawingMode(_curPoint);
                            this.pivotLeftBound = leftBound;
                            this.pivotRightBound = rightBound;
                            this.pivotUpperBound = upperBound;
                            this.pivotLowerBound = lowerBound;
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
                #endregion
            }
            if (_mouseStatus == DrawingSetting.MoseStatus.Move)
            {
                #region Switch DrawingMode
                if (drawingStatus == DrawingSetting.DrawingStatus.Draw || drawingStatus == DrawingSetting.DrawingStatus.Adjust)
                {
                    switch (drawingMode)
                    {
                        case DrawingSetting.DrawingMode.Drawing:
                        case DrawingSetting.DrawingMode.HonLeft:
                            {

                                leftBound = Math.Min(_curPoint.X, pivotRightBound);

                                break;
                            }
                        case DrawingSetting.DrawingMode.HonRight:
                            {
                                rightBound = Math.Max(_curPoint.X, pivotLeftBound);
                                break;
                            }
                        case DrawingSetting.DrawingMode.VerUp:
                            {
                                upperBound = Math.Min(_curPoint.Y, pivotLowerBound);
                                break;
                            }
                        case DrawingSetting.DrawingMode.VerDown:
                            {
                                lowerBound = Math.Max(_curPoint.Y, pivotUpperBound);
                                break;
                            }
                        case DrawingSetting.DrawingMode.UpLeft:
                            {
                                upperBound = Math.Min(_curPoint.Y, pivotLowerBound);
                                lowerBound = Math.Max(_curPoint.Y, pivotLowerBound);
                                leftBound = Math.Min(_curPoint.X, pivotRightBound);
                                rightBound = Math.Max(_curPoint.X, pivotRightBound);
                                break;
                            }
                        case DrawingSetting.DrawingMode.DownRight:
                            {
                                upperBound = Math.Min(_curPoint.Y, pivotUpperBound);
                                lowerBound = Math.Max(_curPoint.Y, pivotUpperBound);
                                leftBound = Math.Min(_curPoint.X, pivotLeftBound);
                                rightBound = Math.Max(_curPoint.X, pivotLeftBound);
                                break;
                            }
                        case DrawingSetting.DrawingMode.UpRight:
                            {
                                upperBound = Math.Min(_curPoint.Y, pivotLowerBound);
                                lowerBound = Math.Max(_curPoint.Y, pivotLowerBound);
                                leftBound = Math.Min(_curPoint.X, pivotLeftBound);
                                rightBound = Math.Max(_curPoint.X, pivotLeftBound);
                                break;
                            }
                        case DrawingSetting.DrawingMode.DownLeft:
                            {
                                upperBound = Math.Min(_curPoint.Y, pivotUpperBound);
                                lowerBound = Math.Max(_curPoint.Y, pivotUpperBound);
                                leftBound = Math.Min(_curPoint.X, pivotRightBound);
                                rightBound = Math.Max(_curPoint.X, pivotRightBound);
                                break;
                            }
                        case DrawingSetting.DrawingMode.Move:
                            {
                                this.leftBound += _curPoint.X - pivotMove.X;
                                this.rightBound += _curPoint.X - pivotMove.X;
                                this.upperBound += _curPoint.Y - pivotMove.Y;
                                this.lowerBound += _curPoint.Y - pivotMove.Y;
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

        public override System.Windows.Forms.Cursor checkCursor(Point _curPoint)
        {
            switch (checkDrawingMode(_curPoint))
            {
                case DrawingSetting.DrawingMode.DownLeft:
                case DrawingSetting.DrawingMode.UpRight:
                    return Cursors.SizeNESW;
                case DrawingSetting.DrawingMode.DownRight:
                case DrawingSetting.DrawingMode.UpLeft:
                    return Cursors.SizeNWSE;
                case DrawingSetting.DrawingMode.HonRight:
                case DrawingSetting.DrawingMode.HonLeft:
                    return Cursors.SizeWE;
                case DrawingSetting.DrawingMode.VerUp:
                case DrawingSetting.DrawingMode.VerDown:
                    return Cursors.SizeNS;
                case DrawingSetting.DrawingMode.Move:
                    return Cursors.SizeAll;
                default: return Cursors.Cross;
            }
        }

        public override void setDoneStatus()
        {
            doneStatus = true;
        }

        public override Bitmap CurrentShape
        {
            get { return generateImage(); }
        }

        public override bool DoneStatus
        {
            get { return doneStatus; }
        }
        #endregion

        protected DrawingSetting.DrawingMode checkDrawingMode(Point _curPoint)
        {
            int X = _curPoint.X;
            int Y = _curPoint.Y;
            int Xmid = (leftBound + rightBound) / 2;
            int Ymid = (upperBound + lowerBound) / 2;
            if ((X > leftBound - pointRadius) && (X < leftBound + pointRadius) && (Y > upperBound - pointRadius) && (Y < upperBound + pointRadius))
                return DrawingSetting.DrawingMode.UpLeft;

            if ((X > leftBound - pointRadius) && (X < leftBound + pointRadius) && (Y > lowerBound - pointRadius) && (Y < lowerBound + pointRadius))
                return DrawingSetting.DrawingMode.DownLeft;

            if ((X > rightBound - pointRadius) && (X < rightBound + pointRadius) && (Y > lowerBound - pointRadius) && (Y < lowerBound + pointRadius))
                return DrawingSetting.DrawingMode.DownRight;

            if ((X > rightBound - pointRadius) && (X < rightBound + pointRadius) && (Y > upperBound - pointRadius) && (Y < upperBound + pointRadius))
                return DrawingSetting.DrawingMode.UpRight;

            if ((X > Xmid - pointRadius) && (X < Xmid + pointRadius) && (Y > upperBound - pointRadius) && (Y < upperBound + pointRadius))
                return DrawingSetting.DrawingMode.VerUp;

            if ((X > Xmid - pointRadius) && (X < Xmid + pointRadius) && (Y > lowerBound - pointRadius) && (Y < lowerBound + pointRadius))
                return DrawingSetting.DrawingMode.VerDown;

            if ((X > rightBound - pointRadius) && (X < rightBound + pointRadius) && (Y > Ymid - pointRadius) && (Y < Ymid + pointRadius))
                return DrawingSetting.DrawingMode.HonRight;

            if ((X > leftBound - pointRadius) && (X < leftBound + pointRadius) && (Y > Ymid - pointRadius) && (Y < Ymid + pointRadius))
                return DrawingSetting.DrawingMode.HonLeft;

            if (X >= leftBound && X <= rightBound && Y >= upperBound && Y <= lowerBound)
                return DrawingSetting.DrawingMode.Move;

            return DrawingSetting.DrawingMode.Normal;
        }

        protected abstract Bitmap generateImage();
       
        protected void drawFrame(Bitmap bmp)
        {
            if (!doneStatus)
            {
                using (Graphics gr = Graphics.FromImage(bmp))
                {
                    Pen pen = new Pen(Color.LightSkyBlue);
                    pen.Width = 0.1f;
                    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                    gr.DrawRectangle(pen, new System.Drawing.Rectangle(leftBound, upperBound, rightBound - leftBound, lowerBound - upperBound));

                    int[] xArr = new int[3];
                    int[] yArr = new int[3];

                    xArr[0] = leftBound;
                    xArr[1] = (leftBound + rightBound) / 2;
                    xArr[2] = rightBound;

                    yArr[0] = upperBound;
                    yArr[1] = (upperBound + lowerBound) / 2;
                    yArr[2] = lowerBound;

                    for (int i = 0; i < xArr.Length; i++)
                        for (int j = 0; j < yArr.Length; j++)
                        {
                            if (i == 1 && j == 1)
                                continue;
                            drawEditPoint(xArr[i], yArr[j], gr);
                        }
                }
            }
        }

        //Hàm vẽ các điểm xung quay để chỉnh sửa
        protected void drawEditPoint(int x, int y, Graphics gr)
        {
            int left = x - HANDLE_POINT_RADIUS;
            int upper = y - HANDLE_POINT_RADIUS;
            gr.FillRectangle(Brushes.White, left, upper, 2 * HANDLE_POINT_RADIUS, 2 * HANDLE_POINT_RADIUS);
            gr.DrawRectangle(Pens.Black, left, upper, 2 * HANDLE_POINT_RADIUS, 2 * HANDLE_POINT_RADIUS);
        }

        public override void RotateShape(double angleInDegrees)
        {
            Point p1 = new Point(leftBound, upperBound);
            Point p2 = new Point(rightBound, upperBound);
            Point p3 = new Point(rightBound, lowerBound);
            Point p4 = new Point(leftBound, lowerBound);
            Point centerPoint = new Point((rightBound - leftBound) / 2 + leftBound, (lowerBound - upperBound) / 2 + upperBound);
            
            p1 = DrawingSetting.RotatePoint(p1, centerPoint, angleInDegrees);
            p2 = DrawingSetting.RotatePoint(p2, centerPoint, angleInDegrees);
            p3 = DrawingSetting.RotatePoint(p3, centerPoint, angleInDegrees);
            p4 = DrawingSetting.RotatePoint(p4, centerPoint, angleInDegrees);

            leftBound = p4.X;
            rightBound = p1.X;
            upperBound = p1.Y;
            lowerBound = p2.Y;
        }
    }
}
