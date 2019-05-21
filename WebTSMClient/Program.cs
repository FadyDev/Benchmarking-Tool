using System;
using System.Collections.Generic;
using WebTSMClient.FormatsRetrivalMeters;

namespace WebTSMClient

{
	class Program
	{
		static void Main(string[] args)
		{
			Start:
			Console.WriteLine("\nWeb TSM Benchmarking Client\n--------------------\n");
			Console.Write("Automated test run? y/n: ");
			string automatedTest = Console.ReadLine();
			if(automatedTest.ToLower() == "y")
			{
				Console.WriteLine("Running Automated test...");
				System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
				stopwatch.Start();

				FullAutomatedTest();

				stopwatch.Stop();
				Console.WriteLine("Automated test finished.\nTotal Time: " + stopwatch.Elapsed.ToString());
				Console.ReadKey();
			}
			else if(automatedTest.ToLower() == "n")
			{
				Console.WriteLine();
				Console.WriteLine("Manual Tests");
				Console.WriteLine("choose a Content type for the test run\n");
				Console.WriteLine("JSON    BSON     MsgPack     protoBuff     ZeroFormatter\n");
				Console.Write("your Choice:");
				string choice = Console.ReadLine();
				Console.WriteLine();
				Console.Write("ApplyCompression? Y/N:  ");
				string applyCompressionAnswer = Console.ReadLine();
				bool applyCompression = false;
				string compressiontype = "No Compression";

				if(choice == "")
				{
					choice = "json";
				}

				if(applyCompressionAnswer.ToLower() == "y")
				{
					applyCompression = true;
					Console.Write("gzip deflate or br? ");
					compressiontype = Console.ReadLine();
				}

				string choiceName = "";


				IDataRetrievalMeter meter;
				switch(choice.ToLower())
				{
					case "json":
						choiceName = "JSON";
						meter = new JsonDataRetrievalMeter();
						break;
					case "bson":
						choiceName = "BSON";
						meter = new BsonDataRetrievalMeter();
						break;
					case "msgpack":
						choiceName = "MessagePack";
						meter = new MsgPackDataRetrievalMeter();
						break;
					case "protobuff":
						choiceName = "Protocol Buffers";
						meter = new ProtoBuffDataRetrievalMeter();
						break;
					case "zero":
						choiceName = "Zeroformatter";
						meter = new ZeroFormatterDataRetrievalMeter();
						break;
					default:
						goto Start;
				}
				Console.WriteLine($"Performing a GET test using {choiceName} with {compressiontype}");
				var result = meter.Measure(10, 0, 1000, 20, applyCompression, compressiontype);
				result.Result.WriteToFile("result.csv", choiceName, compressiontype);


				Console.WriteLine($"Test with {choiceName} and {compressiontype} is done. Results saved");
				Console.WriteLine("Press Enter to choose a test. ESC to exit");
				var key = Console.ReadKey();
				if(key.Key == ConsoleKey.Escape)
				{
					Environment.Exit(0);
				}
				else if(key.Key == ConsoleKey.Enter)
				{
					goto Start;
				}

				Console.ReadLine();
			}
			else
			{
				goto Start;
			}
		}

		public static void FullAutomatedTest()
		{
			string[] formats = { "JSON", "BSON", "MessagePack", "ProtocolBuffer", "ZeroFormatter" };
			List<TestingProperties> propList = new List<TestingProperties> {
				new TestingProperties(false, "NoCompression"),
				new TestingProperties(true, "gzip"),
				new TestingProperties(true, "deflate"),
				new TestingProperties(true, "br")
			};

			foreach(var format in formats)
			{
				IDataRetrievalMeter meter;
				switch(format)
				{
					case "JSON":
						meter = new JsonDataRetrievalMeter();
						break;
					case "BSON":
						meter = new BsonDataRetrievalMeter();
						break;
					case "MessagePack":
						meter = new MsgPackDataRetrievalMeter();
						break;
					case "ProtocolBuffer":
						meter = new ProtoBuffDataRetrievalMeter();
						break;
					case "ZeroFormatter":
						meter = new ZeroFormatterDataRetrievalMeter();
						break;
					default:
						meter = new JsonDataRetrievalMeter();
						break;
				}


				foreach(var item in propList)
				{
					var result = meter.Measure(10, 0, 105102, 1000, item.ApplayCompression, item.Compression);
					result.Result.WriteToFile(@"C:\Users\Fady\Desktop\FormatsTests\Client\" + $"{format}{item.Compression}.csv", format, item.Compression);
				}
			}

		}
	}
}
