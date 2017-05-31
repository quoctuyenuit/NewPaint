using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSPaint.Shape
{
    class SelectShape : DrawingFrame
    {
        public SelectShape(Size surfaceSize, Point p, Drawing.MainPanel _drawingSpace)
            : base(surfaceSize, p)
        {
            this.drawingSpace = _drawingSpace;
        }

        private Drawing.MainPanel drawingSpace;

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
            else if (drawingStatus == DrawingSetting.DrawingStatus.Draw)
            {

            }
            else
            {
                using (Graphics gr = Graphics.FromImage(bmp))
                {
                    gr.DrawImage(CurrentImage, leftBound, upperBound, rightBound - leftBound, lowerBound - upperBound);
                }
            }

            drawFrame(bmp);

            return bmp;
        }

        public override void updateShape(Point _curPoint, Tools.DrawingProperties _properties, DrawingSetting.MoseStatus _mouseStatus)
        {
            try
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
                                int left = (leftBound >= 0) ? leftBound : 0;
                                int upper = (upperBound >= 0) ? upperBound : 0;
                                int width = (rightBound < this.drawingSpace.Size.Width) ? rightBound - left : this.drawingSpace.Size.Width - left;
                                int height = (lowerBound < this.drawingSpace.Size.Height) ? lowerBound - upper : this.drawingSpace.Size.Height - upper;

                                leftBound = left;
                                upperBound = upper;

                                lowerBound = height + upper;
                                rightBound = left + width;

                                Bitmap bmp = this.drawingSpace.ContentPanel.Content.Clone(new Rectangle(left, upper, width, height), this.drawingSpace.ContentPanel.Content.PixelFormat);

                                Bitmap temp = this.drawingSpace.Image.Clone(new Rectangle(0, 0, this.drawingSpace.Image.Width, this.drawingSpace.Image.Height), this.drawingSpace.Image.PixelFormat);

                                this.drawingSpace.ListBack.Push(temp);

                                using (Graphics gr = Graphics.FromImage(this.drawingSpace.ContentPanel.Content))
                                {
                                    gr.FillRectangle(Brushes.White, leftBound, upperBound, bmp.Size.Width, bmp.Size.Height);
                                }

                                this.CurrentImage = bmp;
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
            catch (Exception ex) { }
        }
    }
}
