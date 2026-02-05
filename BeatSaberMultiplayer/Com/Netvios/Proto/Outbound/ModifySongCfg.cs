using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Com.Netvios.Proto.Outbound
{
	// Token: 0x02000019 RID: 25
	public sealed class ModifySongCfg : IMessage<ModifySongCfg>, IMessage, IEquatable<ModifySongCfg>, IDeepCloneable<ModifySongCfg>
	{
		// Token: 0x1700009D RID: 157
		// (get) Token: 0x060001D7 RID: 471 RVA: 0x0000A544 File Offset: 0x00008744
		[DebuggerNonUserCode]
		public static MessageParser<ModifySongCfg> Parser
		{
			get
			{
				return ModifySongCfg._parser;
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x060001D8 RID: 472 RVA: 0x0000A54B File Offset: 0x0000874B
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return BeatSaberOutboundMessageReflection.Descriptor.MessageTypes[17];
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x060001D9 RID: 473 RVA: 0x0000A55E File Offset: 0x0000875E
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return ModifySongCfg.Descriptor;
			}
		}

		// Token: 0x060001DA RID: 474 RVA: 0x0000A565 File Offset: 0x00008765
		[DebuggerNonUserCode]
		public ModifySongCfg()
		{
		}

		// Token: 0x060001DB RID: 475 RVA: 0x0000A583 File Offset: 0x00008783
		[DebuggerNonUserCode]
		public ModifySongCfg(ModifySongCfg other)
			: this()
		{
			this.roomId_ = other.roomId_;
			this.success_ = other.success_;
			this.message_ = other.message_;
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x060001DC RID: 476 RVA: 0x0000A5C0 File Offset: 0x000087C0
		[DebuggerNonUserCode]
		public ModifySongCfg Clone()
		{
			return new ModifySongCfg(this);
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x060001DD RID: 477 RVA: 0x0000A5C8 File Offset: 0x000087C8
		// (set) Token: 0x060001DE RID: 478 RVA: 0x0000A5D0 File Offset: 0x000087D0
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

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x060001DF RID: 479 RVA: 0x0000A5E3 File Offset: 0x000087E3
		// (set) Token: 0x060001E0 RID: 480 RVA: 0x0000A5EB File Offset: 0x000087EB
		[DebuggerNonUserCode]
		public bool Success
		{
			get
			{
				return this.success_;
			}
			set
			{
				this.success_ = value;
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x060001E1 RID: 481 RVA: 0x0000A5F4 File Offset: 0x000087F4
		// (set) Token: 0x060001E2 RID: 482 RVA: 0x0000A5FC File Offset: 0x000087FC
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

		// Token: 0x060001E3 RID: 483 RVA: 0x0000A60F File Offset: 0x0000880F
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as ModifySongCfg);
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x0000A620 File Offset: 0x00008820
		[DebuggerNonUserCode]
		public bool Equals(ModifySongCfg other)
		{
			return other != null && (other == this || (!(this.RoomId != other.RoomId) && this.Success == other.Success && !(this.Message != other.Message) && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x0000A684 File Offset: 0x00008884
		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (this.RoomId.Length != 0)
			{
				num ^= this.RoomId.GetHashCode();
			}
			if (this.Success)
			{
				num ^= this.Success.GetHashCode();
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

		// Token: 0x060001E6 RID: 486 RVA: 0x0000460F File Offset: 0x0000280F
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x0000A6FC File Offset: 0x000088FC
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.RoomId.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteString(this.RoomId);
			}
			if (this.Success)
			{
				output.WriteRawTag(16);
				output.WriteBool(this.Success);
			}
			if (this.Message.Length != 0)
			{
				output.WriteRawTag(26);
				output.WriteString(this.Message);
			}
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x0000A77C File Offset: 0x0000897C
		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.RoomId.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.RoomId);
			}
			if (this.Success)
			{
				num += 2;
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

		// Token: 0x060001E9 RID: 489 RVA: 0x0000A7E8 File Offset: 0x000089E8
		[DebuggerNonUserCode]
		public void MergeFrom(ModifySongCfg other)
		{
			if (other == null)
			{
				return;
			}
			if (other.RoomId.Length != 0)
			{
				this.RoomId = other.RoomId;
			}
			if (other.Success)
			{
				this.Success = other.Success;
			}
			if (other.Message.Length != 0)
			{
				this.Message = other.Message;
			}
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x060001EA RID: 490 RVA: 0x0000A858 File Offset: 0x00008A58
		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num != 10U)
				{
					if (num != 16U)
					{
						if (num != 26U)
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
						this.Success = input.ReadBool();
					}
				}
				else
				{
					this.RoomId = input.ReadString();
				}
			}
		}

		// Token: 0x04000103 RID: 259
		private static readonly MessageParser<ModifySongCfg> _parser = new MessageParser<ModifySongCfg>(() => new ModifySongCfg());

		// Token: 0x04000104 RID: 260
		private UnknownFieldSet _unknownFields;

		// Token: 0x04000105 RID: 261
		public const int RoomIdFieldNumber = 1;

		// Token: 0x04000106 RID: 262
		private string roomId_ = "";

		// Token: 0x04000107 RID: 263
		public const int SuccessFieldNumber = 2;

		// Token: 0x04000108 RID: 264
		private bool success_;

		// Token: 0x04000109 RID: 265
		public const int MessageFieldNumber = 3;

		// Token: 0x0400010A RID: 266
		private string message_ = "";
	}
}
