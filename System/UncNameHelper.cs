using System;

namespace System
{
	// Token: 0x02000045 RID: 69
	internal class UncNameHelper
	{
		// Token: 0x060003E4 RID: 996 RVA: 0x0001BCE2 File Offset: 0x00019EE2
		private UncNameHelper()
		{
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x0001BCEA File Offset: 0x00019EEA
		internal static string ParseCanonicalName(string str, int start, int end, ref bool loopback)
		{
			return DomainNameHelper.ParseCanonicalName(str, start, end, ref loopback);
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x0001BCF8 File Offset: 0x00019EF8
		internal unsafe static bool IsValid(char* name, ushort start, ref int returnedEnd, bool notImplicitFile)
		{
			ushort num = (ushort)returnedEnd;
			if (start == num)
			{
				return false;
			}
			bool flag = false;
			ushort num2;
			for (num2 = start; num2 < num; num2 += 1)
			{
				if (name[num2] == '/' || name[num2] == '\\' || (notImplicitFile && (name[num2] == ':' || name[num2] == '?' || name[num2] == '#')))
				{
					num = num2;
					break;
				}
				if (name[num2] == '.')
				{
					num2 += 1;
					break;
				}
				if (char.IsLetter(name[num2]) || name[num2] == '-' || name[num2] == '_')
				{
					flag = true;
				}
				else if (name[num2] < '0' || name[num2] > '9')
				{
					return false;
				}
			}
			if (!flag)
			{
				return false;
			}
			while (num2 < num)
			{
				if (name[num2] == '/' || name[num2] == '\\' || (notImplicitFile && (name[num2] == ':' || name[num2] == '?' || name[num2] == '#')))
				{
					num = num2;
					break;
				}
				if (name[num2] == '.')
				{
					if (!flag || (num2 - 1 >= start && name[num2 - 1] == '.'))
					{
						return false;
					}
					flag = false;
				}
				else if (name[num2] == '-' || name[num2] == '_')
				{
					if (!flag)
					{
						return false;
					}
				}
				else
				{
					if (!char.IsLetter(name[num2]) && (name[num2] < '0' || name[num2] > '9'))
					{
						return false;
					}
					if (!flag)
					{
						flag = true;
					}
				}
				num2 += 1;
			}
			if (num2 - 1 >= start && name[num2 - 1] == '.')
			{
				flag = true;
			}
			if (!flag)
			{
				return false;
			}
			returnedEnd = (int)num;
			return true;
		}

		// Token: 0x0400045B RID: 1115
		internal const int MaximumInternetNameLength = 256;
	}
}
