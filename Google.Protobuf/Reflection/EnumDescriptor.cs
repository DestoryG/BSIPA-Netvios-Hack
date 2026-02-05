using System;
using System.Collections.Generic;
using Google.Protobuf.Collections;

namespace Google.Protobuf.Reflection
{
	// Token: 0x0200006A RID: 106
	public sealed class EnumDescriptor : DescriptorBase
	{
		// Token: 0x06000750 RID: 1872 RVA: 0x0001A9D0 File Offset: 0x00018BD0
		internal EnumDescriptor(EnumDescriptorProto proto, FileDescriptor file, MessageDescriptor parent, int index, Type clrType)
			: base(file, file.ComputeFullName(parent, proto.Name), index)
		{
			EnumDescriptor <>4__this = this;
			this.proto = proto;
			this.clrType = clrType;
			this.containingType = parent;
			if (proto.Value.Count == 0)
			{
				throw new DescriptorValidationException(this, "Enums must contain at least one value.");
			}
			this.values = DescriptorUtil.ConvertAndMakeReadOnly<EnumValueDescriptorProto, EnumValueDescriptor>(proto.Value, (EnumValueDescriptorProto value, int i) => new EnumValueDescriptor(value, file, <>4__this, i));
			base.File.DescriptorPool.AddSymbol(this);
		}

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x06000751 RID: 1873 RVA: 0x0001AA6E File Offset: 0x00018C6E
		internal EnumDescriptorProto Proto
		{
			get
			{
				return this.proto;
			}
		}

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x06000752 RID: 1874 RVA: 0x0001AA76 File Offset: 0x00018C76
		public override string Name
		{
			get
			{
				return this.proto.Name;
			}
		}

		// Token: 0x06000753 RID: 1875 RVA: 0x0001AA83 File Offset: 0x00018C83
		internal override IReadOnlyList<DescriptorBase> GetNestedDescriptorListForField(int fieldNumber)
		{
			if (fieldNumber == 2)
			{
				return (IReadOnlyList<DescriptorBase>)this.Values;
			}
			return null;
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x06000754 RID: 1876 RVA: 0x0001AA96 File Offset: 0x00018C96
		public Type ClrType
		{
			get
			{
				return this.clrType;
			}
		}

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x06000755 RID: 1877 RVA: 0x0001AA9E File Offset: 0x00018C9E
		public MessageDescriptor ContainingType
		{
			get
			{
				return this.containingType;
			}
		}

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x06000756 RID: 1878 RVA: 0x0001AAA6 File Offset: 0x00018CA6
		public IList<EnumValueDescriptor> Values
		{
			get
			{
				return this.values;
			}
		}

		// Token: 0x06000757 RID: 1879 RVA: 0x0001AAAE File Offset: 0x00018CAE
		public EnumValueDescriptor FindValueByNumber(int number)
		{
			return base.File.DescriptorPool.FindEnumValueByNumber(this, number);
		}

		// Token: 0x06000758 RID: 1880 RVA: 0x0001AAC2 File Offset: 0x00018CC2
		public EnumValueDescriptor FindValueByName(string name)
		{
			return base.File.DescriptorPool.FindSymbol<EnumValueDescriptor>(base.FullName + "." + name);
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x06000759 RID: 1881 RVA: 0x0001AAE5 File Offset: 0x00018CE5
		[Obsolete("CustomOptions are obsolete. Use the GetOptions() method.")]
		public CustomOptions CustomOptions
		{
			get
			{
				EnumOptions options = this.Proto.Options;
				IDictionary<int, IExtensionValue> dictionary;
				if (options == null)
				{
					dictionary = null;
				}
				else
				{
					ExtensionSet<EnumOptions> extensions = options._extensions;
					dictionary = ((extensions != null) ? extensions.ValuesByNumber : null);
				}
				return new CustomOptions(dictionary);
			}
		}

		// Token: 0x0600075A RID: 1882 RVA: 0x0001AB0F File Offset: 0x00018D0F
		public EnumOptions GetOptions()
		{
			EnumOptions options = this.Proto.Options;
			if (options == null)
			{
				return null;
			}
			return options.Clone();
		}

		// Token: 0x0600075B RID: 1883 RVA: 0x0001AB28 File Offset: 0x00018D28
		[Obsolete("GetOption is obsolete. Use the GetOptions() method.")]
		public T GetOption<T>(Extension<EnumOptions, T> extension)
		{
			T extension2 = this.Proto.Options.GetExtension<T>(extension);
			if (!(extension2 is IDeepCloneable<T>))
			{
				return extension2;
			}
			return (extension2 as IDeepCloneable<T>).Clone();
		}

		// Token: 0x0600075C RID: 1884 RVA: 0x0001AB66 File Offset: 0x00018D66
		[Obsolete("GetOption is obsolete. Use the GetOptions() method.")]
		public RepeatedField<T> GetOption<T>(RepeatedExtension<EnumOptions, T> extension)
		{
			return this.Proto.Options.GetExtension<T>(extension).Clone();
		}

		// Token: 0x040002DB RID: 731
		private readonly EnumDescriptorProto proto;

		// Token: 0x040002DC RID: 732
		private readonly MessageDescriptor containingType;

		// Token: 0x040002DD RID: 733
		private readonly IList<EnumValueDescriptor> values;

		// Token: 0x040002DE RID: 734
		private readonly Type clrType;
	}
}
