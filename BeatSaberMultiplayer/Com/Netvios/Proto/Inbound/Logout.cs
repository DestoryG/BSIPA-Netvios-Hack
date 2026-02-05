using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Com.Netvios.Proto.Inbound
{
	// Token: 0x02000034 RID: 52
	public sealed class Logout : IMessage<Logout>, IMessage, IEquatable<Logout>, IDeepCloneable<Logout>
	{
		// Token: 0x17000171 RID: 369
		// (get) Token: 0x0600045B RID: 1115 RVA: 0x00013F51 File Offset: 0x00012151
		[DebuggerNonUserCode]
		public static MessageParser<Logout> Parser
		{
			get
			{
				return Logout._parser;
			}
		}

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x0600045C RID: 1116 RVA: 0x00013F58 File Offset: 0x00012158
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return BeatSaberInboundMessageReflection.Descriptor.MessageTypes[4];
			}
		}

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x0600045D RID: 1117 RVA: 0x00013F6A File Offset: 0x0001216A
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return Logout.Descriptor;
			}
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x0000370C File Offset: 0x0000190C
		[DebuggerNonUserCode]
		public Logout()
		{
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x00013F71 File Offset: 0x00012171
		[DebuggerNonUserCode]
		public Logout(Logout other)
			: this()
		{
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x00013F8A File Offset: 0x0001218A
		[DebuggerNonUserCode]
		public Logout Clone()
		{
			return new Logout(this);
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x00013F92 File Offset: 0x00012192
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as Logout);
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x00013FA0 File Offset: 0x000121A0
		[DebuggerNonUserCode]
		public bool Equals(Logout other)
		{
			return other != null && (other == this || object.Equals(this._unknownFields, other._unknownFields));
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x00013FC0 File Offset: 0x000121C0
		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (this._unknownFields != null)
			{
				num ^= this._unknownFields.GetHashCode();
			}
			return num;
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x0000460F File Offset: 0x0000280F
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x00013FE6 File Offset: 0x000121E6
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x06000466 RID: 1126 RVA: 0x00013FFC File Offset: 0x000121FC
		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this._unknownFields != null)
			{
				num += this._unknownFields.CalculateSize();
			}
			return num;
		}

		// Token: 0x06000467 RID: 1127 RVA: 0x00014022 File Offset: 0x00012222
		[DebuggerNonUserCode]
		public void MergeFrom(Logout other)
		{
			if (other == null)
			{
				return;
			}
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x0001403F File Offset: 0x0001223F
		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			while (input.ReadTag() != 0U)
			{
				this._unknownFields = UnknownFieldSet.MergeFieldFrom(this._unknownFields, input);
			}
		}

		// Token: 0x04000233 RID: 563
		private static readonly MessageParser<Logout> _parser = new MessageParser<Logout>(() => new Logout());

		// Token: 0x04000234 RID: 564
		private UnknownFieldSet _unknownFields;
	}
}
