namespace TicTacToe
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
			this.label1 = new System.Windows.Forms.Label();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.label2 = new System.Windows.Forms.Label();
			this.button10 = new System.Windows.Forms.Button();
			this.radioButton1 = new System.Windows.Forms.RadioButton();
			this.radioButton2 = new System.Windows.Forms.RadioButton();
			this.radioButton3 = new System.Windows.Forms.RadioButton();
			this.button9 = new TicTacToe.UButton();
			this.button8 = new TicTacToe.UButton();
			this.button7 = new TicTacToe.UButton();
			this.button6 = new TicTacToe.UButton();
			this.button5 = new TicTacToe.UButton();
			this.button4 = new TicTacToe.UButton();
			this.button3 = new TicTacToe.UButton();
			this.button2 = new TicTacToe.UButton();
			this.button1 = new TicTacToe.UButton();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.label1.Location = new System.Drawing.Point(224, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(109, 20);
			this.label1.TabIndex = 9;
			this.label1.Text = "现在轮到：";
			// 
			// pictureBox1
			// 
			this.pictureBox1.Location = new System.Drawing.Point(252, 32);
			this.pictureBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(51, 50);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBox1.TabIndex = 10;
			this.pictureBox1.TabStop = false;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(237, 89);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(0, 15);
			this.label2.TabIndex = 11;
			// 
			// button10
			// 
			this.button10.Location = new System.Drawing.Point(228, 182);
			this.button10.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.button10.Name = "button10";
			this.button10.Size = new System.Drawing.Size(105, 31);
			this.button10.TabIndex = 12;
			this.button10.Text = "重新开始";
			this.button10.UseVisualStyleBackColor = true;
			this.button10.Click += new System.EventHandler(this.ReFresh);
			// 
			// radioButton1
			// 
			this.radioButton1.AutoSize = true;
			this.radioButton1.Checked = true;
			this.radioButton1.Location = new System.Drawing.Point(228, 113);
			this.radioButton1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.radioButton1.Name = "radioButton1";
			this.radioButton1.Size = new System.Drawing.Size(88, 19);
			this.radioButton1.TabIndex = 13;
			this.radioButton1.TabStop = true;
			this.radioButton1.Tag = 1;
			this.radioButton1.Text = "人人对战";
			this.radioButton1.UseVisualStyleBackColor = true;
			this.radioButton1.CheckedChanged += new System.EventHandler(this.playStatus);
			// 
			// radioButton2
			// 
			this.radioButton2.AutoSize = true;
			this.radioButton2.Location = new System.Drawing.Point(228, 135);
			this.radioButton2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.radioButton2.Name = "radioButton2";
			this.radioButton2.Size = new System.Drawing.Size(88, 19);
			this.radioButton2.TabIndex = 14;
			this.radioButton2.Tag = 2;
			this.radioButton2.Text = "电脑先手";
			this.radioButton2.UseVisualStyleBackColor = true;
			this.radioButton2.CheckedChanged += new System.EventHandler(this.playStatus);
			// 
			// radioButton3
			// 
			this.radioButton3.AutoSize = true;
			this.radioButton3.Location = new System.Drawing.Point(228, 157);
			this.radioButton3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.radioButton3.Name = "radioButton3";
			this.radioButton3.Size = new System.Drawing.Size(73, 19);
			this.radioButton3.TabIndex = 15;
			this.radioButton3.Tag = 3;
			this.radioButton3.Text = "人先手";
			this.radioButton3.UseVisualStyleBackColor = true;
			this.radioButton3.CheckedChanged += new System.EventHandler(this.playStatus);
			// 
			// button9
			// 
			this.button9.BackColor = System.Drawing.Color.Transparent;
			this.button9.Location = new System.Drawing.Point(143, 140);
			this.button9.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.button9.Name = "button9";
			this.button9.Size = new System.Drawing.Size(75, 75);
			this.button9.TabIndex = 6;
			this.button9.UseVisualStyleBackColor = false;
			this.button9.Click += new System.EventHandler(this.OnClick);
			// 
			// button8
			// 
			this.button8.BackColor = System.Drawing.Color.Transparent;
			this.button8.Location = new System.Drawing.Point(71, 140);
			this.button8.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.button8.Name = "button8";
			this.button8.Size = new System.Drawing.Size(75, 75);
			this.button8.TabIndex = 7;
			this.button8.UseVisualStyleBackColor = false;
			this.button8.Click += new System.EventHandler(this.OnClick);
			// 
			// button7
			// 
			this.button7.BackColor = System.Drawing.Color.Transparent;
			this.button7.Location = new System.Drawing.Point(0, 140);
			this.button7.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.button7.Name = "button7";
			this.button7.Size = new System.Drawing.Size(75, 75);
			this.button7.TabIndex = 8;
			this.button7.UseVisualStyleBackColor = false;
			this.button7.Click += new System.EventHandler(this.OnClick);
			// 
			// button6
			// 
			this.button6.BackColor = System.Drawing.Color.Transparent;
			this.button6.Location = new System.Drawing.Point(143, 71);
			this.button6.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.button6.Name = "button6";
			this.button6.Size = new System.Drawing.Size(75, 75);
			this.button6.TabIndex = 3;
			this.button6.UseVisualStyleBackColor = false;
			this.button6.Click += new System.EventHandler(this.OnClick);
			// 
			// button5
			// 
			this.button5.BackColor = System.Drawing.Color.Transparent;
			this.button5.Location = new System.Drawing.Point(71, 71);
			this.button5.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.button5.Name = "button5";
			this.button5.Size = new System.Drawing.Size(75, 75);
			this.button5.TabIndex = 4;
			this.button5.UseVisualStyleBackColor = false;
			this.button5.Click += new System.EventHandler(this.OnClick);
			// 
			// button4
			// 
			this.button4.BackColor = System.Drawing.Color.Transparent;
			this.button4.Location = new System.Drawing.Point(0, 71);
			this.button4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(75, 75);
			this.button4.TabIndex = 5;
			this.button4.UseVisualStyleBackColor = false;
			this.button4.Click += new System.EventHandler(this.OnClick);
			// 
			// button3
			// 
			this.button3.BackColor = System.Drawing.Color.Transparent;
			this.button3.Location = new System.Drawing.Point(143, 0);
			this.button3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(75, 75);
			this.button3.TabIndex = 2;
			this.button3.UseVisualStyleBackColor = false;
			this.button3.Click += new System.EventHandler(this.OnClick);
			// 
			// button2
			// 
			this.button2.BackColor = System.Drawing.Color.Transparent;
			this.button2.Location = new System.Drawing.Point(71, 0);
			this.button2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(75, 75);
			this.button2.TabIndex = 1;
			this.button2.UseVisualStyleBackColor = false;
			this.button2.Click += new System.EventHandler(this.OnClick);
			// 
			// button1
			// 
			this.button1.BackColor = System.Drawing.Color.Transparent;
			this.button1.Location = new System.Drawing.Point(0, 0);
			this.button1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 75);
			this.button1.TabIndex = 0;
			this.button1.UseVisualStyleBackColor = false;
			this.button1.Click += new System.EventHandler(this.OnClick);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(345, 212);
			this.Controls.Add(this.radioButton3);
			this.Controls.Add(this.radioButton2);
			this.Controls.Add(this.radioButton1);
			this.Controls.Add(this.button10);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.button9);
			this.Controls.Add(this.button8);
			this.Controls.Add(this.button7);
			this.Controls.Add(this.button6);
			this.Controls.Add(this.button5);
			this.Controls.Add(this.button4);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.Name = "Form1";
			this.Text = "Form1";
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private UButton button1;
        private UButton button2;
        private UButton button3;
        private UButton button4;
        private UButton button5;
        private UButton button6;
        private UButton button7;
        private UButton button8;
        private UButton button9;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton3;
    }
}

