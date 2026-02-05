using System;

namespace System
{
	// Token: 0x02000006 RID: 6
	[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
	internal class MonoLimitationAttribute : MonoTODOAttribute
	{
		// Token: 0x06000007 RID: 7 RVA: 0x0000208A File Offset: 0x0000028A
		public MonoLimitationAttribute(string comment)
			: base(comment)
		{
		}
	}
}
