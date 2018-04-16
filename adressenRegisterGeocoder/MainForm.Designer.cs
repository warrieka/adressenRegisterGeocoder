namespace adressenRegisterGeocoder
{
   partial class MainForm
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
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
         this.mapBox = new SharpMap.Forms.MapBox();
         this.saveBtn = new System.Windows.Forms.Button();
         this.saveShapeDlg = new System.Windows.Forms.SaveFileDialog();
         this.splitContainer = new System.Windows.Forms.SplitContainer();
         this.statusStrip1 = new System.Windows.Forms.StatusStrip();
         this.msgLbl = new System.Windows.Forms.ToolStripStatusLabel();
         this.ZoekBtn = new System.Windows.Forms.Button();
         this.label4 = new System.Windows.Forms.Label();
         this.straatTxt = new System.Windows.Forms.TextBox();
         this.label3 = new System.Windows.Forms.Label();
         this.huisnrTxt = new System.Windows.Forms.TextBox();
         this.label2 = new System.Windows.Forms.Label();
         this.label1 = new System.Windows.Forms.Label();
         this.postcodeTxt = new System.Windows.Forms.TextBox();
         this.gemeenteTxt = new System.Windows.Forms.TextBox();
         this.basemapCbx = new System.Windows.Forms.ComboBox();
         this.fullExtendBtn = new System.Windows.Forms.Button();
         this.zoomOUTBtn = new System.Windows.Forms.Button();
         this.zoomINBtn = new System.Windows.Forms.Button();
         ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
         this.splitContainer.Panel1.SuspendLayout();
         this.splitContainer.Panel2.SuspendLayout();
         this.splitContainer.SuspendLayout();
         this.statusStrip1.SuspendLayout();
         this.SuspendLayout();
         // 
         // mapBox
         // 
         this.mapBox.ActiveTool = SharpMap.Forms.MapBox.Tools.Pan;
         this.mapBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.mapBox.BackColor = System.Drawing.Color.White;
         this.mapBox.Cursor = System.Windows.Forms.Cursors.Hand;
         this.mapBox.FineZoomFactor = 10D;
         this.mapBox.Location = new System.Drawing.Point(0, 0);
         this.mapBox.MapQueryMode = SharpMap.Forms.MapBox.MapQueryType.TopMostLayer;
         this.mapBox.Name = "mapBox";
         this.mapBox.QueryGrowFactor = 5F;
         this.mapBox.QueryLayerIndex = 0;
         this.mapBox.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
         this.mapBox.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
         this.mapBox.ShowProgressUpdate = false;
         this.mapBox.Size = new System.Drawing.Size(411, 471);
         this.mapBox.TabIndex = 0;
         this.mapBox.Text = "mapBox";
         this.mapBox.WheelZoomMagnitude = -2D;
         // 
         // saveBtn
         // 
         this.saveBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.saveBtn.Location = new System.Drawing.Point(304, 477);
         this.saveBtn.Name = "saveBtn";
         this.saveBtn.Size = new System.Drawing.Size(104, 28);
         this.saveBtn.TabIndex = 10;
         this.saveBtn.TabStop = false;
         this.saveBtn.Text = "Save";
         this.saveBtn.UseVisualStyleBackColor = true;
         this.saveBtn.Click += new System.EventHandler(this.saveBtn_Click);
         // 
         // saveShapeDlg
         // 
         this.saveShapeDlg.DefaultExt = "shp";
         this.saveShapeDlg.Filter = "Shapefile|*.shp";
         // 
         // splitContainer
         // 
         this.splitContainer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
         this.splitContainer.Location = new System.Drawing.Point(0, 0);
         this.splitContainer.Name = "splitContainer";
         // 
         // splitContainer.Panel1
         // 
         this.splitContainer.Panel1.Controls.Add(this.statusStrip1);
         this.splitContainer.Panel1.Controls.Add(this.ZoekBtn);
         this.splitContainer.Panel1.Controls.Add(this.label4);
         this.splitContainer.Panel1.Controls.Add(this.straatTxt);
         this.splitContainer.Panel1.Controls.Add(this.label3);
         this.splitContainer.Panel1.Controls.Add(this.huisnrTxt);
         this.splitContainer.Panel1.Controls.Add(this.label2);
         this.splitContainer.Panel1.Controls.Add(this.label1);
         this.splitContainer.Panel1.Controls.Add(this.postcodeTxt);
         this.splitContainer.Panel1.Controls.Add(this.gemeenteTxt);
         // 
         // splitContainer.Panel2
         // 
         this.splitContainer.Panel2.Controls.Add(this.basemapCbx);
         this.splitContainer.Panel2.Controls.Add(this.fullExtendBtn);
         this.splitContainer.Panel2.Controls.Add(this.zoomOUTBtn);
         this.splitContainer.Panel2.Controls.Add(this.zoomINBtn);
         this.splitContainer.Panel2.Controls.Add(this.mapBox);
         this.splitContainer.Panel2.Controls.Add(this.saveBtn);
         this.splitContainer.Size = new System.Drawing.Size(872, 512);
         this.splitContainer.SplitterDistance = 453;
         this.splitContainer.TabIndex = 2;
         // 
         // statusStrip1
         // 
         this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
         this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.msgLbl});
         this.statusStrip1.Location = new System.Drawing.Point(0, 484);
         this.statusStrip1.Name = "statusStrip1";
         this.statusStrip1.Size = new System.Drawing.Size(449, 24);
         this.statusStrip1.TabIndex = 9;
         this.statusStrip1.Text = "statusStrip1";
         // 
         // msgLbl
         // 
         this.msgLbl.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
         this.msgLbl.Name = "msgLbl";
         this.msgLbl.Size = new System.Drawing.Size(0, 19);
         // 
         // ZoekBtn
         // 
         this.ZoekBtn.Location = new System.Drawing.Point(10, 256);
         this.ZoekBtn.Name = "ZoekBtn";
         this.ZoekBtn.Size = new System.Drawing.Size(75, 23);
         this.ZoekBtn.TabIndex = 8;
         this.ZoekBtn.Text = "Zoek";
         this.ZoekBtn.UseVisualStyleBackColor = true;
         this.ZoekBtn.Click += new System.EventHandler(this.ZoekBtn_Click);
         // 
         // label4
         // 
         this.label4.AutoSize = true;
         this.label4.Location = new System.Drawing.Point(7, 143);
         this.label4.Name = "label4";
         this.label4.Size = new System.Drawing.Size(46, 17);
         this.label4.TabIndex = 7;
         this.label4.Text = "Straat";
         // 
         // straatTxt
         // 
         this.straatTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.straatTxt.Location = new System.Drawing.Point(10, 163);
         this.straatTxt.Name = "straatTxt";
         this.straatTxt.Size = new System.Drawing.Size(423, 22);
         this.straatTxt.TabIndex = 5;
         // 
         // label3
         // 
         this.label3.AutoSize = true;
         this.label3.Location = new System.Drawing.Point(7, 198);
         this.label3.Name = "label3";
         this.label3.RightToLeft = System.Windows.Forms.RightToLeft.No;
         this.label3.Size = new System.Drawing.Size(49, 17);
         this.label3.TabIndex = 5;
         this.label3.Text = "Huisnr";
         // 
         // huisnrTxt
         // 
         this.huisnrTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.huisnrTxt.Location = new System.Drawing.Point(10, 218);
         this.huisnrTxt.Name = "huisnrTxt";
         this.huisnrTxt.Size = new System.Drawing.Size(423, 22);
         this.huisnrTxt.TabIndex = 6;
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(7, 82);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(67, 17);
         this.label2.TabIndex = 3;
         this.label2.Text = "Postcode";
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(7, 25);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(74, 17);
         this.label1.TabIndex = 2;
         this.label1.Text = "Gemeente";
         // 
         // postcodeTxt
         // 
         this.postcodeTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.postcodeTxt.Location = new System.Drawing.Point(10, 102);
         this.postcodeTxt.Name = "postcodeTxt";
         this.postcodeTxt.Size = new System.Drawing.Size(423, 22);
         this.postcodeTxt.TabIndex = 2;
         // 
         // gemeenteTxt
         // 
         this.gemeenteTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.gemeenteTxt.Location = new System.Drawing.Point(10, 45);
         this.gemeenteTxt.Name = "gemeenteTxt";
         this.gemeenteTxt.Size = new System.Drawing.Size(423, 22);
         this.gemeenteTxt.TabIndex = 0;
         // 
         // basemapCbx
         // 
         this.basemapCbx.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
         this.basemapCbx.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.basemapCbx.FormattingEnabled = true;
         this.basemapCbx.Items.AddRange(new object[] {
            "GRB",
            "Luchtfoto",
            "Antwerpen"});
         this.basemapCbx.Location = new System.Drawing.Point(3, 477);
         this.basemapCbx.Name = "basemapCbx";
         this.basemapCbx.Size = new System.Drawing.Size(121, 24);
         this.basemapCbx.TabIndex = 8;
         this.basemapCbx.TabStop = false;
         this.basemapCbx.SelectedIndexChanged += new System.EventHandler(this.basemapCbx_SelectedIndexChanged);
         // 
         // fullExtendBtn
         // 
         this.fullExtendBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.fullExtendBtn.BackgroundImage = global::adressenRegisterGeocoder.Properties.Resources.full_extent;
         this.fullExtendBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
         this.fullExtendBtn.Location = new System.Drawing.Point(373, 39);
         this.fullExtendBtn.Name = "fullExtendBtn";
         this.fullExtendBtn.Size = new System.Drawing.Size(28, 28);
         this.fullExtendBtn.TabIndex = 4;
         this.fullExtendBtn.UseVisualStyleBackColor = true;
         this.fullExtendBtn.Click += new System.EventHandler(this.fullExtendBtn_Click);
         // 
         // zoomOUTBtn
         // 
         this.zoomOUTBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.zoomOUTBtn.Location = new System.Drawing.Point(373, 73);
         this.zoomOUTBtn.Name = "zoomOUTBtn";
         this.zoomOUTBtn.Size = new System.Drawing.Size(28, 23);
         this.zoomOUTBtn.TabIndex = 3;
         this.zoomOUTBtn.Text = "-";
         this.zoomOUTBtn.UseVisualStyleBackColor = true;
         this.zoomOUTBtn.Click += new System.EventHandler(this.zoomOUTBtn_Click);
         // 
         // zoomINBtn
         // 
         this.zoomINBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.zoomINBtn.Location = new System.Drawing.Point(373, 10);
         this.zoomINBtn.Name = "zoomINBtn";
         this.zoomINBtn.Size = new System.Drawing.Size(28, 23);
         this.zoomINBtn.TabIndex = 2;
         this.zoomINBtn.Text = "+";
         this.zoomINBtn.UseVisualStyleBackColor = true;
         this.zoomINBtn.Click += new System.EventHandler(this.zoomINBtn_Click);
         // 
         // MainForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(872, 512);
         this.Controls.Add(this.splitContainer);
         this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
         this.Name = "MainForm";
         this.Text = "Adressen Geocoderen";
         this.splitContainer.Panel1.ResumeLayout(false);
         this.splitContainer.Panel1.PerformLayout();
         this.splitContainer.Panel2.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
         this.splitContainer.ResumeLayout(false);
         this.statusStrip1.ResumeLayout(false);
         this.statusStrip1.PerformLayout();
         this.ResumeLayout(false);

      }

      #endregion

      private SharpMap.Forms.MapBox mapBox;
      private System.Windows.Forms.Button saveBtn;
      private System.Windows.Forms.SaveFileDialog saveShapeDlg;
      private System.Windows.Forms.SplitContainer splitContainer;
      private System.Windows.Forms.Label label4;
      private System.Windows.Forms.TextBox straatTxt;
      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.TextBox huisnrTxt;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.TextBox postcodeTxt;
      private System.Windows.Forms.TextBox gemeenteTxt;
      private System.Windows.Forms.Button ZoekBtn;
      private System.Windows.Forms.Button zoomOUTBtn;
      private System.Windows.Forms.Button zoomINBtn;
      private System.Windows.Forms.Button fullExtendBtn;
      private System.Windows.Forms.ComboBox basemapCbx;
      private System.Windows.Forms.StatusStrip statusStrip1;
      private System.Windows.Forms.ToolStripStatusLabel msgLbl;
   }
}

