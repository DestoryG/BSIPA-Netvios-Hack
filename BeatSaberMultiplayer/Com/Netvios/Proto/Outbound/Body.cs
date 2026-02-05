using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Com.Netvios.Proto.Outbound
{
	// Token: 0x0200002E RID: 46
	public sealed class Body : IMessage<Body>, IMessage, IEquatable<Body>, IDeepCloneable<Body>
	{
		// Token: 0x1700013E RID: 318
		// (get) Token: 0x060003C8 RID: 968 RVA: 0x00010DF0 File Offset: 0x0000EFF0
		[DebuggerNonUserCode]
		public static MessageParser<Body> Parser
		{
			get
			{
				return Body._parser;
			}
		}

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x060003C9 RID: 969 RVA: 0x00010DF7 File Offset: 0x0000EFF7
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return OutboundMessageReflection.Descriptor.MessageTypes[0];
			}
		}

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x060003CA RID: 970 RVA: 0x00010E09 File Offset: 0x0000F009
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return Body.Descriptor;
			}
		}

		// Token: 0x060003CB RID: 971 RVA: 0x00010E10 File Offset: 0x0000F010
		[DebuggerNonUserCode]
		public Body()
		{
		}

		// Token: 0x060003CC RID: 972 RVA: 0x00010E24 File Offset: 0x0000F024
		[DebuggerNonUserCode]
		public Body(Body other)
			: this()
		{
			this.game_ = other.game_;
			this.code_ = other.code_;
			this.message_ = other.message_;
			this.took_ = other.took_;
			if (other.DataCase == Body.DataOneofCase.BeatSaberBody)
			{
				this.BeatSaberBody = other.BeatSaberBody.Clone();
			}
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x060003CD RID: 973 RVA: 0x00010E92 File Offset: 0x0000F092
		[DebuggerNonUserCode]
		public Body Clone()
		{
			return new Body(this);
		}

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x060003CE RID: 974 RVA: 0x00010E9A File Offset: 0x0000F09A
		// (set) Token: 0x060003CF RID: 975 RVA: 0x00010EA2 File Offset: 0x0000F0A2
		[DebuggerNonUserCode]
		public GameType Game
		{
			get
			{
				return this.game_;
			}
			set
			{
				this.game_ = value;
			}
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x060003D0 RID: 976 RVA: 0x00010EAB File Offset: 0x0000F0AB
		// (set) Token: 0x060003D1 RID: 977 RVA: 0x00010EB3 File Offset: 0x0000F0B3
		[DebuggerNonUserCode]
		public int Code
		{
			get
			{
				return this.code_;
			}
			set
			{
				this.code_ = value;
			}
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x060003D2 RID: 978 RVA: 0x00010EBC File Offset: 0x0000F0BC
		// (set) Token: 0x060003D3 RID: 979 RVA: 0x00010EC4 File Offset: 0x0000F0C4
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

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x060003D4 RID: 980 RVA: 0x00010ED7 File Offset: 0x0000F0D7
		// (set) Token: 0x060003D5 RID: 981 RVA: 0x00010EDF File Offset: 0x0000F0DF
		[DebuggerNonUserCode]
		public int Took
		{
			get
			{
				return this.took_;
			}
			set
			{
				this.took_ = value;
			}
		}

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x060003D6 RID: 982 RVA: 0x00010EE8 File Offset: 0x0000F0E8
		// (set) Token: 0x060003D7 RID: 983 RVA: 0x00010F00 File Offset: 0x0000F100
		[DebuggerNonUserCode]
		public BeatSaberBody BeatSaberBody
		{
			get
			{
				if (this.dataCase_ != Body.DataOneofCase.BeatSaberBody)
				{
					return null;
				}
				return (BeatSaberBody)this.data_;
			}
			set
			{
				this.data_ = value;
				this.dataCase_ = ((value == null) ? Body.DataOneofCase.None : Body.DataOneofCase.BeatSaberBody);
			}
		}

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x060003D8 RID: 984 RVA: 0x00010F16 File Offset: 0x0000F116
		[DebuggerNonUserCode]
		public Body.DataOneofCase DataCase
		{
			get
			{
				return this.dataCase_;
			}
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x00010F1E File Offset: 0x0000F11E
		[DebuggerNonUserCode]
		public void ClearData()
		{
			this.dataCase_ = Body.DataOneofCase.None;
			this.data_ = null;
		}

		// Token: 0x060003DA RID: 986 RVA: 0x00010F2E File Offset: 0x0000F12E
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as Body);
		}

		// Token: 0x060003DB RID: 987 RVA: 0x00010F3C File Offset: 0x0000F13C
		[DebuggerNonUserCode]
		public bool Equals(Body other)
		{
			return other != null && (other == this || (this.Game == other.Game && this.Code == other.Code && !(this.Message != other.Message) && this.Took == other.Took && object.Equals(this.BeatSaberBody, other.BeatSaberBody) && this.DataCase == other.DataCase && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x060003DC RID: 988 RVA: 0x00010FD0 File Offset: 0x0000F1D0
		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (this.Game != GameType.BeatSaber)
			{
				num ^= this.Game.GetHashCode();
			}
			if (this.Code != 0)
			{
				num ^= this.Code.GetHashCode();
			}
			if (this.Message.Length != 0)
			{
				num ^= this.Message.GetHashCode();
			}
			if (this.Took != 0)
			{
				num ^= this.Took.GetHashCode();
			}
			if (this.dataCase_ == Body.DataOneofCase.BeatSaberBody)
			{
				num ^= this.BeatSaberBody.GetHashCode();
			}
			num ^= (int)this.dataCase_;
			if (this._unknownFields != null)
			{
				num ^= this._unknownFields.GetHashCode();
			}
			return num;
		}

		// Token: 0x060003DD RID: 989 RVA: 0x0000460F File Offset: 0x0000280F
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x060003DE RID: 990 RVA: 0x00011084 File Offset: 0x0000F284
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.Game != GameType.BeatSaber)
			{
				output.WriteRawTag(8);
				output.WriteEnum((int)this.Game);
			}
			if (this.Code != 0)
			{
				output.WriteRawTag(16);
				output.WriteInt32(this.Code);
			}
			if (this.Message.Length != 0)
			{
				output.WriteRawTag(26);
				output.WriteString(this.Message);
			}
			if (this.Took != 0)
			{
				output.WriteRawTag(32);
				output.WriteInt32(this.Took);
			}
			if (this.dataCase_ == Body.DataOneofCase.BeatSaberBody)
			{
				output.WriteRawTag(42);
				output.WriteMessage(this.BeatSaberBody);
			}
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x060003DF RID: 991 RVA: 0x00011138 File Offset: 0x0000F338
		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Game != GameType.BeatSaber)
			{
				num += 1 + CodedOutputStream.ComputeEnumSize((int)this.Game);
			}
			if (this.Code != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Code);
			}
			if (this.Message.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Message);
			}
			if (this.Took != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Took);
			}
			if (this.dataCase_ == Body.DataOneofCase.BeatSaberBody)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.BeatSaberBody);
			}
			if (this._unknownFields != null)
			{
				num += this._unknownFields.CalculateSize();
			}
			return num;
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x000111DC File Offset: 0x0000F3DC
		[DebuggerNonUserCode]
		public void MergeFrom(Body other)
		{
			if (other == null)
			{
				return;
			}
			if (other.Game != GameType.BeatSaber)
			{
				this.Game = other.Game;
			}
			if (other.Code != 0)
			{
				this.Code = other.Code;
			}
			if (other.Message.Length != 0)
			{
				this.Message = other.Message;
			}
			if (other.Took != 0)
			{
				this.Took = other.Took;
			}
			if (other.DataCase == Body.DataOneofCase.BeatSaberBody)
			{
				if (this.BeatSaberBody == null)
				{
					this.BeatSaberBody = new BeatSaberBody();
				}
				this.BeatSaberBody.MergeFrom(other.BeatSaberBody);
			}
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x00011288 File Offset: 0x0000F488
		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 16U)
				{
					if (num == 8U)
					{
						this.Game = (GameType)input.ReadEnum();
						continue;
					}
					if (num == 16U)
					{
						this.Code = input.ReadInt32();
						continue;
					}
				}
				else
				{
					if (num == 26U)
					{
						this.Message = input.ReadString();
						continue;
					}
					if (num == 32U)
					{
						this.Took = input.ReadInt32();
						continue;
					}
					if (num == 42U)
					{
						BeatSaberBody beatSaberBody = new BeatSaberBody();
						if (this.dataCase_ == Body.DataOneofCase.BeatSaberBody)
						{
							beatSaberBody.MergeFrom(this.BeatSaberBody);
						}
						input.ReadMessage(beatSaberBody);
						this.BeatSaberBody = beatSaberBody;
						continue;
					}
				}
				this._unknownFields = UnknownFieldSet.MergeFieldFrom(this._unknownFields, input);
			}
		}

		// Token: 0x040001F9 RID: 505
		private static readonly MessageParser<Body> _parser = new MessageParser<Body>(() => new Body());

		// Token: 0x040001FA RID: 506
		private UnknownFieldSet _unknownFields;

		// Token: 0x040001FB RID: 507
		public const int GameFieldNumber = 1;

		// Token: 0x040001FC RID: 508
		private GameType game_;

		// Token: 0x040001FD RID: 509
		public const int CodeFieldNumber = 2;

		// Token: 0x040001FE RID: 510
		private int code_;

		// Token: 0x040001FF RID: 511
		public const int MessageFieldNumber = 3;

		// Token: 0x04000200 RID: 512
		private string message_ = "";

		// Token: 0x04000201 RID: 513
		public const int TookFieldNumber = 4;

		// Token: 0x04000202 RID: 514
		private int took_;

		// Token: 0x04000203 RID: 515
		public const int BeatSaberBodyFieldNumber = 5;

		// Token: 0x04000204 RID: 516
		private object data_;

		// Token: 0x04000205 RID: 517
		private Body.DataOneofCase dataCase_;

		// Token: 0x020000C7 RID: 199
		public enum DataOneofCase
		{
			// Token: 0x04000544 RID: 1348
			None,
			// Token: 0x04000545 RID: 1349
			BeatSaberBody = 5
		}
	}
}
