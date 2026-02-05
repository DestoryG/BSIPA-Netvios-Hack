using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Security.Permissions;
using System.Threading;

namespace System.Diagnostics
{
	// Token: 0x020004DF RID: 1247
	[HostProtection(SecurityAction.LinkDemand, Synchronization = true, SharedState = true)]
	public sealed class PerformanceCounterCategory
	{
		// Token: 0x06002F17 RID: 12055 RVA: 0x000D3418 File Offset: 0x000D1618
		public PerformanceCounterCategory()
		{
			this.machineName = ".";
		}

		// Token: 0x06002F18 RID: 12056 RVA: 0x000D342B File Offset: 0x000D162B
		public PerformanceCounterCategory(string categoryName)
			: this(categoryName, ".")
		{
		}

		// Token: 0x06002F19 RID: 12057 RVA: 0x000D343C File Offset: 0x000D163C
		public PerformanceCounterCategory(string categoryName, string machineName)
		{
			if (categoryName == null)
			{
				throw new ArgumentNullException("categoryName");
			}
			if (categoryName.Length == 0)
			{
				throw new ArgumentException(SR.GetString("InvalidParameter", new object[] { "categoryName", categoryName }));
			}
			if (!SyntaxCheck.CheckMachineName(machineName))
			{
				throw new ArgumentException(SR.GetString("InvalidParameter", new object[] { "machineName", machineName }));
			}
			PerformanceCounterPermission performanceCounterPermission = new PerformanceCounterPermission(PerformanceCounterPermissionAccess.Browse, machineName, categoryName);
			performanceCounterPermission.Demand();
			this.categoryName = categoryName;
			this.machineName = machineName;
		}

		// Token: 0x17000B79 RID: 2937
		// (get) Token: 0x06002F1A RID: 12058 RVA: 0x000D34CE File Offset: 0x000D16CE
		// (set) Token: 0x06002F1B RID: 12059 RVA: 0x000D34D8 File Offset: 0x000D16D8
		public string CategoryName
		{
			get
			{
				return this.categoryName;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (value.Length == 0)
				{
					throw new ArgumentException(SR.GetString("InvalidProperty", new object[] { "CategoryName", value }));
				}
				lock (this)
				{
					PerformanceCounterPermission performanceCounterPermission = new PerformanceCounterPermission(PerformanceCounterPermissionAccess.Browse, this.machineName, value);
					performanceCounterPermission.Demand();
					this.categoryName = value;
				}
			}
		}

		// Token: 0x17000B7A RID: 2938
		// (get) Token: 0x06002F1C RID: 12060 RVA: 0x000D3560 File Offset: 0x000D1760
		public string CategoryHelp
		{
			get
			{
				if (this.categoryName == null)
				{
					throw new InvalidOperationException(SR.GetString("CategoryNameNotSet"));
				}
				if (this.categoryHelp == null)
				{
					this.categoryHelp = PerformanceCounterLib.GetCategoryHelp(this.machineName, this.categoryName);
				}
				return this.categoryHelp;
			}
		}

		// Token: 0x17000B7B RID: 2939
		// (get) Token: 0x06002F1D RID: 12061 RVA: 0x000D35A0 File Offset: 0x000D17A0
		public PerformanceCounterCategoryType CategoryType
		{
			get
			{
				CategorySample categorySample = PerformanceCounterLib.GetCategorySample(this.machineName, this.categoryName);
				if (categorySample.IsMultiInstance)
				{
					return PerformanceCounterCategoryType.MultiInstance;
				}
				if (PerformanceCounterLib.IsCustomCategory(".", this.categoryName))
				{
					return PerformanceCounterLib.GetCategoryType(".", this.categoryName);
				}
				return PerformanceCounterCategoryType.SingleInstance;
			}
		}

		// Token: 0x17000B7C RID: 2940
		// (get) Token: 0x06002F1E RID: 12062 RVA: 0x000D35ED File Offset: 0x000D17ED
		// (set) Token: 0x06002F1F RID: 12063 RVA: 0x000D35F8 File Offset: 0x000D17F8
		public string MachineName
		{
			get
			{
				return this.machineName;
			}
			set
			{
				if (!SyntaxCheck.CheckMachineName(value))
				{
					throw new ArgumentException(SR.GetString("InvalidProperty", new object[] { "MachineName", value }));
				}
				lock (this)
				{
					if (this.categoryName != null)
					{
						PerformanceCounterPermission performanceCounterPermission = new PerformanceCounterPermission(PerformanceCounterPermissionAccess.Browse, value, this.categoryName);
						performanceCounterPermission.Demand();
					}
					this.machineName = value;
				}
			}
		}

		// Token: 0x06002F20 RID: 12064 RVA: 0x000D367C File Offset: 0x000D187C
		public bool CounterExists(string counterName)
		{
			if (counterName == null)
			{
				throw new ArgumentNullException("counterName");
			}
			if (this.categoryName == null)
			{
				throw new InvalidOperationException(SR.GetString("CategoryNameNotSet"));
			}
			return PerformanceCounterLib.CounterExists(this.machineName, this.categoryName, counterName);
		}

		// Token: 0x06002F21 RID: 12065 RVA: 0x000D36B6 File Offset: 0x000D18B6
		public static bool CounterExists(string counterName, string categoryName)
		{
			return PerformanceCounterCategory.CounterExists(counterName, categoryName, ".");
		}

		// Token: 0x06002F22 RID: 12066 RVA: 0x000D36C4 File Offset: 0x000D18C4
		public static bool CounterExists(string counterName, string categoryName, string machineName)
		{
			if (counterName == null)
			{
				throw new ArgumentNullException("counterName");
			}
			if (categoryName == null)
			{
				throw new ArgumentNullException("categoryName");
			}
			if (categoryName.Length == 0)
			{
				throw new ArgumentException(SR.GetString("InvalidParameter", new object[] { "categoryName", categoryName }));
			}
			if (!SyntaxCheck.CheckMachineName(machineName))
			{
				throw new ArgumentException(SR.GetString("InvalidParameter", new object[] { "machineName", machineName }));
			}
			PerformanceCounterPermission performanceCounterPermission = new PerformanceCounterPermission(PerformanceCounterPermissionAccess.Browse, machineName, categoryName);
			performanceCounterPermission.Demand();
			return PerformanceCounterLib.CounterExists(machineName, categoryName, counterName);
		}

		// Token: 0x06002F23 RID: 12067 RVA: 0x000D3758 File Offset: 0x000D1958
		[Obsolete("This method has been deprecated.  Please use System.Diagnostics.PerformanceCounterCategory.Create(string categoryName, string categoryHelp, PerformanceCounterCategoryType categoryType, string counterName, string counterHelp) instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		public static PerformanceCounterCategory Create(string categoryName, string categoryHelp, string counterName, string counterHelp)
		{
			CounterCreationData counterCreationData = new CounterCreationData(counterName, counterHelp, PerformanceCounterType.NumberOfItems32);
			return PerformanceCounterCategory.Create(categoryName, categoryHelp, PerformanceCounterCategoryType.Unknown, new CounterCreationDataCollection(new CounterCreationData[] { counterCreationData }));
		}

		// Token: 0x06002F24 RID: 12068 RVA: 0x000D378C File Offset: 0x000D198C
		public static PerformanceCounterCategory Create(string categoryName, string categoryHelp, PerformanceCounterCategoryType categoryType, string counterName, string counterHelp)
		{
			CounterCreationData counterCreationData = new CounterCreationData(counterName, counterHelp, PerformanceCounterType.NumberOfItems32);
			return PerformanceCounterCategory.Create(categoryName, categoryHelp, categoryType, new CounterCreationDataCollection(new CounterCreationData[] { counterCreationData }));
		}

		// Token: 0x06002F25 RID: 12069 RVA: 0x000D37BE File Offset: 0x000D19BE
		[Obsolete("This method has been deprecated.  Please use System.Diagnostics.PerformanceCounterCategory.Create(string categoryName, string categoryHelp, PerformanceCounterCategoryType categoryType, CounterCreationDataCollection counterData) instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		public static PerformanceCounterCategory Create(string categoryName, string categoryHelp, CounterCreationDataCollection counterData)
		{
			return PerformanceCounterCategory.Create(categoryName, categoryHelp, PerformanceCounterCategoryType.Unknown, counterData);
		}

		// Token: 0x06002F26 RID: 12070 RVA: 0x000D37CC File Offset: 0x000D19CC
		public static PerformanceCounterCategory Create(string categoryName, string categoryHelp, PerformanceCounterCategoryType categoryType, CounterCreationDataCollection counterData)
		{
			if (categoryType < PerformanceCounterCategoryType.Unknown || categoryType > PerformanceCounterCategoryType.MultiInstance)
			{
				throw new ArgumentOutOfRangeException("categoryType");
			}
			if (counterData == null)
			{
				throw new ArgumentNullException("counterData");
			}
			PerformanceCounterCategory.CheckValidCategory(categoryName);
			if (categoryHelp != null)
			{
				PerformanceCounterCategory.CheckValidHelp(categoryHelp);
			}
			string text = ".";
			PerformanceCounterPermission performanceCounterPermission = new PerformanceCounterPermission(PerformanceCounterPermissionAccess.Administer, text, categoryName);
			performanceCounterPermission.Demand();
			SharedUtils.CheckNtEnvironment();
			Mutex mutex = null;
			RuntimeHelpers.PrepareConstrainedRegions();
			PerformanceCounterCategory performanceCounterCategory;
			try
			{
				SharedUtils.EnterMutex("netfxperf.1.0", ref mutex);
				if (PerformanceCounterLib.IsCustomCategory(text, categoryName) || PerformanceCounterLib.CategoryExists(text, categoryName))
				{
					throw new InvalidOperationException(SR.GetString("PerformanceCategoryExists", new object[] { categoryName }));
				}
				PerformanceCounterCategory.CheckValidCounterLayout(counterData);
				PerformanceCounterLib.RegisterCategory(categoryName, categoryType, categoryHelp, counterData);
				performanceCounterCategory = new PerformanceCounterCategory(categoryName, text);
			}
			finally
			{
				if (mutex != null)
				{
					mutex.ReleaseMutex();
					mutex.Close();
				}
			}
			return performanceCounterCategory;
		}

		// Token: 0x06002F27 RID: 12071 RVA: 0x000D389C File Offset: 0x000D1A9C
		internal static void CheckValidCategory(string categoryName)
		{
			if (categoryName == null)
			{
				throw new ArgumentNullException("categoryName");
			}
			if (!PerformanceCounterCategory.CheckValidId(categoryName, 80))
			{
				throw new ArgumentException(SR.GetString("PerfInvalidCategoryName", new object[] { 1, 80 }));
			}
			if (categoryName.Length > 1024 - "netfxcustomperfcounters.1.0".Length)
			{
				throw new ArgumentException(SR.GetString("CategoryNameTooLong"));
			}
		}

		// Token: 0x06002F28 RID: 12072 RVA: 0x000D3914 File Offset: 0x000D1B14
		internal static void CheckValidCounter(string counterName)
		{
			if (counterName == null)
			{
				throw new ArgumentNullException("counterName");
			}
			if (!PerformanceCounterCategory.CheckValidId(counterName, 32767))
			{
				throw new ArgumentException(SR.GetString("PerfInvalidCounterName", new object[] { 1, 32767 }));
			}
		}

		// Token: 0x06002F29 RID: 12073 RVA: 0x000D3968 File Offset: 0x000D1B68
		internal static bool CheckValidId(string id, int maxLength)
		{
			if (id.Length == 0 || id.Length > maxLength)
			{
				return false;
			}
			for (int i = 0; i < id.Length; i++)
			{
				char c = id[i];
				if ((i == 0 || i == id.Length - 1) && c == ' ')
				{
					return false;
				}
				if (c == '"')
				{
					return false;
				}
				if (char.IsControl(c))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06002F2A RID: 12074 RVA: 0x000D39C8 File Offset: 0x000D1BC8
		internal static void CheckValidHelp(string help)
		{
			if (help == null)
			{
				throw new ArgumentNullException("help");
			}
			if (help.Length > 32767)
			{
				throw new ArgumentException(SR.GetString("PerfInvalidHelp", new object[] { 0, 32767 }));
			}
		}

		// Token: 0x06002F2B RID: 12075 RVA: 0x000D3A1C File Offset: 0x000D1C1C
		internal static void CheckValidCounterLayout(CounterCreationDataCollection counterData)
		{
			Hashtable hashtable = new Hashtable();
			for (int i = 0; i < counterData.Count; i++)
			{
				if (counterData[i].CounterName == null || counterData[i].CounterName.Length == 0)
				{
					throw new ArgumentException(SR.GetString("InvalidCounterName"));
				}
				int num = (int)counterData[i].CounterType;
				if (num == 1073874176 || num == 575735040 || num == 592512256 || num == 574686464 || num == 591463680 || num == 537003008 || num == 549585920 || num == 805438464)
				{
					if (counterData.Count <= i + 1)
					{
						throw new InvalidOperationException(SR.GetString("CounterLayout"));
					}
					num = (int)counterData[i + 1].CounterType;
					if (!PerformanceCounterLib.IsBaseCounter(num))
					{
						throw new InvalidOperationException(SR.GetString("CounterLayout"));
					}
				}
				else if (PerformanceCounterLib.IsBaseCounter(num))
				{
					if (i == 0)
					{
						throw new InvalidOperationException(SR.GetString("CounterLayout"));
					}
					num = (int)counterData[i - 1].CounterType;
					if (num != 1073874176 && num != 575735040 && num != 592512256 && num != 574686464 && num != 591463680 && num != 537003008 && num != 549585920 && num != 805438464)
					{
						throw new InvalidOperationException(SR.GetString("CounterLayout"));
					}
				}
				if (hashtable.ContainsKey(counterData[i].CounterName))
				{
					throw new ArgumentException(SR.GetString("DuplicateCounterName", new object[] { counterData[i].CounterName }));
				}
				hashtable.Add(counterData[i].CounterName, string.Empty);
				if (counterData[i].CounterHelp == null || counterData[i].CounterHelp.Length == 0)
				{
					counterData[i].CounterHelp = counterData[i].CounterName;
				}
			}
		}

		// Token: 0x06002F2C RID: 12076 RVA: 0x000D3C0C File Offset: 0x000D1E0C
		public static void Delete(string categoryName)
		{
			PerformanceCounterCategory.CheckValidCategory(categoryName);
			string text = ".";
			PerformanceCounterPermission performanceCounterPermission = new PerformanceCounterPermission(PerformanceCounterPermissionAccess.Administer, text, categoryName);
			performanceCounterPermission.Demand();
			SharedUtils.CheckNtEnvironment();
			categoryName = categoryName.ToLower(CultureInfo.InvariantCulture);
			Mutex mutex = null;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				SharedUtils.EnterMutex("netfxperf.1.0", ref mutex);
				if (!PerformanceCounterLib.IsCustomCategory(text, categoryName))
				{
					throw new InvalidOperationException(SR.GetString("CantDeleteCategory"));
				}
				SharedPerformanceCounter.RemoveAllInstances(categoryName);
				PerformanceCounterLib.UnregisterCategory(categoryName);
				PerformanceCounterLib.CloseAllLibraries();
			}
			finally
			{
				if (mutex != null)
				{
					mutex.ReleaseMutex();
					mutex.Close();
				}
			}
		}

		// Token: 0x06002F2D RID: 12077 RVA: 0x000D3CA8 File Offset: 0x000D1EA8
		public static bool Exists(string categoryName)
		{
			return PerformanceCounterCategory.Exists(categoryName, ".");
		}

		// Token: 0x06002F2E RID: 12078 RVA: 0x000D3CB8 File Offset: 0x000D1EB8
		public static bool Exists(string categoryName, string machineName)
		{
			if (categoryName == null)
			{
				throw new ArgumentNullException("categoryName");
			}
			if (categoryName.Length == 0)
			{
				throw new ArgumentException(SR.GetString("InvalidParameter", new object[] { "categoryName", categoryName }));
			}
			if (!SyntaxCheck.CheckMachineName(machineName))
			{
				throw new ArgumentException(SR.GetString("InvalidParameter", new object[] { "machineName", machineName }));
			}
			PerformanceCounterPermission performanceCounterPermission = new PerformanceCounterPermission(PerformanceCounterPermissionAccess.Browse, machineName, categoryName);
			performanceCounterPermission.Demand();
			return PerformanceCounterLib.IsCustomCategory(machineName, categoryName) || PerformanceCounterLib.CategoryExists(machineName, categoryName);
		}

		// Token: 0x06002F2F RID: 12079 RVA: 0x000D3D48 File Offset: 0x000D1F48
		internal static string[] GetCounterInstances(string categoryName, string machineName)
		{
			PerformanceCounterPermission performanceCounterPermission = new PerformanceCounterPermission(PerformanceCounterPermissionAccess.Browse, machineName, categoryName);
			performanceCounterPermission.Demand();
			CategorySample categorySample = PerformanceCounterLib.GetCategorySample(machineName, categoryName);
			if (categorySample.InstanceNameTable.Count == 0)
			{
				return new string[0];
			}
			string[] array = new string[categorySample.InstanceNameTable.Count];
			categorySample.InstanceNameTable.Keys.CopyTo(array, 0);
			if (array.Length == 1 && array[0].CompareTo("systemdiagnosticsperfcounterlibsingleinstance") == 0)
			{
				return new string[0];
			}
			return array;
		}

		// Token: 0x06002F30 RID: 12080 RVA: 0x000D3DC0 File Offset: 0x000D1FC0
		public PerformanceCounter[] GetCounters()
		{
			if (this.GetInstanceNames().Length != 0)
			{
				throw new ArgumentException(SR.GetString("InstanceNameRequired"));
			}
			return this.GetCounters("");
		}

		// Token: 0x06002F31 RID: 12081 RVA: 0x000D3DE8 File Offset: 0x000D1FE8
		public PerformanceCounter[] GetCounters(string instanceName)
		{
			if (instanceName == null)
			{
				throw new ArgumentNullException("instanceName");
			}
			if (this.categoryName == null)
			{
				throw new InvalidOperationException(SR.GetString("CategoryNameNotSet"));
			}
			if (instanceName.Length != 0 && !this.InstanceExists(instanceName))
			{
				throw new InvalidOperationException(SR.GetString("MissingInstance", new object[] { instanceName, this.categoryName }));
			}
			string[] counters = PerformanceCounterLib.GetCounters(this.machineName, this.categoryName);
			PerformanceCounter[] array = new PerformanceCounter[counters.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new PerformanceCounter(this.categoryName, counters[i], instanceName, this.machineName, true);
			}
			return array;
		}

		// Token: 0x06002F32 RID: 12082 RVA: 0x000D3E92 File Offset: 0x000D2092
		public static PerformanceCounterCategory[] GetCategories()
		{
			return PerformanceCounterCategory.GetCategories(".");
		}

		// Token: 0x06002F33 RID: 12083 RVA: 0x000D3EA0 File Offset: 0x000D20A0
		public static PerformanceCounterCategory[] GetCategories(string machineName)
		{
			if (!SyntaxCheck.CheckMachineName(machineName))
			{
				throw new ArgumentException(SR.GetString("InvalidParameter", new object[] { "machineName", machineName }));
			}
			PerformanceCounterPermission performanceCounterPermission = new PerformanceCounterPermission(PerformanceCounterPermissionAccess.Browse, machineName, "*");
			performanceCounterPermission.Demand();
			string[] categories = PerformanceCounterLib.GetCategories(machineName);
			PerformanceCounterCategory[] array = new PerformanceCounterCategory[categories.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new PerformanceCounterCategory(categories[i], machineName);
			}
			return array;
		}

		// Token: 0x06002F34 RID: 12084 RVA: 0x000D3F15 File Offset: 0x000D2115
		public string[] GetInstanceNames()
		{
			if (this.categoryName == null)
			{
				throw new InvalidOperationException(SR.GetString("CategoryNameNotSet"));
			}
			return PerformanceCounterCategory.GetCounterInstances(this.categoryName, this.machineName);
		}

		// Token: 0x06002F35 RID: 12085 RVA: 0x000D3F40 File Offset: 0x000D2140
		public bool InstanceExists(string instanceName)
		{
			if (instanceName == null)
			{
				throw new ArgumentNullException("instanceName");
			}
			if (this.categoryName == null)
			{
				throw new InvalidOperationException(SR.GetString("CategoryNameNotSet"));
			}
			CategorySample categorySample = PerformanceCounterLib.GetCategorySample(this.machineName, this.categoryName);
			return categorySample.InstanceNameTable.ContainsKey(instanceName);
		}

		// Token: 0x06002F36 RID: 12086 RVA: 0x000D3F91 File Offset: 0x000D2191
		public static bool InstanceExists(string instanceName, string categoryName)
		{
			return PerformanceCounterCategory.InstanceExists(instanceName, categoryName, ".");
		}

		// Token: 0x06002F37 RID: 12087 RVA: 0x000D3FA0 File Offset: 0x000D21A0
		public static bool InstanceExists(string instanceName, string categoryName, string machineName)
		{
			if (instanceName == null)
			{
				throw new ArgumentNullException("instanceName");
			}
			if (categoryName == null)
			{
				throw new ArgumentNullException("categoryName");
			}
			if (categoryName.Length == 0)
			{
				throw new ArgumentException(SR.GetString("InvalidParameter", new object[] { "categoryName", categoryName }));
			}
			if (!SyntaxCheck.CheckMachineName(machineName))
			{
				throw new ArgumentException(SR.GetString("InvalidParameter", new object[] { "machineName", machineName }));
			}
			PerformanceCounterCategory performanceCounterCategory = new PerformanceCounterCategory(categoryName, machineName);
			return performanceCounterCategory.InstanceExists(instanceName);
		}

		// Token: 0x06002F38 RID: 12088 RVA: 0x000D402C File Offset: 0x000D222C
		public InstanceDataCollectionCollection ReadCategory()
		{
			if (this.categoryName == null)
			{
				throw new InvalidOperationException(SR.GetString("CategoryNameNotSet"));
			}
			CategorySample categorySample = PerformanceCounterLib.GetCategorySample(this.machineName, this.categoryName);
			return categorySample.ReadCategory();
		}

		// Token: 0x040027AA RID: 10154
		private string categoryName;

		// Token: 0x040027AB RID: 10155
		private string categoryHelp;

		// Token: 0x040027AC RID: 10156
		private string machineName;

		// Token: 0x040027AD RID: 10157
		internal const int MaxCategoryNameLength = 80;

		// Token: 0x040027AE RID: 10158
		internal const int MaxCounterNameLength = 32767;

		// Token: 0x040027AF RID: 10159
		internal const int MaxHelpLength = 32767;

		// Token: 0x040027B0 RID: 10160
		private const string perfMutexName = "netfxperf.1.0";
	}
}
