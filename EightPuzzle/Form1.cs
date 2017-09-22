using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace EightPuzzle
{
	public partial class Form1 : Form
	{
		private int mapLength = 3, mapSize;
		private Size eachLatticeSize;
		Bitmap bit;
		private int[] factorial;

		//优先队列
		private class PriorityQueue<T> where T : IComparable<T>
		{
			private ArrayList minHeap;

			public PriorityQueue()
			{
				minHeap = new ArrayList();
			}

			public void Add(T node)
			{
				minHeap.Add(node);
				int x = minHeap.Count - 1, dad;
				object tmp = default(T);
				while (x > 0)
				{
					dad = (x - 1) >> 1;
					if (((T)minHeap[x]).CompareTo((T)minHeap[dad]) < 0)
					{
						tmp = minHeap[x];
						minHeap[x] = minHeap[dad];
						minHeap[dad] = tmp;
					}
					else
						break;
					x = dad;
				}
			}

			public bool Empty()
			{
				return minHeap.Count <= 0;
			}

			public T Pop()
			{
				T head = (T)minHeap[0];
				int x, son, x1 = 0;
				object tmp = default(T);
				minHeap[0] = minHeap[minHeap.Count - 1];
				minHeap.RemoveAt(minHeap.Count - 1);
				do
				{
					x = x1;
					son = (x << 1) + 1;
					if (son >= minHeap.Count) break;
					if (((T)minHeap[x]).CompareTo((T)minHeap[son]) > 0)
					{
						x1 = son;
						tmp = minHeap[x];
						minHeap[x] = minHeap[son];
						minHeap[son] = tmp;
					}
					++son;
					if (son >= minHeap.Count) break;
					if (((T)minHeap[x]).CompareTo((T)minHeap[son]) > 0)
					{
						x1 = son;
						tmp = minHeap[x];
						minHeap[x] = minHeap[son];
						minHeap[son] = tmp;
					}
				} while (x < x1);
				return head;
			}
		}

		//每个格子的编号，以及对应编号所在的格子
		//以3*3为例，目标状态为：
		//0 1 2
		//3 4 5
		//6 7 8
		//8为灰色格子编号

		private int[] num;
		private int grayBlockPosition;//灰色格子的位置
		private PictureBox[] picBox;
		private Point[] position;//每个格子的位置
		private bool isRecieveKeyboard;//是否接收键盘消息

		public Form1()
		{
			InitializeComponent();
			bit = Properties.Resources._2875;
			Reset();
			/*foreach (int t in num) Console.Write(t);
			Console.WriteLine();*/
		}

		//每个格子的初始属性设置
		public void SetAttr(int index, Image img, Point point)
		{
			((System.ComponentModel.ISupportInitialize)(picBox[index])).BeginInit();
			if (index != mapSize - 1)
				picBox[index].Image = img;
			else
				picBox[index].BackColor = Color.LightGray;
			picBox[index].Location = point;
			picBox[index].Name = "MyButton" + num.ToString();
			picBox[index].Size = new Size(eachLatticeSize.Width - 1, eachLatticeSize.Height - 1);
			picBox[index].TabStop = false;
			((System.ComponentModel.ISupportInitialize)(picBox[index])).EndInit();
		}

		//重新开始
		private void Replay(object sender, EventArgs e)
		{
			InitialPosition(num);
			for (int i = 0; i < mapSize; ++i)
			{
				if (num[i] == mapSize - 1) grayBlockPosition = i;
				picBox[num[i]].Location = position[i];
			}
		}

		//属性重置函数
		private void Reset()
		{
			Size tmpSize = ClientSize;
			tmpSize.Width -= 3;
			tmpSize.Height -= 3;
			eachLatticeSize = tmpSize;
			eachLatticeSize.Width /= mapLength;
			eachLatticeSize.Height /= mapLength;
			isRecieveKeyboard = true;
			bit = new Bitmap(bit, tmpSize);
			mapSize = mapLength * mapLength;
			num = new int[mapSize];
			position = new Point[mapSize];
			factorial = new int[mapSize + 1];
			picBox = new PictureBox[mapSize];
			for (int i = 0; i < mapSize; ++i)
			{
				position[i] = new Point(2 + (i % mapLength) * eachLatticeSize.Width, 2 + (i / mapLength) * eachLatticeSize.Height);
				picBox[i] = new PictureBox();
				factorial[i] = i == 0 ? 1 : i * factorial[i - 1];
			}
			factorial[mapSize] = mapSize * factorial[mapSize - 1];

			InitialPosition(num);
			for (int i = 0; i < mapSize; ++i)
			{
				if (num[i] == mapSize - 1) grayBlockPosition = i;
				Image img = bit.Clone(new Rectangle((num[i] % mapLength) * eachLatticeSize.Width, (num[i] / mapLength) * eachLatticeSize.Height, eachLatticeSize.Width, eachLatticeSize.Height), bit.PixelFormat);
				SetAttr(num[i], img, position[i]);
				Controls.Add(picBox[i]);
			}
			picBox[mapSize - 1].BringToFront();
		}

		private bool Feasibility(int[] num)
		{
			//树状数组求逆序数对
			int[] BIT = new int[mapSize];
			int sum = 0, i, x, cnt = 0;
			for(i = 0; i < mapSize; ++i)
				if(num[i] != mapSize - 1)
				{
					++cnt;
					x = num[i] + 1;
					sum += cnt;
					while(x > 0)
					{
						sum -= BIT[x];
						x -= x & -x;
					}
					x = num[i] + 1;
					while(x < mapSize)
					{
						++BIT[x];
						x += x & -x;
					}
				}
			return (sum & 1) == 0;
		}

		//随机位置初始化函数
		private void InitialPosition(int[] num)
		{
			Random ro = new Random();
			List<int> list = new List<int>();
			int i, index;
			while (true)
			{
				list.Clear();
				for (i = 0; i < mapSize; ++i) list.Add(i);
				for (i = 0; i < mapSize; ++i)
				{
					index = ro.Next(mapSize - i);
					num[i] = list[index];
					list.RemoveAt(index);
				}
				if (Feasibility(num)) break;
			}
		}

		private void MoveGrayBlock(object sender, KeyEventArgs e)
		{
			if (!isRecieveKeyboard) return;
			switch (e.KeyCode)
			{
				case Keys.W:
				case Keys.Up:
					if(grayBlockPosition > 2)
					{
						ChangePosition(grayBlockPosition, grayBlockPosition - 3);
						grayBlockPosition -= 3;
					}
					break;
				case Keys.A:
				case Keys.Left:
					if (grayBlockPosition % 3 > 0)
					{
						ChangePosition(grayBlockPosition, grayBlockPosition - 1);
						grayBlockPosition -= 1;
					}
					break;
				case Keys.S:
				case Keys.Down:
					if (grayBlockPosition < 6)
					{
						ChangePosition(grayBlockPosition, grayBlockPosition + 3);
						grayBlockPosition += 3;
					}
					break;
				case Keys.D:
				case Keys.Right:
					if (grayBlockPosition % 3 < 2)
					{
						ChangePosition(grayBlockPosition, grayBlockPosition + 1);
						grayBlockPosition += 1;
					}
					break;
				default:
					break;
			}
			if (WinCheck())
				MessageBox.Show("You Win");
		}

		private void ChangePosition(int index1, int index2)
		{
			Point tmp = picBox[num[index1]].Location;
			Invoke((EventHandler)(delegate
			{
				picBox[num[index1]].Location = picBox[num[index2]].Location;
				picBox[num[index2]].Location = tmp;
			}));
			Swap(ref num[index1], ref num[index2]);
		}

		//哈希函数-康拓展开
		private int Cantor(int[] num)
		{
			//同样用树状数组求逆序数
			int[] BIT = new int[mapSize + 1];
			int ans = 0, i, x, tmp;
			for (i = mapSize - 1; i >= 0; --i)
			{
				tmp = 0;
				x = num[i] + 1;
				while(x > 0)
				{
					tmp += BIT[x];
					x -= x & -x;
				}
				x = num[i] + 1;
				while(x < mapSize)
				{
					++BIT[x];
					x += x & -x;
				}
				ans += tmp * factorial[mapSize - i - 1];
			}
			return ans;
		}

		//康拓展开的逆运算
		private void CantorReverse(int hash, int[] rev)
		{
			bool[] vis = new bool[mapSize];
			for (int i = 0; i < mapSize; ++i)
			{
				int x = hash / factorial[mapSize - i - 1];
				for (int j = 0; j <= x; ++j)
					if (vis[j]) ++x;
				rev[i] = x;
				vis[x] = true;
				hash %= factorial[mapSize - i - 1];
			}
		}

		//任意两格子间的曼哈顿距离
		private int Distance(int x, int y)
		{
			return Math.Abs(x % 3 - y % 3) + Math.Abs(x / 3 - y / 3);
		}

		//当前状态到结果距离的估计函数（所有格子的曼哈顿距离之和）
		private int H(int[] rev)
		{
			int ans = 0;
			for (int i = 0; i < mapSize; ++i)
				if(rev[i] != mapSize - 1) ans += Distance(rev[i], i);
			return ans;
		}

		//A*算法中存储每个状态信息的结构体
		private class BlockStatus
		{
			//通过什么操作得到当前状态的(1->上 2->左 3->下 4->右)，由什么状态得到的
			public int direction, pre;
			public BlockStatus(int _direction, int _pre)
			{
				direction = _direction;
				pre = _pre;
			}
		}
		//节点类型，用于优先队列
		private class Node : IComparable<Node>
		{
			//分别表示A*算法中估价函数的值，当前状态值，搜索深度,灰格子的位置
			public int value, now, depth, grayBlockPosition;
			public Node(int _depth, int _now, int _value, int _grayBlockPosition)
			{
				value = _value;
				depth = _depth;
				now = _now;
				grayBlockPosition = _grayBlockPosition;
			}

			public int CompareTo(Node other)
			{
				return value > other.value ? 1 : (value == other.value ? 0 : -1);
			}
		}

		//A*算法
		private void A_Star()
		{
			List<int> ans = new List<int>();
			Dictionary<int, BlockStatus> hash = new Dictionary<int, BlockStatus>();
			PriorityQueue<Node> queue = new PriorityQueue<Node>();
			int[] rev = new int[mapSize];
			int now = Cantor(num), aim = Cantor(new int[9] { 0, 1, 2, 3, 4, 5, 6, 7, 8 }), nowDepth, nowGrayBlockPosition, tmpHash;
			hash.Add(now, new BlockStatus(-1, -1));
			queue.Add(new Node(0, now, H(num), grayBlockPosition));

			while (!queue.Empty() && !hash.ContainsKey(aim))
			{
				Node tmp = queue.Pop();
				now = tmp.now;
				nowDepth = tmp.depth;
				nowGrayBlockPosition = tmp.grayBlockPosition;
				CantorReverse(now, rev);
				//Console.WriteLine(rev.ToString());
				//上
				if(nowGrayBlockPosition > 2)
				{
					Swap(ref rev[nowGrayBlockPosition], ref rev[nowGrayBlockPosition - 3]);
					tmpHash = Cantor(rev);
					if (!hash.ContainsKey(tmpHash))
					{
						hash.Add(tmpHash, new BlockStatus(1, now));
						queue.Add(new Node(nowDepth + 1, tmpHash, nowDepth + 1 + H(rev), nowGrayBlockPosition - 3));
					}
					Swap(ref rev[nowGrayBlockPosition], ref rev[nowGrayBlockPosition - 3]);
				}
				//左
				if (nowGrayBlockPosition % 3 > 0)
				{
					Swap(ref rev[nowGrayBlockPosition], ref rev[nowGrayBlockPosition - 1]);
					tmpHash = Cantor(rev);
					if (!hash.ContainsKey(tmpHash))
					{
						hash.Add(tmpHash, new BlockStatus(2, now));
						queue.Add(new Node(nowDepth + 1, tmpHash, nowDepth + 1 + H(rev), nowGrayBlockPosition - 1));
					}
					Swap(ref rev[nowGrayBlockPosition], ref rev[nowGrayBlockPosition - 1]);
				}
				//下
				if (nowGrayBlockPosition < 6)
				{
					Swap(ref rev[nowGrayBlockPosition], ref rev[nowGrayBlockPosition + 3]);
					tmpHash = Cantor(rev);
					if (!hash.ContainsKey(tmpHash))
					{
						hash.Add(tmpHash, new BlockStatus(3, now));
						queue.Add(new Node(nowDepth + 1, tmpHash, nowDepth + 1 + H(rev), nowGrayBlockPosition + 3));
					}
					Swap(ref rev[nowGrayBlockPosition], ref rev[nowGrayBlockPosition + 3]);
				}
				//右
				if (nowGrayBlockPosition % 3 < 2)
				{
					Swap(ref rev[nowGrayBlockPosition], ref rev[nowGrayBlockPosition + 1]);
					tmpHash = Cantor(rev);
					if (!hash.ContainsKey(tmpHash))
					{
						hash.Add(tmpHash, new BlockStatus(4, now));
						queue.Add(new Node(nowDepth + 1, tmpHash, nowDepth + 1 + H(rev), nowGrayBlockPosition + 1));
					}
					Swap(ref rev[nowGrayBlockPosition], ref rev[nowGrayBlockPosition + 1]);
				}
			}

			while (hash.TryGetValue(aim, out BlockStatus tmpBlockStatus))
			{
				ans.Add(tmpBlockStatus.direction);
				aim = tmpBlockStatus.pre;
			}

			isRecieveKeyboard = false;
			num.CopyTo(rev, 0);
			do
			{
				rev.CopyTo(num, 0);
				for (int i = 0; i < mapSize; ++i)
					Invoke((EventHandler)(delegate
					{
						picBox[num[i]].Location = position[i];
					}));

				nowGrayBlockPosition = grayBlockPosition;
				for (int i = ans.Count - 2; i >= 0; --i)
				{
					switch (ans[i])
					{
						case 1:
							ChangePosition(nowGrayBlockPosition, nowGrayBlockPosition - 3);
							nowGrayBlockPosition -= 3;
							break;
						case 2:
							ChangePosition(nowGrayBlockPosition, nowGrayBlockPosition - 1);
							nowGrayBlockPosition -= 1;
							break;
						case 3:
							ChangePosition(nowGrayBlockPosition, nowGrayBlockPosition + 3);
							nowGrayBlockPosition += 3;
							break;
						case 4:
							ChangePosition(nowGrayBlockPosition, nowGrayBlockPosition + 1);
							nowGrayBlockPosition += 1;
							break;
						default:
							break;
					}
					Thread.Sleep(400);
				}
			} while (MessageBox.Show("是否重新演示？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes);

			rev.CopyTo(num, 0);
			for (int i = 0; i < mapSize; ++i)
				Invoke((EventHandler)(delegate
				{
					picBox[num[i]].Location = position[i];
				}));
			isRecieveKeyboard = true;
		}

		private void Swap<T>(ref T v1, ref T v2)
		{
			T tmp = v1;
			v1 = v2;
			v2 = tmp;
		}

		private void GetHint(object sender, EventArgs e)
		{
			//A_Star();
			Thread newThread = new Thread(A_Star);
			newThread.IsBackground = true;
			newThread.Start();
		}

		private bool WinCheck()
		{
			for (int i = 0; i < mapSize; ++i)
				if (num[i] != i) return false;
			return true;
		}
	}
}
