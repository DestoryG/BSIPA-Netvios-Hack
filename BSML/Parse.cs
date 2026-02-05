using System;
using System.Globalization;

namespace BeatSaberMarkupLanguage
{
	// Token: 0x02000008 RID: 8
	public static class Parse
	{
		// Token: 0x06000035 RID: 53 RVA: 0x00002DDC File Offset: 0x00000FDC
		public static float Float(string s)
		{
			float num;
			try
			{
				num = float.Parse(s, CultureInfo.InvariantCulture);
			}
			catch
			{
				throw new Exception("Could not parse float: " + s);
			}
			return num;
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002E1C File Offset: 0x0000101C
		public static bool Bool(string s)
		{
			bool flag;
			try
			{
				flag = bool.Parse(s);
			}
			catch
			{
				throw new Exception("Could not parse bool: " + s);
			}
			return flag;
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00002E58 File Offset: 0x00001058
		public static int Int(string s)
		{
			int num;
			try
			{
				num = int.Parse(s, CultureInfo.InvariantCulture);
			}
			catch
			{
				throw new Exception("Could not parse int: " + s);
			}
			return num;
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002E98 File Offset: 0x00001098
		public static string InvariantToString(this object obj)
		{
			if (obj is float)
			{
				return ((float)obj).ToString(CultureInfo.InvariantCulture);
			}
			if (obj is double)
			{
				return ((double)obj).ToString(CultureInfo.InvariantCulture);
			}
			return obj.ToString();
		}
	}
}
