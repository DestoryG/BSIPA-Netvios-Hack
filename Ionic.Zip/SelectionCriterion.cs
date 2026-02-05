using System;
using System.Diagnostics;
using Ionic.Zip;

namespace Ionic
{
	// Token: 0x02000010 RID: 16
	internal abstract class SelectionCriterion
	{
		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000096 RID: 150 RVA: 0x00003658 File Offset: 0x00001858
		// (set) Token: 0x06000097 RID: 151 RVA: 0x00003660 File Offset: 0x00001860
		internal virtual bool Verbose { get; set; }

		// Token: 0x06000098 RID: 152
		internal abstract bool Evaluate(string filename);

		// Token: 0x06000099 RID: 153 RVA: 0x00003669 File Offset: 0x00001869
		[Conditional("SelectorTrace")]
		protected static void CriterionTrace(string format, params object[] args)
		{
		}

		// Token: 0x0600009A RID: 154
		internal abstract bool Evaluate(ZipEntry entry);
	}
}
