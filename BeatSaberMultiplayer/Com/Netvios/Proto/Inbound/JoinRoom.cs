using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Com.Netvios.Proto.Inbound
{
	// Token: 0x0200003A RID: 58
	public sealed class JoinRoom : IMessage<JoinRoom>, IMessage, IEquatable<JoinRoom>, IDeepCloneable<JoinRoom>
	{
		// Token: 0x1700018A RID: 394
		// (get) Token: 0x060004C3 RID: 1219 RVA: 0x00014C4F File Offset: 0x00012E4F
		[DebuggerNonUserCode]
		public static MessageParser<JoinRoom> Parser
		{
			get
			{
				return JoinRoom._parser;
			}
		}

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x060004C4 RID: 1220 RVA: 0x00014C56 File Offset: 0x00012E56
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return BeatSaberInboundMessageReflection.Descriptor.MessageTypes[10];
			}
		}

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x060004C5 RID: 1221 RVA: 0x00014C69 File Offset: 0x00012E69
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return JoinRoom.Descriptor;
			}
		}

		// Token: 0x060004C6 RID: 1222 RVA: 0x00014C70 File Offset: 0x00012E70
		[DebuggerNonUserCode]
		public JoinRoom()
		{
		}

		// Token: 0x060004C7 RID: 1223 RVA: 0x00014C8E File Offset: 0x00012E8E
		[DebuggerNonUserCode]
		public JoinRoom(JoinRoom other)
			: this()
		{
			this.roomId_ = other.roomId_;
			this.password_ = other.password_;
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x060004C8 RID: 1224 RVA: 0x00014CBF File Offset: 0x00012EBF
		[DebuggerNonUserCode]
		public JoinRoom Clone()
		{
			return new JoinRoom(this);
		}

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x060004C9 RID: 1225 RVA: 0x00014CC7 File Offset: 0x00012EC7
		// (set) Token: 0x060004CA RID: 1226 RVA: 0x00014CCF File Offset: 0x00012ECF
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

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x060004CB RID: 1227 RVA: 0x00014CE2 File Offset: 0x00012EE2
		// (set) Token: 0x060004CC RID: 1228 RVA: 0x00014CEA File Offset: 0x00012EEA
		[DebuggerNonUserCode]
		public string Password
		{
			get
			{
				return this.password_;
			}
			set
			{
				this.password_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		// Token: 0x060004CD RID: 1229 RVA: 0x00014CFD File Offset: 0x00012EFD
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as JoinRoom);
		}

		// Token: 0x060004CE RID: 1230 RVA: 0x00014D0C File Offset: 0x00012F0C
		[DebuggerNonUserCode]
		public bool Equals(JoinRoom other)
		{
			return other != null && (other == this || (!(this.RoomId != other.RoomId) && !(this.Password != other.Password) && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x060004CF RID: 1231 RVA: 0x00014D60 File Offset: 0x00012F60
		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (this.RoomId.Length != 0)
			{
				num ^= this.RoomId.GetHashCode();
			}
			if (this.Password.Length != 0)
			{
				num ^= this.Password.GetHashCode();
			}
			if (this._unknownFields != null)
			{
				num ^= this._unknownFields.GetHashCode();
			}
			return num;
		}

		// Token: 0x060004D0 RID: 1232 RVA: 0x0000460F File Offset: 0x0000280F
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x060004D1 RID: 1233 RVA: 0x00014DBC File Offset: 0x00012FBC
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.RoomId.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteString(this.RoomId);
			}
			if (this.Password.Length != 0)
			{
				output.WriteRawTag(18);
				output.WriteString(this.Password);
			}
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x060004D2 RID: 1234 RVA: 0x00014E20 File Offset: 0x00013020
		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.RoomId.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.RoomId);
			}
			if (this.Password.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Password);
			}
			if (this._unknownFields != null)
			{
				num += this._unknownFields.CalculateSize();
			}
			return num;
		}

		// Token: 0x060004D3 RID: 1235 RVA: 0x00014E80 File Offset: 0x00013080
		[DebuggerNonUserCode]
		public void MergeFrom(JoinRoom other)
		{
			if (other == null)
			{
				return;
			}
			if (other.RoomId.Length != 0)
			{
				this.RoomId = other.RoomId;
			}
			if (other.Password.Length != 0)
			{
				this.Password = other.Password;
			}
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x060004D4 RID: 1236 RVA: 0x00014EDC File Offset: 0x000130DC
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
						this._unknownFields = UnknownFieldSet.MergeFieldFrom(this._unknownFields, input);
					}
					else
					{
						this.Password = input.ReadString();
					}
				}
				else
				{
					this.RoomId = input.ReadString();
				}
			}
		}

		// Token: 0x0400024D RID: 589
		private static readonly MessageParser<JoinRoom> _parser = new MessageParser<JoinRoom>(() => new JoinRoom());

		// Token: 0x0400024E RID: 590
		private UnknownFieldSet _unknownFields;

		// Token: 0x0400024F RID: 591
		public const int RoomIdFieldNumber = 1;

		// Token: 0x04000250 RID: 592
		private string roomId_ = "";

		// Token: 0x04000251 RID: 593
		public const int PasswordFieldNumber = 2;

		// Token: 0x04000252 RID: 594
		private string password_ = "";
	}
}
