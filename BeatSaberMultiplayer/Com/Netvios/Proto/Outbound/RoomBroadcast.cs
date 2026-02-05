using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Com.Netvios.Proto.Outbound
{
	// Token: 0x0200001B RID: 27
	public sealed class RoomBroadcast : IMessage<RoomBroadcast>, IMessage, IEquatable<RoomBroadcast>, IDeepCloneable<RoomBroadcast>
	{
		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000201 RID: 513 RVA: 0x0000AC6C File Offset: 0x00008E6C
		[DebuggerNonUserCode]
		public static MessageParser<RoomBroadcast> Parser
		{
			get
			{
				return RoomBroadcast._parser;
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000202 RID: 514 RVA: 0x0000AC73 File Offset: 0x00008E73
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return BeatSaberOutboundMessageReflection.Descriptor.MessageTypes[19];
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000203 RID: 515 RVA: 0x0000AC86 File Offset: 0x00008E86
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return RoomBroadcast.Descriptor;
			}
		}

		// Token: 0x06000204 RID: 516 RVA: 0x0000AC8D File Offset: 0x00008E8D
		[DebuggerNonUserCode]
		public RoomBroadcast()
		{
		}

		// Token: 0x06000205 RID: 517 RVA: 0x0000ACB6 File Offset: 0x00008EB6
		[DebuggerNonUserCode]
		public RoomBroadcast(RoomBroadcast other)
			: this()
		{
			this.roomId_ = other.roomId_;
			this.type_ = other.type_;
			this.content_ = other.content_;
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x06000206 RID: 518 RVA: 0x0000ACF3 File Offset: 0x00008EF3
		[DebuggerNonUserCode]
		public RoomBroadcast Clone()
		{
			return new RoomBroadcast(this);
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06000207 RID: 519 RVA: 0x0000ACFB File Offset: 0x00008EFB
		// (set) Token: 0x06000208 RID: 520 RVA: 0x0000AD03 File Offset: 0x00008F03
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

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x06000209 RID: 521 RVA: 0x0000AD16 File Offset: 0x00008F16
		// (set) Token: 0x0600020A RID: 522 RVA: 0x0000AD1E File Offset: 0x00008F1E
		[DebuggerNonUserCode]
		public string Type
		{
			get
			{
				return this.type_;
			}
			set
			{
				this.type_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x0600020B RID: 523 RVA: 0x0000AD31 File Offset: 0x00008F31
		// (set) Token: 0x0600020C RID: 524 RVA: 0x0000AD39 File Offset: 0x00008F39
		[DebuggerNonUserCode]
		public string Content
		{
			get
			{
				return this.content_;
			}
			set
			{
				this.content_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		// Token: 0x0600020D RID: 525 RVA: 0x0000AD4C File Offset: 0x00008F4C
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as RoomBroadcast);
		}

		// Token: 0x0600020E RID: 526 RVA: 0x0000AD5C File Offset: 0x00008F5C
		[DebuggerNonUserCode]
		public bool Equals(RoomBroadcast other)
		{
			return other != null && (other == this || (!(this.RoomId != other.RoomId) && !(this.Type != other.Type) && !(this.Content != other.Content) && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x0600020F RID: 527 RVA: 0x0000ADC4 File Offset: 0x00008FC4
		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (this.RoomId.Length != 0)
			{
				num ^= this.RoomId.GetHashCode();
			}
			if (this.Type.Length != 0)
			{
				num ^= this.Type.GetHashCode();
			}
			if (this.Content.Length != 0)
			{
				num ^= this.Content.GetHashCode();
			}
			if (this._unknownFields != null)
			{
				num ^= this._unknownFields.GetHashCode();
			}
			return num;
		}

		// Token: 0x06000210 RID: 528 RVA: 0x0000460F File Offset: 0x0000280F
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x06000211 RID: 529 RVA: 0x0000AE3C File Offset: 0x0000903C
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.RoomId.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteString(this.RoomId);
			}
			if (this.Type.Length != 0)
			{
				output.WriteRawTag(18);
				output.WriteString(this.Type);
			}
			if (this.Content.Length != 0)
			{
				output.WriteRawTag(26);
				output.WriteString(this.Content);
			}
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x06000212 RID: 530 RVA: 0x0000AEC0 File Offset: 0x000090C0
		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.RoomId.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.RoomId);
			}
			if (this.Type.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Type);
			}
			if (this.Content.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Content);
			}
			if (this._unknownFields != null)
			{
				num += this._unknownFields.CalculateSize();
			}
			return num;
		}

		// Token: 0x06000213 RID: 531 RVA: 0x0000AF40 File Offset: 0x00009140
		[DebuggerNonUserCode]
		public void MergeFrom(RoomBroadcast other)
		{
			if (other == null)
			{
				return;
			}
			if (other.RoomId.Length != 0)
			{
				this.RoomId = other.RoomId;
			}
			if (other.Type.Length != 0)
			{
				this.Type = other.Type;
			}
			if (other.Content.Length != 0)
			{
				this.Content = other.Content;
			}
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x06000214 RID: 532 RVA: 0x0000AFB4 File Offset: 0x000091B4
		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num != 10U)
				{
					if (num != 18U)
					{
						if (num != 26U)
						{
							this._unknownFields = UnknownFieldSet.MergeFieldFrom(this._unknownFields, input);
						}
						else
						{
							this.Content = input.ReadString();
						}
					}
					else
					{
						this.Type = input.ReadString();
					}
				}
				else
				{
					this.RoomId = input.ReadString();
				}
			}
		}

		// Token: 0x04000113 RID: 275
		private static readonly MessageParser<RoomBroadcast> _parser = new MessageParser<RoomBroadcast>(() => new RoomBroadcast());

		// Token: 0x04000114 RID: 276
		private UnknownFieldSet _unknownFields;

		// Token: 0x04000115 RID: 277
		public const int RoomIdFieldNumber = 1;

		// Token: 0x04000116 RID: 278
		private string roomId_ = "";

		// Token: 0x04000117 RID: 279
		public const int TypeFieldNumber = 2;

		// Token: 0x04000118 RID: 280
		private string type_ = "";

		// Token: 0x04000119 RID: 281
		public const int ContentFieldNumber = 3;

		// Token: 0x0400011A RID: 282
		private string content_ = "";
	}
}
