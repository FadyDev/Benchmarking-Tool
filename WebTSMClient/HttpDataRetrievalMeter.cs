using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace WebTSMClient

{
	public abstract class HttpDataRetrievalMeter
	{

		private HttpClient _client { get; }
		protected abstract string MediaType { get; }
		protected static readonly string URL = "http://localhost:20100/DEV/api/repositories/ZAMS/timeseries/";
		protected static readonly string CREDENTIALS = "fady:******";

		public HttpDataRetrievalMeter()
		{
			var handler = new HttpClientHandler()
			{
				AutomaticDecompression = System.Net.DecompressionMethods.None

			};
			_client = new HttpClient(handler);
			_client.BaseAddress = new Uri(URL);
			_client.DefaultRequestHeaders.Accept.Clear();
			_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaType));
			var byteArray = Encoding.ASCII.GetBytes(CREDENTIALS);
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

		}
		protected virtual async Task<HttpResponseMessage> MakeRequestAsync(int numberOfDataPoints, bool applyCompression, string compressionType)
		{
			var req = new HttpRequestMessage(HttpMethod.Get, $"{numberOfDataPoints}/data");
			if(applyCompression)
			{
				req.Headers.TryAddWithoutValidation("Accept-Encoding", compressionType);

			}
			return await _client.SendAsync(req);

		}
	}
}
