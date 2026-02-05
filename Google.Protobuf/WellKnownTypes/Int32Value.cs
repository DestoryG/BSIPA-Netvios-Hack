using System;
using System.Diagnostics;
using Google.Protobuf.Reflection;

namespace Google.Protobuf.WellKnownTypes
{
	// Token: 0x02000049 RID: 73
	public sealed class Int32Value : IMessage<Int32Value>, IMessage, IEquatable<Int32Value>, IDeepCloneable<Int32Value>
	{
		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000407 RID: 1031 RVA: 0x00010959 File Offset: 0x0000EB59
		[DebuggerNonUserCode]
		public static MessageParser<Int32Value> Parser
		{
			get
			{
				return Int32Value._parser;
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x06000408 RID: 1032 RVA: 0x00010960 File Offset: 0x0000EB60
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return WrappersReflection.Descriptor.MessageTypes[4];
			}
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x06000409 RID: 1033 RVA: 0x00010972 File Offset: 0x0000EB72
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return Int32Value.Descriptor;
			}
		}

		// Token: 0x0600040A RID: 1034 RVA: 0x00010979 File Offset: 0x0000EB79
		[DebuggerNonUserCode]
		public Int32Value()
		{
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x00010981 File Offset: 0x0000EB81
		[DebuggerNonUserCode]
		public Int32Value(Int32Value other)
			: this()
		{
			this.value_ = other.value_;
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x000109A6 File Offset: 0x0000EBA6
		[DebuggerNonUserCode]
		public Int32Value Clone()
		{
			return new Int32Value(this);
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x0600040D RID: 1037 RVA: 0x000109AE File Offset: 0x0000EBAE
		// (set) Token: 0x0600040E RID: 1038 RVA: 0x000109B6 File Offset: 0x0000EBB6
		[DebuggerNonUserCode]
		public int Value
		{
			get
			{
				return this.value_;
			}
			set
			{
				this.value_ = value;
			}
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x000109BF File Offset: 0x0000EBBF
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as Int32Value);
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x000109CD File Offset: 0x0000EBCD
		[DebuggerNonUserCode]
		public bool Equals(Int32Value other)
		{
			return other != null && (other == this || (this.Value == other.Value && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x000109FC File Offset: 0x0000EBFC
		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (this.Value != 0)
			{
				num ^= this.Value.GetHashCode();
			}
			if (this._unknownFields != null)
			{
				num ^= this._unknownFields.GetHashCode();
			}
			return num;
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x00010A3B File Offset: 0x0000EC3B
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x00010A43 File Offset: 0x0000EC43
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.Value != 0)
			{
				output.WriteRawTag(8);
				output.WriteInt32(this.Value);
			}
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x00010A74 File Offset: 0x0000EC74
		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Value != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Value);
			}
			if (this._unknownFields != null)
			{
				num += this._unknownFields.CalculateSize();
			}
			return num;
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x00010AB2 File Offset: 0x0000ECB2
		[DebuggerNonUserCode]
		public void MergeFrom(Int32Value other)
		{
			if (other == null)
			{
				return;
			}
			if (other.Value != 0)
			{
				this.Value = other.Value;
			}
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x00010AE4 File Offset: 0x0000ECE4
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
					this.Value = input.ReadInt32();
				}
			}
		}

		// Token: 0x0400013C RID: 316
		private static readonly MessageParser<Int32Value> _parser = new MessageParser<Int32Value>(() => new Int32Value());

		// Token: 0x0400013D RID: 317
		private UnknownFieldSet _unknownFields;

		// Token: 0x0400013E RID: 318
		public const int ValueFieldNumber = 1;

		// Token: 0x0400013F RID: 319
		private int value_;
	}
}
