using System;
using System.Collections.Generic;
using Google.Protobuf.Collections;

namespace Google.Protobuf.Reflection
{
	// Token: 0x0200007F RID: 127
	public sealed class ServiceDescriptor : DescriptorBase
	{
		// Token: 0x06000827 RID: 2087 RVA: 0x0001CE94 File Offset: 0x0001B094
		internal ServiceDescriptor(ServiceDescriptorProto proto, FileDescriptor file, int index)
			: base(file, file.ComputeFullName(null, proto.Name), index)
		{
			ServiceDescriptor <>4__this = this;
			this.proto = proto;
			this.methods = DescriptorUtil.ConvertAndMakeReadOnly<MethodDescriptorProto, MethodDescriptor>(proto.Method, (MethodDescriptorProto method, int i) => new MethodDescriptor(method, file, <>4__this, i));
			file.DescriptorPool.AddSymbol(this);
		}

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x06000828 RID: 2088 RVA: 0x0001CF09 File Offset: 0x0001B109
		public override string Name
		{
			get
			{
				return this.proto.Name;
			}
		}

		// Token: 0x06000829 RID: 2089 RVA: 0x0001CF16 File Offset: 0x0001B116
		internal override IReadOnlyList<DescriptorBase> GetNestedDescriptorListForField(int fieldNumber)
		{
			if (fieldNumber == 2)
			{
				return (IReadOnlyList<DescriptorBase>)this.methods;
			}
			return null;
		}

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x0600082A RID: 2090 RVA: 0x0001CF29 File Offset: 0x0001B129
		internal ServiceDescriptorProto Proto
		{
			get
			{
				return this.proto;
			}
		}

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x0600082B RID: 2091 RVA: 0x0001CF31 File Offset: 0x0001B131
		public IList<MethodDescriptor> Methods
		{
			get
			{
				return this.methods;
			}
		}

		// Token: 0x0600082C RID: 2092 RVA: 0x0001CF39 File Offset: 0x0001B139
		public MethodDescriptor FindMethodByName(string name)
		{
			return base.File.DescriptorPool.FindSymbol<MethodDescriptor>(base.FullName + "." + name);
		}

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x0600082D RID: 2093 RVA: 0x0001CF5C File Offset: 0x0001B15C
		[Obsolete("CustomOptions are obsolete. Use the GetOptions() method.")]
		public CustomOptions CustomOptions
		{
			get
			{
				ServiceOptions options = this.Proto.Options;
				IDictionary<int, IExtensionValue> dictionary;
				if (options == null)
				{
					dictionary = null;
				}
				else
				{
					ExtensionSet<ServiceOptions> extensions = options._extensions;
					dictionary = ((extensions != null) ? extensions.ValuesByNumber : null);
				}
				return new CustomOptions(dictionary);
			}
		}

		// Token: 0x0600082E RID: 2094 RVA: 0x0001CF86 File Offset: 0x0001B186
		public ServiceOptions GetOptions()
		{
			ServiceOptions options = this.Proto.Options;
			if (options == null)
			{
				return null;
			}
			return options.Clone();
		}

		// Token: 0x0600082F RID: 2095 RVA: 0x0001CFA0 File Offset: 0x0001B1A0
		[Obsolete("GetOption is obsolete. Use the GetOptions() method.")]
		public T GetOption<T>(Extension<ServiceOptions, T> extension)
		{
			T extension2 = this.Proto.Options.GetExtension<T>(extension);
			if (!(extension2 is IDeepCloneable<T>))
			{
				return extension2;
			}
			return (extension2 as IDeepCloneable<T>).Clone();
		}

		// Token: 0x06000830 RID: 2096 RVA: 0x0001CFDE File Offset: 0x0001B1DE
		[Obsolete("GetOption is obsolete. Use the GetOptions() method.")]
		public RepeatedField<T> GetOption<T>(RepeatedExtension<ServiceOptions, T> extension)
		{
			return this.Proto.Options.GetExtension<T>(extension).Clone();
		}

		// Token: 0x06000831 RID: 2097 RVA: 0x0001CFF8 File Offset: 0x0001B1F8
		internal void CrossLink()
		{
			foreach (MethodDescriptor methodDescriptor in this.methods)
			{
				methodDescriptor.CrossLink();
			}
		}

		// Token: 0x04000342 RID: 834
		private readonly ServiceDescriptorProto proto;

		// Token: 0x04000343 RID: 835
		private readonly IList<MethodDescriptor> methods;
	}
}
