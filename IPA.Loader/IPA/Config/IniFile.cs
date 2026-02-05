using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace IPA.Config
{
	/// <summary>
	/// Create a New INI file to store or load data
	/// </summary>
	// Token: 0x02000061 RID: 97
	[Obsolete("Jesus, this uses old 16-bit system calls!")]
	internal class IniFile
	{
		// Token: 0x060002C2 RID: 706
		[DllImport("KERNEL32.DLL", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "GetPrivateProfileStringW", ExactSpelling = true, SetLastError = true)]
		private static extern int GetPrivateProfileString(string lpSection, string lpKey, string lpDefault, StringBuilder lpReturnString, int nSize, string lpFileName);

		// Token: 0x060002C3 RID: 707
		[DllImport("KERNEL32.DLL", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "WritePrivateProfileStringW", ExactSpelling = true, SetLastError = true)]
		private static extern int WritePrivateProfileString(string lpSection, string lpKey, string lpValue, string lpFileName);

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x060002C4 RID: 708 RVA: 0x0000D9E4 File Offset: 0x0000BBE4
		// (set) Token: 0x060002C5 RID: 709 RVA: 0x0000D9EC File Offset: 0x0000BBEC
		public FileInfo IniFileInfo
		{
			get
			{
				return this._iniFileInfo;
			}
			set
			{
				this._iniFileInfo = value;
				if (this._iniFileInfo.Exists)
				{
					return;
				}
				DirectoryInfo directory = this._iniFileInfo.Directory;
				if (directory != null)
				{
					directory.Create();
				}
				this._iniFileInfo.Create();
			}
		}

		/// <summary>
		/// INIFile Constructor.
		/// </summary>
		/// <PARAM name="iniPath"></PARAM>
		// Token: 0x060002C6 RID: 710 RVA: 0x0000DA25 File Offset: 0x0000BC25
		public IniFile(string iniPath)
		{
			this.IniFileInfo = new FileInfo(iniPath);
		}

		/// <summary>
		/// Write Data to the INI File
		/// </summary>
		/// <PARAM name="section"></PARAM>
		/// Section name
		/// <PARAM name="key"></PARAM>
		/// Key Name
		/// <PARAM name="value"></PARAM>
		/// Value Name
		// Token: 0x060002C7 RID: 711 RVA: 0x0000DA39 File Offset: 0x0000BC39
		public void IniWriteValue(string section, string key, string value)
		{
			IniFile.WritePrivateProfileString(section, key, value, this.IniFileInfo.FullName);
		}

		/// <summary>
		/// Read Data Value From the Ini File
		/// </summary>
		/// <PARAM name="section"></PARAM>
		/// <PARAM name="key"></PARAM>
		/// <returns></returns>
		// Token: 0x060002C8 RID: 712 RVA: 0x0000DA50 File Offset: 0x0000BC50
		public string IniReadValue(string section, string key)
		{
			StringBuilder result = new StringBuilder(1023);
			IniFile.GetPrivateProfileString(section, key, "", result, 1023, this.IniFileInfo.FullName);
			return result.ToString();
		}

		// Token: 0x04000108 RID: 264
		private FileInfo _iniFileInfo;
	}
}
