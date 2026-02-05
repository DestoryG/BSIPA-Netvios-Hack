using System;

namespace BeatSaverSharp
{
	// Token: 0x02000006 RID: 6
	public struct ApplicationAgent
	{
		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600002A RID: 42 RVA: 0x00002999 File Offset: 0x00000B99
		// (set) Token: 0x0600002B RID: 43 RVA: 0x000029A1 File Offset: 0x00000BA1
		public string Name { readonly get; private set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600002C RID: 44 RVA: 0x000029AA File Offset: 0x00000BAA
		// (set) Token: 0x0600002D RID: 45 RVA: 0x000029B2 File Offset: 0x00000BB2
		public string Version { readonly get; private set; }

		// Token: 0x0600002E RID: 46 RVA: 0x000029BB File Offset: 0x00000BBB
		public ApplicationAgent(string name, Version version)
		{
			this.Name = name;
			this.Version = version.ToString();
		}

		// Token: 0x0600002F RID: 47 RVA: 0x000029D0 File Offset: 0x00000BD0
		public ApplicationAgent(string name, string version)
		{
			this.Name = name;
			this.Version = version;
		}
	}
}
