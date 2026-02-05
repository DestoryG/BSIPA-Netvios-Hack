using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020003DB RID: 987
	[AttributeUsage(AttributeTargets.Parameter)]
	[global::__DynamicallyInvokable]
	public sealed class DefaultParameterValueAttribute : Attribute
	{
		// Token: 0x060025F4 RID: 9716 RVA: 0x000B03C8 File Offset: 0x000AE5C8
		[global::__DynamicallyInvokable]
		public DefaultParameterValueAttribute(object value)
		{
			this.value = value;
		}

		// Token: 0x17000973 RID: 2419
		// (get) Token: 0x060025F5 RID: 9717 RVA: 0x000B03D7 File Offset: 0x000AE5D7
		[global::__DynamicallyInvokable]
		public object Value
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.value;
			}
		}

		// Token: 0x04002078 RID: 8312
		private object value;
	}
}
