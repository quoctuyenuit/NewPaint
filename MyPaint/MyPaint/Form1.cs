using MyPaint.Properties;
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

namespace MyPaint
{
    public partial class Form1 : Form
    {
        Drawing.MainPanel DrawingSpace;

        private Stack<Bitmap> listForward;

        private Bitmap copyBitmap;

        public Bitmap CopyBitmap
        {
            get { return copyBitmap; }
            set { copyBitmap = value; }
        }

        void init()
        {
            this.DrawingSpace = new Drawing.MainPanel(this.Size);
            this.DrawingSpace.BackColor = System.Drawing.Color.White;
            this.DrawingSpace.Cursor = System.Windows.Forms.Cursors.Cross;
            this.DrawingSpace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DrawingSpace.Location = new System.Drawing.Point(0, 150);
            this.DrawingSpace.Name = "mainPanel1";
            this.DrawingSpace.TabIndex = 1;
            this.DrawingSpace.ContextMenuStrip = contextMenuStrip;
            this.FreeSpace.Controls.Add(this.DrawingSpace);
            this.listForward = new Stack<Bitmap>();
        }
        public Form1()
        {
            InitializeComponent();

            WindowState = FormWindowState.Maximized;
            Tools.PaintTools.DrawingTool = Tools.PaintTools.EnumDrawingTool.FreePen;
            Tools.PaintTools.DrawingColor = Color.Black;
            Tools.PaintTools.ColorHatchBrush1 = Color.White;
            Tools.PaintTools.ColorHatchBrush2 = Color.White;
            Tools.PaintTools.HatchStyleBrush = HatchStyle.BackwardDiagonal;
            Tools.PaintTools.PenWidth = 1;
            Tools.PaintTools.DrawingBrush = Brushes.Yellow;
            Tools.PaintTools.BrushStatus = Tools.PaintTools.EnumBrushStatus.UnFill;
            this.selectColor.EditValue = Color.Black;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            init();
        }

        #region File

        private void btnNew_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (this.DrawingSpace.ListBack.Count == 0)
                return;

            DialogResult result = MessageBox.Show("Do you want to save change to Untitled?", "Paint", MessageBoxButtons.YesNoCancel);
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                btnSave_ItemClick(null, null);

                this.DrawingSpace.embed();

                Bitmap bmp = this.DrawingSpace.ListBack.Last();

                this.DrawingSpace.ContentPanel.Content = bmp;
                this.DrawingSpace.DrawingPanel.Content = new Bitmap(bmp.Size.Width, bmp.Size.Height);
                this.DrawingSpace.Refresh();
                this.DrawingSpace.ListBack.Clear();
            }
            else if (result == System.Windows.Forms.DialogResult.No)
            {
                this.DrawingSpace.embed();

                Bitmap bmp = this.DrawingSpace.ListBack.Last();

                this.DrawingSpace.ContentPanel.Content = bmp;
                this.DrawingSpace.DrawingPanel.Content = new Bitmap(bmp.Size.Width, bmp.Size.Height);
                this.DrawingSpace.Refresh();
                this.DrawingSpace.ListBack.Clear();
            }
            else
                return;
        }

        private void btnOpen_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = "All Image Files|*.bmp;*.ico;*.gif;*.jpeg;*.jpg;" +
                    "*.jfif;*.png;*.tif;*.tiff;*.wmf;*.emf|" +
                    "Windows Bitmap (*.bmp)|*.bmp|" +
                    "All Files (*.*)|*.*";
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    Bitmap bmp = new Bitmap(dlg.FileName);

                    this.DrawingSpace.DrawingPanel.ActiveShape = new Shape.ImageShape(this.DrawingSpace.Size, new Point(0, 0), bmp);

                    this.DrawingSpace.DrawingPanel.updateContent();

                    this.DrawingSpace.Refresh();
                }
            }
            
        }

        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.DrawingSpace.embed();
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "JPEG(.jpg)|*.jpg|PNG(.png)|*.png|Bitmap(.bmp)|*.bmp";
            dlg.FileName = "Untitled";
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Bitmap bmp = new Bitmap(this.DrawingSpace.Image);
                string ext = dlg.FileName.Substring(dlg.FileName.LastIndexOf('.') + 1);
                switch (ext)
                {
                    case "png": bmp.Save(dlg.FileName, System.Drawing.Imaging.ImageFormat.Png);
                        break;
                    case "jpg": bmp.Save(dlg.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;
                    case "bmp": bmp.Save(dlg.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
                        break;
                }
            }
        }

        #endregion

        #region Setting Color and Pen width

        private void size1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Tools.PaintTools.PenWidth = 1;
            this.penWitdhStatus.Text = "1 px";
        }

        private void size2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Tools.PaintTools.PenWidth = 3;
            this.penWitdhStatus.Text = "3 px";
        }

        private void size3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Tools.PaintTools.PenWidth = 5;
            this.penWitdhStatus.Text = "5 px";
        }

        private void size4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Tools.PaintTools.PenWidth = 7;
            this.penWitdhStatus.Text = "7 px";
        }

        #endregion

        #region ContextMenuStrip Event

        private void contextMenu_Opening(object sender, CancelEventArgs e)
        {
            foreach (ToolStripItem item in contextMenuStrip.Items)
                item.Enabled = true;

            if (this.DrawingSpace.Cursor != Cursors.SizeAll)
            {
                foreach (ToolStripItem item in contextMenuStrip.Items)
                    item.Enabled = (item.Name != menuItemSaveFile.Name && item.Name != menuItemOpenFile.Name) ? false : true;

                if (!btnSelect.Checked)
                    menuItemCopy.Enabled = false;
            }
           

            if (copyBitmap != null)
                menuItemPaste.Enabled = true;
            else
                menuItemPaste.Enabled = false;
        }

        private void menuItemOpenFile_Click(object sender, EventArgs e)
        {
            btnOpen_ItemClick(null, null);
        }

        private void menuItemSaveFile_Click(object sender, EventArgs e)
        {
            btnSave_ItemClick(null, null);
        }

        private void menuSubItemUnFill_Click(object sender, EventArgs e)
        {
            Tools.PaintTools.BrushStatus = Tools.PaintTools.EnumBrushStatus.UnFill;
        }

        private void menuSubItemSolidBrush_Click(object sender, EventArgs e)
        {
            using (ColorDialog dlg = new ColorDialog())
            {
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    SolidBrush _brush = new SolidBrush(dlg.Color);
                    Tools.PaintTools.DrawingBrush = _brush;
                    Tools.PaintTools.BrushStatus = Tools.PaintTools.EnumBrushStatus.Fill;
                }
            }
        }

        private void menuSubItemLinearGradientBrush_Click(object sender, EventArgs e)
        {
            using (FillEvent.LinearGradientBrush dlg = new FillEvent.LinearGradientBrush())
            {
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    LinearGradientBrush _brush = new LinearGradientBrush(new Point(0, 10), new Point(200, 10), dlg._ForeColor, dlg._BackColor);
                    Tools.PaintTools.ColorHatchBrush1 = dlg._BackColor;
                    Tools.PaintTools.ColorHatchBrush2 = dlg._ForeColor;
                    Tools.PaintTools.DrawingBrush = _brush;
                    Tools.PaintTools.BrushStatus = Tools.PaintTools.EnumBrushStatus.Fill;
                }
            }
        }

        private void menuSubItemTextureBrush_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = "All Image Files|*.bmp;*.ico;*.gif;*.jpeg;*.jpg;" +
                    "*.jfif;*.png;*.tif;*.tiff;*.wmf;*.emf|" +
                    "Windows Bitmap (*.bmp)|*.bmp|" +
                    "All Files (*.*)|*.*";
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    Image img = new Bitmap(dlg.FileName);
                    TextureBrush _brush = new TextureBrush(img);
                    Tools.PaintTools.DrawingBrush = _brush;
                    Tools.PaintTools.BrushStatus = Tools.PaintTools.EnumBrushStatus.Fill;
                }
            }
        }

        private void menuSubItemHatchBrush_Click(object sender, EventArgs e)
        {
            using (FillEvent.HatchBrush dlg = new FillEvent.HatchBrush())
            {
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    HatchStyle style = dlg.HatchStyle;
                    HatchBrush _brush = new HatchBrush(style, dlg._ForeColor, dlg._BackColor);
                    Tools.PaintTools.ColorHatchBrush1 = dlg._BackColor;
                    Tools.PaintTools.ColorHatchBrush2 = dlg._ForeColor;
                    Tools.PaintTools.HatchStyleBrush = dlg.HatchStyle;
                    Tools.PaintTools.DrawingBrush = _brush;
                    Tools.PaintTools.BrushStatus = Tools.PaintTools.EnumBrushStatus.Fill;
                }
            }
        }

        private void menuItemCopy_Click(object sender, EventArgs e)
        {
            try
            {
                Bitmap bmp = this.DrawingSpace.DrawingPanel.ActiveShape.CurrentImage;
                if (bmp != null)
                {
                    this.copyBitmap = bmp.Clone(new Rectangle(0, 0, bmp.Size.Width, bmp.Size.Height), bmp.PixelFormat);
                }
            }
            catch (Exception ex) { }
            btnPaste.Enabled = true;
        }

        private void menuItemCut_Click(object sender, EventArgs e)
        {
            try
            {
                Bitmap bmp = this.DrawingSpace.DrawingPanel.ActiveShape.CurrentImage;
                if (bmp != null)
                {
                    this.copyBitmap = bmp.Clone(new Rectangle(0, 0, bmp.Size.Width, bmp.Size.Height), bmp.PixelFormat);
                }
                btnUndo_ItemClick(null, null);
            }
            catch (Exception ex) { }
            btnPaste.Enabled = true;
        }

        private void menuItemPaste_Click(object sender, EventArgs e)
        {
            this.DrawingSpace.embed();
            if (this.copyBitmap != null)
            {
                this.DrawingSpace.DrawingPanel.ActiveShape = new Shape.ImageShape(this.DrawingSpace.Size, new Point(0, 0), this.copyBitmap);

                this.DrawingSpace.DrawingPanel.updateContent();

                this.DrawingSpace.Refresh();
            }

        }

        private void menuItemDelete_Click(object sender, EventArgs e)
        {
            btnUndo_ItemClick(null, null);
        }

        #endregion

        #region Button and Item click
        private void btnUndo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.DrawingSpace.embed();
            if (this.DrawingSpace.ListBack.Count == 0)
                return;
            Bitmap bmp = this.DrawingSpace.ListBack.Pop();
            Bitmap temp = this.DrawingSpace.ContentPanel.Content.Clone(new Rectangle(0,0,this.DrawingSpace.ContentPanel.Content.Size.Width, this.DrawingSpace.ContentPanel.Content.Size.Height), this.DrawingSpace.ContentPanel.Content.PixelFormat);
            this.listForward.Push(temp);
            
            this.DrawingSpace.ContentPanel.Content = bmp;
            this.DrawingSpace.DrawingPanel.Content = new Bitmap(bmp.Size.Width, bmp.Size.Height);
            this.DrawingSpace.Refresh();
            
        }

        private void btnRedo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.DrawingSpace.embed();
            if (this.listForward.Count == 0)
                return;

            Bitmap bmp = this.listForward.Pop();
            Bitmap temp = this.DrawingSpace.ContentPanel.Content.Clone(new Rectangle(0, 0, this.DrawingSpace.ContentPanel.Content.Size.Width, this.DrawingSpace.ContentPanel.Content.Size.Height), this.DrawingSpace.ContentPanel.Content.PixelFormat);
            
            this.DrawingSpace.ListBack.Push(temp);
            this.DrawingSpace.ContentPanel.Content = bmp;
            this.DrawingSpace.DrawingPanel.Content = new Bitmap(bmp.Size.Width, bmp.Size.Height);
            this.DrawingSpace.Refresh();
        }

        private void btnSelect_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.DrawingSpace.embed();
            btnFill.Enabled = false;
            btnCopy.Enabled = true;
            btnCut.Enabled = true;
            Tools.PaintTools.DrawingTool = Tools.PaintTools.EnumDrawingTool.Select;
            this.currentShape.Image = Resources.selectIcon;
            if (!btnSelect.Checked)
                btnSelect.Checked = true;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == (Keys.Control | Keys.Z))
                btnUndo_ItemClick(null, null);
            else if(e.KeyData == (Keys.Control|Keys.Y))
                btnRedo_ItemClick(null, null);
            else if (e.KeyData == (Keys.Control | Keys.O))
                btnOpen_ItemClick(null, null);
            else if (e.KeyData == (Keys.Control | Keys.S))
                btnSave_ItemClick(null, null);
            else if (e.KeyData == (Keys.Control | Keys.C))
                menuItemCopy_Click(null, null);
            else if (e.KeyData == (Keys.Control | Keys.X))
                menuItemCut_Click(null, null);
            else if (e.KeyData == (Keys.Control | Keys.V))
                menuItemPaste_Click(null, null);
            else if (e.KeyData == Keys.Delete)
                menuItemDelete_Click(null, null);
            else if(e.KeyData == (Keys.Control|Keys.N))
                btnNew_ItemClick(null, null);
        }

        private void menu_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == (Keys.Control | Keys.Z))
                btnUndo_ItemClick(null, null);
            else if (e.KeyData == (Keys.Control | Keys.Y))
                btnRedo_ItemClick(null, null);
            else if (e.KeyData == (Keys.Control | Keys.O))
                btnOpen_ItemClick(null, null);
            else if (e.KeyData == (Keys.Control | Keys.S))
                btnSave_ItemClick(null, null);
            else if (e.KeyData == (Keys.Control | Keys.C))
                menuItemCopy_Click(null, null);
            else if (e.KeyData == (Keys.Control | Keys.X))
                menuItemCut_Click(null, null);
            else if (e.KeyData == (Keys.Control | Keys.V))
                menuItemPaste_Click(null, null);
            else if (e.KeyData == Keys.Delete)
                menuItemDelete_Click(null, null);
        }

        private void ribbonGalleryBarItem1_GalleryItemClick(object sender, DevExpress.XtraBars.Ribbon.GalleryItemClickEventArgs e)
        {
            this.DrawingSpace.embed();
            btnSelect.Checked = false;
            switch(e.Item.Caption)
            {
                case "pencil":
                    {
                        this.btnFill.Enabled = false;
                        Tools.PaintTools.DrawingTool = Tools.PaintTools.EnumDrawingTool.FreePen;
                        this.currentShape.Image = Resources.pencil_icon;
                        break;
                    }
                case "line":
                    {
                        this.btnFill.Enabled = false;
                        Tools.PaintTools.DrawingTool = Tools.PaintTools.EnumDrawingTool.Line;
                        this.currentShape.Image = Resources.Line_icon;
                        break;
                    }
                case "rectangle":
                    {
                        this.btnFill.Enabled = true;
                        Tools.PaintTools.DrawingTool = Tools.PaintTools.EnumDrawingTool.Rectangle;
                        this.currentShape.Image = Resources.rectangle_icon;
                        break;
                    }
                case "ellipse":
                    {
                        this.btnFill.Enabled = true;
                        Tools.PaintTools.DrawingTool = Tools.PaintTools.EnumDrawingTool.Ellipse;
                        this.currentShape.Image = Resources.ellipse_icon;
                        break;
                    }
                case "triangle":
                    {
                        this.btnFill.Enabled = true;
                        Tools.PaintTools.DrawingTool = Tools.PaintTools.EnumDrawingTool.Triangle;
                        this.currentShape.Image = Resources.Triangle_icon;
                        break;
                    }
                case "squareTriangle":
                    {
                        this.btnFill.Enabled = true;
                        Tools.PaintTools.DrawingTool = Tools.PaintTools.EnumDrawingTool.SquareTriangle;
                        this.currentShape.Image = Resources.squareTriangleIcon;
                        break;
                    }
                case "diamond":
                    {
                        this.btnFill.Enabled = true;
                        Tools.PaintTools.DrawingTool = Tools.PaintTools.EnumDrawingTool.Diamond;
                        this.currentShape.Image = Resources.diamondIcon;
                        break;
                    }
                case "pentagon":
                    {
                        this.btnFill.Enabled = true;
                        Tools.PaintTools.DrawingTool = Tools.PaintTools.EnumDrawingTool.Pentagon;
                        this.currentShape.Image = Resources.pentagon_icon;
                        break;
                    }
                case "downArrow":
                    {
                        this.btnFill.Enabled = true;
                        Tools.PaintTools.DrawingTool = Tools.PaintTools.EnumDrawingTool.DownArrow;
                        this.currentShape.Image = Resources.down_icon;
                        break;
                    }
                case "upArrow":
                    {
                        this.btnFill.Enabled = true;
                        Tools.PaintTools.DrawingTool = Tools.PaintTools.EnumDrawingTool.UpArrow;
                        this.currentShape.Image = Resources.down_icon;
                        break;
                    }
                case "rightArrow":
                    {
                        this.btnFill.Enabled = true;
                        Tools.PaintTools.DrawingTool = Tools.PaintTools.EnumDrawingTool.RightArrow;
                        this.currentShape.Image = Resources.down_icon;
                        break;
                    }
                case "leftArrow":
                    {
                        this.btnFill.Enabled = true;
                        Tools.PaintTools.DrawingTool = Tools.PaintTools.EnumDrawingTool.LeftArrow;
                        this.currentShape.Image = Resources.down_icon;
                        break;
                    }
                case "polygon":
                    {
                        this.btnFill.Enabled = true;
                        Tools.PaintTools.DrawingTool = Tools.PaintTools.EnumDrawingTool.Polygon;
                        this.currentShape.Image = Resources.polygonShapeIcon;
                        break;
                    }
                case "bezier":
                    {
                        this.btnFill.Enabled = true;
                        Tools.PaintTools.DrawingTool = Tools.PaintTools.EnumDrawingTool.Bezier;
                        this.currentShape.Image = Resources.bezierShapeIcon;
                        break;
                    }
            }
        }

        private void btnEraser3px_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.DrawingSpace.embed();
            btnSelect.Checked = false;
            Tools.PaintTools.EraserWidth = 3;
            Tools.PaintTools.DrawingTool = Tools.PaintTools.EnumDrawingTool.Eraser;

            btnEraser5px.Checked = false;
            btnEraser10px.Checked = false;
            btnEraser20px.Checked = false;

            if(!btnEraser3px.Checked)
            {
                btnEraser3px.Checked = true;
            }
            this.currentShape.Image = Resources.eraserIcon;
        }

        private void btnEraser5px_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.DrawingSpace.embed();
            btnSelect.Checked = false;
            Tools.PaintTools.EraserWidth = 5;
            Tools.PaintTools.DrawingTool = Tools.PaintTools.EnumDrawingTool.Eraser;

            btnEraser3px.Checked = false;
            btnEraser10px.Checked = false;
            btnEraser20px.Checked = false;

            if (!btnEraser5px.Checked)
            {
                btnEraser5px.Checked = true;
            }
            this.currentShape.Image = Resources.eraserIcon;
        }

        private void btnEraser10px_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.DrawingSpace.embed();
            btnSelect.Checked = false;
            Tools.PaintTools.EraserWidth = 10;
            Tools.PaintTools.DrawingTool = Tools.PaintTools.EnumDrawingTool.Eraser;

            btnEraser3px.Checked = false;
            btnEraser5px.Checked = false;
            btnEraser20px.Checked = false;

            if (!btnEraser10px.Checked)
            {
                btnEraser10px.Checked = true;
            }
            this.currentShape.Image = Resources.eraserIcon;

        }

        private void btnEraser20px_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.DrawingSpace.embed();
            btnSelect.Checked = false;
            Tools.PaintTools.EraserWidth = 20;
            Tools.PaintTools.DrawingTool = Tools.PaintTools.EnumDrawingTool.Eraser;

            btnEraser3px.Checked = false;
            btnEraser5px.Checked = false;
            btnEraser10px.Checked = false;

            if (!btnEraser20px.Checked)
            {
                btnEraser20px.Checked = true;
            }
            this.currentShape.Image = Resources.eraserIcon;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DrawingSpace.ListBack.Count == 0)
                return;

            DialogResult result = MessageBox.Show("Do you want to save change to Untitled?", "Paint", MessageBoxButtons.YesNoCancel);
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                btnSave_ItemClick(null, null);
                return;
            }
            else if (result == System.Windows.Forms.DialogResult.No)
            {
                return;
            }
            else
                e.Cancel = true;
        }

        private void selectColor_EditValueChanged(object sender, EventArgs e)
        {
            Tools.PaintTools.DrawingColor = (Color)selectColor.EditValue;
            this.colorStatus.BackColor = Tools.PaintTools.DrawingColor;
        }

        private void btnCopy_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            menuItemCopy_Click(null, null);
        }

        private void btnCut_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            menuItemCut_Click(null, null);
        }

        private void btnPaste_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            menuItemPaste_Click(null, null);
        }

        #endregion

        private void noFill_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            menuSubItemUnFill_Click(null, null);
        }

        private void solidColor_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            menuSubItemSolidBrush_Click(null, null);
        }

        private void linearBrush_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            menuSubItemLinearGradientBrush_Click(null, null);
        }

        private void textureBrush_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            menuSubItemTextureBrush_Click(null, null);
        }

        private void hatchBrush_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            menuSubItemHatchBrush_Click(null, null);
        }

        private void menuItemRotate_Click(object sender, EventArgs e)
        {
            this.DrawingSpace.DrawingPanel.ActiveShape.RotateShape(90);
            this.DrawingSpace.refreshPanel();
        }
    }
}
