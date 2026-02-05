using System;
using System.Diagnostics;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Google.Protobuf.WellKnownTypes
{
	// Token: 0x02000037 RID: 55
	public sealed class Struct : IMessage<Struct>, IMessage, IEquatable<Struct>, IDeepCloneable<Struct>
	{
		// Token: 0x17000071 RID: 113
		// (get) Token: 0x060002DA RID: 730 RVA: 0x0000CFBC File Offset: 0x0000B1BC
		[DebuggerNonUserCode]
		public static MessageParser<Struct> Parser
		{
			get
			{
				return Struct._parser;
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x060002DB RID: 731 RVA: 0x0000CFC3 File Offset: 0x0000B1C3
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return StructReflection.Descriptor.MessageTypes[0];
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x060002DC RID: 732 RVA: 0x0000CFD5 File Offset: 0x0000B1D5
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return Struct.Descriptor;
			}
		}

		// Token: 0x060002DD RID: 733 RVA: 0x0000CFDC File Offset: 0x0000B1DC
		[DebuggerNonUserCode]
		public Struct()
		{
		}

		// Token: 0x060002DE RID: 734 RVA: 0x0000CFEF File Offset: 0x0000B1EF
		[DebuggerNonUserCode]
		public Struct(Struct other)
			: this()
		{
			this.fields_ = other.fields_.Clone();
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x060002DF RID: 735 RVA: 0x0000D019 File Offset: 0x0000B219
		[DebuggerNonUserCode]
		public Struct Clone()
		{
			return new Struct(this);
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x060002E0 RID: 736 RVA: 0x0000D021 File Offset: 0x0000B221
		[DebuggerNonUserCode]
		public MapField<string, Value> Fields
		{
			get
			{
				return this.fields_;
			}
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x0000D029 File Offset: 0x0000B229
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as Struct);
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x0000D037 File Offset: 0x0000B237
		[DebuggerNonUserCode]
		public bool Equals(Struct other)
		{
			return other != null && (other == this || (this.Fields.Equals(other.Fields) && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x0000D06C File Offset: 0x0000B26C
		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			num ^= this.Fields.GetHashCode();
			if (this._unknownFields != null)
			{
				num ^= this._unknownFields.GetHashCode();
			}
			return num;
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x0000D0A0 File Offset: 0x0000B2A0
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x0000D0A8 File Offset: 0x0000B2A8
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			this.fields_.WriteTo(output, Struct._map_fields_codec);
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x0000D0D0 File Offset: 0x0000B2D0
		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			num += this.fields_.CalculateSize(Struct._map_fields_codec);
			if (this._unknownFields != null)
			{
				num += this._unknownFields.CalculateSize();
			}
			return num;
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x0000D109 File Offset: 0x0000B309
		[DebuggerNonUserCode]
		public void MergeFrom(Struct other)
		{
			if (other == null)
			{
				return;
			}
			this.fields_.Add(other.fields_);
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x0000D138 File Offset: 0x0000B338
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
					this.fields_.AddEntriesFrom(input, Struct._map_fields_codec);
				}
			}
		}

		// Token: 0x040000C1 RID: 193
		private static readonly MessageParser<Struct> _parser = new MessageParser<Struct>(() => new Struct());

		// Token: 0x040000C2 RID: 194
		private UnknownFieldSet _unknownFields;

		// Token: 0x040000C3 RID: 195
		public const int FieldsFieldNumber = 1;

		// Token: 0x040000C4 RID: 196
		private static readonly MapField<string, Value>.Codec _map_fields_codec = new MapField<string, Value>.Codec(FieldCodec.ForString(10U, ""), FieldCodec.ForMessage<Value>(18U, Value.Parser), 10U);

		// Token: 0x040000C5 RID: 197
		private readonly MapField<string, Value> fields_ = new MapField<string, Value>();
	}
}
