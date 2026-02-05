using System;
using System.Diagnostics;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Google.Protobuf.WellKnownTypes
{
	// Token: 0x02000045 RID: 69
	public sealed class DoubleValue : IMessage<DoubleValue>, IMessage, IEquatable<DoubleValue>, IDeepCloneable<DoubleValue>
	{
		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x060003C3 RID: 963 RVA: 0x00010182 File Offset: 0x0000E382
		[DebuggerNonUserCode]
		public static MessageParser<DoubleValue> Parser
		{
			get
			{
				return DoubleValue._parser;
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x060003C4 RID: 964 RVA: 0x00010189 File Offset: 0x0000E389
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return WrappersReflection.Descriptor.MessageTypes[0];
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x060003C5 RID: 965 RVA: 0x0001019B File Offset: 0x0000E39B
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return DoubleValue.Descriptor;
			}
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x000101A2 File Offset: 0x0000E3A2
		[DebuggerNonUserCode]
		public DoubleValue()
		{
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x000101AA File Offset: 0x0000E3AA
		[DebuggerNonUserCode]
		public DoubleValue(DoubleValue other)
			: this()
		{
			this.value_ = other.value_;
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x000101CF File Offset: 0x0000E3CF
		[DebuggerNonUserCode]
		public DoubleValue Clone()
		{
			return new DoubleValue(this);
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x060003C9 RID: 969 RVA: 0x000101D7 File Offset: 0x0000E3D7
		// (set) Token: 0x060003CA RID: 970 RVA: 0x000101DF File Offset: 0x0000E3DF
		[DebuggerNonUserCode]
		public double Value
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

		// Token: 0x060003CB RID: 971 RVA: 0x000101E8 File Offset: 0x0000E3E8
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as DoubleValue);
		}

		// Token: 0x060003CC RID: 972 RVA: 0x000101F6 File Offset: 0x0000E3F6
		[DebuggerNonUserCode]
		public bool Equals(DoubleValue other)
		{
			return other != null && (other == this || (ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(this.Value, other.Value) && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x060003CD RID: 973 RVA: 0x00010230 File Offset: 0x0000E430
		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (this.Value != 0.0)
			{
				num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(this.Value);
			}
			if (this._unknownFields != null)
			{
				num ^= this._unknownFields.GetHashCode();
			}
			return num;
		}

		// Token: 0x060003CE RID: 974 RVA: 0x0001027A File Offset: 0x0000E47A
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x060003CF RID: 975 RVA: 0x00010282 File Offset: 0x0000E482
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.Value != 0.0)
			{
				output.WriteRawTag(9);
				output.WriteDouble(this.Value);
			}
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x000102C0 File Offset: 0x0000E4C0
		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Value != 0.0)
			{
				num += 9;
			}
			if (this._unknownFields != null)
			{
				num += this._unknownFields.CalculateSize();
			}
			return num;
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x000102FC File Offset: 0x0000E4FC
		[DebuggerNonUserCode]
		public void MergeFrom(DoubleValue other)
		{
			if (other == null)
			{
				return;
			}
			if (other.Value != 0.0)
			{
				this.Value = other.Value;
			}
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x00010338 File Offset: 0x0000E538
		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num != 9U)
				{
					this._unknownFields = UnknownFieldSet.MergeFieldFrom(this._unknownFields, input);
				}
				else
				{
					this.Value = input.ReadDouble();
				}
			}
		}

		// Token: 0x0400012C RID: 300
		private static readonly MessageParser<DoubleValue> _parser = new MessageParser<DoubleValue>(() => new DoubleValue());

		// Token: 0x0400012D RID: 301
		private UnknownFieldSet _unknownFields;

		// Token: 0x0400012E RID: 302
		public const int ValueFieldNumber = 1;

		// Token: 0x0400012F RID: 303
		private double value_;
	}
}
