using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPaint.Shape
{
    class DrawingSetting
    {
        public enum DrawingMode { HonLeft, HonRight, VerUp, VerDown, UpLeft, UpRight, DownLeft, DownRight, Move, Drawing, Normal };
        public enum DrawingStatus { PreDraw, Draw, Adjust, Free };
        public enum MoseStatus { Move, Up, Down };
    }
}
