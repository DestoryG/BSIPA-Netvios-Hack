using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;

namespace System.Diagnostics
{
	// Token: 0x02000504 RID: 1284
	[HostProtection(SecurityAction.LinkDemand, Synchronization = true, SharedState = true)]
	internal sealed class SharedPerformanceCounter
	{
		// Token: 0x17000BF5 RID: 3061
		// (get) Token: 0x060030B0 RID: 12464 RVA: 0x000DBF18 File Offset: 0x000DA118
		private static ProcessData ProcessData
		{
			get
			{
				if (SharedPerformanceCounter.procData == null)
				{
					new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Assert();
					try
					{
						int currentProcessId = Microsoft.Win32.NativeMethods.GetCurrentProcessId();
						long num = -1L;
						using (Microsoft.Win32.SafeHandles.SafeProcessHandle safeProcessHandle = Microsoft.Win32.SafeHandles.SafeProcessHandle.OpenProcess(1024, false, currentProcessId))
						{
							if (!safeProcessHandle.IsInvalid)
							{
								long num2;
								Microsoft.Win32.NativeMethods.GetProcessTimes(safeProcessHandle, out num, out num2, out num2, out num2);
							}
						}
						SharedPerformanceCounter.procData = new ProcessData(currentProcessId, num);
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
				}
				return SharedPerformanceCounter.procData;
			}
		}

		// Token: 0x060030B1 RID: 12465 RVA: 0x000DBFAC File Offset: 0x000DA1AC
		internal SharedPerformanceCounter(string catName, string counterName, string instanceName)
			: this(catName, counterName, instanceName, PerformanceCounterInstanceLifetime.Global)
		{
		}

		// Token: 0x060030B2 RID: 12466 RVA: 0x000DBFB8 File Offset: 0x000DA1B8
		internal SharedPerformanceCounter(string catName, string counterName, string instanceName, PerformanceCounterInstanceLifetime lifetime)
		{
			this.categoryName = catName;
			this.categoryNameHashCode = SharedPerformanceCounter.GetWstrHashCode(this.categoryName);
			this.categoryData = this.GetCategoryData();
			if (this.categoryData.UseUniqueSharedMemory)
			{
				if (instanceName != null && instanceName.Length > 127)
				{
					throw new InvalidOperationException(SR.GetString("InstanceNameTooLong"));
				}
			}
			else if (lifetime != PerformanceCounterInstanceLifetime.Global)
			{
				throw new InvalidOperationException(SR.GetString("ProcessLifetimeNotValidInGlobal"));
			}
			if (counterName != null && instanceName != null && this.categoryData.CounterNames.Contains(counterName))
			{
				this.counterEntryPointer = this.GetCounter(counterName, instanceName, this.categoryData.EnableReuse, lifetime);
			}
		}

		// Token: 0x17000BF6 RID: 3062
		// (get) Token: 0x060030B3 RID: 12467 RVA: 0x000DC06F File Offset: 0x000DA26F
		private SharedPerformanceCounter.FileMapping FileView
		{
			get
			{
				return this.categoryData.FileMapping;
			}
		}

		// Token: 0x17000BF7 RID: 3063
		// (get) Token: 0x060030B4 RID: 12468 RVA: 0x000DC07C File Offset: 0x000DA27C
		// (set) Token: 0x060030B5 RID: 12469 RVA: 0x000DC096 File Offset: 0x000DA296
		internal long Value
		{
			get
			{
				if (this.counterEntryPointer == null)
				{
					return 0L;
				}
				return SharedPerformanceCounter.GetValue(this.counterEntryPointer);
			}
			set
			{
				if (this.counterEntryPointer == null)
				{
					return;
				}
				SharedPerformanceCounter.SetValue(this.counterEntryPointer, value);
			}
		}

		// Token: 0x060030B6 RID: 12470 RVA: 0x000DC0B0 File Offset: 0x000DA2B0
		private unsafe int CalculateAndAllocateMemory(int totalSize, out int alignmentAdjustment)
		{
			alignmentAdjustment = 0;
			int num;
			int num2;
			do
			{
				num = *(UIntPtr)this.baseAddress;
				this.ResolveOffset(num, 0);
				num2 = this.CalculateMemory(num, totalSize, out alignmentAdjustment);
				int num3 = (int)(this.baseAddress + (long)num2) & 7;
				int num4 = (8 - num3) & 7;
				num2 += num4;
			}
			while (SafeNativeMethods.InterlockedCompareExchange((IntPtr)this.baseAddress, num2, num) != num);
			return num;
		}

		// Token: 0x060030B7 RID: 12471 RVA: 0x000DC10C File Offset: 0x000DA30C
		private int CalculateMemory(int oldOffset, int totalSize, out int alignmentAdjustment)
		{
			int num = this.CalculateMemoryNoBoundsCheck(oldOffset, totalSize, out alignmentAdjustment);
			if (num > this.FileView.FileMappingSize || num < 0)
			{
				throw new InvalidOperationException(SR.GetString("CountersOOM"));
			}
			return num;
		}

		// Token: 0x060030B8 RID: 12472 RVA: 0x000DC148 File Offset: 0x000DA348
		private int CalculateMemoryNoBoundsCheck(int oldOffset, int totalSize, out int alignmentAdjustment)
		{
			Thread.MemoryBarrier();
			int num = (int)(this.baseAddress + (long)oldOffset) & 7;
			alignmentAdjustment = (8 - num) & 7;
			int num2 = totalSize + alignmentAdjustment;
			return oldOffset + num2;
		}

		// Token: 0x060030B9 RID: 12473 RVA: 0x000DC17C File Offset: 0x000DA37C
		private unsafe int CreateCategory(SharedPerformanceCounter.CategoryEntry* lastCategoryPointer, int instanceNameHashCode, string instanceName, PerformanceCounterInstanceLifetime lifetime)
		{
			int num = 0;
			int num2 = (this.categoryName.Length + 1) * 2;
			int num3 = SharedPerformanceCounter.CategoryEntrySize + SharedPerformanceCounter.InstanceEntrySize + SharedPerformanceCounter.CounterEntrySize * this.categoryData.CounterNames.Count + num2;
			for (int i = 0; i < this.categoryData.CounterNames.Count; i++)
			{
				num3 += (((string)this.categoryData.CounterNames[i]).Length + 1) * 2;
			}
			int num4;
			int num5;
			int num6;
			if (this.categoryData.UseUniqueSharedMemory)
			{
				num4 = 256;
				num3 += SharedPerformanceCounter.ProcessLifetimeEntrySize + num4;
				num5 = *(UIntPtr)this.baseAddress;
				num = this.CalculateMemory(num5, num3, out num6);
				if (num5 == this.InitialOffset)
				{
					lastCategoryPointer->IsConsistent = 0;
				}
			}
			else
			{
				num4 = (instanceName.Length + 1) * 2;
				num3 += num4;
				num5 = this.CalculateAndAllocateMemory(num3, out num6);
			}
			long num7 = this.ResolveOffset(num5, num3 + num6);
			SharedPerformanceCounter.CategoryEntry* ptr;
			SharedPerformanceCounter.InstanceEntry* ptr2;
			if (num5 == this.InitialOffset)
			{
				ptr = num7;
				num7 += (long)(SharedPerformanceCounter.CategoryEntrySize + num6);
				ptr2 = num7;
			}
			else
			{
				num7 += (long)num6;
				ptr = num7;
				num7 += (long)SharedPerformanceCounter.CategoryEntrySize;
				ptr2 = num7;
			}
			num7 += (long)SharedPerformanceCounter.InstanceEntrySize;
			SharedPerformanceCounter.CounterEntry* ptr3 = num7;
			num7 += (long)(SharedPerformanceCounter.CounterEntrySize * this.categoryData.CounterNames.Count);
			if (this.categoryData.UseUniqueSharedMemory)
			{
				SharedPerformanceCounter.ProcessLifetimeEntry* ptr4 = num7;
				num7 += (long)SharedPerformanceCounter.ProcessLifetimeEntrySize;
				ptr3->LifetimeOffset = ptr4 - this.baseAddress / (long)sizeof(SharedPerformanceCounter.ProcessLifetimeEntry);
				SharedPerformanceCounter.PopulateLifetimeEntry(ptr4, lifetime);
			}
			ptr->CategoryNameHashCode = this.categoryNameHashCode;
			ptr->NextCategoryOffset = 0;
			ptr->FirstInstanceOffset = ptr2 - this.baseAddress / (long)sizeof(SharedPerformanceCounter.InstanceEntry);
			ptr->CategoryNameOffset = (int)(num7 - this.baseAddress);
			SharedPerformanceCounter.SafeMarshalCopy(this.categoryName, (IntPtr)num7);
			num7 += (long)num2;
			ptr2->InstanceNameHashCode = instanceNameHashCode;
			ptr2->NextInstanceOffset = 0;
			ptr2->FirstCounterOffset = ptr3 - this.baseAddress / (long)sizeof(SharedPerformanceCounter.CounterEntry);
			ptr2->RefCount = 1;
			ptr2->InstanceNameOffset = (int)(num7 - this.baseAddress);
			SharedPerformanceCounter.SafeMarshalCopy(instanceName, (IntPtr)num7);
			num7 += (long)num4;
			string text = (string)this.categoryData.CounterNames[0];
			ptr3->CounterNameHashCode = SharedPerformanceCounter.GetWstrHashCode(text);
			SharedPerformanceCounter.SetValue(ptr3, 0L);
			ptr3->CounterNameOffset = (int)(num7 - this.baseAddress);
			SharedPerformanceCounter.SafeMarshalCopy(text, (IntPtr)num7);
			num7 += (long)((text.Length + 1) * 2);
			for (int j = 1; j < this.categoryData.CounterNames.Count; j++)
			{
				SharedPerformanceCounter.CounterEntry* ptr5 = ptr3;
				text = (string)this.categoryData.CounterNames[j];
				ptr3++;
				ptr3->CounterNameHashCode = SharedPerformanceCounter.GetWstrHashCode(text);
				SharedPerformanceCounter.SetValue(ptr3, 0L);
				ptr3->CounterNameOffset = (int)(num7 - this.baseAddress);
				SharedPerformanceCounter.SafeMarshalCopy(text, (IntPtr)num7);
				num7 += (long)((text.Length + 1) * 2);
				ptr5->NextCounterOffset = ptr3 - this.baseAddress / (long)sizeof(SharedPerformanceCounter.CounterEntry);
			}
			int num8 = ptr - this.baseAddress / (long)sizeof(SharedPerformanceCounter.CategoryEntry);
			lastCategoryPointer->IsConsistent = 0;
			if (num8 != this.InitialOffset)
			{
				lastCategoryPointer->NextCategoryOffset = num8;
			}
			if (this.categoryData.UseUniqueSharedMemory)
			{
				*(UIntPtr)this.baseAddress = num;
				lastCategoryPointer->IsConsistent = 1;
			}
			return num8;
		}

		// Token: 0x060030BA RID: 12474 RVA: 0x000DC504 File Offset: 0x000DA704
		private unsafe int CreateInstance(SharedPerformanceCounter.CategoryEntry* categoryPointer, int instanceNameHashCode, string instanceName, PerformanceCounterInstanceLifetime lifetime)
		{
			int num = SharedPerformanceCounter.InstanceEntrySize + SharedPerformanceCounter.CounterEntrySize * this.categoryData.CounterNames.Count;
			int num2 = 0;
			int num3;
			int num4;
			int num5;
			if (this.categoryData.UseUniqueSharedMemory)
			{
				num3 = 256;
				num += SharedPerformanceCounter.ProcessLifetimeEntrySize + num3;
				num4 = *(UIntPtr)this.baseAddress;
				num2 = this.CalculateMemory(num4, num, out num5);
			}
			else
			{
				num3 = (instanceName.Length + 1) * 2;
				num += num3;
				for (int i = 0; i < this.categoryData.CounterNames.Count; i++)
				{
					num += (((string)this.categoryData.CounterNames[i]).Length + 1) * 2;
				}
				num4 = this.CalculateAndAllocateMemory(num, out num5);
			}
			num4 += num5;
			long num6 = this.ResolveOffset(num4, num);
			SharedPerformanceCounter.InstanceEntry* ptr = num6;
			num6 += (long)SharedPerformanceCounter.InstanceEntrySize;
			SharedPerformanceCounter.CounterEntry* ptr2 = num6;
			num6 += (long)(SharedPerformanceCounter.CounterEntrySize * this.categoryData.CounterNames.Count);
			if (this.categoryData.UseUniqueSharedMemory)
			{
				SharedPerformanceCounter.ProcessLifetimeEntry* ptr3 = num6;
				num6 += (long)SharedPerformanceCounter.ProcessLifetimeEntrySize;
				ptr2->LifetimeOffset = ptr3 - this.baseAddress / (long)sizeof(SharedPerformanceCounter.ProcessLifetimeEntry);
				SharedPerformanceCounter.PopulateLifetimeEntry(ptr3, lifetime);
			}
			ptr->InstanceNameHashCode = instanceNameHashCode;
			ptr->NextInstanceOffset = 0;
			ptr->FirstCounterOffset = ptr2 - this.baseAddress / (long)sizeof(SharedPerformanceCounter.CounterEntry);
			ptr->RefCount = 1;
			ptr->InstanceNameOffset = (int)(num6 - this.baseAddress);
			SharedPerformanceCounter.SafeMarshalCopy(instanceName, (IntPtr)num6);
			num6 += (long)num3;
			if (this.categoryData.UseUniqueSharedMemory)
			{
				SharedPerformanceCounter.InstanceEntry* ptr4 = this.ResolveOffset(categoryPointer->FirstInstanceOffset, SharedPerformanceCounter.InstanceEntrySize);
				SharedPerformanceCounter.CounterEntry* ptr5 = this.ResolveOffset(ptr4->FirstCounterOffset, SharedPerformanceCounter.CounterEntrySize);
				ptr2->CounterNameHashCode = ptr5->CounterNameHashCode;
				SharedPerformanceCounter.SetValue(ptr2, 0L);
				ptr2->CounterNameOffset = ptr5->CounterNameOffset;
				for (int j = 1; j < this.categoryData.CounterNames.Count; j++)
				{
					SharedPerformanceCounter.CounterEntry* ptr6 = ptr2;
					ptr2++;
					ptr5 = this.ResolveOffset(ptr5->NextCounterOffset, SharedPerformanceCounter.CounterEntrySize);
					ptr2->CounterNameHashCode = ptr5->CounterNameHashCode;
					SharedPerformanceCounter.SetValue(ptr2, 0L);
					ptr2->CounterNameOffset = ptr5->CounterNameOffset;
					ptr6->NextCounterOffset = ptr2 - this.baseAddress / (long)sizeof(SharedPerformanceCounter.CounterEntry);
				}
			}
			else
			{
				SharedPerformanceCounter.CounterEntry* ptr7 = null;
				for (int k = 0; k < this.categoryData.CounterNames.Count; k++)
				{
					string text = (string)this.categoryData.CounterNames[k];
					ptr2->CounterNameHashCode = SharedPerformanceCounter.GetWstrHashCode(text);
					ptr2->CounterNameOffset = (int)(num6 - this.baseAddress);
					SharedPerformanceCounter.SafeMarshalCopy(text, (IntPtr)num6);
					num6 += (long)((text.Length + 1) * 2);
					SharedPerformanceCounter.SetValue(ptr2, 0L);
					if (k != 0)
					{
						ptr7->NextCounterOffset = ptr2 - this.baseAddress / (long)sizeof(SharedPerformanceCounter.CounterEntry);
					}
					ptr7 = ptr2;
					ptr2++;
				}
			}
			int num7 = ptr - this.baseAddress / (long)sizeof(SharedPerformanceCounter.InstanceEntry);
			categoryPointer->IsConsistent = 0;
			ptr->NextInstanceOffset = categoryPointer->FirstInstanceOffset;
			categoryPointer->FirstInstanceOffset = num7;
			if (this.categoryData.UseUniqueSharedMemory)
			{
				*(UIntPtr)this.baseAddress = num2;
				categoryPointer->IsConsistent = 1;
			}
			return num4;
		}

		// Token: 0x060030BB RID: 12475 RVA: 0x000DC85C File Offset: 0x000DAA5C
		private unsafe int CreateCounter(SharedPerformanceCounter.CounterEntry* lastCounterPointer, int counterNameHashCode, string counterName)
		{
			int num = (counterName.Length + 1) * 2;
			int num2 = sizeof(SharedPerformanceCounter.CounterEntry) + num;
			int num4;
			int num3 = this.CalculateAndAllocateMemory(num2, out num4);
			num3 += num4;
			long num5 = this.ResolveOffset(num3, num2);
			SharedPerformanceCounter.CounterEntry* ptr = num5;
			num5 += (long)sizeof(SharedPerformanceCounter.CounterEntry);
			ptr->CounterNameOffset = (int)(num5 - this.baseAddress);
			ptr->CounterNameHashCode = counterNameHashCode;
			ptr->NextCounterOffset = 0;
			SharedPerformanceCounter.SetValue(ptr, 0L);
			SharedPerformanceCounter.SafeMarshalCopy(counterName, (IntPtr)num5);
			lastCounterPointer->NextCounterOffset = ptr - this.baseAddress / (long)sizeof(SharedPerformanceCounter.CounterEntry);
			return num3;
		}

		// Token: 0x060030BC RID: 12476 RVA: 0x000DC8EF File Offset: 0x000DAAEF
		private unsafe static void PopulateLifetimeEntry(SharedPerformanceCounter.ProcessLifetimeEntry* lifetimeEntry, PerformanceCounterInstanceLifetime lifetime)
		{
			if (lifetime == PerformanceCounterInstanceLifetime.Process)
			{
				lifetimeEntry->LifetimeType = 1;
				lifetimeEntry->ProcessId = SharedPerformanceCounter.ProcessData.ProcessId;
				lifetimeEntry->StartupTime = SharedPerformanceCounter.ProcessData.StartupTime;
				return;
			}
			lifetimeEntry->ProcessId = 0;
			lifetimeEntry->StartupTime = 0L;
		}

		// Token: 0x060030BD RID: 12477 RVA: 0x000DC92C File Offset: 0x000DAB2C
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		private unsafe static void WaitAndEnterCriticalSection(int* spinLockPointer, out bool taken)
		{
			SharedPerformanceCounter.WaitForCriticalSection(spinLockPointer);
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
			}
			finally
			{
				int num = Interlocked.CompareExchange(ref *spinLockPointer, 1, 0);
				taken = num == 0;
			}
		}

		// Token: 0x060030BE RID: 12478 RVA: 0x000DC968 File Offset: 0x000DAB68
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		private unsafe static void WaitForCriticalSection(int* spinLockPointer)
		{
			int num = 5000;
			while (num > 0 && *spinLockPointer != 0)
			{
				if (*spinLockPointer != 0)
				{
					Thread.Sleep(1);
				}
				num--;
			}
			if (num == 0 && *spinLockPointer != 0)
			{
				*spinLockPointer = 0;
			}
		}

		// Token: 0x060030BF RID: 12479 RVA: 0x000DC99D File Offset: 0x000DAB9D
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		private unsafe static void ExitCriticalSection(int* spinLockPointer)
		{
			*spinLockPointer = 0;
		}

		// Token: 0x060030C0 RID: 12480 RVA: 0x000DC9A4 File Offset: 0x000DABA4
		internal static int GetWstrHashCode(string wstr)
		{
			uint num = 5381U;
			uint num2 = 0U;
			while ((ulong)num2 < (ulong)((long)wstr.Length))
			{
				num = ((num << 5) + num) ^ (uint)wstr[(int)num2];
				num2 += 1U;
			}
			return (int)num;
		}

		// Token: 0x060030C1 RID: 12481 RVA: 0x000DC9DC File Offset: 0x000DABDC
		private unsafe int GetStringLength(char* startChar)
		{
			char* ptr = startChar;
			ulong num = (ulong)(this.baseAddress + (long)this.FileView.FileMappingSize);
			while (ptr < num - 2UL)
			{
				if (*ptr == '\0')
				{
					return (int)((long)(ptr - startChar));
				}
				ptr++;
			}
			throw new InvalidOperationException(SR.GetString("MappingCorrupted"));
		}

		// Token: 0x060030C2 RID: 12482 RVA: 0x000DCA28 File Offset: 0x000DAC28
		private unsafe bool StringEquals(string stringA, int offset)
		{
			char* ptr = this.ResolveOffset(offset, 0);
			ulong num = (ulong)(this.baseAddress + (long)this.FileView.FileMappingSize);
			int i;
			for (i = 0; i < stringA.Length; i++)
			{
				if (ptr + i != num - 2UL)
				{
					throw new InvalidOperationException(SR.GetString("MappingCorrupted"));
				}
				if (stringA[i] != ptr[i])
				{
					return false;
				}
			}
			if (ptr + i != num - 2UL)
			{
				throw new InvalidOperationException(SR.GetString("MappingCorrupted"));
			}
			return ptr[i] == '\0';
		}

		// Token: 0x060030C3 RID: 12483 RVA: 0x000DCABC File Offset: 0x000DACBC
		private unsafe SharedPerformanceCounter.CategoryData GetCategoryData()
		{
			SharedPerformanceCounter.CategoryData categoryData = (SharedPerformanceCounter.CategoryData)SharedPerformanceCounter.categoryDataTable[this.categoryName];
			if (categoryData == null)
			{
				Hashtable hashtable = SharedPerformanceCounter.categoryDataTable;
				lock (hashtable)
				{
					categoryData = (SharedPerformanceCounter.CategoryData)SharedPerformanceCounter.categoryDataTable[this.categoryName];
					if (categoryData == null)
					{
						categoryData = new SharedPerformanceCounter.CategoryData();
						categoryData.FileMappingName = "netfxcustomperfcounters.1.0";
						categoryData.MutexName = this.categoryName;
						RegistryPermission registryPermission = new RegistryPermission(PermissionState.Unrestricted);
						registryPermission.Assert();
						RegistryKey registryKey = null;
						try
						{
							registryKey = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Services\\" + this.categoryName + "\\Performance");
							object value = registryKey.GetValue("CategoryOptions");
							if (value != null)
							{
								int num = (int)value;
								categoryData.EnableReuse = (num & 1) != 0;
								if ((num & 2) != 0)
								{
									categoryData.UseUniqueSharedMemory = true;
									this.InitialOffset = 8;
									categoryData.FileMappingName = "netfxcustomperfcounters.1.0" + this.categoryName;
								}
							}
							object value2 = registryKey.GetValue("FileMappingSize");
							int num2;
							if (value2 != null && categoryData.UseUniqueSharedMemory)
							{
								num2 = (int)value2;
								if (num2 < 32768)
								{
									num2 = 32768;
								}
								if (num2 > 33554432)
								{
									num2 = 33554432;
								}
							}
							else
							{
								num2 = SharedPerformanceCounter.GetFileMappingSizeFromConfig();
								if (categoryData.UseUniqueSharedMemory)
								{
									num2 >>= 2;
								}
							}
							object value3 = registryKey.GetValue("Counter Names");
							byte[] array = value3 as byte[];
							if (array != null)
							{
								ArrayList arrayList = new ArrayList();
								try
								{
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
									int num3 = 0;
									for (int i = 0; i < array.Length - 1; i += 2)
									{
										if (array[i] == 0 && array[i + 1] == 0 && num3 != i)
										{
											string text = new string((sbyte*)ptr, num3, i - num3, Encoding.Unicode);
											arrayList.Add(text.ToLowerInvariant());
											num3 = i + 2;
										}
									}
								}
								finally
								{
									byte[] array2 = null;
								}
								categoryData.CounterNames = arrayList;
							}
							else
							{
								string[] array3 = (string[])value3;
								for (int j = 0; j < array3.Length; j++)
								{
									array3[j] = array3[j].ToLowerInvariant();
								}
								categoryData.CounterNames = new ArrayList(array3);
							}
							if (SharedUtils.CurrentEnvironment == 1)
							{
								categoryData.FileMappingName = "Global\\" + categoryData.FileMappingName;
								categoryData.MutexName = "Global\\" + this.categoryName;
							}
							categoryData.FileMapping = new SharedPerformanceCounter.FileMapping(categoryData.FileMappingName, num2, this.InitialOffset);
							SharedPerformanceCounter.categoryDataTable[this.categoryName] = categoryData;
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
				}
			}
			this.baseAddress = (long)categoryData.FileMapping.FileViewAddress;
			if (categoryData.UseUniqueSharedMemory)
			{
				this.InitialOffset = 8;
			}
			return categoryData;
		}

		// Token: 0x060030C4 RID: 12484 RVA: 0x000DCDE0 File Offset: 0x000DAFE0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static int GetFileMappingSizeFromConfig()
		{
			return DiagnosticsConfiguration.PerfomanceCountersFileMappingSize;
		}

		// Token: 0x060030C5 RID: 12485 RVA: 0x000DCDE8 File Offset: 0x000DAFE8
		private static void RemoveCategoryData(string categoryName)
		{
			Hashtable hashtable = SharedPerformanceCounter.categoryDataTable;
			lock (hashtable)
			{
				SharedPerformanceCounter.categoryDataTable.Remove(categoryName);
			}
		}

		// Token: 0x060030C6 RID: 12486 RVA: 0x000DCE2C File Offset: 0x000DB02C
		private unsafe SharedPerformanceCounter.CounterEntry* GetCounter(string counterName, string instanceName, bool enableReuse, PerformanceCounterInstanceLifetime lifetime)
		{
			int wstrHashCode = SharedPerformanceCounter.GetWstrHashCode(counterName);
			int num;
			if (instanceName != null && instanceName.Length != 0)
			{
				num = SharedPerformanceCounter.GetWstrHashCode(instanceName);
			}
			else
			{
				num = SharedPerformanceCounter.SingleInstanceHashCode;
				instanceName = "systemdiagnosticssharedsingleinstance";
			}
			Mutex mutex = null;
			SharedPerformanceCounter.CounterEntry* ptr = null;
			SharedPerformanceCounter.InstanceEntry* ptr2 = null;
			RuntimeHelpers.PrepareConstrainedRegions();
			SharedPerformanceCounter.CounterEntry* ptr5;
			try
			{
				SharedUtils.EnterMutexWithoutGlobal(this.categoryData.MutexName, ref mutex);
				SharedPerformanceCounter.CategoryEntry* ptr3;
				while (!this.FindCategory(&ptr3))
				{
					bool flag;
					if (this.categoryData.UseUniqueSharedMemory)
					{
						flag = true;
					}
					else
					{
						SharedPerformanceCounter.WaitAndEnterCriticalSection(&ptr3->SpinLock, out flag);
					}
					if (flag)
					{
						int num2;
						try
						{
							num2 = this.CreateCategory(ptr3, num, instanceName, lifetime);
						}
						finally
						{
							if (!this.categoryData.UseUniqueSharedMemory)
							{
								SharedPerformanceCounter.ExitCriticalSection(&ptr3->SpinLock);
							}
						}
						ptr3 = this.ResolveOffset(num2, SharedPerformanceCounter.CategoryEntrySize);
						ptr2 = this.ResolveOffset(ptr3->FirstInstanceOffset, SharedPerformanceCounter.InstanceEntrySize);
						bool flag2 = this.FindCounter(wstrHashCode, counterName, ptr2, &ptr);
						return ptr;
					}
				}
				bool flag3;
				while (!this.FindInstance(num, instanceName, ptr3, &ptr2, true, lifetime, out flag3))
				{
					SharedPerformanceCounter.InstanceEntry* ptr4 = ptr2;
					bool flag4;
					if (this.categoryData.UseUniqueSharedMemory)
					{
						flag4 = true;
					}
					else
					{
						SharedPerformanceCounter.WaitAndEnterCriticalSection(&ptr4->SpinLock, out flag4);
					}
					if (flag4)
					{
						try
						{
							bool flag5 = false;
							if (enableReuse && flag3)
							{
								flag5 = this.TryReuseInstance(num, instanceName, ptr3, &ptr2, lifetime, ptr4);
							}
							if (!flag5)
							{
								int num3 = this.CreateInstance(ptr3, num, instanceName, lifetime);
								ptr2 = this.ResolveOffset(num3, SharedPerformanceCounter.InstanceEntrySize);
								bool flag2 = this.FindCounter(wstrHashCode, counterName, ptr2, &ptr);
								return ptr;
							}
						}
						finally
						{
							if (!this.categoryData.UseUniqueSharedMemory)
							{
								SharedPerformanceCounter.ExitCriticalSection(&ptr4->SpinLock);
							}
						}
					}
				}
				if (this.categoryData.UseUniqueSharedMemory)
				{
					bool flag2 = this.FindCounter(wstrHashCode, counterName, ptr2, &ptr);
					ptr5 = ptr;
				}
				else
				{
					while (!this.FindCounter(wstrHashCode, counterName, ptr2, &ptr))
					{
						bool flag6;
						SharedPerformanceCounter.WaitAndEnterCriticalSection(&ptr->SpinLock, out flag6);
						if (flag6)
						{
							try
							{
								int num4 = this.CreateCounter(ptr, wstrHashCode, counterName);
								return this.ResolveOffset(num4, SharedPerformanceCounter.CounterEntrySize);
							}
							finally
							{
								SharedPerformanceCounter.ExitCriticalSection(&ptr->SpinLock);
							}
						}
					}
					ptr5 = ptr;
				}
			}
			finally
			{
				try
				{
					if (ptr != null && ptr2 != null)
					{
						this.thisInstanceOffset = this.ResolveAddress(ptr2, SharedPerformanceCounter.InstanceEntrySize);
					}
				}
				catch (InvalidOperationException)
				{
					this.thisInstanceOffset = -1;
				}
				if (mutex != null)
				{
					mutex.ReleaseMutex();
					mutex.Close();
				}
			}
			return ptr5;
		}

		// Token: 0x060030C7 RID: 12487 RVA: 0x000DD108 File Offset: 0x000DB308
		private unsafe bool FindCategory(SharedPerformanceCounter.CategoryEntry** returnCategoryPointerReference)
		{
			SharedPerformanceCounter.CategoryEntry* ptr = this.ResolveOffset(this.InitialOffset, SharedPerformanceCounter.CategoryEntrySize);
			SharedPerformanceCounter.CategoryEntry* ptr2 = ptr;
			SharedPerformanceCounter.CategoryEntry* ptr3;
			for (;;)
			{
				if (ptr2->IsConsistent == 0)
				{
					this.Verify(ptr2);
				}
				if (ptr2->CategoryNameHashCode == this.categoryNameHashCode && this.StringEquals(this.categoryName, ptr2->CategoryNameOffset))
				{
					break;
				}
				ptr3 = ptr2;
				if (ptr2->NextCategoryOffset == 0)
				{
					goto IL_006C;
				}
				ptr2 = this.ResolveOffset(ptr2->NextCategoryOffset, SharedPerformanceCounter.CategoryEntrySize);
			}
			*(IntPtr*)returnCategoryPointerReference = ptr2;
			return true;
			IL_006C:
			*(IntPtr*)returnCategoryPointerReference = ptr3;
			return false;
		}

		// Token: 0x060030C8 RID: 12488 RVA: 0x000DD188 File Offset: 0x000DB388
		private unsafe bool FindCounter(int counterNameHashCode, string counterName, SharedPerformanceCounter.InstanceEntry* instancePointer, SharedPerformanceCounter.CounterEntry** returnCounterPointerReference)
		{
			SharedPerformanceCounter.CounterEntry* ptr = this.ResolveOffset(instancePointer->FirstCounterOffset, SharedPerformanceCounter.CounterEntrySize);
			while (ptr->CounterNameHashCode != counterNameHashCode || !this.StringEquals(counterName, ptr->CounterNameOffset))
			{
				SharedPerformanceCounter.CounterEntry* ptr2 = ptr;
				if (ptr->NextCounterOffset == 0)
				{
					*(IntPtr*)returnCounterPointerReference = ptr2;
					return false;
				}
				ptr = this.ResolveOffset(ptr->NextCounterOffset, SharedPerformanceCounter.CounterEntrySize);
			}
			*(IntPtr*)returnCounterPointerReference = ptr;
			return true;
		}

		// Token: 0x060030C9 RID: 12489 RVA: 0x000DD1EC File Offset: 0x000DB3EC
		private unsafe bool FindInstance(int instanceNameHashCode, string instanceName, SharedPerformanceCounter.CategoryEntry* categoryPointer, SharedPerformanceCounter.InstanceEntry** returnInstancePointerReference, bool activateUnusedInstances, PerformanceCounterInstanceLifetime lifetime, out bool foundFreeInstance)
		{
			SharedPerformanceCounter.InstanceEntry* ptr = this.ResolveOffset(categoryPointer->FirstInstanceOffset, SharedPerformanceCounter.InstanceEntrySize);
			foundFreeInstance = false;
			if (ptr->InstanceNameHashCode == SharedPerformanceCounter.SingleInstanceHashCode)
			{
				if (this.StringEquals("systemdiagnosticssharedsingleinstance", ptr->InstanceNameOffset))
				{
					if (instanceName != "systemdiagnosticssharedsingleinstance")
					{
						throw new InvalidOperationException(SR.GetString("SingleInstanceOnly", new object[] { this.categoryName }));
					}
				}
				else if (instanceName == "systemdiagnosticssharedsingleinstance")
				{
					throw new InvalidOperationException(SR.GetString("MultiInstanceOnly", new object[] { this.categoryName }));
				}
			}
			else if (instanceName == "systemdiagnosticssharedsingleinstance")
			{
				throw new InvalidOperationException(SR.GetString("MultiInstanceOnly", new object[] { this.categoryName }));
			}
			bool flag = activateUnusedInstances;
			if (activateUnusedInstances)
			{
				int num = SharedPerformanceCounter.InstanceEntrySize + SharedPerformanceCounter.ProcessLifetimeEntrySize + 256 + SharedPerformanceCounter.CounterEntrySize * this.categoryData.CounterNames.Count;
				int num2 = *(UIntPtr)this.baseAddress;
				int num4;
				int num3 = this.CalculateMemoryNoBoundsCheck(num2, num, out num4);
				if (num3 <= this.FileView.FileMappingSize && num3 >= 0)
				{
					long num5 = DateTime.Now.Ticks - Volatile.Read(ref SharedPerformanceCounter.LastInstanceLifetimeSweepTick);
					if (num5 < 300000000L)
					{
						flag = false;
					}
				}
			}
			new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Assert();
			bool flag3;
			try
			{
				bool flag2;
				SharedPerformanceCounter.InstanceEntry* ptr2;
				for (;;)
				{
					flag2 = false;
					if (flag && ptr->RefCount != 0)
					{
						flag2 = true;
						this.VerifyLifetime(ptr);
					}
					if (ptr->InstanceNameHashCode == instanceNameHashCode && this.StringEquals(instanceName, ptr->InstanceNameOffset))
					{
						break;
					}
					if (ptr->RefCount == 0)
					{
						foundFreeInstance = true;
					}
					ptr2 = ptr;
					if (ptr->NextInstanceOffset == 0)
					{
						goto IL_031E;
					}
					ptr = this.ResolveOffset(ptr->NextInstanceOffset, SharedPerformanceCounter.InstanceEntrySize);
				}
				*(IntPtr*)returnInstancePointerReference = ptr;
				SharedPerformanceCounter.CounterEntry* ptr3 = this.ResolveOffset(ptr->FirstCounterOffset, SharedPerformanceCounter.CounterEntrySize);
				SharedPerformanceCounter.ProcessLifetimeEntry* ptr4;
				if (this.categoryData.UseUniqueSharedMemory)
				{
					ptr4 = this.ResolveOffset(ptr3->LifetimeOffset, SharedPerformanceCounter.ProcessLifetimeEntrySize);
				}
				else
				{
					ptr4 = null;
				}
				if (!flag2 && ptr->RefCount != 0)
				{
					this.VerifyLifetime(ptr);
				}
				if (ptr->RefCount != 0)
				{
					if (ptr4 != null && ptr4->ProcessId != 0)
					{
						if (lifetime != PerformanceCounterInstanceLifetime.Process)
						{
							throw new InvalidOperationException(SR.GetString("CantConvertProcessToGlobal"));
						}
						if (SharedPerformanceCounter.ProcessData.ProcessId != ptr4->ProcessId)
						{
							throw new InvalidOperationException(SR.GetString("InstanceAlreadyExists", new object[] { instanceName }));
						}
						if (ptr4->StartupTime != -1L && SharedPerformanceCounter.ProcessData.StartupTime != -1L && SharedPerformanceCounter.ProcessData.StartupTime != ptr4->StartupTime)
						{
							throw new InvalidOperationException(SR.GetString("InstanceAlreadyExists", new object[] { instanceName }));
						}
					}
					else if (lifetime == PerformanceCounterInstanceLifetime.Process)
					{
						throw new InvalidOperationException(SR.GetString("CantConvertGlobalToProcess"));
					}
					return true;
				}
				if (activateUnusedInstances)
				{
					Mutex mutex = null;
					RuntimeHelpers.PrepareConstrainedRegions();
					try
					{
						SharedUtils.EnterMutexWithoutGlobal(this.categoryData.MutexName, ref mutex);
						this.ClearCounterValues(ptr);
						if (ptr4 != null)
						{
							SharedPerformanceCounter.PopulateLifetimeEntry(ptr4, lifetime);
						}
						ptr->RefCount = 1;
						return true;
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
				return false;
				IL_031E:
				*(IntPtr*)returnInstancePointerReference = ptr2;
				flag3 = false;
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
				if (flag)
				{
					Volatile.Write(ref SharedPerformanceCounter.LastInstanceLifetimeSweepTick, DateTime.Now.Ticks);
				}
			}
			return flag3;
		}

		// Token: 0x060030CA RID: 12490 RVA: 0x000DD578 File Offset: 0x000DB778
		private unsafe bool TryReuseInstance(int instanceNameHashCode, string instanceName, SharedPerformanceCounter.CategoryEntry* categoryPointer, SharedPerformanceCounter.InstanceEntry** returnInstancePointerReference, PerformanceCounterInstanceLifetime lifetime, SharedPerformanceCounter.InstanceEntry* lockInstancePointer)
		{
			SharedPerformanceCounter.InstanceEntry* ptr = this.ResolveOffset(categoryPointer->FirstInstanceOffset, SharedPerformanceCounter.InstanceEntrySize);
			SharedPerformanceCounter.InstanceEntry* ptr4;
			for (;;)
			{
				if (ptr->RefCount == 0)
				{
					long num;
					bool flag;
					if (this.categoryData.UseUniqueSharedMemory)
					{
						num = this.ResolveOffset(ptr->InstanceNameOffset, 256);
						flag = true;
					}
					else
					{
						num = this.ResolveOffset(ptr->InstanceNameOffset, 0);
						int stringLength = this.GetStringLength(num);
						flag = stringLength == instanceName.Length;
					}
					bool flag2 = lockInstancePointer == ptr || this.categoryData.UseUniqueSharedMemory;
					if (flag)
					{
						bool flag3;
						if (flag2)
						{
							flag3 = true;
						}
						else
						{
							SharedPerformanceCounter.WaitAndEnterCriticalSection(&ptr->SpinLock, out flag3);
						}
						if (flag3)
						{
							try
							{
								SharedPerformanceCounter.SafeMarshalCopy(instanceName, (IntPtr)num);
								ptr->InstanceNameHashCode = instanceNameHashCode;
								*(IntPtr*)returnInstancePointerReference = ptr;
								this.ClearCounterValues(*(IntPtr*)returnInstancePointerReference);
								if (this.categoryData.UseUniqueSharedMemory)
								{
									SharedPerformanceCounter.CounterEntry* ptr2 = this.ResolveOffset(ptr->FirstCounterOffset, SharedPerformanceCounter.CounterEntrySize);
									SharedPerformanceCounter.ProcessLifetimeEntry* ptr3 = this.ResolveOffset(ptr2->LifetimeOffset, SharedPerformanceCounter.ProcessLifetimeEntrySize);
									SharedPerformanceCounter.PopulateLifetimeEntry(ptr3, lifetime);
								}
								((IntPtr*)returnInstancePointerReference)->RefCount = 1;
								return true;
							}
							finally
							{
								if (!flag2)
								{
									SharedPerformanceCounter.ExitCriticalSection(&ptr->SpinLock);
								}
							}
						}
					}
				}
				ptr4 = ptr;
				if (ptr->NextInstanceOffset == 0)
				{
					break;
				}
				ptr = this.ResolveOffset(ptr->NextInstanceOffset, SharedPerformanceCounter.InstanceEntrySize);
			}
			*(IntPtr*)returnInstancePointerReference = ptr4;
			return false;
		}

		// Token: 0x060030CB RID: 12491 RVA: 0x000DD6DC File Offset: 0x000DB8DC
		private unsafe void Verify(SharedPerformanceCounter.CategoryEntry* currentCategoryPointer)
		{
			if (!this.categoryData.UseUniqueSharedMemory)
			{
				return;
			}
			Mutex mutex = null;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				SharedUtils.EnterMutexWithoutGlobal(this.categoryData.MutexName, ref mutex);
				this.VerifyCategory(currentCategoryPointer);
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

		// Token: 0x060030CC RID: 12492 RVA: 0x000DD73C File Offset: 0x000DB93C
		private unsafe void VerifyCategory(SharedPerformanceCounter.CategoryEntry* currentCategoryPointer)
		{
			int num = *(UIntPtr)this.baseAddress;
			this.ResolveOffset(num, 0);
			int num2 = this.ResolveAddress(currentCategoryPointer, SharedPerformanceCounter.CategoryEntrySize);
			if (num2 >= num)
			{
				currentCategoryPointer->SpinLock = 0;
				currentCategoryPointer->CategoryNameHashCode = 0;
				currentCategoryPointer->CategoryNameOffset = 0;
				currentCategoryPointer->FirstInstanceOffset = 0;
				currentCategoryPointer->NextCategoryOffset = 0;
				currentCategoryPointer->IsConsistent = 0;
				return;
			}
			if (currentCategoryPointer->NextCategoryOffset > num)
			{
				currentCategoryPointer->NextCategoryOffset = 0;
			}
			else if (currentCategoryPointer->NextCategoryOffset != 0)
			{
				this.VerifyCategory(this.ResolveOffset(currentCategoryPointer->NextCategoryOffset, SharedPerformanceCounter.CategoryEntrySize));
			}
			if (currentCategoryPointer->FirstInstanceOffset != 0)
			{
				if (currentCategoryPointer->FirstInstanceOffset > num)
				{
					SharedPerformanceCounter.InstanceEntry* ptr = this.ResolveOffset(currentCategoryPointer->FirstInstanceOffset, SharedPerformanceCounter.InstanceEntrySize);
					currentCategoryPointer->FirstInstanceOffset = ptr->NextInstanceOffset;
					if (currentCategoryPointer->FirstInstanceOffset > num)
					{
						currentCategoryPointer->FirstInstanceOffset = 0;
					}
				}
				if (currentCategoryPointer->FirstInstanceOffset != 0)
				{
					this.VerifyInstance(this.ResolveOffset(currentCategoryPointer->FirstInstanceOffset, SharedPerformanceCounter.InstanceEntrySize));
				}
			}
			currentCategoryPointer->IsConsistent = 1;
		}

		// Token: 0x060030CD RID: 12493 RVA: 0x000DD834 File Offset: 0x000DBA34
		private unsafe void VerifyInstance(SharedPerformanceCounter.InstanceEntry* currentInstancePointer)
		{
			int num = *(UIntPtr)this.baseAddress;
			this.ResolveOffset(num, 0);
			if (currentInstancePointer->NextInstanceOffset > num)
			{
				currentInstancePointer->NextInstanceOffset = 0;
				return;
			}
			if (currentInstancePointer->NextInstanceOffset != 0)
			{
				this.VerifyInstance(this.ResolveOffset(currentInstancePointer->NextInstanceOffset, SharedPerformanceCounter.InstanceEntrySize));
			}
		}

		// Token: 0x060030CE RID: 12494 RVA: 0x000DD884 File Offset: 0x000DBA84
		private unsafe void VerifyLifetime(SharedPerformanceCounter.InstanceEntry* currentInstancePointer)
		{
			SharedPerformanceCounter.CounterEntry* ptr = this.ResolveOffset(currentInstancePointer->FirstCounterOffset, SharedPerformanceCounter.CounterEntrySize);
			if (ptr->LifetimeOffset != 0)
			{
				SharedPerformanceCounter.ProcessLifetimeEntry* ptr2 = this.ResolveOffset(ptr->LifetimeOffset, SharedPerformanceCounter.ProcessLifetimeEntrySize);
				if (ptr2->LifetimeType == 1)
				{
					int processId = ptr2->ProcessId;
					long startupTime = ptr2->StartupTime;
					if (processId != 0)
					{
						if (processId == SharedPerformanceCounter.ProcessData.ProcessId)
						{
							if (SharedPerformanceCounter.ProcessData.StartupTime != -1L && startupTime != -1L && SharedPerformanceCounter.ProcessData.StartupTime != startupTime)
							{
								currentInstancePointer->RefCount = 0;
								return;
							}
						}
						else
						{
							using (Microsoft.Win32.SafeHandles.SafeProcessHandle safeProcessHandle = Microsoft.Win32.SafeHandles.SafeProcessHandle.OpenProcess(1024, false, processId))
							{
								int lastWin32Error = Marshal.GetLastWin32Error();
								if (lastWin32Error == 87 && safeProcessHandle.IsInvalid)
								{
									currentInstancePointer->RefCount = 0;
									return;
								}
								long num;
								long num2;
								if (!safeProcessHandle.IsInvalid && startupTime != -1L && Microsoft.Win32.NativeMethods.GetProcessTimes(safeProcessHandle, out num, out num2, out num2, out num2) && num != startupTime)
								{
									currentInstancePointer->RefCount = 0;
									return;
								}
							}
							using (Microsoft.Win32.SafeHandles.SafeProcessHandle safeProcessHandle2 = Microsoft.Win32.SafeHandles.SafeProcessHandle.OpenProcess(1048576, false, processId))
							{
								if (!safeProcessHandle2.IsInvalid)
								{
									using (ProcessWaitHandle processWaitHandle = new ProcessWaitHandle(safeProcessHandle2))
									{
										if (processWaitHandle.WaitOne(0, false))
										{
											currentInstancePointer->RefCount = 0;
										}
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x060030CF RID: 12495 RVA: 0x000DDA04 File Offset: 0x000DBC04
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		internal unsafe long IncrementBy(long value)
		{
			if (this.counterEntryPointer == null)
			{
				return 0L;
			}
			SharedPerformanceCounter.CounterEntry* ptr = this.counterEntryPointer;
			return SharedPerformanceCounter.AddToValue(ptr, value);
		}

		// Token: 0x060030D0 RID: 12496 RVA: 0x000DDA2C File Offset: 0x000DBC2C
		internal long Increment()
		{
			if (this.counterEntryPointer == null)
			{
				return 0L;
			}
			return SharedPerformanceCounter.IncrementUnaligned(this.counterEntryPointer);
		}

		// Token: 0x060030D1 RID: 12497 RVA: 0x000DDA46 File Offset: 0x000DBC46
		internal long Decrement()
		{
			if (this.counterEntryPointer == null)
			{
				return 0L;
			}
			return SharedPerformanceCounter.DecrementUnaligned(this.counterEntryPointer);
		}

		// Token: 0x060030D2 RID: 12498 RVA: 0x000DDA60 File Offset: 0x000DBC60
		internal static void RemoveAllInstances(string categoryName)
		{
			SharedPerformanceCounter sharedPerformanceCounter = new SharedPerformanceCounter(categoryName, null, null);
			sharedPerformanceCounter.RemoveAllInstances();
			SharedPerformanceCounter.RemoveCategoryData(categoryName);
		}

		// Token: 0x060030D3 RID: 12499 RVA: 0x000DDA84 File Offset: 0x000DBC84
		private unsafe void RemoveAllInstances()
		{
			SharedPerformanceCounter.CategoryEntry* ptr;
			if (!this.FindCategory(&ptr))
			{
				return;
			}
			SharedPerformanceCounter.InstanceEntry* ptr2 = this.ResolveOffset(ptr->FirstInstanceOffset, SharedPerformanceCounter.InstanceEntrySize);
			Mutex mutex = null;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				SharedUtils.EnterMutexWithoutGlobal(this.categoryData.MutexName, ref mutex);
				for (;;)
				{
					this.RemoveOneInstance(ptr2, true);
					if (ptr2->NextInstanceOffset == 0)
					{
						break;
					}
					ptr2 = this.ResolveOffset(ptr2->NextInstanceOffset, SharedPerformanceCounter.InstanceEntrySize);
				}
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

		// Token: 0x060030D4 RID: 12500 RVA: 0x000DDB10 File Offset: 0x000DBD10
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		internal unsafe void RemoveInstance(string instanceName, PerformanceCounterInstanceLifetime instanceLifetime)
		{
			if (instanceName == null || instanceName.Length == 0)
			{
				return;
			}
			int wstrHashCode = SharedPerformanceCounter.GetWstrHashCode(instanceName);
			SharedPerformanceCounter.CategoryEntry* ptr;
			if (!this.FindCategory(&ptr))
			{
				return;
			}
			SharedPerformanceCounter.InstanceEntry* ptr2 = null;
			bool flag = false;
			Mutex mutex = null;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				SharedUtils.EnterMutexWithoutGlobal(this.categoryData.MutexName, ref mutex);
				if (this.thisInstanceOffset != -1)
				{
					try
					{
						ptr2 = this.ResolveOffset(this.thisInstanceOffset, SharedPerformanceCounter.InstanceEntrySize);
						if (ptr2->InstanceNameHashCode == wstrHashCode && this.StringEquals(instanceName, ptr2->InstanceNameOffset))
						{
							flag = true;
							SharedPerformanceCounter.CounterEntry* ptr3 = this.ResolveOffset(ptr2->FirstCounterOffset, SharedPerformanceCounter.CounterEntrySize);
							if (this.categoryData.UseUniqueSharedMemory)
							{
								SharedPerformanceCounter.ProcessLifetimeEntry* ptr4 = this.ResolveOffset(ptr3->LifetimeOffset, SharedPerformanceCounter.ProcessLifetimeEntrySize);
								if (ptr4 != null && ptr4->LifetimeType == 1 && ptr4->ProcessId != 0)
								{
									flag &= instanceLifetime == PerformanceCounterInstanceLifetime.Process;
									flag &= SharedPerformanceCounter.ProcessData.ProcessId == ptr4->ProcessId;
									if (ptr4->StartupTime != -1L && SharedPerformanceCounter.ProcessData.StartupTime != -1L)
									{
										flag &= SharedPerformanceCounter.ProcessData.StartupTime == ptr4->StartupTime;
									}
								}
								else
								{
									flag &= instanceLifetime != PerformanceCounterInstanceLifetime.Process;
								}
							}
						}
					}
					catch (InvalidOperationException)
					{
						flag = false;
					}
					if (!flag)
					{
						this.thisInstanceOffset = -1;
					}
				}
				bool flag2;
				if (flag || this.FindInstance(wstrHashCode, instanceName, ptr, &ptr2, false, instanceLifetime, out flag2))
				{
					if (ptr2 != null)
					{
						this.RemoveOneInstance(ptr2, false);
					}
				}
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

		// Token: 0x060030D5 RID: 12501 RVA: 0x000DDCC4 File Offset: 0x000DBEC4
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		private unsafe void RemoveOneInstance(SharedPerformanceCounter.InstanceEntry* instancePointer, bool clearValue)
		{
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				if (!this.categoryData.UseUniqueSharedMemory)
				{
					while (!flag)
					{
						SharedPerformanceCounter.WaitAndEnterCriticalSection(&instancePointer->SpinLock, out flag);
					}
				}
				instancePointer->RefCount = 0;
				if (clearValue)
				{
					this.ClearCounterValues(instancePointer);
				}
			}
			finally
			{
				if (flag)
				{
					SharedPerformanceCounter.ExitCriticalSection(&instancePointer->SpinLock);
				}
			}
		}

		// Token: 0x060030D6 RID: 12502 RVA: 0x000DDD2C File Offset: 0x000DBF2C
		private unsafe void ClearCounterValues(SharedPerformanceCounter.InstanceEntry* instancePointer)
		{
			SharedPerformanceCounter.CounterEntry* ptr = null;
			if (instancePointer->FirstCounterOffset != 0)
			{
				ptr = this.ResolveOffset(instancePointer->FirstCounterOffset, SharedPerformanceCounter.CounterEntrySize);
			}
			while (ptr != null)
			{
				SharedPerformanceCounter.SetValue(ptr, 0L);
				if (ptr->NextCounterOffset != 0)
				{
					ptr = this.ResolveOffset(ptr->NextCounterOffset, SharedPerformanceCounter.CounterEntrySize);
				}
				else
				{
					ptr = null;
				}
			}
		}

		// Token: 0x060030D7 RID: 12503 RVA: 0x000DDD88 File Offset: 0x000DBF88
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		private unsafe static long AddToValue(SharedPerformanceCounter.CounterEntry* counterEntry, long addend)
		{
			if (SharedPerformanceCounter.IsMisaligned(counterEntry))
			{
				ulong num = (ulong)((SharedPerformanceCounter.CounterEntryMisaligned*)counterEntry)->Value_hi;
				num <<= 32;
				num |= (ulong)((SharedPerformanceCounter.CounterEntryMisaligned*)counterEntry)->Value_lo;
				num += (ulong)addend;
				((SharedPerformanceCounter.CounterEntryMisaligned*)counterEntry)->Value_hi = (int)(num >> 32);
				((SharedPerformanceCounter.CounterEntryMisaligned*)counterEntry)->Value_lo = (int)(num & (ulong)(-1));
				return (long)num;
			}
			return Interlocked.Add(ref counterEntry->Value, addend);
		}

		// Token: 0x060030D8 RID: 12504 RVA: 0x000DDDDE File Offset: 0x000DBFDE
		private unsafe static long DecrementUnaligned(SharedPerformanceCounter.CounterEntry* counterEntry)
		{
			if (SharedPerformanceCounter.IsMisaligned(counterEntry))
			{
				return SharedPerformanceCounter.AddToValue(counterEntry, -1L);
			}
			return Interlocked.Decrement(ref counterEntry->Value);
		}

		// Token: 0x060030D9 RID: 12505 RVA: 0x000DDDFC File Offset: 0x000DBFFC
		private unsafe static long GetValue(SharedPerformanceCounter.CounterEntry* counterEntry)
		{
			if (SharedPerformanceCounter.IsMisaligned(counterEntry))
			{
				ulong num = (ulong)((SharedPerformanceCounter.CounterEntryMisaligned*)counterEntry)->Value_hi;
				num <<= 32;
				return (long)(num | (ulong)((SharedPerformanceCounter.CounterEntryMisaligned*)counterEntry)->Value_lo);
			}
			return counterEntry->Value;
		}

		// Token: 0x060030DA RID: 12506 RVA: 0x000DDE32 File Offset: 0x000DC032
		private unsafe static long IncrementUnaligned(SharedPerformanceCounter.CounterEntry* counterEntry)
		{
			if (SharedPerformanceCounter.IsMisaligned(counterEntry))
			{
				return SharedPerformanceCounter.AddToValue(counterEntry, 1L);
			}
			return Interlocked.Increment(ref counterEntry->Value);
		}

		// Token: 0x060030DB RID: 12507 RVA: 0x000DDE50 File Offset: 0x000DC050
		private unsafe static void SetValue(SharedPerformanceCounter.CounterEntry* counterEntry, long value)
		{
			if (SharedPerformanceCounter.IsMisaligned(counterEntry))
			{
				((SharedPerformanceCounter.CounterEntryMisaligned*)counterEntry)->Value_lo = (int)(value & (long)((ulong)(-1)));
				((SharedPerformanceCounter.CounterEntryMisaligned*)counterEntry)->Value_hi = (int)(value >> 32);
				return;
			}
			counterEntry->Value = value;
		}

		// Token: 0x060030DC RID: 12508 RVA: 0x000DDE85 File Offset: 0x000DC085
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		private unsafe static bool IsMisaligned(SharedPerformanceCounter.CounterEntry* counterEntry)
		{
			return (counterEntry & 7L) != null;
		}

		// Token: 0x060030DD RID: 12509 RVA: 0x000DDE90 File Offset: 0x000DC090
		private long ResolveOffset(int offset, int sizeToRead)
		{
			if (offset > this.FileView.FileMappingSize - sizeToRead || offset < 0)
			{
				throw new InvalidOperationException(SR.GetString("MappingCorrupted"));
			}
			return this.baseAddress + (long)offset;
		}

		// Token: 0x060030DE RID: 12510 RVA: 0x000DDECC File Offset: 0x000DC0CC
		private int ResolveAddress(long address, int sizeToRead)
		{
			int num = (int)(address - this.baseAddress);
			if (num > this.FileView.FileMappingSize - sizeToRead || num < 0)
			{
				throw new InvalidOperationException(SR.GetString("MappingCorrupted"));
			}
			return num;
		}

		// Token: 0x060030DF RID: 12511 RVA: 0x000DDF08 File Offset: 0x000DC108
		private static void SafeMarshalCopy(string str, IntPtr nativePointer)
		{
			char[] array = new char[str.Length + 1];
			str.CopyTo(0, array, 0, str.Length);
			array[str.Length] = '\0';
			Marshal.Copy(array, 0, nativePointer, array.Length);
		}

		// Token: 0x040028B1 RID: 10417
		private const int MaxSpinCount = 5000;

		// Token: 0x040028B2 RID: 10418
		internal const int DefaultCountersFileMappingSize = 524288;

		// Token: 0x040028B3 RID: 10419
		internal const int MaxCountersFileMappingSize = 33554432;

		// Token: 0x040028B4 RID: 10420
		internal const int MinCountersFileMappingSize = 32768;

		// Token: 0x040028B5 RID: 10421
		internal const int InstanceNameMaxLength = 127;

		// Token: 0x040028B6 RID: 10422
		internal const int InstanceNameSlotSize = 256;

		// Token: 0x040028B7 RID: 10423
		internal const string SingleInstanceName = "systemdiagnosticssharedsingleinstance";

		// Token: 0x040028B8 RID: 10424
		internal const string DefaultFileMappingName = "netfxcustomperfcounters.1.0";

		// Token: 0x040028B9 RID: 10425
		internal static readonly int SingleInstanceHashCode = SharedPerformanceCounter.GetWstrHashCode("systemdiagnosticssharedsingleinstance");

		// Token: 0x040028BA RID: 10426
		private static Hashtable categoryDataTable = new Hashtable(StringComparer.Ordinal);

		// Token: 0x040028BB RID: 10427
		private static readonly int CategoryEntrySize = Marshal.SizeOf(typeof(SharedPerformanceCounter.CategoryEntry));

		// Token: 0x040028BC RID: 10428
		private static readonly int InstanceEntrySize = Marshal.SizeOf(typeof(SharedPerformanceCounter.InstanceEntry));

		// Token: 0x040028BD RID: 10429
		private static readonly int CounterEntrySize = Marshal.SizeOf(typeof(SharedPerformanceCounter.CounterEntry));

		// Token: 0x040028BE RID: 10430
		private static readonly int ProcessLifetimeEntrySize = Marshal.SizeOf(typeof(SharedPerformanceCounter.ProcessLifetimeEntry));

		// Token: 0x040028BF RID: 10431
		private static long LastInstanceLifetimeSweepTick;

		// Token: 0x040028C0 RID: 10432
		private const long InstanceLifetimeSweepWindow = 300000000L;

		// Token: 0x040028C1 RID: 10433
		private static volatile ProcessData procData;

		// Token: 0x040028C2 RID: 10434
		internal int InitialOffset = 4;

		// Token: 0x040028C3 RID: 10435
		private SharedPerformanceCounter.CategoryData categoryData;

		// Token: 0x040028C4 RID: 10436
		private long baseAddress;

		// Token: 0x040028C5 RID: 10437
		private unsafe SharedPerformanceCounter.CounterEntry* counterEntryPointer;

		// Token: 0x040028C6 RID: 10438
		private string categoryName;

		// Token: 0x040028C7 RID: 10439
		private int categoryNameHashCode;

		// Token: 0x040028C8 RID: 10440
		private int thisInstanceOffset = -1;

		// Token: 0x02000887 RID: 2183
		private class FileMapping
		{
			// Token: 0x0600457B RID: 17787 RVA: 0x00121A05 File Offset: 0x0011FC05
			public FileMapping(string fileMappingName, int fileMappingSize, int initialOffset)
			{
				this.Initialize(fileMappingName, fileMappingSize, initialOffset);
			}

			// Token: 0x17000FB5 RID: 4021
			// (get) Token: 0x0600457C RID: 17788 RVA: 0x00121A16 File Offset: 0x0011FC16
			internal IntPtr FileViewAddress
			{
				get
				{
					if (this.fileViewAddress.IsInvalid)
					{
						throw new InvalidOperationException(SR.GetString("SharedMemoryGhosted"));
					}
					return this.fileViewAddress.DangerousGetHandle();
				}
			}

			// Token: 0x0600457D RID: 17789 RVA: 0x00121A40 File Offset: 0x0011FC40
			private unsafe void Initialize(string fileMappingName, int fileMappingSize, int initialOffset)
			{
				SharedUtils.CheckEnvironment();
				SafeLocalMemHandle safeLocalMemHandle = null;
				new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Assert();
				try
				{
					string text = "D:(A;OICI;FRFWGRGW;;;AU)(A;OICI;FRFWGRGW;;;S-1-5-33)";
					if (!SafeLocalMemHandle.ConvertStringSecurityDescriptorToSecurityDescriptor(text, 1, out safeLocalMemHandle, IntPtr.Zero))
					{
						throw new InvalidOperationException(SR.GetString("SetSecurityDescriptorFailed"));
					}
					Microsoft.Win32.NativeMethods.SECURITY_ATTRIBUTES security_ATTRIBUTES = new Microsoft.Win32.NativeMethods.SECURITY_ATTRIBUTES();
					security_ATTRIBUTES.lpSecurityDescriptor = safeLocalMemHandle;
					security_ATTRIBUTES.bInheritHandle = false;
					int num = 14;
					int num2 = 0;
					bool flag = false;
					while (!flag && num > 0)
					{
						this.fileMappingHandle = Microsoft.Win32.NativeMethods.CreateFileMapping((IntPtr)(-1), security_ATTRIBUTES, 4, 0, fileMappingSize, fileMappingName);
						if (Marshal.GetLastWin32Error() != 5 || !this.fileMappingHandle.IsInvalid)
						{
							flag = true;
						}
						else
						{
							this.fileMappingHandle.SetHandleAsInvalid();
							this.fileMappingHandle = Microsoft.Win32.NativeMethods.OpenFileMapping(2, false, fileMappingName);
							if (Marshal.GetLastWin32Error() != 2 || !this.fileMappingHandle.IsInvalid)
							{
								flag = true;
							}
							else
							{
								num--;
								if (num2 == 0)
								{
									num2 = 10;
								}
								else
								{
									Thread.Sleep(num2);
									num2 *= 2;
								}
							}
						}
					}
					if (this.fileMappingHandle.IsInvalid)
					{
						throw new InvalidOperationException(SR.GetString("CantCreateFileMapping"));
					}
					this.fileViewAddress = SafeFileMapViewHandle.MapViewOfFile(this.fileMappingHandle, 2, 0, 0, UIntPtr.Zero);
					if (this.fileViewAddress.IsInvalid)
					{
						throw new InvalidOperationException(SR.GetString("CantMapFileView"));
					}
					Microsoft.Win32.NativeMethods.MEMORY_BASIC_INFORMATION memory_BASIC_INFORMATION = default(Microsoft.Win32.NativeMethods.MEMORY_BASIC_INFORMATION);
					if (Microsoft.Win32.NativeMethods.VirtualQuery(this.fileViewAddress, ref memory_BASIC_INFORMATION, (IntPtr)sizeof(Microsoft.Win32.NativeMethods.MEMORY_BASIC_INFORMATION)) == IntPtr.Zero)
					{
						throw new InvalidOperationException(SR.GetString("CantGetMappingSize"));
					}
					this.FileMappingSize = (int)(uint)memory_BASIC_INFORMATION.RegionSize;
				}
				finally
				{
					if (safeLocalMemHandle != null)
					{
						safeLocalMemHandle.Close();
					}
					CodeAccessPermission.RevertAssert();
				}
				SafeNativeMethods.InterlockedCompareExchange(this.fileViewAddress.DangerousGetHandle(), initialOffset, 0);
			}

			// Token: 0x0400378F RID: 14223
			internal int FileMappingSize;

			// Token: 0x04003790 RID: 14224
			private SafeFileMapViewHandle fileViewAddress;

			// Token: 0x04003791 RID: 14225
			private Microsoft.Win32.SafeHandles.SafeFileMappingHandle fileMappingHandle;
		}

		// Token: 0x02000888 RID: 2184
		private struct CategoryEntry
		{
			// Token: 0x04003792 RID: 14226
			public int SpinLock;

			// Token: 0x04003793 RID: 14227
			public int CategoryNameHashCode;

			// Token: 0x04003794 RID: 14228
			public int CategoryNameOffset;

			// Token: 0x04003795 RID: 14229
			public int FirstInstanceOffset;

			// Token: 0x04003796 RID: 14230
			public int NextCategoryOffset;

			// Token: 0x04003797 RID: 14231
			public int IsConsistent;
		}

		// Token: 0x02000889 RID: 2185
		private struct InstanceEntry
		{
			// Token: 0x04003798 RID: 14232
			public int SpinLock;

			// Token: 0x04003799 RID: 14233
			public int InstanceNameHashCode;

			// Token: 0x0400379A RID: 14234
			public int InstanceNameOffset;

			// Token: 0x0400379B RID: 14235
			public int RefCount;

			// Token: 0x0400379C RID: 14236
			public int FirstCounterOffset;

			// Token: 0x0400379D RID: 14237
			public int NextInstanceOffset;
		}

		// Token: 0x0200088A RID: 2186
		private struct CounterEntry
		{
			// Token: 0x0400379E RID: 14238
			public int SpinLock;

			// Token: 0x0400379F RID: 14239
			public int CounterNameHashCode;

			// Token: 0x040037A0 RID: 14240
			public int CounterNameOffset;

			// Token: 0x040037A1 RID: 14241
			public int LifetimeOffset;

			// Token: 0x040037A2 RID: 14242
			public long Value;

			// Token: 0x040037A3 RID: 14243
			public int NextCounterOffset;

			// Token: 0x040037A4 RID: 14244
			public int padding2;
		}

		// Token: 0x0200088B RID: 2187
		private struct CounterEntryMisaligned
		{
			// Token: 0x040037A5 RID: 14245
			public int SpinLock;

			// Token: 0x040037A6 RID: 14246
			public int CounterNameHashCode;

			// Token: 0x040037A7 RID: 14247
			public int CounterNameOffset;

			// Token: 0x040037A8 RID: 14248
			public int LifetimeOffset;

			// Token: 0x040037A9 RID: 14249
			public int Value_lo;

			// Token: 0x040037AA RID: 14250
			public int Value_hi;

			// Token: 0x040037AB RID: 14251
			public int NextCounterOffset;

			// Token: 0x040037AC RID: 14252
			public int padding2;
		}

		// Token: 0x0200088C RID: 2188
		private struct ProcessLifetimeEntry
		{
			// Token: 0x040037AD RID: 14253
			public int LifetimeType;

			// Token: 0x040037AE RID: 14254
			public int ProcessId;

			// Token: 0x040037AF RID: 14255
			public long StartupTime;
		}

		// Token: 0x0200088D RID: 2189
		private class CategoryData
		{
			// Token: 0x040037B0 RID: 14256
			public SharedPerformanceCounter.FileMapping FileMapping;

			// Token: 0x040037B1 RID: 14257
			public bool EnableReuse;

			// Token: 0x040037B2 RID: 14258
			public bool UseUniqueSharedMemory;

			// Token: 0x040037B3 RID: 14259
			public string FileMappingName;

			// Token: 0x040037B4 RID: 14260
			public string MutexName;

			// Token: 0x040037B5 RID: 14261
			public ArrayList CounterNames;
		}
	}
}
