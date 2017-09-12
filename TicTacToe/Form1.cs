using System;
using System.Windows.Forms;

namespace TicTacToe
{
	public partial class Form1 : Form
    {
        private int[] isClick;//isClick[i]是button(i + 1)的状态
		private int iPlayStatus;//iPlayerStatus是当前游戏状态，1为人人对战，2为人机电脑先手，3为人机人先手
		private bool player;//player用来判断人人对战时现在轮到第几个玩家

		private const bool onTheOffensive = true;//先手玩家
		private const int Player1 = 1;
		private const int Player2 = 4;
		private const int Draw = 3;//和局状态的表示

		public Form1()
        {
            InitializeComponent();
            isClick = new int[9];
            player = onTheOffensive;
            iPlayStatus = 1;
            pictureBox1.Image = player ? Properties.Resources.player1 : Properties.Resources.player2;
        }

		//电脑下棋函数
        private void Computer()
        {
            int i, max = -2, maxi = -1;
			Button btn;
            for(i = 0; i < 9; ++i)
                if(isClick[i] == 0)
                {
                    int tmp;
                    isClick[i] = Player2;
					if (WinnerCheck(false) == Player2) tmp = 1;
					else
					if (WinnerCheck(false) == Draw) tmp = 0;
					else
                    tmp = -Dfs(false);
					//Console.Write((i + 1).ToString() + ":" + tmp.ToString() + " ");
                    if(tmp > max)
                    {
                        max = tmp;
                        maxi = i;
                    }
                    isClick[i] = 0;
                }
			//Console.WriteLine(" ");
			switch (maxi)
			{
				case 0:
					btn = button1;
					break;
				case 1:
					btn = button2;
					break;
				case 2:
					btn = button3;
					break;
				case 3:
					btn = button4;
					break;
				case 4:
					btn = button5;
					break;
				case 5:
					btn = button6;
					break;
				case 6:
					btn = button7;
					break;
				case 7:
					btn = button8;
					break;
				case 8:
					btn = button9;
					break;
				default:
					btn = null;
					break;
			}
			btn.Image = Properties.Resources.player2;
			pictureBox1.Image = Properties.Resources.player1;
			btn.Enabled = false;
			isClick[maxi] = 4;
		}

		//极小极大搜索，1表示必胜态，0表示和局态，-1表示必败态
        private int Dfs(bool isComputer)
        {
            int max = -1, i, now;
            bool mustPut, isDraw;
            mustPut = false;
			isDraw = false;
            if (isComputer) now = Player2; else now = Player1;

			//搜索终止条件，下一步必胜或已经和局
            for(i = 0; i < 9; ++i)
                if(isClick[i] == 0)
                {
                    isClick[i] = now;
                    if(WinnerCheck(false) == now)
                    {
                        mustPut = true;
                        isClick[i] = 0;
                        break;
                    }else
					if(WinnerCheck(false) == 3)
					{
						isDraw = true;
						isClick[i] = 0;
						break;
					}
                    isClick[i] = 0;
                }
            if (mustPut) return 1;
			if (isDraw) return 0;

			for(i = 0; i < 9; ++i)
				if(isClick[i] == 0)
				{
					isClick[i] = now;
					max = Math.Max(max, -Dfs(!isComputer));
					isClick[i] = 0;
				}
            return max;
        }

        private int WinnerCheck(bool changeStatus)
        {
            int Winner = 0, sum, i;
            bool bIsClick;

            sum = isClick[0] + isClick[1] + isClick[2];
            if (sum == 3)Winner = Player1;
            else
            if (sum == 12)Winner = Player2;

            sum = isClick[0] + isClick[4] + isClick[8];
            if (sum == 3)Winner = Player1;
            else
            if (sum == 12) Winner = Player2;

            sum = isClick[0] + isClick[3] + isClick[6];
            if (sum == 3) Winner = Player1;
            else
            if (sum == 12) Winner = Player2;

            sum = isClick[2] + isClick[5] + isClick[8];
            if (sum == 3) Winner = Player1;
            else
            if (sum == 12) Winner = Player2;

            sum = isClick[2] + isClick[4] + isClick[6];
            if (sum == 3) Winner = Player1;
            else
            if (sum == 12) Winner = Player2;

            sum = isClick[3] + isClick[4] + isClick[5];
            if (sum == 3) Winner = Player1;
            else
            if (sum == 12) Winner = Player2;

            sum = isClick[1] + isClick[4] + isClick[7];
            if (sum == 3) Winner = Player1;
            else
            if (sum == 12) Winner = Player2;

            sum = isClick[6] + isClick[7] + isClick[8];
            if (sum == 3) Winner = Player1;
            else
            if (sum == 12) Winner = Player2;

            bIsClick = true;
			/*Console.WriteLine(isClick[0] + " " + isClick[1] + " " + isClick[2] + " " 
				+ isClick[3] + " " + isClick[4] + " " + isClick[5] + " " 
				+ isClick[6] + " " + isClick[7] + " " + isClick[8]);*/
            for(i = 0; i < 9; ++i)
                if (isClick[i] == 0)
                {
                    bIsClick = false;
                    break;
                }
            if (bIsClick && Winner == 0)
            {
                if (changeStatus)
                {
                    label2.Text = "Draw!";
                    pictureBox1.Image = null;
                    button1.Enabled = false;
                    button2.Enabled = false;
                    button3.Enabled = false;
                    button6.Enabled = false;
                    button5.Enabled = false;
                    button4.Enabled = false;
                    button9.Enabled = false;
                    button8.Enabled = false;
                    button7.Enabled = false;
                }
                return Draw;
            }

            if (Winner != 0)
            {
                if (changeStatus)
                {
                    label2.Text = "Winner :" + (Winner == Player1 ? "×" : "○");
                    pictureBox1.Image = null;
                    button1.Enabled = false;
                    button2.Enabled = false;
                    button3.Enabled = false;
                    button6.Enabled = false;
                    button5.Enabled = false;
                    button4.Enabled = false;
                    button9.Enabled = false;
                    button8.Enabled = false;
                    button7.Enabled = false;
                }
                return Winner;
            }
            return 0;
        }

        private void OnClick(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            button.Enabled = false;
            if (player)
            {
                isClick[button.Name[6] - '1'] = Player1;
                button.Image = Properties.Resources.player1;
                pictureBox1.Image = Properties.Resources.player2;
                if (iPlayStatus == 1) player = !player;else
                if (WinnerCheck(true) != Draw)Computer();
            }
            else
            {
                isClick[button.Name[6] - '1'] = Player2;
                button.Image = Properties.Resources.player2;
                pictureBox1.Image = Properties.Resources.player1;
                player = !player;
            }
            WinnerCheck(true);
        }

        private void ReFresh()
        {
            Array.Clear(isClick, 0, 9);
            player = onTheOffensive;
            pictureBox1.Image = player ? Properties.Resources.player1 : Properties.Resources.player2;
            button1.Enabled = true;
            button1.Image = null;
            button2.Enabled = true;
            button2.Image = null;
            button3.Enabled = true;
            button3.Image = null;
            button6.Enabled = true;
            button6.Image = null;
            button5.Enabled = true;
            button5.Image = null;
            button4.Enabled = true;
            button4.Image = null;
            button9.Enabled = true;
            button9.Image = null;
            button8.Enabled = true;
            button8.Image = null;
            button7.Enabled = true;
            button7.Image = null;
        }

        private void REFRESH(object sender, EventArgs e)
        {
            ReFresh();
        }

        private void PlayStatus(object sender, EventArgs e)
        {
            RadioButton rbt = (RadioButton)sender;
            if (rbt.Checked == true) iPlayStatus = (int)rbt.Tag;
            ReFresh();

			//电脑先手直接下中间格子
            if (iPlayStatus == 2)
            {
                isClick[4] = Player2;
                button5.Image = Properties.Resources.player2;
				button5.Enabled = false;
                pictureBox1.Image = Properties.Resources.player1;
            }
        }
    }
}
