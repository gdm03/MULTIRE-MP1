namespace MP1
{
    partial class MP1Form
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
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.colorHistogramToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.selectedImageBox = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chbutton = new System.Windows.Forms.Button();
            this.perceptualbutton = new System.Windows.Forms.Button();
            this.colorcoherencebutton = new System.Windows.Forms.Button();
            this.chcenterbutton = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.selectedImageBox)).BeginInit();
            this.SuspendLayout();
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.openToolStripMenuItem.Text = "Open Image";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.colorHistogramToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // colorHistogramToolStripMenuItem
            // 
            this.colorHistogramToolStripMenuItem.Name = "colorHistogramToolStripMenuItem";
            this.colorHistogramToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.colorHistogramToolStripMenuItem.Text = "Color Histogram";
            this.colorHistogramToolStripMenuItem.Click += new System.EventHandler(this.colorHistogramToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(502, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // selectedImageBox
            // 
            this.selectedImageBox.Location = new System.Drawing.Point(76, 42);
            this.selectedImageBox.MaximumSize = new System.Drawing.Size(200, 200);
            this.selectedImageBox.Name = "selectedImageBox";
            this.selectedImageBox.Size = new System.Drawing.Size(100, 100);
            this.selectedImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.selectedImageBox.TabIndex = 2;
            this.selectedImageBox.TabStop = false;
            this.selectedImageBox.Click += new System.EventHandler(this.selectedImage_Click);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Location = new System.Drawing.Point(255, 42);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(235, 354);
            this.panel1.TabIndex = 3;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // chbutton
            // 
            this.chbutton.Location = new System.Drawing.Point(24, 163);
            this.chbutton.Name = "chbutton";
            this.chbutton.Size = new System.Drawing.Size(203, 37);
            this.chbutton.TabIndex = 4;
            this.chbutton.Text = "Color Histogram";
            this.chbutton.UseVisualStyleBackColor = true;
            this.chbutton.Click += new System.EventHandler(this.button1_Click);
            // 
            // perceptualbutton
            // 
            this.perceptualbutton.Location = new System.Drawing.Point(24, 206);
            this.perceptualbutton.Name = "perceptualbutton";
            this.perceptualbutton.Size = new System.Drawing.Size(203, 42);
            this.perceptualbutton.TabIndex = 5;
            this.perceptualbutton.Text = "CH with Perceptual Similarity";
            this.perceptualbutton.UseVisualStyleBackColor = true;
            // 
            // colorcoherencebutton
            // 
            this.colorcoherencebutton.Location = new System.Drawing.Point(24, 254);
            this.colorcoherencebutton.Name = "colorcoherencebutton";
            this.colorcoherencebutton.Size = new System.Drawing.Size(203, 45);
            this.colorcoherencebutton.TabIndex = 6;
            this.colorcoherencebutton.Text = "Histogram Refinement with Color Coherence";
            this.colorcoherencebutton.UseVisualStyleBackColor = true;
            // 
            // chcenterbutton
            // 
            this.chcenterbutton.Location = new System.Drawing.Point(24, 305);
            this.chcenterbutton.Name = "chcenterbutton";
            this.chcenterbutton.Size = new System.Drawing.Size(203, 39);
            this.chcenterbutton.TabIndex = 7;
            this.chcenterbutton.Text = "CH with Centering Refinement";
            this.chcenterbutton.UseVisualStyleBackColor = true;
            // 
            // MP1Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(502, 408);
            this.Controls.Add(this.chcenterbutton);
            this.Controls.Add(this.colorcoherencebutton);
            this.Controls.Add(this.perceptualbutton);
            this.Controls.Add(this.chbutton);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.selectedImageBox);
            this.Controls.Add(this.menuStrip1);
            this.Name = "MP1Form";
            this.Text = "MP1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.selectedImageBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.PictureBox selectedImageBox;
        private System.Windows.Forms.ToolStripMenuItem colorHistogramToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button chbutton;
        private System.Windows.Forms.Button perceptualbutton;
        private System.Windows.Forms.Button colorcoherencebutton;
        private System.Windows.Forms.Button chcenterbutton;
    }
}

