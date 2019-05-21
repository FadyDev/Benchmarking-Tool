using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Threading.Tasks;
using BrotliSharpLib;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

namespace WebTSMClient.FormatsRetrivalMeters

{
	class BsonDataRetrievalMeter : HttpDataRetrievalMeter, IDataRetrievalMeter
	{
		protected override string MediaType => "application/bson";

		public async Task<TotalMeasurementResult> Measure(int sampleSize, int minNumberOfDataPoints, int maxNumberOfDataPoints, int stepSize, bool applyCompression, string compressionType)
		{
			double transferTime = 0;
			double deserializationTime = 0;
			string contentEncoding = "";
			Stream responseStream = null;
			double fileSize = 0;

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
					contentEncoding = responseMessage.Content.Headers.ContentEncoding.ToString();

					if(responseMessage.IsSuccessStatusCode)
					{
						var packet = await responseMessage.Content.ReadAsStreamAsync();
						stopwatch.Stop();
						transferTime += stopwatch.ElapsedMilliseconds;
						stopwatch.Reset();
						fileSize += packet.Length;

						stopwatch.Start();
						//Decompression if compressed
						if(contentEncoding.ToLower().Contains("gzip"))
						{
							responseStream = new GZipStream(packet, CompressionMode.Decompress);

						}
						else if(contentEncoding.ToLower().Contains("deflate"))
						{
							responseStream = new DeflateStream(packet, CompressionMode.Decompress);

						}
						else if(contentEncoding.ToLower().Contains("br"))
						{
							var s = new BrotliStream(packet, CompressionMode.Decompress, true);
							responseStream = new MemoryStream();
							s.CopyTo(responseStream);

						}
						else
						{
							responseStream = packet;

						}

						//Deserialization

						using(BsonReader reader = new BsonReader(responseStream))
						{
							reader.ReadRootValueAsArray = true;
							JsonSerializer bsonDeserializer = new JsonSerializer();
							TimeSeriesData[] timeSeries = bsonDeserializer.Deserialize<TimeSeriesData[]>(reader);
							stopwatch.Stop();
							deserializationTime += stopwatch.ElapsedMilliseconds;
							stopwatch.Reset();
						}
					}

				}

				measurementResult.DataPoints = i;
				measurementResult.TransferAndSerializationTime = transferTime / sampleSize;
				measurementResult.DeserializationTime = deserializationTime / sampleSize;
				measurementResult.FileSize = fileSize / sampleSize;
				totalMeasurementResult.ResultList.Add(measurementResult);
			}

			return totalMeasurementResult;
		}
	}
}
