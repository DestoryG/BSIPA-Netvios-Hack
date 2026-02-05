using System;
using System.Diagnostics;

namespace Google.Protobuf.Reflection
{
	// Token: 0x02000059 RID: 89
	public sealed class MethodDescriptorProto : IMessage<MethodDescriptorProto>, IMessage, IEquatable<MethodDescriptorProto>, IDeepCloneable<MethodDescriptorProto>
	{
		// Token: 0x17000138 RID: 312
		// (get) Token: 0x06000571 RID: 1393 RVA: 0x00015707 File Offset: 0x00013907
		[DebuggerNonUserCode]
		public static MessageParser<MethodDescriptorProto> Parser
		{
			get
			{
				return MethodDescriptorProto._parser;
			}
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x06000572 RID: 1394 RVA: 0x0001570E File Offset: 0x0001390E
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return DescriptorReflection.Descriptor.MessageTypes[9];
			}
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x06000573 RID: 1395 RVA: 0x00015721 File Offset: 0x00013921
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return MethodDescriptorProto.Descriptor;
			}
		}

		// Token: 0x06000574 RID: 1396 RVA: 0x00015728 File Offset: 0x00013928
		[DebuggerNonUserCode]
		public MethodDescriptorProto()
		{
		}

		// Token: 0x06000575 RID: 1397 RVA: 0x00015730 File Offset: 0x00013930
		[DebuggerNonUserCode]
		public MethodDescriptorProto(MethodDescriptorProto other)
			: this()
		{
			this._hasBits0 = other._hasBits0;
			this.name_ = other.name_;
			this.inputType_ = other.inputType_;
			this.outputType_ = other.outputType_;
			this.options_ = ((other.options_ != null) ? other.options_.Clone() : null);
			this.clientStreaming_ = other.clientStreaming_;
			this.serverStreaming_ = other.serverStreaming_;
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x06000576 RID: 1398 RVA: 0x000157B8 File Offset: 0x000139B8
		[DebuggerNonUserCode]
		public MethodDescriptorProto Clone()
		{
			return new MethodDescriptorProto(this);
		}

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x06000577 RID: 1399 RVA: 0x000157C0 File Offset: 0x000139C0
		// (set) Token: 0x06000578 RID: 1400 RVA: 0x000157D1 File Offset: 0x000139D1
		[DebuggerNonUserCode]
		public string Name
		{
			get
			{
				return this.name_ ?? MethodDescriptorProto.NameDefaultValue;
			}
			set
			{
				this.name_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x06000579 RID: 1401 RVA: 0x000157E4 File Offset: 0x000139E4
		[DebuggerNonUserCode]
		public bool HasName
		{
			get
			{
				return this.name_ != null;
			}
		}

		// Token: 0x0600057A RID: 1402 RVA: 0x000157EF File Offset: 0x000139EF
		[DebuggerNonUserCode]
		public void ClearName()
		{
			this.name_ = null;
		}

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x0600057B RID: 1403 RVA: 0x000157F8 File Offset: 0x000139F8
		// (set) Token: 0x0600057C RID: 1404 RVA: 0x00015809 File Offset: 0x00013A09
		[DebuggerNonUserCode]
		public string InputType
		{
			get
			{
				return this.inputType_ ?? MethodDescriptorProto.InputTypeDefaultValue;
			}
			set
			{
				this.inputType_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x0600057D RID: 1405 RVA: 0x0001581C File Offset: 0x00013A1C
		[DebuggerNonUserCode]
		public bool HasInputType
		{
			get
			{
				return this.inputType_ != null;
			}
		}

		// Token: 0x0600057E RID: 1406 RVA: 0x00015827 File Offset: 0x00013A27
		[DebuggerNonUserCode]
		public void ClearInputType()
		{
			this.inputType_ = null;
		}

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x0600057F RID: 1407 RVA: 0x00015830 File Offset: 0x00013A30
		// (set) Token: 0x06000580 RID: 1408 RVA: 0x00015841 File Offset: 0x00013A41
		[DebuggerNonUserCode]
		public string OutputType
		{
			get
			{
				return this.outputType_ ?? MethodDescriptorProto.OutputTypeDefaultValue;
			}
			set
			{
				this.outputType_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x06000581 RID: 1409 RVA: 0x00015854 File Offset: 0x00013A54
		[DebuggerNonUserCode]
		public bool HasOutputType
		{
			get
			{
				return this.outputType_ != null;
			}
		}

		// Token: 0x06000582 RID: 1410 RVA: 0x0001585F File Offset: 0x00013A5F
		[DebuggerNonUserCode]
		public void ClearOutputType()
		{
			this.outputType_ = null;
		}

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x06000583 RID: 1411 RVA: 0x00015868 File Offset: 0x00013A68
		// (set) Token: 0x06000584 RID: 1412 RVA: 0x00015870 File Offset: 0x00013A70
		[DebuggerNonUserCode]
		public MethodOptions Options
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

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x06000585 RID: 1413 RVA: 0x00015879 File Offset: 0x00013A79
		// (set) Token: 0x06000586 RID: 1414 RVA: 0x00015891 File Offset: 0x00013A91
		[DebuggerNonUserCode]
		public bool ClientStreaming
		{
			get
			{
				if ((this._hasBits0 & 1) != 0)
				{
					return this.clientStreaming_;
				}
				return MethodDescriptorProto.ClientStreamingDefaultValue;
			}
			set
			{
				this._hasBits0 |= 1;
				this.clientStreaming_ = value;
			}
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x06000587 RID: 1415 RVA: 0x000158A8 File Offset: 0x00013AA8
		[DebuggerNonUserCode]
		public bool HasClientStreaming
		{
			get
			{
				return (this._hasBits0 & 1) != 0;
			}
		}

		// Token: 0x06000588 RID: 1416 RVA: 0x000158B5 File Offset: 0x00013AB5
		[DebuggerNonUserCode]
		public void ClearClientStreaming()
		{
			this._hasBits0 &= -2;
		}

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x06000589 RID: 1417 RVA: 0x000158C6 File Offset: 0x00013AC6
		// (set) Token: 0x0600058A RID: 1418 RVA: 0x000158DE File Offset: 0x00013ADE
		[DebuggerNonUserCode]
		public bool ServerStreaming
		{
			get
			{
				if ((this._hasBits0 & 2) != 0)
				{
					return this.serverStreaming_;
				}
				return MethodDescriptorProto.ServerStreamingDefaultValue;
			}
			set
			{
				this._hasBits0 |= 2;
				this.serverStreaming_ = value;
			}
		}

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x0600058B RID: 1419 RVA: 0x000158F5 File Offset: 0x00013AF5
		[DebuggerNonUserCode]
		public bool HasServerStreaming
		{
			get
			{
				return (this._hasBits0 & 2) != 0;
			}
		}

		// Token: 0x0600058C RID: 1420 RVA: 0x00015902 File Offset: 0x00013B02
		[DebuggerNonUserCode]
		public void ClearServerStreaming()
		{
			this._hasBits0 &= -3;
		}

		// Token: 0x0600058D RID: 1421 RVA: 0x00015913 File Offset: 0x00013B13
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as MethodDescriptorProto);
		}

		// Token: 0x0600058E RID: 1422 RVA: 0x00015924 File Offset: 0x00013B24
		[DebuggerNonUserCode]
		public bool Equals(MethodDescriptorProto other)
		{
			return other != null && (other == this || (!(this.Name != other.Name) && !(this.InputType != other.InputType) && !(this.OutputType != other.OutputType) && object.Equals(this.Options, other.Options) && this.ClientStreaming == other.ClientStreaming && this.ServerStreaming == other.ServerStreaming && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x0600058F RID: 1423 RVA: 0x000159C4 File Offset: 0x00013BC4
		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (this.HasName)
			{
				num ^= this.Name.GetHashCode();
			}
			if (this.HasInputType)
			{
				num ^= this.InputType.GetHashCode();
			}
			if (this.HasOutputType)
			{
				num ^= this.OutputType.GetHashCode();
			}
			if (this.options_ != null)
			{
				num ^= this.Options.GetHashCode();
			}
			if (this.HasClientStreaming)
			{
				num ^= this.ClientStreaming.GetHashCode();
			}
			if (this.HasServerStreaming)
			{
				num ^= this.ServerStreaming.GetHashCode();
			}
			if (this._unknownFields != null)
			{
				num ^= this._unknownFields.GetHashCode();
			}
			return num;
		}

		// Token: 0x06000590 RID: 1424 RVA: 0x00015A74 File Offset: 0x00013C74
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x06000591 RID: 1425 RVA: 0x00015A7C File Offset: 0x00013C7C
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.HasName)
			{
				output.WriteRawTag(10);
				output.WriteString(this.Name);
			}
			if (this.HasInputType)
			{
				output.WriteRawTag(18);
				output.WriteString(this.InputType);
			}
			if (this.HasOutputType)
			{
				output.WriteRawTag(26);
				output.WriteString(this.OutputType);
			}
			if (this.options_ != null)
			{
				output.WriteRawTag(34);
				output.WriteMessage(this.Options);
			}
			if (this.HasClientStreaming)
			{
				output.WriteRawTag(40);
				output.WriteBool(this.ClientStreaming);
			}
			if (this.HasServerStreaming)
			{
				output.WriteRawTag(48);
				output.WriteBool(this.ServerStreaming);
			}
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x06000592 RID: 1426 RVA: 0x00015B48 File Offset: 0x00013D48
		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.HasName)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Name);
			}
			if (this.HasInputType)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.InputType);
			}
			if (this.HasOutputType)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.OutputType);
			}
			if (this.options_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.Options);
			}
			if (this.HasClientStreaming)
			{
				num += 2;
			}
			if (this.HasServerStreaming)
			{
				num += 2;
			}
			if (this._unknownFields != null)
			{
				num += this._unknownFields.CalculateSize();
			}
			return num;
		}

		// Token: 0x06000593 RID: 1427 RVA: 0x00015BE8 File Offset: 0x00013DE8
		[DebuggerNonUserCode]
		public void MergeFrom(MethodDescriptorProto other)
		{
			if (other == null)
			{
				return;
			}
			if (other.HasName)
			{
				this.Name = other.Name;
			}
			if (other.HasInputType)
			{
				this.InputType = other.InputType;
			}
			if (other.HasOutputType)
			{
				this.OutputType = other.OutputType;
			}
			if (other.options_ != null)
			{
				if (this.options_ == null)
				{
					this.Options = new MethodOptions();
				}
				this.Options.MergeFrom(other.Options);
			}
			if (other.HasClientStreaming)
			{
				this.ClientStreaming = other.ClientStreaming;
			}
			if (other.HasServerStreaming)
			{
				this.ServerStreaming = other.ServerStreaming;
			}
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x06000594 RID: 1428 RVA: 0x00015CA0 File Offset: 0x00013EA0
		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 26U)
				{
					if (num == 10U)
					{
						this.Name = input.ReadString();
						continue;
					}
					if (num == 18U)
					{
						this.InputType = input.ReadString();
						continue;
					}
					if (num == 26U)
					{
						this.OutputType = input.ReadString();
						continue;
					}
				}
				else
				{
					if (num == 34U)
					{
						if (this.options_ == null)
						{
							this.Options = new MethodOptions();
						}
						input.ReadMessage(this.Options);
						continue;
					}
					if (num == 40U)
					{
						this.ClientStreaming = input.ReadBool();
						continue;
					}
					if (num == 48U)
					{
						this.ServerStreaming = input.ReadBool();
						continue;
					}
				}
				this._unknownFields = UnknownFieldSet.MergeFieldFrom(this._unknownFields, input);
			}
		}

		// Token: 0x040001F0 RID: 496
		private static readonly MessageParser<MethodDescriptorProto> _parser = new MessageParser<MethodDescriptorProto>(() => new MethodDescriptorProto());

		// Token: 0x040001F1 RID: 497
		private UnknownFieldSet _unknownFields;

		// Token: 0x040001F2 RID: 498
		private int _hasBits0;

		// Token: 0x040001F3 RID: 499
		public const int NameFieldNumber = 1;

		// Token: 0x040001F4 RID: 500
		private static readonly string NameDefaultValue = "";

		// Token: 0x040001F5 RID: 501
		private string name_;

		// Token: 0x040001F6 RID: 502
		public const int InputTypeFieldNumber = 2;

		// Token: 0x040001F7 RID: 503
		private static readonly string InputTypeDefaultValue = "";

		// Token: 0x040001F8 RID: 504
		private string inputType_;

		// Token: 0x040001F9 RID: 505
		public const int OutputTypeFieldNumber = 3;

		// Token: 0x040001FA RID: 506
		private static readonly string OutputTypeDefaultValue = "";

		// Token: 0x040001FB RID: 507
		private string outputType_;

		// Token: 0x040001FC RID: 508
		public const int OptionsFieldNumber = 4;

		// Token: 0x040001FD RID: 509
		private MethodOptions options_;

		// Token: 0x040001FE RID: 510
		public const int ClientStreamingFieldNumber = 5;

		// Token: 0x040001FF RID: 511
		private static readonly bool ClientStreamingDefaultValue = false;

		// Token: 0x04000200 RID: 512
		private bool clientStreaming_;

		// Token: 0x04000201 RID: 513
		public const int ServerStreamingFieldNumber = 6;

		// Token: 0x04000202 RID: 514
		private static readonly bool ServerStreamingDefaultValue = false;

		// Token: 0x04000203 RID: 515
		private bool serverStreaming_;
	}
}
