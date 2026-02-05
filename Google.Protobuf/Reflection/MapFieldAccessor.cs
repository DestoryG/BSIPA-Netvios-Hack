using System;
using System.Collections;
using System.Reflection;

namespace Google.Protobuf.Reflection
{
	// Token: 0x02000076 RID: 118
	internal sealed class MapFieldAccessor : FieldAccessorBase
	{
		// Token: 0x060007D0 RID: 2000 RVA: 0x0001C0B3 File Offset: 0x0001A2B3
		internal MapFieldAccessor(PropertyInfo property, FieldDescriptor descriptor)
			: base(property, descriptor)
		{
		}

		// Token: 0x060007D1 RID: 2001 RVA: 0x0001C0BD File Offset: 0x0001A2BD
		public override void Clear(IMessage message)
		{
			((IDictionary)base.GetValue(message)).Clear();
		}

		// Token: 0x060007D2 RID: 2002 RVA: 0x0001C0D0 File Offset: 0x0001A2D0
		public override bool HasValue(IMessage message)
		{
			throw new InvalidOperationException("HasValue is not implemented for map fields");
		}

		// Token: 0x060007D3 RID: 2003 RVA: 0x0001C0DC File Offset: 0x0001A2DC
		public override void SetValue(IMessage message, object value)
		{
			throw new InvalidOperationException("SetValue is not implemented for map fields");
		}
	}
}
