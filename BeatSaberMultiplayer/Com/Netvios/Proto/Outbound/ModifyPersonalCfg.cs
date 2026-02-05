using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Com.Netvios.Proto.Outbound
{
	// Token: 0x02000017 RID: 23
	public sealed class ModifyPersonalCfg : IMessage<ModifyPersonalCfg>, IMessage, IEquatable<ModifyPersonalCfg>, IDeepCloneable<ModifyPersonalCfg>
	{
		// Token: 0x17000091 RID: 145
		// (get) Token: 0x060001AD RID: 429 RVA: 0x00009E1E File Offset: 0x0000801E
		[DebuggerNonUserCode]
		public static MessageParser<ModifyPersonalCfg> Parser
		{
			get
			{
				return ModifyPersonalCfg._parser;
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x060001AE RID: 430 RVA: 0x00009E25 File Offset: 0x00008025
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return BeatSaberOutboundMessageReflection.Descriptor.MessageTypes[15];
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x060001AF RID: 431 RVA: 0x00009E38 File Offset: 0x00008038
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return ModifyPersonalCfg.Descriptor;
			}
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x00009E3F File Offset: 0x0000803F
		[DebuggerNonUserCode]
		public ModifyPersonalCfg()
		{
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x00009E5D File Offset: 0x0000805D
		[DebuggerNonUserCode]
		public ModifyPersonalCfg(ModifyPersonalCfg other)
			: this()
		{
			this.roomId_ = other.roomId_;
			this.success_ = other.success_;
			this.message_ = other.message_;
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x00009E9A File Offset: 0x0000809A
		[DebuggerNonUserCode]
		public ModifyPersonalCfg Clone()
		{
			return new ModifyPersonalCfg(this);
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x060001B3 RID: 435 RVA: 0x00009EA2 File Offset: 0x000080A2
		// (set) Token: 0x060001B4 RID: 436 RVA: 0x00009EAA File Offset: 0x000080AA
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

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x060001B5 RID: 437 RVA: 0x00009EBD File Offset: 0x000080BD
		// (set) Token: 0x060001B6 RID: 438 RVA: 0x00009EC5 File Offset: 0x000080C5
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

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x060001B7 RID: 439 RVA: 0x00009ECE File Offset: 0x000080CE
		// (set) Token: 0x060001B8 RID: 440 RVA: 0x00009ED6 File Offset: 0x000080D6
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

		// Token: 0x060001B9 RID: 441 RVA: 0x00009EE9 File Offset: 0x000080E9
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as ModifyPersonalCfg);
		}

		// Token: 0x060001BA RID: 442 RVA: 0x00009EF8 File Offset: 0x000080F8
		[DebuggerNonUserCode]
		public bool Equals(ModifyPersonalCfg other)
		{
			return other != null && (other == this || (!(this.RoomId != other.RoomId) && this.Success == other.Success && !(this.Message != other.Message) && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x060001BB RID: 443 RVA: 0x00009F5C File Offset: 0x0000815C
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

		// Token: 0x060001BC RID: 444 RVA: 0x0000460F File Offset: 0x0000280F
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x060001BD RID: 445 RVA: 0x00009FD4 File Offset: 0x000081D4
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

		// Token: 0x060001BE RID: 446 RVA: 0x0000A054 File Offset: 0x00008254
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

		// Token: 0x060001BF RID: 447 RVA: 0x0000A0C0 File Offset: 0x000082C0
		[DebuggerNonUserCode]
		public void MergeFrom(ModifyPersonalCfg other)
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

		// Token: 0x060001C0 RID: 448 RVA: 0x0000A130 File Offset: 0x00008330
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

		// Token: 0x040000F3 RID: 243
		private static readonly MessageParser<ModifyPersonalCfg> _parser = new MessageParser<ModifyPersonalCfg>(() => new ModifyPersonalCfg());

		// Token: 0x040000F4 RID: 244
		private UnknownFieldSet _unknownFields;

		// Token: 0x040000F5 RID: 245
		public const int RoomIdFieldNumber = 1;

		// Token: 0x040000F6 RID: 246
		private string roomId_ = "";

		// Token: 0x040000F7 RID: 247
		public const int SuccessFieldNumber = 2;

		// Token: 0x040000F8 RID: 248
		private bool success_;

		// Token: 0x040000F9 RID: 249
		public const int MessageFieldNumber = 3;

		// Token: 0x040000FA RID: 250
		private string message_ = "";
	}
}
