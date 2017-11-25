using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Threading;
using pair = System.Collections.Generic.KeyValuePair<int, int>;
using tri = System.Collections.Generic.KeyValuePair<int, System.Collections.Generic.KeyValuePair<int, int>>;

namespace ChineseChess
{
	//有个小bug
	//控件picturebox刚出现(闪烁的时候visible刚设定为true)，click事件谁也接收不到(就一瞬间)
	public partial class Controller
	{
		//view和model
		public View view;
		public Model model;
		//人工智障
		AI ai;
		//当前操作玩家和当前操作的棋子
		private Player currentPlayer;
		private ChessType currentChess;
		private System.Timers.Timer timer;
		private Player[,] playPos;
		private ChessType[,] typePos;
		//黑棋的卒在y轴的前进方向
		private int direction;
		//当前是哪个玩家在操作
		private Player whichPlayer;
		//哪个是电脑
		private Player computer;
		//用来悔棋的
		public struct MoveOperation{
			public Point start, end;
			public ChessType typeS, typeE;
			public MoveOperation(Point _start, Point _end, ChessType _typeS, ChessType _typeE)
			{
				start = _start;
				end = _end;
				typeS = _typeS;
				typeE = _typeE;
			}
		}
		List<MoveOperation> list;

		public Controller(View _view)
		{
			view = _view;
			view.controller = this;
			model = new Model();
			ai = new AI();
			currentPlayer = Player.None;
			currentChess = ChessType.None;
			playPos = new Player[9, 10];
			typePos = new ChessType[9, 10];
			timer = new System.Timers.Timer
			{
				Interval = 400,
				Enabled = false
			};
			timer.SynchronizingObject = view;
			timer.Elapsed += ChessTwinkle;
			list = new List<MoveOperation>();
		}

		private void Swap<T>(ref T lsh, ref T rsh)
		{
			T tmp = lsh;
			lsh = rsh;
			rsh = tmp;
		}

		//选中棋子闪烁
		private void ChessTwinkle(object sender, System.Timers.ElapsedEventArgs e)
		{
			model[currentPlayer, currentChess].Live = !model[currentPlayer, currentChess].Live;
		}

		public void SetChess(View.ChessPiece chess)
		{
			chess.Picture.Click += (object sender, EventArgs e)=> OnClick(chess.Player, chess.Type);
		}

		//悔棋设置
		public void SetBack(Button button)
		{
			button.Click += (object sender, EventArgs e) =>
			{
				timer.Enabled = false;
				if(currentPlayer != Player.None && currentChess != ChessType.None)
				{
					model[currentPlayer, currentChess].Live = true;
					currentPlayer = Player.None;
					currentChess = ChessType.None;
				}
				while(list.Count > 0)
				{
					MoveOperation op = list[list.Count - 1];
					list.RemoveAt(list.Count - 1);
					ChessType typeS = op.typeS, typeE = op.typeE;
					Player playerE = whichPlayer, playerS = (Player)((int)whichPlayer ^ 1);
					CType tmp = ai.DelPiece(op.end.Y * 9 + op.end.X);
					model[playerS, typeS].Location = op.start;
					model[playerS, typeS].Live = true;
					ai.AddPiece(op.start.Y * 9 + op.start.X, tmp);
					if(typeE != ChessType.None)
					{
						model[playerE, typeE].Location = op.end;
						model[playerE, typeE].Live = true;
						int ThisOrThat = tmp >= CType.ThatRook ? 0 : 7;
						ai.AddPiece(op.end.Y * 9 + op.end.X, ThisOrThat + ChessType2CType(typeE));
					}
					playPos[op.start.X, op.start.Y] = playerS;
					typePos[op.start.X, op.start.Y] = typeS;
					playPos[op.end.X, op.end.Y] = typeE == ChessType.None ? Player.None : playerE;
					typePos[op.end.X, op.end.Y] = typeE;
					whichPlayer = playerS;
					ChessEnableSet(whichPlayer);
					if (whichPlayer != computer)
						break;
				}
				list.Clear();
			};
		}

		//不是自己的棋不给选择
		private void ChessEnableSet(Player whichPlayer)
		{
			int i = (int)whichPlayer;
			Console.WriteLine(i);
			view.whichPlayer[i].Visible = true;
			view.whichPlayer[i ^ 1].Visible = false;
			view.whichPlayer[i ^ 1].Refresh();
			//view.whichPlayer[i].Refresh();
			for (ChessType j = ChessType.Rook1; j <= ChessType.Pawn5; ++j)
			{
				model[whichPlayer, j].Enabled = true;
				model[(Player)(i ^ 1), j].Enabled = false;
			}
		}

		//棋盘初始设置（棋盘存储、鼠标事件添加）
		public void SetChessBoard(PictureBox chessboard, Player player)
		{
			for(int i = 0; i < 9; ++i)
				for(int j = 0; j < 10; ++j)
				{
					playPos[i, j] = Player.None;
					typePos[i, j] = ChessType.None;
				}
			model.Reset(player);
			for(Player i = Player.Black; i <= Player.Red; ++i)
				for(ChessType j = ChessType.Rook1; j <= ChessType.Pawn5; ++j)
				{
					playPos[model[i, j].Location.X, model[i, j].Location.Y] = i;
					typePos[model[i, j].Location.X, model[i, j].Location.Y] = j;
				}
			direction = model[Player.Black, ChessType.King].Location.Y == 0 ? 1 : -1;
			whichPlayer = player;
			computer = (Player)((int)whichPlayer ^ 1);
			ChessEnableSet(whichPlayer);
			chessboard.MouseClick += (object sender, MouseEventArgs e) => {
				timer.Enabled = false;
				if(currentPlayer != Player.None && currentChess != ChessType.None)
				{
					model[currentPlayer, currentChess].Live = true;
					PictureBox tmp = (PictureBox)sender;
					double length = (double)tmp.Size.Width / 9;
					int x = e.Location.X, y = e.Location.Y;
					x = (int)Math.Round((x - length / 2) / length);
					y = (int)Math.Round((y - length / 2) / length);
					if(ChessboardClick(x, y))
					{
						Thread thread = new Thread((ThreadStart)delegate {
							DateTime start = DateTime.Now;
							pair move;
							for (int i = 1; ; ++i)
							{
								tri aiMove = ai.MaxSearch(i);
								move = aiMove.Value;
								//Console.WriteLine(move.Key + " " + move.Value);
								if (aiMove.Key > 2000 || aiMove.Key < -2000 || (DateTime.Now - start).TotalSeconds > 1)
									break;
								//Console.WriteLine(i);
							}
							if (move.Key < 0)
							{
								Console.WriteLine("没法走了");
							}
							else
							{
								currentPlayer = computer;
								currentChess = typePos[move.Key % 9, move.Key / 9];
								view.Invoke((EventHandler)delegate
								{
									ChessboardClick(move.Value % 9, move.Value / 9);
								});
							}
						});
						thread.Start();
					}
				}
			};
		}

		//棋盘落子判断
		private bool ChessboardClick(int x, int y)
		{
			bool value = false;
			if ((x != model[currentPlayer, currentChess].Location.X ||
					y != model[currentPlayer, currentChess].Location.Y) && LogicCheck(x, y, true))
			{
				whichPlayer = (Player)((int)whichPlayer ^ 1);
				ChessEnableSet(whichPlayer);
				//将军逻辑判断
				Point loc = model[whichPlayer, ChessType.King].Location;
				if (currentChess != ChessType.King)
				{
					if (LogicCheck(loc.X, loc.Y, false)) MessageBox.Show("将军");
				}
				else
				{
					currentPlayer = whichPlayer;
					for(currentChess = ChessType.Rook1; currentChess < ChessType.None; ++currentChess)
						if(currentChess != ChessType.King && LogicCheck(x, y, false))
						{
							MessageBox.Show("将军");
							break;
						}
				}
				value = true;
			}
			currentPlayer = Player.None;
			currentChess = ChessType.None;
			if (!(model[Player.Red, ChessType.King].Live))
			{
				MessageBox.Show("黑棋胜!");
			}
			else
			if (!(model[Player.Black, ChessType.King].Live))
			{
				MessageBox.Show("红棋胜!");
			}
			return value;
		}

		//棋子单击判断（注意，由于对方棋子enabled设置为false，因此单击事件是失效的）
		private void OnClick(Player player, ChessType type)
		{
			if (currentPlayer != player || currentChess != type)
			{
				timer.Enabled = false;
				if (currentPlayer != Player.None && currentChess != ChessType.None)
					model[currentPlayer, currentChess].Live = true;
				currentPlayer = player;
				currentChess = type;
				timer.Enabled = true;
			}
			else
			{
				if (timer.Enabled)
				{
					timer.Enabled = false;
					if (currentPlayer != Player.None && currentChess != ChessType.None)
						model[currentPlayer, currentChess].Live = true;
					currentPlayer = Player.None;
					currentChess = ChessType.None;
				}
				else
					timer.Enabled = true;
			}
		}

		//各种走法判断、吃子判断等等
		private bool LogicCheck(int x, int y, bool canMove)
		{
			if (playPos[x, y] == currentPlayer) return false;
			int currentX = model[currentPlayer, currentChess].Location.X, currentY = model[currentPlayer, currentChess].Location.Y;
			#region 将帅照面逻辑判断
			//如果不是移动将或者帅
			if(currentChess != ChessType.King)
			{
				if (model[Player.Red, ChessType.King].Location.X == model[Player.Black, ChessType.King].Location.X
				&& model[Player.Red, ChessType.King].Location.X == currentX && currentX != x)
				{
					int miny = model[Player.Red, ChessType.King].Location.Y;
					int maxy = model[Player.Black, ChessType.King].Location.Y;
					if (miny > maxy) Swap(ref miny, ref maxy);
					bool bj = true;
					++miny;
					for(int i = miny; i < maxy; ++i)
						if(playPos[currentX, i] != Player.None && i != currentY)
						{
							bj = false;
							break;
						}
					if (bj)
					{
						MessageBox.Show("将帅不能照面！");
						return false;
					}
				}
			}
			#endregion
			switch (currentChess)
			{
				case ChessType.Rook1:
				case ChessType.Rook2:
				#region 車判断逻辑
					if (x == currentX)
					{
						int miny = y, maxy = currentY;
						if (miny > maxy) Swap(ref miny, ref maxy);
						miny += 1;
						for (int i = miny; i < maxy; ++i)
							if (playPos[x, i] != Player.None && typePos[x, i] != ChessType.None)
								return false;
					}
					else
					if (y == currentY)
					{
						int minx = x, maxx = currentX;
						if (minx > maxx) Swap(ref minx, ref maxx);
						minx += 1;
						for (int i = minx; i < maxx; ++i)
							if (playPos[i, y] != Player.None && typePos[i, y] != ChessType.None)
								return false;
					}
					else
						return false;
					break;
				#endregion
				case ChessType.Knight1:
				case ChessType.Knight2:
				#region 马判断逻辑
					if (currentY == y + 2 && (currentX == x + 1 || currentX + 1 == x))
					{
						if (playPos[currentX, currentY - 1] != Player.None && typePos[currentX, currentY - 1] != ChessType.None)
							return false;
					}
					else
					if (currentX == x + 2 && (currentY == y + 1 || currentY + 1 == y))
					{
						if (playPos[currentX - 1, currentY] != Player.None && typePos[currentX - 1, currentY] != ChessType.None)
							return false;
					}
					else
					if (currentY + 2 == y && (currentX == x + 1 || currentX + 1 == x))
					{
						if (playPos[currentX, currentY + 1] != Player.None && typePos[currentX, currentY + 1] != ChessType.None)
							return false;
					}
					else
					if (currentX + 2 == x && (currentY == y + 1 || currentY + 1 == y))
					{
						if (playPos[currentX + 1, currentY] != Player.None && typePos[currentX + 1, currentY] != ChessType.None)
							return false;
					}
					else
						return false;
					break;
				#endregion
				case ChessType.Elephant1:
				case ChessType.Elephant2:
				#region 象/相逻辑判断
					if ((currentY <= 4 && y <= 4) || (currentY >= 5 && y >= 5))
					{
						if (currentX + 2 == x && currentY + 2 == y)
						{
							if (playPos[currentX + 1, currentY + 1] != Player.None && typePos[currentX + 1, currentY + 1] != ChessType.None)
								return false;
						}
						else
						if (currentX + 2 == x && currentY - 2 == y)
						{
							if (playPos[currentX + 1, currentY - 1] != Player.None && typePos[currentX + 1, currentY - 1] != ChessType.None)
								return false;
						}
						else
						if (currentX - 2 == x && currentY + 2 == y)
						{
							if (playPos[currentX - 1, currentY + 1] != Player.None && typePos[currentX - 1, currentY + 1] != ChessType.None)
								return false;
						}
						else
						if (currentX - 2 == x && currentY - 2 == y)
						{
							if (playPos[currentX - 1, currentY - 1] != Player.None && typePos[currentX - 1, currentY - 1] != ChessType.None)
								return false;
						}
						else
							return false;
					}
					else
						return false;
					break;
				#endregion
				case ChessType.Mandarin1:
				case ChessType.Mandarin2:
				#region 士/仕逻辑判断
					if (x < 3 || x > 5) return false;
					if ((currentY <= 2 && y <= 2) || (currentY >= 7 && y >= 7))
					{
						if (Math.Abs(x - currentX) != 1 || Math.Abs(y - currentY) != 1)
							return false;
					}
					else
						return false;
					break;
				#endregion
				case ChessType.Cannon1:
				case ChessType.Cannon2:
				#region 炮逻辑判断
					int chessNum = 0;
					if (x == currentX)
					{
						int miny = y, maxy = currentY;
						if (miny > maxy) Swap(ref miny, ref maxy);
						miny += 1;
						for (int i = miny; i < maxy; ++i)
							if (playPos[x, i] != Player.None && typePos[x, i] != ChessType.None)
								++chessNum;
					}
					else
					if (y == currentY)
					{
						int minx = x, maxx = currentX;
						if (minx > maxx) Swap(ref minx, ref maxx);
						minx += 1;
						for (int i = minx; i < maxx; ++i)
							if (playPos[i, y] != Player.None && typePos[i, y] != ChessType.None)
								++chessNum;
					}
					else
						return false;
					if (playPos[x, y] != Player.None && typePos[x, y] != ChessType.None)
					{
						if (chessNum != 1) return false;
					}
					else
						if (chessNum != 0) return false;
					break;
				#endregion
				case ChessType.King:
				#region 将/帅逻辑判断
					if (x < 3 || x > 5) return false;
					if ((currentY <= 2 && y <= 2) || (currentY >= 7 && y >= 7))
					{
						if (Math.Abs(x - currentX) + Math.Abs(y - currentY) != 1)
							return false;
					}
					else
						return false;
					//如果是移动将或者帅
					Player anotherPlayer = (Player)((int)currentPlayer ^ 1);
					if (x == model[anotherPlayer, ChessType.King].Location.X)
					{
						int miny = model[anotherPlayer, ChessType.King].Location.Y;
						int maxy = y;
						if (miny > maxy) Swap(ref miny, ref maxy);
						bool bj = true;
						++miny;
						for (int i = miny; i < maxy; ++i)
							if (playPos[x, i] != Player.None)
							{
								bj = false;
								break;
							}
						if (bj)
						{
							MessageBox.Show("将帅不能照面！");
							return false;
						}
					}
					break;
				#endregion
				case ChessType.Pawn1:
				case ChessType.Pawn2:
				case ChessType.Pawn3:
				case ChessType.Pawn4:
				case ChessType.Pawn5:
				#region 兵/卒逻辑判断
					if (currentPlayer == Player.Black)
					{
						if((direction == 1 && currentY <= 4) || (direction == -1 && currentY >= 5))
						{
							if (x != currentX || y != currentY + direction)
								return false;
						}
						else
						{
							if (!((x == currentX && y == currentY + direction) || (y == currentY && Math.Abs(x - currentX) == 1)))
								return false;
						}
					}
					else
					if (currentPlayer == Player.Red)
					{
						if ((direction == -1 && currentY <= 4) || (direction == 1 && currentY >= 5))
						{
							if (x != currentX || y != currentY - direction)
								return false;
						}
						else
						{
							//MessageBox.Show(x + " " + y + " " + currentX + " " + currentY);
							if (!((x == currentX && y == currentY - direction) || (y == currentY && Math.Abs(x - currentX) == 1)))
								return false;
						}
					}
					else
						return false;
					break;
				#endregion
				default:
					return false;
			}
			//如果需要判断完逻辑后移动
			if (canMove)
			{
				//加入悔棋的队列
				while (list.Count > 1)
					list.RemoveAt(0);
				list.Add(new MoveOperation(model[currentPlayer, currentChess].Location, new Point(x, y), currentChess, typePos[x, y]));
				//吃子
				if (playPos[x, y] != Player.None && typePos[x, y] != ChessType.None)
				{
					Player tmpPlayer = playPos[x, y];
					ChessType tmpType = typePos[x, y];
					model[tmpPlayer, tmpType].Live = false;
					ai.DelPiece(y * 9 + x);
				}
				//棋子移动
				playPos[currentX, currentY] = Player.None;
				typePos[currentX, currentY] = ChessType.None;
				CType moveType = ai.DelPiece(currentY * 9 + currentX);
				playPos[x, y] = currentPlayer;
				typePos[x, y] = currentChess;
				ai.AddPiece(y * 9 + x, moveType);
				model[currentPlayer, currentChess].Location = new Point(x, y);
			}
			return true;
		}
	}
}
