using System;

namespace System.Runtime.CompilerServices
{
	// Token: 0x0200031B RID: 795
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
	internal class IgnoresAccessChecksToAttribute : Attribute
	{
		// Token: 0x1700038D RID: 909
		// (get) Token: 0x06001233 RID: 4659 RVA: 0x0003CDAA File Offset: 0x0003AFAA
		public string AssemblyName { get; }

		// Token: 0x06001234 RID: 4660 RVA: 0x0003CDB2 File Offset: 0x0003AFB2
		public IgnoresAccessChecksToAttribute(string assemblyName)
		{
			this.AssemblyName = assemblyName;
		}
	}
}
