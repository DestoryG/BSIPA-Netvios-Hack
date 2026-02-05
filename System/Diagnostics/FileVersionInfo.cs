using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using Microsoft.Win32;

namespace System.Diagnostics
{
	// Token: 0x020004D7 RID: 1239
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public sealed class FileVersionInfo
	{
		// Token: 0x06002EB1 RID: 11953 RVA: 0x000D2054 File Offset: 0x000D0254
		private FileVersionInfo(string fileName)
		{
			this.fileName = fileName;
		}

		// Token: 0x17000B49 RID: 2889
		// (get) Token: 0x06002EB2 RID: 11954 RVA: 0x000D2063 File Offset: 0x000D0263
		public string Comments
		{
			get
			{
				return this.comments;
			}
		}

		// Token: 0x17000B4A RID: 2890
		// (get) Token: 0x06002EB3 RID: 11955 RVA: 0x000D206B File Offset: 0x000D026B
		public string CompanyName
		{
			get
			{
				return this.companyName;
			}
		}

		// Token: 0x17000B4B RID: 2891
		// (get) Token: 0x06002EB4 RID: 11956 RVA: 0x000D2073 File Offset: 0x000D0273
		public int FileBuildPart
		{
			get
			{
				return this.fileBuild;
			}
		}

		// Token: 0x17000B4C RID: 2892
		// (get) Token: 0x06002EB5 RID: 11957 RVA: 0x000D207B File Offset: 0x000D027B
		public string FileDescription
		{
			get
			{
				return this.fileDescription;
			}
		}

		// Token: 0x17000B4D RID: 2893
		// (get) Token: 0x06002EB6 RID: 11958 RVA: 0x000D2083 File Offset: 0x000D0283
		public int FileMajorPart
		{
			get
			{
				return this.fileMajor;
			}
		}

		// Token: 0x17000B4E RID: 2894
		// (get) Token: 0x06002EB7 RID: 11959 RVA: 0x000D208B File Offset: 0x000D028B
		public int FileMinorPart
		{
			get
			{
				return this.fileMinor;
			}
		}

		// Token: 0x17000B4F RID: 2895
		// (get) Token: 0x06002EB8 RID: 11960 RVA: 0x000D2093 File Offset: 0x000D0293
		public string FileName
		{
			get
			{
				new FileIOPermission(FileIOPermissionAccess.PathDiscovery, this.fileName).Demand();
				return this.fileName;
			}
		}

		// Token: 0x17000B50 RID: 2896
		// (get) Token: 0x06002EB9 RID: 11961 RVA: 0x000D20AC File Offset: 0x000D02AC
		public int FilePrivatePart
		{
			get
			{
				return this.filePrivate;
			}
		}

		// Token: 0x17000B51 RID: 2897
		// (get) Token: 0x06002EBA RID: 11962 RVA: 0x000D20B4 File Offset: 0x000D02B4
		public string FileVersion
		{
			get
			{
				return this.fileVersion;
			}
		}

		// Token: 0x17000B52 RID: 2898
		// (get) Token: 0x06002EBB RID: 11963 RVA: 0x000D20BC File Offset: 0x000D02BC
		public string InternalName
		{
			get
			{
				return this.internalName;
			}
		}

		// Token: 0x17000B53 RID: 2899
		// (get) Token: 0x06002EBC RID: 11964 RVA: 0x000D20C4 File Offset: 0x000D02C4
		public bool IsDebug
		{
			get
			{
				return (this.fileFlags & 1) != 0;
			}
		}

		// Token: 0x17000B54 RID: 2900
		// (get) Token: 0x06002EBD RID: 11965 RVA: 0x000D20D1 File Offset: 0x000D02D1
		public bool IsPatched
		{
			get
			{
				return (this.fileFlags & 4) != 0;
			}
		}

		// Token: 0x17000B55 RID: 2901
		// (get) Token: 0x06002EBE RID: 11966 RVA: 0x000D20DE File Offset: 0x000D02DE
		public bool IsPrivateBuild
		{
			get
			{
				return (this.fileFlags & 8) != 0;
			}
		}

		// Token: 0x17000B56 RID: 2902
		// (get) Token: 0x06002EBF RID: 11967 RVA: 0x000D20EB File Offset: 0x000D02EB
		public bool IsPreRelease
		{
			get
			{
				return (this.fileFlags & 2) != 0;
			}
		}

		// Token: 0x17000B57 RID: 2903
		// (get) Token: 0x06002EC0 RID: 11968 RVA: 0x000D20F8 File Offset: 0x000D02F8
		public bool IsSpecialBuild
		{
			get
			{
				return (this.fileFlags & 32) != 0;
			}
		}

		// Token: 0x17000B58 RID: 2904
		// (get) Token: 0x06002EC1 RID: 11969 RVA: 0x000D2106 File Offset: 0x000D0306
		public string Language
		{
			get
			{
				return this.language;
			}
		}

		// Token: 0x17000B59 RID: 2905
		// (get) Token: 0x06002EC2 RID: 11970 RVA: 0x000D210E File Offset: 0x000D030E
		public string LegalCopyright
		{
			get
			{
				return this.legalCopyright;
			}
		}

		// Token: 0x17000B5A RID: 2906
		// (get) Token: 0x06002EC3 RID: 11971 RVA: 0x000D2116 File Offset: 0x000D0316
		public string LegalTrademarks
		{
			get
			{
				return this.legalTrademarks;
			}
		}

		// Token: 0x17000B5B RID: 2907
		// (get) Token: 0x06002EC4 RID: 11972 RVA: 0x000D211E File Offset: 0x000D031E
		public string OriginalFilename
		{
			get
			{
				return this.originalFilename;
			}
		}

		// Token: 0x17000B5C RID: 2908
		// (get) Token: 0x06002EC5 RID: 11973 RVA: 0x000D2126 File Offset: 0x000D0326
		public string PrivateBuild
		{
			get
			{
				return this.privateBuild;
			}
		}

		// Token: 0x17000B5D RID: 2909
		// (get) Token: 0x06002EC6 RID: 11974 RVA: 0x000D212E File Offset: 0x000D032E
		public int ProductBuildPart
		{
			get
			{
				return this.productBuild;
			}
		}

		// Token: 0x17000B5E RID: 2910
		// (get) Token: 0x06002EC7 RID: 11975 RVA: 0x000D2136 File Offset: 0x000D0336
		public int ProductMajorPart
		{
			get
			{
				return this.productMajor;
			}
		}

		// Token: 0x17000B5F RID: 2911
		// (get) Token: 0x06002EC8 RID: 11976 RVA: 0x000D213E File Offset: 0x000D033E
		public int ProductMinorPart
		{
			get
			{
				return this.productMinor;
			}
		}

		// Token: 0x17000B60 RID: 2912
		// (get) Token: 0x06002EC9 RID: 11977 RVA: 0x000D2146 File Offset: 0x000D0346
		public string ProductName
		{
			get
			{
				return this.productName;
			}
		}

		// Token: 0x17000B61 RID: 2913
		// (get) Token: 0x06002ECA RID: 11978 RVA: 0x000D214E File Offset: 0x000D034E
		public int ProductPrivatePart
		{
			get
			{
				return this.productPrivate;
			}
		}

		// Token: 0x17000B62 RID: 2914
		// (get) Token: 0x06002ECB RID: 11979 RVA: 0x000D2156 File Offset: 0x000D0356
		public string ProductVersion
		{
			get
			{
				return this.productVersion;
			}
		}

		// Token: 0x17000B63 RID: 2915
		// (get) Token: 0x06002ECC RID: 11980 RVA: 0x000D215E File Offset: 0x000D035E
		public string SpecialBuild
		{
			get
			{
				return this.specialBuild;
			}
		}

		// Token: 0x06002ECD RID: 11981 RVA: 0x000D2168 File Offset: 0x000D0368
		private static string ConvertTo8DigitHex(int value)
		{
			string text = Convert.ToString(value, 16);
			text = text.ToUpper(CultureInfo.InvariantCulture);
			if (text.Length == 8)
			{
				return text;
			}
			StringBuilder stringBuilder = new StringBuilder(8);
			for (int i = text.Length; i < 8; i++)
			{
				stringBuilder.Append("0");
			}
			stringBuilder.Append(text);
			return stringBuilder.ToString();
		}

		// Token: 0x06002ECE RID: 11982 RVA: 0x000D21C8 File Offset: 0x000D03C8
		private static Microsoft.Win32.NativeMethods.VS_FIXEDFILEINFO GetFixedFileInfo(IntPtr memPtr)
		{
			IntPtr zero = IntPtr.Zero;
			int num;
			if (Microsoft.Win32.UnsafeNativeMethods.VerQueryValue(new HandleRef(null, memPtr), "\\", ref zero, out num))
			{
				Microsoft.Win32.NativeMethods.VS_FIXEDFILEINFO vs_FIXEDFILEINFO = new Microsoft.Win32.NativeMethods.VS_FIXEDFILEINFO();
				Marshal.PtrToStructure(zero, vs_FIXEDFILEINFO);
				return vs_FIXEDFILEINFO;
			}
			return new Microsoft.Win32.NativeMethods.VS_FIXEDFILEINFO();
		}

		// Token: 0x06002ECF RID: 11983 RVA: 0x000D2208 File Offset: 0x000D0408
		private static string GetFileVersionLanguage(IntPtr memPtr)
		{
			int num = FileVersionInfo.GetVarEntry(memPtr) >> 16;
			StringBuilder stringBuilder = new StringBuilder(256);
			Microsoft.Win32.UnsafeNativeMethods.VerLanguageName(num, stringBuilder, stringBuilder.Capacity);
			return stringBuilder.ToString();
		}

		// Token: 0x06002ED0 RID: 11984 RVA: 0x000D2240 File Offset: 0x000D0440
		private static string GetFileVersionString(IntPtr memPtr, string name)
		{
			string text = "";
			IntPtr zero = IntPtr.Zero;
			int num;
			if (Microsoft.Win32.UnsafeNativeMethods.VerQueryValue(new HandleRef(null, memPtr), name, ref zero, out num) && zero != IntPtr.Zero)
			{
				text = Marshal.PtrToStringAuto(zero);
			}
			return text;
		}

		// Token: 0x06002ED1 RID: 11985 RVA: 0x000D2284 File Offset: 0x000D0484
		private static int GetVarEntry(IntPtr memPtr)
		{
			IntPtr zero = IntPtr.Zero;
			int num;
			if (Microsoft.Win32.UnsafeNativeMethods.VerQueryValue(new HandleRef(null, memPtr), "\\VarFileInfo\\Translation", ref zero, out num))
			{
				return ((int)Marshal.ReadInt16(zero) << 16) + (int)Marshal.ReadInt16((IntPtr)((long)zero + 2L));
			}
			return 67699940;
		}

		// Token: 0x06002ED2 RID: 11986 RVA: 0x000D22D4 File Offset: 0x000D04D4
		private bool GetVersionInfoForCodePage(IntPtr memIntPtr, string codepage)
		{
			string text = "\\\\StringFileInfo\\\\{0}\\\\{1}";
			this.companyName = FileVersionInfo.GetFileVersionString(memIntPtr, string.Format(CultureInfo.InvariantCulture, text, new object[] { codepage, "CompanyName" }));
			this.fileDescription = FileVersionInfo.GetFileVersionString(memIntPtr, string.Format(CultureInfo.InvariantCulture, text, new object[] { codepage, "FileDescription" }));
			this.fileVersion = FileVersionInfo.GetFileVersionString(memIntPtr, string.Format(CultureInfo.InvariantCulture, text, new object[] { codepage, "FileVersion" }));
			this.internalName = FileVersionInfo.GetFileVersionString(memIntPtr, string.Format(CultureInfo.InvariantCulture, text, new object[] { codepage, "InternalName" }));
			this.legalCopyright = FileVersionInfo.GetFileVersionString(memIntPtr, string.Format(CultureInfo.InvariantCulture, text, new object[] { codepage, "LegalCopyright" }));
			this.originalFilename = FileVersionInfo.GetFileVersionString(memIntPtr, string.Format(CultureInfo.InvariantCulture, text, new object[] { codepage, "OriginalFilename" }));
			this.productName = FileVersionInfo.GetFileVersionString(memIntPtr, string.Format(CultureInfo.InvariantCulture, text, new object[] { codepage, "ProductName" }));
			this.productVersion = FileVersionInfo.GetFileVersionString(memIntPtr, string.Format(CultureInfo.InvariantCulture, text, new object[] { codepage, "ProductVersion" }));
			this.comments = FileVersionInfo.GetFileVersionString(memIntPtr, string.Format(CultureInfo.InvariantCulture, text, new object[] { codepage, "Comments" }));
			this.legalTrademarks = FileVersionInfo.GetFileVersionString(memIntPtr, string.Format(CultureInfo.InvariantCulture, text, new object[] { codepage, "LegalTrademarks" }));
			this.privateBuild = FileVersionInfo.GetFileVersionString(memIntPtr, string.Format(CultureInfo.InvariantCulture, text, new object[] { codepage, "PrivateBuild" }));
			this.specialBuild = FileVersionInfo.GetFileVersionString(memIntPtr, string.Format(CultureInfo.InvariantCulture, text, new object[] { codepage, "SpecialBuild" }));
			this.language = FileVersionInfo.GetFileVersionLanguage(memIntPtr);
			Microsoft.Win32.NativeMethods.VS_FIXEDFILEINFO fixedFileInfo = FileVersionInfo.GetFixedFileInfo(memIntPtr);
			this.fileMajor = FileVersionInfo.HIWORD(fixedFileInfo.dwFileVersionMS);
			this.fileMinor = FileVersionInfo.LOWORD(fixedFileInfo.dwFileVersionMS);
			this.fileBuild = FileVersionInfo.HIWORD(fixedFileInfo.dwFileVersionLS);
			this.filePrivate = FileVersionInfo.LOWORD(fixedFileInfo.dwFileVersionLS);
			this.productMajor = FileVersionInfo.HIWORD(fixedFileInfo.dwProductVersionMS);
			this.productMinor = FileVersionInfo.LOWORD(fixedFileInfo.dwProductVersionMS);
			this.productBuild = FileVersionInfo.HIWORD(fixedFileInfo.dwProductVersionLS);
			this.productPrivate = FileVersionInfo.LOWORD(fixedFileInfo.dwProductVersionLS);
			this.fileFlags = fixedFileInfo.dwFileFlags;
			return this.fileVersion != string.Empty;
		}

		// Token: 0x06002ED3 RID: 11987 RVA: 0x000D258A File Offset: 0x000D078A
		[FileIOPermission(SecurityAction.Assert, AllFiles = FileIOPermissionAccess.PathDiscovery)]
		private static string GetFullPathWithAssert(string fileName)
		{
			return Path.GetFullPath(fileName);
		}

		// Token: 0x06002ED4 RID: 11988 RVA: 0x000D2594 File Offset: 0x000D0794
		public unsafe static FileVersionInfo GetVersionInfo(string fileName)
		{
			if (!File.Exists(fileName))
			{
				string fullPathWithAssert = FileVersionInfo.GetFullPathWithAssert(fileName);
				new FileIOPermission(FileIOPermissionAccess.Read, fullPathWithAssert).Demand();
				throw new FileNotFoundException(fileName);
			}
			int num;
			int fileVersionInfoSize = Microsoft.Win32.UnsafeNativeMethods.GetFileVersionInfoSize(fileName, out num);
			FileVersionInfo fileVersionInfo = new FileVersionInfo(fileName);
			if (fileVersionInfoSize != 0)
			{
				byte[] array = new byte[fileVersionInfoSize];
				byte[] array2;
				byte* ptr;
				if ((array2 = array) == null || array2.Length == 0)
				{
					ptr = null;
				}
				else
				{
					ptr = &array2[0];
				}
				IntPtr intPtr = new IntPtr((void*)ptr);
				if (Microsoft.Win32.UnsafeNativeMethods.GetFileVersionInfo(fileName, 0, fileVersionInfoSize, new HandleRef(null, intPtr)))
				{
					int varEntry = FileVersionInfo.GetVarEntry(intPtr);
					if (!fileVersionInfo.GetVersionInfoForCodePage(intPtr, FileVersionInfo.ConvertTo8DigitHex(varEntry)))
					{
						int[] array3 = new int[] { 67699888, 67699940, 67698688 };
						foreach (int num2 in array3)
						{
							if (num2 != varEntry && fileVersionInfo.GetVersionInfoForCodePage(intPtr, FileVersionInfo.ConvertTo8DigitHex(num2)))
							{
								break;
							}
						}
					}
				}
				array2 = null;
			}
			return fileVersionInfo;
		}

		// Token: 0x06002ED5 RID: 11989 RVA: 0x000D2680 File Offset: 0x000D0880
		private static int HIWORD(int dword)
		{
			return Microsoft.Win32.NativeMethods.Util.HIWORD(dword);
		}

		// Token: 0x06002ED6 RID: 11990 RVA: 0x000D2688 File Offset: 0x000D0888
		private static int LOWORD(int dword)
		{
			return Microsoft.Win32.NativeMethods.Util.LOWORD(dword);
		}

		// Token: 0x06002ED7 RID: 11991 RVA: 0x000D2690 File Offset: 0x000D0890
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder(128);
			string text = "\r\n";
			stringBuilder.Append("File:             ");
			stringBuilder.Append(this.FileName);
			stringBuilder.Append(text);
			stringBuilder.Append("InternalName:     ");
			stringBuilder.Append(this.InternalName);
			stringBuilder.Append(text);
			stringBuilder.Append("OriginalFilename: ");
			stringBuilder.Append(this.OriginalFilename);
			stringBuilder.Append(text);
			stringBuilder.Append("FileVersion:      ");
			stringBuilder.Append(this.FileVersion);
			stringBuilder.Append(text);
			stringBuilder.Append("FileDescription:  ");
			stringBuilder.Append(this.FileDescription);
			stringBuilder.Append(text);
			stringBuilder.Append("Product:          ");
			stringBuilder.Append(this.ProductName);
			stringBuilder.Append(text);
			stringBuilder.Append("ProductVersion:   ");
			stringBuilder.Append(this.ProductVersion);
			stringBuilder.Append(text);
			stringBuilder.Append("Debug:            ");
			stringBuilder.Append(this.IsDebug.ToString());
			stringBuilder.Append(text);
			stringBuilder.Append("Patched:          ");
			stringBuilder.Append(this.IsPatched.ToString());
			stringBuilder.Append(text);
			stringBuilder.Append("PreRelease:       ");
			stringBuilder.Append(this.IsPreRelease.ToString());
			stringBuilder.Append(text);
			stringBuilder.Append("PrivateBuild:     ");
			stringBuilder.Append(this.IsPrivateBuild.ToString());
			stringBuilder.Append(text);
			stringBuilder.Append("SpecialBuild:     ");
			stringBuilder.Append(this.IsSpecialBuild.ToString());
			stringBuilder.Append(text);
			stringBuilder.Append("Language:         ");
			stringBuilder.Append(this.Language);
			stringBuilder.Append(text);
			return stringBuilder.ToString();
		}

		// Token: 0x0400277E RID: 10110
		private string fileName;

		// Token: 0x0400277F RID: 10111
		private string companyName;

		// Token: 0x04002780 RID: 10112
		private string fileDescription;

		// Token: 0x04002781 RID: 10113
		private string fileVersion;

		// Token: 0x04002782 RID: 10114
		private string internalName;

		// Token: 0x04002783 RID: 10115
		private string legalCopyright;

		// Token: 0x04002784 RID: 10116
		private string originalFilename;

		// Token: 0x04002785 RID: 10117
		private string productName;

		// Token: 0x04002786 RID: 10118
		private string productVersion;

		// Token: 0x04002787 RID: 10119
		private string comments;

		// Token: 0x04002788 RID: 10120
		private string legalTrademarks;

		// Token: 0x04002789 RID: 10121
		private string privateBuild;

		// Token: 0x0400278A RID: 10122
		private string specialBuild;

		// Token: 0x0400278B RID: 10123
		private string language;

		// Token: 0x0400278C RID: 10124
		private int fileMajor;

		// Token: 0x0400278D RID: 10125
		private int fileMinor;

		// Token: 0x0400278E RID: 10126
		private int fileBuild;

		// Token: 0x0400278F RID: 10127
		private int filePrivate;

		// Token: 0x04002790 RID: 10128
		private int productMajor;

		// Token: 0x04002791 RID: 10129
		private int productMinor;

		// Token: 0x04002792 RID: 10130
		private int productBuild;

		// Token: 0x04002793 RID: 10131
		private int productPrivate;

		// Token: 0x04002794 RID: 10132
		private int fileFlags;
	}
}
