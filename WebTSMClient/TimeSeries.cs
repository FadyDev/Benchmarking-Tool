using System;

namespace WebTSMClient
{
	class TimeSeries
	{
		public DateTime From { get; set; }
		public DateTime To { get; set; }
		public double Value { get; set; }
		public int Flag { get; set; }

		public TimeSeries(DateTime from, DateTime to, double value, int flag)
		{
			From = from;
			To = to;
			Value = value;
			Flag = flag;
		}
	}
}
