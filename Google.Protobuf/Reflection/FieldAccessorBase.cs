using System;
using System.Reflection;

namespace Google.Protobuf.Reflection
{
	// Token: 0x0200006E RID: 110
	internal abstract class FieldAccessorBase : IFieldAccessor
	{
		// Token: 0x06000772 RID: 1906 RVA: 0x0001AED8 File Offset: 0x000190D8
		internal FieldAccessorBase(PropertyInfo property, FieldDescriptor descriptor)
		{
			this.descriptor = descriptor;
			this.getValueDelegate = ReflectionUtil.CreateFuncIMessageObject(property.GetGetMethod());
		}

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x06000773 RID: 1907 RVA: 0x0001AEF8 File Offset: 0x000190F8
		public FieldDescriptor Descriptor
		{
			get
			{
				return this.descriptor;
			}
		}

		// Token: 0x06000774 RID: 1908 RVA: 0x0001AF00 File Offset: 0x00019100
		public object GetValue(IMessage message)
		{
			return this.getValueDelegate(message);
		}

		// Token: 0x06000775 RID: 1909
		public abstract bool HasValue(IMessage message);

		// Token: 0x06000776 RID: 1910
		public abstract void Clear(IMessage message);

		// Token: 0x06000777 RID: 1911
		public abstract void SetValue(IMessage message, object value);

		// Token: 0x040002E7 RID: 743
		private readonly Func<IMessage, object> getValueDelegate;

		// Token: 0x040002E8 RID: 744
		private readonly FieldDescriptor descriptor;
	}
}
