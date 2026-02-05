using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace IPA.Netvios
{
	/// <summary>
	/// 公共功能类
	/// </summary>
	// Token: 0x0200002E RID: 46
	public class Utils
	{
		/// <summary>
		/// 获取文件MD5值
		/// </summary>
		/// <param name="fileName">文件绝对路径</param>
		/// <returns></returns>
		// Token: 0x06000100 RID: 256 RVA: 0x00004BE4 File Offset: 0x00002DE4
		public static string GetMD5HashFromFile(string fileName)
		{
			string text;
			try
			{
				FileStream file = new FileStream(fileName, FileMode.Open);
				byte[] retVal = new MD5CryptoServiceProvider().ComputeHash(file);
				file.Close();
				StringBuilder sb = new StringBuilder();
				for (int i = 0; i < retVal.Length; i++)
				{
					sb.Append(retVal[i].ToString("x2"));
				}
				text = sb.ToString();
			}
			catch (Exception ex)
			{
				throw new Exception("GetMD5HashFromFile() fail,error:" + ex.Message);
			}
			return text;
		}

		/// <summary>
		/// check me
		/// </summary>
		/// <returns></returns>
		// Token: 0x06000101 RID: 257 RVA: 0x00004C6C File Offset: 0x00002E6C
		public static bool CheckIPA()
		{
			return true;
		}
	}
}
