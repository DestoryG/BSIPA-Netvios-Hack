using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Win32;

namespace System.CodeDom.Compiler
{
	// Token: 0x02000684 RID: 1668
	internal static class RedistVersionInfo
	{
		// Token: 0x06003D6D RID: 15725 RVA: 0x000FBF90 File Offset: 0x000FA190
		public static string GetCompilerPath(IDictionary<string, string> provOptions, string compilerExecutable)
		{
			string text = Executor.GetRuntimeInstallDirectory();
			if (provOptions != null)
			{
				string text2;
				bool flag = provOptions.TryGetValue("CompilerDirectoryPath", out text2);
				string text3;
				bool flag2 = provOptions.TryGetValue("CompilerVersion", out text3);
				if (flag && flag2)
				{
					throw new InvalidOperationException(SR.GetString("Cannot_Specify_Both_Compiler_Path_And_Version", new object[] { "CompilerDirectoryPath", "CompilerVersion" }));
				}
				if (flag)
				{
					return text2;
				}
				if (flag2 && !(text3 == "v4.0"))
				{
					if (!(text3 == "v3.5"))
					{
						if (!(text3 == "v2.0"))
						{
							text = null;
						}
						else
						{
							text = RedistVersionInfo.GetCompilerPathFromRegistry(text3);
						}
					}
					else
					{
						text = RedistVersionInfo.GetCompilerPathFromRegistry(text3);
					}
				}
			}
			if (text == null)
			{
				throw new InvalidOperationException(SR.GetString("CompilerNotFound", new object[] { compilerExecutable }));
			}
			return text;
		}

		// Token: 0x06003D6E RID: 15726 RVA: 0x000FC058 File Offset: 0x000FA258
		private static string GetCompilerPathFromRegistry(string versionVal)
		{
			string environmentVariable = Environment.GetEnvironmentVariable("COMPLUS_InstallRoot");
			string environmentVariable2 = Environment.GetEnvironmentVariable("COMPLUS_Version");
			string text;
			if (!string.IsNullOrEmpty(environmentVariable) && !string.IsNullOrEmpty(environmentVariable2))
			{
				text = Path.Combine(environmentVariable, environmentVariable2);
				if (Directory.Exists(text))
				{
					return text;
				}
			}
			string text2 = versionVal.Substring(1);
			string text3 = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\MSBuild\\ToolsVersions\\" + text2;
			text = Registry.GetValue(text3, "MSBuildToolsPath", null) as string;
			if (text != null && Directory.Exists(text))
			{
				return text;
			}
			return null;
		}

		// Token: 0x04002CB9 RID: 11449
		internal const string DirectoryPath = "CompilerDirectoryPath";

		// Token: 0x04002CBA RID: 11450
		internal const string NameTag = "CompilerVersion";

		// Token: 0x04002CBB RID: 11451
		internal const string DefaultVersion = "v4.0";

		// Token: 0x04002CBC RID: 11452
		internal const string InPlaceVersion = "v4.0";

		// Token: 0x04002CBD RID: 11453
		internal const string RedistVersion = "v3.5";

		// Token: 0x04002CBE RID: 11454
		internal const string RedistVersion20 = "v2.0";

		// Token: 0x04002CBF RID: 11455
		private const string MSBuildToolsPath = "MSBuildToolsPath";

		// Token: 0x04002CC0 RID: 11456
		private const string dotNetFrameworkRegistryPath = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\MSBuild\\ToolsVersions\\";
	}
}
