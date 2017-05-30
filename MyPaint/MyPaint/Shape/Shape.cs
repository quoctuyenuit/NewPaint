using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyPaint.Shape
{
    abstract class Shape
    {
        public const int pointRadius = 5;
        protected Size surfaceSize;
        protected DrawingSetting.DrawingMode drawingMode;
        protected DrawingSetting.DrawingStatus drawingStatus;
        protected Tools.DrawingProperties drawingProperties;

        public Bitmap CurrentImage//Bitmap chứa nội dung hình ảnh đang vẽ, áp dụng cho copy hình ở chế độ select
        {
            get;
            set;
        }

        public Shape(Size surfaceSize)
        {
            this.surfaceSize = surfaceSize;
        }
        public abstract void updateShape(Point _curPoint, Tools.DrawingProperties _properties, DrawingSetting.MoseStatus _mouseStatus);
        public abstract void setDoneStatus();
        public abstract Cursor checkCursor(Point _curPoint);
        public abstract void RotateShape(double angleInDegrees);
        
        protected Pen genratePen(Tools.DrawingProperties properties)
        {
            Pen customPen = new Pen(properties.ActiveColor, properties.PenWidth);
            return customPen;
        }

        public abstract Bitmap CurrentShape//Bitmap chứa cả khung hình hiện tại đang vẽ
        {
            get;
        }
        public abstract bool DoneStatus
        {
            get;
        }
    }
}
