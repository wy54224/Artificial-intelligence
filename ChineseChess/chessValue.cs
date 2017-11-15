using System;
namespace ChineseChess
{
	public class ChessValue
	{
		//象棋一共14种，象棋棋盘大小9*10
		private class ZobristHash
		{
			private long [,,]zobrist;

			public ZobristHash()
			{
				zobrist = new long[9, 10, 14];
				Random ro = new Random();
				for (int i = 0; i < 9; ++i)
					for (int j = 0; j < 10; ++j)
						for (int k = 0; k < 14; ++k)
							zobrist[i, j, k] = (long)(ro.NextDouble() * Int64.MaxValue);
			}

			private int GetChessNum(Player player, ChessType type)
			{
				int chessNum = (int)player * 7;
				switch (type)
				{
					case ChessType.Rook1:
					case ChessType.Rook2:
						break;
					case ChessType.Knight1:
					case ChessType.Knight2:
						chessNum += 1;
						break;
					case ChessType.Elephant1:
					case ChessType.Elephant2:
						chessNum += 2;
						break;
					case ChessType.Mandarin1:
					case ChessType.Mandarin2:
						chessNum += 3;
						break;
					case ChessType.King:
						chessNum += 4;
						break;
					case ChessType.Cannon1:
					case ChessType.Cannon2:
						chessNum += 5;
						break;
					case ChessType.Pawn1:
					case ChessType.Pawn2:
					case ChessType.Pawn3:
					case ChessType.Pawn4:
					case ChessType.Pawn5:
						chessNum += 6;
						break;
					default:
						return -1;
				}
				return chessNum;
			}

			public long GetHash(Model model)
			{
				long tmpHash = 0;
				for(Player player = Player.Black; player < Player.None; ++player)
					for(ChessType type = ChessType.Rook1; type < ChessType.None; ++type)
						if(model[player, type].Live)
						{
							int x = model[player, type].Location.X;
							int y = model[player, type].Location.Y;
							tmpHash = tmpHash ^ zobrist[x, y, GetChessNum(player, type)];
						}
				return tmpHash;
			}
		}
	}
}
