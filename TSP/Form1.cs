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
		//初始输入文件名
        private String pointFileName = "eil101.tsp", routeFileName = "eil101.opt.tour";
        private int mapSize, picBoxWidth ,picBoxHeight;
		//opt数组分别存放3种操作的使用次数（使用次数越大越容易被选择来用，初始值为optInitialValue）
		private const int optInitialValue = 500;
		private int[] opt = new int[3];
		//全局Random变量（解决new Random()过快生成数据不随机的问题----Random()默认构造函数用时间当seed）
		Random ro = new Random();
		//开始时间
		DateTime start;

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
			start = DateTime.Now;
            button1.Enabled = false;
			label1.Text = "";
			label2.Text = "";
			label3.Text = "";
			label8.Text = "";
			mapSize = 0;
			for (int i = 0; i < opt.Length; ++i) opt[i] = optInitialValue;
			//文件存在性检测
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

			//读入文件并存储得到的数据到pointList中
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
			//图像文件的创建
            if(origin != null)origin.Dispose();
            if(aim != null)aim.Dispose();
            aim = new Bitmap(picBoxWidth, picBoxHeight);
            pictureBox2.Image = aim;

			//将前面得到的list转化为数组，pointf为实际绘图的坐标点（和原坐标存在等比例关系）
            Point[] point = pointList.ToArray();
            PointF[] pointf = new PointF[point.Length];
			//mapSize 地图坐标的最大值+10， picBoxWidth和picBoxHeight是Bitmap图像的宽高
			float scaleRateX = mapSize / (float)picBoxWidth, scaleRateY = mapSize / (float)picBoxHeight;
            for (int i = 0; i < point.Length; ++i) {
                pointf[i].X = point[i].X / scaleRateX;
                pointf[i].Y = point[i].Y / scaleRateY;
            }
			//存放给出的TSP最优解样例
            int[] aimRoute = new int[pointList.Count];
            PointF[] aimPathf = new PointF[pointList.Count];
			Point[] aimPath = new Point[pointList.Count];
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
                    aimPathf[count] = pointf[tmp - 1];
					aimPath[count] = point[tmp - 1];
                    aimRoute[count++] = tmp;
                }
                srr.Close();
            }

			label2.Text = GetDis(aimPath).ToString();

			//将最优解画出来
            using (Graphics ga = Graphics.FromImage(aim))
            {
                using(GraphicsPath gpa = new GraphicsPath())
                {
                    gpa.AddPolygon(aimPathf);
                    ga.DrawPath(new Pen(Color.Black) { Width = 2 }, gpa);
					pictureBox2.Invalidate();
                }
            }

			//创建新线程执行解TSP的算法
            new Thread((ThreadStart)delegate
            {
				if(radioButton1.Checked)
					HillClimbing(point, pointf);
				else
					SA(point, pointf);
				Invoke((EventHandler)delegate
				{
					button1.Enabled = true;
					TimeSpan tim = DateTime.Now - start;
					label8.Text = "Run time=" + tim.TotalSeconds + "s";
				});
			}) { IsBackground = true }.Start();
        }

		//点位置初始化的函数（随机连线）
		void Randomization(ref PointF[] pointf, ref Point[] point)
        {
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

		//获取当前遍历路径长
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

		//状态转移函数，包含3种操作
		int GetNext(Point[] point, PointF[] pointf)
		{
			int x, y;
			do
			{
				x = ro.Next(point.Length);
				y = ro.Next(point.Length);
			} while (x == y);

			double ranRate = ro.Next(0) / (double)System.Int32.MaxValue, sum = opt[0] + opt[1] + opt[2];
			//操作1：将区间[x, y]的点翻转
			if (ranRate < opt[0] / sum)
			{
				if (x > y) Swap(ref x, ref y);
				while (x < y)
				{
					Swap(ref point[x], ref point[y]);
					Swap(ref pointf[x], ref pointf[y]);
					++x;
					--y;
				}
				return 0;
			}
			//操作2：交换两点
			else
			if (ranRate < (opt[0] + opt[1]) / sum)
			{
				Swap(ref point[x], ref point[y]);
				Swap(ref pointf[x], ref pointf[y]);
				return 1;
			}
			//操作3：将一点放到另一点的后面
			else
			{
				if (x > y) Swap(ref x, ref y);
				++x;
				while(x < y)
				{
					Swap(ref point[x], ref point[y]);
					Swap(ref pointf[x], ref pointf[y]);
					++x;
				}
				return 2;
			}
		}

		//爬山法
        void HillClimbing(Point[] point, PointF[] pointf)
        {
            Randomization(ref pointf, ref point);

			double now = GetDis(point), tmp;
            Random ro = new Random();
			bool isBlock = false;
			int x, y, l1 = 0, whichOpt;
			Point[] tmpPoint = new Point[point.Length];
			PointF[] tmpPointf = new PointF[pointf.Length];

			while(true)
            {
				point.CopyTo(tmpPoint, 0);
				pointf.CopyTo(tmpPointf, 0);
				whichOpt = GetNext(point, pointf);
				
				tmp = GetDis(point);
				if (tmp < now)
				{
					++opt[whichOpt];
					now = tmp;
					//防止主线程绘图卡死
					while (isBlock)
						Thread.Sleep(1);
					isBlock = true;
					new Thread((ThreadStart)delegate {
						//在运行过程中直接关闭Form1时Invoke仍会调用（background线程也没用）
						//此时会抛出“无法访问已释放的对象”的异常（Form1对象Dispose了）
						//一种不完全的解决方法，用try catch忽略掉错误
						try
						{
							Invoke((EventHandler)delegate {
								label3.Text = now.ToString();
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
					tmpPoint.CopyTo(point, 0);
					tmpPointf.CopyTo(pointf, 0);
					++l1;
				}
				if (l1 > 100000)
					break;
            }
		}

		//模拟退火（Simulated annealing algorithm）
		//T 初始温度，delta降温系数，SALoop算法markov链长
		const double T = 1.2, delta = 0.99;
		const int SALoop = 15000;
		//Limit为每个温度下运行最大的不降温结点数
		//BLimit为允许的最大连续超出Limit的温度数量
		const int Limit = SALoop / 5;
		const int BLimit = SALoop / 15;
		void SA(Point[] point, PointF[] pointf)
		{
			Randomization(ref pointf, ref point);

			double now = GetDis(point), tmp, t = T, de, best;
			PointF[] bestPathf = new PointF[pointf.Length];
			Point[] bestPath = new Point[point.Length];
			Point[] tmpPoint = new Point[point.Length];
			PointF[] tmpPointf = new PointF[pointf.Length];
			bool isBlock = false, isChange;
			int l1 = 0, l2 = 0, whichOpt;
			best = now;

			while(true)
			{
				//显示当前温度
				try{
					Invoke((EventHandler)delegate
					{
						label1.Text = t.ToString();
					});
				}
				catch{
					return;
				}
				for(int i = 0; i < SALoop; ++i)
				{
					point.CopyTo(tmpPoint, 0);
					pointf.CopyTo(tmpPointf, 0);

					whichOpt = GetNext(point, pointf);

					tmp = GetDis(point);
					de = now - tmp;
					isChange = false;
					if (de > 0)
					{
						++opt[whichOpt];
						now = tmp;
						isChange = true;
						l1 = 0;
						l2 = 0;
						if (tmp < best)
						{
							best = tmp;
							pointf.CopyTo(bestPathf, 0);
							point.CopyTo(bestPath, 0);
						}
					}
					else
					{
						if (Math.Exp(de / t) > ro.Next() / (double)System.Int32.MaxValue)
						{
							now = tmp;
							isChange = true;
						}
						else
						{
							tmpPoint.CopyTo(point, 0);
							tmpPointf.CopyTo(pointf, 0);
						}
						++l1;
					}
					if (isChange)
					{
						while (isBlock)
							Thread.Sleep(1);
						isBlock = true;
						//这里可以改进：不用每次创建一个线程而是用一个线程每次唤醒它
						//创建线程的原因：不用卡在Invoke而是继续执行
						new Thread((ThreadStart)delegate {
							try
							{
								Invoke((EventHandler)delegate {
									label3.Text = now.ToString();
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
					if(l1 > Limit)
					{
						++l2;
						break;
					}
				}
				if (l2 > BLimit)
					break;
				//自己加的东西：每次在该温度下结束后用当前最优值覆盖当前值
				bestPath.CopyTo(point, 0);
				bestPathf.CopyTo(pointf, 0);
				t *= delta;
			}

			//最后再把最优值输出
			while (isBlock)
				Thread.Sleep(1);
			isBlock = true;
			new Thread((ThreadStart)delegate {
				try
				{
					Invoke((EventHandler)delegate {
						label3.Text = best.ToString();
						if (origin != null) origin.Dispose();
						origin = new Bitmap(picBoxWidth, picBoxHeight);
						using (Graphics go = Graphics.FromImage(origin))
						{
							using (GraphicsPath gpo = new GraphicsPath())
							{
								gpo.AddPolygon(bestPathf);
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
    }
}
