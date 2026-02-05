using System;
using System.Diagnostics;

namespace Google.Protobuf.Reflection
{
	// Token: 0x02000057 RID: 87
	public sealed class EnumValueDescriptorProto : IMessage<EnumValueDescriptorProto>, IMessage, IEquatable<EnumValueDescriptorProto>, IDeepCloneable<EnumValueDescriptorProto>
	{
		// Token: 0x17000129 RID: 297
		// (get) Token: 0x06000542 RID: 1346 RVA: 0x00014F1F File Offset: 0x0001311F
		[DebuggerNonUserCode]
		public static MessageParser<EnumValueDescriptorProto> Parser
		{
			get
			{
				return EnumValueDescriptorProto._parser;
			}
		}

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x06000543 RID: 1347 RVA: 0x00014F26 File Offset: 0x00013126
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return DescriptorReflection.Descriptor.MessageTypes[7];
			}
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x06000544 RID: 1348 RVA: 0x00014F38 File Offset: 0x00013138
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return EnumValueDescriptorProto.Descriptor;
			}
		}

		// Token: 0x06000545 RID: 1349 RVA: 0x00014F3F File Offset: 0x0001313F
		[DebuggerNonUserCode]
		public EnumValueDescriptorProto()
		{
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x00014F48 File Offset: 0x00013148
		[DebuggerNonUserCode]
		public EnumValueDescriptorProto(EnumValueDescriptorProto other)
			: this()
		{
			this._hasBits0 = other._hasBits0;
			this.name_ = other.name_;
			this.number_ = other.number_;
			this.options_ = ((other.options_ != null) ? other.options_.Clone() : null);
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x06000547 RID: 1351 RVA: 0x00014FAC File Offset: 0x000131AC
		[DebuggerNonUserCode]
		public EnumValueDescriptorProto Clone()
		{
			return new EnumValueDescriptorProto(this);
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x06000548 RID: 1352 RVA: 0x00014FB4 File Offset: 0x000131B4
		// (set) Token: 0x06000549 RID: 1353 RVA: 0x00014FC5 File Offset: 0x000131C5
		[DebuggerNonUserCode]
		public string Name
		{
			get
			{
				return this.name_ ?? EnumValueDescriptorProto.NameDefaultValue;
			}
			set
			{
				this.name_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x0600054A RID: 1354 RVA: 0x00014FD8 File Offset: 0x000131D8
		[DebuggerNonUserCode]
		public bool HasName
		{
			get
			{
				return this.name_ != null;
			}
		}

		// Token: 0x0600054B RID: 1355 RVA: 0x00014FE3 File Offset: 0x000131E3
		[DebuggerNonUserCode]
		public void ClearName()
		{
			this.name_ = null;
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x0600054C RID: 1356 RVA: 0x00014FEC File Offset: 0x000131EC
		// (set) Token: 0x0600054D RID: 1357 RVA: 0x00015004 File Offset: 0x00013204
		[DebuggerNonUserCode]
		public int Number
		{
			get
			{
				if ((this._hasBits0 & 1) != 0)
				{
					return this.number_;
				}
				return EnumValueDescriptorProto.NumberDefaultValue;
			}
			set
			{
				this._hasBits0 |= 1;
				this.number_ = value;
			}
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x0600054E RID: 1358 RVA: 0x0001501B File Offset: 0x0001321B
		[DebuggerNonUserCode]
		public bool HasNumber
		{
			get
			{
				return (this._hasBits0 & 1) != 0;
			}
		}

		// Token: 0x0600054F RID: 1359 RVA: 0x00015028 File Offset: 0x00013228
		[DebuggerNonUserCode]
		public void ClearNumber()
		{
			this._hasBits0 &= -2;
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x06000550 RID: 1360 RVA: 0x00015039 File Offset: 0x00013239
		// (set) Token: 0x06000551 RID: 1361 RVA: 0x00015041 File Offset: 0x00013241
		[DebuggerNonUserCode]
		public EnumValueOptions Options
		{
			get
			{
				return this.options_;
			}
			set
			{
				this.options_ = value;
			}
		}

		// Token: 0x06000552 RID: 1362 RVA: 0x0001504A File Offset: 0x0001324A
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as EnumValueDescriptorProto);
		}

		// Token: 0x06000553 RID: 1363 RVA: 0x00015058 File Offset: 0x00013258
		[DebuggerNonUserCode]
		public bool Equals(EnumValueDescriptorProto other)
		{
			return other != null && (other == this || (!(this.Name != other.Name) && this.Number == other.Number && object.Equals(this.Options, other.Options) && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x06000554 RID: 1364 RVA: 0x000150BC File Offset: 0x000132BC
		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (this.HasName)
			{
				num ^= this.Name.GetHashCode();
			}
			if (this.HasNumber)
			{
				num ^= this.Number.GetHashCode();
			}
			if (this.options_ != null)
			{
				num ^= this.Options.GetHashCode();
			}
			if (this._unknownFields != null)
			{
				num ^= this._unknownFields.GetHashCode();
			}
			return num;
		}

		// Token: 0x06000555 RID: 1365 RVA: 0x00015127 File Offset: 0x00013327
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x06000556 RID: 1366 RVA: 0x00015130 File Offset: 0x00013330
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.HasName)
			{
				output.WriteRawTag(10);
				output.WriteString(this.Name);
			}
			if (this.HasNumber)
			{
				output.WriteRawTag(16);
				output.WriteInt32(this.Number);
			}
			if (this.options_ != null)
			{
				output.WriteRawTag(26);
				output.WriteMessage(this.Options);
			}
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x06000557 RID: 1367 RVA: 0x000151A8 File Offset: 0x000133A8
		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.HasName)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Name);
			}
			if (this.HasNumber)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Number);
			}
			if (this.options_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.Options);
			}
			if (this._unknownFields != null)
			{
				num += this._unknownFields.CalculateSize();
			}
			return num;
		}

		// Token: 0x06000558 RID: 1368 RVA: 0x00015218 File Offset: 0x00013418
		[DebuggerNonUserCode]
		public void MergeFrom(EnumValueDescriptorProto other)
		{
			if (other == null)
			{
				return;
			}
			if (other.HasName)
			{
				this.Name = other.Name;
			}
			if (other.HasNumber)
			{
				this.Number = other.Number;
			}
			if (other.options_ != null)
			{
				if (this.options_ == null)
				{
					this.Options = new EnumValueOptions();
				}
				this.Options.MergeFrom(other.Options);
			}
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x06000559 RID: 1369 RVA: 0x00015294 File Offset: 0x00013494
		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num != 10U)
				{
					if (num != 16U)
					{
						if (num != 26U)
						{
							this._unknownFields = UnknownFieldSet.MergeFieldFrom(this._unknownFields, input);
						}
						else
						{
							if (this.options_ == null)
							{
								this.Options = new EnumValueOptions();
							}
							input.ReadMessage(this.Options);
						}
					}
					else
					{
						this.Number = input.ReadInt32();
					}
				}
				else
				{
					this.Name = input.ReadString();
				}
			}
		}

		// Token: 0x040001DB RID: 475
		private static readonly MessageParser<EnumValueDescriptorProto> _parser = new MessageParser<EnumValueDescriptorProto>(() => new EnumValueDescriptorProto());

		// Token: 0x040001DC RID: 476
		private UnknownFieldSet _unknownFields;

		// Token: 0x040001DD RID: 477
		private int _hasBits0;

		// Token: 0x040001DE RID: 478
		public const int NameFieldNumber = 1;

		// Token: 0x040001DF RID: 479
		private static readonly string NameDefaultValue = "";

		// Token: 0x040001E0 RID: 480
		private string name_;

		// Token: 0x040001E1 RID: 481
		public const int NumberFieldNumber = 2;

		// Token: 0x040001E2 RID: 482
		private static readonly int NumberDefaultValue = 0;

		// Token: 0x040001E3 RID: 483
		private int number_;

		// Token: 0x040001E4 RID: 484
		public const int OptionsFieldNumber = 3;

		// Token: 0x040001E5 RID: 485
		private EnumValueOptions options_;
	}
}
