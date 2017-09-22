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
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.提示ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.重新开始ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenuStrip1.SuspendLayout();
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
			this.contextMenuStrip1.Size = new System.Drawing.Size(176, 104);
			// 
			// 提示ToolStripMenuItem
			// 
			this.提示ToolStripMenuItem.Name = "提示ToolStripMenuItem";
			this.提示ToolStripMenuItem.Size = new System.Drawing.Size(175, 24);
			this.提示ToolStripMenuItem.Text = "提示";
			this.提示ToolStripMenuItem.Click += new System.EventHandler(this.GetHint);
			// 
			// 重新开始ToolStripMenuItem
			// 
			this.重新开始ToolStripMenuItem.Name = "重新开始ToolStripMenuItem";
			this.重新开始ToolStripMenuItem.Size = new System.Drawing.Size(175, 24);
			this.重新开始ToolStripMenuItem.Text = "重新开始";
			this.重新开始ToolStripMenuItem.Click += new System.EventHandler(this.Replay);
			// 
			// 设置ToolStripMenuItem
			// 
			this.设置ToolStripMenuItem.Name = "设置ToolStripMenuItem";
			this.设置ToolStripMenuItem.Size = new System.Drawing.Size(175, 24);
			this.设置ToolStripMenuItem.Text = "设置";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(360, 360);
			this.ContextMenuStrip = this.contextMenuStrip1;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.KeyPreview = true;
			this.MaximizeBox = false;
			this.Name = "Form1";
			this.Text = "EightPuzzle";
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MoveGrayBlock);
			this.contextMenuStrip1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private System.Windows.Forms.ToolStripMenuItem 提示ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 重新开始ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 设置ToolStripMenuItem;
	}
}

