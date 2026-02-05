using System;
using Microsoft.Win32;

namespace System.Diagnostics
{
	// Token: 0x020004E5 RID: 1253
	internal class CategoryEntry
	{
		// Token: 0x06002F69 RID: 12137 RVA: 0x000D5E70 File Offset: 0x000D4070
		internal CategoryEntry(NativeMethods.PERF_OBJECT_TYPE perfObject)
		{
			this.NameIndex = perfObject.ObjectNameTitleIndex;
			this.HelpIndex = perfObject.ObjectHelpTitleIndex;
			this.CounterIndexes = new int[perfObject.NumCounters];
			this.HelpIndexes = new int[perfObject.NumCounters];
		}

		// Token: 0x040027E3 RID: 10211
		internal int NameIndex;

		// Token: 0x040027E4 RID: 10212
		internal int HelpIndex;

		// Token: 0x040027E5 RID: 10213
		internal int[] CounterIndexes;

		// Token: 0x040027E6 RID: 10214
		internal int[] HelpIndexes;
	}
}
