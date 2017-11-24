using System.Collections.Generic;
using System;
using pair = System.Collections.Generic.KeyValuePair<int, int>;

namespace ChineseChess
{
	partial class Controller
	{
		//象棋一共14种
		//This的棋子
		//1-車 2-马 3-象 4-士 5-将 6-炮 7-卒 0-空
		//That的棋子
		//8-車 9-马 10-象 11-士 12-帅 13-炮 14-卒
		public enum CType{
			None,
			ThisRook, ThisKnight, ThisElephant, ThisMandarin, ThisKing, ThisCannon, ThisPawn,
			ThatRook, ThatKnight, ThatElephant, ThatMandarin, ThatKing, ThatCannon, ThatPawn
		};

		public CType ChessType2CType(ChessType type)
		{
			if (ChessType.Rook1 <= type && ChessType.King >= type)
				return (CType)((int)type + (int)CType.ThisRook);
			if (ChessType.Mandarin2 <= type && ChessType.Rook2 >= type)
				return (CType)(8 - (int)type + (int)CType.ThisRook);
			if (ChessType.Cannon1 == type || ChessType.Cannon2 == type)
				return CType.ThisCannon;
			return CType.ThisPawn;
		}
		//車和炮使用一种单独的着法生成器(预置+位棋盘)，其他棋子使用预置着法生成器
		//象棋棋盘大小9*10
		//0   1   2   3   4   5   6   7   8
		//9   10  11  12  13  14  15  16  17
		//18  19  20  21  22  23  24  25  26
		//27  28  29  30  31  32  33  34  35
		//36  37  38  39  40  41  42  43  44
		//45  46  47  48  49  50  51  52  53
		//54  55  56  57  58  59  60  61  62
		//63  64  65  66  67  68  69  70  71
		//72  73  74  75  76  77  78  79  80
		//81  82  83  84  85  86  87  88  89

		//人工智障
		//注意——人工智障是that，人是this
		private class AI
		{
			//预置着法生成器
			//存储下一步的走法以及马脚和象眼(其他棋子置0，为多余的存储空间)
			public List<pair>[,] chessSet;
			//车炮预置着法生成器
			public int[,,] DirectMove;
			public int[,,] CannonFly;

			//价值评估
			//为了减少每次计算黑棋反转的代价存储的时候就存一个翻转数组up side down
			private int[][] value = {
				//車
				//0
				new int[90]{206,208,207,213,214,213,207,208,206,
							206,212,209,216,233,216,209,212,206,
							206,208,207,214,216,214,207,208,206,
							206,213,213,216,216,216,213,213,206,
							208,211,211,214,215,214,211,211,208,
							208,212,212,214,215,214,212,212,208,
							204,209,204,212,214,212,204,209,204,
							198,208,204,212,212,212,204,208,198,
							200,208,206,212,200,212,206,208,200,
							194,206,204,212,200,212,204,206,194},
				//1
				new int[90]{194,206,204,212,200,212,204,206,194,
							200,208,206,212,200,212,206,208,200,
							198,208,204,212,212,212,204,208,198,
							204,209,204,212,214,212,204,209,204,
							208,212,212,214,215,214,212,212,208,
							208,211,211,214,215,214,211,211,208,
							206,213,213,216,216,216,213,213,206,
							206,208,207,214,216,214,207,208,206,
							206,212,209,216,233,216,209,212,206,
							206,208,207,213,214,213,207,208,206},
				//马
				//2
				new int[90]{90, 90, 90, 96, 90, 96, 90, 90, 90,
							90, 96,103, 97, 94, 97,103, 96, 90,
							92, 98, 99,103, 99,103, 99, 98, 92,
							93,108,100,107,100,107,100,108, 93,
							90,100, 99,103,104,103, 99,100, 90,
							90, 98,101,102,103,102,101, 98, 90,
							92, 94, 98, 95, 98, 95, 98, 94, 92,
							93, 92, 94, 95, 92, 95, 94, 92, 93,
							85, 90, 92, 93, 78, 93, 92, 90, 85,
							88, 85, 90, 88, 90, 88, 90, 85, 88},
				//3
				new int[90]{88, 85, 90, 88, 90, 88, 90, 85, 88,
							85, 90, 92, 93, 78, 93, 92, 90, 85,
							93, 92, 94, 95, 92, 95, 94, 92, 93,
							92, 94, 98, 95, 98, 95, 98, 94, 92,
							90, 98,101,102,103,102,101, 98, 90,
							90,100, 99,103,104,103, 99,100, 90,
							93,108,100,107,100,107,100,108, 93,
							92, 98, 99,103, 99,103, 99, 98, 92,
							90, 96,103, 97, 94, 97,103, 96, 90,
							90, 90, 90, 96, 90, 96, 90, 90, 90},
				//象 & 士
				//4
				new int[90]{ 0,  0, 20, 20,  0, 20, 20,  0,  0,
							 0,  0,  0,  0, 23,  0,  0,  0,  0,
							18,  0,  0, 20, 23, 20,  0,  0, 18,
							 0,  0,  0,  0,  0,  0,  0,  0,  0,
							 0,  0, 20,  0,  0,  0, 20,  0,  0,
							 0,  0, 20,  0,  0,  0, 20,  0,  0,
							 0,  0,  0,  0,  0,  0,  0,  0,  0,
							18,  0,  0, 20, 23, 20,  0,  0, 18,
							 0,  0,  0,  0, 23,  0,  0,  0,  0,
							 0,  0, 20, 20,  0, 20, 20,  0,  0},
				//5
				new int[90]{ 0,  0, 20, 20,  0, 20, 20,  0,  0,
							 0,  0,  0,  0, 23,  0,  0,  0,  0,
							18,  0,  0, 20, 23, 20,  0,  0, 18,
							 0,  0,  0,  0,  0,  0,  0,  0,  0,
							 0,  0, 20,  0,  0,  0, 20,  0,  0,
							 0,  0, 20,  0,  0,  0, 20,  0,  0,
							 0,  0,  0,  0,  0,  0,  0,  0,  0,
							18,  0,  0, 20, 23, 20,  0,  0, 18,
							 0,  0,  0,  0, 23,  0,  0,  0,  0,
							 0,  0, 20, 20,  0, 20, 20,  0,  0},
				//卒 & 帅
				//6
				new int[90]{ 9,  9,  9, 11, 13, 11,  9,  9,  9,
							19, 24, 34, 42, 44, 42, 34, 24, 19,
							19, 24, 32, 37, 37, 37, 32, 24, 19,
							19, 23, 27, 29, 30, 29, 27, 23, 19,
							14, 18, 20, 27, 29, 27, 20, 18, 14,
							 7,  0, 13,  0, 16,  0, 13,  0,  7,
							 7,  0,  7,  0, 15,  0,  7,  0,  7,
							 0,  0,  0,  10001,  10001,  10001,  0,  0,  0,
							 0,  0,  0,  10002,  10002,  10002,  0,  0,  0,
							 0,  0,  0, 10011, 10015, 10011,  0,  0,  0},
				//7
				new int[90]{ 0,  0,  0, 10011, 10015, 10011,  0,  0,  0,
							 0,  0,  0,  10002,  10002,  10002,  0,  0,  0,
							 0,  0,  0,  10001,  10001,  10001,  0,  0,  0,
							 7,  0,  7,  0, 15,  0,  7,  0,  7,
							 7,  0, 13,  0, 16,  0, 13,  0,  7,
							14, 18, 20, 27, 29, 27, 20, 18, 14,
							19, 23, 27, 29, 30, 29, 27, 23, 19,
							19, 24, 32, 37, 37, 37, 32, 24, 19,
							19, 24, 34, 42, 44, 42, 34, 24, 19,
							 9,  9,  9, 11, 13, 11,  9,  9,  9},
				//炮
				//8
				new int[90]{100,100, 96, 91, 90, 91, 96,100,100,
							 98, 98, 96, 92, 89, 92, 96, 98, 98, 
							 97, 97, 96, 91, 92, 91, 96, 97, 97,
							 96, 99, 99, 98,100, 98, 99, 99, 96,
							 96, 96, 96, 96,100, 96, 96, 96, 96,
							 95, 96, 99, 96,100, 96, 99, 96, 95,
							 96, 96, 96, 96, 96, 96, 96, 96, 96,
							 97, 96,100, 99,101, 99,100, 96, 97,
							 96, 97, 98, 98, 98, 98, 98, 97, 96,
							 96, 96, 97, 99, 99, 99, 97, 96, 96},
				//9
				new int[90]{ 96, 96, 97, 99, 99, 99, 97, 96, 96,
							 96, 97, 98, 98, 98, 98, 98, 97, 96,
							 97, 96,100, 99,101, 99,100, 96, 97,
							 96, 96, 96, 96, 96, 96, 96, 96, 96,
							 95, 96, 99, 96,100, 96, 99, 96, 95,
							 96, 96, 96, 96,100, 96, 96, 96, 96,
							 96, 99, 99, 98,100, 98, 99, 99, 96,
							 97, 97, 96, 91, 92, 91, 96, 97, 97,
							 98, 98, 96, 92, 89, 92, 96, 98, 98,
							100,100, 96, 91, 90, 91, 96,100,100},
			};

			//行棋盘和列棋盘
			int[] Row, Col;
			//全局棋盘
			CType[,] map;
			List<int>[] pos;
			//己方棋局估价和对方棋局估价
			int ThisValue, ThatValue;

			public AI()
			{
				int x, y;
				//生成预置着法生成器
				//预置着法生成器的构造
				//卒和将单独放一个棋盘，但是要分红棋和黑棋来放
				//士和象放一个棋盘
				//马的走法单独放一个棋盘
				//总共需要4个棋盘
				chessSet = new List<pair>[90, 4];
				//象、士和马的走法
				int[][] dx = {
					//象
					new int[4]{ 2, -2, -2, 2},
					//士
					new int[4]{ 1, -1, -1, 1},
					//马
					new int[8] { 2, 1, 1, 2, -2, -1, -1, -2 }
				};
				int[][] dy = {
					//象
					new int[4]{ 2, 2, -2, -2},
					//士
					new int[4]{ 1, 1, -1, -1},
					//马
					new int[8] { 1, -2, 2, -1, 1, 2, -2, -1 }
				};
				for(int i = 0; i < 90; ++i)
				{
					x = i % 9;
					y = i / 9;
					//卒 & 将
					chessSet[i, 0] = new List<pair>();
					//兵
					if(y < 7)
					{
						if(y < 5)
						{
							if (y > 0)
								chessSet[i, 0].Add(new pair(i - 9, 0));
							if (x > 0)
								chessSet[i, 0].Add(new pair(i - 1, 0));
							if (x < 8)
								chessSet[i, 0].Add(new pair(i + 1, 0));
						}
						else
						{
							if ((x & 1) == 0)
								chessSet[i, 0].Add(new pair(i - 9, 0));
						}
					}
					//将
					else
					{
						if (x >= 3 && x <= 5)
						{
							if (x > 3)
								chessSet[i, 0].Add(new pair(i - 1, 0));
							if (x < 5)
								chessSet[i, 0].Add(new pair(i + 1, 0));
							if (y > 7)
								chessSet[i, 0].Add(new pair(i - 9, 0));
							if (y < 9)
								chessSet[i, 0].Add(new pair(i + 9, 0));
						}
					}
					chessSet[i, 1] = new List<pair>();
					//卒
					if(y > 2)
					{
						if(y > 4)
						{
							if (y < 9)
								chessSet[i, 1].Add(new pair(i + 9, 0));
							if (x > 0)
								chessSet[i, 1].Add(new pair(i - 1, 0));
							if (x < 8)
								chessSet[i, 1].Add(new pair(i + 1, 0));
						}
						else
						{
							if ((x & 1) == 0)
								chessSet[i, 1].Add(new pair(i + 9, 0));
						}
					}
					//帅
					else
					{
						if (x >= 3 && x <= 5)
						{
							if (x > 3)
								chessSet[i, 1].Add(new pair(i - 1, 0));
							if (x < 5)
								chessSet[i, 1].Add(new pair(i + 1, 0));
							if (y > 0)
								chessSet[i, 1].Add(new pair(i - 9, 0));
							if (y < 2)
								chessSet[i, 1].Add(new pair(i + 9, 0));
						}
					}
					//象 & 士
					chessSet[i, 2] = new List<pair>();
					//象
					if(((x == 2 || x == 6) && (y == 0 || y == 4 || y == 5 || y == 9)) 
						|| ((y == 2 || y == 7) && (x == 0 || x == 4 || x == 8)))
					{
						for(int j = 0; j < 4; ++j)
						{
							int xx = x + dx[0][j];
							int yy = y + dy[0][j];
							if (xx >= 0 && xx <= 8 && yy >= 0 && yy <= 9 && ((y <= 4 && yy <= 4) || (y >= 5 && yy >= 5)))
							{
								//象眼
								int px = x + dx[0][j] / 2;
								int py = y + dy[0][j] / 2;
								chessSet[i, 2].Add(new pair(xx + yy * 9, px + py * 9));
							}
						}
					}
					//士
					if((x == 3 || x == 5) && (y == 0 || y == 2 || y == 7 || y == 9)
						|| (x == 4 && (y == 1 || y == 8)))
					{
						for(int j = 0; j < 4; ++j)
						{
							int xx = x + dx[1][j];
							int yy = y + dy[1][j];
							if (xx >= 3 && xx <= 5 && ((yy >= 0 && yy <= 2) || (yy >= 7 && yy <= 9)))
								chessSet[i, 2].Add(new pair(xx + yy * 9, 0));
						}
					}
					//马
					chessSet[i, 3] = new List<pair>();
					for (int j = 0; j < 8; ++j)
					{
						int xx = x + dx[2][j];
						int yy = y + dy[2][j];
						if(xx >= 0 && xx <= 8 && yy >= 0 && yy <= 9)
						{
							//马脚
							int px = Math.Abs(dx[2][j]) == 2 ? x + dx[2][j] / 2 : x;
							int py = Math.Abs(dy[2][j]) == 2 ? y + dy[2][j] / 2 : y;
							chessSet[i, 3].Add(new pair(xx + yy * 9, px + py * 9));
						}
					}
				}
				//位棋盘——棋盘状态压缩
				//10行9列
				//每行9个元素共2^9种状态，每列10个元素2^10种状态
				//DirectMove[i, j, 0]DirectMove[i, j, 1]分别表示状态为i的行(列)，车|炮位置为j，可移动的左右闭区间
				DirectMove = new int[1024, 10, 2];
				//CannonFly则表示向左和向右飞炮能吃的子
				CannonFly = new int[1024, 10, 2];
				for (int i = 0; i < 1024; ++i)
				{
					int lastJ = -1, beforeLastJ = -1;
					for (int j = 0; j < 10; ++j)
						if (((i >> j) & 1) == 1)
						{
							DirectMove[i, j, 0] = lastJ + 1;
							if (lastJ >= 0) DirectMove[i, lastJ, 1] = j - 1;
							CannonFly[i, j, 0] = beforeLastJ;
							if (beforeLastJ >= 0) CannonFly[i, beforeLastJ, 1] = j;
							beforeLastJ = lastJ;
							lastJ = j;
						}
						else
							DirectMove[i, j, 0] = DirectMove[i, j, 1] = CannonFly[i, j, 0] = CannonFly[i, j, 1] = -1;
					if (beforeLastJ >= 0) CannonFly[i, beforeLastJ, 1] = -1;
					if (lastJ >= 0)
					{
						DirectMove[i, lastJ, 1] = 9;
						CannonFly[i, lastJ, 1] = -1;
					}
				}

				//AI初始化
				//棋局价值只要取一样就行了，因为在局面判断是取它们的差
				ThisValue = ThatValue = 0;
				//初始的行、列位棋盘
				Row = new int[10];
				Row[0] = Row[9] = 511;
				Row[1] = Row[8] = Row[4] = Row[5] = 0;
				Row[2] = Row[7] = 130;
				Row[3] = Row[6] = 341;
				Col = new int[9];
				Col[0] = Col[8] = Col[2] = Col[6] = Col[4] = 585;
				Col[1] = Col[7] = 645;
				Col[3] = Col[5] = 513;

				//初始棋盘
				map = new CType[10, 9];
				map[0, 0] = map[0, 8] = CType.ThatRook;
				map[0, 1] = map[0, 7] = CType.ThatKnight;
				map[0, 2] = map[0, 6] = CType.ThatElephant;
				map[0, 3] = map[0, 5] = CType.ThatMandarin;
				map[0, 4] = CType.ThatKing;
				map[2, 1] = map[2, 7] = CType.ThatCannon;
				map[3, 0] = map[3, 2] = map[3, 4] = map[3, 6] = map[3, 8] = CType.ThatPawn;

				map[9, 0] = map[9, 8] = CType.ThisRook;
				map[9, 1] = map[9, 7] = CType.ThisKnight;
				map[9, 2] = map[9, 6] = CType.ThisElephant;
				map[9, 3] = map[9, 5] = CType.ThisMandarin;
				map[9, 4] = CType.ThisKing;
				map[7, 1] = map[7, 7] = CType.ThisCannon;
				map[6, 0] = map[6, 2] = map[6, 4] = map[6, 6] = map[6, 8] = CType.ThisPawn;

				//初始棋子位置
				pos = new List<int>[15];
				for (int i = 1; i < 15; ++i) pos[i] = new List<int>();
				for (int i = 0; i < 10; ++i)
					for (int j = 0; j < 9; ++j)
						if (map[i, j] != CType.None)
							pos[(int)map[i, j]].Add(i * 9 + j);
			}
			
			//获取对应位置棋子的估值
			private int ValueCheck(int position, CType type)
			{
				int AddOne = type <= CType.ThisPawn ? 0 : 1;
				int ValueType = ((int)type - 1) % 7;
				if (ValueType >= 3) --ValueType;
				if (ValueType >= 5) ValueType = 3;
				return value[(ValueType << 1) + AddOne][position];
			}

			//增加棋子到棋盘上
			public void AddPiece(int position, CType type)
			{
				if (type == CType.None) return;
				int x = position / 9, y = position % 9;
				if (map[x, y] != CType.None) map[20, 20] = CType.None;
				map[x, y] = type;
				Row[x] += 1 << y;
				Col[y] += 1 << x;
				pos[(int)type].Add(position);
				if (type <= CType.ThisPawn)
					ThisValue += ValueCheck(position, type);
				else
					ThatValue += ValueCheck(position, type);
			}

			//从棋盘上删除棋子
			public CType DelPiece(int position)
			{
				int x = position / 9, y = position % 9;
				CType type = map[x, y];
				if (type == CType.None) return type;
				map[x, y] = CType.None;
				Row[x] -= 1 << y;
				Col[y] -= 1 << x;
				pos[(int)type].Remove(position);
				if (type <= CType.ThisPawn)
					ThisValue -= ValueCheck(position, type);
				else
					ThatValue -= ValueCheck(position, type);
				return type;
			}

			//获取结果的max搜索
			public pair MaxSearch()
			{
				int vi = Int32.MinValue;
				pair ans = new pair(-1, -1);
				//初始是从that方开始的
				List<pair> moves = GetMoves(false);
				for(int i = 0; i < moves.Count; ++i)
				{
					int cur = moves[i].Key, aim = moves[i].Value;

					CType attack = DelPiece(cur);
					CType defense = DelPiece(aim);
					AddPiece(aim, attack);
					if(pos[(int)CType.ThisKing].Count == 1 && pos[(int)CType.ThatKing].Count == 1)
					{
						int thisKingCol = pos[(int)CType.ThisKing][0] % 9;
						int thatKingCol = pos[(int)CType.ThatKing][0] % 9;
						if (!(thisKingCol == thatKingCol && Col[thisKingCol] == 513))
						{
							int tmp = -AlphaBeta(vi, 4, true);
							if (tmp > vi)
							{
								vi = tmp;
								ans = moves[i];
							}
						}
					}else
					{
						int tmp = -AlphaBeta(vi, 4, true);
						if (tmp > vi)
						{
							vi = tmp;
							ans = moves[i];
						}
					}
					DelPiece(aim);
					AddPiece(aim, defense);
					AddPiece(cur, attack);
					if (vi > 2000)
						break;
				}
				Console.WriteLine(vi);
				return ans;
			}

			//生成当前棋局的所有走法
			List<pair> GetMoves(bool ThisOrThat)
			{
				int ThisOrThatNum = ThisOrThat ? 0 : 7;
				List<pair> tmp = new List<pair>();
				//車
				int now = ThisOrThatNum + (int)CType.ThisRook;
				for(int i = 0; i < pos[now].Count; ++i)
				{
					int cur = pos[now][i];
					int x = cur / 9, y = cur % 9;
					//行
					int l = DirectMove[Row[x], y, 0];
					int r = DirectMove[Row[x], y, 1] > 8 ? 8 : DirectMove[Row[x], y, 1];
					for (int j = l; j <= r; ++j)
						if(j != y)
						tmp.Add(new pair(cur, x * 9 + j));
					//行吃子
					--l;
					if (l >= 0 && map[x, l] != CType.None && ThisOrThat ^ (map[x, l] < CType.ThatRook))
						tmp.Add(new pair(cur, x * 9 + l));
					++r;
					if (r < 9 && map[x, r] != CType.None && ThisOrThat ^ (map[x, r] < CType.ThatRook))
						tmp.Add(new pair(cur, x * 9 + r));
					//列
					l = DirectMove[Col[y], x, 0];
					r = DirectMove[Col[y], x, 1];
					for (int j = l; j <= r; ++j)
						if(j != x)
						tmp.Add(new pair(cur, j * 9 + y));
					//列吃子
					--l;
					if (l >= 0 && map[l, y] != CType.None && ThisOrThat ^ (map[l, y] < CType.ThatRook))
						tmp.Add(new pair(cur, l * 9 + y));
					++r;
					if (r < 10 && map[r, y] != CType.None && ThisOrThat ^ (map[r, y] < CType.ThatRook))
						tmp.Add(new pair(cur, r * 9 + y));
				}
				//炮
				now = ThisOrThatNum + (int)CType.ThisCannon;
				for(int i = 0; i < pos[now].Count; ++i)
				{
					int cur = pos[now][i];
					int x = cur / 9, y = cur % 9;
					//行
					int l = DirectMove[Row[x], y, 0];
					int r = DirectMove[Row[x], y, 1] > 8 ? 8 : DirectMove[Row[x], y, 1];
					for (int j = l; j <= r; ++j)
						if(j != y)
						tmp.Add(new pair(cur, x * 9 + j));
					//行吃子
					l = CannonFly[Row[x], y, 0];
					if (l >= 0 && map[x, l] != CType.None && ThisOrThat ^ (map[x, l] < CType.ThatRook))
						tmp.Add(new pair(cur, x * 9 + l));
					r = CannonFly[Row[x], y, 1];
					if (r >= 0 && map[x, r] != CType.None && ThisOrThat ^ (map[x, r] < CType.ThatRook))
						tmp.Add(new pair(cur, x * 9 + r));
					//列
					l = DirectMove[Col[y], x, 0];
					r = DirectMove[Col[y], x, 1];
					for (int j = l; j <= r; ++j)
						if(j != x)
						tmp.Add(new pair(cur, j * 9 + y));
					//列吃子
					l = CannonFly[Col[y], x, 0];
					if (l >= 0 && map[l, y] != CType.None && ThisOrThat ^ (map[l, y] < CType.ThatRook))
						tmp.Add(new pair(cur, l * 9 + y));
					r = CannonFly[Col[y], x, 0];
					if (r >= 0 && map[r, y] != CType.None && ThisOrThat ^ (map[r, y] < CType.ThatRook))
						tmp.Add(new pair(cur, r * 9 + y));
				}
				//马
				now = ThisOrThatNum + (int)CType.ThisKnight;
				for(int i = 0; i < pos[now].Count; ++i)
				{
					int cur = pos[now][i];
					for(int j = 0; j < chessSet[cur, 3].Count; ++j)
					{
						int px = chessSet[cur, 3][j].Value / 9, py = chessSet[cur, 3][j].Value % 9;
						if (map[px, py] == CType.None)
						{
							int x = chessSet[cur, 3][j].Key / 9, y = chessSet[cur, 3][j].Key % 9;
							if(map[x, y] == CType.None || ThisOrThat ^ (map[x, y] < CType.ThatRook))
								tmp.Add(new pair(cur, chessSet[cur, 3][j].Key));
						}
					}
				}
				//象和士
				//象
				now = ThisOrThatNum + (int)CType.ThisElephant;
				for (int i = 0; i < pos[now].Count; ++i)
				{
					int cur = pos[now][i];
					for (int j = 0; j < chessSet[cur, 2].Count; ++j)
					{
						int px = chessSet[cur, 2][j].Value / 9, py = chessSet[cur, 2][j].Value % 9;
						if (map[px, py] == CType.None)
						{
							int x = chessSet[cur, 2][j].Key / 9, y = chessSet[cur, 2][j].Key % 9;
							if (map[x, y] == CType.None || ThisOrThat ^ (map[x, y] < CType.ThatRook))
								tmp.Add(new pair(cur, chessSet[cur, 2][j].Key));
						}
					}
				}
				//士
				now = ThisOrThatNum + (int)CType.ThisMandarin;
				for (int i = 0; i < pos[now].Count; ++i)
				{
					int cur = pos[now][i];
					for (int j = 0; j < chessSet[cur, 2].Count; ++j)
					{
						int x = chessSet[cur, 2][j].Key / 9, y = chessSet[cur, 2][j].Key % 9;
						if (map[x, y] == CType.None || ThisOrThat ^ (map[x, y] < CType.ThatRook))
							tmp.Add(new pair(cur, chessSet[cur, 2][j].Key));
					}
				}
				//王和兵
				//王
				now = ThisOrThatNum + (int)CType.ThisKing;
				int whichMatrix = ThisOrThat ? 0 : 1;
				for (int i = 0; i < pos[now].Count; ++i)
				{
					int cur = pos[now][i];
					for (int j = 0; j < chessSet[cur, whichMatrix].Count; ++j)
					{
						int x = chessSet[cur, whichMatrix][j].Key / 9, y = chessSet[cur, whichMatrix][j].Key % 9;
						if (map[x, y] == CType.None || ThisOrThat ^ (map[x, y] < CType.ThatRook))
							tmp.Add(new pair(cur, chessSet[cur, whichMatrix][j].Key));
					}
				}
				//兵
				now = ThisOrThatNum + (int)CType.ThisPawn;
				for (int i = 0; i < pos[now].Count; ++i)
				{
					int cur = pos[now][i];
					for (int j = 0; j < chessSet[cur, whichMatrix].Count; ++j)
					{
						int x = chessSet[cur, whichMatrix][j].Key / 9, y = chessSet[cur, whichMatrix][j].Key % 9;
						if (map[x, y] == CType.None || ThisOrThat ^ (map[x, y] < CType.ThatRook))
							tmp.Add(new pair(cur, chessSet[cur, whichMatrix][j].Key));
					}
				}
				return tmp;
			}

			//极小极大搜索
			int AlphaBeta(int vf, int depth, bool ThisOrThat)
			{
				if(depth <= 0 || pos[(int)CType.ThisKing].Count == 0 || pos[(int)CType.ThatKing].Count == 0)
					return ThisOrThat ? ThisValue - ThatValue : ThatValue - ThisValue;
				//vi暂存当前AlphaBeta过程最大值
				int vi = Int32.MinValue;
				List<pair> moves = GetMoves(ThisOrThat);
				for(int i = 0; i < moves.Count; ++i)
				{
					int cur = moves[i].Key, aim = moves[i].Value;
					CType attack = DelPiece(cur);
					CType defense = DelPiece(aim);
					AddPiece(aim, attack);
					if (pos[(int)CType.ThisKing].Count == 1 && pos[(int)CType.ThatKing].Count == 1)
					{
						int thisKingCol = pos[(int)CType.ThisKing][0] % 9;
						int thatKingCol = pos[(int)CType.ThatKing][0] % 9;
						if (!(thisKingCol == thatKingCol && Col[thisKingCol] == 513))
							vi = Math.Max(-AlphaBeta(vi, depth - 1, !ThisOrThat), vi);
					}else
						vi = Math.Max(-AlphaBeta(vi, depth - 1, !ThisOrThat), vi);
					DelPiece(aim);
					AddPiece(aim, defense);
					AddPiece(cur, attack);
					if (vi + vf >= 0)
						return vi;
				}
				return vi;
			}
		}
	}
}
