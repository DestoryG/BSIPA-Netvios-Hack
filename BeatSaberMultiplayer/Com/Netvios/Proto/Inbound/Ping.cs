using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Com.Netvios.Proto.Inbound
{
	// Token: 0x02000031 RID: 49
	public sealed class Ping : IMessage<Ping>, IMessage, IEquatable<Ping>, IDeepCloneable<Ping>
	{
		// Token: 0x17000163 RID: 355
		// (get) Token: 0x06000424 RID: 1060 RVA: 0x000137A9 File Offset: 0x000119A9
		[DebuggerNonUserCode]
		public static MessageParser<Ping> Parser
		{
			get
			{
				return Ping._parser;
			}
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x06000425 RID: 1061 RVA: 0x000137B0 File Offset: 0x000119B0
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return BeatSaberInboundMessageReflection.Descriptor.MessageTypes[1];
			}
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x06000426 RID: 1062 RVA: 0x000137C2 File Offset: 0x000119C2
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return Ping.Descriptor;
			}
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x0000370C File Offset: 0x0000190C
		[DebuggerNonUserCode]
		public Ping()
		{
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x000137C9 File Offset: 0x000119C9
		[DebuggerNonUserCode]
		public Ping(Ping other)
			: this()
		{
			this.sequence_ = other.sequence_;
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x000137EE File Offset: 0x000119EE
		[DebuggerNonUserCode]
		public Ping Clone()
		{
			return new Ping(this);
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x0600042A RID: 1066 RVA: 0x000137F6 File Offset: 0x000119F6
		// (set) Token: 0x0600042B RID: 1067 RVA: 0x000137FE File Offset: 0x000119FE
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

		// Token: 0x0600042C RID: 1068 RVA: 0x00013807 File Offset: 0x00011A07
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as Ping);
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x00013815 File Offset: 0x00011A15
		[DebuggerNonUserCode]
		public bool Equals(Ping other)
		{
			return other != null && (other == this || (this.Sequence == other.Sequence && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x00013844 File Offset: 0x00011A44
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

		// Token: 0x0600042F RID: 1071 RVA: 0x0000460F File Offset: 0x0000280F
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x00013883 File Offset: 0x00011A83
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

		// Token: 0x06000431 RID: 1073 RVA: 0x000138B4 File Offset: 0x00011AB4
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

		// Token: 0x06000432 RID: 1074 RVA: 0x000138F2 File Offset: 0x00011AF2
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

		// Token: 0x06000433 RID: 1075 RVA: 0x00013924 File Offset: 0x00011B24
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

		// Token: 0x04000223 RID: 547
		private static readonly MessageParser<Ping> _parser = new MessageParser<Ping>(() => new Ping());

		// Token: 0x04000224 RID: 548
		private UnknownFieldSet _unknownFields;

		// Token: 0x04000225 RID: 549
		public const int SequenceFieldNumber = 1;

		// Token: 0x04000226 RID: 550
		private int sequence_;
	}
}
