using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Com.Netvios.Proto.Inbound
{
	// Token: 0x0200003E RID: 62
	public sealed class ChangeRoomOwner : IMessage<ChangeRoomOwner>, IMessage, IEquatable<ChangeRoomOwner>, IDeepCloneable<ChangeRoomOwner>
	{
		// Token: 0x1700019C RID: 412
		// (get) Token: 0x0600050B RID: 1291 RVA: 0x0001563A File Offset: 0x0001383A
		[DebuggerNonUserCode]
		public static MessageParser<ChangeRoomOwner> Parser
		{
			get
			{
				return ChangeRoomOwner._parser;
			}
		}

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x0600050C RID: 1292 RVA: 0x00015641 File Offset: 0x00013841
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return BeatSaberInboundMessageReflection.Descriptor.MessageTypes[14];
			}
		}

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x0600050D RID: 1293 RVA: 0x00015654 File Offset: 0x00013854
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return ChangeRoomOwner.Descriptor;
			}
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x0001565B File Offset: 0x0001385B
		[DebuggerNonUserCode]
		public ChangeRoomOwner()
		{
		}

		// Token: 0x0600050F RID: 1295 RVA: 0x0001566E File Offset: 0x0001386E
		[DebuggerNonUserCode]
		public ChangeRoomOwner(ChangeRoomOwner other)
			: this()
		{
			this.roomId_ = other.roomId_;
			this.targetPlayerId_ = other.targetPlayerId_;
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x06000510 RID: 1296 RVA: 0x0001569F File Offset: 0x0001389F
		[DebuggerNonUserCode]
		public ChangeRoomOwner Clone()
		{
			return new ChangeRoomOwner(this);
		}

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x06000511 RID: 1297 RVA: 0x000156A7 File Offset: 0x000138A7
		// (set) Token: 0x06000512 RID: 1298 RVA: 0x000156AF File Offset: 0x000138AF
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

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x06000513 RID: 1299 RVA: 0x000156C2 File Offset: 0x000138C2
		// (set) Token: 0x06000514 RID: 1300 RVA: 0x000156CA File Offset: 0x000138CA
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

		// Token: 0x06000515 RID: 1301 RVA: 0x000156D3 File Offset: 0x000138D3
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as ChangeRoomOwner);
		}

		// Token: 0x06000516 RID: 1302 RVA: 0x000156E4 File Offset: 0x000138E4
		[DebuggerNonUserCode]
		public bool Equals(ChangeRoomOwner other)
		{
			return other != null && (other == this || (!(this.RoomId != other.RoomId) && this.TargetPlayerId == other.TargetPlayerId && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x06000517 RID: 1303 RVA: 0x00015734 File Offset: 0x00013934
		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (this.RoomId.Length != 0)
			{
				num ^= this.RoomId.GetHashCode();
			}
			if (this.TargetPlayerId != 0L)
			{
				num ^= this.TargetPlayerId.GetHashCode();
			}
			if (this._unknownFields != null)
			{
				num ^= this._unknownFields.GetHashCode();
			}
			return num;
		}

		// Token: 0x06000518 RID: 1304 RVA: 0x0000460F File Offset: 0x0000280F
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x06000519 RID: 1305 RVA: 0x00015790 File Offset: 0x00013990
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.RoomId.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteString(this.RoomId);
			}
			if (this.TargetPlayerId != 0L)
			{
				output.WriteRawTag(16);
				output.WriteInt64(this.TargetPlayerId);
			}
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x0600051A RID: 1306 RVA: 0x000157F0 File Offset: 0x000139F0
		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.RoomId.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.RoomId);
			}
			if (this.TargetPlayerId != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.TargetPlayerId);
			}
			if (this._unknownFields != null)
			{
				num += this._unknownFields.CalculateSize();
			}
			return num;
		}

		// Token: 0x0600051B RID: 1307 RVA: 0x0001584C File Offset: 0x00013A4C
		[DebuggerNonUserCode]
		public void MergeFrom(ChangeRoomOwner other)
		{
			if (other == null)
			{
				return;
			}
			if (other.RoomId.Length != 0)
			{
				this.RoomId = other.RoomId;
			}
			if (other.TargetPlayerId != 0L)
			{
				this.TargetPlayerId = other.TargetPlayerId;
			}
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x000158A4 File Offset: 0x00013AA4
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
						this._unknownFields = UnknownFieldSet.MergeFieldFrom(this._unknownFields, input);
					}
					else
					{
						this.TargetPlayerId = input.ReadInt64();
					}
				}
				else
				{
					this.RoomId = input.ReadString();
				}
			}
		}

		// Token: 0x04000261 RID: 609
		private static readonly MessageParser<ChangeRoomOwner> _parser = new MessageParser<ChangeRoomOwner>(() => new ChangeRoomOwner());

		// Token: 0x04000262 RID: 610
		private UnknownFieldSet _unknownFields;

		// Token: 0x04000263 RID: 611
		public const int RoomIdFieldNumber = 1;

		// Token: 0x04000264 RID: 612
		private string roomId_ = "";

		// Token: 0x04000265 RID: 613
		public const int TargetPlayerIdFieldNumber = 2;

		// Token: 0x04000266 RID: 614
		private long targetPlayerId_;
	}
}
