using System;
using System.Diagnostics;
using Google.Protobuf.Reflection;

namespace Google.Protobuf.WellKnownTypes
{
	// Token: 0x02000028 RID: 40
	public sealed class Any : IMessage<Any>, IMessage, IEquatable<Any>, IDeepCloneable<Any>
	{
		// Token: 0x1700003D RID: 61
		// (get) Token: 0x0600020D RID: 525 RVA: 0x0000A5DB File Offset: 0x000087DB
		[DebuggerNonUserCode]
		public static MessageParser<Any> Parser
		{
			get
			{
				return Any._parser;
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x0600020E RID: 526 RVA: 0x0000A5E2 File Offset: 0x000087E2
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return AnyReflection.Descriptor.MessageTypes[0];
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x0600020F RID: 527 RVA: 0x0000A5F4 File Offset: 0x000087F4
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return Any.Descriptor;
			}
		}

		// Token: 0x06000210 RID: 528 RVA: 0x0000A5FB File Offset: 0x000087FB
		[DebuggerNonUserCode]
		public Any()
		{
		}

		// Token: 0x06000211 RID: 529 RVA: 0x0000A619 File Offset: 0x00008819
		[DebuggerNonUserCode]
		public Any(Any other)
			: this()
		{
			this.typeUrl_ = other.typeUrl_;
			this.value_ = other.value_;
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x06000212 RID: 530 RVA: 0x0000A64A File Offset: 0x0000884A
		[DebuggerNonUserCode]
		public Any Clone()
		{
			return new Any(this);
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000213 RID: 531 RVA: 0x0000A652 File Offset: 0x00008852
		// (set) Token: 0x06000214 RID: 532 RVA: 0x0000A65A File Offset: 0x0000885A
		[DebuggerNonUserCode]
		public string TypeUrl
		{
			get
			{
				return this.typeUrl_;
			}
			set
			{
				this.typeUrl_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000215 RID: 533 RVA: 0x0000A66D File Offset: 0x0000886D
		// (set) Token: 0x06000216 RID: 534 RVA: 0x0000A675 File Offset: 0x00008875
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

		// Token: 0x06000217 RID: 535 RVA: 0x0000A688 File Offset: 0x00008888
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as Any);
		}

		// Token: 0x06000218 RID: 536 RVA: 0x0000A698 File Offset: 0x00008898
		[DebuggerNonUserCode]
		public bool Equals(Any other)
		{
			return other != null && (other == this || (!(this.TypeUrl != other.TypeUrl) && !(this.Value != other.Value) && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x06000219 RID: 537 RVA: 0x0000A6EC File Offset: 0x000088EC
		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (this.TypeUrl.Length != 0)
			{
				num ^= this.TypeUrl.GetHashCode();
			}
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

		// Token: 0x0600021A RID: 538 RVA: 0x0000A748 File Offset: 0x00008948
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x0600021B RID: 539 RVA: 0x0000A750 File Offset: 0x00008950
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.TypeUrl.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteString(this.TypeUrl);
			}
			if (this.Value.Length != 0)
			{
				output.WriteRawTag(18);
				output.WriteBytes(this.Value);
			}
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x0600021C RID: 540 RVA: 0x0000A7B4 File Offset: 0x000089B4
		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.TypeUrl.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.TypeUrl);
			}
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

		// Token: 0x0600021D RID: 541 RVA: 0x0000A814 File Offset: 0x00008A14
		[DebuggerNonUserCode]
		public void MergeFrom(Any other)
		{
			if (other == null)
			{
				return;
			}
			if (other.TypeUrl.Length != 0)
			{
				this.TypeUrl = other.TypeUrl;
			}
			if (other.Value.Length != 0)
			{
				this.Value = other.Value;
			}
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x0600021E RID: 542 RVA: 0x0000A870 File Offset: 0x00008A70
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
						this.Value = input.ReadBytes();
					}
				}
				else
				{
					this.TypeUrl = input.ReadString();
				}
			}
		}

		// Token: 0x0600021F RID: 543 RVA: 0x0000A8C1 File Offset: 0x00008AC1
		private static string GetTypeUrl(MessageDescriptor descriptor, string prefix)
		{
			if (!prefix.EndsWith("/"))
			{
				return prefix + "/" + descriptor.FullName;
			}
			return prefix + descriptor.FullName;
		}

		// Token: 0x06000220 RID: 544 RVA: 0x0000A8F0 File Offset: 0x00008AF0
		public static string GetTypeName(string typeUrl)
		{
			ProtoPreconditions.CheckNotNull<string>(typeUrl, "typeUrl");
			int num = typeUrl.LastIndexOf('/');
			if (num != -1)
			{
				return typeUrl.Substring(num + 1);
			}
			return "";
		}

		// Token: 0x06000221 RID: 545 RVA: 0x0000A925 File Offset: 0x00008B25
		public bool Is(MessageDescriptor descriptor)
		{
			ProtoPreconditions.CheckNotNull<MessageDescriptor>(descriptor, "descriptor");
			return Any.GetTypeName(this.TypeUrl) == descriptor.FullName;
		}

		// Token: 0x06000222 RID: 546 RVA: 0x0000A94C File Offset: 0x00008B4C
		public T Unpack<T>() where T : IMessage, new()
		{
			T t = new T();
			if (Any.GetTypeName(this.TypeUrl) != t.Descriptor.FullName)
			{
				throw new InvalidProtocolBufferException(string.Concat(new string[]
				{
					"Full type name for ",
					t.Descriptor.Name,
					" is ",
					t.Descriptor.FullName,
					"; Any message's type url is ",
					this.TypeUrl
				}));
			}
			t.MergeFrom(this.Value);
			return t;
		}

		// Token: 0x06000223 RID: 547 RVA: 0x0000A9F4 File Offset: 0x00008BF4
		public bool TryUnpack<T>(out T result) where T : IMessage, new()
		{
			T t = new T();
			if (Any.GetTypeName(this.TypeUrl) != t.Descriptor.FullName)
			{
				result = default(T);
				return false;
			}
			t.MergeFrom(this.Value);
			result = t;
			return true;
		}

		// Token: 0x06000224 RID: 548 RVA: 0x0000AA4D File Offset: 0x00008C4D
		public static Any Pack(IMessage message)
		{
			return Any.Pack(message, "type.googleapis.com");
		}

		// Token: 0x06000225 RID: 549 RVA: 0x0000AA5A File Offset: 0x00008C5A
		public static Any Pack(IMessage message, string typeUrlPrefix)
		{
			ProtoPreconditions.CheckNotNull<IMessage>(message, "message");
			ProtoPreconditions.CheckNotNull<string>(typeUrlPrefix, "typeUrlPrefix");
			return new Any
			{
				TypeUrl = Any.GetTypeUrl(message.Descriptor, typeUrlPrefix),
				Value = message.ToByteString()
			};
		}

		// Token: 0x0400006F RID: 111
		private static readonly MessageParser<Any> _parser = new MessageParser<Any>(() => new Any());

		// Token: 0x04000070 RID: 112
		private UnknownFieldSet _unknownFields;

		// Token: 0x04000071 RID: 113
		public const int TypeUrlFieldNumber = 1;

		// Token: 0x04000072 RID: 114
		private string typeUrl_ = "";

		// Token: 0x04000073 RID: 115
		public const int ValueFieldNumber = 2;

		// Token: 0x04000074 RID: 116
		private ByteString value_ = ByteString.Empty;

		// Token: 0x04000075 RID: 117
		private const string DefaultPrefix = "type.googleapis.com";
	}
}
