using System;
using System.Windows.Forms;

namespace ChineseChess
{
	public class Controller
	{
		public View view;
		public Model model;
		private Player currentPlayer;
		private ChessType currentChess;
		public Controller(View _view)
		{
			view = _view;
			view.controller = this;
			model = new Model();
		}

		public void SetChess(View.ChessPiece chess)
		{
			chess.Picture.Click += (object sender, EventArgs e)=> OnClick(chess.Player, chess.Type);
		}

		private void OnClick(Player player, ChessType type)
		{
			model[player, type].Live = false;
		}
	}
}
