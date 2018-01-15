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
        public static Bitmap origin, aim;
		private double aimValue;
		//初始输入文件名
        private String pointFileName = "eil101.tsp", routeFileName = "eil101.opt.tour";
        public static int mapSize, picBoxWidth ,picBoxHeight;
		public static int geneLenght = 101;
		//opt数组分别存放3种操作的使用次数（使用次数越大越容易被选择来用，初始值为optInitialValue）
		private const int optInitialValue = 500;
		private int[] opt = new int[3];
		//全局Random变量（解决new Random()过快生成数据不随机的问题----Random()默认构造函数用时间当seed）
		public static Random ro = new Random();
		private float scaleRateX, scaleRateY;
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
			//mapSize 地图坐标的最大值+10， picBoxWidth和picBoxHeight是Bitmap图像的宽高
			scaleRateX = mapSize / (float)picBoxWidth;
			scaleRateY = mapSize / (float)picBoxHeight;
            //geneLenght = pointList.Count;
			//存放给出的TSP最优解样例
            int[] aimRoute = new int[pointList.Count];
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
					aimPath[count] = point[tmp - 1];
                    aimRoute[count++] = tmp;
                }
                srr.Close();
            }

			aimValue = GetDis(aimPath);
			label2.Text = aimValue.ToString();

			//将最优解画出来
            using (Graphics ga = Graphics.FromImage(aim))
            {
                using(GraphicsPath gpa = new GraphicsPath())
                {

					GPA_AddPolygon(gpa, aimPath);
                    ga.DrawPath(new Pen(Color.Black) { Width = 2 }, gpa);
					pictureBox2.Invalidate();
                }
            }

			//创建新线程执行解TSP的算法
            new Thread((ThreadStart)delegate
            {
				if (radioButton1.Checked)
				{
					Invoke((EventHandler)delegate
					{
						label4.Text = "G = ";
					});
					GeneticAlgorithm(point);
				}
				else
				{
					Invoke((EventHandler)delegate
					{
						label4.Text = "T = ";
					});
					SA(point);
				}
				Invoke((EventHandler)delegate
				{
					button1.Enabled = true;
					TimeSpan tim = DateTime.Now - start;
					label8.Text = "Run time=" + tim.TotalSeconds + "s";
				});
			}) { IsBackground = true }.Start();
        }

		private void GPA_AddPolygon(GraphicsPath gpa, Point[] path)
		{
			PointF[] pathf = new PointF[path.Length];
			for(int i = 0; i < path.Length; ++i)
			{
				pathf[i].X = path[i].X / scaleRateX;
				pathf[i].Y = path[i].Y / scaleRateY;
			}
			gpa.AddPolygon(pathf);
		}

		//点位置初始化的函数（随机连线）
		public static void Randomization(ref Point[] point)
        {
			List<int> list = new List<int>();
			Point[] tmp = new Point[point.Length];
			int i, index;
			list.Clear();
			for (i = 0; i < point.Length; ++i) list.Add(i);
			for (i = 0; i < point.Length; ++i)
			{
				index = ro.Next(point.Length - i);
				tmp[i] = point[list[index]];
				list.RemoveAt(index);
			}
			tmp.CopyTo(point, 0);
		}

		//获取当前遍历路径长
        public static double GetDis(Point[] point)
        {
            double tmp = Math.Sqrt(Math.Pow(point[0].X - point[point.Length - 1].X, 2) + Math.Pow(point[0].Y - point[point.Length - 1].Y, 2));
            for(int i = 1; i < point.Length; ++i)
                tmp += Math.Sqrt(Math.Pow(point[i - 1].X - point[i].X, 2) + Math.Pow(point[i - 1].Y - point[i].Y, 2));
            return tmp;
        }

        public static void Swap<T>(ref T lsh, ref T rsh)
        {
            T tmp = lsh;
            lsh = rsh;
            rsh = tmp;
        }

		//状态转移函数，包含3种操作
		int GetNext(Point[] point)
		{
			int x, y;
			do
			{
				x = ro.Next(point.Length);
				y = ro.Next(point.Length);
			} while (x == y);

			double ranRate = ro.Next() / (double)System.Int32.MaxValue, sum = opt[0] + opt[1] + opt[2];
			//操作1：将区间[x, y]的点翻转
			if (ranRate < opt[0] / sum)
			{
				if (x > y) Swap(ref x, ref y);
				while (x < y)
				{
					Swap(ref point[x], ref point[y]);
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
					++x;
				}
				return 2;
			}
		}

        //Life类记录基因的序列和得分
        class Life
        {
			public int[] gene;
            public double score;
            public Life()
            {
                gene = new int[geneLenght];
                score = 0;
            }
            public void Deepcopy(Life oldGene)
            {
                score = oldGene.score;
				oldGene.gene.CopyTo(gene, 0);
            }
        }

        class GA
        {
			//Npop为每一代的个数，live为种群，pbest为最优个体
			//geneLenght基因长度（城市点数）
			//generation代数
			//IsEnd是否已经收敛
			//TerminalTiems判断收敛的迭代数
			//Times最优个体没有更新的代数
			const int Npop = 100;
			const int Nch = 10;
			const int TerminalTimes = 1500 / Nch;
			public int generation = 0, Times = 0;
			public Life[] lives = new Life[Npop];
			public bool IsEnd;
            Life pbest;
			public Point[] pPoint;
			//每个点距离它最近的10个点
			struct DisPair
			{
				public int To;
				public double Dis;
				public DisPair(int _To, double _Dis)
				{
					To = _To;
					Dis = _Dis;
				}
			}
			List<DisPair>[] Nearest;
            //初始化
            public void Init(Point[] point)
            {
                geneLenght = point.Length;
				pPoint = point;
				IsEnd = true;
				pbest = new Life();
				Nearest = new List<DisPair>[geneLenght];
				for (int i = 0; i < geneLenght; ++i)
					Nearest[i] = new List<DisPair>();
				for (int i = 0; i < geneLenght; ++i)
					for(int j = i + 1; j < geneLenght; ++j)
					{
						double Dis = Math.Sqrt(Math.Pow(pPoint[i].X - pPoint[j].X, 2) + Math.Pow(pPoint[i].Y - pPoint[j].Y, 2));
						Nearest[i].Add(new DisPair(j, Dis));
						Nearest[j].Add(new DisPair(i, Dis));
					}
				for (int i = 0; i < geneLenght; ++i)
				{
					Nearest[i].Sort((x, y) => {
						return x.Dis == y.Dis ? 0 : ((x.Dis > y.Dis) ? 1 : -1);
					});
					Nearest[i] = new List<DisPair>(Nearest[i].GetRange(0, 10));
				}
				double minDis = Double.MaxValue;
                for (int i = 0; i < Npop; i++)
                {
					lives[i] = new Life();
					RandomSequence(ref lives[i].gene);
					double tmpDis = Two_Opt(ref lives[i].gene);
					lives[i].score = tmpDis;
					if(tmpDis < minDis)
					{
						minDis = tmpDis;
						pbest.Deepcopy(lives[i]);
					}
                }
            }

			//获取距离
			public double Local_GetDis(int[] seq)
			{
				double tmp = Math.Sqrt(Math.Pow(pPoint[seq[0]].X - pPoint[seq[seq.Length - 1]].X, 2) + Math.Pow(pPoint[seq[0]].Y - pPoint[seq[seq.Length - 1]].Y, 2));
				for (int i = 1; i < seq.Length; ++i)
					tmp += Math.Sqrt(Math.Pow(pPoint[seq[i - 1]].X - pPoint[seq[i]].X, 2) + Math.Pow(pPoint[seq[i - 1]].Y - pPoint[seq[i]].Y, 2));
				return tmp;
			}

			public Point[] GetPoint(int[] seq)
			{
				Point[] tmp = new Point[seq.Length];
				for (int i = 0; i < seq.Length; ++i)
					tmp[i] = pPoint[seq[i]];
				return tmp;
			}

			//2-Opt局部贪心
			public double Two_Opt(ref int[] seq)
			{
				for (int i = 0; i < geneLenght - 4; i++)
					for (int j = i + 2; j < geneLenght - 2; j++)
						//若交叉，交换x2 x3，并把中间的顺序颠倒
						if (IsCross(pPoint[seq[i]], pPoint[seq[i + 1]], pPoint[seq[j]], pPoint[seq[j + 1]]))
						{
							int p, q;
							for (p = i + 1, q = j; p < q; p++, q--)
								Swap(ref seq[p], ref seq[q]);
						}
				return Local_GetDis(seq);
			}

			struct Edge
			{
				public int To;
				public char Type;
				public Edge(int _To, char _Type)
				{
					To = _To;
					Type = _Type;
				}
			}

			private void DelMap(List<Edge>[]map, int x, int y, char type)
			{
				for(int i = 0; i < map[x].Count; ++i)
					if(map[x][i].To == y && map[x][i].Type == type)
					{
						map[x].RemoveAt(i);
						break;
					}
				for(int i = 0; i < map[y].Count; ++i)
					if(map[y][i].To == x && map[y][i].Type == type)
					{
						map[y].RemoveAt(i);
						break;
					}
			}

			//找出所有的effective AB回路（不存在重边的）
			public List<List<int>> SearchEffectiveABcycles(Life Pa, Life Pb)
			{
				int N = geneLenght;
				List<List<int>> tmp = new List<List<int>>();
				List<Edge>[] map = new List<Edge>[N];
				//stk+stkLength = 一个简单的栈
				int[] stk = new int[geneLenght];
				int stkLength = 0;
				//bj数组，判断有没走过以及从这个点到下一个点是什么类型的边（A or B ?）
				char[] bj = new char[N];
				//上1一条边的类型（寻找AB cycle需要每条相邻的边类型不同）
				char LastType;
				//生成邻接表，A表示属于PA的边，B表示属于PB的边
				for (int i = 0; i < N; ++i)
					map[i] = new List<Edge>();
				for(int i = 0; i < N; ++i)
				{
					int j = (i + 1) % N;
					map[Pa.gene[i]].Add(new Edge(Pa.gene[j], 'A'));
					map[Pa.gene[j]].Add(new Edge(Pa.gene[i], 'A'));
					map[Pb.gene[i]].Add(new Edge(Pb.gene[j], 'B'));
					map[Pb.gene[j]].Add(new Edge(Pb.gene[i], 'B'));
				}
				
				//用循环实现不回溯DFS搜索
				for (int i = 0; i < N; ++i)
					if(map[i].Count > 0)
					{
						stk[stkLength++] = i;
						do
						{
							LastType = stkLength < 2 ? '\0' : bj[stk[stkLength - 2]];
							int Top = stk[stkLength - 1];
							for (int j = 0; j < map[Top].Count; ++j)
								if (map[Top][j].Type != LastType && (stkLength < 2 || (stkLength >= 2 && stk[stkLength - 2] != map[Top][j].To)))
								{
									//如果没走过就尽量走啦
									if (bj[map[Top][j].To] != 'A' && bj[map[Top][j].To] != 'B')
									{
										bj[Top] = map[Top][j].Type;
										LastType = bj[Top];
										stk[stkLength++] = map[Top][j].To;
										DelMap(map, Top, map[Top][j].To, map[Top][j].Type);
										break;
									}
									else
									//走过的话就判断一下是不是AB cycle，是就把它弄出来
									if(bj[map[Top][j].To] != map[Top][j].Type)
									{
										int Block = map[Top][j].To;
										tmp.Add(new List<int>());
										int l = tmp.Count - 1;
										while (stk[stkLength - 1] != Block)
										{
											bj[stk[--stkLength]] = '\0';
											tmp[l].Add(stk[stkLength]);
										}
										bj[stk[--stkLength]] = '\0';
										tmp[l].Add(stk[stkLength]);
										tmp[l].Add(LastType == 'A' ? -1 : -2);
										DelMap(map, Top, map[Top][j].To, map[Top][j].Type);
										break;
									}
								}
							if (stkLength == 0)
								break;
							//走到死路了，当前节点无法扩展，弹出栈
							if(Top == stk[stkLength - 1])
								bj[stk[--stkLength]] = '\0';
						} while (stkLength > 0);
						if (tmp.Count >= Nch)
							break;
					}
				return tmp;
			}

			//搜出所有subtours
			void DFS(int index, ref List<int>[] map, ref bool[] bj, ref List<int>tmpList)
			{
				tmpList.Add(index);
				for(int i = 0; i < map[index].Count; ++i)
					if (!bj[map[index][i]])
					{
						bj[map[index][i]] = true;
						DFS(map[index][i], ref map, ref bj, ref tmpList);
					}
			}

			//算两点间距离
			double CalDis(Point x, Point y)
			{
				return Math.Sqrt(Math.Pow(x.X - y.X, 2) + Math.Pow(x.Y - y.Y, 2));
			}

			//叉积
			int Cross(Point A, Point B)
			{
				return A.X * B.Y - A.Y * B.X;
			}

			//叉积判断交叉
			bool IsCross(Point A, Point B, Point C, Point D)
			{
				Point AB = new Point(B.X - A.X, B.Y - A.Y);
				Point AC = new Point(C.X - A.X, C.Y - A.Y);
				Point AD = new Point(D.X - A.X, D.Y - A.Y);
				Point CD = new Point(D.X - C.X, D.Y - C.Y);
				Point CA = new Point(A.X - C.X, A.Y - C.Y);
				Point CB = new Point(B.X - C.X, B.X - C.Y);
				return (Cross(AB, AC) * Cross(AB, AD) <= 0) && (Cross(CD, CA) * Cross(CD, CB) <= 0);
			}

			//怎么合并subtours
			//这里可以改进（手搓一个堆来优化排序）
			void Merge(ref List<List<int>> subTours)
			{
				Point[] Loc = new Point[geneLenght];
				while (subTours.Count > 1)
				{
					subTours.Sort((x, y) => {
						return (x.Count == y.Count) ? 0 : ((x.Count > y.Count) ? 1 : -1);
					});
					for (int i = 0; i < subTours.Count; ++i)
						for (int j = 0; j < subTours[i].Count; ++j)
							Loc[subTours[i][j]] = new Point(i, j);
					double Min = Double.MinValue;
					Point a1 = new Point(1, 0), a2= new Point(1, 1), b1 = new Point(0, 0);
					for (int i = 0; i < subTours[0].Count; ++i)
						for(int j = 0; j < Nearest[subTours[0][i]].Count; ++j)
						{
							Point b = Loc[Nearest[subTours[0][i]][j].To];
							int x1 = subTours[0][i], x2 = subTours[0][(i + 1) % subTours[0].Count];
							int y1 = subTours[b.X][b.Y], y2 = subTours[b.X][(b.Y + 1) % subTours[b.X].Count];
							//删两条边再增加两条边
							double tmp = -CalDis(pPoint[x1], pPoint[x2]) - CalDis(pPoint[y1], pPoint[y2]);
							//判断交叉
							if (IsCross(pPoint[x1], pPoint[y1], pPoint[x2], pPoint[y2]))
								Swap(ref y1, ref y2);
							tmp += CalDis(pPoint[x1], pPoint[y1]) + CalDis(pPoint[x2], pPoint[y2]);
							if(tmp < Min)
							{
								Min = tmp;
								a1 = Loc[y1];
								a2 = Loc[y2];
								b1 = new Point(0, i);
							}
						}
					//合并
					List<int> tmplist = new List<int>();
					for (int i = 0; i <= b1.Y; ++i) tmplist.Add(subTours[0][i]);
					int step = a1.Y < a2.Y ? -1 : 1;
					for (int i = a1.Y; i != a2.Y; i = (i + subTours[a1.X].Count + step) % subTours[a1.X].Count)
						tmplist.Add(subTours[a1.X][i]);
					tmplist.Add(subTours[a1.X][a2.Y]);
					for (int i = b1.Y + 1; i < subTours[0].Count; ++i) tmplist.Add(subTours[0][i]);
					//删去旧的 增加新的
					subTours.RemoveAt(0);
					subTours[0] = tmplist;
				}
			}

			//产生新后代
			public Life NewChild(Life Pa, Life Pb, int ChildNum, List<List<int>>ABC)
            {
				//局部EAX
				//ABC是前面AB cycle搜索算法搜出来的结果
				Life tmp = new Life();
				int N = Pa.gene.Length;
				List<int>[] map = new List<int>[Pa.gene.Length];
				for (int i = 0; i < N; ++i)
					map[i] = new List<int>();
				for(int i = 0; i < N; ++i)
				{
					int j = (i + 1) % N;
					map[Pa.gene[i]].Add(Pa.gene[j]);
					map[Pa.gene[j]].Add(Pa.gene[i]);
				}
				//前面结果的存储中会多存一位这个cycle开始是什么类型的边
				bool bj = ABC[ChildNum][ABC[ChildNum].Count - 1] == -1 ? true : false;
				for (int i = 0; i < ABC[ChildNum].Count - 1; ++i)
				{
					int j = (i + 1) % (ABC[ChildNum].Count - 1);
					if (bj)
					{
						//A类型的边，从PA中删去
						for (int k = 0; k < map[ABC[ChildNum][i]].Count; ++k)
							if (map[ABC[ChildNum][i]][k] == ABC[ChildNum][j])
								map[ABC[ChildNum][i]].RemoveAt(k);
						for (int k = 0; k < map[ABC[ChildNum][j]].Count; ++k)
							if (map[ABC[ChildNum][j]][k] == ABC[ChildNum][i])
								map[ABC[ChildNum][j]].RemoveAt(k);
					}
					else
					{
						//B类型的边，增加到PA中
						map[ABC[ChildNum][i]].Add(ABC[ChildNum][j]);
						map[ABC[ChildNum][j]].Add(ABC[ChildNum][i]);
					}
					bj = !bj;
				}
				//找出当前中间解的subtours
				bool[] hash = new bool[N];
				List<List<int>> subTours = new List<List<int>>();
				for (int i = 0; i < N; ++i)
					if (!hash[i])
					{
						hash[i] = true;
						List<int> tmpList = new List<int>();
						DFS(i, ref map, ref hash, ref tmpList);
						subTours.Add(tmpList);
					}
				//合并subtours变成tour
				Merge(ref subTours);
				tmp.gene = subTours[0].ToArray();
				tmp.score = Local_GetDis(tmp.gene);
				return tmp;
            }

			//生成随机序列
			public void RandomSequence(ref int[] seq)
			{
				List<int> list = new List<int>();
				int i, index;
				for (i = 0; i < seq.Length; ++i) list.Add(i);
				for (i = 0; i < seq.Length; ++i)
				{
					index = ro.Next(seq.Length - i);
					seq[i] = list[index];
					list.RemoveAt(index);
				}
			}

			//EAX获取下一代
			int[] seq = new int[Npop];
			public void Next()
            {
				++generation;
				++Times;
				//随机一个顺序
				RandomSequence(ref seq);
				for(int i = 0; i < seq.Length; ++i)
				{
					Life Pa = new Life();
					Pa.Deepcopy(lives[seq[i]]);
					Life Pb = lives[seq[(i + 1) % seq.Length]];
					List<List<int>> tmp = SearchEffectiveABcycles(Pa, Pb);
					for (int j = 0; j < Nch && j < tmp.Count; ++j)
					{
						Life child = NewChild(lives[seq[i]], Pb, j, tmp);
						if (child.score < Pa.score)
							Swap(ref Pa, ref child);
					}
					lives[seq[i]] = Pa;
					if (lives[seq[i]].score < pbest.score)
					{
						pbest.Deepcopy(lives[seq[i]]);
						Times = 0;
					}
				}
				if(Times > TerminalTimes)
					IsEnd = false;
            }

            public Life Get_pbest()
            {
				return pbest;
            }

        }

        //遗传算法
        void GeneticAlgorithm(Point[] point)
        {
            GA ga = new GA();
            ga.Init(point);
            bool isBlock = false;
			while (ga.IsEnd)
			{
				ga.Next();
				Life pbest = ga.Get_pbest();
				//Console.WriteLine(pbest.gene.Length);
				double now = pbest.score;
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
							//label9.Text = ga.generation.ToString();
							label9.Text = ((pbest.score - aimValue) / aimValue * 100).ToString("f2") + "%";
							label1.Text = ga.generation.ToString();
							if (origin != null) origin.Dispose();
                            
                            origin = new Bitmap(picBoxWidth, picBoxHeight);
                            using (Graphics go = Graphics.FromImage(origin))
                            {

                                using (GraphicsPath gpo = new GraphicsPath())
                                {
                                    GPA_AddPolygon(gpo, ga.GetPoint(pbest.gene));
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

        //模拟退火（Simulated annealing algorithm）
        //T 初始温度，delta降温系数，SALoop算法markov链长
        const double T = 1.2, delta = 0.99;
		const int SALoop = 15000;
		//Limit为每个温度下运行最大的不降温结点数
		//BLimit为允许的最大连续超出Limit的温度数量
		const int Limit = SALoop / 5;
		const int BLimit = SALoop / 15;
		void SA(Point[] point)
		{
			Randomization(ref point);

			double now = GetDis(point), tmp, t = T, de, best;
			Point[] bestPath = new Point[point.Length];
			Point[] tmpPoint = new Point[point.Length];
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

					whichOpt = GetNext(point);

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
									label9.Text = ((now - aimValue) / aimValue * 100).ToString("f2") + "%";
									if (origin != null) origin.Dispose();
									origin = new Bitmap(picBoxWidth, picBoxHeight);
									using (Graphics go = Graphics.FromImage(origin))
									{
										using (GraphicsPath gpa = new GraphicsPath())
										{
											GPA_AddPolygon(gpa, point);
											go.DrawPath(new Pen(Color.Black) { Width = 2 }, gpa);
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
						label9.Text = ((best - aimValue) / aimValue * 100).ToString("f2") + "%";
						if (origin != null) origin.Dispose();
						origin = new Bitmap(picBoxWidth, picBoxHeight);
						using (Graphics go = Graphics.FromImage(origin))
						{
							using (GraphicsPath gpo = new GraphicsPath())
							{
								GPA_AddPolygon(gpo, bestPath);
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
