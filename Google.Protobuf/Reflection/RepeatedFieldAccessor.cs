using System;
using System.Collections;
using System.Reflection;

namespace Google.Protobuf.Reflection
{
	// Token: 0x0200007E RID: 126
	internal sealed class RepeatedFieldAccessor : FieldAccessorBase
	{
		// Token: 0x06000823 RID: 2083 RVA: 0x0001CE5F File Offset: 0x0001B05F
		internal RepeatedFieldAccessor(PropertyInfo property, FieldDescriptor descriptor)
			: base(property, descriptor)
		{
		}

		// Token: 0x06000824 RID: 2084 RVA: 0x0001CE69 File Offset: 0x0001B069
		public override void Clear(IMessage message)
		{
			((IList)base.GetValue(message)).Clear();
		}

		// Token: 0x06000825 RID: 2085 RVA: 0x0001CE7C File Offset: 0x0001B07C
		public override bool HasValue(IMessage message)
		{
			throw new InvalidOperationException("HasValue is not implemented for repeated fields");
		}

		// Token: 0x06000826 RID: 2086 RVA: 0x0001CE88 File Offset: 0x0001B088
		public override void SetValue(IMessage message, object value)
		{
			throw new InvalidOperationException("SetValue is not implemented for repeated fields");
		}
	}
}
