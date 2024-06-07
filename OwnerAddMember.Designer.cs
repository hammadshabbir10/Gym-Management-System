namespace Member_Forms
{
    partial class OwnerAddMember
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
            this.comboBoxGyms = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.comboBoxMemberID = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.label9 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // comboBoxGyms
            // 
            this.comboBoxGyms.FormattingEnabled = true;
            this.comboBoxGyms.Items.AddRange(new object[] {
            "Male",
            "Female",
            "Others"});
            this.comboBoxGyms.Location = new System.Drawing.Point(279, 275);
            this.comboBoxGyms.Name = "comboBoxGyms";
            this.comboBoxGyms.Size = new System.Drawing.Size(173, 21);
            this.comboBoxGyms.TabIndex = 90;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Bodoni MT", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.SystemColors.ScrollBar;
            this.label7.Location = new System.Drawing.Point(331, 242);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(64, 19);
            this.label7.TabIndex = 89;
            this.label7.Text = "Gym ID";
            // 
            // comboBoxMemberID
            // 
            this.comboBoxMemberID.FormattingEnabled = true;
            this.comboBoxMemberID.Items.AddRange(new object[] {
            "Male",
            "Female",
            "Others"});
            this.comboBoxMemberID.Location = new System.Drawing.Point(279, 124);
            this.comboBoxMemberID.Name = "comboBoxMemberID";
            this.comboBoxMemberID.Size = new System.Drawing.Size(173, 21);
            this.comboBoxMemberID.TabIndex = 88;
            this.comboBoxMemberID.SelectedIndexChanged += new System.EventHandler(this.comboBoxMemberID_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Bodoni MT", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ScrollBar;
            this.label2.Location = new System.Drawing.Point(321, 84);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 19);
            this.label2.TabIndex = 87;
            this.label2.Text = "MemberID";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.SteelBlue;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button1.ForeColor = System.Drawing.SystemColors.ScrollBar;
            this.button1.Location = new System.Drawing.Point(250, 331);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(246, 39);
            this.button1.TabIndex = 78;
            this.button1.Text = "Register";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Bodoni MT", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label1.Location = new System.Drawing.Point(273, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(223, 32);
            this.label1.TabIndex = 71;
            this.label1.Text = "Member Addition ";
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.Location = new System.Drawing.Point(279, 206);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(173, 20);
            this.dateTimePicker2.TabIndex = 92;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Bodoni MT", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.SystemColors.ScrollBar;
            this.label9.Location = new System.Drawing.Point(298, 166);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(132, 19);
            this.label9.TabIndex = 91;
            this.label9.Text = "Registration Date";
            // 
            // OwnerAddMember
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.WindowText;
            this.ClientSize = new System.Drawing.Size(751, 399);
            this.Controls.Add(this.dateTimePicker2);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.comboBoxGyms);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.comboBoxMemberID);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Name = "OwnerAddMember";
            this.Text = "OwnerAddMember";
            this.Load += new System.EventHandler(this.OwnerAddMember_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxGyms;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboBoxMemberID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.Label label9;
    }
}