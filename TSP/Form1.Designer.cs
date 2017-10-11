namespace TSP
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.pictureBox2 = new System.Windows.Forms.PictureBox();
			this.button1 = new System.Windows.Forms.Button();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.textBox2 = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.radioButton1 = new System.Windows.Forms.RadioButton();
			this.radioButton2 = new System.Windows.Forms.RadioButton();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
			this.SuspendLayout();
			// 
			// pictureBox1
			// 
			this.pictureBox1.Location = new System.Drawing.Point(2, 2);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(498, 498);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			// 
			// pictureBox2
			// 
			this.pictureBox2.Location = new System.Drawing.Point(681, 2);
			this.pictureBox2.Name = "pictureBox2";
			this.pictureBox2.Size = new System.Drawing.Size(498, 498);
			this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBox2.TabIndex = 1;
			this.pictureBox2.TabStop = false;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(523, 132);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(131, 36);
			this.button1.TabIndex = 2;
			this.button1.Text = "Start";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.Start);
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(523, 197);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(131, 25);
			this.textBox1.TabIndex = 3;
			// 
			// textBox2
			// 
			this.textBox2.Location = new System.Drawing.Point(523, 246);
			this.textBox2.Name = "textBox2";
			this.textBox2.Size = new System.Drawing.Size(131, 25);
			this.textBox2.TabIndex = 4;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(554, 314);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(0, 15);
			this.label1.TabIndex = 5;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(554, 356);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(0, 15);
			this.label2.TabIndex = 6;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(554, 398);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(0, 15);
			this.label3.TabIndex = 7;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(525, 314);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(23, 15);
			this.label4.TabIndex = 8;
			this.label4.Text = "T=";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(509, 356);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(39, 15);
			this.label5.TabIndex = 9;
			this.label5.Text = "Aim=";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(509, 398);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(39, 15);
			this.label6.TabIndex = 10;
			this.label6.Text = "Now=";
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(509, 431);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(0, 15);
			this.label8.TabIndex = 11;
			// 
			// radioButton1
			// 
			this.radioButton1.AutoSize = true;
			this.radioButton1.Checked = true;
			this.radioButton1.Location = new System.Drawing.Point(523, 59);
			this.radioButton1.Name = "radioButton1";
			this.radioButton1.Size = new System.Drawing.Size(73, 19);
			this.radioButton1.TabIndex = 12;
			this.radioButton1.TabStop = true;
			this.radioButton1.Text = "爬山法";
			this.radioButton1.UseVisualStyleBackColor = true;
			// 
			// radioButton2
			// 
			this.radioButton2.AutoSize = true;
			this.radioButton2.Location = new System.Drawing.Point(523, 84);
			this.radioButton2.Name = "radioButton2";
			this.radioButton2.Size = new System.Drawing.Size(88, 19);
			this.radioButton2.TabIndex = 13;
			this.radioButton2.TabStop = true;
			this.radioButton2.Text = "模拟退火";
			this.radioButton2.UseVisualStyleBackColor = true;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1182, 503);
			this.Controls.Add(this.radioButton2);
			this.Controls.Add(this.radioButton1);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.textBox2);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.pictureBox2);
			this.Controls.Add(this.pictureBox1);
			this.DoubleBuffered = true;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "Form1";
			this.Opacity = 0.92D;
			this.Text = "TSP";
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.RadioButton radioButton1;
		private System.Windows.Forms.RadioButton radioButton2;
	}
}

