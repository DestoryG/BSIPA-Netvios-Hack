using System;
using System.Diagnostics;
using Google.Protobuf.Reflection;

namespace Google.Protobuf.WellKnownTypes
{
	// Token: 0x0200004D RID: 77
	public sealed class BytesValue : IMessage<BytesValue>, IMessage, IEquatable<BytesValue>, IDeepCloneable<BytesValue>
	{
		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x0600044B RID: 1099 RVA: 0x0001110E File Offset: 0x0000F30E
		[DebuggerNonUserCode]
		public static MessageParser<BytesValue> Parser
		{
			get
			{
				return BytesValue._parser;
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x0600044C RID: 1100 RVA: 0x00011115 File Offset: 0x0000F315
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return WrappersReflection.Descriptor.MessageTypes[8];
			}
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x0600044D RID: 1101 RVA: 0x00011127 File Offset: 0x0000F327
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return BytesValue.Descriptor;
			}
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x0001112E File Offset: 0x0000F32E
		[DebuggerNonUserCode]
		public BytesValue()
		{
		}

		// Token: 0x0600044F RID: 1103 RVA: 0x00011141 File Offset: 0x0000F341
		[DebuggerNonUserCode]
		public BytesValue(BytesValue other)
			: this()
		{
			this.value_ = other.value_;
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x00011166 File Offset: 0x0000F366
		[DebuggerNonUserCode]
		public BytesValue Clone()
		{
			return new BytesValue(this);
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x06000451 RID: 1105 RVA: 0x0001116E File Offset: 0x0000F36E
		// (set) Token: 0x06000452 RID: 1106 RVA: 0x00011176 File Offset: 0x0000F376
		[DebuggerNonUserCode]
		public ByteString Value
		{
			get
			{
				return this.value_;
			}
			set
			{
				this.value_ = ProtoPreconditions.CheckNotNull<ByteString>(value, "value");
			}
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x00011189 File Offset: 0x0000F389
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as BytesValue);
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x00011197 File Offset: 0x0000F397
		[DebuggerNonUserCode]
		public bool Equals(BytesValue other)
		{
			return other != null && (other == this || (!(this.Value != other.Value) && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x000111CC File Offset: 0x0000F3CC
		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (this.Value.Length != 0)
			{
				num ^= this.Value.GetHashCode();
			}
			if (this._unknownFields != null)
			{
				num ^= this._unknownFields.GetHashCode();
			}
			return num;
		}

		// Token: 0x06000456 RID: 1110 RVA: 0x0001120D File Offset: 0x0000F40D
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x00011215 File Offset: 0x0000F415
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.Value.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteBytes(this.Value);
			}
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x0001124C File Offset: 0x0000F44C
		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Value.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeBytesSize(this.Value);
			}
			if (this._unknownFields != null)
			{
				num += this._unknownFields.CalculateSize();
			}
			return num;
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x0001128F File Offset: 0x0000F48F
		[DebuggerNonUserCode]
		public void MergeFrom(BytesValue other)
		{
			if (other == null)
			{
				return;
			}
			if (other.Value.Length != 0)
			{
				this.Value = other.Value;
			}
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x000112C8 File Offset: 0x0000F4C8
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
					this.Value = input.ReadBytes();
				}
			}
		}

		// Token: 0x0400014C RID: 332
		private static readonly MessageParser<BytesValue> _parser = new MessageParser<BytesValue>(() => new BytesValue());

		// Token: 0x0400014D RID: 333
		private UnknownFieldSet _unknownFields;

		// Token: 0x0400014E RID: 334
		public const int ValueFieldNumber = 1;

		// Token: 0x0400014F RID: 335
		private ByteString value_ = ByteString.Empty;
	}
}
