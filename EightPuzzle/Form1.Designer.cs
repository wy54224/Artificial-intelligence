namespace EightPuzzle
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
			this.components = new System.ComponentModel.Container();
			System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
			System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
			System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
			System.Windows.Forms.DataVisualization.Charting.Title title3 = new System.Windows.Forms.DataVisualization.Charting.Title();
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.提示ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.重新开始ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.listBox1 = new System.Windows.Forms.ListBox();
			this.label1 = new System.Windows.Forms.Label();
			this.radioButton1 = new System.Windows.Forms.RadioButton();
			this.radioButton2 = new System.Windows.Forms.RadioButton();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
			this.button1 = new System.Windows.Forms.Button();
			this.contextMenuStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
			this.SuspendLayout();
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.提示ToolStripMenuItem,
            this.重新开始ToolStripMenuItem,
            this.设置ToolStripMenuItem});
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size(139, 76);
			// 
			// 提示ToolStripMenuItem
			// 
			this.提示ToolStripMenuItem.Name = "提示ToolStripMenuItem";
			this.提示ToolStripMenuItem.Size = new System.Drawing.Size(138, 24);
			this.提示ToolStripMenuItem.Text = "提示";
			this.提示ToolStripMenuItem.Click += new System.EventHandler(this.GetHint);
			// 
			// 重新开始ToolStripMenuItem
			// 
			this.重新开始ToolStripMenuItem.Name = "重新开始ToolStripMenuItem";
			this.重新开始ToolStripMenuItem.Size = new System.Drawing.Size(138, 24);
			this.重新开始ToolStripMenuItem.Text = "重新开始";
			this.重新开始ToolStripMenuItem.Click += new System.EventHandler(this.Replay);
			// 
			// 设置ToolStripMenuItem
			// 
			this.设置ToolStripMenuItem.Name = "设置ToolStripMenuItem";
			this.设置ToolStripMenuItem.Size = new System.Drawing.Size(138, 24);
			this.设置ToolStripMenuItem.Text = "设置";
			// 
			// listBox1
			// 
			this.listBox1.FormattingEnabled = true;
			this.listBox1.ItemHeight = 15;
			this.listBox1.Location = new System.Drawing.Point(333, 3);
			this.listBox1.Name = "listBox1";
			this.listBox1.Size = new System.Drawing.Size(174, 304);
			this.listBox1.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(333, 317);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(0, 15);
			this.label1.TabIndex = 2;
			// 
			// radioButton1
			// 
			this.radioButton1.AutoSize = true;
			this.radioButton1.Checked = true;
			this.radioButton1.Location = new System.Drawing.Point(510, 12);
			this.radioButton1.Name = "radioButton1";
			this.radioButton1.Size = new System.Drawing.Size(135, 19);
			this.radioButton1.TabIndex = 3;
			this.radioButton1.TabStop = true;
			this.radioButton1.Text = "H1(曼哈顿距离)";
			this.radioButton1.UseVisualStyleBackColor = true;
			// 
			// radioButton2
			// 
			this.radioButton2.AutoSize = true;
			this.radioButton2.Location = new System.Drawing.Point(510, 37);
			this.radioButton2.Name = "radioButton2";
			this.radioButton2.Size = new System.Drawing.Size(150, 19);
			this.radioButton2.TabIndex = 4;
			this.radioButton2.Text = "H2(不匹配格子数)";
			this.radioButton2.UseVisualStyleBackColor = true;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(333, 336);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(0, 15);
			this.label2.TabIndex = 5;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(513, 69);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(113, 15);
			this.label3.TabIndex = 6;
			this.label3.Text = "F是否满足单调?";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(513, 93);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(0, 15);
			this.label4.TabIndex = 7;
			// 
			// chart1
			// 
			chartArea3.AxisX.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
			chartArea3.AxisY.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
			chartArea3.Name = "ChartArea1";
			this.chart1.ChartAreas.Add(chartArea3);
			legend3.Enabled = false;
			legend3.Name = "Legend1";
			this.chart1.Legends.Add(legend3);
			this.chart1.Location = new System.Drawing.Point(510, 111);
			this.chart1.Name = "chart1";
			series3.ChartArea = "ChartArea1";
			series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
			series3.IsVisibleInLegend = false;
			series3.IsXValueIndexed = true;
			series3.Legend = "Legend1";
			series3.Name = "Series1";
			this.chart1.Series.Add(series3);
			this.chart1.Size = new System.Drawing.Size(150, 122);
			this.chart1.TabIndex = 8;
			title3.Name = "Title1";
			title3.Text = "F的变化情况";
			this.chart1.Titles.Add(title3);
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(510, 254);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(150, 34);
			this.button1.TabIndex = 9;
			this.button1.Text = "开启9数码模式";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.OnClick);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(660, 360);
			this.ContextMenuStrip = this.contextMenuStrip1;
			this.Controls.Add(this.button1);
			this.Controls.Add(this.chart1);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.radioButton2);
			this.Controls.Add(this.radioButton1);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.listBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.KeyPreview = true;
			this.MaximizeBox = false;
			this.Name = "Form1";
			this.Text = "EightPuzzle";
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MoveGrayBlock);
			this.contextMenuStrip1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private System.Windows.Forms.ToolStripMenuItem 提示ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 重新开始ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 设置ToolStripMenuItem;
		private System.Windows.Forms.ListBox listBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.RadioButton radioButton1;
		private System.Windows.Forms.RadioButton radioButton2;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
		private System.Windows.Forms.Button button1;
	}
}

