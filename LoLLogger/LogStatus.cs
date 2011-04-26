using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoLLogs
{
	public class LogStatus
	{
		public long offset;

		public LogStatus()
		{
			offset = 0;
		}

		public bool LogHasChanged(long currentOffset)
		{
			return currentOffset > offset;
		}
	}
}
