using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Com.Netvios.Proto.Outbound
{
	// Token: 0x0200001A RID: 26
	public sealed class RoomSubmitScore : IMessage<RoomSubmitScore>, IMessage, IEquatable<RoomSubmitScore>, IDeepCloneable<RoomSubmitScore>
	{
		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x060001EC RID: 492 RVA: 0x0000A8D8 File Offset: 0x00008AD8
		[DebuggerNonUserCode]
		public static MessageParser<RoomSubmitScore> Parser
		{
			get
			{
				return RoomSubmitScore._parser;
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x060001ED RID: 493 RVA: 0x0000A8DF File Offset: 0x00008ADF
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return BeatSaberOutboundMessageReflection.Descriptor.MessageTypes[18];
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x060001EE RID: 494 RVA: 0x0000A8F2 File Offset: 0x00008AF2
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return RoomSubmitScore.Descriptor;
			}
		}

		// Token: 0x060001EF RID: 495 RVA: 0x0000A8F9 File Offset: 0x00008AF9
		[DebuggerNonUserCode]
		public RoomSubmitScore()
		{
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x0000A917 File Offset: 0x00008B17
		[DebuggerNonUserCode]
		public RoomSubmitScore(RoomSubmitScore other)
			: this()
		{
			this.roomId_ = other.roomId_;
			this.success_ = other.success_;
			this.message_ = other.message_;
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x0000A954 File Offset: 0x00008B54
		[DebuggerNonUserCode]
		public RoomSubmitScore Clone()
		{
			return new RoomSubmitScore(this);
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x060001F2 RID: 498 RVA: 0x0000A95C File Offset: 0x00008B5C
		// (set) Token: 0x060001F3 RID: 499 RVA: 0x0000A964 File Offset: 0x00008B64
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

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x060001F4 RID: 500 RVA: 0x0000A977 File Offset: 0x00008B77
		// (set) Token: 0x060001F5 RID: 501 RVA: 0x0000A97F File Offset: 0x00008B7F
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

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x060001F6 RID: 502 RVA: 0x0000A988 File Offset: 0x00008B88
		// (set) Token: 0x060001F7 RID: 503 RVA: 0x0000A990 File Offset: 0x00008B90
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

		// Token: 0x060001F8 RID: 504 RVA: 0x0000A9A3 File Offset: 0x00008BA3
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as RoomSubmitScore);
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x0000A9B4 File Offset: 0x00008BB4
		[DebuggerNonUserCode]
		public bool Equals(RoomSubmitScore other)
		{
			return other != null && (other == this || (!(this.RoomId != other.RoomId) && this.Success == other.Success && !(this.Message != other.Message) && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x060001FA RID: 506 RVA: 0x0000AA18 File Offset: 0x00008C18
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

		// Token: 0x060001FB RID: 507 RVA: 0x0000460F File Offset: 0x0000280F
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x060001FC RID: 508 RVA: 0x0000AA90 File Offset: 0x00008C90
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

		// Token: 0x060001FD RID: 509 RVA: 0x0000AB10 File Offset: 0x00008D10
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

		// Token: 0x060001FE RID: 510 RVA: 0x0000AB7C File Offset: 0x00008D7C
		[DebuggerNonUserCode]
		public void MergeFrom(RoomSubmitScore other)
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

		// Token: 0x060001FF RID: 511 RVA: 0x0000ABEC File Offset: 0x00008DEC
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

		// Token: 0x0400010B RID: 267
		private static readonly MessageParser<RoomSubmitScore> _parser = new MessageParser<RoomSubmitScore>(() => new RoomSubmitScore());

		// Token: 0x0400010C RID: 268
		private UnknownFieldSet _unknownFields;

		// Token: 0x0400010D RID: 269
		public const int RoomIdFieldNumber = 1;

		// Token: 0x0400010E RID: 270
		private string roomId_ = "";

		// Token: 0x0400010F RID: 271
		public const int SuccessFieldNumber = 2;

		// Token: 0x04000110 RID: 272
		private bool success_;

		// Token: 0x04000111 RID: 273
		public const int MessageFieldNumber = 3;

		// Token: 0x04000112 RID: 274
		private string message_ = "";
	}
}
