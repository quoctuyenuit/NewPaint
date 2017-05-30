using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPaint.Tools
{
    class DrawingProperties
    {
        private Color activeColor;
        private Brush activeBrush;
        public Brush ActiveBrush
        {
            get { return activeBrush; }
            set { activeBrush = value; }
        }
        private int penWidth;

        public Color ActiveColor
        {
            get { return activeColor; }
            set { activeColor = value; }
        }

        public int PenWidth
        {
            get { return penWidth; }
            set { penWidth = value; }
        }
    }
}
