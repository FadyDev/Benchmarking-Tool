namespace WebTSMClient

{
	public class TestingProperties
	{
		public bool ApplayCompression { get; set; }
		public string Compression { get; set; }

		public TestingProperties(bool applayCompression, string compression)
		{
			ApplayCompression = applayCompression;
			Compression = compression;
		}
	}
}
