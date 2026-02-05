using System;
using System.Collections.Generic;
using Google.Protobuf.Collections;

namespace Google.Protobuf.Reflection
{
	// Token: 0x0200006B RID: 107
	public sealed class EnumValueDescriptor : DescriptorBase
	{
		// Token: 0x0600075D RID: 1885 RVA: 0x0001AB80 File Offset: 0x00018D80
		internal EnumValueDescriptor(EnumValueDescriptorProto proto, FileDescriptor file, EnumDescriptor parent, int index)
			: base(file, parent.FullName + "." + proto.Name, index)
		{
			this.proto = proto;
			this.enumDescriptor = parent;
			file.DescriptorPool.AddSymbol(this);
			file.DescriptorPool.AddEnumValueByNumber(this);
		}

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x0600075E RID: 1886 RVA: 0x0001ABD2 File Offset: 0x00018DD2
		internal EnumValueDescriptorProto Proto
		{
			get
			{
				return this.proto;
			}
		}

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x0600075F RID: 1887 RVA: 0x0001ABDA File Offset: 0x00018DDA
		public override string Name
		{
			get
			{
				return this.proto.Name;
			}
		}

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x06000760 RID: 1888 RVA: 0x0001ABE7 File Offset: 0x00018DE7
		public int Number
		{
			get
			{
				return this.Proto.Number;
			}
		}

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x06000761 RID: 1889 RVA: 0x0001ABF4 File Offset: 0x00018DF4
		public EnumDescriptor EnumDescriptor
		{
			get
			{
				return this.enumDescriptor;
			}
		}

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x06000762 RID: 1890 RVA: 0x0001ABFC File Offset: 0x00018DFC
		[Obsolete("CustomOptions are obsolete. Use the GetOptions() method.")]
		public CustomOptions CustomOptions
		{
			get
			{
				EnumValueOptions options = this.Proto.Options;
				IDictionary<int, IExtensionValue> dictionary;
				if (options == null)
				{
					dictionary = null;
				}
				else
				{
					ExtensionSet<EnumValueOptions> extensions = options._extensions;
					dictionary = ((extensions != null) ? extensions.ValuesByNumber : null);
				}
				return new CustomOptions(dictionary);
			}
		}

		// Token: 0x06000763 RID: 1891 RVA: 0x0001AC26 File Offset: 0x00018E26
		public EnumValueOptions GetOptions()
		{
			EnumValueOptions options = this.Proto.Options;
			if (options == null)
			{
				return null;
			}
			return options.Clone();
		}

		// Token: 0x06000764 RID: 1892 RVA: 0x0001AC40 File Offset: 0x00018E40
		[Obsolete("GetOption is obsolete. Use the GetOptions() method.")]
		public T GetOption<T>(Extension<EnumValueOptions, T> extension)
		{
			T extension2 = this.Proto.Options.GetExtension<T>(extension);
			if (!(extension2 is IDeepCloneable<T>))
			{
				return extension2;
			}
			return (extension2 as IDeepCloneable<T>).Clone();
		}

		// Token: 0x06000765 RID: 1893 RVA: 0x0001AC7E File Offset: 0x00018E7E
		[Obsolete("GetOption is obsolete. Use the GetOptions() method.")]
		public RepeatedField<T> GetOption<T>(RepeatedExtension<EnumValueOptions, T> extension)
		{
			return this.Proto.Options.GetExtension<T>(extension).Clone();
		}

		// Token: 0x040002DF RID: 735
		private readonly EnumDescriptor enumDescriptor;

		// Token: 0x040002E0 RID: 736
		private readonly EnumValueDescriptorProto proto;
	}
}
