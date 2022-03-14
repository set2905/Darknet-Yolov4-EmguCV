namespace DarknetYOLOv4
{
    partial class ObjectDetectorForm
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
            this.StartButton = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.PlayModeComboCox = new System.Windows.Forms.ComboBox();
            this.isFpsFixedBox = new System.Windows.Forms.CheckBox();
            this.FixedFpsValueBox = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FixedFpsValueBox)).BeginInit();
            this.SuspendLayout();
            // 
            // StartButton
            // 
            this.StartButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(42)))), ((int)(((byte)(86)))));
            this.StartButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(42)))), ((int)(((byte)(86)))));
            this.StartButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.StartButton.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StartButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(189)))), ((int)(((byte)(50)))));
            this.StartButton.Location = new System.Drawing.Point(5, 17);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(103, 44);
            this.StartButton.TabIndex = 0;
            this.StartButton.Text = "START";
            this.StartButton.UseVisualStyleBackColor = false;
            this.StartButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(-2, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(704, 515);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(60)))), ((int)(((byte)(117)))));
            this.label1.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.Info;
            this.label1.Location = new System.Drawing.Point(706, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(353, 417);
            this.label1.TabIndex = 2;
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(60)))), ((int)(((byte)(117)))));
            this.panel1.Controls.Add(this.StartButton);
            this.panel1.Location = new System.Drawing.Point(706, 444);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(353, 71);
            this.panel1.TabIndex = 3;
            // 
            // PlayModeComboCox
            // 
            this.PlayModeComboCox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.PlayModeComboCox.FormattingEnabled = true;
            this.PlayModeComboCox.Location = new System.Drawing.Point(711, 0);
            this.PlayModeComboCox.Name = "PlayModeComboCox";
            this.PlayModeComboCox.Size = new System.Drawing.Size(164, 21);
            this.PlayModeComboCox.TabIndex = 1;
            this.PlayModeComboCox.SelectedIndexChanged += new System.EventHandler(this.PlayModeComboCox_SelectedIndexChanged);
            // 
            // isFpsFixedBox
            // 
            this.isFpsFixedBox.AutoSize = true;
            this.isFpsFixedBox.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.isFpsFixedBox.Location = new System.Drawing.Point(924, 3);
            this.isFpsFixedBox.Name = "isFpsFixedBox";
            this.isFpsFixedBox.Size = new System.Drawing.Size(74, 17);
            this.isFpsFixedBox.TabIndex = 4;
            this.isFpsFixedBox.Text = "Fixed FPS";
            this.isFpsFixedBox.UseVisualStyleBackColor = true;
            this.isFpsFixedBox.CheckedChanged += new System.EventHandler(this.isFpsFixedBox_CheckedChanged);
            // 
            // FixedFpsValueBox
            // 
            this.FixedFpsValueBox.Location = new System.Drawing.Point(992, 1);
            this.FixedFpsValueBox.Name = "FixedFpsValueBox";
            this.FixedFpsValueBox.Size = new System.Drawing.Size(39, 20);
            this.FixedFpsValueBox.TabIndex = 5;
            this.FixedFpsValueBox.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // ObjectDetectorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(59)))), ((int)(((byte)(72)))));
            this.ClientSize = new System.Drawing.Size(1059, 517);
            this.Controls.Add(this.FixedFpsValueBox);
            this.Controls.Add(this.isFpsFixedBox);
            this.Controls.Add(this.PlayModeComboCox);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Name = "ObjectDetectorForm";
            this.Text = "ObjectDetectorForm";
            this.Load += new System.EventHandler(this.ObjectDetectorForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.FixedFpsValueBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button StartButton;
        public System.Windows.Forms.Label label1;
        public System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.ComboBox PlayModeComboCox;
        public System.Windows.Forms.CheckBox isFpsFixedBox;
        public System.Windows.Forms.NumericUpDown FixedFpsValueBox;
    }
}