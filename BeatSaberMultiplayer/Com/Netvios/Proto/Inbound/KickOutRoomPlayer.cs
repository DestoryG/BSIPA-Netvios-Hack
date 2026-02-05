using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Com.Netvios.Proto.Inbound
{
	// Token: 0x0200003C RID: 60
	public sealed class KickOutRoomPlayer : IMessage<KickOutRoomPlayer>, IMessage, IEquatable<KickOutRoomPlayer>, IDeepCloneable<KickOutRoomPlayer>
	{
		// Token: 0x17000193 RID: 403
		// (get) Token: 0x060004E7 RID: 1255 RVA: 0x00015156 File Offset: 0x00013356
		[DebuggerNonUserCode]
		public static MessageParser<KickOutRoomPlayer> Parser
		{
			get
			{
				return KickOutRoomPlayer._parser;
			}
		}

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x060004E8 RID: 1256 RVA: 0x0001515D File Offset: 0x0001335D
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return BeatSaberInboundMessageReflection.Descriptor.MessageTypes[12];
			}
		}

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x060004E9 RID: 1257 RVA: 0x00015170 File Offset: 0x00013370
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return KickOutRoomPlayer.Descriptor;
			}
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x00015177 File Offset: 0x00013377
		[DebuggerNonUserCode]
		public KickOutRoomPlayer()
		{
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x0001518A File Offset: 0x0001338A
		[DebuggerNonUserCode]
		public KickOutRoomPlayer(KickOutRoomPlayer other)
			: this()
		{
			this.roomId_ = other.roomId_;
			this.targetPlayerId_ = other.targetPlayerId_;
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x000151BB File Offset: 0x000133BB
		[DebuggerNonUserCode]
		public KickOutRoomPlayer Clone()
		{
			return new KickOutRoomPlayer(this);
		}

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x060004ED RID: 1261 RVA: 0x000151C3 File Offset: 0x000133C3
		// (set) Token: 0x060004EE RID: 1262 RVA: 0x000151CB File Offset: 0x000133CB
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

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x060004EF RID: 1263 RVA: 0x000151DE File Offset: 0x000133DE
		// (set) Token: 0x060004F0 RID: 1264 RVA: 0x000151E6 File Offset: 0x000133E6
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

		// Token: 0x060004F1 RID: 1265 RVA: 0x000151EF File Offset: 0x000133EF
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as KickOutRoomPlayer);
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x00015200 File Offset: 0x00013400
		[DebuggerNonUserCode]
		public bool Equals(KickOutRoomPlayer other)
		{
			return other != null && (other == this || (!(this.RoomId != other.RoomId) && this.TargetPlayerId == other.TargetPlayerId && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x00015250 File Offset: 0x00013450
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

		// Token: 0x060004F4 RID: 1268 RVA: 0x0000460F File Offset: 0x0000280F
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x000152AC File Offset: 0x000134AC
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

		// Token: 0x060004F6 RID: 1270 RVA: 0x0001530C File Offset: 0x0001350C
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

		// Token: 0x060004F7 RID: 1271 RVA: 0x00015368 File Offset: 0x00013568
		[DebuggerNonUserCode]
		public void MergeFrom(KickOutRoomPlayer other)
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

		// Token: 0x060004F8 RID: 1272 RVA: 0x000153C0 File Offset: 0x000135C0
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

		// Token: 0x04000257 RID: 599
		private static readonly MessageParser<KickOutRoomPlayer> _parser = new MessageParser<KickOutRoomPlayer>(() => new KickOutRoomPlayer());

		// Token: 0x04000258 RID: 600
		private UnknownFieldSet _unknownFields;

		// Token: 0x04000259 RID: 601
		public const int RoomIdFieldNumber = 1;

		// Token: 0x0400025A RID: 602
		private string roomId_ = "";

		// Token: 0x0400025B RID: 603
		public const int TargetPlayerIdFieldNumber = 2;

		// Token: 0x0400025C RID: 604
		private long targetPlayerId_;
	}
}
