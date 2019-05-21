using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace WebTSMClient
{



	class HttpActions //deprecated
	{
		protected static readonly string URL = "http://localhost:20101/DEV/api/repositories/ZAMS/timeseries/";
		protected static readonly string MEDIATYPE = "application/json";
		protected static readonly string CREDENTIALS = "fady:dailybuild";

		public static async Task<int> PerformGetAsync(int numberOfDataPoints)
		{
			int time = 0;
			using(var client = new HttpClient())
			{

				client.BaseAddress = new Uri(URL);
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MEDIATYPE));
				var byteArray = Encoding.ASCII.GetBytes(CREDENTIALS);
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));


				//Console.WriteLine("Performing a GET request");

				System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
				stopwatch.Start();

				HttpResponseMessage responseMessage = await client.GetAsync($"{numberOfDataPoints}/data");

				if(responseMessage.IsSuccessStatusCode)
				{
					//TimeSeries[] timeSeries = await responseMessage.Content.ReadAsAsync<TimeSeries[]>(); // response + deserialization
					//TimeSeries[] timeSeries;
					await responseMessage.Content.ReadAsStreamAsync(); // you can save this in a var and then do the deserialization separetly in the array
					stopwatch.Stop();
					time = stopwatch.Elapsed.Milliseconds;
					//Console.WriteLine($"Elapsed Time in Milliseconds: {time}");

				}
				else
				{
					//Console.WriteLine("Error!");
				}
			}
			return time;
		}

	}
}
