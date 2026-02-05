using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Com.Netvios.Proto.Outbound
{
	// Token: 0x0200001F RID: 31
	public sealed class KickedOutNotice : IMessage<KickedOutNotice>, IMessage, IEquatable<KickedOutNotice>, IDeepCloneable<KickedOutNotice>
	{
		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x0600025D RID: 605 RVA: 0x0000BE75 File Offset: 0x0000A075
		[DebuggerNonUserCode]
		public static MessageParser<KickedOutNotice> Parser
		{
			get
			{
				return KickedOutNotice._parser;
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x0600025E RID: 606 RVA: 0x0000BE7C File Offset: 0x0000A07C
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return BeatSaberOutboundMessageReflection.Descriptor.MessageTypes[23];
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x0600025F RID: 607 RVA: 0x0000BE8F File Offset: 0x0000A08F
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return KickedOutNotice.Descriptor;
			}
		}

		// Token: 0x06000260 RID: 608 RVA: 0x0000BE96 File Offset: 0x0000A096
		[DebuggerNonUserCode]
		public KickedOutNotice()
		{
		}

		// Token: 0x06000261 RID: 609 RVA: 0x0000BEA9 File Offset: 0x0000A0A9
		[DebuggerNonUserCode]
		public KickedOutNotice(KickedOutNotice other)
			: this()
		{
			this.targetPlayerId_ = other.targetPlayerId_;
			this.message_ = other.message_;
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x06000262 RID: 610 RVA: 0x0000BEDA File Offset: 0x0000A0DA
		[DebuggerNonUserCode]
		public KickedOutNotice Clone()
		{
			return new KickedOutNotice(this);
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000263 RID: 611 RVA: 0x0000BEE2 File Offset: 0x0000A0E2
		// (set) Token: 0x06000264 RID: 612 RVA: 0x0000BEEA File Offset: 0x0000A0EA
		[DebuggerNonUserCode]
		public long TargetPlayerId
		{
			get
			{
				return this.targetPlayerId_;
			}
			set
			{
				this.targetPlayerId_ = value;
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000265 RID: 613 RVA: 0x0000BEF3 File Offset: 0x0000A0F3
		// (set) Token: 0x06000266 RID: 614 RVA: 0x0000BEFB File Offset: 0x0000A0FB
		[DebuggerNonUserCode]
		public string Message
		{
			get
			{
				return this.message_;
			}
			set
			{
				this.message_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		// Token: 0x06000267 RID: 615 RVA: 0x0000BF0E File Offset: 0x0000A10E
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as KickedOutNotice);
		}

		// Token: 0x06000268 RID: 616 RVA: 0x0000BF1C File Offset: 0x0000A11C
		[DebuggerNonUserCode]
		public bool Equals(KickedOutNotice other)
		{
			return other != null && (other == this || (this.TargetPlayerId == other.TargetPlayerId && !(this.Message != other.Message) && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x06000269 RID: 617 RVA: 0x0000BF6C File Offset: 0x0000A16C
		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (this.TargetPlayerId != 0L)
			{
				num ^= this.TargetPlayerId.GetHashCode();
			}
			if (this.Message.Length != 0)
			{
				num ^= this.Message.GetHashCode();
			}
			if (this._unknownFields != null)
			{
				num ^= this._unknownFields.GetHashCode();
			}
			return num;
		}

		// Token: 0x0600026A RID: 618 RVA: 0x0000460F File Offset: 0x0000280F
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x0600026B RID: 619 RVA: 0x0000BFC8 File Offset: 0x0000A1C8
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.TargetPlayerId != 0L)
			{
				output.WriteRawTag(8);
				output.WriteInt64(this.TargetPlayerId);
			}
			if (this.Message.Length != 0)
			{
				output.WriteRawTag(18);
				output.WriteString(this.Message);
			}
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x0600026C RID: 620 RVA: 0x0000C028 File Offset: 0x0000A228
		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.TargetPlayerId != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.TargetPlayerId);
			}
			if (this.Message.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Message);
			}
			if (this._unknownFields != null)
			{
				num += this._unknownFields.CalculateSize();
			}
			return num;
		}

		// Token: 0x0600026D RID: 621 RVA: 0x0000C084 File Offset: 0x0000A284
		[DebuggerNonUserCode]
		public void MergeFrom(KickedOutNotice other)
		{
			if (other == null)
			{
				return;
			}
			if (other.TargetPlayerId != 0L)
			{
				this.TargetPlayerId = other.TargetPlayerId;
			}
			if (other.Message.Length != 0)
			{
				this.Message = other.Message;
			}
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x0600026E RID: 622 RVA: 0x0000C0DC File Offset: 0x0000A2DC
		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num != 8U)
				{
					if (num != 18U)
					{
						this._unknownFields = UnknownFieldSet.MergeFieldFrom(this._unknownFields, input);
					}
					else
					{
						this.Message = input.ReadString();
					}
				}
				else
				{
					this.TargetPlayerId = input.ReadInt64();
				}
			}
		}

		// Token: 0x0400013F RID: 319
		private static readonly MessageParser<KickedOutNotice> _parser = new MessageParser<KickedOutNotice>(() => new KickedOutNotice());

		// Token: 0x04000140 RID: 320
		private UnknownFieldSet _unknownFields;

		// Token: 0x04000141 RID: 321
		public const int TargetPlayerIdFieldNumber = 1;

		// Token: 0x04000142 RID: 322
		private long targetPlayerId_;

		// Token: 0x04000143 RID: 323
		public const int MessageFieldNumber = 2;

		// Token: 0x04000144 RID: 324
		private string message_ = "";
	}
}
