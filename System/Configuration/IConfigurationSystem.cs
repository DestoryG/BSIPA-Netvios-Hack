using System;
using System.Runtime.InteropServices;

namespace System.Configuration
{
	// Token: 0x02000090 RID: 144
	[ComVisible(false)]
	public interface IConfigurationSystem
	{
		// Token: 0x0600056B RID: 1387
		object GetConfig(string configKey);

		// Token: 0x0600056C RID: 1388
		void Init();
	}
}
