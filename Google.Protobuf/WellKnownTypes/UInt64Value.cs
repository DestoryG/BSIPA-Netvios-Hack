using System;
using System.Diagnostics;
using Google.Protobuf.Reflection;

namespace Google.Protobuf.WellKnownTypes
{
	// Token: 0x02000048 RID: 72
	public sealed class UInt64Value : IMessage<UInt64Value>, IMessage, IEquatable<UInt64Value>, IDeepCloneable<UInt64Value>
	{
		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x060003F6 RID: 1014 RVA: 0x00010775 File Offset: 0x0000E975
		[DebuggerNonUserCode]
		public static MessageParser<UInt64Value> Parser
		{
			get
			{
				return UInt64Value._parser;
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x060003F7 RID: 1015 RVA: 0x0001077C File Offset: 0x0000E97C
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return WrappersReflection.Descriptor.MessageTypes[3];
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x060003F8 RID: 1016 RVA: 0x0001078E File Offset: 0x0000E98E
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return UInt64Value.Descriptor;
			}
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x00010795 File Offset: 0x0000E995
		[DebuggerNonUserCode]
		public UInt64Value()
		{
		}

		// Token: 0x060003FA RID: 1018 RVA: 0x0001079D File Offset: 0x0000E99D
		[DebuggerNonUserCode]
		public UInt64Value(UInt64Value other)
			: this()
		{
			this.value_ = other.value_;
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x060003FB RID: 1019 RVA: 0x000107C2 File Offset: 0x0000E9C2
		[DebuggerNonUserCode]
		public UInt64Value Clone()
		{
			return new UInt64Value(this);
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x060003FC RID: 1020 RVA: 0x000107CA File Offset: 0x0000E9CA
		// (set) Token: 0x060003FD RID: 1021 RVA: 0x000107D2 File Offset: 0x0000E9D2
		[DebuggerNonUserCode]
		public ulong Value
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

		// Token: 0x060003FE RID: 1022 RVA: 0x000107DB File Offset: 0x0000E9DB
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as UInt64Value);
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x000107E9 File Offset: 0x0000E9E9
		[DebuggerNonUserCode]
		public bool Equals(UInt64Value other)
		{
			return other != null && (other == this || (this.Value == other.Value && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x00010818 File Offset: 0x0000EA18
		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (this.Value != 0UL)
			{
				num ^= this.Value.GetHashCode();
			}
			if (this._unknownFields != null)
			{
				num ^= this._unknownFields.GetHashCode();
			}
			return num;
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x00010857 File Offset: 0x0000EA57
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x06000402 RID: 1026 RVA: 0x0001085F File Offset: 0x0000EA5F
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.Value != 0UL)
			{
				output.WriteRawTag(8);
				output.WriteUInt64(this.Value);
			}
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x00010890 File Offset: 0x0000EA90
		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Value != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.Value);
			}
			if (this._unknownFields != null)
			{
				num += this._unknownFields.CalculateSize();
			}
			return num;
		}

		// Token: 0x06000404 RID: 1028 RVA: 0x000108CE File Offset: 0x0000EACE
		[DebuggerNonUserCode]
		public void MergeFrom(UInt64Value other)
		{
			if (other == null)
			{
				return;
			}
			if (other.Value != 0UL)
			{
				this.Value = other.Value;
			}
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x00010900 File Offset: 0x0000EB00
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
					this.Value = input.ReadUInt64();
				}
			}
		}

		// Token: 0x04000138 RID: 312
		private static readonly MessageParser<UInt64Value> _parser = new MessageParser<UInt64Value>(() => new UInt64Value());

		// Token: 0x04000139 RID: 313
		private UnknownFieldSet _unknownFields;

		// Token: 0x0400013A RID: 314
		public const int ValueFieldNumber = 1;

		// Token: 0x0400013B RID: 315
		private ulong value_;
	}
}
