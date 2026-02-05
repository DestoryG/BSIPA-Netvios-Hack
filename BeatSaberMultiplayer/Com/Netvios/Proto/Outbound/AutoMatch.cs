using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Com.Netvios.Proto.Outbound
{
	// Token: 0x0200001E RID: 30
	public sealed class AutoMatch : IMessage<AutoMatch>, IMessage, IEquatable<AutoMatch>, IDeepCloneable<AutoMatch>
	{
		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x0600024C RID: 588 RVA: 0x0000BCAD File Offset: 0x00009EAD
		[DebuggerNonUserCode]
		public static MessageParser<AutoMatch> Parser
		{
			get
			{
				return AutoMatch._parser;
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x0600024D RID: 589 RVA: 0x0000BCB4 File Offset: 0x00009EB4
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return BeatSaberOutboundMessageReflection.Descriptor.MessageTypes[22];
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x0600024E RID: 590 RVA: 0x0000BCC7 File Offset: 0x00009EC7
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return AutoMatch.Descriptor;
			}
		}

		// Token: 0x0600024F RID: 591 RVA: 0x0000370C File Offset: 0x0000190C
		[DebuggerNonUserCode]
		public AutoMatch()
		{
		}

		// Token: 0x06000250 RID: 592 RVA: 0x0000BCCE File Offset: 0x00009ECE
		[DebuggerNonUserCode]
		public AutoMatch(AutoMatch other)
			: this()
		{
			this.success_ = other.success_;
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x06000251 RID: 593 RVA: 0x0000BCF3 File Offset: 0x00009EF3
		[DebuggerNonUserCode]
		public AutoMatch Clone()
		{
			return new AutoMatch(this);
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x06000252 RID: 594 RVA: 0x0000BCFB File Offset: 0x00009EFB
		// (set) Token: 0x06000253 RID: 595 RVA: 0x0000BD03 File Offset: 0x00009F03
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

		// Token: 0x06000254 RID: 596 RVA: 0x0000BD0C File Offset: 0x00009F0C
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as AutoMatch);
		}

		// Token: 0x06000255 RID: 597 RVA: 0x0000BD1A File Offset: 0x00009F1A
		[DebuggerNonUserCode]
		public bool Equals(AutoMatch other)
		{
			return other != null && (other == this || (this.Success == other.Success && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x06000256 RID: 598 RVA: 0x0000BD48 File Offset: 0x00009F48
		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (this.Success)
			{
				num ^= this.Success.GetHashCode();
			}
			if (this._unknownFields != null)
			{
				num ^= this._unknownFields.GetHashCode();
			}
			return num;
		}

		// Token: 0x06000257 RID: 599 RVA: 0x0000460F File Offset: 0x0000280F
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x06000258 RID: 600 RVA: 0x0000BD87 File Offset: 0x00009F87
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.Success)
			{
				output.WriteRawTag(8);
				output.WriteBool(this.Success);
			}
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x06000259 RID: 601 RVA: 0x0000BDB8 File Offset: 0x00009FB8
		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Success)
			{
				num += 2;
			}
			if (this._unknownFields != null)
			{
				num += this._unknownFields.CalculateSize();
			}
			return num;
		}

		// Token: 0x0600025A RID: 602 RVA: 0x0000BDEA File Offset: 0x00009FEA
		[DebuggerNonUserCode]
		public void MergeFrom(AutoMatch other)
		{
			if (other == null)
			{
				return;
			}
			if (other.Success)
			{
				this.Success = other.Success;
			}
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x0600025B RID: 603 RVA: 0x0000BE1C File Offset: 0x0000A01C
		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num != 8U)
				{
					this._unknownFields = UnknownFieldSet.MergeFieldFrom(this._unknownFields, input);
				}
				else
				{
					this.Success = input.ReadBool();
				}
			}
		}

		// Token: 0x0400013B RID: 315
		private static readonly MessageParser<AutoMatch> _parser = new MessageParser<AutoMatch>(() => new AutoMatch());

		// Token: 0x0400013C RID: 316
		private UnknownFieldSet _unknownFields;

		// Token: 0x0400013D RID: 317
		public const int SuccessFieldNumber = 1;

		// Token: 0x0400013E RID: 318
		private bool success_;
	}
}
