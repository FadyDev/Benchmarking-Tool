using System;
using System.Net.Http;
using System.Threading.Tasks;
using FlatSharp;

namespace WebTSMClient.FormatsRetrivalMeters
{
	[Obsolete]
	class FlatBuffersDataRetrievalMeter : HttpDataRetrievalMeter, IDataRetrievalMeter
	{
		protected override string MediaType => "application/octet-stream";

		public async Task<TotalMeasurementResult> Measure(int sampleSize, int minNumberOfDataPoints, int maxNumberOfDataPoints, int stepSize, bool applyCompression, string compressionType)
		{
			double transferTime = 0;
			double deserializationTime = 0;

			TotalMeasurementResult totalMeasurementResult = new TotalMeasurementResult();

			for(var i = minNumberOfDataPoints; i <= maxNumberOfDataPoints; i += stepSize)
			{
				MeasurementResult measurementResult = new MeasurementResult();

				for(int j = 0; j < sampleSize; j++)
				{
					//Streaming
					System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
					stopwatch.Start();
					HttpResponseMessage responseMessage = await base.MakeRequestAsync(i, applyCompression, compressionType);
					if(responseMessage.IsSuccessStatusCode)
					{
						var packet = await responseMessage.Content.ReadAsStreamAsync();
						stopwatch.Stop();
						transferTime += stopwatch.ElapsedMilliseconds;
						stopwatch.Reset();



						int length = Convert.ToInt32(packet.Length);
						byte[] data = new byte[length];
						packet.Read(data, 0, length);
						packet.Close();


						//Deserialization

						stopwatch.Start();
						TimeSeriesData[] timeSeries = FlatBufferSerializer.Default.Parse<TimeSeriesData[]>(new ArrayInputBuffer(data));
						stopwatch.Stop();
						deserializationTime += stopwatch.ElapsedMilliseconds;
						stopwatch.Reset();

					}

				}

				measurementResult.DataPoints = i;
				measurementResult.TransferAndSerializationTime = transferTime / sampleSize;
				measurementResult.DeserializationTime = deserializationTime / sampleSize;
				totalMeasurementResult.ResultList.Add(measurementResult);
			}

			return totalMeasurementResult;
		}
	}
}

