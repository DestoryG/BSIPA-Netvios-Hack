using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Com.Netvios.Proto.Inbound
{
	// Token: 0x0200003D RID: 61
	public sealed class StartGame : IMessage<StartGame>, IMessage, IEquatable<StartGame>, IDeepCloneable<StartGame>
	{
		// Token: 0x17000198 RID: 408
		// (get) Token: 0x060004FA RID: 1274 RVA: 0x0001542D File Offset: 0x0001362D
		[DebuggerNonUserCode]
		public static MessageParser<StartGame> Parser
		{
			get
			{
				return StartGame._parser;
			}
		}

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x060004FB RID: 1275 RVA: 0x00015434 File Offset: 0x00013634
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return BeatSaberInboundMessageReflection.Descriptor.MessageTypes[13];
			}
		}

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x060004FC RID: 1276 RVA: 0x00015447 File Offset: 0x00013647
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return StartGame.Descriptor;
			}
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x0001544E File Offset: 0x0001364E
		[DebuggerNonUserCode]
		public StartGame()
		{
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x00015461 File Offset: 0x00013661
		[DebuggerNonUserCode]
		public StartGame(StartGame other)
			: this()
		{
			this.roomId_ = other.roomId_;
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x060004FF RID: 1279 RVA: 0x00015486 File Offset: 0x00013686
		[DebuggerNonUserCode]
		public StartGame Clone()
		{
			return new StartGame(this);
		}

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x06000500 RID: 1280 RVA: 0x0001548E File Offset: 0x0001368E
		// (set) Token: 0x06000501 RID: 1281 RVA: 0x00015496 File Offset: 0x00013696
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

		// Token: 0x06000502 RID: 1282 RVA: 0x000154A9 File Offset: 0x000136A9
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as StartGame);
		}

		// Token: 0x06000503 RID: 1283 RVA: 0x000154B7 File Offset: 0x000136B7
		[DebuggerNonUserCode]
		public bool Equals(StartGame other)
		{
			return other != null && (other == this || (!(this.RoomId != other.RoomId) && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x000154EC File Offset: 0x000136EC
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

		// Token: 0x06000505 RID: 1285 RVA: 0x0000460F File Offset: 0x0000280F
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x0001552D File Offset: 0x0001372D
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

		// Token: 0x06000507 RID: 1287 RVA: 0x00015564 File Offset: 0x00013764
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

		// Token: 0x06000508 RID: 1288 RVA: 0x000155A7 File Offset: 0x000137A7
		[DebuggerNonUserCode]
		public void MergeFrom(StartGame other)
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

		// Token: 0x06000509 RID: 1289 RVA: 0x000155E0 File Offset: 0x000137E0
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

		// Token: 0x0400025D RID: 605
		private static readonly MessageParser<StartGame> _parser = new MessageParser<StartGame>(() => new StartGame());

		// Token: 0x0400025E RID: 606
		private UnknownFieldSet _unknownFields;

		// Token: 0x0400025F RID: 607
		public const int RoomIdFieldNumber = 1;

		// Token: 0x04000260 RID: 608
		private string roomId_ = "";
	}
}
