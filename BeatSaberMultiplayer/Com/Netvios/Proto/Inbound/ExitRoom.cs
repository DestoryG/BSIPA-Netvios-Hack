using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Com.Netvios.Proto.Inbound
{
	// Token: 0x0200003B RID: 59
	public sealed class ExitRoom : IMessage<ExitRoom>, IMessage, IEquatable<ExitRoom>, IDeepCloneable<ExitRoom>
	{
		// Token: 0x1700018F RID: 399
		// (get) Token: 0x060004D6 RID: 1238 RVA: 0x00014F49 File Offset: 0x00013149
		[DebuggerNonUserCode]
		public static MessageParser<ExitRoom> Parser
		{
			get
			{
				return ExitRoom._parser;
			}
		}

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x060004D7 RID: 1239 RVA: 0x00014F50 File Offset: 0x00013150
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return BeatSaberInboundMessageReflection.Descriptor.MessageTypes[11];
			}
		}

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x060004D8 RID: 1240 RVA: 0x00014F63 File Offset: 0x00013163
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return ExitRoom.Descriptor;
			}
		}

		// Token: 0x060004D9 RID: 1241 RVA: 0x00014F6A File Offset: 0x0001316A
		[DebuggerNonUserCode]
		public ExitRoom()
		{
		}

		// Token: 0x060004DA RID: 1242 RVA: 0x00014F7D File Offset: 0x0001317D
		[DebuggerNonUserCode]
		public ExitRoom(ExitRoom other)
			: this()
		{
			this.roomId_ = other.roomId_;
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x060004DB RID: 1243 RVA: 0x00014FA2 File Offset: 0x000131A2
		[DebuggerNonUserCode]
		public ExitRoom Clone()
		{
			return new ExitRoom(this);
		}

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x060004DC RID: 1244 RVA: 0x00014FAA File Offset: 0x000131AA
		// (set) Token: 0x060004DD RID: 1245 RVA: 0x00014FB2 File Offset: 0x000131B2
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

		// Token: 0x060004DE RID: 1246 RVA: 0x00014FC5 File Offset: 0x000131C5
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as ExitRoom);
		}

		// Token: 0x060004DF RID: 1247 RVA: 0x00014FD3 File Offset: 0x000131D3
		[DebuggerNonUserCode]
		public bool Equals(ExitRoom other)
		{
			return other != null && (other == this || (!(this.RoomId != other.RoomId) && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x060004E0 RID: 1248 RVA: 0x00015008 File Offset: 0x00013208
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

		// Token: 0x060004E1 RID: 1249 RVA: 0x0000460F File Offset: 0x0000280F
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x060004E2 RID: 1250 RVA: 0x00015049 File Offset: 0x00013249
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

		// Token: 0x060004E3 RID: 1251 RVA: 0x00015080 File Offset: 0x00013280
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

		// Token: 0x060004E4 RID: 1252 RVA: 0x000150C3 File Offset: 0x000132C3
		[DebuggerNonUserCode]
		public void MergeFrom(ExitRoom other)
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

		// Token: 0x060004E5 RID: 1253 RVA: 0x000150FC File Offset: 0x000132FC
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

		// Token: 0x04000253 RID: 595
		private static readonly MessageParser<ExitRoom> _parser = new MessageParser<ExitRoom>(() => new ExitRoom());

		// Token: 0x04000254 RID: 596
		private UnknownFieldSet _unknownFields;

		// Token: 0x04000255 RID: 597
		public const int RoomIdFieldNumber = 1;

		// Token: 0x04000256 RID: 598
		private string roomId_ = "";
	}
}
