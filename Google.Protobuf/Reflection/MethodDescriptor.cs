using System;
using System.Collections.Generic;
using Google.Protobuf.Collections;

namespace Google.Protobuf.Reflection
{
	// Token: 0x02000078 RID: 120
	public sealed class MethodDescriptor : DescriptorBase
	{
		// Token: 0x17000228 RID: 552
		// (get) Token: 0x060007EE RID: 2030 RVA: 0x0001C6BF File Offset: 0x0001A8BF
		public ServiceDescriptor Service
		{
			get
			{
				return this.service;
			}
		}

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x060007EF RID: 2031 RVA: 0x0001C6C7 File Offset: 0x0001A8C7
		public MessageDescriptor InputType
		{
			get
			{
				return this.inputType;
			}
		}

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x060007F0 RID: 2032 RVA: 0x0001C6CF File Offset: 0x0001A8CF
		public MessageDescriptor OutputType
		{
			get
			{
				return this.outputType;
			}
		}

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x060007F1 RID: 2033 RVA: 0x0001C6D7 File Offset: 0x0001A8D7
		public bool IsClientStreaming
		{
			get
			{
				return this.proto.ClientStreaming;
			}
		}

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x060007F2 RID: 2034 RVA: 0x0001C6E4 File Offset: 0x0001A8E4
		public bool IsServerStreaming
		{
			get
			{
				return this.proto.ServerStreaming;
			}
		}

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x060007F3 RID: 2035 RVA: 0x0001C6F1 File Offset: 0x0001A8F1
		[Obsolete("CustomOptions are obsolete. Use the GetOptions() method.")]
		public CustomOptions CustomOptions
		{
			get
			{
				MethodOptions options = this.Proto.Options;
				IDictionary<int, IExtensionValue> dictionary;
				if (options == null)
				{
					dictionary = null;
				}
				else
				{
					ExtensionSet<MethodOptions> extensions = options._extensions;
					dictionary = ((extensions != null) ? extensions.ValuesByNumber : null);
				}
				return new CustomOptions(dictionary);
			}
		}

		// Token: 0x060007F4 RID: 2036 RVA: 0x0001C71B File Offset: 0x0001A91B
		public MethodOptions GetOptions()
		{
			MethodOptions options = this.Proto.Options;
			if (options == null)
			{
				return null;
			}
			return options.Clone();
		}

		// Token: 0x060007F5 RID: 2037 RVA: 0x0001C734 File Offset: 0x0001A934
		[Obsolete("GetOption is obsolete. Use the GetOptions() method.")]
		public T GetOption<T>(Extension<MethodOptions, T> extension)
		{
			T extension2 = this.Proto.Options.GetExtension<T>(extension);
			if (!(extension2 is IDeepCloneable<T>))
			{
				return extension2;
			}
			return (extension2 as IDeepCloneable<T>).Clone();
		}

		// Token: 0x060007F6 RID: 2038 RVA: 0x0001C772 File Offset: 0x0001A972
		[Obsolete("GetOption is obsolete. Use the GetOptions() method.")]
		public RepeatedField<T> GetOption<T>(RepeatedExtension<MethodOptions, T> extension)
		{
			return this.Proto.Options.GetExtension<T>(extension).Clone();
		}

		// Token: 0x060007F7 RID: 2039 RVA: 0x0001C78A File Offset: 0x0001A98A
		internal MethodDescriptor(MethodDescriptorProto proto, FileDescriptor file, ServiceDescriptor parent, int index)
			: base(file, parent.FullName + "." + proto.Name, index)
		{
			this.proto = proto;
			this.service = parent;
			file.DescriptorPool.AddSymbol(this);
		}

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x060007F8 RID: 2040 RVA: 0x0001C7C5 File Offset: 0x0001A9C5
		internal MethodDescriptorProto Proto
		{
			get
			{
				return this.proto;
			}
		}

		// Token: 0x1700022F RID: 559
		// (get) Token: 0x060007F9 RID: 2041 RVA: 0x0001C7CD File Offset: 0x0001A9CD
		public override string Name
		{
			get
			{
				return this.proto.Name;
			}
		}

		// Token: 0x060007FA RID: 2042 RVA: 0x0001C7DC File Offset: 0x0001A9DC
		internal void CrossLink()
		{
			IDescriptor descriptor = base.File.DescriptorPool.LookupSymbol(this.Proto.InputType, this);
			if (!(descriptor is MessageDescriptor))
			{
				throw new DescriptorValidationException(this, "\"" + this.Proto.InputType + "\" is not a message type.");
			}
			this.inputType = (MessageDescriptor)descriptor;
			descriptor = base.File.DescriptorPool.LookupSymbol(this.Proto.OutputType, this);
			if (!(descriptor is MessageDescriptor))
			{
				throw new DescriptorValidationException(this, "\"" + this.Proto.OutputType + "\" is not a message type.");
			}
			this.outputType = (MessageDescriptor)descriptor;
		}

		// Token: 0x0400032F RID: 815
		private readonly MethodDescriptorProto proto;

		// Token: 0x04000330 RID: 816
		private readonly ServiceDescriptor service;

		// Token: 0x04000331 RID: 817
		private MessageDescriptor inputType;

		// Token: 0x04000332 RID: 818
		private MessageDescriptor outputType;
	}
}
