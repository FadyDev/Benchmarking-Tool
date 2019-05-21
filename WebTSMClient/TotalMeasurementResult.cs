using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WebTSMClient

{
	class TotalMeasurementResult
	{
		public List<MeasurementResult> ResultList { get; } = new List<MeasurementResult>();

		public void WriteToFile(string fileName, string format, string compression)
		{
			File.WriteAllLines(fileName, ResultList.Select(x => string.Join(";", x.DataPoints, x.TransferAndSerializationTime, x.DeserializationTime,x.FileSize, format, compression)));
		}
	}
}
