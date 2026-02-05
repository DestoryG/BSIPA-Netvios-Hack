using System;
using System.Security.Cryptography;
using System.Text;

namespace NetViosCommon.Utility
{
	// Token: 0x02000005 RID: 5
	public class EncryptUtil
	{
		// Token: 0x0600000E RID: 14 RVA: 0x0000214C File Offset: 0x0000034C
		public static string Sha256(string randomString)
		{
			HashAlgorithm hashAlgorithm = new SHA256Managed();
			StringBuilder stringBuilder = new StringBuilder();
			foreach (byte b in hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(randomString)))
			{
				stringBuilder.Append(b.ToString("x2"));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000021A0 File Offset: 0x000003A0
		public static string GetMd5Hash(MD5 md5Hash, string input)
		{
			byte[] array = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < array.Length; i++)
			{
				stringBuilder.Append(array[i].ToString("x2"));
			}
			return stringBuilder.ToString();
		}
	}
}
