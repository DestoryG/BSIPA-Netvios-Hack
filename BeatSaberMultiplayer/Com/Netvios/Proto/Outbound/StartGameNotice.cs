using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Com.Netvios.Proto.Outbound
{
	// Token: 0x02000022 RID: 34
	public sealed class StartGameNotice : IMessage<StartGameNotice>, IMessage, IEquatable<StartGameNotice>, IDeepCloneable<StartGameNotice>
	{
		// Token: 0x170000DD RID: 221
		// (get) Token: 0x060002A5 RID: 677 RVA: 0x0000CD60 File Offset: 0x0000AF60
		[DebuggerNonUserCode]
		public static MessageParser<StartGameNotice> Parser
		{
			get
			{
				return StartGameNotice._parser;
			}
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x060002A6 RID: 678 RVA: 0x0000CD67 File Offset: 0x0000AF67
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return BeatSaberOutboundMessageReflection.Descriptor.MessageTypes[26];
			}
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x060002A7 RID: 679 RVA: 0x0000CD7A File Offset: 0x0000AF7A
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return StartGameNotice.Descriptor;
			}
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x0000CD81 File Offset: 0x0000AF81
		[DebuggerNonUserCode]
		public StartGameNotice()
		{
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x0000CD94 File Offset: 0x0000AF94
		[DebuggerNonUserCode]
		public StartGameNotice(StartGameNotice other)
			: this()
		{
			this.roomId_ = other.roomId_;
			this.songCfg_ = ((other.songCfg_ != null) ? other.songCfg_.Clone() : null);
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x060002AA RID: 682 RVA: 0x0000CDE0 File Offset: 0x0000AFE0
		[DebuggerNonUserCode]
		public StartGameNotice Clone()
		{
			return new StartGameNotice(this);
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x060002AB RID: 683 RVA: 0x0000CDE8 File Offset: 0x0000AFE8
		// (set) Token: 0x060002AC RID: 684 RVA: 0x0000CDF0 File Offset: 0x0000AFF0
		[DebuggerNonUserCode]
		public string RoomId
		{
			get
			{
				return this.roomId_;
			}
			set
			{
				this.roomId_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x060002AD RID: 685 RVA: 0x0000CE03 File Offset: 0x0000B003
		// (set) Token: 0x060002AE RID: 686 RVA: 0x0000CE0B File Offset: 0x0000B00B
		[DebuggerNonUserCode]
		public SongCfg SongCfg
		{
			get
			{
				return this.songCfg_;
			}
			set
			{
				this.songCfg_ = value;
			}
		}

		// Token: 0x060002AF RID: 687 RVA: 0x0000CE14 File Offset: 0x0000B014
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as StartGameNotice);
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x0000CE24 File Offset: 0x0000B024
		[DebuggerNonUserCode]
		public bool Equals(StartGameNotice other)
		{
			return other != null && (other == this || (!(this.RoomId != other.RoomId) && object.Equals(this.SongCfg, other.SongCfg) && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x0000CE78 File Offset: 0x0000B078
		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (this.RoomId.Length != 0)
			{
				num ^= this.RoomId.GetHashCode();
			}
			if (this.songCfg_ != null)
			{
				num ^= this.SongCfg.GetHashCode();
			}
			if (this._unknownFields != null)
			{
				num ^= this._unknownFields.GetHashCode();
			}
			return num;
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x0000460F File Offset: 0x0000280F
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x0000CED0 File Offset: 0x0000B0D0
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.RoomId.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteString(this.RoomId);
			}
			if (this.songCfg_ != null)
			{
				output.WriteRawTag(42);
				output.WriteMessage(this.SongCfg);
			}
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x0000CF30 File Offset: 0x0000B130
		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.RoomId.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.RoomId);
			}
			if (this.songCfg_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.SongCfg);
			}
			if (this._unknownFields != null)
			{
				num += this._unknownFields.CalculateSize();
			}
			return num;
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x0000CF8C File Offset: 0x0000B18C
		[DebuggerNonUserCode]
		public void MergeFrom(StartGameNotice other)
		{
			if (other == null)
			{
				return;
			}
			if (other.RoomId.Length != 0)
			{
				this.RoomId = other.RoomId;
			}
			if (other.songCfg_ != null)
			{
				if (this.songCfg_ == null)
				{
					this.SongCfg = new SongCfg();
				}
				this.SongCfg.MergeFrom(other.SongCfg);
			}
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x0000CFFC File Offset: 0x0000B1FC
		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num != 10U)
				{
					if (num != 42U)
					{
						this._unknownFields = UnknownFieldSet.MergeFieldFrom(this._unknownFields, input);
					}
					else
					{
						if (this.songCfg_ == null)
						{
							this.SongCfg = new SongCfg();
						}
						input.ReadMessage(this.SongCfg);
					}
				}
				else
				{
					this.RoomId = input.ReadString();
				}
			}
		}

		// Token: 0x04000162 RID: 354
		private static readonly MessageParser<StartGameNotice> _parser = new MessageParser<StartGameNotice>(() => new StartGameNotice());

		// Token: 0x04000163 RID: 355
		private UnknownFieldSet _unknownFields;

		// Token: 0x04000164 RID: 356
		public const int RoomIdFieldNumber = 1;

		// Token: 0x04000165 RID: 357
		private string roomId_ = "";

		// Token: 0x04000166 RID: 358
		public const int SongCfgFieldNumber = 5;

		// Token: 0x04000167 RID: 359
		private SongCfg songCfg_;
	}
}
