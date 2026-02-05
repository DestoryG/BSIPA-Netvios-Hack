using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Com.Netvios.Proto.Inbound
{
	// Token: 0x02000033 RID: 51
	public sealed class Renew : IMessage<Renew>, IMessage, IEquatable<Renew>, IDeepCloneable<Renew>
	{
		// Token: 0x1700016E RID: 366
		// (get) Token: 0x0600044C RID: 1100 RVA: 0x00013E2A File Offset: 0x0001202A
		[DebuggerNonUserCode]
		public static MessageParser<Renew> Parser
		{
			get
			{
				return Renew._parser;
			}
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x0600044D RID: 1101 RVA: 0x00013E31 File Offset: 0x00012031
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return BeatSaberInboundMessageReflection.Descriptor.MessageTypes[3];
			}
		}

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x0600044E RID: 1102 RVA: 0x00013E43 File Offset: 0x00012043
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return Renew.Descriptor;
			}
		}

		// Token: 0x0600044F RID: 1103 RVA: 0x0000370C File Offset: 0x0000190C
		[DebuggerNonUserCode]
		public Renew()
		{
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x00013E4A File Offset: 0x0001204A
		[DebuggerNonUserCode]
		public Renew(Renew other)
			: this()
		{
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x06000451 RID: 1105 RVA: 0x00013E63 File Offset: 0x00012063
		[DebuggerNonUserCode]
		public Renew Clone()
		{
			return new Renew(this);
		}

		// Token: 0x06000452 RID: 1106 RVA: 0x00013E6B File Offset: 0x0001206B
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as Renew);
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x00013E79 File Offset: 0x00012079
		[DebuggerNonUserCode]
		public bool Equals(Renew other)
		{
			return other != null && (other == this || object.Equals(this._unknownFields, other._unknownFields));
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x00013E98 File Offset: 0x00012098
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

		// Token: 0x06000455 RID: 1109 RVA: 0x0000460F File Offset: 0x0000280F
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x06000456 RID: 1110 RVA: 0x00013EBE File Offset: 0x000120BE
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x00013ED4 File Offset: 0x000120D4
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

		// Token: 0x06000458 RID: 1112 RVA: 0x00013EFA File Offset: 0x000120FA
		[DebuggerNonUserCode]
		public void MergeFrom(Renew other)
		{
			if (other == null)
			{
				return;
			}
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x00013F17 File Offset: 0x00012117
		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			while (input.ReadTag() != 0U)
			{
				this._unknownFields = UnknownFieldSet.MergeFieldFrom(this._unknownFields, input);
			}
		}

		// Token: 0x04000231 RID: 561
		private static readonly MessageParser<Renew> _parser = new MessageParser<Renew>(() => new Renew());

		// Token: 0x04000232 RID: 562
		private UnknownFieldSet _unknownFields;
	}
}
