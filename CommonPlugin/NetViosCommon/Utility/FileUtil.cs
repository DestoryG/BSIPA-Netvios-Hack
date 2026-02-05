using System;
using System.IO;

namespace NetViosCommon.Utility
{
	// Token: 0x02000006 RID: 6
	public class FileUtil
	{
		// Token: 0x06000011 RID: 17 RVA: 0x000021F4 File Offset: 0x000003F4
		public static bool CreateDir(string dir)
		{
			try
			{
				Directory.CreateDirectory(dir);
			}
			catch (Exception)
			{
				return false;
			}
			return true;
		}
	}
}
