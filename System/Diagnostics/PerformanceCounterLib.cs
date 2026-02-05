using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using Microsoft.Win32;

namespace System.Diagnostics
{
	// Token: 0x020004E3 RID: 1251
	internal class PerformanceCounterLib
	{
		// Token: 0x17000B7D RID: 2941
		// (get) Token: 0x06002F39 RID: 12089 RVA: 0x000D406C File Offset: 0x000D226C
		private static object InternalSyncObject
		{
			get
			{
				if (PerformanceCounterLib.s_InternalSyncObject == null)
				{
					object obj = new object();
					Interlocked.CompareExchange(ref PerformanceCounterLib.s_InternalSyncObject, obj, null);
				}
				return PerformanceCounterLib.s_InternalSyncObject;
			}
		}

		// Token: 0x06002F3A RID: 12090 RVA: 0x000D4098 File Offset: 0x000D2298
		internal PerformanceCounterLib(string machineName, string lcid)
		{
			this.machineName = machineName;
			this.perfLcid = lcid;
		}

		// Token: 0x17000B7E RID: 2942
		// (get) Token: 0x06002F3B RID: 12091 RVA: 0x000D40D0 File Offset: 0x000D22D0
		internal static string ComputerName
		{
			get
			{
				if (PerformanceCounterLib.computerName == null)
				{
					object internalSyncObject = PerformanceCounterLib.InternalSyncObject;
					lock (internalSyncObject)
					{
						if (PerformanceCounterLib.computerName == null)
						{
							StringBuilder stringBuilder = new StringBuilder(256);
							SafeNativeMethods.GetComputerName(stringBuilder, new int[] { stringBuilder.Capacity });
							PerformanceCounterLib.computerName = stringBuilder.ToString();
						}
					}
				}
				return PerformanceCounterLib.computerName;
			}
		}

		// Token: 0x17000B7F RID: 2943
		// (get) Token: 0x06002F3C RID: 12092 RVA: 0x000D4150 File Offset: 0x000D2350
		private unsafe Hashtable CategoryTable
		{
			get
			{
				if (this.categoryTable == null)
				{
					object categoryTableLock = this.CategoryTableLock;
					lock (categoryTableLock)
					{
						if (this.categoryTable == null)
						{
							byte[] performanceData = this.GetPerformanceData("Global");
							byte[] array;
							byte* ptr;
							if ((array = performanceData) == null || array.Length == 0)
							{
								ptr = null;
							}
							else
							{
								ptr = &array[0];
							}
							IntPtr intPtr = new IntPtr((void*)ptr);
							IntPtr intPtr2 = intPtr;
							Microsoft.Win32.NativeMethods.PERF_DATA_BLOCK.ValidateBeforeRead(intPtr, performanceData.Length, intPtr2);
							Microsoft.Win32.NativeMethods.PERF_DATA_BLOCK perf_DATA_BLOCK = new Microsoft.Win32.NativeMethods.PERF_DATA_BLOCK();
							Marshal.PtrToStructure(intPtr2, perf_DATA_BLOCK);
							perf_DATA_BLOCK.Validate(performanceData.Length);
							intPtr2 = (IntPtr)((long)intPtr2 + (long)perf_DATA_BLOCK.HeaderLength);
							int numObjectTypes = perf_DATA_BLOCK.NumObjectTypes;
							long num = (long)new IntPtr((void*)ptr) + (long)perf_DATA_BLOCK.TotalByteLength;
							Hashtable hashtable = new Hashtable(numObjectTypes, StringComparer.OrdinalIgnoreCase);
							int num2 = 0;
							while (num2 < numObjectTypes && (long)intPtr2 < num)
							{
								Microsoft.Win32.NativeMethods.PERF_OBJECT_TYPE.ValidateBeforeRead(intPtr, performanceData.Length, intPtr2);
								Microsoft.Win32.NativeMethods.PERF_OBJECT_TYPE perf_OBJECT_TYPE = new Microsoft.Win32.NativeMethods.PERF_OBJECT_TYPE();
								Marshal.PtrToStructure(intPtr2, perf_OBJECT_TYPE);
								perf_OBJECT_TYPE.Validate(performanceData.Length - (int)((long)intPtr2 - (long)intPtr));
								CategoryEntry categoryEntry = new CategoryEntry(perf_OBJECT_TYPE);
								IntPtr intPtr3 = (IntPtr)((long)intPtr2 + (long)perf_OBJECT_TYPE.TotalByteLength);
								intPtr2 = (IntPtr)((long)intPtr2 + (long)perf_OBJECT_TYPE.HeaderLength);
								int num3 = 0;
								int num4 = -1;
								for (int i = 0; i < categoryEntry.CounterIndexes.Length; i++)
								{
									Microsoft.Win32.NativeMethods.PERF_COUNTER_DEFINITION.ValidateBeforeRead(intPtr, performanceData.Length, intPtr2);
									Microsoft.Win32.NativeMethods.PERF_COUNTER_DEFINITION perf_COUNTER_DEFINITION = new Microsoft.Win32.NativeMethods.PERF_COUNTER_DEFINITION();
									Marshal.PtrToStructure(intPtr2, perf_COUNTER_DEFINITION);
									perf_COUNTER_DEFINITION.Validate(performanceData.Length - (int)((long)intPtr2 - (long)intPtr));
									if (perf_COUNTER_DEFINITION.CounterNameTitleIndex != num4)
									{
										categoryEntry.CounterIndexes[num3] = perf_COUNTER_DEFINITION.CounterNameTitleIndex;
										categoryEntry.HelpIndexes[num3] = perf_COUNTER_DEFINITION.CounterHelpTitleIndex;
										num4 = perf_COUNTER_DEFINITION.CounterNameTitleIndex;
										num3++;
									}
									intPtr2 = (IntPtr)((long)intPtr2 + (long)perf_COUNTER_DEFINITION.ByteLength);
								}
								if (num3 < categoryEntry.CounterIndexes.Length)
								{
									int[] array2 = new int[num3];
									int[] array3 = new int[num3];
									Array.Copy(categoryEntry.CounterIndexes, array2, num3);
									Array.Copy(categoryEntry.HelpIndexes, array3, num3);
									categoryEntry.CounterIndexes = array2;
									categoryEntry.HelpIndexes = array3;
								}
								string text = (string)this.NameTable[categoryEntry.NameIndex];
								if (text != null)
								{
									hashtable[text] = categoryEntry;
								}
								intPtr2 = intPtr3;
								num2++;
							}
							this.categoryTable = hashtable;
							array = null;
						}
					}
				}
				return this.categoryTable;
			}
		}

		// Token: 0x17000B80 RID: 2944
		// (get) Token: 0x06002F3D RID: 12093 RVA: 0x000D441C File Offset: 0x000D261C
		internal Hashtable HelpTable
		{
			get
			{
				if (this.helpTable == null)
				{
					object helpTableLock = this.HelpTableLock;
					lock (helpTableLock)
					{
						if (this.helpTable == null)
						{
							this.helpTable = this.GetStringTable(true);
						}
					}
				}
				return this.helpTable;
			}
		}

		// Token: 0x17000B81 RID: 2945
		// (get) Token: 0x06002F3E RID: 12094 RVA: 0x000D447C File Offset: 0x000D267C
		private static string IniFilePath
		{
			get
			{
				if (PerformanceCounterLib.iniFilePath == null)
				{
					object internalSyncObject = PerformanceCounterLib.InternalSyncObject;
					lock (internalSyncObject)
					{
						if (PerformanceCounterLib.iniFilePath == null)
						{
							EnvironmentPermission environmentPermission = new EnvironmentPermission(PermissionState.Unrestricted);
							environmentPermission.Assert();
							try
							{
								PerformanceCounterLib.iniFilePath = Path.GetTempFileName();
							}
							finally
							{
								CodeAccessPermission.RevertAssert();
							}
						}
					}
				}
				return PerformanceCounterLib.iniFilePath;
			}
		}

		// Token: 0x17000B82 RID: 2946
		// (get) Token: 0x06002F3F RID: 12095 RVA: 0x000D44FC File Offset: 0x000D26FC
		internal Hashtable NameTable
		{
			get
			{
				if (this.nameTable == null)
				{
					object nameTableLock = this.NameTableLock;
					lock (nameTableLock)
					{
						if (this.nameTable == null)
						{
							this.nameTable = this.GetStringTable(false);
						}
					}
				}
				return this.nameTable;
			}
		}

		// Token: 0x17000B83 RID: 2947
		// (get) Token: 0x06002F40 RID: 12096 RVA: 0x000D455C File Offset: 0x000D275C
		private static string SymbolFilePath
		{
			get
			{
				if (PerformanceCounterLib.symbolFilePath == null)
				{
					object internalSyncObject = PerformanceCounterLib.InternalSyncObject;
					lock (internalSyncObject)
					{
						if (PerformanceCounterLib.symbolFilePath == null)
						{
							EnvironmentPermission environmentPermission = new EnvironmentPermission(PermissionState.Unrestricted);
							environmentPermission.Assert();
							string tempPath = Path.GetTempPath();
							CodeAccessPermission.RevertAssert();
							PermissionSet permissionSet = new PermissionSet(PermissionState.None);
							permissionSet.AddPermission(new EnvironmentPermission(PermissionState.Unrestricted));
							permissionSet.AddPermission(new FileIOPermission(FileIOPermissionAccess.Write, tempPath));
							permissionSet.Assert();
							try
							{
								PerformanceCounterLib.symbolFilePath = Path.GetTempFileName();
							}
							finally
							{
								PermissionSet.RevertAssert();
							}
						}
					}
				}
				return PerformanceCounterLib.symbolFilePath;
			}
		}

		// Token: 0x06002F41 RID: 12097 RVA: 0x000D4614 File Offset: 0x000D2814
		internal static bool CategoryExists(string machine, string category)
		{
			PerformanceCounterLib performanceCounterLib = PerformanceCounterLib.GetPerformanceCounterLib(machine, new CultureInfo(9));
			if (performanceCounterLib.CategoryExists(category))
			{
				return true;
			}
			if (CultureInfo.CurrentCulture.Parent.LCID != 9)
			{
				for (CultureInfo cultureInfo = CultureInfo.CurrentCulture; cultureInfo != CultureInfo.InvariantCulture; cultureInfo = cultureInfo.Parent)
				{
					performanceCounterLib = PerformanceCounterLib.GetPerformanceCounterLib(machine, cultureInfo);
					if (performanceCounterLib.CategoryExists(category))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06002F42 RID: 12098 RVA: 0x000D4678 File Offset: 0x000D2878
		internal bool CategoryExists(string category)
		{
			return this.CategoryTable.ContainsKey(category);
		}

		// Token: 0x06002F43 RID: 12099 RVA: 0x000D4688 File Offset: 0x000D2888
		internal static void CloseAllLibraries()
		{
			if (PerformanceCounterLib.libraryTable != null)
			{
				foreach (object obj in PerformanceCounterLib.libraryTable.Values)
				{
					PerformanceCounterLib performanceCounterLib = (PerformanceCounterLib)obj;
					performanceCounterLib.Close();
				}
				PerformanceCounterLib.libraryTable = null;
			}
		}

		// Token: 0x06002F44 RID: 12100 RVA: 0x000D46F8 File Offset: 0x000D28F8
		internal static void CloseAllTables()
		{
			if (PerformanceCounterLib.libraryTable != null)
			{
				foreach (object obj in PerformanceCounterLib.libraryTable.Values)
				{
					PerformanceCounterLib performanceCounterLib = (PerformanceCounterLib)obj;
					performanceCounterLib.CloseTables();
				}
			}
		}

		// Token: 0x06002F45 RID: 12101 RVA: 0x000D4760 File Offset: 0x000D2960
		internal void CloseTables()
		{
			this.nameTable = null;
			this.helpTable = null;
			this.categoryTable = null;
			this.customCategoryTable = null;
		}

		// Token: 0x06002F46 RID: 12102 RVA: 0x000D477E File Offset: 0x000D297E
		internal void Close()
		{
			if (this.performanceMonitor != null)
			{
				this.performanceMonitor.Close();
				this.performanceMonitor = null;
			}
			this.CloseTables();
		}

		// Token: 0x06002F47 RID: 12103 RVA: 0x000D47A0 File Offset: 0x000D29A0
		internal static bool CounterExists(string machine, string category, string counter)
		{
			PerformanceCounterLib performanceCounterLib = PerformanceCounterLib.GetPerformanceCounterLib(machine, new CultureInfo(9));
			bool flag = false;
			bool flag2 = performanceCounterLib.CounterExists(category, counter, ref flag);
			if (!flag && CultureInfo.CurrentCulture.Parent.LCID != 9)
			{
				for (CultureInfo cultureInfo = CultureInfo.CurrentCulture; cultureInfo != CultureInfo.InvariantCulture; cultureInfo = cultureInfo.Parent)
				{
					performanceCounterLib = PerformanceCounterLib.GetPerformanceCounterLib(machine, cultureInfo);
					flag2 = performanceCounterLib.CounterExists(category, counter, ref flag);
					if (flag2)
					{
						break;
					}
				}
			}
			if (!flag)
			{
				throw new InvalidOperationException(SR.GetString("MissingCategory"));
			}
			return flag2;
		}

		// Token: 0x06002F48 RID: 12104 RVA: 0x000D4820 File Offset: 0x000D2A20
		private bool CounterExists(string category, string counter, ref bool categoryExists)
		{
			categoryExists = false;
			if (!this.CategoryTable.ContainsKey(category))
			{
				return false;
			}
			categoryExists = true;
			CategoryEntry categoryEntry = (CategoryEntry)this.CategoryTable[category];
			for (int i = 0; i < categoryEntry.CounterIndexes.Length; i++)
			{
				int num = categoryEntry.CounterIndexes[i];
				string text = (string)this.NameTable[num];
				if (text == null)
				{
					text = string.Empty;
				}
				if (string.Compare(text, counter, StringComparison.OrdinalIgnoreCase) == 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002F49 RID: 12105 RVA: 0x000D48A0 File Offset: 0x000D2AA0
		private static void CreateIniFile(string categoryName, string categoryHelp, CounterCreationDataCollection creationData, string[] languageIds)
		{
			FileIOPermission fileIOPermission = new FileIOPermission(PermissionState.Unrestricted);
			fileIOPermission.Assert();
			try
			{
				StreamWriter streamWriter = new StreamWriter(PerformanceCounterLib.IniFilePath, false, Encoding.Unicode);
				try
				{
					streamWriter.WriteLine("");
					streamWriter.WriteLine("[info]");
					streamWriter.Write("drivername");
					streamWriter.Write("=");
					streamWriter.WriteLine(categoryName);
					streamWriter.Write("symbolfile");
					streamWriter.Write("=");
					streamWriter.WriteLine(Path.GetFileName(PerformanceCounterLib.SymbolFilePath));
					streamWriter.WriteLine("");
					streamWriter.WriteLine("[languages]");
					foreach (string text in languageIds)
					{
						streamWriter.Write(text);
						streamWriter.Write("=");
						streamWriter.Write("language");
						streamWriter.WriteLine(text);
					}
					streamWriter.WriteLine("");
					streamWriter.WriteLine("[objects]");
					foreach (string text2 in languageIds)
					{
						streamWriter.Write("OBJECT_");
						streamWriter.Write("1_");
						streamWriter.Write(text2);
						streamWriter.Write("_NAME");
						streamWriter.Write("=");
						streamWriter.WriteLine(categoryName);
					}
					streamWriter.WriteLine("");
					streamWriter.WriteLine("[text]");
					foreach (string text3 in languageIds)
					{
						streamWriter.Write("OBJECT_");
						streamWriter.Write("1_");
						streamWriter.Write(text3);
						streamWriter.Write("_NAME");
						streamWriter.Write("=");
						streamWriter.WriteLine(categoryName);
						streamWriter.Write("OBJECT_");
						streamWriter.Write("1_");
						streamWriter.Write(text3);
						streamWriter.Write("_HELP");
						streamWriter.Write("=");
						if (categoryHelp == null || categoryHelp == string.Empty)
						{
							streamWriter.WriteLine(SR.GetString("HelpNotAvailable"));
						}
						else
						{
							streamWriter.WriteLine(categoryHelp);
						}
						int num = 0;
						foreach (object obj in creationData)
						{
							CounterCreationData counterCreationData = (CounterCreationData)obj;
							num++;
							streamWriter.WriteLine("");
							streamWriter.Write("DEVICE_COUNTER_");
							streamWriter.Write(num.ToString(CultureInfo.InvariantCulture));
							streamWriter.Write("_");
							streamWriter.Write(text3);
							streamWriter.Write("_NAME");
							streamWriter.Write("=");
							streamWriter.WriteLine(counterCreationData.CounterName);
							streamWriter.Write("DEVICE_COUNTER_");
							streamWriter.Write(num.ToString(CultureInfo.InvariantCulture));
							streamWriter.Write("_");
							streamWriter.Write(text3);
							streamWriter.Write("_HELP");
							streamWriter.Write("=");
							streamWriter.WriteLine(counterCreationData.CounterHelp);
						}
					}
					streamWriter.WriteLine("");
				}
				finally
				{
					streamWriter.Close();
				}
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
		}

		// Token: 0x06002F4A RID: 12106 RVA: 0x000D4C14 File Offset: 0x000D2E14
		private static void CreateRegistryEntry(string categoryName, PerformanceCounterCategoryType categoryType, CounterCreationDataCollection creationData, ref bool iniRegistered)
		{
			RegistryKey registryKey = null;
			RegistryKey registryKey2 = null;
			RegistryKey registryKey3 = null;
			RegistryPermission registryPermission = new RegistryPermission(PermissionState.Unrestricted);
			registryPermission.Assert();
			try
			{
				registryKey = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Services", true);
				registryKey2 = registryKey.OpenSubKey(categoryName + "\\Performance", true);
				if (registryKey2 == null)
				{
					registryKey2 = registryKey.CreateSubKey(categoryName + "\\Performance");
				}
				registryKey2.SetValue("Open", "OpenPerformanceData");
				registryKey2.SetValue("Collect", "CollectPerformanceData");
				registryKey2.SetValue("Close", "ClosePerformanceData");
				registryKey2.SetValue("Library", "netfxperf.dll");
				registryKey2.SetValue("IsMultiInstance", (int)categoryType, RegistryValueKind.DWord);
				registryKey2.SetValue("CategoryOptions", 3, RegistryValueKind.DWord);
				string[] array = new string[creationData.Count];
				string[] array2 = new string[creationData.Count];
				for (int i = 0; i < creationData.Count; i++)
				{
					array[i] = creationData[i].CounterName;
					array2[i] = ((int)creationData[i].CounterType).ToString(CultureInfo.InvariantCulture);
				}
				registryKey3 = registryKey.OpenSubKey(categoryName + "\\Linkage", true);
				if (registryKey3 == null)
				{
					registryKey3 = registryKey.CreateSubKey(categoryName + "\\Linkage");
				}
				registryKey3.SetValue("Export", new string[] { categoryName });
				registryKey2.SetValue("Counter Types", array2);
				registryKey2.SetValue("Counter Names", array);
				object value = registryKey2.GetValue("First Counter");
				if (value != null)
				{
					iniRegistered = true;
				}
				else
				{
					iniRegistered = false;
				}
			}
			finally
			{
				if (registryKey2 != null)
				{
					registryKey2.Close();
				}
				if (registryKey3 != null)
				{
					registryKey3.Close();
				}
				if (registryKey != null)
				{
					registryKey.Close();
				}
				CodeAccessPermission.RevertAssert();
			}
		}

		// Token: 0x06002F4B RID: 12107 RVA: 0x000D4DE4 File Offset: 0x000D2FE4
		private static void CreateSymbolFile(CounterCreationDataCollection creationData)
		{
			FileIOPermission fileIOPermission = new FileIOPermission(PermissionState.Unrestricted);
			fileIOPermission.Assert();
			try
			{
				StreamWriter streamWriter = new StreamWriter(PerformanceCounterLib.SymbolFilePath);
				try
				{
					streamWriter.Write("#define");
					streamWriter.Write(" ");
					streamWriter.Write("OBJECT_");
					streamWriter.WriteLine("1 0;");
					for (int i = 1; i <= creationData.Count; i++)
					{
						streamWriter.Write("#define");
						streamWriter.Write(" ");
						streamWriter.Write("DEVICE_COUNTER_");
						streamWriter.Write(i.ToString(CultureInfo.InvariantCulture));
						streamWriter.Write(" ");
						streamWriter.Write((i * 2).ToString(CultureInfo.InvariantCulture));
						streamWriter.WriteLine(";");
					}
					streamWriter.WriteLine("");
				}
				finally
				{
					streamWriter.Close();
				}
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
		}

		// Token: 0x06002F4C RID: 12108 RVA: 0x000D4EDC File Offset: 0x000D30DC
		private static void DeleteRegistryEntry(string categoryName)
		{
			RegistryKey registryKey = null;
			RegistryPermission registryPermission = new RegistryPermission(PermissionState.Unrestricted);
			registryPermission.Assert();
			try
			{
				registryKey = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Services", true);
				bool flag = false;
				using (RegistryKey registryKey2 = registryKey.OpenSubKey(categoryName, true))
				{
					if (registryKey2 != null)
					{
						if (registryKey2.GetValueNames().Length == 0)
						{
							flag = true;
						}
						else
						{
							registryKey2.DeleteSubKeyTree("Linkage");
							registryKey2.DeleteSubKeyTree("Performance");
						}
					}
				}
				if (flag)
				{
					registryKey.DeleteSubKeyTree(categoryName);
				}
			}
			finally
			{
				if (registryKey != null)
				{
					registryKey.Close();
				}
				CodeAccessPermission.RevertAssert();
			}
		}

		// Token: 0x06002F4D RID: 12109 RVA: 0x000D4F80 File Offset: 0x000D3180
		private static void DeleteTemporaryFiles()
		{
			try
			{
				File.Delete(PerformanceCounterLib.IniFilePath);
			}
			catch
			{
			}
			try
			{
				File.Delete(PerformanceCounterLib.SymbolFilePath);
			}
			catch
			{
			}
		}

		// Token: 0x06002F4E RID: 12110 RVA: 0x000D4FC8 File Offset: 0x000D31C8
		internal bool FindCustomCategory(string category, out PerformanceCounterCategoryType categoryType)
		{
			RegistryKey registryKey = null;
			RegistryKey registryKey2 = null;
			categoryType = PerformanceCounterCategoryType.Unknown;
			if (this.customCategoryTable == null)
			{
				Interlocked.CompareExchange<Hashtable>(ref this.customCategoryTable, new Hashtable(StringComparer.OrdinalIgnoreCase), null);
			}
			if (this.customCategoryTable.ContainsKey(category))
			{
				categoryType = (PerformanceCounterCategoryType)this.customCategoryTable[category];
				return true;
			}
			PermissionSet permissionSet = new PermissionSet(PermissionState.None);
			permissionSet.AddPermission(new RegistryPermission(PermissionState.Unrestricted));
			permissionSet.AddPermission(new SecurityPermission(SecurityPermissionFlag.UnmanagedCode));
			permissionSet.Assert();
			try
			{
				string text = "SYSTEM\\CurrentControlSet\\Services\\" + category + "\\Performance";
				if (this.machineName == "." || string.Compare(this.machineName, PerformanceCounterLib.ComputerName, StringComparison.OrdinalIgnoreCase) == 0)
				{
					registryKey = Registry.LocalMachine.OpenSubKey(text);
				}
				else
				{
					registryKey2 = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, "\\\\" + this.machineName);
					if (registryKey2 != null)
					{
						try
						{
							registryKey = registryKey2.OpenSubKey(text);
						}
						catch (SecurityException)
						{
							categoryType = PerformanceCounterCategoryType.Unknown;
							this.customCategoryTable[category] = categoryType;
							return false;
						}
					}
				}
				if (registryKey != null)
				{
					object value = registryKey.GetValue("Library", null, RegistryValueOptions.DoNotExpandEnvironmentNames);
					if (value != null && value is string && (string.Compare((string)value, "netfxperf.dll", StringComparison.OrdinalIgnoreCase) == 0 || ((string)value).EndsWith("\\netfxperf.dll", StringComparison.OrdinalIgnoreCase)))
					{
						object value2 = registryKey.GetValue("IsMultiInstance");
						if (value2 != null)
						{
							categoryType = (PerformanceCounterCategoryType)value2;
							if (categoryType < PerformanceCounterCategoryType.Unknown || categoryType > PerformanceCounterCategoryType.MultiInstance)
							{
								categoryType = PerformanceCounterCategoryType.Unknown;
							}
						}
						else
						{
							categoryType = PerformanceCounterCategoryType.Unknown;
						}
						object value3 = registryKey.GetValue("First Counter");
						if (value3 != null)
						{
							int num = (int)value3;
							this.customCategoryTable[category] = categoryType;
							return true;
						}
					}
				}
			}
			finally
			{
				if (registryKey != null)
				{
					registryKey.Close();
				}
				if (registryKey2 != null)
				{
					registryKey2.Close();
				}
				PermissionSet.RevertAssert();
			}
			return false;
		}

		// Token: 0x06002F4F RID: 12111 RVA: 0x000D51D0 File Offset: 0x000D33D0
		internal static string[] GetCategories(string machineName)
		{
			PerformanceCounterLib performanceCounterLib;
			for (CultureInfo cultureInfo = CultureInfo.CurrentCulture; cultureInfo != CultureInfo.InvariantCulture; cultureInfo = cultureInfo.Parent)
			{
				performanceCounterLib = PerformanceCounterLib.GetPerformanceCounterLib(machineName, cultureInfo);
				string[] categories = performanceCounterLib.GetCategories();
				if (categories.Length != 0)
				{
					return categories;
				}
			}
			performanceCounterLib = PerformanceCounterLib.GetPerformanceCounterLib(machineName, new CultureInfo(9));
			return performanceCounterLib.GetCategories();
		}

		// Token: 0x06002F50 RID: 12112 RVA: 0x000D5220 File Offset: 0x000D3420
		internal string[] GetCategories()
		{
			ICollection keys = this.CategoryTable.Keys;
			string[] array = new string[keys.Count];
			keys.CopyTo(array, 0);
			return array;
		}

		// Token: 0x06002F51 RID: 12113 RVA: 0x000D5250 File Offset: 0x000D3450
		internal static string GetCategoryHelp(string machine, string category)
		{
			PerformanceCounterLib performanceCounterLib;
			string text;
			if (CultureInfo.CurrentCulture.Parent.LCID != 9)
			{
				for (CultureInfo cultureInfo = CultureInfo.CurrentCulture; cultureInfo != CultureInfo.InvariantCulture; cultureInfo = cultureInfo.Parent)
				{
					performanceCounterLib = PerformanceCounterLib.GetPerformanceCounterLib(machine, cultureInfo);
					text = performanceCounterLib.GetCategoryHelp(category);
					if (text != null)
					{
						return text;
					}
				}
			}
			performanceCounterLib = PerformanceCounterLib.GetPerformanceCounterLib(machine, new CultureInfo(9));
			text = performanceCounterLib.GetCategoryHelp(category);
			if (text == null)
			{
				throw new InvalidOperationException(SR.GetString("MissingCategory"));
			}
			return text;
		}

		// Token: 0x06002F52 RID: 12114 RVA: 0x000D52C8 File Offset: 0x000D34C8
		private string GetCategoryHelp(string category)
		{
			CategoryEntry categoryEntry = (CategoryEntry)this.CategoryTable[category];
			if (categoryEntry == null)
			{
				return null;
			}
			return (string)this.HelpTable[categoryEntry.HelpIndex];
		}

		// Token: 0x06002F53 RID: 12115 RVA: 0x000D5308 File Offset: 0x000D3508
		internal static CategorySample GetCategorySample(string machine, string category)
		{
			PerformanceCounterLib performanceCounterLib = PerformanceCounterLib.GetPerformanceCounterLib(machine, new CultureInfo(9));
			CategorySample categorySample = performanceCounterLib.GetCategorySample(category);
			if (categorySample == null && CultureInfo.CurrentCulture.Parent.LCID != 9)
			{
				for (CultureInfo cultureInfo = CultureInfo.CurrentCulture; cultureInfo != CultureInfo.InvariantCulture; cultureInfo = cultureInfo.Parent)
				{
					performanceCounterLib = PerformanceCounterLib.GetPerformanceCounterLib(machine, cultureInfo);
					categorySample = performanceCounterLib.GetCategorySample(category);
					if (categorySample != null)
					{
						return categorySample;
					}
				}
			}
			if (categorySample == null)
			{
				throw new InvalidOperationException(SR.GetString("MissingCategory"));
			}
			return categorySample;
		}

		// Token: 0x06002F54 RID: 12116 RVA: 0x000D5384 File Offset: 0x000D3584
		private CategorySample GetCategorySample(string category)
		{
			CategoryEntry categoryEntry = (CategoryEntry)this.CategoryTable[category];
			if (categoryEntry == null)
			{
				return null;
			}
			byte[] performanceData = this.GetPerformanceData(categoryEntry.NameIndex.ToString(CultureInfo.InvariantCulture));
			if (performanceData == null)
			{
				throw new InvalidOperationException(SR.GetString("CantReadCategory", new object[] { category }));
			}
			return new CategorySample(performanceData, categoryEntry, this);
		}

		// Token: 0x06002F55 RID: 12117 RVA: 0x000D53E8 File Offset: 0x000D35E8
		internal static string[] GetCounters(string machine, string category)
		{
			PerformanceCounterLib performanceCounterLib = PerformanceCounterLib.GetPerformanceCounterLib(machine, new CultureInfo(9));
			bool flag = false;
			string[] array = performanceCounterLib.GetCounters(category, ref flag);
			if (!flag && CultureInfo.CurrentCulture.Parent.LCID != 9)
			{
				for (CultureInfo cultureInfo = CultureInfo.CurrentCulture; cultureInfo != CultureInfo.InvariantCulture; cultureInfo = cultureInfo.Parent)
				{
					performanceCounterLib = PerformanceCounterLib.GetPerformanceCounterLib(machine, cultureInfo);
					array = performanceCounterLib.GetCounters(category, ref flag);
					if (flag)
					{
						return array;
					}
				}
			}
			if (!flag)
			{
				throw new InvalidOperationException(SR.GetString("MissingCategory"));
			}
			return array;
		}

		// Token: 0x06002F56 RID: 12118 RVA: 0x000D5468 File Offset: 0x000D3668
		private string[] GetCounters(string category, ref bool categoryExists)
		{
			categoryExists = false;
			CategoryEntry categoryEntry = (CategoryEntry)this.CategoryTable[category];
			if (categoryEntry == null)
			{
				return null;
			}
			categoryExists = true;
			int num = 0;
			string[] array = new string[categoryEntry.CounterIndexes.Length];
			for (int i = 0; i < array.Length; i++)
			{
				int num2 = categoryEntry.CounterIndexes[i];
				string text = (string)this.NameTable[num2];
				if (text != null && text != string.Empty)
				{
					array[num] = text;
					num++;
				}
			}
			if (num < array.Length)
			{
				string[] array2 = new string[num];
				Array.Copy(array, array2, num);
				array = array2;
			}
			return array;
		}

		// Token: 0x06002F57 RID: 12119 RVA: 0x000D550C File Offset: 0x000D370C
		internal static string GetCounterHelp(string machine, string category, string counter)
		{
			bool flag = false;
			PerformanceCounterLib performanceCounterLib;
			string text;
			if (CultureInfo.CurrentCulture.Parent.LCID != 9)
			{
				for (CultureInfo cultureInfo = CultureInfo.CurrentCulture; cultureInfo != CultureInfo.InvariantCulture; cultureInfo = cultureInfo.Parent)
				{
					performanceCounterLib = PerformanceCounterLib.GetPerformanceCounterLib(machine, cultureInfo);
					text = performanceCounterLib.GetCounterHelp(category, counter, ref flag);
					if (flag)
					{
						return text;
					}
				}
			}
			performanceCounterLib = PerformanceCounterLib.GetPerformanceCounterLib(machine, new CultureInfo(9));
			text = performanceCounterLib.GetCounterHelp(category, counter, ref flag);
			if (!flag)
			{
				throw new InvalidOperationException(SR.GetString("MissingCategoryDetail", new object[] { category }));
			}
			return text;
		}

		// Token: 0x06002F58 RID: 12120 RVA: 0x000D5594 File Offset: 0x000D3794
		private string GetCounterHelp(string category, string counter, ref bool categoryExists)
		{
			categoryExists = false;
			CategoryEntry categoryEntry = (CategoryEntry)this.CategoryTable[category];
			if (categoryEntry == null)
			{
				return null;
			}
			categoryExists = true;
			int num = -1;
			for (int i = 0; i < categoryEntry.CounterIndexes.Length; i++)
			{
				int num2 = categoryEntry.CounterIndexes[i];
				string text = (string)this.NameTable[num2];
				if (text == null)
				{
					text = string.Empty;
				}
				if (string.Compare(text, counter, StringComparison.OrdinalIgnoreCase) == 0)
				{
					num = categoryEntry.HelpIndexes[i];
					break;
				}
			}
			if (num == -1)
			{
				throw new InvalidOperationException(SR.GetString("MissingCounter", new object[] { counter }));
			}
			string text2 = (string)this.HelpTable[num];
			if (text2 == null)
			{
				return string.Empty;
			}
			return text2;
		}

		// Token: 0x06002F59 RID: 12121 RVA: 0x000D5656 File Offset: 0x000D3856
		internal string GetCounterName(int index)
		{
			if (this.NameTable.ContainsKey(index))
			{
				return (string)this.NameTable[index];
			}
			return "";
		}

		// Token: 0x06002F5A RID: 12122 RVA: 0x000D5688 File Offset: 0x000D3888
		private static string[] GetLanguageIds()
		{
			RegistryKey registryKey = null;
			string[] array = new string[0];
			new RegistryPermission(PermissionState.Unrestricted).Assert();
			try
			{
				registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Perflib");
				if (registryKey != null)
				{
					array = registryKey.GetSubKeyNames();
				}
			}
			finally
			{
				if (registryKey != null)
				{
					registryKey.Close();
				}
				CodeAccessPermission.RevertAssert();
			}
			return array;
		}

		// Token: 0x06002F5B RID: 12123 RVA: 0x000D56E8 File Offset: 0x000D38E8
		internal static PerformanceCounterLib GetPerformanceCounterLib(string machineName, CultureInfo culture)
		{
			SharedUtils.CheckEnvironment();
			string text = culture.LCID.ToString("X3", CultureInfo.InvariantCulture);
			if (machineName.CompareTo(".") == 0)
			{
				machineName = PerformanceCounterLib.ComputerName.ToLower(CultureInfo.InvariantCulture);
			}
			else
			{
				machineName = machineName.ToLower(CultureInfo.InvariantCulture);
			}
			if (PerformanceCounterLib.libraryTable == null)
			{
				object internalSyncObject = PerformanceCounterLib.InternalSyncObject;
				lock (internalSyncObject)
				{
					if (PerformanceCounterLib.libraryTable == null)
					{
						PerformanceCounterLib.libraryTable = new Hashtable();
					}
				}
			}
			string text2 = machineName + ":" + text;
			if (PerformanceCounterLib.libraryTable.Contains(text2))
			{
				return (PerformanceCounterLib)PerformanceCounterLib.libraryTable[text2];
			}
			PerformanceCounterLib performanceCounterLib = new PerformanceCounterLib(machineName, text);
			PerformanceCounterLib.libraryTable[text2] = performanceCounterLib;
			return performanceCounterLib;
		}

		// Token: 0x06002F5C RID: 12124 RVA: 0x000D57D8 File Offset: 0x000D39D8
		internal byte[] GetPerformanceData(string item)
		{
			if (this.performanceMonitor == null)
			{
				object internalSyncObject = PerformanceCounterLib.InternalSyncObject;
				lock (internalSyncObject)
				{
					if (this.performanceMonitor == null)
					{
						this.performanceMonitor = new PerformanceMonitor(this.machineName);
					}
				}
			}
			return this.performanceMonitor.GetData(item);
		}

		// Token: 0x06002F5D RID: 12125 RVA: 0x000D5840 File Offset: 0x000D3A40
		private Hashtable GetStringTable(bool isHelp)
		{
			PermissionSet permissionSet = new PermissionSet(PermissionState.None);
			permissionSet.AddPermission(new RegistryPermission(PermissionState.Unrestricted));
			permissionSet.AddPermission(new SecurityPermission(SecurityPermissionFlag.UnmanagedCode));
			permissionSet.Assert();
			RegistryKey registryKey;
			if (string.Compare(this.machineName, PerformanceCounterLib.ComputerName, StringComparison.OrdinalIgnoreCase) == 0)
			{
				registryKey = Registry.PerformanceData;
			}
			else
			{
				registryKey = RegistryKey.OpenRemoteBaseKey(RegistryHive.PerformanceData, this.machineName);
			}
			Hashtable hashtable;
			try
			{
				string[] array = null;
				int i = 14;
				int num = 0;
				while (i > 0)
				{
					try
					{
						if (!isHelp)
						{
							array = (string[])registryKey.GetValue("Counter " + this.perfLcid);
						}
						else
						{
							array = (string[])registryKey.GetValue("Explain " + this.perfLcid);
						}
						if (array != null && array.Length != 0)
						{
							break;
						}
						i--;
						if (num == 0)
						{
							num = 10;
						}
						else
						{
							Thread.Sleep(num);
							num *= 2;
						}
					}
					catch (IOException)
					{
						array = null;
						break;
					}
					catch (InvalidCastException)
					{
						array = null;
						break;
					}
				}
				if (array == null)
				{
					hashtable = new Hashtable();
				}
				else
				{
					hashtable = new Hashtable(array.Length / 2);
					int j = 0;
					while (j < array.Length / 2)
					{
						string text = array[j * 2 + 1];
						if (text == null)
						{
							text = string.Empty;
						}
						int num2;
						if (!int.TryParse(array[j * 2], NumberStyles.Integer, CultureInfo.InvariantCulture, out num2))
						{
							if (isHelp)
							{
								throw new InvalidOperationException(SR.GetString("CategoryHelpCorrupt", new object[] { array[j * 2] }));
							}
							throw new InvalidOperationException(SR.GetString("CounterNameCorrupt", new object[] { array[j * 2] }));
						}
						else
						{
							hashtable[num2] = text;
							j++;
						}
					}
				}
			}
			finally
			{
				registryKey.Close();
			}
			return hashtable;
		}

		// Token: 0x06002F5E RID: 12126 RVA: 0x000D5A28 File Offset: 0x000D3C28
		internal static bool IsCustomCategory(string machine, string category)
		{
			PerformanceCounterLib performanceCounterLib = PerformanceCounterLib.GetPerformanceCounterLib(machine, new CultureInfo(9));
			if (performanceCounterLib.IsCustomCategory(category))
			{
				return true;
			}
			if (CultureInfo.CurrentCulture.Parent.LCID != 9)
			{
				for (CultureInfo cultureInfo = CultureInfo.CurrentCulture; cultureInfo != CultureInfo.InvariantCulture; cultureInfo = cultureInfo.Parent)
				{
					performanceCounterLib = PerformanceCounterLib.GetPerformanceCounterLib(machine, cultureInfo);
					if (performanceCounterLib.IsCustomCategory(category))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06002F5F RID: 12127 RVA: 0x000D5A8C File Offset: 0x000D3C8C
		internal static bool IsBaseCounter(int type)
		{
			return type == 1073939458 || type == 1107494144 || type == 1073939459 || type == 1073939712 || type == 1073939457;
		}

		// Token: 0x06002F60 RID: 12128 RVA: 0x000D5AB8 File Offset: 0x000D3CB8
		private bool IsCustomCategory(string category)
		{
			PerformanceCounterCategoryType performanceCounterCategoryType;
			return this.FindCustomCategory(category, out performanceCounterCategoryType);
		}

		// Token: 0x06002F61 RID: 12129 RVA: 0x000D5AD0 File Offset: 0x000D3CD0
		internal static PerformanceCounterCategoryType GetCategoryType(string machine, string category)
		{
			PerformanceCounterCategoryType performanceCounterCategoryType = PerformanceCounterCategoryType.Unknown;
			PerformanceCounterLib performanceCounterLib = PerformanceCounterLib.GetPerformanceCounterLib(machine, new CultureInfo(9));
			if (!performanceCounterLib.FindCustomCategory(category, out performanceCounterCategoryType) && CultureInfo.CurrentCulture.Parent.LCID != 9)
			{
				for (CultureInfo cultureInfo = CultureInfo.CurrentCulture; cultureInfo != CultureInfo.InvariantCulture; cultureInfo = cultureInfo.Parent)
				{
					performanceCounterLib = PerformanceCounterLib.GetPerformanceCounterLib(machine, cultureInfo);
					if (performanceCounterLib.FindCustomCategory(category, out performanceCounterCategoryType))
					{
						return performanceCounterCategoryType;
					}
				}
			}
			return performanceCounterCategoryType;
		}

		// Token: 0x06002F62 RID: 12130 RVA: 0x000D5B38 File Offset: 0x000D3D38
		internal static void RegisterCategory(string categoryName, PerformanceCounterCategoryType categoryType, string categoryHelp, CounterCreationDataCollection creationData)
		{
			try
			{
				bool flag = false;
				PerformanceCounterLib.CreateRegistryEntry(categoryName, categoryType, creationData, ref flag);
				if (!flag)
				{
					string[] languageIds = PerformanceCounterLib.GetLanguageIds();
					PerformanceCounterLib.CreateIniFile(categoryName, categoryHelp, creationData, languageIds);
					PerformanceCounterLib.CreateSymbolFile(creationData);
					PerformanceCounterLib.RegisterFiles(PerformanceCounterLib.IniFilePath, false);
				}
				PerformanceCounterLib.CloseAllTables();
				PerformanceCounterLib.CloseAllLibraries();
			}
			finally
			{
				PerformanceCounterLib.DeleteTemporaryFiles();
			}
		}

		// Token: 0x06002F63 RID: 12131 RVA: 0x000D5B98 File Offset: 0x000D3D98
		private static void RegisterFiles(string arg0, bool unregister)
		{
			ProcessStartInfo processStartInfo = new ProcessStartInfo();
			processStartInfo.UseShellExecute = false;
			processStartInfo.CreateNoWindow = true;
			processStartInfo.ErrorDialog = false;
			processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
			processStartInfo.WorkingDirectory = Environment.SystemDirectory;
			if (unregister)
			{
				processStartInfo.FileName = Environment.SystemDirectory + "\\unlodctr.exe";
			}
			else
			{
				processStartInfo.FileName = Environment.SystemDirectory + "\\lodctr.exe";
			}
			int num = 0;
			new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Assert();
			try
			{
				processStartInfo.Arguments = "\"" + arg0 + "\"";
				Process process = Process.Start(processStartInfo);
				process.WaitForExit();
				num = process.ExitCode;
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			if (num == 5)
			{
				throw new UnauthorizedAccessException(SR.GetString("CantChangeCategoryRegistration", new object[] { arg0 }));
			}
			if (unregister && num == 2)
			{
				num = 0;
			}
			if (num != 0)
			{
				throw SharedUtils.CreateSafeWin32Exception(num);
			}
		}

		// Token: 0x06002F64 RID: 12132 RVA: 0x000D5C84 File Offset: 0x000D3E84
		internal static void UnregisterCategory(string categoryName)
		{
			PerformanceCounterLib.RegisterFiles(categoryName, true);
			PerformanceCounterLib.DeleteRegistryEntry(categoryName);
			PerformanceCounterLib.CloseAllTables();
			PerformanceCounterLib.CloseAllLibraries();
		}

		// Token: 0x040027BB RID: 10171
		internal const string PerfShimName = "netfxperf.dll";

		// Token: 0x040027BC RID: 10172
		private const string PerfShimFullNameSuffix = "\\netfxperf.dll";

		// Token: 0x040027BD RID: 10173
		private const string PerfShimPathExp = "%systemroot%\\system32\\netfxperf.dll";

		// Token: 0x040027BE RID: 10174
		internal const string OpenEntryPoint = "OpenPerformanceData";

		// Token: 0x040027BF RID: 10175
		internal const string CollectEntryPoint = "CollectPerformanceData";

		// Token: 0x040027C0 RID: 10176
		internal const string CloseEntryPoint = "ClosePerformanceData";

		// Token: 0x040027C1 RID: 10177
		internal const string SingleInstanceName = "systemdiagnosticsperfcounterlibsingleinstance";

		// Token: 0x040027C2 RID: 10178
		private const string PerflibPath = "SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Perflib";

		// Token: 0x040027C3 RID: 10179
		internal const string ServicePath = "SYSTEM\\CurrentControlSet\\Services";

		// Token: 0x040027C4 RID: 10180
		private const string categorySymbolPrefix = "OBJECT_";

		// Token: 0x040027C5 RID: 10181
		private const string conterSymbolPrefix = "DEVICE_COUNTER_";

		// Token: 0x040027C6 RID: 10182
		private const string helpSufix = "_HELP";

		// Token: 0x040027C7 RID: 10183
		private const string nameSufix = "_NAME";

		// Token: 0x040027C8 RID: 10184
		private const string textDefinition = "[text]";

		// Token: 0x040027C9 RID: 10185
		private const string infoDefinition = "[info]";

		// Token: 0x040027CA RID: 10186
		private const string languageDefinition = "[languages]";

		// Token: 0x040027CB RID: 10187
		private const string objectDefinition = "[objects]";

		// Token: 0x040027CC RID: 10188
		private const string driverNameKeyword = "drivername";

		// Token: 0x040027CD RID: 10189
		private const string symbolFileKeyword = "symbolfile";

		// Token: 0x040027CE RID: 10190
		private const string defineKeyword = "#define";

		// Token: 0x040027CF RID: 10191
		private const string languageKeyword = "language";

		// Token: 0x040027D0 RID: 10192
		private const string DllName = "netfxperf.dll";

		// Token: 0x040027D1 RID: 10193
		private const int EnglishLCID = 9;

		// Token: 0x040027D2 RID: 10194
		private static volatile string computerName;

		// Token: 0x040027D3 RID: 10195
		private static volatile string iniFilePath;

		// Token: 0x040027D4 RID: 10196
		private static volatile string symbolFilePath;

		// Token: 0x040027D5 RID: 10197
		private PerformanceMonitor performanceMonitor;

		// Token: 0x040027D6 RID: 10198
		private string machineName;

		// Token: 0x040027D7 RID: 10199
		private string perfLcid;

		// Token: 0x040027D8 RID: 10200
		private Hashtable customCategoryTable;

		// Token: 0x040027D9 RID: 10201
		private static volatile Hashtable libraryTable;

		// Token: 0x040027DA RID: 10202
		private Hashtable categoryTable;

		// Token: 0x040027DB RID: 10203
		private Hashtable nameTable;

		// Token: 0x040027DC RID: 10204
		private Hashtable helpTable;

		// Token: 0x040027DD RID: 10205
		private readonly object CategoryTableLock = new object();

		// Token: 0x040027DE RID: 10206
		private readonly object NameTableLock = new object();

		// Token: 0x040027DF RID: 10207
		private readonly object HelpTableLock = new object();

		// Token: 0x040027E0 RID: 10208
		private static object s_InternalSyncObject;
	}
}
