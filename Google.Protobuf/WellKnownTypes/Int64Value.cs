using System;
using System.Diagnostics;
using Google.Protobuf.Reflection;

namespace Google.Protobuf.WellKnownTypes
{
	// Token: 0x02000047 RID: 71
	public sealed class Int64Value : IMessage<Int64Value>, IMessage, IEquatable<Int64Value>, IDeepCloneable<Int64Value>
	{
		// Token: 0x170000BC RID: 188
		// (get) Token: 0x060003E5 RID: 997 RVA: 0x00010592 File Offset: 0x0000E792
		[DebuggerNonUserCode]
		public static MessageParser<Int64Value> Parser
		{
			get
			{
				return Int64Value._parser;
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x060003E6 RID: 998 RVA: 0x00010599 File Offset: 0x0000E799
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return WrappersReflection.Descriptor.MessageTypes[2];
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x060003E7 RID: 999 RVA: 0x000105AB File Offset: 0x0000E7AB
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return Int64Value.Descriptor;
			}
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x000105B2 File Offset: 0x0000E7B2
		[DebuggerNonUserCode]
		public Int64Value()
		{
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x000105BA File Offset: 0x0000E7BA
		[DebuggerNonUserCode]
		public Int64Value(Int64Value other)
			: this()
		{
			this.value_ = other.value_;
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x000105DF File Offset: 0x0000E7DF
		[DebuggerNonUserCode]
		public Int64Value Clone()
		{
			return new Int64Value(this);
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x060003EB RID: 1003 RVA: 0x000105E7 File Offset: 0x0000E7E7
		// (set) Token: 0x060003EC RID: 1004 RVA: 0x000105EF File Offset: 0x0000E7EF
		[DebuggerNonUserCode]
		public long Value
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

		// Token: 0x060003ED RID: 1005 RVA: 0x000105F8 File Offset: 0x0000E7F8
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as Int64Value);
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x00010606 File Offset: 0x0000E806
		[DebuggerNonUserCode]
		public bool Equals(Int64Value other)
		{
			return other != null && (other == this || (this.Value == other.Value && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x00010634 File Offset: 0x0000E834
		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (this.Value != 0L)
			{
				num ^= this.Value.GetHashCode();
			}
			if (this._unknownFields != null)
			{
				num ^= this._unknownFields.GetHashCode();
			}
			return num;
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x00010673 File Offset: 0x0000E873
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x0001067B File Offset: 0x0000E87B
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.Value != 0L)
			{
				output.WriteRawTag(8);
				output.WriteInt64(this.Value);
			}
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x000106AC File Offset: 0x0000E8AC
		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Value != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.Value);
			}
			if (this._unknownFields != null)
			{
				num += this._unknownFields.CalculateSize();
			}
			return num;
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x000106EA File Offset: 0x0000E8EA
		[DebuggerNonUserCode]
		public void MergeFrom(Int64Value other)
		{
			if (other == null)
			{
				return;
			}
			if (other.Value != 0L)
			{
				this.Value = other.Value;
			}
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x0001071C File Offset: 0x0000E91C
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
					this.Value = input.ReadInt64();
				}
			}
		}

		// Token: 0x04000134 RID: 308
		private static readonly MessageParser<Int64Value> _parser = new MessageParser<Int64Value>(() => new Int64Value());

		// Token: 0x04000135 RID: 309
		private UnknownFieldSet _unknownFields;

		// Token: 0x04000136 RID: 310
		public const int ValueFieldNumber = 1;

		// Token: 0x04000137 RID: 311
		private long value_;
	}
}
