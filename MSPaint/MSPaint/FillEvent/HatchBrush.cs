using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MSPaint.FillEvent
{
    public partial class HatchBrush : Form
    {
        private Color _foreColor;

        public Color _ForeColor
        {
            get { return _foreColor; }
            set { _foreColor = value; }
        }

        private Color _backColor;

        public Color _BackColor
        {
            get { return _backColor; }
            set { _backColor = value; }
        }

        private HatchStyle _hatchStyle;

        public HatchStyle HatchStyle
        {
            get { return _hatchStyle; }
            set { _hatchStyle = value; }
        }

        public HatchBrush()
        {
            InitializeComponent();

            cbBackColor.Color = Tools.PaintTools.ColorHatchBrush1;
            cbForeColor.Color = Tools.PaintTools.ColorHatchBrush2;

            List<HatchStyle> _list = new List<HatchStyle>();

            #region AddStyle
            _list.Add(HatchStyle.BackwardDiagonal);
            _list.Add(HatchStyle.Cross);
            _list.Add(HatchStyle.DarkDownwardDiagonal);
            _list.Add(HatchStyle.DarkHorizontal);
            _list.Add(HatchStyle.DarkUpwardDiagonal);
            _list.Add(HatchStyle.DarkVertical);
            _list.Add(HatchStyle.DashedDownwardDiagonal);
            _list.Add(HatchStyle.DashedHorizontal);
            _list.Add(HatchStyle.DashedUpwardDiagonal);
            _list.Add(HatchStyle.DashedVertical);
            _list.Add(HatchStyle.DiagonalBrick);
            _list.Add(HatchStyle.DiagonalCross);
            _list.Add(HatchStyle.Divot);
            _list.Add(HatchStyle.DottedDiamond);
            _list.Add(HatchStyle.DottedGrid);
            _list.Add(HatchStyle.ForwardDiagonal);
            _list.Add(HatchStyle.Horizontal);
            _list.Add(HatchStyle.HorizontalBrick);
            _list.Add(HatchStyle.LargeCheckerBoard);
            _list.Add(HatchStyle.LargeConfetti);
            _list.Add(HatchStyle.LargeGrid);
            _list.Add(HatchStyle.LightDownwardDiagonal);
            _list.Add(HatchStyle.LightHorizontal);
            _list.Add(HatchStyle.LightUpwardDiagonal);
            _list.Add(HatchStyle.LightVertical);
            _list.Add(HatchStyle.Max);
            _list.Add(HatchStyle.Min);
            _list.Add(HatchStyle.NarrowHorizontal);
            _list.Add(HatchStyle.NarrowVertical);
            _list.Add(HatchStyle.OutlinedDiamond);
            _list.Add(HatchStyle.Percent05);
            _list.Add(HatchStyle.Percent10);
            _list.Add(HatchStyle.Percent20);
            _list.Add(HatchStyle.Percent25);
            _list.Add(HatchStyle.Percent30);
            _list.Add(HatchStyle.Percent40);
            _list.Add(HatchStyle.Percent50);
            _list.Add(HatchStyle.Percent60);
            _list.Add(HatchStyle.Percent70);
            _list.Add(HatchStyle.Percent75);
            _list.Add(HatchStyle.Percent80);
            _list.Add(HatchStyle.Percent90);
            _list.Add(HatchStyle.Plaid);
            _list.Add(HatchStyle.Shingle);
            _list.Add(HatchStyle.SmallCheckerBoard);
            _list.Add(HatchStyle.SmallConfetti);
            _list.Add(HatchStyle.SmallGrid);
            _list.Add(HatchStyle.SolidDiamond);
            _list.Add(HatchStyle.Sphere);
            _list.Add(HatchStyle.Trellis);
            _list.Add(HatchStyle.Vertical);
            _list.Add(HatchStyle.Wave);
            _list.Add(HatchStyle.Weave);
            _list.Add(HatchStyle.WideDownwardDiagonal);
            _list.Add(HatchStyle.WideUpwardDiagonal);
            _list.Add(HatchStyle.ZigZag);
            #endregion

            cbStyle.Properties.Items.AddRange(_list);

            cbStyle.SelectedItem = Tools.PaintTools.HatchStyleBrush;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this._ForeColor = cbForeColor.Color;
            this._BackColor = cbBackColor.Color;
            this._hatchStyle = (HatchStyle)cbStyle.SelectedItem;
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

    }
}
