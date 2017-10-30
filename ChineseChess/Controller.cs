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
			chessboard.MouseClick += (object sender, MouseEventArgs e) => {
				if(currentPlayer != Player.None && currentChess != ChessType.None)
				{
					timer.Enabled = false;
					model[currentPlayer, currentChess].Live = true;
					PictureBox tmp = (PictureBox)sender;
					double length = (double)tmp.Size.Width / 9;
					int x = e.Location.X, y = e.Location.Y;
					x = (int)Math.Round((x - length / 2) / length);
					y = (int)Math.Round((y - length / 2) / length);
					MessageBox.Show(x.ToString() + " " + y.ToString());
					LogicCheck(x, y);
					currentPlayer = Player.None;
					currentChess = ChessType.None;
				}
			};
		}

		private void OnClick(Player player, ChessType type)
		{
			if (currentPlayer != player || currentChess != type)
			{
				timer.Enabled = true;
				if(currentPlayer != Player.None && currentChess != ChessType.None)
					model[currentPlayer, currentChess].Live = true;
			}
			else
				timer.Enabled = !timer.Enabled;
			currentPlayer = player;
			currentChess = type;
		}

		//各种走法判断、吃子判断等等
		private void LogicCheck(int x, int y)
		{
			switch (currentChess)
			{
				case ChessType.Rook1:
				case ChessType.Rook2:
					if(x == model[currentPlayer, currentChess].Location.X)
					{
						int miny = y, maxy = model[currentPlayer, currentChess].Location.Y, i;
						if (miny > maxy) Swap(ref miny, ref maxy);
						miny += 1;
					}
					else
					if(y == model[currentPlayer, currentChess].Location.Y)
					{
						
					}
					break;
			}
			model[currentPlayer, currentChess].Location = new Point(x, y);
		}
	}
}
