namespace Map
{
    partial class Main
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.selectTypesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.depthfirstSearchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.breadthToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.computeDistanceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startCharTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.endCharTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.seriesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sequenceOutputTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.resetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.coordinatesLabel = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectTypesToolStripMenuItem,
            this.computeDistanceToolStripMenuItem,
            this.startCharTextBox,
            this.endCharTextBox,
            this.seriesToolStripMenuItem,
            this.sequenceOutputTextBox,
            this.resetToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1165, 27);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // selectTypesToolStripMenuItem
            // 
            this.selectTypesToolStripMenuItem.Checked = true;
            this.selectTypesToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.selectTypesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.depthfirstSearchToolStripMenuItem,
            this.breadthToolStripMenuItem});
            this.selectTypesToolStripMenuItem.Name = "selectTypesToolStripMenuItem";
            this.selectTypesToolStripMenuItem.Size = new System.Drawing.Size(121, 23);
            this.selectTypesToolStripMenuItem.Text = "Select Search Types";
            // 
            // depthfirstSearchToolStripMenuItem
            // 
            this.depthfirstSearchToolStripMenuItem.Name = "depthfirstSearchToolStripMenuItem";
            this.depthfirstSearchToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.depthfirstSearchToolStripMenuItem.Text = "Depth-first Search";
            this.depthfirstSearchToolStripMenuItem.Click += new System.EventHandler(this.depthfirstSearchToolStripMenuItem_Click);
            // 
            // breadthToolStripMenuItem
            // 
            this.breadthToolStripMenuItem.Name = "breadthToolStripMenuItem";
            this.breadthToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.breadthToolStripMenuItem.Text = "Breadth-first Search";
            this.breadthToolStripMenuItem.Click += new System.EventHandler(this.breadthToolStripMenuItem_Click);
            // 
            // computeDistanceToolStripMenuItem
            // 
            this.computeDistanceToolStripMenuItem.Name = "computeDistanceToolStripMenuItem";
            this.computeDistanceToolStripMenuItem.Size = new System.Drawing.Size(76, 23);
            this.computeDistanceToolStripMenuItem.Text = "Get Routes";
            this.computeDistanceToolStripMenuItem.Click += new System.EventHandler(this.computeDistanceToolStripMenuItem_Click);
            // 
            // startCharTextBox
            // 
            this.startCharTextBox.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.startCharTextBox.Name = "startCharTextBox";
            this.startCharTextBox.Size = new System.Drawing.Size(100, 23);
            this.startCharTextBox.ToolTipText = "Start Character";
            // 
            // endCharTextBox
            // 
            this.endCharTextBox.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.endCharTextBox.Name = "endCharTextBox";
            this.endCharTextBox.Size = new System.Drawing.Size(100, 23);
            this.endCharTextBox.ToolTipText = "End Character";
            // 
            // seriesToolStripMenuItem
            // 
            this.seriesToolStripMenuItem.Name = "seriesToolStripMenuItem";
            this.seriesToolStripMenuItem.Size = new System.Drawing.Size(49, 23);
            this.seriesToolStripMenuItem.Text = "Series";
            // 
            // sequenceOutputTextBox
            // 
            this.sequenceOutputTextBox.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.sequenceOutputTextBox.Name = "sequenceOutputTextBox";
            this.sequenceOutputTextBox.ReadOnly = true;
            this.sequenceOutputTextBox.Size = new System.Drawing.Size(500, 23);
            this.sequenceOutputTextBox.ToolTipText = "Sequence Output";
            // 
            // resetToolStripMenuItem
            // 
            this.resetToolStripMenuItem.Name = "resetToolStripMenuItem";
            this.resetToolStripMenuItem.Size = new System.Drawing.Size(47, 23);
            this.resetToolStripMenuItem.Text = "Reset";
            this.resetToolStripMenuItem.Click += new System.EventHandler(this.resetToolStripMenuItem_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(0, 27);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1165, 700);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            // 
            // coordinatesLabel
            // 
            this.coordinatesLabel.AutoSize = true;
            this.coordinatesLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.coordinatesLabel.Location = new System.Drawing.Point(1493, 4);
            this.coordinatesLabel.Name = "coordinatesLabel";
            this.coordinatesLabel.Size = new System.Drawing.Size(51, 20);
            this.coordinatesLabel.TabIndex = 3;
            this.coordinatesLabel.Text = "label1";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1165, 727);
            this.Controls.Add(this.coordinatesLabel);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Main";
            this.Text = "Main";
            this.Load += new System.EventHandler(this.Main_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem computeDistanceToolStripMenuItem;
        private System.Windows.Forms.ToolStripTextBox startCharTextBox;
        private System.Windows.Forms.ToolStripTextBox endCharTextBox;
        private System.Windows.Forms.ToolStripTextBox sequenceOutputTextBox;
        private System.Windows.Forms.ToolStripMenuItem seriesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectTypesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem depthfirstSearchToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem breadthToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetToolStripMenuItem;
        private System.Windows.Forms.Label coordinatesLabel;
    }
}