using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using Google.Protobuf.Collections;

namespace Google.Protobuf.Reflection
{
	// Token: 0x0200007A RID: 122
	public sealed class OneofDescriptor : DescriptorBase
	{
		// Token: 0x06000801 RID: 2049 RVA: 0x0001C94C File Offset: 0x0001AB4C
		internal OneofDescriptor(OneofDescriptorProto proto, FileDescriptor file, MessageDescriptor parent, int index, string clrName)
			: base(file, file.ComputeFullName(parent, proto.Name), index)
		{
			this.proto = proto;
			this.containingType = parent;
			file.DescriptorPool.AddSymbol(this);
			FieldDescriptorProto fieldDescriptorProto = parent.Proto.Field.FirstOrDefault((FieldDescriptorProto fieldProto) => fieldProto.OneofIndex == index);
			this.IsSynthetic = fieldDescriptorProto != null && fieldDescriptorProto.Proto3Optional;
			this.accessor = this.CreateAccessor(clrName);
		}

		// Token: 0x17000231 RID: 561
		// (get) Token: 0x06000802 RID: 2050 RVA: 0x0001C9D8 File Offset: 0x0001ABD8
		public override string Name
		{
			get
			{
				return this.proto.Name;
			}
		}

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x06000803 RID: 2051 RVA: 0x0001C9E5 File Offset: 0x0001ABE5
		public MessageDescriptor ContainingType
		{
			get
			{
				return this.containingType;
			}
		}

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x06000804 RID: 2052 RVA: 0x0001C9ED File Offset: 0x0001ABED
		public IList<FieldDescriptor> Fields
		{
			get
			{
				return this.fields;
			}
		}

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x06000805 RID: 2053 RVA: 0x0001C9F5 File Offset: 0x0001ABF5
		public bool IsSynthetic { get; }

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x06000806 RID: 2054 RVA: 0x0001C9FD File Offset: 0x0001ABFD
		public OneofAccessor Accessor
		{
			get
			{
				return this.accessor;
			}
		}

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x06000807 RID: 2055 RVA: 0x0001CA05 File Offset: 0x0001AC05
		[Obsolete("CustomOptions are obsolete. Use the GetOptions method.")]
		public CustomOptions CustomOptions
		{
			get
			{
				OneofOptions options = this.proto.Options;
				IDictionary<int, IExtensionValue> dictionary;
				if (options == null)
				{
					dictionary = null;
				}
				else
				{
					ExtensionSet<OneofOptions> extensions = options._extensions;
					dictionary = ((extensions != null) ? extensions.ValuesByNumber : null);
				}
				return new CustomOptions(dictionary);
			}
		}

		// Token: 0x06000808 RID: 2056 RVA: 0x0001CA2F File Offset: 0x0001AC2F
		public OneofOptions GetOptions()
		{
			OneofOptions options = this.proto.Options;
			if (options == null)
			{
				return null;
			}
			return options.Clone();
		}

		// Token: 0x06000809 RID: 2057 RVA: 0x0001CA48 File Offset: 0x0001AC48
		[Obsolete("GetOption is obsolete. Use the GetOptions() method.")]
		public T GetOption<T>(Extension<OneofOptions, T> extension)
		{
			T extension2 = this.proto.Options.GetExtension<T>(extension);
			if (!(extension2 is IDeepCloneable<T>))
			{
				return extension2;
			}
			return (extension2 as IDeepCloneable<T>).Clone();
		}

		// Token: 0x0600080A RID: 2058 RVA: 0x0001CA86 File Offset: 0x0001AC86
		[Obsolete("GetOption is obsolete. Use the GetOptions() method.")]
		public RepeatedField<T> GetOption<T>(RepeatedExtension<OneofOptions, T> extension)
		{
			return this.proto.Options.GetExtension<T>(extension).Clone();
		}

		// Token: 0x0600080B RID: 2059 RVA: 0x0001CAA0 File Offset: 0x0001ACA0
		internal void CrossLink()
		{
			List<FieldDescriptor> list = new List<FieldDescriptor>();
			foreach (FieldDescriptor fieldDescriptor in this.ContainingType.Fields.InDeclarationOrder())
			{
				if (fieldDescriptor.ContainingOneof == this)
				{
					list.Add(fieldDescriptor);
				}
			}
			this.fields = new ReadOnlyCollection<FieldDescriptor>(list);
		}

		// Token: 0x0600080C RID: 2060 RVA: 0x0001CB14 File Offset: 0x0001AD14
		private OneofAccessor CreateAccessor(string clrName)
		{
			if (clrName == null)
			{
				return null;
			}
			if (this.IsSynthetic)
			{
				return OneofAccessor.ForSyntheticOneof(this);
			}
			PropertyInfo property = this.containingType.ClrType.GetProperty(clrName + "Case");
			if (property == null)
			{
				throw new DescriptorValidationException(this, string.Format("Property {0}Case not found in {1}", clrName, this.containingType.ClrType));
			}
			if (!property.CanRead)
			{
				throw new ArgumentException(string.Format("Cannot read from property {0}Case in {1}", clrName, this.containingType.ClrType));
			}
			MethodInfo method = this.containingType.ClrType.GetMethod("Clear" + clrName);
			if (method == null)
			{
				throw new DescriptorValidationException(this, string.Format("Method Clear{0} not found in {1}", clrName, this.containingType.ClrType));
			}
			return OneofAccessor.ForRegularOneof(this, property, method);
		}

		// Token: 0x04000336 RID: 822
		private readonly OneofDescriptorProto proto;

		// Token: 0x04000337 RID: 823
		private MessageDescriptor containingType;

		// Token: 0x04000338 RID: 824
		private IList<FieldDescriptor> fields;

		// Token: 0x04000339 RID: 825
		private readonly OneofAccessor accessor;
	}
}
