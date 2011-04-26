using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoLLogs
{
	public class LogHistory
	{
		public Nil.SerialisableDictionary<string, LogStatus> logMap;

		public LogHistory()
		{
			logMap = new Nil.SerialisableDictionary<string, LogStatus>();
		}
	}
}
