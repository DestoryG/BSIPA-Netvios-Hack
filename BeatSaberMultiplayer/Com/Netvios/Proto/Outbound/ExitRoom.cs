using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Com.Netvios.Proto.Outbound
{
	// Token: 0x02000013 RID: 19
	public sealed class ExitRoom : IMessage<ExitRoom>, IMessage, IEquatable<ExitRoom>, IDeepCloneable<ExitRoom>
	{
		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000159 RID: 345 RVA: 0x00008FF1 File Offset: 0x000071F1
		[DebuggerNonUserCode]
		public static MessageParser<ExitRoom> Parser
		{
			get
			{
				return ExitRoom._parser;
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x0600015A RID: 346 RVA: 0x00008FF8 File Offset: 0x000071F8
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return BeatSaberOutboundMessageReflection.Descriptor.MessageTypes[11];
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x0600015B RID: 347 RVA: 0x0000900B File Offset: 0x0000720B
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return ExitRoom.Descriptor;
			}
		}

		// Token: 0x0600015C RID: 348 RVA: 0x00009012 File Offset: 0x00007212
		[DebuggerNonUserCode]
		public ExitRoom()
		{
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00009025 File Offset: 0x00007225
		[DebuggerNonUserCode]
		public ExitRoom(ExitRoom other)
			: this()
		{
			this.roomId_ = other.roomId_;
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x0600015E RID: 350 RVA: 0x0000904A File Offset: 0x0000724A
		[DebuggerNonUserCode]
		public ExitRoom Clone()
		{
			return new ExitRoom(this);
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x0600015F RID: 351 RVA: 0x00009052 File Offset: 0x00007252
		// (set) Token: 0x06000160 RID: 352 RVA: 0x0000905A File Offset: 0x0000725A
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

		// Token: 0x06000161 RID: 353 RVA: 0x0000906D File Offset: 0x0000726D
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as ExitRoom);
		}

		// Token: 0x06000162 RID: 354 RVA: 0x0000907B File Offset: 0x0000727B
		[DebuggerNonUserCode]
		public bool Equals(ExitRoom other)
		{
			return other != null && (other == this || (!(this.RoomId != other.RoomId) && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x06000163 RID: 355 RVA: 0x000090B0 File Offset: 0x000072B0
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

		// Token: 0x06000164 RID: 356 RVA: 0x0000460F File Offset: 0x0000280F
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x06000165 RID: 357 RVA: 0x000090F1 File Offset: 0x000072F1
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

		// Token: 0x06000166 RID: 358 RVA: 0x00009128 File Offset: 0x00007328
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

		// Token: 0x06000167 RID: 359 RVA: 0x0000916B File Offset: 0x0000736B
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

		// Token: 0x06000168 RID: 360 RVA: 0x000091A4 File Offset: 0x000073A4
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

		// Token: 0x040000D3 RID: 211
		private static readonly MessageParser<ExitRoom> _parser = new MessageParser<ExitRoom>(() => new ExitRoom());

		// Token: 0x040000D4 RID: 212
		private UnknownFieldSet _unknownFields;

		// Token: 0x040000D5 RID: 213
		public const int RoomIdFieldNumber = 1;

		// Token: 0x040000D6 RID: 214
		private string roomId_ = "";
	}
}
