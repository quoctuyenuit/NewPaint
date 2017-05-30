namespace MyPaint.FillEvent
{
    partial class HatchBrush
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.cbForeColor = new DevExpress.XtraEditors.ColorPickEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.cbBackColor = new DevExpress.XtraEditors.ColorPickEdit();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbStyle = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.btnOk = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.cbForeColor.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbBackColor.Properties)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbStyle.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.labelControl1.Location = new System.Drawing.Point(36, 29);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(66, 18);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "ForeColor:";
            // 
            // cbForeColor
            // 
            this.cbForeColor.EditValue = System.Drawing.Color.White;
            this.cbForeColor.Location = new System.Drawing.Point(119, 26);
            this.cbForeColor.Name = "cbForeColor";
            this.cbForeColor.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.cbForeColor.Properties.Appearance.Options.UseFont = true;
            this.cbForeColor.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbForeColor.Size = new System.Drawing.Size(186, 24);
            this.cbForeColor.TabIndex = 1;
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.labelControl2.Location = new System.Drawing.Point(36, 61);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(68, 18);
            this.labelControl2.TabIndex = 0;
            this.labelControl2.Text = "BackColor:";
            // 
            // cbBackColor
            // 
            this.cbBackColor.EditValue = System.Drawing.Color.White;
            this.cbBackColor.Location = new System.Drawing.Point(119, 58);
            this.cbBackColor.Name = "cbBackColor";
            this.cbBackColor.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.cbBackColor.Properties.Appearance.Options.UseFont = true;
            this.cbBackColor.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbBackColor.Size = new System.Drawing.Size(186, 24);
            this.cbBackColor.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cbStyle);
            this.panel1.Controls.Add(this.cbBackColor);
            this.panel1.Controls.Add(this.labelControl3);
            this.panel1.Controls.Add(this.labelControl2);
            this.panel1.Controls.Add(this.cbForeColor);
            this.panel1.Controls.Add(this.labelControl1);
            this.panel1.Location = new System.Drawing.Point(13, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(340, 143);
            this.panel1.TabIndex = 2;
            // 
            // cbStyle
            // 
            this.cbStyle.Location = new System.Drawing.Point(119, 90);
            this.cbStyle.Name = "cbStyle";
            this.cbStyle.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.cbStyle.Properties.Appearance.Options.UseFont = true;
            this.cbStyle.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbStyle.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cbStyle.Size = new System.Drawing.Size(186, 24);
            this.cbStyle.TabIndex = 2;
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.labelControl3.Location = new System.Drawing.Point(36, 93);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(36, 18);
            this.labelControl3.TabIndex = 0;
            this.labelControl3.Text = "Style:";
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(279, 166);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 33);
            this.btnOk.TabIndex = 3;
            this.btnOk.Text = "OK";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // HatchBrush
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(366, 211);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HatchBrush";
            this.Text = "HatchBrush";
            ((System.ComponentModel.ISupportInitialize)(this.cbForeColor.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbBackColor.Properties)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbStyle.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.ColorPickEdit cbForeColor;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.ColorPickEdit cbBackColor;
        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraEditors.SimpleButton btnOk;
        private DevExpress.XtraEditors.ComboBoxEdit cbStyle;
        private DevExpress.XtraEditors.LabelControl labelControl3;
    }
}