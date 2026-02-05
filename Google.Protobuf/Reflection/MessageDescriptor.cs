using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Google.Protobuf.Collections;

namespace Google.Protobuf.Reflection
{
	// Token: 0x02000077 RID: 119
	public sealed class MessageDescriptor : DescriptorBase
	{
		// Token: 0x060007D4 RID: 2004 RVA: 0x0001C0E8 File Offset: 0x0001A2E8
		internal MessageDescriptor(DescriptorProto proto, FileDescriptor file, MessageDescriptor parent, int typeIndex, GeneratedClrTypeInfo generatedCodeInfo)
			: base(file, file.ComputeFullName(parent, proto.Name), typeIndex)
		{
			MessageDescriptor <>4__this = this;
			this.Proto = proto;
			GeneratedClrTypeInfo generatedCodeInfo2 = generatedCodeInfo;
			this.Parser = ((generatedCodeInfo2 != null) ? generatedCodeInfo2.Parser : null);
			GeneratedClrTypeInfo generatedCodeInfo3 = generatedCodeInfo;
			this.ClrType = ((generatedCodeInfo3 != null) ? generatedCodeInfo3.ClrType : null);
			this.ContainingType = parent;
			this.Oneofs = DescriptorUtil.ConvertAndMakeReadOnly<OneofDescriptorProto, OneofDescriptor>(proto.OneofDecl, delegate(OneofDescriptorProto oneof, int index)
			{
				FileDescriptor file2 = file;
				MessageDescriptor <>4__this5 = <>4__this;
				GeneratedClrTypeInfo generatedCodeInfo5 = generatedCodeInfo;
				return new OneofDescriptor(oneof, file2, <>4__this5, index, (generatedCodeInfo5 != null) ? generatedCodeInfo5.OneofNames[index] : null);
			});
			int num = 0;
			using (IEnumerator<OneofDescriptor> enumerator = this.Oneofs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.IsSynthetic)
					{
						num++;
					}
					else if (num != 0)
					{
						throw new ArgumentException("All synthetic oneofs should come after real oneofs");
					}
				}
			}
			this.RealOneofCount = this.Oneofs.Count - num;
			this.NestedTypes = DescriptorUtil.ConvertAndMakeReadOnly<DescriptorProto, MessageDescriptor>(proto.NestedType, delegate(DescriptorProto type, int index)
			{
				FileDescriptor file3 = file;
				MessageDescriptor <>4__this2 = <>4__this;
				GeneratedClrTypeInfo generatedCodeInfo6 = generatedCodeInfo;
				return new MessageDescriptor(type, file3, <>4__this2, index, (generatedCodeInfo6 != null) ? generatedCodeInfo6.NestedTypes[index] : null);
			});
			this.EnumTypes = DescriptorUtil.ConvertAndMakeReadOnly<EnumDescriptorProto, EnumDescriptor>(proto.EnumType, delegate(EnumDescriptorProto type, int index)
			{
				FileDescriptor file4 = file;
				MessageDescriptor <>4__this3 = <>4__this;
				GeneratedClrTypeInfo generatedCodeInfo7 = generatedCodeInfo;
				return new EnumDescriptor(type, file4, <>4__this3, index, (generatedCodeInfo7 != null) ? generatedCodeInfo7.NestedEnums[index] : null);
			});
			GeneratedClrTypeInfo generatedCodeInfo4 = generatedCodeInfo;
			this.Extensions = new ExtensionCollection(this, (generatedCodeInfo4 != null) ? generatedCodeInfo4.Extensions : null);
			this.fieldsInDeclarationOrder = DescriptorUtil.ConvertAndMakeReadOnly<FieldDescriptorProto, FieldDescriptor>(proto.Field, delegate(FieldDescriptorProto field, int index)
			{
				FileDescriptor file5 = file;
				MessageDescriptor <>4__this4 = <>4__this;
				GeneratedClrTypeInfo generatedCodeInfo8 = generatedCodeInfo;
				return new FieldDescriptor(field, file5, <>4__this4, index, (generatedCodeInfo8 != null) ? generatedCodeInfo8.PropertyNames[index] : null, null);
			});
			this.fieldsInNumberOrder = new ReadOnlyCollection<FieldDescriptor>(this.fieldsInDeclarationOrder.OrderBy((FieldDescriptor field) => field.FieldNumber).ToArray<FieldDescriptor>());
			this.jsonFieldMap = MessageDescriptor.CreateJsonFieldMap(this.fieldsInNumberOrder);
			file.DescriptorPool.AddSymbol(this);
			this.Fields = new MessageDescriptor.FieldCollection(this);
		}

		// Token: 0x060007D5 RID: 2005 RVA: 0x0001C2D4 File Offset: 0x0001A4D4
		private static ReadOnlyDictionary<string, FieldDescriptor> CreateJsonFieldMap(IList<FieldDescriptor> fields)
		{
			Dictionary<string, FieldDescriptor> dictionary = new Dictionary<string, FieldDescriptor>();
			foreach (FieldDescriptor fieldDescriptor in fields)
			{
				dictionary[fieldDescriptor.Name] = fieldDescriptor;
				dictionary[fieldDescriptor.JsonName] = fieldDescriptor;
			}
			return new ReadOnlyDictionary<string, FieldDescriptor>(dictionary);
		}

		// Token: 0x1700021A RID: 538
		// (get) Token: 0x060007D6 RID: 2006 RVA: 0x0001C33C File Offset: 0x0001A53C
		public override string Name
		{
			get
			{
				return this.Proto.Name;
			}
		}

		// Token: 0x060007D7 RID: 2007 RVA: 0x0001C349 File Offset: 0x0001A549
		internal override IReadOnlyList<DescriptorBase> GetNestedDescriptorListForField(int fieldNumber)
		{
			switch (fieldNumber)
			{
			case 2:
				return (IReadOnlyList<DescriptorBase>)this.fieldsInDeclarationOrder;
			case 3:
				return (IReadOnlyList<DescriptorBase>)this.NestedTypes;
			case 4:
				return (IReadOnlyList<DescriptorBase>)this.EnumTypes;
			default:
				return null;
			}
		}

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x060007D8 RID: 2008 RVA: 0x0001C386 File Offset: 0x0001A586
		internal DescriptorProto Proto { get; }

		// Token: 0x060007D9 RID: 2009 RVA: 0x0001C38E File Offset: 0x0001A58E
		internal bool IsExtensionsInitialized(IMessage message)
		{
			if (this.Proto.ExtensionRange.Count == 0)
			{
				return true;
			}
			if (this.extensionSetIsInitialized == null)
			{
				this.extensionSetIsInitialized = ReflectionUtil.CreateIsInitializedCaller(this.ClrType);
			}
			return this.extensionSetIsInitialized(message);
		}

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x060007DA RID: 2010 RVA: 0x0001C3C9 File Offset: 0x0001A5C9
		public Type ClrType { get; }

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x060007DB RID: 2011 RVA: 0x0001C3D1 File Offset: 0x0001A5D1
		public MessageParser Parser { get; }

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x060007DC RID: 2012 RVA: 0x0001C3D9 File Offset: 0x0001A5D9
		internal bool IsWellKnownType
		{
			get
			{
				return base.File.Package == "google.protobuf" && MessageDescriptor.WellKnownTypeNames.Contains(base.File.Name);
			}
		}

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x060007DD RID: 2013 RVA: 0x0001C409 File Offset: 0x0001A609
		internal bool IsWrapperType
		{
			get
			{
				return base.File.Package == "google.protobuf" && base.File.Name == "google/protobuf/wrappers.proto";
			}
		}

		// Token: 0x17000220 RID: 544
		// (get) Token: 0x060007DE RID: 2014 RVA: 0x0001C439 File Offset: 0x0001A639
		public MessageDescriptor ContainingType { get; }

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x060007DF RID: 2015 RVA: 0x0001C441 File Offset: 0x0001A641
		public MessageDescriptor.FieldCollection Fields { get; }

		// Token: 0x17000222 RID: 546
		// (get) Token: 0x060007E0 RID: 2016 RVA: 0x0001C449 File Offset: 0x0001A649
		public ExtensionCollection Extensions { get; }

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x060007E1 RID: 2017 RVA: 0x0001C451 File Offset: 0x0001A651
		public IList<MessageDescriptor> NestedTypes { get; }

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x060007E2 RID: 2018 RVA: 0x0001C459 File Offset: 0x0001A659
		public IList<EnumDescriptor> EnumTypes { get; }

		// Token: 0x17000225 RID: 549
		// (get) Token: 0x060007E3 RID: 2019 RVA: 0x0001C461 File Offset: 0x0001A661
		public IList<OneofDescriptor> Oneofs { get; }

		// Token: 0x17000226 RID: 550
		// (get) Token: 0x060007E4 RID: 2020 RVA: 0x0001C469 File Offset: 0x0001A669
		public int RealOneofCount { get; }

		// Token: 0x060007E5 RID: 2021 RVA: 0x0001C471 File Offset: 0x0001A671
		public FieldDescriptor FindFieldByName(string name)
		{
			return base.File.DescriptorPool.FindSymbol<FieldDescriptor>(base.FullName + "." + name);
		}

		// Token: 0x060007E6 RID: 2022 RVA: 0x0001C494 File Offset: 0x0001A694
		public FieldDescriptor FindFieldByNumber(int number)
		{
			return base.File.DescriptorPool.FindFieldByNumber(this, number);
		}

		// Token: 0x060007E7 RID: 2023 RVA: 0x0001C4A8 File Offset: 0x0001A6A8
		public T FindDescriptor<T>(string name) where T : class, IDescriptor
		{
			return base.File.DescriptorPool.FindSymbol<T>(base.FullName + "." + name);
		}

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x060007E8 RID: 2024 RVA: 0x0001C4CB File Offset: 0x0001A6CB
		[Obsolete("CustomOptions are obsolete. Use the GetOptions() method.")]
		public CustomOptions CustomOptions
		{
			get
			{
				MessageOptions options = this.Proto.Options;
				IDictionary<int, IExtensionValue> dictionary;
				if (options == null)
				{
					dictionary = null;
				}
				else
				{
					ExtensionSet<MessageOptions> extensions = options._extensions;
					dictionary = ((extensions != null) ? extensions.ValuesByNumber : null);
				}
				return new CustomOptions(dictionary);
			}
		}

		// Token: 0x060007E9 RID: 2025 RVA: 0x0001C4F5 File Offset: 0x0001A6F5
		public MessageOptions GetOptions()
		{
			MessageOptions options = this.Proto.Options;
			if (options == null)
			{
				return null;
			}
			return options.Clone();
		}

		// Token: 0x060007EA RID: 2026 RVA: 0x0001C510 File Offset: 0x0001A710
		[Obsolete("GetOption is obsolete. Use the GetOptions() method.")]
		public T GetOption<T>(Extension<MessageOptions, T> extension)
		{
			T extension2 = this.Proto.Options.GetExtension<T>(extension);
			if (!(extension2 is IDeepCloneable<T>))
			{
				return extension2;
			}
			return (extension2 as IDeepCloneable<T>).Clone();
		}

		// Token: 0x060007EB RID: 2027 RVA: 0x0001C54E File Offset: 0x0001A74E
		[Obsolete("GetOption is obsolete. Use the GetOptions() method.")]
		public RepeatedField<T> GetOption<T>(RepeatedExtension<MessageOptions, T> extension)
		{
			return this.Proto.Options.GetExtension<T>(extension).Clone();
		}

		// Token: 0x060007EC RID: 2028 RVA: 0x0001C568 File Offset: 0x0001A768
		internal void CrossLink()
		{
			foreach (MessageDescriptor messageDescriptor in this.NestedTypes)
			{
				messageDescriptor.CrossLink();
			}
			foreach (FieldDescriptor fieldDescriptor in this.fieldsInDeclarationOrder)
			{
				fieldDescriptor.CrossLink();
			}
			foreach (OneofDescriptor oneofDescriptor in this.Oneofs)
			{
				oneofDescriptor.CrossLink();
			}
			this.Extensions.CrossLink();
		}

		// Token: 0x04000320 RID: 800
		private static readonly HashSet<string> WellKnownTypeNames = new HashSet<string> { "google/protobuf/any.proto", "google/protobuf/api.proto", "google/protobuf/duration.proto", "google/protobuf/empty.proto", "google/protobuf/wrappers.proto", "google/protobuf/timestamp.proto", "google/protobuf/field_mask.proto", "google/protobuf/source_context.proto", "google/protobuf/struct.proto", "google/protobuf/type.proto" };

		// Token: 0x04000321 RID: 801
		private readonly IList<FieldDescriptor> fieldsInDeclarationOrder;

		// Token: 0x04000322 RID: 802
		private readonly IList<FieldDescriptor> fieldsInNumberOrder;

		// Token: 0x04000323 RID: 803
		private readonly IDictionary<string, FieldDescriptor> jsonFieldMap;

		// Token: 0x04000324 RID: 804
		private Func<IMessage, bool> extensionSetIsInitialized;

		// Token: 0x020000EE RID: 238
		public sealed class FieldCollection
		{
			// Token: 0x06000A0B RID: 2571 RVA: 0x000207A3 File Offset: 0x0001E9A3
			internal FieldCollection(MessageDescriptor messageDescriptor)
			{
				this.messageDescriptor = messageDescriptor;
			}

			// Token: 0x06000A0C RID: 2572 RVA: 0x000207B2 File Offset: 0x0001E9B2
			public IList<FieldDescriptor> InDeclarationOrder()
			{
				return this.messageDescriptor.fieldsInDeclarationOrder;
			}

			// Token: 0x06000A0D RID: 2573 RVA: 0x000207BF File Offset: 0x0001E9BF
			public IList<FieldDescriptor> InFieldNumberOrder()
			{
				return this.messageDescriptor.fieldsInNumberOrder;
			}

			// Token: 0x06000A0E RID: 2574 RVA: 0x000207CC File Offset: 0x0001E9CC
			internal IDictionary<string, FieldDescriptor> ByJsonName()
			{
				return this.messageDescriptor.jsonFieldMap;
			}

			// Token: 0x1700026C RID: 620
			public FieldDescriptor this[int number]
			{
				get
				{
					FieldDescriptor fieldDescriptor = this.messageDescriptor.FindFieldByNumber(number);
					if (fieldDescriptor == null)
					{
						throw new KeyNotFoundException("No such field number");
					}
					return fieldDescriptor;
				}
			}

			// Token: 0x1700026D RID: 621
			public FieldDescriptor this[string name]
			{
				get
				{
					FieldDescriptor fieldDescriptor = this.messageDescriptor.FindFieldByName(name);
					if (fieldDescriptor == null)
					{
						throw new KeyNotFoundException("No such field name");
					}
					return fieldDescriptor;
				}
			}

			// Token: 0x04000415 RID: 1045
			private readonly MessageDescriptor messageDescriptor;
		}
	}
}
