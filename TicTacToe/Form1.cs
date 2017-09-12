using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TicTacToe
{
    public partial class Form1 : Form
    {
        private struct Point
        {
            int x, y;
        }

        private int[] isClick;
        private int iPlayStatus;
        private bool player;
        private const bool onTheOffensive = true;
        public Form1()
        {
            InitializeComponent();
            isClick = new int[9];
            player = onTheOffensive;
            iPlayStatus = 1;
            pictureBox1.Image = player ? Properties.Resources.player1 : Properties.Resources.player2;
        }

        private void Computer()
        {
            int i, max, maxi;
			Button btn;
            max = -2;
            maxi = -1;
            for(i = 0; i < 9; ++i)
                if(isClick[i] == 0)
                {
                    int tmp;
                    isClick[i] = 4;
					if (WinnerCheck(false) == 4) tmp = 1;
					else
					if (WinnerCheck(false) == 3) tmp = 0;
					else
                    tmp = -dfs(false);
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

        private int dfs(bool isComputer)
        {
            int max = -1, i, now;
            bool mustPut, isTrace;
            mustPut = false;
			isTrace = false;
            if (isComputer) now = 4; else now = 1;
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
						isTrace = true;
						isClick[i] = 0;
						break;
					}
                    isClick[i] = 0;
                }
            if (mustPut) return 1;
			if (isTrace) return 0;
			max = -1;
			for(i = 0; i < 9; ++i)
				if(isClick[i] == 0)
				{
					isClick[i] = now;
					max = Math.Max(max, -dfs(!isComputer));
					isClick[i] = 0;
				}
            return max;
        }

        private int WinnerCheck(bool changeStatus)
        {
            int Winner;
            int sum;
            bool bIsClick;
            int i;
            Winner = 0;
            sum = 0;

            sum = isClick[0] + isClick[1] + isClick[2];
            if(sum == 3)Winner = 1;
            else
            if(sum == 12)Winner = 4;

            sum = isClick[0] + isClick[4] + isClick[8];
            if(sum == 3)Winner = 1;
            else
            if(sum == 12) Winner = 4;

            sum = isClick[0] + isClick[3] + isClick[6];
            if (sum == 3) Winner = 1;
            else
            if (sum == 12) Winner = 4;

            sum = isClick[2] + isClick[5] + isClick[8];
            if (sum == 3) Winner = 1;
            else
            if (sum == 12) Winner = 4;

            sum = isClick[2] + isClick[4] + isClick[6];
            if (sum == 3) Winner = 1;
            else
            if (sum == 12) Winner = 4;

            sum = isClick[3] + isClick[4] + isClick[5];
            if (sum == 3) Winner = 1;
            else
            if (sum == 12) Winner = 4;

            sum = isClick[1] + isClick[4] + isClick[7];
            if (sum == 3) Winner = 1;
            else
            if (sum == 12) Winner = 4;

            sum = isClick[6] + isClick[7] + isClick[8];
            if (sum == 3) Winner = 1;
            else
            if (sum == 12) Winner = 4;

            bIsClick = true;
            for(i = 0; i < 9; ++i)
                if (isClick[i] == 0)
                {
                    bIsClick = false;
                    break;
                }
            if (bIsClick)
            {
                if (changeStatus)
                {
                    label2.Text = "Trace!";
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
                return 3;
            }

            if (Winner != 0)
            {
                if (changeStatus)
                {
                    label2.Text = "Winner :" + (Winner == 1 ? "×" : "○");
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
                isClick[button.Name[6] - '1'] = 1;
                button.Image = Properties.Resources.player1;
                pictureBox1.Image = Properties.Resources.player2;
                if (iPlayStatus == 1) player = !player;else
                if (WinnerCheck(true) != 3)Computer();
            }
            else
            {
                isClick[button.Name[6] - '1'] = 4;
                button.Image = Properties.Resources.player2;
                pictureBox1.Image = Properties.Resources.player1;
                player = !player;
            }
            WinnerCheck(true);
        }

        private void refresh()
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

        private void ReFresh(object sender, EventArgs e)
        {
            refresh();
        }

        private void playStatus(object sender, EventArgs e)
        {
            RadioButton rbt = (RadioButton)sender;
            if (rbt.Checked == true) iPlayStatus = (int)rbt.Tag;
            refresh();
            if (iPlayStatus == 2)
            {
                isClick[4] = 4;
                button5.Image = Properties.Resources.player2;
				button5.Enabled = false;
                pictureBox1.Image = Properties.Resources.player1;
            }
        }
    }
}
