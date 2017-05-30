using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyPaint.Tools
{
    class MainPaneMouseEventArgs : MouseEventArgs
    {
        private Bitmap content;

        public MainPaneMouseEventArgs(MouseEventArgs e, Bitmap _content)
            : base(e.Button, e.Clicks, e.X, e.Y, e.Delta)
        {
            this.content = _content;
        }
    }
}
