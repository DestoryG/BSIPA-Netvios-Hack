using System;
using System.Diagnostics;
using Google.Protobuf.Reflection;

namespace Google.Protobuf.WellKnownTypes
{
	// Token: 0x0200004C RID: 76
	public sealed class StringValue : IMessage<StringValue>, IMessage, IEquatable<StringValue>, IDeepCloneable<StringValue>
	{
		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x0600043A RID: 1082 RVA: 0x00010EF9 File Offset: 0x0000F0F9
		[DebuggerNonUserCode]
		public static MessageParser<StringValue> Parser
		{
			get
			{
				return StringValue._parser;
			}
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x0600043B RID: 1083 RVA: 0x00010F00 File Offset: 0x0000F100
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return WrappersReflection.Descriptor.MessageTypes[7];
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x0600043C RID: 1084 RVA: 0x00010F12 File Offset: 0x0000F112
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return StringValue.Descriptor;
			}
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x00010F19 File Offset: 0x0000F119
		[DebuggerNonUserCode]
		public StringValue()
		{
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x00010F2C File Offset: 0x0000F12C
		[DebuggerNonUserCode]
		public StringValue(StringValue other)
			: this()
		{
			this.value_ = other.value_;
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x00010F51 File Offset: 0x0000F151
		[DebuggerNonUserCode]
		public StringValue Clone()
		{
			return new StringValue(this);
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x06000440 RID: 1088 RVA: 0x00010F59 File Offset: 0x0000F159
		// (set) Token: 0x06000441 RID: 1089 RVA: 0x00010F61 File Offset: 0x0000F161
		[DebuggerNonUserCode]
		public string Value
		{
			get
			{
				return this.value_;
			}
			set
			{
				this.value_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		// Token: 0x06000442 RID: 1090 RVA: 0x00010F74 File Offset: 0x0000F174
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as StringValue);
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x00010F82 File Offset: 0x0000F182
		[DebuggerNonUserCode]
		public bool Equals(StringValue other)
		{
			return other != null && (other == this || (!(this.Value != other.Value) && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x00010FB8 File Offset: 0x0000F1B8
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

		// Token: 0x06000445 RID: 1093 RVA: 0x00010FF9 File Offset: 0x0000F1F9
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x00011001 File Offset: 0x0000F201
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.Value.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteString(this.Value);
			}
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x00011038 File Offset: 0x0000F238
		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Value.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Value);
			}
			if (this._unknownFields != null)
			{
				num += this._unknownFields.CalculateSize();
			}
			return num;
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x0001107B File Offset: 0x0000F27B
		[DebuggerNonUserCode]
		public void MergeFrom(StringValue other)
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

		// Token: 0x06000449 RID: 1097 RVA: 0x000110B4 File Offset: 0x0000F2B4
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
					this.Value = input.ReadString();
				}
			}
		}

		// Token: 0x04000148 RID: 328
		private static readonly MessageParser<StringValue> _parser = new MessageParser<StringValue>(() => new StringValue());

		// Token: 0x04000149 RID: 329
		private UnknownFieldSet _unknownFields;

		// Token: 0x0400014A RID: 330
		public const int ValueFieldNumber = 1;

		// Token: 0x0400014B RID: 331
		private string value_ = "";
	}
}
