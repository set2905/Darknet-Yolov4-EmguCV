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
            this.ScreenShotButton = new System.Windows.Forms.Button();
            this.FileDialogButton = new System.Windows.Forms.Button();
            this.PlayModeComboCox = new System.Windows.Forms.ComboBox();
            this.isFpsFixedBox = new System.Windows.Forms.CheckBox();
            this.FixedFpsValueBox = new System.Windows.Forms.NumericUpDown();
            this.VideoChoicePanel = new System.Windows.Forms.Panel();
            this.VideoButton4 = new System.Windows.Forms.Button();
            this.VideoButton3 = new System.Windows.Forms.Button();
            this.VideoButton2 = new System.Windows.Forms.Button();
            this.VideoButton1 = new System.Windows.Forms.Button();
            this.RightPanel = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FixedFpsValueBox)).BeginInit();
            this.VideoChoicePanel.SuspendLayout();
            this.RightPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // StartButton
            // 
            this.StartButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(42)))), ((int)(((byte)(86)))));
            this.StartButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.StartButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(42)))), ((int)(((byte)(86)))));
            this.StartButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.StartButton.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StartButton.ForeColor = System.Drawing.Color.White;
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
            this.pictureBox1.Enabled = false;
            this.pictureBox1.Location = new System.Drawing.Point(32, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1024, 768);
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
            this.label1.Location = new System.Drawing.Point(3, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(353, 671);
            this.label1.TabIndex = 2;
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(60)))), ((int)(((byte)(117)))));
            this.panel1.Controls.Add(this.ScreenShotButton);
            this.panel1.Controls.Add(this.FileDialogButton);
            this.panel1.Controls.Add(this.StartButton);
            this.panel1.Location = new System.Drawing.Point(3, 703);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(353, 71);
            this.panel1.TabIndex = 3;
            // 
            // ScreenShotButton
            // 
            this.ScreenShotButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(42)))), ((int)(((byte)(86)))));
            this.ScreenShotButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(42)))), ((int)(((byte)(86)))));
            this.ScreenShotButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.ScreenShotButton.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ScreenShotButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(189)))), ((int)(((byte)(50)))));
            this.ScreenShotButton.Location = new System.Drawing.Point(223, 17);
            this.ScreenShotButton.Name = "ScreenShotButton";
            this.ScreenShotButton.Size = new System.Drawing.Size(125, 44);
            this.ScreenShotButton.TabIndex = 2;
            this.ScreenShotButton.Text = "Screenshot";
            this.ScreenShotButton.UseVisualStyleBackColor = false;
            this.ScreenShotButton.Click += new System.EventHandler(this.ScreenShotButton_Click);
            // 
            // FileDialogButton
            // 
            this.FileDialogButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(42)))), ((int)(((byte)(86)))));
            this.FileDialogButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(42)))), ((int)(((byte)(86)))));
            this.FileDialogButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.FileDialogButton.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FileDialogButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(189)))), ((int)(((byte)(50)))));
            this.FileDialogButton.Location = new System.Drawing.Point(114, 17);
            this.FileDialogButton.Name = "FileDialogButton";
            this.FileDialogButton.Size = new System.Drawing.Size(103, 44);
            this.FileDialogButton.TabIndex = 1;
            this.FileDialogButton.Text = "Set File";
            this.FileDialogButton.UseVisualStyleBackColor = false;
            this.FileDialogButton.Click += new System.EventHandler(this.FileDialogButton_Click);
            // 
            // PlayModeComboCox
            // 
            this.PlayModeComboCox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.PlayModeComboCox.FormattingEnabled = true;
            this.PlayModeComboCox.Location = new System.Drawing.Point(8, 5);
            this.PlayModeComboCox.Name = "PlayModeComboCox";
            this.PlayModeComboCox.Size = new System.Drawing.Size(164, 21);
            this.PlayModeComboCox.TabIndex = 1;
            this.PlayModeComboCox.SelectedIndexChanged += new System.EventHandler(this.PlayModeComboCox_SelectedIndexChanged);
            // 
            // isFpsFixedBox
            // 
            this.isFpsFixedBox.AutoSize = true;
            this.isFpsFixedBox.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.isFpsFixedBox.Location = new System.Drawing.Point(221, 8);
            this.isFpsFixedBox.Name = "isFpsFixedBox";
            this.isFpsFixedBox.Size = new System.Drawing.Size(74, 17);
            this.isFpsFixedBox.TabIndex = 4;
            this.isFpsFixedBox.Text = "Fixed FPS";
            this.isFpsFixedBox.UseVisualStyleBackColor = true;
            this.isFpsFixedBox.CheckedChanged += new System.EventHandler(this.isFpsFixedBox_CheckedChanged);
            // 
            // FixedFpsValueBox
            // 
            this.FixedFpsValueBox.Location = new System.Drawing.Point(289, 6);
            this.FixedFpsValueBox.Name = "FixedFpsValueBox";
            this.FixedFpsValueBox.Size = new System.Drawing.Size(39, 20);
            this.FixedFpsValueBox.TabIndex = 5;
            this.FixedFpsValueBox.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // VideoChoicePanel
            // 
            this.VideoChoicePanel.Controls.Add(this.VideoButton4);
            this.VideoChoicePanel.Controls.Add(this.VideoButton3);
            this.VideoChoicePanel.Controls.Add(this.VideoButton2);
            this.VideoChoicePanel.Controls.Add(this.VideoButton1);
            this.VideoChoicePanel.Enabled = false;
            this.VideoChoicePanel.Location = new System.Drawing.Point(32, 12);
            this.VideoChoicePanel.Name = "VideoChoicePanel";
            this.VideoChoicePanel.Size = new System.Drawing.Size(1024, 768);
            this.VideoChoicePanel.TabIndex = 6;
            this.VideoChoicePanel.Visible = false;
            // 
            // VideoButton4
            // 
            this.VideoButton4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.VideoButton4.Location = new System.Drawing.Point(498, 388);
            this.VideoButton4.Name = "VideoButton4";
            this.VideoButton4.Size = new System.Drawing.Size(523, 377);
            this.VideoButton4.TabIndex = 3;
            this.VideoButton4.UseVisualStyleBackColor = true;
            this.VideoButton4.Click += new System.EventHandler(this.VideoButton4_Click);
            // 
            // VideoButton3
            // 
            this.VideoButton3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.VideoButton3.Location = new System.Drawing.Point(3, 388);
            this.VideoButton3.Name = "VideoButton3";
            this.VideoButton3.Size = new System.Drawing.Size(489, 377);
            this.VideoButton3.TabIndex = 2;
            this.VideoButton3.UseVisualStyleBackColor = true;
            this.VideoButton3.Click += new System.EventHandler(this.button2_Click);
            // 
            // VideoButton2
            // 
            this.VideoButton2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.VideoButton2.Location = new System.Drawing.Point(498, -1);
            this.VideoButton2.Name = "VideoButton2";
            this.VideoButton2.Size = new System.Drawing.Size(526, 383);
            this.VideoButton2.TabIndex = 1;
            this.VideoButton2.UseVisualStyleBackColor = true;
            this.VideoButton2.Click += new System.EventHandler(this.VideoButton2_Click);
            // 
            // VideoButton1
            // 
            this.VideoButton1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.VideoButton1.Location = new System.Drawing.Point(0, 0);
            this.VideoButton1.Name = "VideoButton1";
            this.VideoButton1.Size = new System.Drawing.Size(492, 382);
            this.VideoButton1.TabIndex = 0;
            this.VideoButton1.UseVisualStyleBackColor = true;
            this.VideoButton1.Click += new System.EventHandler(this.VideoButton1_Click);
            // 
            // RightPanel
            // 
            this.RightPanel.Controls.Add(this.label1);
            this.RightPanel.Controls.Add(this.panel1);
            this.RightPanel.Controls.Add(this.FixedFpsValueBox);
            this.RightPanel.Controls.Add(this.PlayModeComboCox);
            this.RightPanel.Controls.Add(this.isFpsFixedBox);
            this.RightPanel.Location = new System.Drawing.Point(1065, 3);
            this.RightPanel.Name = "RightPanel";
            this.RightPanel.Size = new System.Drawing.Size(354, 777);
            this.RightPanel.TabIndex = 7;
            // 
            // ObjectDetectorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(59)))), ((int)(((byte)(72)))));
            this.ClientSize = new System.Drawing.Size(1431, 783);
            this.ControlBox = false;
            this.Controls.Add(this.RightPanel);
            this.Controls.Add(this.VideoChoicePanel);
            this.Controls.Add(this.pictureBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ObjectDetectorForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "ObjectDetectorForm";
            this.Load += new System.EventHandler(this.ObjectDetectorForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.FixedFpsValueBox)).EndInit();
            this.VideoChoicePanel.ResumeLayout(false);
            this.RightPanel.ResumeLayout(false);
            this.RightPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button StartButton;
        public System.Windows.Forms.Label label1;
        public System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.ComboBox PlayModeComboCox;
        public System.Windows.Forms.CheckBox isFpsFixedBox;
        public System.Windows.Forms.NumericUpDown FixedFpsValueBox;
        private System.Windows.Forms.Button FileDialogButton;
        private System.Windows.Forms.Panel VideoChoicePanel;
        private System.Windows.Forms.Button VideoButton1;
        private System.Windows.Forms.Button VideoButton3;
        private System.Windows.Forms.Button VideoButton2;
        private System.Windows.Forms.Button VideoButton4;
        private System.Windows.Forms.Panel RightPanel;
        private System.Windows.Forms.Button ScreenShotButton;
    }
}