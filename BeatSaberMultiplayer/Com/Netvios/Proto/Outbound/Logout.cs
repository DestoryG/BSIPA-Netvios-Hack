using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Com.Netvios.Proto.Outbound
{
	// Token: 0x0200000C RID: 12
	public sealed class Logout : IMessage<Logout>, IMessage, IEquatable<Logout>, IDeepCloneable<Logout>
	{
		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060000A6 RID: 166 RVA: 0x0000680A File Offset: 0x00004A0A
		[DebuggerNonUserCode]
		public static MessageParser<Logout> Parser
		{
			get
			{
				return Logout._parser;
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060000A7 RID: 167 RVA: 0x00006811 File Offset: 0x00004A11
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return BeatSaberOutboundMessageReflection.Descriptor.MessageTypes[4];
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060000A8 RID: 168 RVA: 0x00006823 File Offset: 0x00004A23
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return Logout.Descriptor;
			}
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x0000682A File Offset: 0x00004A2A
		[DebuggerNonUserCode]
		public Logout()
		{
		}

		// Token: 0x060000AA RID: 170 RVA: 0x0000683D File Offset: 0x00004A3D
		[DebuggerNonUserCode]
		public Logout(Logout other)
			: this()
		{
			this.appChannel_ = other.appChannel_;
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00006862 File Offset: 0x00004A62
		[DebuggerNonUserCode]
		public Logout Clone()
		{
			return new Logout(this);
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060000AC RID: 172 RVA: 0x0000686A File Offset: 0x00004A6A
		// (set) Token: 0x060000AD RID: 173 RVA: 0x00006872 File Offset: 0x00004A72
		[DebuggerNonUserCode]
		public string AppChannel
		{
			get
			{
				return this.appChannel_;
			}
			set
			{
				this.appChannel_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00006885 File Offset: 0x00004A85
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as Logout);
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00006893 File Offset: 0x00004A93
		[DebuggerNonUserCode]
		public bool Equals(Logout other)
		{
			return other != null && (other == this || (!(this.AppChannel != other.AppChannel) && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x000068C8 File Offset: 0x00004AC8
		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (this.AppChannel.Length != 0)
			{
				num ^= this.AppChannel.GetHashCode();
			}
			if (this._unknownFields != null)
			{
				num ^= this._unknownFields.GetHashCode();
			}
			return num;
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x0000460F File Offset: 0x0000280F
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00006909 File Offset: 0x00004B09
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.AppChannel.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteString(this.AppChannel);
			}
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00006940 File Offset: 0x00004B40
		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.AppChannel.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.AppChannel);
			}
			if (this._unknownFields != null)
			{
				num += this._unknownFields.CalculateSize();
			}
			return num;
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00006983 File Offset: 0x00004B83
		[DebuggerNonUserCode]
		public void MergeFrom(Logout other)
		{
			if (other == null)
			{
				return;
			}
			if (other.AppChannel.Length != 0)
			{
				this.AppChannel = other.AppChannel;
			}
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x000069BC File Offset: 0x00004BBC
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
					this.AppChannel = input.ReadString();
				}
			}
		}

		// Token: 0x04000073 RID: 115
		private static readonly MessageParser<Logout> _parser = new MessageParser<Logout>(() => new Logout());

		// Token: 0x04000074 RID: 116
		private UnknownFieldSet _unknownFields;

		// Token: 0x04000075 RID: 117
		public const int AppChannelFieldNumber = 1;

		// Token: 0x04000076 RID: 118
		private string appChannel_ = "";
	}
}
