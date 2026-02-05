using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using Google.Protobuf.Reflection;

namespace Google.Protobuf.WellKnownTypes
{
	// Token: 0x0200002E RID: 46
	public sealed class Duration : IMessage<Duration>, IMessage, IEquatable<Duration>, IDeepCloneable<Duration>, ICustomDiagnosticMessage
	{
		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000274 RID: 628 RVA: 0x0000BD3B File Offset: 0x00009F3B
		[DebuggerNonUserCode]
		public static MessageParser<Duration> Parser
		{
			get
			{
				return Duration._parser;
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000275 RID: 629 RVA: 0x0000BD42 File Offset: 0x00009F42
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return DurationReflection.Descriptor.MessageTypes[0];
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000276 RID: 630 RVA: 0x0000BD54 File Offset: 0x00009F54
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return Duration.Descriptor;
			}
		}

		// Token: 0x06000277 RID: 631 RVA: 0x0000BD5B File Offset: 0x00009F5B
		[DebuggerNonUserCode]
		public Duration()
		{
		}

		// Token: 0x06000278 RID: 632 RVA: 0x0000BD63 File Offset: 0x00009F63
		[DebuggerNonUserCode]
		public Duration(Duration other)
			: this()
		{
			this.seconds_ = other.seconds_;
			this.nanos_ = other.nanos_;
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x06000279 RID: 633 RVA: 0x0000BD94 File Offset: 0x00009F94
		[DebuggerNonUserCode]
		public Duration Clone()
		{
			return new Duration(this);
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x0600027A RID: 634 RVA: 0x0000BD9C File Offset: 0x00009F9C
		// (set) Token: 0x0600027B RID: 635 RVA: 0x0000BDA4 File Offset: 0x00009FA4
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

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x0600027C RID: 636 RVA: 0x0000BDAD File Offset: 0x00009FAD
		// (set) Token: 0x0600027D RID: 637 RVA: 0x0000BDB5 File Offset: 0x00009FB5
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

		// Token: 0x0600027E RID: 638 RVA: 0x0000BDBE File Offset: 0x00009FBE
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as Duration);
		}

		// Token: 0x0600027F RID: 639 RVA: 0x0000BDCC File Offset: 0x00009FCC
		[DebuggerNonUserCode]
		public bool Equals(Duration other)
		{
			return other != null && (other == this || (this.Seconds == other.Seconds && this.Nanos == other.Nanos && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x06000280 RID: 640 RVA: 0x0000BE0C File Offset: 0x0000A00C
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

		// Token: 0x06000281 RID: 641 RVA: 0x0000BE64 File Offset: 0x0000A064
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x06000282 RID: 642 RVA: 0x0000BE6C File Offset: 0x0000A06C
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

		// Token: 0x06000283 RID: 643 RVA: 0x0000BEC4 File Offset: 0x0000A0C4
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

		// Token: 0x06000284 RID: 644 RVA: 0x0000BF1C File Offset: 0x0000A11C
		[DebuggerNonUserCode]
		public void MergeFrom(Duration other)
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

		// Token: 0x06000285 RID: 645 RVA: 0x0000BF6C File Offset: 0x0000A16C
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

		// Token: 0x06000286 RID: 646 RVA: 0x0000BFBC File Offset: 0x0000A1BC
		internal static bool IsNormalized(long seconds, int nanoseconds)
		{
			return seconds >= -315576000000L && seconds <= 315576000000L && nanoseconds >= -999999999 && nanoseconds <= 999999999 && Math.Sign(seconds) * Math.Sign(nanoseconds) != -1;
		}

		// Token: 0x06000287 RID: 647 RVA: 0x0000BFFB File Offset: 0x0000A1FB
		public TimeSpan ToTimeSpan()
		{
			if (!Duration.IsNormalized(this.Seconds, this.Nanos))
			{
				throw new InvalidOperationException("Duration was not a valid normalized duration");
			}
			checked
			{
				return TimeSpan.FromTicks(this.Seconds * 10000000L + unchecked((long)(this.Nanos / 100)));
			}
		}

		// Token: 0x06000288 RID: 648 RVA: 0x0000C038 File Offset: 0x0000A238
		public static Duration FromTimeSpan(TimeSpan timeSpan)
		{
			long ticks = timeSpan.Ticks;
			long num = ticks / 10000000L;
			int num2 = checked((int)(ticks % 10000000L) * 100);
			return new Duration
			{
				Seconds = num,
				Nanos = num2
			};
		}

		// Token: 0x06000289 RID: 649 RVA: 0x0000C074 File Offset: 0x0000A274
		public static Duration operator -(Duration value)
		{
			ProtoPreconditions.CheckNotNull<Duration>(value, "value");
			return checked(Duration.Normalize(0L - value.Seconds, 0 - value.Nanos));
		}

		// Token: 0x0600028A RID: 650 RVA: 0x0000C098 File Offset: 0x0000A298
		public static Duration operator +(Duration lhs, Duration rhs)
		{
			ProtoPreconditions.CheckNotNull<Duration>(lhs, "lhs");
			ProtoPreconditions.CheckNotNull<Duration>(rhs, "rhs");
			return checked(Duration.Normalize(lhs.Seconds + rhs.Seconds, lhs.Nanos + rhs.Nanos));
		}

		// Token: 0x0600028B RID: 651 RVA: 0x0000C0D1 File Offset: 0x0000A2D1
		public static Duration operator -(Duration lhs, Duration rhs)
		{
			ProtoPreconditions.CheckNotNull<Duration>(lhs, "lhs");
			ProtoPreconditions.CheckNotNull<Duration>(rhs, "rhs");
			return checked(Duration.Normalize(lhs.Seconds - rhs.Seconds, lhs.Nanos - rhs.Nanos));
		}

		// Token: 0x0600028C RID: 652 RVA: 0x0000C10C File Offset: 0x0000A30C
		internal static Duration Normalize(long seconds, int nanoseconds)
		{
			int num = nanoseconds / 1000000000;
			seconds += (long)num;
			nanoseconds -= num * 1000000000;
			if (seconds < 0L && nanoseconds > 0)
			{
				seconds += 1L;
				nanoseconds -= 1000000000;
			}
			else if (seconds > 0L && nanoseconds < 0)
			{
				seconds -= 1L;
				nanoseconds += 1000000000;
			}
			return new Duration
			{
				Seconds = seconds,
				Nanos = nanoseconds
			};
		}

		// Token: 0x0600028D RID: 653 RVA: 0x0000C178 File Offset: 0x0000A378
		internal static string ToJson(long seconds, int nanoseconds, bool diagnosticOnly)
		{
			if (Duration.IsNormalized(seconds, nanoseconds))
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append('"');
				if (seconds == 0L && nanoseconds < 0)
				{
					stringBuilder.Append('-');
				}
				stringBuilder.Append(seconds.ToString("d", CultureInfo.InvariantCulture));
				Duration.AppendNanoseconds(stringBuilder, Math.Abs(nanoseconds));
				stringBuilder.Append("s\"");
				return stringBuilder.ToString();
			}
			if (diagnosticOnly)
			{
				return string.Format(CultureInfo.InvariantCulture, "{{ \"@warning\": \"Invalid Duration\", \"seconds\": \"{0}\", \"nanos\": {1} }}", seconds, nanoseconds);
			}
			throw new InvalidOperationException("Non-normalized duration value");
		}

		// Token: 0x0600028E RID: 654 RVA: 0x0000C20D File Offset: 0x0000A40D
		public string ToDiagnosticString()
		{
			return Duration.ToJson(this.Seconds, this.Nanos, true);
		}

		// Token: 0x0600028F RID: 655 RVA: 0x0000C224 File Offset: 0x0000A424
		internal static void AppendNanoseconds(StringBuilder builder, int nanos)
		{
			if (nanos != 0)
			{
				builder.Append('.');
				if (nanos % 1000000 == 0)
				{
					builder.Append((nanos / 1000000).ToString("d3", CultureInfo.InvariantCulture));
					return;
				}
				if (nanos % 1000 == 0)
				{
					builder.Append((nanos / 1000).ToString("d6", CultureInfo.InvariantCulture));
					return;
				}
				builder.Append(nanos.ToString("d9", CultureInfo.InvariantCulture));
			}
		}

		// Token: 0x040000A2 RID: 162
		private static readonly MessageParser<Duration> _parser = new MessageParser<Duration>(() => new Duration());

		// Token: 0x040000A3 RID: 163
		private UnknownFieldSet _unknownFields;

		// Token: 0x040000A4 RID: 164
		public const int SecondsFieldNumber = 1;

		// Token: 0x040000A5 RID: 165
		private long seconds_;

		// Token: 0x040000A6 RID: 166
		public const int NanosFieldNumber = 2;

		// Token: 0x040000A7 RID: 167
		private int nanos_;

		// Token: 0x040000A8 RID: 168
		public const int NanosecondsPerSecond = 1000000000;

		// Token: 0x040000A9 RID: 169
		public const int NanosecondsPerTick = 100;

		// Token: 0x040000AA RID: 170
		public const long MaxSeconds = 315576000000L;

		// Token: 0x040000AB RID: 171
		public const long MinSeconds = -315576000000L;

		// Token: 0x040000AC RID: 172
		internal const int MaxNanoseconds = 999999999;

		// Token: 0x040000AD RID: 173
		internal const int MinNanoseconds = -999999999;
	}
}
