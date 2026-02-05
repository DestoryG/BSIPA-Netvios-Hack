using System;

namespace System.Runtime.Serialization
{
	// Token: 0x02000099 RID: 153
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = true, AllowMultiple = true)]
	public sealed class KnownTypeAttribute : Attribute
	{
		// Token: 0x06000AAA RID: 2730 RVA: 0x0002D403 File Offset: 0x0002B603
		private KnownTypeAttribute()
		{
		}

		// Token: 0x06000AAB RID: 2731 RVA: 0x0002D40B File Offset: 0x0002B60B
		public KnownTypeAttribute(Type type)
		{
			this.type = type;
		}

		// Token: 0x06000AAC RID: 2732 RVA: 0x0002D41A File Offset: 0x0002B61A
		public KnownTypeAttribute(string methodName)
		{
			this.methodName = methodName;
		}

		// Token: 0x17000220 RID: 544
		// (get) Token: 0x06000AAD RID: 2733 RVA: 0x0002D429 File Offset: 0x0002B629
		public string MethodName
		{
			get
			{
				return this.methodName;
			}
		}

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x06000AAE RID: 2734 RVA: 0x0002D431 File Offset: 0x0002B631
		public Type Type
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x040004BA RID: 1210
		private string methodName;

		// Token: 0x040004BB RID: 1211
		private Type type;
	}
}
