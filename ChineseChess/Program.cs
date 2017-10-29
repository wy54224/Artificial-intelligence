using System;
using System.Windows.Forms;
using ChineseChess;

namespace ChineseChess
{
	static class Program
	{
		/// <summary>
		/// 应用程序的主入口点。
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Controller controller = new Controller(new View());
			Application.Run(controller.view);
		}
	}
}
