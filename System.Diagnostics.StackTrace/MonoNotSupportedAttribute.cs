using System;

namespace System
{
	// Token: 0x02000007 RID: 7
	[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
	internal class MonoNotSupportedAttribute : MonoTODOAttribute
	{
		// Token: 0x06000008 RID: 8 RVA: 0x00002093 File Offset: 0x00000293
		public MonoNotSupportedAttribute(string comment)
			: base(comment)
		{
		}
	}
}
