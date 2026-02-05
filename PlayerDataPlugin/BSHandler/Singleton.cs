using System;

namespace PlayerDataPlugin.BSHandler
{
	// Token: 0x0200000E RID: 14
	public class Singleton<T> where T : class, new()
	{
		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000067 RID: 103 RVA: 0x00002F2F File Offset: 0x0000112F
		public static T Instance
		{
			get
			{
				if (Singleton<T>.mInstance == null)
				{
					Singleton<T>.mInstance = new T();
				}
				return Singleton<T>.mInstance;
			}
		}

		// Token: 0x04000028 RID: 40
		private static T mInstance;
	}
}
