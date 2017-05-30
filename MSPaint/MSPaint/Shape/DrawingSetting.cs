using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPaint.Shape
{
    class DrawingSetting
    {
        static public System.Drawing.Point RotatePoint(System.Drawing.Point pointToRotate, System.Drawing.Point centerPoint, double angleInDegrees)
        {
            double angleInRadians = angleInDegrees * (Math.PI / 180);
            double cosTheta = Math.Cos(angleInRadians);
            double sinTheta = Math.Sin(angleInRadians);
            return new System.Drawing.Point
            {
                X =
                    (int)
                    (cosTheta * (pointToRotate.X - centerPoint.X) -
                    sinTheta * (pointToRotate.Y - centerPoint.Y) + centerPoint.X),
                Y =
                    (int)
                    (sinTheta * (pointToRotate.X - centerPoint.X) +
                    cosTheta * (pointToRotate.Y - centerPoint.Y) + centerPoint.Y)
            };
        }

        public enum DrawingMode { HonLeft, HonRight, VerUp, VerDown, UpLeft, UpRight, DownLeft, DownRight, Move, Drawing, Normal };
        public enum DrawingStatus { PreDraw, Draw, Adjust, Free };
        public enum MoseStatus { Move, Up, Down };
    }
}
