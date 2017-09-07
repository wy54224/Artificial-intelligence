using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TicTacToe
{
    partial class TicTacToe
    {
        private class UButton : Button
        {
            public UButton()
            {
                FlatStyle = FlatStyle.Standard;
                SetStyle(ControlStyles.Selectable, false);
            }
        }
    }
}
