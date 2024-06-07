namespace Member_Forms
{
    partial class MemberAddGym
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
            this.button1 = new System.Windows.Forms.Button();
            this.comboBoxGyms = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.SteelBlue;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button1.Font = new System.Drawing.Font("Bodoni MT", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.button1.Location = new System.Drawing.Point(300, 229);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(217, 44);
            this.button1.TabIndex = 42;
            this.button1.Text = "Get Add";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // comboBoxGyms
            // 
            this.comboBoxGyms.FormattingEnabled = true;
            this.comboBoxGyms.Location = new System.Drawing.Point(300, 155);
            this.comboBoxGyms.Name = "comboBoxGyms";
            this.comboBoxGyms.Size = new System.Drawing.Size(217, 21);
            this.comboBoxGyms.TabIndex = 41;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Bodoni MT", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label1.Location = new System.Drawing.Point(342, 93);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(122, 32);
            this.label1.TabIndex = 40;
            this.label1.Text = "Gym List";
            // 
            // MemberAddGym
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.WindowText;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.comboBoxGyms);
            this.Controls.Add(this.label1);
            this.Name = "MemberAddGym";
            this.Text = "MemberAddGym";
            this.Load += new System.EventHandler(this.MemberAddGym_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox comboBoxGyms;
        private System.Windows.Forms.Label label1;
    }
}