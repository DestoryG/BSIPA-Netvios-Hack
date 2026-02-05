using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Com.Netvios.Proto.Outbound
{
	// Token: 0x02000018 RID: 24
	public sealed class ModifyRoomCfg : IMessage<ModifyRoomCfg>, IMessage, IEquatable<ModifyRoomCfg>, IDeepCloneable<ModifyRoomCfg>
	{
		// Token: 0x17000097 RID: 151
		// (get) Token: 0x060001C2 RID: 450 RVA: 0x0000A1B0 File Offset: 0x000083B0
		[DebuggerNonUserCode]
		public static MessageParser<ModifyRoomCfg> Parser
		{
			get
			{
				return ModifyRoomCfg._parser;
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x060001C3 RID: 451 RVA: 0x0000A1B7 File Offset: 0x000083B7
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return BeatSaberOutboundMessageReflection.Descriptor.MessageTypes[16];
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x060001C4 RID: 452 RVA: 0x0000A1CA File Offset: 0x000083CA
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return ModifyRoomCfg.Descriptor;
			}
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x0000A1D1 File Offset: 0x000083D1
		[DebuggerNonUserCode]
		public ModifyRoomCfg()
		{
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x0000A1EF File Offset: 0x000083EF
		[DebuggerNonUserCode]
		public ModifyRoomCfg(ModifyRoomCfg other)
			: this()
		{
			this.roomId_ = other.roomId_;
			this.success_ = other.success_;
			this.message_ = other.message_;
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x0000A22C File Offset: 0x0000842C
		[DebuggerNonUserCode]
		public ModifyRoomCfg Clone()
		{
			return new ModifyRoomCfg(this);
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x060001C8 RID: 456 RVA: 0x0000A234 File Offset: 0x00008434
		// (set) Token: 0x060001C9 RID: 457 RVA: 0x0000A23C File Offset: 0x0000843C
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

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x060001CA RID: 458 RVA: 0x0000A24F File Offset: 0x0000844F
		// (set) Token: 0x060001CB RID: 459 RVA: 0x0000A257 File Offset: 0x00008457
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

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x060001CC RID: 460 RVA: 0x0000A260 File Offset: 0x00008460
		// (set) Token: 0x060001CD RID: 461 RVA: 0x0000A268 File Offset: 0x00008468
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

		// Token: 0x060001CE RID: 462 RVA: 0x0000A27B File Offset: 0x0000847B
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as ModifyRoomCfg);
		}

		// Token: 0x060001CF RID: 463 RVA: 0x0000A28C File Offset: 0x0000848C
		[DebuggerNonUserCode]
		public bool Equals(ModifyRoomCfg other)
		{
			return other != null && (other == this || (!(this.RoomId != other.RoomId) && this.Success == other.Success && !(this.Message != other.Message) && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x0000A2F0 File Offset: 0x000084F0
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

		// Token: 0x060001D1 RID: 465 RVA: 0x0000460F File Offset: 0x0000280F
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x0000A368 File Offset: 0x00008568
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

		// Token: 0x060001D3 RID: 467 RVA: 0x0000A3E8 File Offset: 0x000085E8
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

		// Token: 0x060001D4 RID: 468 RVA: 0x0000A454 File Offset: 0x00008654
		[DebuggerNonUserCode]
		public void MergeFrom(ModifyRoomCfg other)
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

		// Token: 0x060001D5 RID: 469 RVA: 0x0000A4C4 File Offset: 0x000086C4
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

		// Token: 0x040000FB RID: 251
		private static readonly MessageParser<ModifyRoomCfg> _parser = new MessageParser<ModifyRoomCfg>(() => new ModifyRoomCfg());

		// Token: 0x040000FC RID: 252
		private UnknownFieldSet _unknownFields;

		// Token: 0x040000FD RID: 253
		public const int RoomIdFieldNumber = 1;

		// Token: 0x040000FE RID: 254
		private string roomId_ = "";

		// Token: 0x040000FF RID: 255
		public const int SuccessFieldNumber = 2;

		// Token: 0x04000100 RID: 256
		private bool success_;

		// Token: 0x04000101 RID: 257
		public const int MessageFieldNumber = 3;

		// Token: 0x04000102 RID: 258
		private string message_ = "";
	}
}
