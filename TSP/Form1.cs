using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Windows.Forms;

namespace TSP
{
    public partial class Form1 : Form
    {
        private Bitmap origin, aim;
        private String pointFileName = "att48.tsp", routeFileName = "att48.opt.tour";
        private int mapSize, picBoxWidth ,picBoxHeight;

        public Form1()
        {
            InitializeComponent();
            textBox1.Text = pointFileName;
            textBox2.Text = routeFileName;
            picBoxWidth = pictureBox1.Width << 1;
            picBoxHeight = pictureBox1.Height << 1;
        }

        private void Start(object sender, EventArgs e)
        {
            button1.Enabled = false;
            if (!File.Exists(textBox1.Text))
            {
                MessageBox.Show(textBox1.Text + " does not exist.");
                return;
            }
            if (!File.Exists(textBox2.Text))
            {
                MessageBox.Show(textBox2.Text + " does not exist.");
                return;
            }
            mapSize = 0;
            List<Point> pointList = new List<Point>();
            using (StreamReader srp = File.OpenText(textBox1.Text))
            {
                String Line;
                while((Line = srp.ReadLine()) != null)
                {
                    int x = 0, y = 0, tmp = 0, cnt = 0;
                    for(int i = 0; i < Line.Length; ++i)
                    {
                        if (Line[i] >= '0' && Line[i] <= '9')
                        {
                            tmp = tmp * 10 + Line[i] - '0';
                        }
                        else
                        if (cnt != 0 || (cnt == 0 && tmp == pointList.Count + 1))
                        {
                            ++cnt;
                            if (cnt == 2) x = tmp;
                            else
                                if (cnt == 3) y = tmp;
                            tmp = 0;
                        }
                        else tmp = 0;
                    }
                    if (tmp != 0) y = tmp;
                    if (x > mapSize) mapSize = x;
                    if (y > mapSize) mapSize = y;
                    pointList.Add(new Point(x, y));
                }
                srp.Close();
            }
            mapSize += 10;
            if(origin != null)origin.Dispose();
            if(aim != null)aim.Dispose();
            aim = new Bitmap(picBoxWidth, picBoxHeight);
            pictureBox2.Image = aim;

            Point[] point = pointList.ToArray();
            PointF[] pointf = new PointF[point.Length];
            float scaleRateX = mapSize / (float)picBoxWidth, scaleRateY = mapSize / (float)picBoxHeight;
            for (int i = 0; i < point.Length; ++i) {
                pointf[i].X = point[i].X / scaleRateX;
                pointf[i].Y = point[i].Y / scaleRateY;
            }
            int[] aimRoute = new int[pointList.Count];
            PointF[] aimPath = new PointF[pointList.Count];
            int count = 0;
            using (StreamReader srr = File.OpenText(textBox2.Text))
            {
                String Line;
                while((Line = srr.ReadLine()) != null)
                {
                    int tmp = 0;
                    for (int i = 0; i < Line.Length; ++i)
                        if (Line[i] >= '0' && Line[i] <= '9')
                            tmp = tmp * 10 + Line[i] - '0';
                    aimPath[count] = pointf[tmp - 1];
                    aimRoute[count++] = tmp;
                }
                srr.Close();
            }

            using (Graphics ga = Graphics.FromImage(aim))
            {
                using(GraphicsPath gpa = new GraphicsPath())
                {
                    gpa.AddPolygon(aimPath);
                    ga.DrawPath(new Pen(Color.Black) { Width = 2 }, gpa);
					pictureBox2.Invalidate();
                }
            }

            new Thread((ThreadStart)delegate
            {
                HillClimbing(point, pointf);
            }) { IsBackground = true }.Start();
        }

        void Randomization(ref PointF[] pointf, ref Point[] point)
        {
            Random ro = new Random();
            List<int> list = new List<int>();
            Point[] tmp = new Point[point.Length];
            PointF[] tmpf = new PointF[pointf.Length];
            int i, index;
            list.Clear();
            for (i = 0; i < point.Length; ++i) list.Add(i);
            for (i = 0; i < point.Length; ++i)
            {
                index = ro.Next(point.Length - i);
                tmp[i] = point[list[index]];
                tmpf[i] = pointf[list[index]];
                list.RemoveAt(index);
            }
            tmp.CopyTo(point, 0);
            tmpf.CopyTo(pointf, 0);
        }

        double GetDis(Point[] point)
        {
            double tmp = Math.Sqrt(Math.Pow(point[0].X - point[point.Length - 1].X, 2) + Math.Pow(point[0].Y - point[point.Length - 1].Y, 2));
            for(int i = 1; i < point.Length; ++i)
                tmp += Math.Sqrt(Math.Pow(point[i - 1].X - point[i].X, 2) + Math.Pow(point[i - 1].Y - point[i].Y, 2));
            return tmp;
        }

        void Swap<T>(ref T lsh, ref T rsh)
        {
            T tmp = lsh;
            lsh = rsh;
            rsh = tmp;
        }

        private const int loopTime = 400000;
        void HillClimbing(Point[] point, PointF[] pointf)
        {
			Thread.Sleep(10);
            Randomization(ref pointf, ref point);

			double now = GetDis(point), tmp;
            Random ro = new Random();
			bool isBlock = false;
            int x, y;

			for (int i = 0; i < loopTime; ++i)
            {
				do
				{
					x = ro.Next(point.Length);
					y = ro.Next(point.Length);
				} while (x == y);

				Swap(ref point[x], ref point[y]);
				Swap(ref pointf[x], ref pointf[y]);
				tmp = GetDis(point);
				if (tmp < now)
				{
					now = tmp;
					isBlock = true;
					new Thread((ThreadStart)delegate {
						try
						{
							Invoke((EventHandler)delegate {
								if (origin != null) origin.Dispose();
								origin = new Bitmap(picBoxWidth, picBoxHeight);
								using (Graphics go = Graphics.FromImage(origin))
								{
									using (GraphicsPath gpo = new GraphicsPath())
									{
										gpo.AddPolygon(pointf);
										go.DrawPath(new Pen(Color.Black) { Width = 2 }, gpo);
										pictureBox1.Image = origin;
									}
								}
							});
						}
						catch
						{
							return;
						}
						isBlock = false;
					}).Start();
				}
				else
				{
					Swap(ref point[x], ref point[y]);
					Swap(ref pointf[x], ref pointf[y]);
				}
				if (isBlock)
					Thread.Sleep(10);
            }
            Invoke((EventHandler)delegate
            {
                button1.Enabled = true;
            });
        }
    }
}
