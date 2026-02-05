using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Com.Netvios.Proto.Inbound
{
	// Token: 0x02000036 RID: 54
	public sealed class ModifyNickname : IMessage<ModifyNickname>, IMessage, IEquatable<ModifyNickname>, IDeepCloneable<ModifyNickname>
	{
		// Token: 0x17000177 RID: 375
		// (get) Token: 0x06000479 RID: 1145 RVA: 0x000141A1 File Offset: 0x000123A1
		[DebuggerNonUserCode]
		public static MessageParser<ModifyNickname> Parser
		{
			get
			{
				return ModifyNickname._parser;
			}
		}

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x0600047A RID: 1146 RVA: 0x000141A8 File Offset: 0x000123A8
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return BeatSaberInboundMessageReflection.Descriptor.MessageTypes[6];
			}
		}

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x0600047B RID: 1147 RVA: 0x000141BA File Offset: 0x000123BA
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return ModifyNickname.Descriptor;
			}
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x000141C1 File Offset: 0x000123C1
		[DebuggerNonUserCode]
		public ModifyNickname()
		{
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x000141D4 File Offset: 0x000123D4
		[DebuggerNonUserCode]
		public ModifyNickname(ModifyNickname other)
			: this()
		{
			this.nickname_ = other.nickname_;
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x000141F9 File Offset: 0x000123F9
		[DebuggerNonUserCode]
		public ModifyNickname Clone()
		{
			return new ModifyNickname(this);
		}

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x0600047F RID: 1151 RVA: 0x00014201 File Offset: 0x00012401
		// (set) Token: 0x06000480 RID: 1152 RVA: 0x00014209 File Offset: 0x00012409
		[DebuggerNonUserCode]
		public string Nickname
		{
			get
			{
				return this.nickname_;
			}
			set
			{
				this.nickname_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x0001421C File Offset: 0x0001241C
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as ModifyNickname);
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x0001422A File Offset: 0x0001242A
		[DebuggerNonUserCode]
		public bool Equals(ModifyNickname other)
		{
			return other != null && (other == this || (!(this.Nickname != other.Nickname) && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x00014260 File Offset: 0x00012460
		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (this.Nickname.Length != 0)
			{
				num ^= this.Nickname.GetHashCode();
			}
			if (this._unknownFields != null)
			{
				num ^= this._unknownFields.GetHashCode();
			}
			return num;
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x0000460F File Offset: 0x0000280F
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x06000485 RID: 1157 RVA: 0x000142A1 File Offset: 0x000124A1
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.Nickname.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteString(this.Nickname);
			}
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x000142D8 File Offset: 0x000124D8
		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Nickname.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Nickname);
			}
			if (this._unknownFields != null)
			{
				num += this._unknownFields.CalculateSize();
			}
			return num;
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x0001431B File Offset: 0x0001251B
		[DebuggerNonUserCode]
		public void MergeFrom(ModifyNickname other)
		{
			if (other == null)
			{
				return;
			}
			if (other.Nickname.Length != 0)
			{
				this.Nickname = other.Nickname;
			}
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x00014354 File Offset: 0x00012554
		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num != 10U)
				{
					this._unknownFields = UnknownFieldSet.MergeFieldFrom(this._unknownFields, input);
				}
				else
				{
					this.Nickname = input.ReadString();
				}
			}
		}

		// Token: 0x04000237 RID: 567
		private static readonly MessageParser<ModifyNickname> _parser = new MessageParser<ModifyNickname>(() => new ModifyNickname());

		// Token: 0x04000238 RID: 568
		private UnknownFieldSet _unknownFields;

		// Token: 0x04000239 RID: 569
		public const int NicknameFieldNumber = 1;

		// Token: 0x0400023A RID: 570
		private string nickname_ = "";
	}
}
