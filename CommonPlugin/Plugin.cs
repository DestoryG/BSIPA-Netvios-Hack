using System;
using IPA;
using IPA.Logging;

namespace CommonPlugin
{
	// Token: 0x02000002 RID: 2
	[Plugin(RuntimeOptions.SingleStartInit)]
	public class Plugin
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		// (set) Token: 0x06000002 RID: 2 RVA: 0x00002057 File Offset: 0x00000257
		internal static Plugin instance { get; private set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000003 RID: 3 RVA: 0x0000205F File Offset: 0x0000025F
		internal static string Name
		{
			get
			{
				return "CommonPlugin";
			}
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002066 File Offset: 0x00000266
		[Init]
		public void Init(Logger logger)
		{
			Plugin.instance = this;
		}

		// Token: 0x06000005 RID: 5 RVA: 0x0000206E File Offset: 0x0000026E
		[OnStart]
		public void OnApplicationStart()
		{
		}

		// Token: 0x06000006 RID: 6 RVA: 0x0000206E File Offset: 0x0000026E
		[OnExit]
		public void OnApplicationQuit()
		{
		}
	}
}
