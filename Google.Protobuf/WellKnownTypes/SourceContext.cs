using System;
using System.Diagnostics;
using Google.Protobuf.Reflection;

namespace Google.Protobuf.WellKnownTypes
{
	// Token: 0x02000034 RID: 52
	public sealed class SourceContext : IMessage<SourceContext>, IMessage, IEquatable<SourceContext>, IDeepCloneable<SourceContext>
	{
		// Token: 0x1700006C RID: 108
		// (get) Token: 0x060002C7 RID: 711 RVA: 0x0000CC1F File Offset: 0x0000AE1F
		[DebuggerNonUserCode]
		public static MessageParser<SourceContext> Parser
		{
			get
			{
				return SourceContext._parser;
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x060002C8 RID: 712 RVA: 0x0000CC26 File Offset: 0x0000AE26
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return SourceContextReflection.Descriptor.MessageTypes[0];
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x060002C9 RID: 713 RVA: 0x0000CC38 File Offset: 0x0000AE38
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return SourceContext.Descriptor;
			}
		}

		// Token: 0x060002CA RID: 714 RVA: 0x0000CC3F File Offset: 0x0000AE3F
		[DebuggerNonUserCode]
		public SourceContext()
		{
		}

		// Token: 0x060002CB RID: 715 RVA: 0x0000CC52 File Offset: 0x0000AE52
		[DebuggerNonUserCode]
		public SourceContext(SourceContext other)
			: this()
		{
			this.fileName_ = other.fileName_;
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x060002CC RID: 716 RVA: 0x0000CC77 File Offset: 0x0000AE77
		[DebuggerNonUserCode]
		public SourceContext Clone()
		{
			return new SourceContext(this);
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x060002CD RID: 717 RVA: 0x0000CC7F File Offset: 0x0000AE7F
		// (set) Token: 0x060002CE RID: 718 RVA: 0x0000CC87 File Offset: 0x0000AE87
		[DebuggerNonUserCode]
		public string FileName
		{
			get
			{
				return this.fileName_;
			}
			set
			{
				this.fileName_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		// Token: 0x060002CF RID: 719 RVA: 0x0000CC9A File Offset: 0x0000AE9A
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as SourceContext);
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x0000CCA8 File Offset: 0x0000AEA8
		[DebuggerNonUserCode]
		public bool Equals(SourceContext other)
		{
			return other != null && (other == this || (!(this.FileName != other.FileName) && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x0000CCDC File Offset: 0x0000AEDC
		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (this.FileName.Length != 0)
			{
				num ^= this.FileName.GetHashCode();
			}
			if (this._unknownFields != null)
			{
				num ^= this._unknownFields.GetHashCode();
			}
			return num;
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x0000CD1D File Offset: 0x0000AF1D
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x0000CD25 File Offset: 0x0000AF25
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.FileName.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteString(this.FileName);
			}
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x0000CD5C File Offset: 0x0000AF5C
		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.FileName.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.FileName);
			}
			if (this._unknownFields != null)
			{
				num += this._unknownFields.CalculateSize();
			}
			return num;
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x0000CD9F File Offset: 0x0000AF9F
		[DebuggerNonUserCode]
		public void MergeFrom(SourceContext other)
		{
			if (other == null)
			{
				return;
			}
			if (other.FileName.Length != 0)
			{
				this.FileName = other.FileName;
			}
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x0000CDD8 File Offset: 0x0000AFD8
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
					this.FileName = input.ReadString();
				}
			}
		}

		// Token: 0x040000BA RID: 186
		private static readonly MessageParser<SourceContext> _parser = new MessageParser<SourceContext>(() => new SourceContext());

		// Token: 0x040000BB RID: 187
		private UnknownFieldSet _unknownFields;

		// Token: 0x040000BC RID: 188
		public const int FileNameFieldNumber = 1;

		// Token: 0x040000BD RID: 189
		private string fileName_ = "";
	}
}
