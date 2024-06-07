namespace Member_Forms
{
    partial class Owner_Interface
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Owner_Interface));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mEMBERSHIPSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tRAINERSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mEMBERSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pERFORMANCEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lOGOUTToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addMemberToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addMemberToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.menuStrip1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(30, 30);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mEMBERSHIPSToolStripMenuItem,
            this.tRAINERSToolStripMenuItem,
            this.addMemberToolStripMenuItem1,
            this.mEMBERSToolStripMenuItem,
            this.pERFORMANCEToolStripMenuItem,
            this.lOGOUTToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(106, 9);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.menuStrip1.Size = new System.Drawing.Size(665, 38);
            this.menuStrip1.Stretch = false;
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // mEMBERSHIPSToolStripMenuItem
            // 
            this.mEMBERSHIPSToolStripMenuItem.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.mEMBERSHIPSToolStripMenuItem.ForeColor = System.Drawing.SystemColors.ScrollBar;
            this.mEMBERSHIPSToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("mEMBERSHIPSToolStripMenuItem.Image")));
            this.mEMBERSHIPSToolStripMenuItem.Name = "mEMBERSHIPSToolStripMenuItem";
            this.mEMBERSHIPSToolStripMenuItem.Size = new System.Drawing.Size(128, 34);
            this.mEMBERSHIPSToolStripMenuItem.Text = "MEMBERSHIPS";
            this.mEMBERSHIPSToolStripMenuItem.Click += new System.EventHandler(this.mEMBERSHIPSToolStripMenuItem_Click);
            // 
            // tRAINERSToolStripMenuItem
            // 
            this.tRAINERSToolStripMenuItem.ForeColor = System.Drawing.SystemColors.ScrollBar;
            this.tRAINERSToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("tRAINERSToolStripMenuItem.Image")));
            this.tRAINERSToolStripMenuItem.Name = "tRAINERSToolStripMenuItem";
            this.tRAINERSToolStripMenuItem.Size = new System.Drawing.Size(101, 34);
            this.tRAINERSToolStripMenuItem.Text = "TRAINERS";
            this.tRAINERSToolStripMenuItem.Click += new System.EventHandler(this.tRAINERSToolStripMenuItem_Click);
            // 
            // mEMBERSToolStripMenuItem
            // 
            this.mEMBERSToolStripMenuItem.ForeColor = System.Drawing.SystemColors.ScrollBar;
            this.mEMBERSToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("mEMBERSToolStripMenuItem.Image")));
            this.mEMBERSToolStripMenuItem.Name = "mEMBERSToolStripMenuItem";
            this.mEMBERSToolStripMenuItem.Size = new System.Drawing.Size(103, 34);
            this.mEMBERSToolStripMenuItem.Text = "MEMBERS";
            this.mEMBERSToolStripMenuItem.Click += new System.EventHandler(this.mEMBERSToolStripMenuItem_Click);
            // 
            // pERFORMANCEToolStripMenuItem
            // 
            this.pERFORMANCEToolStripMenuItem.ForeColor = System.Drawing.SystemColors.ScrollBar;
            this.pERFORMANCEToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("pERFORMANCEToolStripMenuItem.Image")));
            this.pERFORMANCEToolStripMenuItem.Name = "pERFORMANCEToolStripMenuItem";
            this.pERFORMANCEToolStripMenuItem.Size = new System.Drawing.Size(101, 34);
            this.pERFORMANCEToolStripMenuItem.Text = "TRAINERS";
            this.pERFORMANCEToolStripMenuItem.Click += new System.EventHandler(this.pERFORMANCEToolStripMenuItem_Click);
            // 
            // lOGOUTToolStripMenuItem
            // 
            this.lOGOUTToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addMemberToolStripMenuItem});
            this.lOGOUTToolStripMenuItem.ForeColor = System.Drawing.SystemColors.ScrollBar;
            this.lOGOUTToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("lOGOUTToolStripMenuItem.Image")));
            this.lOGOUTToolStripMenuItem.Name = "lOGOUTToolStripMenuItem";
            this.lOGOUTToolStripMenuItem.Size = new System.Drawing.Size(98, 34);
            this.lOGOUTToolStripMenuItem.Text = "LOG OUT";
            this.lOGOUTToolStripMenuItem.Click += new System.EventHandler(this.lOGOUTToolStripMenuItem_Click);
            // 
            // addMemberToolStripMenuItem
            // 
            this.addMemberToolStripMenuItem.Name = "addMemberToolStripMenuItem";
            this.addMemberToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.addMemberToolStripMenuItem.Text = "Add Member";
            // 
            // addMemberToolStripMenuItem1
            // 
            this.addMemberToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2});
            this.addMemberToolStripMenuItem1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addMemberToolStripMenuItem1.ForeColor = System.Drawing.SystemColors.ScrollBar;
            this.addMemberToolStripMenuItem1.Image = ((System.Drawing.Image)(resources.GetObject("addMemberToolStripMenuItem1.Image")));
            this.addMemberToolStripMenuItem1.Name = "addMemberToolStripMenuItem1";
            this.addMemberToolStripMenuItem1.Size = new System.Drawing.Size(128, 34);
            this.addMemberToolStripMenuItem1.Text = "Add Member";
            this.addMemberToolStripMenuItem1.Click += new System.EventHandler(this.addMemberToolStripMenuItem1_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(180, 22);
            this.toolStripMenuItem2.Text = "+";
            // 
            // Owner_Interface
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.menuStrip1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Owner_Interface";
            this.Text = "Owner_Interface";
            this.Load += new System.EventHandler(this.Owner_Interface_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mEMBERSHIPSToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tRAINERSToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mEMBERSToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pERFORMANCEToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lOGOUTToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addMemberToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addMemberToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
    }
}