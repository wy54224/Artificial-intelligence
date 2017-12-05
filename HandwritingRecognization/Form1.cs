using System;
using System.Drawing;
using System.Windows.Forms;

namespace HandwritingRecognization
{
	public partial class Form1 : Form
	{
		private Bitmap WritingCanvas;
		private bool isClick;
		private Point lastPoint;
		private Graphics go;
		private Pen DrawPen;
		public Form1()
		{
			InitializeComponent();
			//将绘图区长和宽设置为16的倍数，方便计算
			WritingArea.Width = WritingArea.Width / 32 * 32;
			WritingArea.Height = WritingArea.Height / 32 * 32;
			WritingCanvas = new Bitmap(32, 32);
			WritingArea.Image = WritingCanvas;
			isClick = false;
			lastPoint = new Point();
			go = Graphics.FromImage(WritingCanvas);
			go.Clear(Color.White);
			DrawPen = new Pen(Color.Black);
		}

		private void OnMouseDown(object sender, MouseEventArgs e)
		{
			isClick = true;
			lastPoint.X = e.X * 32 / WritingArea.Width;
			lastPoint.Y = e.Y * 32 / WritingArea.Height;
		}

		private void OnMouseMove(object sender, MouseEventArgs e)
		{
			if (isClick && e.X < WritingArea.Width && e.Y < WritingArea.Height && e.X >= 0 && e.Y >= 0)
			{
				Point newPoint = new Point(e.X * 32 / WritingArea.Width, e.Y * 32 / WritingArea.Height);
				go.DrawLine(DrawPen, lastPoint, newPoint);
				WritingArea.Invalidate();
				lastPoint = newPoint;
			}
		}

		private void OnMouseUp(object sender, MouseEventArgs e)
		{
			isClick = false;
			go.DrawLine(DrawPen, lastPoint, new Point(e.X * 32 / WritingArea.Width, e.Y * 32 / WritingArea.Height));
			WritingArea.Invalidate();
		}

		private void OnClear(object sender, EventArgs e)
		{
			go.Clear(Color.White);
			WritingArea.Invalidate();
		}

		private int GetAns(ref double[] x)
		{
			return 0;
		}

		private void Recognization(object sender, EventArgs e)
		{
			double []x;
			x = new double[1024];
			for (int i = 0; i < 32; ++i)
				for (int j = 0; j < 32; ++j)
				{
					Color tmp = WritingCanvas.GetPixel(i, j);
					x[i * 32 + j] = (tmp.R > 128 &&  tmp.G > 128 &&  tmp.B > 128) ? 0 : 1;
				}
			int result = GetAns(ref x);
			for (int i = 0; i < 32; ++i)
				for (int j = 0; j < 32; ++j)
					WritingCanvas.SetPixel(i, j, x[i * 32 + j] > 0.5 ? Color.Green : Color.White);
			WritingArea.Invalidate();
		}
	}
}
