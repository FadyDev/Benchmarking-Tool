using System.Threading.Tasks;

namespace WebTSMClient

{
	interface IDataRetrievalMeter
	{

		Task<TotalMeasurementResult> Measure(int sampleSize, int minNumberOfDataPoints, int maxNumberOfDataPoints, int stepSize, bool applyCompression, string compressionType);
	}
}
