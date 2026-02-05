using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Com.Netvios.Proto.Outbound
{
	// Token: 0x02000009 RID: 9
	public sealed class Ping : IMessage<Ping>, IMessage, IEquatable<Ping>, IDeepCloneable<Ping>
	{
		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000061 RID: 97 RVA: 0x00005AF9 File Offset: 0x00003CF9
		[DebuggerNonUserCode]
		public static MessageParser<Ping> Parser
		{
			get
			{
				return Ping._parser;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000062 RID: 98 RVA: 0x00005B00 File Offset: 0x00003D00
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return BeatSaberOutboundMessageReflection.Descriptor.MessageTypes[1];
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000063 RID: 99 RVA: 0x00005B12 File Offset: 0x00003D12
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return Ping.Descriptor;
			}
		}

		// Token: 0x06000064 RID: 100 RVA: 0x0000370C File Offset: 0x0000190C
		[DebuggerNonUserCode]
		public Ping()
		{
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00005B19 File Offset: 0x00003D19
		[DebuggerNonUserCode]
		public Ping(Ping other)
			: this()
		{
			this.sequence_ = other.sequence_;
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00005B3E File Offset: 0x00003D3E
		[DebuggerNonUserCode]
		public Ping Clone()
		{
			return new Ping(this);
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000067 RID: 103 RVA: 0x00005B46 File Offset: 0x00003D46
		// (set) Token: 0x06000068 RID: 104 RVA: 0x00005B4E File Offset: 0x00003D4E
		[DebuggerNonUserCode]
		public int Sequence
		{
			get
			{
				return this.sequence_;
			}
			set
			{
				this.sequence_ = value;
			}
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00005B57 File Offset: 0x00003D57
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as Ping);
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00005B65 File Offset: 0x00003D65
		[DebuggerNonUserCode]
		public bool Equals(Ping other)
		{
			return other != null && (other == this || (this.Sequence == other.Sequence && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00005B94 File Offset: 0x00003D94
		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (this.Sequence != 0)
			{
				num ^= this.Sequence.GetHashCode();
			}
			if (this._unknownFields != null)
			{
				num ^= this._unknownFields.GetHashCode();
			}
			return num;
		}

		// Token: 0x0600006C RID: 108 RVA: 0x0000460F File Offset: 0x0000280F
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00005BD3 File Offset: 0x00003DD3
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.Sequence != 0)
			{
				output.WriteRawTag(8);
				output.WriteInt32(this.Sequence);
			}
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00005C04 File Offset: 0x00003E04
		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Sequence != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Sequence);
			}
			if (this._unknownFields != null)
			{
				num += this._unknownFields.CalculateSize();
			}
			return num;
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00005C42 File Offset: 0x00003E42
		[DebuggerNonUserCode]
		public void MergeFrom(Ping other)
		{
			if (other == null)
			{
				return;
			}
			if (other.Sequence != 0)
			{
				this.Sequence = other.Sequence;
			}
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00005C74 File Offset: 0x00003E74
		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num != 8U)
				{
					this._unknownFields = UnknownFieldSet.MergeFieldFrom(this._unknownFields, input);
				}
				else
				{
					this.Sequence = input.ReadInt32();
				}
			}
		}

		// Token: 0x04000055 RID: 85
		private static readonly MessageParser<Ping> _parser = new MessageParser<Ping>(() => new Ping());

		// Token: 0x04000056 RID: 86
		private UnknownFieldSet _unknownFields;

		// Token: 0x04000057 RID: 87
		public const int SequenceFieldNumber = 1;

		// Token: 0x04000058 RID: 88
		private int sequence_;
	}
}
