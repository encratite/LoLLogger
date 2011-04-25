using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace LoLLogs
{
	static class Program
	{
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Logger logger = new Logger();
			logger.Run();
		}
	}
}
