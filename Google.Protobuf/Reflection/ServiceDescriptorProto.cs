using System;
using System.Diagnostics;
using Google.Protobuf.Collections;

namespace Google.Protobuf.Reflection
{
	// Token: 0x02000058 RID: 88
	public sealed class ServiceDescriptorProto : IMessage<ServiceDescriptorProto>, IMessage, IEquatable<ServiceDescriptorProto>, IDeepCloneable<ServiceDescriptorProto>
	{
		// Token: 0x17000131 RID: 305
		// (get) Token: 0x0600055B RID: 1371 RVA: 0x00015337 File Offset: 0x00013537
		[DebuggerNonUserCode]
		public static MessageParser<ServiceDescriptorProto> Parser
		{
			get
			{
				return ServiceDescriptorProto._parser;
			}
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x0600055C RID: 1372 RVA: 0x0001533E File Offset: 0x0001353E
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return DescriptorReflection.Descriptor.MessageTypes[8];
			}
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x0600055D RID: 1373 RVA: 0x00015350 File Offset: 0x00013550
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return ServiceDescriptorProto.Descriptor;
			}
		}

		// Token: 0x0600055E RID: 1374 RVA: 0x00015357 File Offset: 0x00013557
		[DebuggerNonUserCode]
		public ServiceDescriptorProto()
		{
		}

		// Token: 0x0600055F RID: 1375 RVA: 0x0001536C File Offset: 0x0001356C
		[DebuggerNonUserCode]
		public ServiceDescriptorProto(ServiceDescriptorProto other)
			: this()
		{
			this.name_ = other.name_;
			this.method_ = other.method_.Clone();
			this.options_ = ((other.options_ != null) ? other.options_.Clone() : null);
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x06000560 RID: 1376 RVA: 0x000153C9 File Offset: 0x000135C9
		[DebuggerNonUserCode]
		public ServiceDescriptorProto Clone()
		{
			return new ServiceDescriptorProto(this);
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x06000561 RID: 1377 RVA: 0x000153D1 File Offset: 0x000135D1
		// (set) Token: 0x06000562 RID: 1378 RVA: 0x000153E2 File Offset: 0x000135E2
		[DebuggerNonUserCode]
		public string Name
		{
			get
			{
				return this.name_ ?? ServiceDescriptorProto.NameDefaultValue;
			}
			set
			{
				this.name_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x06000563 RID: 1379 RVA: 0x000153F5 File Offset: 0x000135F5
		[DebuggerNonUserCode]
		public bool HasName
		{
			get
			{
				return this.name_ != null;
			}
		}

		// Token: 0x06000564 RID: 1380 RVA: 0x00015400 File Offset: 0x00013600
		[DebuggerNonUserCode]
		public void ClearName()
		{
			this.name_ = null;
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x06000565 RID: 1381 RVA: 0x00015409 File Offset: 0x00013609
		[DebuggerNonUserCode]
		public RepeatedField<MethodDescriptorProto> Method
		{
			get
			{
				return this.method_;
			}
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x06000566 RID: 1382 RVA: 0x00015411 File Offset: 0x00013611
		// (set) Token: 0x06000567 RID: 1383 RVA: 0x00015419 File Offset: 0x00013619
		[DebuggerNonUserCode]
		public ServiceOptions Options
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

		// Token: 0x06000568 RID: 1384 RVA: 0x00015422 File Offset: 0x00013622
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as ServiceDescriptorProto);
		}

		// Token: 0x06000569 RID: 1385 RVA: 0x00015430 File Offset: 0x00013630
		[DebuggerNonUserCode]
		public bool Equals(ServiceDescriptorProto other)
		{
			return other != null && (other == this || (!(this.Name != other.Name) && this.method_.Equals(other.method_) && object.Equals(this.Options, other.Options) && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x0600056A RID: 1386 RVA: 0x00015498 File Offset: 0x00013698
		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (this.HasName)
			{
				num ^= this.Name.GetHashCode();
			}
			num ^= this.method_.GetHashCode();
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

		// Token: 0x0600056B RID: 1387 RVA: 0x000154F8 File Offset: 0x000136F8
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x0600056C RID: 1388 RVA: 0x00015500 File Offset: 0x00013700
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.HasName)
			{
				output.WriteRawTag(10);
				output.WriteString(this.Name);
			}
			this.method_.WriteTo(output, ServiceDescriptorProto._repeated_method_codec);
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

		// Token: 0x0600056D RID: 1389 RVA: 0x0001556C File Offset: 0x0001376C
		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.HasName)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Name);
			}
			num += this.method_.CalculateSize(ServiceDescriptorProto._repeated_method_codec);
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

		// Token: 0x0600056E RID: 1390 RVA: 0x000155D8 File Offset: 0x000137D8
		[DebuggerNonUserCode]
		public void MergeFrom(ServiceDescriptorProto other)
		{
			if (other == null)
			{
				return;
			}
			if (other.HasName)
			{
				this.Name = other.Name;
			}
			this.method_.Add(other.method_);
			if (other.options_ != null)
			{
				if (this.options_ == null)
				{
					this.Options = new ServiceOptions();
				}
				this.Options.MergeFrom(other.Options);
			}
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x0600056F RID: 1391 RVA: 0x00015654 File Offset: 0x00013854
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
						if (num != 26U)
						{
							this._unknownFields = UnknownFieldSet.MergeFieldFrom(this._unknownFields, input);
						}
						else
						{
							if (this.options_ == null)
							{
								this.Options = new ServiceOptions();
							}
							input.ReadMessage(this.Options);
						}
					}
					else
					{
						this.method_.AddEntriesFrom(input, ServiceDescriptorProto._repeated_method_codec);
					}
				}
				else
				{
					this.Name = input.ReadString();
				}
			}
		}

		// Token: 0x040001E6 RID: 486
		private static readonly MessageParser<ServiceDescriptorProto> _parser = new MessageParser<ServiceDescriptorProto>(() => new ServiceDescriptorProto());

		// Token: 0x040001E7 RID: 487
		private UnknownFieldSet _unknownFields;

		// Token: 0x040001E8 RID: 488
		public const int NameFieldNumber = 1;

		// Token: 0x040001E9 RID: 489
		private static readonly string NameDefaultValue = "";

		// Token: 0x040001EA RID: 490
		private string name_;

		// Token: 0x040001EB RID: 491
		public const int MethodFieldNumber = 2;

		// Token: 0x040001EC RID: 492
		private static readonly FieldCodec<MethodDescriptorProto> _repeated_method_codec = FieldCodec.ForMessage<MethodDescriptorProto>(18U, MethodDescriptorProto.Parser);

		// Token: 0x040001ED RID: 493
		private readonly RepeatedField<MethodDescriptorProto> method_ = new RepeatedField<MethodDescriptorProto>();

		// Token: 0x040001EE RID: 494
		public const int OptionsFieldNumber = 3;

		// Token: 0x040001EF RID: 495
		private ServiceOptions options_;
	}
}
