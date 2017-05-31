using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSPaint.Tools
{
    class PaintTools
    {
        public enum EnumDrawingTool { FreePen, Rectangle, Ellipse, Line, Image, Triangle, SquareTriangle, Diamond, Pentagon, DownArrow, UpArrow, RightArrow, LeftArrow, Eraser, Select, Polygon, Bezier, Free};
        public static EnumDrawingTool DrawingTool;
        public static Color DrawingColor;
        public static Brush DrawingBrush;
        public static Color ColorHatchBrush1;//Lưu lại màu sắc hiện tại của 2 màu khi dùng chế độ HatchBrush để đổ màu
        public static Color ColorHatchBrush2;
        public static System.Drawing.Drawing2D.HatchStyle HatchStyleBrush;
        public enum EnumBrushStatus { Fill, UnFill };
        public static EnumBrushStatus BrushStatus;
        public static int PenWidth;
        public static int EraserWidth;
    }
}
