using System.Drawing;
using System.Windows.Forms;
using System;

namespace ChineseChess
{
	public partial class View : Form
	{

		public class ChessPiece{
			public Player Player;
			public ChessType Type;
			public PictureBox Picture;
			public ChessPiece(Player Player, ChessType Type)
			{
				this.Player = Player;
				this.Type = Type;
				this.Picture = new PictureBox();
			}
		}

		public Controller controller;
		private int gridLength = 1;
		public ChessPiece[] chesspiece;
		public PictureBox picturebox2;

		public View()
		{
			picturebox2 = new PictureBox();
			InitializeComponent();
			picturebox2.Image = Properties.Resources.chessboard;
			picturebox2.SizeMode = PictureBoxSizeMode.StretchImage;
			gridLength = 1;
			pictureBox1.Controls.Add(picturebox2);
		}

		#region 得到对应的棋子图片
		Bitmap GetCorrespondingImage(Player color, ChessType type)
		{
			if(color == Player.Red)
			{
				switch (type)
				{
					case ChessType.Rook1:
					case ChessType.Rook2:
						return Properties.Resources.RedRook;
					case ChessType.Knight1:
					case ChessType.Knight2:
						return Properties.Resources.RedKnight;
					case ChessType.Elephant1:
					case ChessType.Elephant2:
						return Properties.Resources.RedElephant;
					case ChessType.Mandarin1:
					case ChessType.Mandarin2:
						return Properties.Resources.RedMandarin;
					case ChessType.King:
						return Properties.Resources.RedKing;
					case ChessType.Cannon1:
					case ChessType.Cannon2:
						return Properties.Resources.RedCannon;
					case ChessType.Pawn1:
					case ChessType.Pawn2:
					case ChessType.Pawn3:
					case ChessType.Pawn4:
					case ChessType.Pawn5:
						return Properties.Resources.RedPawn;
					default:
						return Properties.Resources.RedRook;
				}
			}
			else
			{
				switch (type)
				{
					case ChessType.Rook1:
					case ChessType.Rook2:
						return Properties.Resources.BlackRook;
					case ChessType.Knight1:
					case ChessType.Knight2:
						return Properties.Resources.BlackKnight;
					case ChessType.Elephant1:
					case ChessType.Elephant2:
						return Properties.Resources.BlackElephant;
					case ChessType.Mandarin1:
					case ChessType.Mandarin2:
						return Properties.Resources.BlackMandarin;
					case ChessType.King:
						return Properties.Resources.BlackKing;
					case ChessType.Cannon1:
					case ChessType.Cannon2:
						return Properties.Resources.BlackCannon;
					case ChessType.Pawn1:
					case ChessType.Pawn2:
					case ChessType.Pawn3:
					case ChessType.Pawn4:
					case ChessType.Pawn5:
						return Properties.Resources.BlackPawn;
					default:
						return Properties.Resources.BlackRook;
				}
			}
		}
		#endregion

		#region 画棋盘
		private void DrawChessBoard()
		{
			controller.SetChessBoard(picturebox2);
			chesspiece = new ChessPiece[32];
			for(int i = 0; i < 16; ++i)
			{
				chesspiece[i] = new ChessPiece(Player.Red, (ChessType)i);
				Binding binding1 = new Binding("Location", controller.model[Player.Red, (ChessType)i], "Location");
				binding1.Format += Binding_Format;
				chesspiece[i].Picture.Image = GetCorrespondingImage(Player.Red, (ChessType)i);
				chesspiece[i].Picture.SizeMode = PictureBoxSizeMode.StretchImage;
				chesspiece[i].Picture.DataBindings.Add(binding1);
				chesspiece[i].Picture.DataBindings.Add("Visible", controller.model[Player.Red, (ChessType)i], "Live");
				chesspiece[i].Picture.Size = new Size(gridLength, gridLength);
				Point tmp1 = controller.model[Player.Red, (ChessType)i].Location;
				tmp1.X *= gridLength;
				tmp1.Y *= gridLength;
				chesspiece[i].Picture.Location = tmp1;
				picturebox2.Controls.Add(chesspiece[i].Picture);
				controller.SetChess(chesspiece[i]);

				chesspiece[i + 16] = new ChessPiece(Player.Black, (ChessType)i);
				Binding binding2 = new Binding("Location", controller.model[Player.Black, (ChessType)i], "Location");
				binding2.Format += Binding_Format;
				chesspiece[i + 16].Picture.Image = GetCorrespondingImage(Player.Black, (ChessType)i);
				chesspiece[i + 16].Picture.SizeMode = PictureBoxSizeMode.StretchImage;
				chesspiece[i + 16].Picture.DataBindings.Add(binding2);
				chesspiece[i + 16].Picture.DataBindings.Add("Visible", controller.model[Player.Black, (ChessType)i], "Live");
				chesspiece[i + 16].Picture.Size = new Size(gridLength, gridLength);
				Point tmp2 = controller.model[Player.Black, (ChessType)i].Location;
				tmp1.X *= gridLength;
				tmp1.Y *= gridLength;
				chesspiece[i + 16].Picture.Location = tmp2;
				picturebox2.Controls.Add(chesspiece[i + 16].Picture);
				controller.SetChess(chesspiece[i + 16]);
			}
		}
		#endregion

		private void Binding_Format(object sender, ConvertEventArgs e)
		{
			Point tmp = (Point)e.Value;
			tmp.X *= gridLength;
			tmp.Y *= gridLength;
			e.Value = tmp;
		}

		//棋盘设置
		void SetChessBoard()
		{
			int x = ClientSize.Height / 10 * 15;
			if (x > ClientSize.Width) x = ClientSize.Width / 15;
			int y = x * 10;
			x *= 15;
			if (y > ClientSize.Height)
			{
				y = ClientSize.Height / 10;
				x = y * 15;
				y *= 10;
			}
			gridLength = x / 15;
			pictureBox1.Width = x;
			pictureBox1.Height = y;
			pictureBox1.Location = new Point((ClientSize.Width - x) / 2, (ClientSize.Height - y) / 2);
			picturebox2.Width = gridLength * 9;
			picturebox2.Height = pictureBox1.Height;
			picturebox2.Location = new Point(gridLength * 3, 0);
		}

		#region 当窗口大小变化棋盘大小也变化
		private void SizeChange(object sender, System.EventArgs e)
		{
			SetChessBoard();
			if (!(controller is null))
			{
				for (int i = 0; i < 16; ++i)
				{
					Point tmp = controller.model[Player.Red, (ChessType)i].Location;
					tmp.X *= gridLength;
					tmp.Y *= gridLength;
					chesspiece[i].Picture.Location = tmp;
					chesspiece[i].Picture.Size = new Size(gridLength, gridLength);
					tmp = controller.model[Player.Black, (ChessType)i].Location;
					tmp.X *= gridLength;
					tmp.Y *= gridLength;
					chesspiece[i + 16].Picture.Location = tmp;
					chesspiece[i + 16].Picture.Size = new Size(gridLength, gridLength);
				}
			}
		}
		#endregion

		private void OnLoad(object sender, EventArgs e)
		{
			SetChessBoard();
			DrawChessBoard();
		}

		FormWindowState lastState = FormWindowState.Normal;
		private void SizeMax(object sender, EventArgs e)
		{
			if (WindowState == FormWindowState.Maximized)
			{
				SizeChange(sender, e);
				lastState = FormWindowState.Maximized;
			}
			else
			if(WindowState == FormWindowState.Normal && lastState == FormWindowState.Maximized)
			{
				SizeChange(sender, e);
				lastState = FormWindowState.Normal;
			}

		}
	}
}
