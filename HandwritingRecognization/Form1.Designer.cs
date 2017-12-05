namespace HandwritingRecognization
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
			this.WritingArea = new System.Windows.Forms.PictureBox();
			this.Check = new System.Windows.Forms.Button();
			this.Clear = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.WritingArea)).BeginInit();
			this.SuspendLayout();
			// 
			// WritingArea
			// 
			this.WritingArea.Location = new System.Drawing.Point(12, 12);
			this.WritingArea.Name = "WritingArea";
			this.WritingArea.Size = new System.Drawing.Size(340, 340);
			this.WritingArea.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.WritingArea.TabIndex = 0;
			this.WritingArea.TabStop = false;
			this.WritingArea.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseDown);
			this.WritingArea.MouseMove += new System.Windows.Forms.MouseEventHandler(this.OnMouseMove);
			this.WritingArea.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnMouseUp);
			// 
			// Check
			// 
			this.Check.Location = new System.Drawing.Point(394, 40);
			this.Check.Name = "Check";
			this.Check.Size = new System.Drawing.Size(107, 43);
			this.Check.TabIndex = 1;
			this.Check.Text = "识别";
			this.Check.UseVisualStyleBackColor = true;
			this.Check.Click += new System.EventHandler(this.Recognization);
			// 
			// Clear
			// 
			this.Clear.Location = new System.Drawing.Point(394, 100);
			this.Clear.Name = "Clear";
			this.Clear.Size = new System.Drawing.Size(107, 43);
			this.Clear.TabIndex = 2;
			this.Clear.Text = "清空";
			this.Clear.UseVisualStyleBackColor = true;
			this.Clear.Click += new System.EventHandler(this.OnClear);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(542, 373);
			this.Controls.Add(this.Clear);
			this.Controls.Add(this.Check);
			this.Controls.Add(this.WritingArea);
			this.Name = "Form1";
			this.Text = "HandwritingRecognization";
			((System.ComponentModel.ISupportInitialize)(this.WritingArea)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PictureBox WritingArea;
		private System.Windows.Forms.Button Check;
		private System.Windows.Forms.Button Clear;
	}
}

