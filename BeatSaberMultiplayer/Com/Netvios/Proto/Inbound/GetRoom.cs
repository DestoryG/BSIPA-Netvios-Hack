using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Com.Netvios.Proto.Inbound
{
	// Token: 0x02000038 RID: 56
	public sealed class GetRoom : IMessage<GetRoom>, IMessage, IEquatable<GetRoom>, IDeepCloneable<GetRoom>
	{
		// Token: 0x17000180 RID: 384
		// (get) Token: 0x0600049D RID: 1181 RVA: 0x0001463C File Offset: 0x0001283C
		[DebuggerNonUserCode]
		public static MessageParser<GetRoom> Parser
		{
			get
			{
				return GetRoom._parser;
			}
		}

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x0600049E RID: 1182 RVA: 0x00014643 File Offset: 0x00012843
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return BeatSaberInboundMessageReflection.Descriptor.MessageTypes[8];
			}
		}

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x0600049F RID: 1183 RVA: 0x00014655 File Offset: 0x00012855
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return GetRoom.Descriptor;
			}
		}

		// Token: 0x060004A0 RID: 1184 RVA: 0x0001465C File Offset: 0x0001285C
		[DebuggerNonUserCode]
		public GetRoom()
		{
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x0001466F File Offset: 0x0001286F
		[DebuggerNonUserCode]
		public GetRoom(GetRoom other)
			: this()
		{
			this.roomId_ = other.roomId_;
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x00014694 File Offset: 0x00012894
		[DebuggerNonUserCode]
		public GetRoom Clone()
		{
			return new GetRoom(this);
		}

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x060004A3 RID: 1187 RVA: 0x0001469C File Offset: 0x0001289C
		// (set) Token: 0x060004A4 RID: 1188 RVA: 0x000146A4 File Offset: 0x000128A4
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

		// Token: 0x060004A5 RID: 1189 RVA: 0x000146B7 File Offset: 0x000128B7
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as GetRoom);
		}

		// Token: 0x060004A6 RID: 1190 RVA: 0x000146C5 File Offset: 0x000128C5
		[DebuggerNonUserCode]
		public bool Equals(GetRoom other)
		{
			return other != null && (other == this || (!(this.RoomId != other.RoomId) && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x060004A7 RID: 1191 RVA: 0x000146F8 File Offset: 0x000128F8
		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (this.RoomId.Length != 0)
			{
				num ^= this.RoomId.GetHashCode();
			}
			if (this._unknownFields != null)
			{
				num ^= this._unknownFields.GetHashCode();
			}
			return num;
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x0000460F File Offset: 0x0000280F
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x060004A9 RID: 1193 RVA: 0x00014739 File Offset: 0x00012939
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.RoomId.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteString(this.RoomId);
			}
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x060004AA RID: 1194 RVA: 0x00014770 File Offset: 0x00012970
		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.RoomId.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.RoomId);
			}
			if (this._unknownFields != null)
			{
				num += this._unknownFields.CalculateSize();
			}
			return num;
		}

		// Token: 0x060004AB RID: 1195 RVA: 0x000147B3 File Offset: 0x000129B3
		[DebuggerNonUserCode]
		public void MergeFrom(GetRoom other)
		{
			if (other == null)
			{
				return;
			}
			if (other.RoomId.Length != 0)
			{
				this.RoomId = other.RoomId;
			}
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x060004AC RID: 1196 RVA: 0x000147EC File Offset: 0x000129EC
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
					this.RoomId = input.ReadString();
				}
			}
		}

		// Token: 0x04000241 RID: 577
		private static readonly MessageParser<GetRoom> _parser = new MessageParser<GetRoom>(() => new GetRoom());

		// Token: 0x04000242 RID: 578
		private UnknownFieldSet _unknownFields;

		// Token: 0x04000243 RID: 579
		public const int RoomIdFieldNumber = 1;

		// Token: 0x04000244 RID: 580
		private string roomId_ = "";
	}
}
