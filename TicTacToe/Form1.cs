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
        private int[] isClick;
        private bool player;
        private const bool onTheOffensive = false;
        public Form1()
        {
            InitializeComponent();
            isClick = new int[9];
            player = onTheOffensive;
            pictureBox1.Image = player ? Properties.Resources.player1 : Properties.Resources.player2;
        }

        private void WinnerCheck()
        {
            int Winner;
            int sum;
            Winner = 0;
            sum = 0;

            sum = isClick[0] + isClick[1] + isClick[2];
            if(sum == 3)Winner = 1;
            else
            if(sum == 12)Winner = 2;

            sum = isClick[0] + isClick[4] + isClick[8];
            if(sum == 3)Winner = 1;
            else
            if(sum == 12) Winner = 2;

            sum = isClick[0] + isClick[3] + isClick[6];
            if (sum == 3) Winner = 1;
            else
            if (sum == 12) Winner = 2;

            sum = isClick[2] + isClick[5] + isClick[8];
            if (sum == 3) Winner = 1;
            else
            if (sum == 12) Winner = 2;

            sum = isClick[2] + isClick[4] + isClick[6];
            if (sum == 3) Winner = 1;
            else
            if (sum == 12) Winner = 2;

            sum = isClick[3] + isClick[4] + isClick[5];
            if (sum == 3) Winner = 1;
            else
            if (sum == 12) Winner = 2;

            sum = isClick[1] + isClick[4] + isClick[7];
            if (sum == 3) Winner = 1;
            else
            if (sum == 12) Winner = 2;

            sum = isClick[6] + isClick[7] + isClick[8];
            if (sum == 3) Winner = 1;
            else
            if (sum == 12) Winner = 2;

            if (Winner != 0)
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
            }
            else
            {
                isClick[button.Name[6] - '1'] = 4;
                button.Image = Properties.Resources.player2;
                pictureBox1.Image = Properties.Resources.player1;
            }
            WinnerCheck();
            player = !player;
        }

        private void ReFresh(object sender, EventArgs e)
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
    }
}
