using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Com.Netvios.Proto.Outbound
{
	// Token: 0x02000026 RID: 38
	public sealed class HeadphoneOnNotice : IMessage<HeadphoneOnNotice>, IMessage, IEquatable<HeadphoneOnNotice>, IDeepCloneable<HeadphoneOnNotice>
	{
		// Token: 0x17000103 RID: 259
		// (get) Token: 0x06000314 RID: 788 RVA: 0x0000E745 File Offset: 0x0000C945
		[DebuggerNonUserCode]
		public static MessageParser<HeadphoneOnNotice> Parser
		{
			get
			{
				return HeadphoneOnNotice._parser;
			}
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x06000315 RID: 789 RVA: 0x0000E74C File Offset: 0x0000C94C
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return BeatSaberOutboundMessageReflection.Descriptor.MessageTypes[30];
			}
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x06000316 RID: 790 RVA: 0x0000E75F File Offset: 0x0000C95F
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return HeadphoneOnNotice.Descriptor;
			}
		}

		// Token: 0x06000317 RID: 791 RVA: 0x0000E766 File Offset: 0x0000C966
		[DebuggerNonUserCode]
		public HeadphoneOnNotice()
		{
		}

		// Token: 0x06000318 RID: 792 RVA: 0x0000E784 File Offset: 0x0000C984
		[DebuggerNonUserCode]
		public HeadphoneOnNotice(HeadphoneOnNotice other)
			: this()
		{
			this.roomId_ = other.roomId_;
			this.ccResponse_ = other.ccResponse_;
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x06000319 RID: 793 RVA: 0x0000E7B5 File Offset: 0x0000C9B5
		[DebuggerNonUserCode]
		public HeadphoneOnNotice Clone()
		{
			return new HeadphoneOnNotice(this);
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x0600031A RID: 794 RVA: 0x0000E7BD File Offset: 0x0000C9BD
		// (set) Token: 0x0600031B RID: 795 RVA: 0x0000E7C5 File Offset: 0x0000C9C5
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

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x0600031C RID: 796 RVA: 0x0000E7D8 File Offset: 0x0000C9D8
		// (set) Token: 0x0600031D RID: 797 RVA: 0x0000E7E0 File Offset: 0x0000C9E0
		[DebuggerNonUserCode]
		public string CcResponse
		{
			get
			{
				return this.ccResponse_;
			}
			set
			{
				this.ccResponse_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		// Token: 0x0600031E RID: 798 RVA: 0x0000E7F3 File Offset: 0x0000C9F3
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as HeadphoneOnNotice);
		}

		// Token: 0x0600031F RID: 799 RVA: 0x0000E804 File Offset: 0x0000CA04
		[DebuggerNonUserCode]
		public bool Equals(HeadphoneOnNotice other)
		{
			return other != null && (other == this || (!(this.RoomId != other.RoomId) && !(this.CcResponse != other.CcResponse) && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x06000320 RID: 800 RVA: 0x0000E858 File Offset: 0x0000CA58
		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (this.RoomId.Length != 0)
			{
				num ^= this.RoomId.GetHashCode();
			}
			if (this.CcResponse.Length != 0)
			{
				num ^= this.CcResponse.GetHashCode();
			}
			if (this._unknownFields != null)
			{
				num ^= this._unknownFields.GetHashCode();
			}
			return num;
		}

		// Token: 0x06000321 RID: 801 RVA: 0x0000460F File Offset: 0x0000280F
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x06000322 RID: 802 RVA: 0x0000E8B4 File Offset: 0x0000CAB4
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.RoomId.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteString(this.RoomId);
			}
			if (this.CcResponse.Length != 0)
			{
				output.WriteRawTag(42);
				output.WriteString(this.CcResponse);
			}
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x06000323 RID: 803 RVA: 0x0000E918 File Offset: 0x0000CB18
		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.RoomId.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.RoomId);
			}
			if (this.CcResponse.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.CcResponse);
			}
			if (this._unknownFields != null)
			{
				num += this._unknownFields.CalculateSize();
			}
			return num;
		}

		// Token: 0x06000324 RID: 804 RVA: 0x0000E978 File Offset: 0x0000CB78
		[DebuggerNonUserCode]
		public void MergeFrom(HeadphoneOnNotice other)
		{
			if (other == null)
			{
				return;
			}
			if (other.RoomId.Length != 0)
			{
				this.RoomId = other.RoomId;
			}
			if (other.CcResponse.Length != 0)
			{
				this.CcResponse = other.CcResponse;
			}
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x06000325 RID: 805 RVA: 0x0000E9D4 File Offset: 0x0000CBD4
		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num != 10U)
				{
					if (num != 42U)
					{
						this._unknownFields = UnknownFieldSet.MergeFieldFrom(this._unknownFields, input);
					}
					else
					{
						this.CcResponse = input.ReadString();
					}
				}
				else
				{
					this.RoomId = input.ReadString();
				}
			}
		}

		// Token: 0x0400019F RID: 415
		private static readonly MessageParser<HeadphoneOnNotice> _parser = new MessageParser<HeadphoneOnNotice>(() => new HeadphoneOnNotice());

		// Token: 0x040001A0 RID: 416
		private UnknownFieldSet _unknownFields;

		// Token: 0x040001A1 RID: 417
		public const int RoomIdFieldNumber = 1;

		// Token: 0x040001A2 RID: 418
		private string roomId_ = "";

		// Token: 0x040001A3 RID: 419
		public const int CcResponseFieldNumber = 5;

		// Token: 0x040001A4 RID: 420
		private string ccResponse_ = "";
	}
}
