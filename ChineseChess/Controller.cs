using System;
using System.Drawing;
using System.Windows.Forms;

namespace ChineseChess
{
	public class Controller
	{
		public View view;
		public Model model;
		private Player currentPlayer;
		private ChessType currentChess;
		private System.Timers.Timer timer;
		private Player[,] playPos;
		private ChessType[,] typePos;
		//黑棋的卒在y轴的前进方向
		private int direction;
		//当前是哪个玩家在操作
		private Player whichPlayer;
		public Controller(View _view)
		{
			view = _view;
			view.controller = this;
			model = new Model();
			currentPlayer = Player.None;
			currentChess = ChessType.None;
			playPos = new Player[9, 10];
			typePos = new ChessType[9, 10];
			timer = new System.Timers.Timer
			{
				Interval = 400,
				Enabled = false
			};
			timer.Elapsed += ChessTwinkle;
		}

		private void Swap<T>(ref T lsh, ref T rsh)
		{
			T tmp = lsh;
			lsh = rsh;
			rsh = tmp;
		}

		private void ChessTwinkle(object sender, System.Timers.ElapsedEventArgs e)
		{
			view.Invoke((EventHandler)(delegate
			{
				model[currentPlayer, currentChess].Live = !model[currentPlayer, currentChess].Live;
			}));
		}

		public void SetChess(View.ChessPiece chess)
		{
			chess.Picture.Click += (object sender, EventArgs e)=> OnClick(chess.Player, chess.Type);
		}

		private void ChessEnableSet(Player whichPlayer)
		{
			int i = (int)whichPlayer;
			view.whichPlayer[i].Visible = true;
			view.whichPlayer[i ^ 1].Visible = false;
			for(ChessType j = ChessType.Rook1; j <= ChessType.Pawn5; ++j)
			{
				model[whichPlayer, j].Enabled = true;
				model[(Player)(i ^ 1), j].Enabled = false;
			}
		}

		public void SetChessBoard(PictureBox chessboard)
		{
			for(int i = 0; i < 9; ++i)
				for(int j = 0; j < 10; ++j)
				{
					playPos[i, j] = Player.None;
					typePos[i, j] = ChessType.None;
				}
			for(Player i = Player.Black; i <= Player.Red; ++i)
				for(ChessType j = ChessType.Rook1; j <= ChessType.Pawn5; ++j)
				{
					playPos[model[i, j].Location.X, model[i, j].Location.Y] = i;
					typePos[model[i, j].Location.X, model[i, j].Location.Y] = j;
				}
			direction = model[Player.Black, ChessType.King].Location.Y == 0 ? 1 : -1;
			whichPlayer = Player.Red;
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
					if ((x != model[currentPlayer, currentChess].Location.X ||
					y != model[currentPlayer, currentChess].Location.Y) && LogicCheck(x, y))
					{
						whichPlayer = (Player)((int)whichPlayer ^ 1);
						ChessEnableSet(whichPlayer);
					}
					//MessageBox.Show(x.ToString() + " " + y.ToString());
					currentPlayer = Player.None;
					currentChess = ChessType.None;
				}
			};
		}

		private void OnClick(Player player, ChessType type)
		{
			if (currentPlayer != player || currentChess != type)
			{
				timer.Enabled = false;
				if (currentPlayer != Player.None && currentChess != ChessType.None)
				{
					model[currentPlayer, currentChess].Live = true;
					if (currentPlayer != player)
					{
						if (LogicCheck(model[player, type].Location.X, model[player, type].Location.Y))
						{
							whichPlayer = (Player)((int)whichPlayer ^ 1);
							ChessEnableSet(whichPlayer);
						}
						currentPlayer = Player.None;
						currentChess = ChessType.None;
					} else {
						timer.Enabled = true;
						currentPlayer = player;
						currentChess = type;
					}
				}
				else
				{
					timer.Enabled = true;
					currentPlayer = player;
					currentChess = type;
				}
			}
			else
			{
				if (timer.Enabled)
				{
					currentPlayer = Player.None;
					currentChess = ChessType.None;
				}
				timer.Enabled = !timer.Enabled;
			}
		}

		//各种走法判断、吃子判断等等
		private bool LogicCheck(int x, int y)
		{
			if (playPos[x, y] == currentPlayer) return false;
			int currentX = model[currentPlayer, currentChess].Location.X, currentY = model[currentPlayer, currentChess].Location.Y;
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
								return false; ;
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
							if (x == currentX && y != currentY + direction)
								return false;
							if (y == currentY && Math.Abs(x - currentX) != 1)
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
							if (x == currentX && y != currentY - direction)
								return false;
							if (y == currentY && Math.Abs(x - currentX) != 1)
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
			//吃子
			if (playPos[x, y] != Player.None && typePos[x, y] != ChessType.None)
			{
				Player tmpPlayer = playPos[x, y];
				ChessType tmpType = typePos[x, y];
				model[tmpPlayer, tmpType].Live = false;
			}
			//棋子移动
			playPos[model[currentPlayer, currentChess].Location.X, model[currentPlayer, currentChess].Location.Y] = Player.None;
			typePos[model[currentPlayer, currentChess].Location.X, model[currentPlayer, currentChess].Location.Y] = ChessType.None;
			playPos[x, y] = currentPlayer;
			typePos[x, y] = currentChess;
			model[currentPlayer, currentChess].Location = new Point(x, y);
			return true;
		}
	}
}
