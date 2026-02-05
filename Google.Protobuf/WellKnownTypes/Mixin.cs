using System;
using System.Diagnostics;
using Google.Protobuf.Reflection;

namespace Google.Protobuf.WellKnownTypes
{
	// Token: 0x0200002C RID: 44
	public sealed class Mixin : IMessage<Mixin>, IMessage, IEquatable<Mixin>, IDeepCloneable<Mixin>
	{
		// Token: 0x17000057 RID: 87
		// (get) Token: 0x0600025F RID: 607 RVA: 0x0000B996 File Offset: 0x00009B96
		[DebuggerNonUserCode]
		public static MessageParser<Mixin> Parser
		{
			get
			{
				return Mixin._parser;
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000260 RID: 608 RVA: 0x0000B99D File Offset: 0x00009B9D
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return ApiReflection.Descriptor.MessageTypes[2];
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000261 RID: 609 RVA: 0x0000B9AF File Offset: 0x00009BAF
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return Mixin.Descriptor;
			}
		}

		// Token: 0x06000262 RID: 610 RVA: 0x0000B9B6 File Offset: 0x00009BB6
		[DebuggerNonUserCode]
		public Mixin()
		{
		}

		// Token: 0x06000263 RID: 611 RVA: 0x0000B9D4 File Offset: 0x00009BD4
		[DebuggerNonUserCode]
		public Mixin(Mixin other)
			: this()
		{
			this.name_ = other.name_;
			this.root_ = other.root_;
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x06000264 RID: 612 RVA: 0x0000BA05 File Offset: 0x00009C05
		[DebuggerNonUserCode]
		public Mixin Clone()
		{
			return new Mixin(this);
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000265 RID: 613 RVA: 0x0000BA0D File Offset: 0x00009C0D
		// (set) Token: 0x06000266 RID: 614 RVA: 0x0000BA15 File Offset: 0x00009C15
		[DebuggerNonUserCode]
		public string Name
		{
			get
			{
				return this.name_;
			}
			set
			{
				this.name_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000267 RID: 615 RVA: 0x0000BA28 File Offset: 0x00009C28
		// (set) Token: 0x06000268 RID: 616 RVA: 0x0000BA30 File Offset: 0x00009C30
		[DebuggerNonUserCode]
		public string Root
		{
			get
			{
				return this.root_;
			}
			set
			{
				this.root_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		// Token: 0x06000269 RID: 617 RVA: 0x0000BA43 File Offset: 0x00009C43
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as Mixin);
		}

		// Token: 0x0600026A RID: 618 RVA: 0x0000BA54 File Offset: 0x00009C54
		[DebuggerNonUserCode]
		public bool Equals(Mixin other)
		{
			return other != null && (other == this || (!(this.Name != other.Name) && !(this.Root != other.Root) && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x0600026B RID: 619 RVA: 0x0000BAA8 File Offset: 0x00009CA8
		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (this.Name.Length != 0)
			{
				num ^= this.Name.GetHashCode();
			}
			if (this.Root.Length != 0)
			{
				num ^= this.Root.GetHashCode();
			}
			if (this._unknownFields != null)
			{
				num ^= this._unknownFields.GetHashCode();
			}
			return num;
		}

		// Token: 0x0600026C RID: 620 RVA: 0x0000BB04 File Offset: 0x00009D04
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x0600026D RID: 621 RVA: 0x0000BB0C File Offset: 0x00009D0C
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.Name.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteString(this.Name);
			}
			if (this.Root.Length != 0)
			{
				output.WriteRawTag(18);
				output.WriteString(this.Root);
			}
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x0600026E RID: 622 RVA: 0x0000BB70 File Offset: 0x00009D70
		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Name.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Name);
			}
			if (this.Root.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Root);
			}
			if (this._unknownFields != null)
			{
				num += this._unknownFields.CalculateSize();
			}
			return num;
		}

		// Token: 0x0600026F RID: 623 RVA: 0x0000BBD0 File Offset: 0x00009DD0
		[DebuggerNonUserCode]
		public void MergeFrom(Mixin other)
		{
			if (other == null)
			{
				return;
			}
			if (other.Name.Length != 0)
			{
				this.Name = other.Name;
			}
			if (other.Root.Length != 0)
			{
				this.Root = other.Root;
			}
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x06000270 RID: 624 RVA: 0x0000BC2C File Offset: 0x00009E2C
		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num != 10U)
				{
					if (num != 18U)
					{
						this._unknownFields = UnknownFieldSet.MergeFieldFrom(this._unknownFields, input);
					}
					else
					{
						this.Root = input.ReadString();
					}
				}
				else
				{
					this.Name = input.ReadString();
				}
			}
		}

		// Token: 0x0400009B RID: 155
		private static readonly MessageParser<Mixin> _parser = new MessageParser<Mixin>(() => new Mixin());

		// Token: 0x0400009C RID: 156
		private UnknownFieldSet _unknownFields;

		// Token: 0x0400009D RID: 157
		public const int NameFieldNumber = 1;

		// Token: 0x0400009E RID: 158
		private string name_ = "";

		// Token: 0x0400009F RID: 159
		public const int RootFieldNumber = 2;

		// Token: 0x040000A0 RID: 160
		private string root_ = "";
	}
}
