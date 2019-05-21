using System;
using FlatSharp.Attributes;
using MessagePack;
using ProtoBuf;
using ZeroFormatter;

namespace WebTSMClient
{
	[MessagePackObject]
	[ProtoContract]
	[FlatBufferStruct]
	[ZeroFormattable]
	public class TimeSeriesData
	{
		[Key(0)]
		[ProtoMember(1)]
		[FlatBufferItem(0)]
		[Index(0)]
		public virtual DateTime From { get; set; }

		[Key(1)]
		[ProtoMember(2)]
		[FlatBufferItem(1)]
		[Index(1)]
		public virtual DateTime To { get; set; }

		[Key(2)]
		[ProtoMember(3)]
		[FlatBufferItem(2)]
		[Index(2)]
		public virtual double Value { get; set; }

		[Key(3)]
		[ProtoMember(4)]
		[FlatBufferItem(3)]
		[Index(3)]
		public virtual int Flag { get; set; }

		public TimeSeriesData(DateTime from, DateTime to, double value, int flag)
		{
			From = from;
			To = to;
			Value = value;
			Flag = flag;
		}

		public TimeSeriesData()
		{
		}
	}
}
