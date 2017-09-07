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
    public partial class TicTacToe : Form
    {
        private bool[] isClick; 
        public TicTacToe()
        {
            InitializeComponent();
            isClick = new bool[9];
        }

        private void OnClick(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            button.Enabled = false;
            switch (button.Name)
            {
                case ""
            }
        }
    }
}
