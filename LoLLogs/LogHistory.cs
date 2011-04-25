using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoLLogs
{
	public class LogHistory
	{
		public List<LogStatus> Logs;

		public LogHistory()
		{
			Logs = new List<LogStatus>();
		}
	}
}
