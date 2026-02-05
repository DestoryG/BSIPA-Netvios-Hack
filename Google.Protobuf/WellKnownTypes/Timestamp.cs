using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using Google.Protobuf.Reflection;

namespace Google.Protobuf.WellKnownTypes
{
	// Token: 0x0200003C RID: 60
	public sealed class Timestamp : IMessage<Timestamp>, IMessage, IEquatable<Timestamp>, IDeepCloneable<Timestamp>, ICustomDiagnosticMessage, IComparable<Timestamp>
	{
		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000322 RID: 802 RVA: 0x0000DBF3 File Offset: 0x0000BDF3
		[DebuggerNonUserCode]
		public static MessageParser<Timestamp> Parser
		{
			get
			{
				return Timestamp._parser;
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000323 RID: 803 RVA: 0x0000DBFA File Offset: 0x0000BDFA
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return TimestampReflection.Descriptor.MessageTypes[0];
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000324 RID: 804 RVA: 0x0000DC0C File Offset: 0x0000BE0C
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return Timestamp.Descriptor;
			}
		}

		// Token: 0x06000325 RID: 805 RVA: 0x0000DC13 File Offset: 0x0000BE13
		[DebuggerNonUserCode]
		public Timestamp()
		{
		}

		// Token: 0x06000326 RID: 806 RVA: 0x0000DC1B File Offset: 0x0000BE1B
		[DebuggerNonUserCode]
		public Timestamp(Timestamp other)
			: this()
		{
			this.seconds_ = other.seconds_;
			this.nanos_ = other.nanos_;
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x06000327 RID: 807 RVA: 0x0000DC4C File Offset: 0x0000BE4C
		[DebuggerNonUserCode]
		public Timestamp Clone()
		{
			return new Timestamp(this);
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000328 RID: 808 RVA: 0x0000DC54 File Offset: 0x0000BE54
		// (set) Token: 0x06000329 RID: 809 RVA: 0x0000DC5C File Offset: 0x0000BE5C
		[DebuggerNonUserCode]
		public long Seconds
		{
			get
			{
				return this.seconds_;
			}
			set
			{
				this.seconds_ = value;
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x0600032A RID: 810 RVA: 0x0000DC65 File Offset: 0x0000BE65
		// (set) Token: 0x0600032B RID: 811 RVA: 0x0000DC6D File Offset: 0x0000BE6D
		[DebuggerNonUserCode]
		public int Nanos
		{
			get
			{
				return this.nanos_;
			}
			set
			{
				this.nanos_ = value;
			}
		}

		// Token: 0x0600032C RID: 812 RVA: 0x0000DC76 File Offset: 0x0000BE76
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as Timestamp);
		}

		// Token: 0x0600032D RID: 813 RVA: 0x0000DC84 File Offset: 0x0000BE84
		[DebuggerNonUserCode]
		public bool Equals(Timestamp other)
		{
			return other != null && (other == this || (this.Seconds == other.Seconds && this.Nanos == other.Nanos && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x0600032E RID: 814 RVA: 0x0000DCC4 File Offset: 0x0000BEC4
		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (this.Seconds != 0L)
			{
				num ^= this.Seconds.GetHashCode();
			}
			if (this.Nanos != 0)
			{
				num ^= this.Nanos.GetHashCode();
			}
			if (this._unknownFields != null)
			{
				num ^= this._unknownFields.GetHashCode();
			}
			return num;
		}

		// Token: 0x0600032F RID: 815 RVA: 0x0000DD1C File Offset: 0x0000BF1C
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x06000330 RID: 816 RVA: 0x0000DD24 File Offset: 0x0000BF24
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.Seconds != 0L)
			{
				output.WriteRawTag(8);
				output.WriteInt64(this.Seconds);
			}
			if (this.Nanos != 0)
			{
				output.WriteRawTag(16);
				output.WriteInt32(this.Nanos);
			}
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x06000331 RID: 817 RVA: 0x0000DD7C File Offset: 0x0000BF7C
		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Seconds != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.Seconds);
			}
			if (this.Nanos != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Nanos);
			}
			if (this._unknownFields != null)
			{
				num += this._unknownFields.CalculateSize();
			}
			return num;
		}

		// Token: 0x06000332 RID: 818 RVA: 0x0000DDD4 File Offset: 0x0000BFD4
		[DebuggerNonUserCode]
		public void MergeFrom(Timestamp other)
		{
			if (other == null)
			{
				return;
			}
			if (other.Seconds != 0L)
			{
				this.Seconds = other.Seconds;
			}
			if (other.Nanos != 0)
			{
				this.Nanos = other.Nanos;
			}
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x06000333 RID: 819 RVA: 0x0000DE2C File Offset: 0x0000C02C
		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num != 8U)
				{
					if (num != 16U)
					{
						this._unknownFields = UnknownFieldSet.MergeFieldFrom(this._unknownFields, input);
					}
					else
					{
						this.Nanos = input.ReadInt32();
					}
				}
				else
				{
					this.Seconds = input.ReadInt64();
				}
			}
		}

		// Token: 0x06000334 RID: 820 RVA: 0x0000DE7C File Offset: 0x0000C07C
		private static bool IsNormalized(long seconds, int nanoseconds)
		{
			return nanoseconds >= 0 && nanoseconds <= 999999999 && seconds >= -62135596800L && seconds <= 253402300799L;
		}

		// Token: 0x06000335 RID: 821 RVA: 0x0000DEA7 File Offset: 0x0000C0A7
		public static Duration operator -(Timestamp lhs, Timestamp rhs)
		{
			ProtoPreconditions.CheckNotNull<Timestamp>(lhs, "lhs");
			ProtoPreconditions.CheckNotNull<Timestamp>(rhs, "rhs");
			return checked(Duration.Normalize(lhs.Seconds - rhs.Seconds, lhs.Nanos - rhs.Nanos));
		}

		// Token: 0x06000336 RID: 822 RVA: 0x0000DEE0 File Offset: 0x0000C0E0
		public static Timestamp operator +(Timestamp lhs, Duration rhs)
		{
			ProtoPreconditions.CheckNotNull<Timestamp>(lhs, "lhs");
			ProtoPreconditions.CheckNotNull<Duration>(rhs, "rhs");
			return checked(Timestamp.Normalize(lhs.Seconds + rhs.Seconds, lhs.Nanos + rhs.Nanos));
		}

		// Token: 0x06000337 RID: 823 RVA: 0x0000DF19 File Offset: 0x0000C119
		public static Timestamp operator -(Timestamp lhs, Duration rhs)
		{
			ProtoPreconditions.CheckNotNull<Timestamp>(lhs, "lhs");
			ProtoPreconditions.CheckNotNull<Duration>(rhs, "rhs");
			return checked(Timestamp.Normalize(lhs.Seconds - rhs.Seconds, lhs.Nanos - rhs.Nanos));
		}

		// Token: 0x06000338 RID: 824 RVA: 0x0000DF54 File Offset: 0x0000C154
		public DateTime ToDateTime()
		{
			if (!Timestamp.IsNormalized(this.Seconds, this.Nanos))
			{
				throw new InvalidOperationException("Timestamp contains invalid values: Seconds={Seconds}; Nanos={Nanos}");
			}
			return Timestamp.UnixEpoch.AddSeconds((double)this.Seconds).AddTicks((long)(this.Nanos / 100));
		}

		// Token: 0x06000339 RID: 825 RVA: 0x0000DFA5 File Offset: 0x0000C1A5
		public DateTimeOffset ToDateTimeOffset()
		{
			return new DateTimeOffset(this.ToDateTime(), TimeSpan.Zero);
		}

		// Token: 0x0600033A RID: 826 RVA: 0x0000DFB8 File Offset: 0x0000C1B8
		public static Timestamp FromDateTime(DateTime dateTime)
		{
			if (dateTime.Kind != DateTimeKind.Utc)
			{
				throw new ArgumentException("Conversion from DateTime to Timestamp requires the DateTime kind to be Utc", "dateTime");
			}
			long num = dateTime.Ticks / 10000000L;
			int num2 = (int)(dateTime.Ticks % 10000000L) * 100;
			return new Timestamp
			{
				Seconds = num - 62135596800L,
				Nanos = num2
			};
		}

		// Token: 0x0600033B RID: 827 RVA: 0x0000E01E File Offset: 0x0000C21E
		public static Timestamp FromDateTimeOffset(DateTimeOffset dateTimeOffset)
		{
			return Timestamp.FromDateTime(dateTimeOffset.UtcDateTime);
		}

		// Token: 0x0600033C RID: 828 RVA: 0x0000E02C File Offset: 0x0000C22C
		internal static Timestamp Normalize(long seconds, int nanoseconds)
		{
			int num = nanoseconds / 1000000000;
			seconds += (long)num;
			nanoseconds -= num * 1000000000;
			if (nanoseconds < 0)
			{
				nanoseconds += 1000000000;
				seconds -= 1L;
			}
			return new Timestamp
			{
				Seconds = seconds,
				Nanos = nanoseconds
			};
		}

		// Token: 0x0600033D RID: 829 RVA: 0x0000E078 File Offset: 0x0000C278
		internal static string ToJson(long seconds, int nanoseconds, bool diagnosticOnly)
		{
			if (Timestamp.IsNormalized(seconds, nanoseconds))
			{
				DateTime dateTime = Timestamp.UnixEpoch.AddSeconds((double)seconds);
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append('"');
				stringBuilder.Append(dateTime.ToString("yyyy'-'MM'-'dd'T'HH:mm:ss", CultureInfo.InvariantCulture));
				Duration.AppendNanoseconds(stringBuilder, nanoseconds);
				stringBuilder.Append("Z\"");
				return stringBuilder.ToString();
			}
			if (diagnosticOnly)
			{
				return string.Format(CultureInfo.InvariantCulture, "{{ \"@warning\": \"Invalid Timestamp\", \"seconds\": \"{0}\", \"nanos\": {1} }}", seconds, nanoseconds);
			}
			throw new InvalidOperationException("Non-normalized timestamp value");
		}

		// Token: 0x0600033E RID: 830 RVA: 0x0000E108 File Offset: 0x0000C308
		public int CompareTo(Timestamp other)
		{
			if (other == null)
			{
				return 1;
			}
			if (this.Seconds < other.Seconds)
			{
				return -1;
			}
			if (this.Seconds > other.Seconds)
			{
				return 1;
			}
			if (this.Nanos < other.Nanos)
			{
				return -1;
			}
			if (this.Nanos <= other.Nanos)
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x0600033F RID: 831 RVA: 0x0000E161 File Offset: 0x0000C361
		public static bool operator <(Timestamp a, Timestamp b)
		{
			return a.CompareTo(b) < 0;
		}

		// Token: 0x06000340 RID: 832 RVA: 0x0000E16D File Offset: 0x0000C36D
		public static bool operator >(Timestamp a, Timestamp b)
		{
			return a.CompareTo(b) > 0;
		}

		// Token: 0x06000341 RID: 833 RVA: 0x0000E179 File Offset: 0x0000C379
		public static bool operator <=(Timestamp a, Timestamp b)
		{
			return a.CompareTo(b) <= 0;
		}

		// Token: 0x06000342 RID: 834 RVA: 0x0000E188 File Offset: 0x0000C388
		public static bool operator >=(Timestamp a, Timestamp b)
		{
			return a.CompareTo(b) >= 0;
		}

		// Token: 0x06000343 RID: 835 RVA: 0x0000E197 File Offset: 0x0000C397
		public static bool operator ==(Timestamp a, Timestamp b)
		{
			if (a == b)
			{
				return true;
			}
			if (a != null)
			{
				return a.Equals(b);
			}
			return b == null;
		}

		// Token: 0x06000344 RID: 836 RVA: 0x0000E1B0 File Offset: 0x0000C3B0
		public static bool operator !=(Timestamp a, Timestamp b)
		{
			return !(a == b);
		}

		// Token: 0x06000345 RID: 837 RVA: 0x0000E1BC File Offset: 0x0000C3BC
		public string ToDiagnosticString()
		{
			return Timestamp.ToJson(this.Seconds, this.Nanos, true);
		}

		// Token: 0x040000D6 RID: 214
		private static readonly MessageParser<Timestamp> _parser = new MessageParser<Timestamp>(() => new Timestamp());

		// Token: 0x040000D7 RID: 215
		private UnknownFieldSet _unknownFields;

		// Token: 0x040000D8 RID: 216
		public const int SecondsFieldNumber = 1;

		// Token: 0x040000D9 RID: 217
		private long seconds_;

		// Token: 0x040000DA RID: 218
		public const int NanosFieldNumber = 2;

		// Token: 0x040000DB RID: 219
		private int nanos_;

		// Token: 0x040000DC RID: 220
		private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		// Token: 0x040000DD RID: 221
		private const long BclSecondsAtUnixEpoch = 62135596800L;

		// Token: 0x040000DE RID: 222
		internal const long UnixSecondsAtBclMaxValue = 253402300799L;

		// Token: 0x040000DF RID: 223
		internal const long UnixSecondsAtBclMinValue = -62135596800L;

		// Token: 0x040000E0 RID: 224
		internal const int MaxNanos = 999999999;
	}
}
