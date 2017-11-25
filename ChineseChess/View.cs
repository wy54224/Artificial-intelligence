using System.Drawing;
using System.Windows.Forms;
using System;

namespace ChineseChess
{
	public partial class View : Form
	{
		//每个棋子及其图片类
		public class ChessPiece{
			public Player Player;
			public ChessType Type;
			public PictureBox Picture;
			public ChessPiece(Player Player, ChessType Type)
			{
				this.Player = Player;
				this.Type = Type;
				Picture = new PictureBox();
			}
		}

		public Controller controller;
		//每个棋子的长宽
		private int gridLength = 1;
		//存储棋子的数组
		public ChessPiece[] chesspiece;
		//棋盘
		public PictureBox picturebox2;
		//显示哪个玩家执棋的图片
		public PictureBox []whichPlayer;
		//悔棋的按钮
		public Button back;
		//注意关联关系:picturebox2, whichPlayer, back是添加到picturebox1上的，棋子chesspiece是添加到picturebox2上的

		public View()
		{
			picturebox2 = new PictureBox();
			whichPlayer = new PictureBox[2];
			whichPlayer[0] = new PictureBox();
			whichPlayer[1] = new PictureBox();
			back = new Button();
			InitializeComponent();
			picturebox2.Image = Properties.Resources.chessboard;
			picturebox2.SizeMode = PictureBoxSizeMode.StretchImage;
			gridLength = 1;
			whichPlayer[1].Image = Properties.Resources.RedKing;
			whichPlayer[1].SizeMode = PictureBoxSizeMode.StretchImage;
			whichPlayer[0].Image = Properties.Resources.BlackKing;
			whichPlayer[0].SizeMode = PictureBoxSizeMode.StretchImage;
			back.Text = "悔棋";
			pictureBox1.Controls.Add(picturebox2);
			pictureBox1.Controls.Add(whichPlayer[0]);
			pictureBox1.Controls.Add(whichPlayer[1]);
			pictureBox1.Controls.Add(back);
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
			//设置己方棋盘和己方棋子颜色
			controller.SetChessBoard(picturebox2, Player.Red);
			//设置悔棋按钮
			controller.SetBack(back);
			chesspiece = new ChessPiece[32];
			for (int i = 0; i < 16; ++i)
			{
				//红棋设置
				chesspiece[i] = new ChessPiece(Player.Red, (ChessType)i);
				//将图片的location属性和model的location绑定起来，并设定两个location之间的转换方程
				Binding binding1 = new Binding("Location", controller.model[Player.Red, (ChessType)i], "Location");
				binding1.Format += Binding_Format;
				chesspiece[i].Picture.Image = GetCorrespondingImage(Player.Red, (ChessType)i);
				chesspiece[i].Picture.SizeMode = PictureBoxSizeMode.StretchImage;
				chesspiece[i].Picture.DataBindings.Add(binding1);
				//Visible属性和棋子是否活着(live)绑定起来
				chesspiece[i].Picture.DataBindings.Add("Visible", controller.model[Player.Red, (ChessType)i], "Live");
				//Enabled属性绑定——非己方下棋阶段可以中断棋子上的单击事件
				chesspiece[i].Picture.DataBindings.Add("Enabled", controller.model[Player.Red, (ChessType)i], "Enabled");
				chesspiece[i].Picture.Enabled = controller.model[Player.Red, (ChessType)i].Enabled;
				chesspiece[i].Picture.Size = new Size(gridLength, gridLength);
				Point tmp1 = controller.model[Player.Red, (ChessType)i].Location;
				tmp1.X *= gridLength;
				tmp1.Y *= gridLength;
				chesspiece[i].Picture.Location = tmp1;
				picturebox2.Controls.Add(chesspiece[i].Picture);
				controller.SetChess(chesspiece[i]);

				//黑棋设置
				chesspiece[i + 16] = new ChessPiece(Player.Black, (ChessType)i);
				Binding binding2 = new Binding("Location", controller.model[Player.Black, (ChessType)i], "Location");
				binding2.Format += Binding_Format;
				chesspiece[i + 16].Picture.Image = GetCorrespondingImage(Player.Black, (ChessType)i);
				chesspiece[i + 16].Picture.SizeMode = PictureBoxSizeMode.StretchImage;
				chesspiece[i + 16].Picture.DataBindings.Add(binding2);
				chesspiece[i + 16].Picture.DataBindings.Add("Visible", controller.model[Player.Black, (ChessType)i], "Live");
				chesspiece[i + 16].Picture.DataBindings.Add("Enabled", controller.model[Player.Black, (ChessType)i], "Enabled");
				chesspiece[i + 16].Picture.Enabled = controller.model[Player.Black, (ChessType)i].Enabled;
				chesspiece[i + 16].Picture.Size = new Size(gridLength, gridLength);
				Point tmp2 = controller.model[Player.Black, (ChessType)i].Location;
				tmp1.X *= gridLength;
				tmp1.Y *= gridLength;
				chesspiece[i + 16].Picture.Location = tmp2;
				picturebox2.Controls.Add(chesspiece[i + 16].Picture);
				//controller.SetChess(chesspiece[i + 16]);
			}
		}
		#endregion

		//location关联的转换方程
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
			//这里是让x和y适应客户端大小并变成15:10的格式
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
			whichPlayer[1].Width = gridLength;
			whichPlayer[1].Height = gridLength;
			whichPlayer[1].Location = new Point(gridLength, gridLength * 17 / 2);
			whichPlayer[0].Width = gridLength;
			whichPlayer[0].Height = gridLength;
			whichPlayer[0].Location = new Point(gridLength * 13, gridLength * 3 / 2);
			back.Location = new Point(gridLength * 25 / 2, gridLength * 9 / 2 + gridLength / 6);
			back.Font = new Font("宋体", gridLength / 4, FontStyle.Regular);
			back.Width = gridLength * 3 / 2;
			back.Height = gridLength / 3 * 2;
			back.FlatStyle = FlatStyle.Flat;
		}

		#region 当窗口大小变化棋盘大小也变化
		private void SizeChange(object sender, System.EventArgs e)
		{
			SetChessBoard();
			if (!(controller == null))
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
			//最大化和还原不属于resize事件
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
