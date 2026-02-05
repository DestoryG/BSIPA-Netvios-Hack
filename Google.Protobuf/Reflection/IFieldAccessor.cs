using System;

namespace Google.Protobuf.Reflection
{
	// Token: 0x02000075 RID: 117
	public interface IFieldAccessor
	{
		// Token: 0x17000219 RID: 537
		// (get) Token: 0x060007CB RID: 1995
		FieldDescriptor Descriptor { get; }

		// Token: 0x060007CC RID: 1996
		void Clear(IMessage message);

		// Token: 0x060007CD RID: 1997
		object GetValue(IMessage message);

		// Token: 0x060007CE RID: 1998
		bool HasValue(IMessage message);

		// Token: 0x060007CF RID: 1999
		void SetValue(IMessage message, object value);
	}
}
