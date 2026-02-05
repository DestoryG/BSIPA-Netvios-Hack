using System;
using System.Reflection;

namespace Google.Protobuf.Reflection
{
	// Token: 0x02000079 RID: 121
	public sealed class OneofAccessor
	{
		// Token: 0x060007FB RID: 2043 RVA: 0x0001C88D File Offset: 0x0001AA8D
		private OneofAccessor(OneofDescriptor descriptor, Func<IMessage, int> caseDelegate, Action<IMessage> clearDelegate)
		{
			this.Descriptor = descriptor;
			this.caseDelegate = caseDelegate;
			this.clearDelegate = clearDelegate;
		}

		// Token: 0x060007FC RID: 2044 RVA: 0x0001C8AA File Offset: 0x0001AAAA
		internal static OneofAccessor ForRegularOneof(OneofDescriptor descriptor, PropertyInfo caseProperty, MethodInfo clearMethod)
		{
			return new OneofAccessor(descriptor, ReflectionUtil.CreateFuncIMessageInt32(caseProperty.GetGetMethod()), ReflectionUtil.CreateActionIMessage(clearMethod));
		}

		// Token: 0x060007FD RID: 2045 RVA: 0x0001C8C4 File Offset: 0x0001AAC4
		internal static OneofAccessor ForSyntheticOneof(OneofDescriptor descriptor)
		{
			return new OneofAccessor(descriptor, delegate(IMessage message)
			{
				if (!descriptor.Fields[0].Accessor.HasValue(message))
				{
					return 0;
				}
				return descriptor.Fields[0].FieldNumber;
			}, delegate(IMessage message)
			{
				descriptor.Fields[0].Accessor.Clear(message);
			});
		}

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x060007FE RID: 2046 RVA: 0x0001C901 File Offset: 0x0001AB01
		public OneofDescriptor Descriptor { get; }

		// Token: 0x060007FF RID: 2047 RVA: 0x0001C909 File Offset: 0x0001AB09
		public void Clear(IMessage message)
		{
			this.clearDelegate(message);
		}

		// Token: 0x06000800 RID: 2048 RVA: 0x0001C918 File Offset: 0x0001AB18
		public FieldDescriptor GetCaseFieldDescriptor(IMessage message)
		{
			int num = this.caseDelegate(message);
			if (num <= 0)
			{
				return null;
			}
			return this.Descriptor.ContainingType.FindFieldByNumber(num);
		}

		// Token: 0x04000333 RID: 819
		private readonly Func<IMessage, int> caseDelegate;

		// Token: 0x04000334 RID: 820
		private readonly Action<IMessage> clearDelegate;
	}
}
