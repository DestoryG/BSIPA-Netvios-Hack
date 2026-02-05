using System;

namespace System
{
	// Token: 0x02000004 RID: 4
	[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
	internal class MonoExtensionAttribute : MonoTODOAttribute
	{
		// Token: 0x06000005 RID: 5 RVA: 0x00002078 File Offset: 0x00000278
		public MonoExtensionAttribute(string comment)
			: base(comment)
		{
		}
	}
}
