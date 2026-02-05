using System;
using System.Diagnostics;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Google.Protobuf.WellKnownTypes
{
	// Token: 0x02000046 RID: 70
	public sealed class FloatValue : IMessage<FloatValue>, IMessage, IEquatable<FloatValue>, IDeepCloneable<FloatValue>
	{
		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x060003D4 RID: 980 RVA: 0x00010392 File Offset: 0x0000E592
		[DebuggerNonUserCode]
		public static MessageParser<FloatValue> Parser
		{
			get
			{
				return FloatValue._parser;
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x060003D5 RID: 981 RVA: 0x00010399 File Offset: 0x0000E599
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return WrappersReflection.Descriptor.MessageTypes[1];
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x060003D6 RID: 982 RVA: 0x000103AB File Offset: 0x0000E5AB
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return FloatValue.Descriptor;
			}
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x000103B2 File Offset: 0x0000E5B2
		[DebuggerNonUserCode]
		public FloatValue()
		{
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x000103BA File Offset: 0x0000E5BA
		[DebuggerNonUserCode]
		public FloatValue(FloatValue other)
			: this()
		{
			this.value_ = other.value_;
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x000103DF File Offset: 0x0000E5DF
		[DebuggerNonUserCode]
		public FloatValue Clone()
		{
			return new FloatValue(this);
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x060003DA RID: 986 RVA: 0x000103E7 File Offset: 0x0000E5E7
		// (set) Token: 0x060003DB RID: 987 RVA: 0x000103EF File Offset: 0x0000E5EF
		[DebuggerNonUserCode]
		public float Value
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

		// Token: 0x060003DC RID: 988 RVA: 0x000103F8 File Offset: 0x0000E5F8
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as FloatValue);
		}

		// Token: 0x060003DD RID: 989 RVA: 0x00010406 File Offset: 0x0000E606
		[DebuggerNonUserCode]
		public bool Equals(FloatValue other)
		{
			return other != null && (other == this || (ProtobufEqualityComparers.BitwiseSingleEqualityComparer.Equals(this.Value, other.Value) && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x060003DE RID: 990 RVA: 0x00010440 File Offset: 0x0000E640
		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (this.Value != 0f)
			{
				num ^= ProtobufEqualityComparers.BitwiseSingleEqualityComparer.GetHashCode(this.Value);
			}
			if (this._unknownFields != null)
			{
				num ^= this._unknownFields.GetHashCode();
			}
			return num;
		}

		// Token: 0x060003DF RID: 991 RVA: 0x00010486 File Offset: 0x0000E686
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x0001048E File Offset: 0x0000E68E
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.Value != 0f)
			{
				output.WriteRawTag(13);
				output.WriteFloat(this.Value);
			}
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x000104C8 File Offset: 0x0000E6C8
		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Value != 0f)
			{
				num += 5;
			}
			if (this._unknownFields != null)
			{
				num += this._unknownFields.CalculateSize();
			}
			return num;
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x000104FF File Offset: 0x0000E6FF
		[DebuggerNonUserCode]
		public void MergeFrom(FloatValue other)
		{
			if (other == null)
			{
				return;
			}
			if (other.Value != 0f)
			{
				this.Value = other.Value;
			}
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x00010538 File Offset: 0x0000E738
		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num != 13U)
				{
					this._unknownFields = UnknownFieldSet.MergeFieldFrom(this._unknownFields, input);
				}
				else
				{
					this.Value = input.ReadFloat();
				}
			}
		}

		// Token: 0x04000130 RID: 304
		private static readonly MessageParser<FloatValue> _parser = new MessageParser<FloatValue>(() => new FloatValue());

		// Token: 0x04000131 RID: 305
		private UnknownFieldSet _unknownFields;

		// Token: 0x04000132 RID: 306
		public const int ValueFieldNumber = 1;

		// Token: 0x04000133 RID: 307
		private float value_;
	}
}
