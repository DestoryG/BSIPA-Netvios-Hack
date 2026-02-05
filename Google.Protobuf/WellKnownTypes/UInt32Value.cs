using System;
using System.Diagnostics;
using Google.Protobuf.Reflection;

namespace Google.Protobuf.WellKnownTypes
{
	// Token: 0x0200004A RID: 74
	public sealed class UInt32Value : IMessage<UInt32Value>, IMessage, IEquatable<UInt32Value>, IDeepCloneable<UInt32Value>
	{
		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000418 RID: 1048 RVA: 0x00010B3D File Offset: 0x0000ED3D
		[DebuggerNonUserCode]
		public static MessageParser<UInt32Value> Parser
		{
			get
			{
				return UInt32Value._parser;
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000419 RID: 1049 RVA: 0x00010B44 File Offset: 0x0000ED44
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return WrappersReflection.Descriptor.MessageTypes[5];
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x0600041A RID: 1050 RVA: 0x00010B56 File Offset: 0x0000ED56
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return UInt32Value.Descriptor;
			}
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x00010B5D File Offset: 0x0000ED5D
		[DebuggerNonUserCode]
		public UInt32Value()
		{
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x00010B65 File Offset: 0x0000ED65
		[DebuggerNonUserCode]
		public UInt32Value(UInt32Value other)
			: this()
		{
			this.value_ = other.value_;
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x0600041D RID: 1053 RVA: 0x00010B8A File Offset: 0x0000ED8A
		[DebuggerNonUserCode]
		public UInt32Value Clone()
		{
			return new UInt32Value(this);
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x0600041E RID: 1054 RVA: 0x00010B92 File Offset: 0x0000ED92
		// (set) Token: 0x0600041F RID: 1055 RVA: 0x00010B9A File Offset: 0x0000ED9A
		[DebuggerNonUserCode]
		public uint Value
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

		// Token: 0x06000420 RID: 1056 RVA: 0x00010BA3 File Offset: 0x0000EDA3
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as UInt32Value);
		}

		// Token: 0x06000421 RID: 1057 RVA: 0x00010BB1 File Offset: 0x0000EDB1
		[DebuggerNonUserCode]
		public bool Equals(UInt32Value other)
		{
			return other != null && (other == this || (this.Value == other.Value && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x00010BE0 File Offset: 0x0000EDE0
		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (this.Value != 0U)
			{
				num ^= this.Value.GetHashCode();
			}
			if (this._unknownFields != null)
			{
				num ^= this._unknownFields.GetHashCode();
			}
			return num;
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x00010C1F File Offset: 0x0000EE1F
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x00010C27 File Offset: 0x0000EE27
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.Value != 0U)
			{
				output.WriteRawTag(8);
				output.WriteUInt32(this.Value);
			}
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x00010C58 File Offset: 0x0000EE58
		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Value != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Value);
			}
			if (this._unknownFields != null)
			{
				num += this._unknownFields.CalculateSize();
			}
			return num;
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x00010C96 File Offset: 0x0000EE96
		[DebuggerNonUserCode]
		public void MergeFrom(UInt32Value other)
		{
			if (other == null)
			{
				return;
			}
			if (other.Value != 0U)
			{
				this.Value = other.Value;
			}
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x00010CC8 File Offset: 0x0000EEC8
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
					this.Value = input.ReadUInt32();
				}
			}
		}

		// Token: 0x04000140 RID: 320
		private static readonly MessageParser<UInt32Value> _parser = new MessageParser<UInt32Value>(() => new UInt32Value());

		// Token: 0x04000141 RID: 321
		private UnknownFieldSet _unknownFields;

		// Token: 0x04000142 RID: 322
		public const int ValueFieldNumber = 1;

		// Token: 0x04000143 RID: 323
		private uint value_;
	}
}
