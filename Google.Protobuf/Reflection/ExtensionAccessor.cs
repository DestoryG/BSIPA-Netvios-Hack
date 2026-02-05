using System;

namespace Google.Protobuf.Reflection
{
	// Token: 0x0200006C RID: 108
	internal sealed class ExtensionAccessor : IFieldAccessor
	{
		// Token: 0x06000766 RID: 1894 RVA: 0x0001AC96 File Offset: 0x00018E96
		internal ExtensionAccessor(FieldDescriptor descriptor)
		{
			this.Descriptor = descriptor;
			this.extension = descriptor.Extension;
			this.helper = ReflectionUtil.CreateExtensionHelper(this.extension);
		}

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x06000767 RID: 1895 RVA: 0x0001ACC2 File Offset: 0x00018EC2
		public FieldDescriptor Descriptor { get; }

		// Token: 0x06000768 RID: 1896 RVA: 0x0001ACCA File Offset: 0x00018ECA
		public void Clear(IMessage message)
		{
			this.helper.ClearExtension(message);
		}

		// Token: 0x06000769 RID: 1897 RVA: 0x0001ACD8 File Offset: 0x00018ED8
		public bool HasValue(IMessage message)
		{
			return this.helper.HasExtension(message);
		}

		// Token: 0x0600076A RID: 1898 RVA: 0x0001ACE6 File Offset: 0x00018EE6
		public object GetValue(IMessage message)
		{
			return this.helper.GetExtension(message);
		}

		// Token: 0x0600076B RID: 1899 RVA: 0x0001ACF4 File Offset: 0x00018EF4
		public void SetValue(IMessage message, object value)
		{
			this.helper.SetExtension(message, value);
		}

		// Token: 0x040002E1 RID: 737
		private readonly Extension extension;

		// Token: 0x040002E2 RID: 738
		private readonly ReflectionUtil.IExtensionReflectionHelper helper;
	}
}
