using System;
using System.Collections.Generic;
using System.Reflection;
using Google.Protobuf.Collections;

namespace Google.Protobuf.Reflection
{
	// Token: 0x0200006F RID: 111
	public sealed class FieldDescriptor : DescriptorBase, IComparable<FieldDescriptor>
	{
		// Token: 0x170001EB RID: 491
		// (get) Token: 0x06000778 RID: 1912 RVA: 0x0001AF0E File Offset: 0x0001910E
		public MessageDescriptor ContainingType { get; }

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x06000779 RID: 1913 RVA: 0x0001AF16 File Offset: 0x00019116
		public OneofDescriptor ContainingOneof { get; }

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x0600077A RID: 1914 RVA: 0x0001AF1E File Offset: 0x0001911E
		public OneofDescriptor RealContainingOneof
		{
			get
			{
				OneofDescriptor containingOneof = this.ContainingOneof;
				if (containingOneof == null || containingOneof.IsSynthetic)
				{
					return null;
				}
				return this.ContainingOneof;
			}
		}

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x0600077B RID: 1915 RVA: 0x0001AF3F File Offset: 0x0001913F
		public string JsonName { get; }

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x0600077C RID: 1916 RVA: 0x0001AF48 File Offset: 0x00019148
		public bool HasPresence
		{
			get
			{
				if (this.Extension == null)
				{
					return !this.IsRepeated && !this.IsMap && (this.FieldType == FieldType.Message || this.ContainingOneof != null || base.File.Syntax == Syntax.Proto2);
				}
				return !this.Extension.IsRepeated;
			}
		}

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x0600077D RID: 1917 RVA: 0x0001AFA4 File Offset: 0x000191A4
		internal FieldDescriptorProto Proto { get; }

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x0600077E RID: 1918 RVA: 0x0001AFAC File Offset: 0x000191AC
		public Extension Extension { get; }

		// Token: 0x0600077F RID: 1919 RVA: 0x0001AFB4 File Offset: 0x000191B4
		internal FieldDescriptor(FieldDescriptorProto proto, FileDescriptor file, MessageDescriptor parent, int index, string propertyName, Extension extension)
			: base(file, file.ComputeFullName(parent, proto.Name), index)
		{
			this.Proto = proto;
			if (proto.Type != (FieldDescriptorProto.Types.Type)0)
			{
				this.fieldType = FieldDescriptor.GetFieldTypeFromProtoType(proto.Type);
			}
			if (this.FieldNumber <= 0)
			{
				throw new DescriptorValidationException(this, "Field numbers must be positive integers.");
			}
			this.ContainingType = parent;
			if (proto.HasOneofIndex)
			{
				if (proto.OneofIndex < 0 || proto.OneofIndex >= parent.Proto.OneofDecl.Count)
				{
					throw new DescriptorValidationException(this, "FieldDescriptorProto.oneof_index is out of range for type " + parent.Name);
				}
				this.ContainingOneof = parent.Oneofs[proto.OneofIndex];
			}
			file.DescriptorPool.AddSymbol(this);
			this.propertyName = propertyName;
			this.Extension = extension;
			this.JsonName = ((this.Proto.JsonName == "") ? JsonFormatter.ToJsonName(this.Proto.Name) : this.Proto.JsonName);
		}

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x06000780 RID: 1920 RVA: 0x0001B0C0 File Offset: 0x000192C0
		public override string Name
		{
			get
			{
				return this.Proto.Name;
			}
		}

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x06000781 RID: 1921 RVA: 0x0001B0CD File Offset: 0x000192CD
		public IFieldAccessor Accessor
		{
			get
			{
				return this.accessor;
			}
		}

		// Token: 0x06000782 RID: 1922 RVA: 0x0001B0D8 File Offset: 0x000192D8
		private static FieldType GetFieldTypeFromProtoType(FieldDescriptorProto.Types.Type type)
		{
			switch (type)
			{
			case FieldDescriptorProto.Types.Type.Double:
				return FieldType.Double;
			case FieldDescriptorProto.Types.Type.Float:
				return FieldType.Float;
			case FieldDescriptorProto.Types.Type.Int64:
				return FieldType.Int64;
			case FieldDescriptorProto.Types.Type.Uint64:
				return FieldType.UInt64;
			case FieldDescriptorProto.Types.Type.Int32:
				return FieldType.Int32;
			case FieldDescriptorProto.Types.Type.Fixed64:
				return FieldType.Fixed64;
			case FieldDescriptorProto.Types.Type.Fixed32:
				return FieldType.Fixed32;
			case FieldDescriptorProto.Types.Type.Bool:
				return FieldType.Bool;
			case FieldDescriptorProto.Types.Type.String:
				return FieldType.String;
			case FieldDescriptorProto.Types.Type.Group:
				return FieldType.Group;
			case FieldDescriptorProto.Types.Type.Message:
				return FieldType.Message;
			case FieldDescriptorProto.Types.Type.Bytes:
				return FieldType.Bytes;
			case FieldDescriptorProto.Types.Type.Uint32:
				return FieldType.UInt32;
			case FieldDescriptorProto.Types.Type.Enum:
				return FieldType.Enum;
			case FieldDescriptorProto.Types.Type.Sfixed32:
				return FieldType.SFixed32;
			case FieldDescriptorProto.Types.Type.Sfixed64:
				return FieldType.SFixed64;
			case FieldDescriptorProto.Types.Type.Sint32:
				return FieldType.SInt32;
			case FieldDescriptorProto.Types.Type.Sint64:
				return FieldType.SInt64;
			default:
				throw new ArgumentException("Invalid type specified");
			}
		}

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x06000783 RID: 1923 RVA: 0x0001B16E File Offset: 0x0001936E
		public bool IsRepeated
		{
			get
			{
				return this.Proto.Label == FieldDescriptorProto.Types.Label.Repeated;
			}
		}

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x06000784 RID: 1924 RVA: 0x0001B17E File Offset: 0x0001937E
		public bool IsRequired
		{
			get
			{
				return this.Proto.Label == FieldDescriptorProto.Types.Label.Required;
			}
		}

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x06000785 RID: 1925 RVA: 0x0001B18E File Offset: 0x0001938E
		public bool IsMap
		{
			get
			{
				return this.fieldType == FieldType.Message && this.messageType.Proto.Options != null && this.messageType.Proto.Options.MapEntry;
			}
		}

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x06000786 RID: 1926 RVA: 0x0001B1C4 File Offset: 0x000193C4
		public bool IsPacked
		{
			get
			{
				if (base.File.Syntax != Syntax.Proto3)
				{
					FieldOptions options = this.Proto.Options;
					return options != null && options.Packed;
				}
				return !this.Proto.Options.HasPacked || this.Proto.Options.Packed;
			}
		}

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x06000787 RID: 1927 RVA: 0x0001B21A File Offset: 0x0001941A
		public bool IsExtension
		{
			get
			{
				return this.Proto.HasExtendee;
			}
		}

		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x06000788 RID: 1928 RVA: 0x0001B227 File Offset: 0x00019427
		public FieldType FieldType
		{
			get
			{
				return this.fieldType;
			}
		}

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x06000789 RID: 1929 RVA: 0x0001B22F File Offset: 0x0001942F
		public int FieldNumber
		{
			get
			{
				return this.Proto.Number;
			}
		}

		// Token: 0x0600078A RID: 1930 RVA: 0x0001B23C File Offset: 0x0001943C
		public int CompareTo(FieldDescriptor other)
		{
			if (other.ContainingType != this.ContainingType)
			{
				throw new ArgumentException("FieldDescriptors can only be compared to other FieldDescriptors for fields of the same message type.");
			}
			return this.FieldNumber - other.FieldNumber;
		}

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x0600078B RID: 1931 RVA: 0x0001B264 File Offset: 0x00019464
		public EnumDescriptor EnumType
		{
			get
			{
				if (this.fieldType != FieldType.Enum)
				{
					throw new InvalidOperationException("EnumType is only valid for enum fields.");
				}
				return this.enumType;
			}
		}

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x0600078C RID: 1932 RVA: 0x0001B281 File Offset: 0x00019481
		public MessageDescriptor MessageType
		{
			get
			{
				if (this.fieldType != FieldType.Message && this.fieldType != FieldType.Group)
				{
					throw new InvalidOperationException("MessageType is only valid for message or group fields.");
				}
				return this.messageType;
			}
		}

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x0600078D RID: 1933 RVA: 0x0001B2A8 File Offset: 0x000194A8
		public MessageDescriptor ExtendeeType
		{
			get
			{
				if (!this.Proto.HasExtendee)
				{
					throw new InvalidOperationException("ExtendeeType is only valid for extension fields.");
				}
				return this.extendeeType;
			}
		}

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x0600078E RID: 1934 RVA: 0x0001B2C8 File Offset: 0x000194C8
		[Obsolete("CustomOptions are obsolete. Use the GetOptions() method.")]
		public CustomOptions CustomOptions
		{
			get
			{
				FieldOptions options = this.Proto.Options;
				IDictionary<int, IExtensionValue> dictionary;
				if (options == null)
				{
					dictionary = null;
				}
				else
				{
					ExtensionSet<FieldOptions> extensions = options._extensions;
					dictionary = ((extensions != null) ? extensions.ValuesByNumber : null);
				}
				return new CustomOptions(dictionary);
			}
		}

		// Token: 0x0600078F RID: 1935 RVA: 0x0001B2F2 File Offset: 0x000194F2
		public FieldOptions GetOptions()
		{
			FieldOptions options = this.Proto.Options;
			if (options == null)
			{
				return null;
			}
			return options.Clone();
		}

		// Token: 0x06000790 RID: 1936 RVA: 0x0001B30C File Offset: 0x0001950C
		[Obsolete("GetOption is obsolete. Use the GetOptions() method.")]
		public T GetOption<T>(Extension<FieldOptions, T> extension)
		{
			T extension2 = this.Proto.Options.GetExtension<T>(extension);
			if (!(extension2 is IDeepCloneable<T>))
			{
				return extension2;
			}
			return (extension2 as IDeepCloneable<T>).Clone();
		}

		// Token: 0x06000791 RID: 1937 RVA: 0x0001B34A File Offset: 0x0001954A
		[Obsolete("GetOption is obsolete. Use the GetOptions() method.")]
		public RepeatedField<T> GetOption<T>(RepeatedExtension<FieldOptions, T> extension)
		{
			return this.Proto.Options.GetExtension<T>(extension).Clone();
		}

		// Token: 0x06000792 RID: 1938 RVA: 0x0001B364 File Offset: 0x00019564
		internal void CrossLink()
		{
			if (this.Proto.HasTypeName)
			{
				IDescriptor descriptor = base.File.DescriptorPool.LookupSymbol(this.Proto.TypeName, this);
				if (this.Proto.HasType)
				{
					if (descriptor is MessageDescriptor)
					{
						this.fieldType = FieldType.Message;
					}
					else
					{
						if (!(descriptor is EnumDescriptor))
						{
							throw new DescriptorValidationException(this, "\"" + this.Proto.TypeName + "\" is not a type.");
						}
						this.fieldType = FieldType.Enum;
					}
				}
				if (this.fieldType == FieldType.Message || this.fieldType == FieldType.Group)
				{
					if (!(descriptor is MessageDescriptor))
					{
						throw new DescriptorValidationException(this, "\"" + this.Proto.TypeName + "\" is not a message type.");
					}
					this.messageType = (MessageDescriptor)descriptor;
					if (this.Proto.HasDefaultValue)
					{
						throw new DescriptorValidationException(this, "Messages can't have default values.");
					}
				}
				else
				{
					if (this.fieldType != FieldType.Enum)
					{
						throw new DescriptorValidationException(this, "Field with primitive type has type_name.");
					}
					if (!(descriptor is EnumDescriptor))
					{
						throw new DescriptorValidationException(this, "\"" + this.Proto.TypeName + "\" is not an enum type.");
					}
					this.enumType = (EnumDescriptor)descriptor;
				}
			}
			else if (this.fieldType == FieldType.Message || this.fieldType == FieldType.Enum)
			{
				throw new DescriptorValidationException(this, "Field with message or enum type missing type_name.");
			}
			if (this.Proto.HasExtendee)
			{
				this.extendeeType = base.File.DescriptorPool.LookupSymbol(this.Proto.Extendee, this) as MessageDescriptor;
			}
			base.File.DescriptorPool.AddFieldByNumber(this);
			if (this.ContainingType != null && this.ContainingType.Proto.Options != null && this.ContainingType.Proto.Options.MessageSetWireFormat)
			{
				throw new DescriptorValidationException(this, "MessageSet format is not supported.");
			}
			this.accessor = this.CreateAccessor();
		}

		// Token: 0x06000793 RID: 1939 RVA: 0x0001B550 File Offset: 0x00019750
		private IFieldAccessor CreateAccessor()
		{
			if (this.Extension != null)
			{
				return new ExtensionAccessor(this);
			}
			if (this.propertyName == null)
			{
				return null;
			}
			PropertyInfo property = this.ContainingType.ClrType.GetProperty(this.propertyName);
			if (property == null)
			{
				throw new DescriptorValidationException(this, string.Format("Property {0} not found in {1}", this.propertyName, this.ContainingType.ClrType));
			}
			if (this.IsMap)
			{
				return new MapFieldAccessor(property, this);
			}
			if (!this.IsRepeated)
			{
				return new SingleFieldAccessor(property, this);
			}
			return new RepeatedFieldAccessor(property, this);
		}

		// Token: 0x040002E9 RID: 745
		private EnumDescriptor enumType;

		// Token: 0x040002EA RID: 746
		private MessageDescriptor extendeeType;

		// Token: 0x040002EB RID: 747
		private MessageDescriptor messageType;

		// Token: 0x040002EC RID: 748
		private FieldType fieldType;

		// Token: 0x040002ED RID: 749
		private readonly string propertyName;

		// Token: 0x040002EE RID: 750
		private IFieldAccessor accessor;
	}
}
