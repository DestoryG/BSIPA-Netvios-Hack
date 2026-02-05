using System;

namespace HarmonyLib
{
	// Token: 0x02000057 RID: 87
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Parameter, AllowMultiple = true)]
	public class HarmonyArgument : Attribute
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000174 RID: 372 RVA: 0x00009847 File Offset: 0x00007A47
		// (set) Token: 0x06000175 RID: 373 RVA: 0x0000984F File Offset: 0x00007A4F
		public string OriginalName { get; private set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000176 RID: 374 RVA: 0x00009858 File Offset: 0x00007A58
		// (set) Token: 0x06000177 RID: 375 RVA: 0x00009860 File Offset: 0x00007A60
		public int Index { get; private set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000178 RID: 376 RVA: 0x00009869 File Offset: 0x00007A69
		// (set) Token: 0x06000179 RID: 377 RVA: 0x00009871 File Offset: 0x00007A71
		public string NewName { get; private set; }

		// Token: 0x0600017A RID: 378 RVA: 0x0000987A File Offset: 0x00007A7A
		public HarmonyArgument(string originalName)
			: this(originalName, null)
		{
		}

		// Token: 0x0600017B RID: 379 RVA: 0x00009884 File Offset: 0x00007A84
		public HarmonyArgument(int index)
			: this(index, null)
		{
		}

		// Token: 0x0600017C RID: 380 RVA: 0x0000988E File Offset: 0x00007A8E
		public HarmonyArgument(string originalName, string newName)
		{
			this.OriginalName = originalName;
			this.Index = -1;
			this.NewName = newName;
		}

		// Token: 0x0600017D RID: 381 RVA: 0x000098AB File Offset: 0x00007AAB
		public HarmonyArgument(int index, string name)
		{
			this.OriginalName = null;
			this.Index = index;
			this.NewName = name;
		}
	}
}
