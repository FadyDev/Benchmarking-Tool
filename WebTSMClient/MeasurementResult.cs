namespace WebTSMClient

{
	struct MeasurementResult
	{
		public int DataPoints { get; set; }
		public double TransferAndSerializationTime { get; set; }
		public double DeserializationTime { get; set; }

		public double FileSize { get; set; }
	}
}
