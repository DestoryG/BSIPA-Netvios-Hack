using System;
using System.Diagnostics;
using Google.Protobuf.Reflection;

namespace Google.Protobuf.WellKnownTypes
{
	// Token: 0x0200004B RID: 75
	public sealed class BoolValue : IMessage<BoolValue>, IMessage, IEquatable<BoolValue>, IDeepCloneable<BoolValue>
	{
		// Token: 0x170000CC RID: 204
		// (get) Token: 0x06000429 RID: 1065 RVA: 0x00010D21 File Offset: 0x0000EF21
		[DebuggerNonUserCode]
		public static MessageParser<BoolValue> Parser
		{
			get
			{
				return BoolValue._parser;
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x0600042A RID: 1066 RVA: 0x00010D28 File Offset: 0x0000EF28
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return WrappersReflection.Descriptor.MessageTypes[6];
			}
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x0600042B RID: 1067 RVA: 0x00010D3A File Offset: 0x0000EF3A
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return BoolValue.Descriptor;
			}
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x00010D41 File Offset: 0x0000EF41
		[DebuggerNonUserCode]
		public BoolValue()
		{
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x00010D49 File Offset: 0x0000EF49
		[DebuggerNonUserCode]
		public BoolValue(BoolValue other)
			: this()
		{
			this.value_ = other.value_;
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x00010D6E File Offset: 0x0000EF6E
		[DebuggerNonUserCode]
		public BoolValue Clone()
		{
			return new BoolValue(this);
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x0600042F RID: 1071 RVA: 0x00010D76 File Offset: 0x0000EF76
		// (set) Token: 0x06000430 RID: 1072 RVA: 0x00010D7E File Offset: 0x0000EF7E
		[DebuggerNonUserCode]
		public bool Value
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

		// Token: 0x06000431 RID: 1073 RVA: 0x00010D87 File Offset: 0x0000EF87
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as BoolValue);
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x00010D95 File Offset: 0x0000EF95
		[DebuggerNonUserCode]
		public bool Equals(BoolValue other)
		{
			return other != null && (other == this || (this.Value == other.Value && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x00010DC4 File Offset: 0x0000EFC4
		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (this.Value)
			{
				num ^= this.Value.GetHashCode();
			}
			if (this._unknownFields != null)
			{
				num ^= this._unknownFields.GetHashCode();
			}
			return num;
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x00010E03 File Offset: 0x0000F003
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x00010E0B File Offset: 0x0000F00B
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.Value)
			{
				output.WriteRawTag(8);
				output.WriteBool(this.Value);
			}
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x00010E3C File Offset: 0x0000F03C
		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Value)
			{
				num += 2;
			}
			if (this._unknownFields != null)
			{
				num += this._unknownFields.CalculateSize();
			}
			return num;
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x00010E6E File Offset: 0x0000F06E
		[DebuggerNonUserCode]
		public void MergeFrom(BoolValue other)
		{
			if (other == null)
			{
				return;
			}
			if (other.Value)
			{
				this.Value = other.Value;
			}
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x00010EA0 File Offset: 0x0000F0A0
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
					this.Value = input.ReadBool();
				}
			}
		}

		// Token: 0x04000144 RID: 324
		private static readonly MessageParser<BoolValue> _parser = new MessageParser<BoolValue>(() => new BoolValue());

		// Token: 0x04000145 RID: 325
		private UnknownFieldSet _unknownFields;

		// Token: 0x04000146 RID: 326
		public const int ValueFieldNumber = 1;

		// Token: 0x04000147 RID: 327
		private bool value_;
	}
}
