using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Win32;

namespace System.Diagnostics
{
	// Token: 0x020004FB RID: 1275
	internal static class NtProcessInfoHelper
	{
		// Token: 0x06003050 RID: 12368 RVA: 0x000DB050 File Offset: 0x000D9250
		private static int GetNewBufferSize(int existingBufferSize, int requiredSize)
		{
			if (requiredSize == 0)
			{
				int num = existingBufferSize * 2;
				if (num < existingBufferSize)
				{
					throw new OutOfMemoryException();
				}
				return num;
			}
			else
			{
				int num2 = requiredSize + 10240;
				if (num2 < requiredSize)
				{
					throw new OutOfMemoryException();
				}
				return num2;
			}
		}

		// Token: 0x06003051 RID: 12369 RVA: 0x000DB084 File Offset: 0x000D9284
		public static ProcessInfo[] GetProcessInfos(Predicate<int> processIdFilter = null)
		{
			int num = 0;
			GCHandle gchandle = default(GCHandle);
			int num2 = 131072;
			long[] array = Interlocked.Exchange<long[]>(ref NtProcessInfoHelper.CachedBuffer, null);
			ProcessInfo[] processInfos;
			try
			{
				int num3;
				do
				{
					if (array == null)
					{
						array = new long[(num2 + 7) / 8];
					}
					else
					{
						num2 = array.Length * 8;
					}
					gchandle = GCHandle.Alloc(array, GCHandleType.Pinned);
					num3 = Microsoft.Win32.NativeMethods.NtQuerySystemInformation(5, gchandle.AddrOfPinnedObject(), num2, out num);
					if (num3 == -1073741820)
					{
						if (gchandle.IsAllocated)
						{
							gchandle.Free();
						}
						array = null;
						num2 = NtProcessInfoHelper.GetNewBufferSize(num2, num);
					}
				}
				while (num3 == -1073741820);
				if (num3 < 0)
				{
					throw new InvalidOperationException(SR.GetString("CouldntGetProcessInfos"), new Win32Exception(num3));
				}
				processInfos = NtProcessInfoHelper.GetProcessInfos(gchandle.AddrOfPinnedObject(), processIdFilter);
			}
			finally
			{
				Interlocked.Exchange<long[]>(ref NtProcessInfoHelper.CachedBuffer, array);
				if (gchandle.IsAllocated)
				{
					gchandle.Free();
				}
			}
			return processInfos;
		}

		// Token: 0x06003052 RID: 12370 RVA: 0x000DB16C File Offset: 0x000D936C
		private static ProcessInfo[] GetProcessInfos(IntPtr dataPtr, Predicate<int> processIdFilter)
		{
			Hashtable hashtable = new Hashtable(60);
			long num = 0L;
			for (;;)
			{
				IntPtr intPtr = (IntPtr)((long)dataPtr + num);
				NtProcessInfoHelper.SystemProcessInformation systemProcessInformation = new NtProcessInfoHelper.SystemProcessInformation();
				Marshal.PtrToStructure(intPtr, systemProcessInformation);
				int num2 = systemProcessInformation.UniqueProcessId.ToInt32();
				if (processIdFilter == null || processIdFilter(num2))
				{
					ProcessInfo processInfo = new ProcessInfo();
					processInfo.processId = num2;
					processInfo.handleCount = (int)systemProcessInformation.HandleCount;
					processInfo.sessionId = (int)systemProcessInformation.SessionId;
					processInfo.poolPagedBytes = (long)(ulong)systemProcessInformation.QuotaPagedPoolUsage;
					processInfo.poolNonpagedBytes = (long)(ulong)systemProcessInformation.QuotaNonPagedPoolUsage;
					processInfo.virtualBytes = (long)(ulong)systemProcessInformation.VirtualSize;
					processInfo.virtualBytesPeak = (long)(ulong)systemProcessInformation.PeakVirtualSize;
					processInfo.workingSetPeak = (long)(ulong)systemProcessInformation.PeakWorkingSetSize;
					processInfo.workingSet = (long)(ulong)systemProcessInformation.WorkingSetSize;
					processInfo.pageFileBytesPeak = (long)(ulong)systemProcessInformation.PeakPagefileUsage;
					processInfo.pageFileBytes = (long)(ulong)systemProcessInformation.PagefileUsage;
					processInfo.privateBytes = (long)(ulong)systemProcessInformation.PrivatePageCount;
					processInfo.basePriority = systemProcessInformation.BasePriority;
					if (systemProcessInformation.NamePtr == IntPtr.Zero)
					{
						if (processInfo.processId == NtProcessManager.SystemProcessID)
						{
							processInfo.processName = "System";
						}
						else if (processInfo.processId == 0)
						{
							processInfo.processName = "Idle";
						}
						else
						{
							processInfo.processName = processInfo.processId.ToString(CultureInfo.InvariantCulture);
						}
					}
					else
					{
						string text = NtProcessInfoHelper.GetProcessShortName(Marshal.PtrToStringUni(systemProcessInformation.NamePtr, (int)(systemProcessInformation.NameLength / 2)));
						if (ProcessManager.IsOSOlderThanXP && text.Length == 15)
						{
							if (text.EndsWith(".", StringComparison.OrdinalIgnoreCase))
							{
								text = text.Substring(0, 14);
							}
							else if (text.EndsWith(".e", StringComparison.OrdinalIgnoreCase))
							{
								text = text.Substring(0, 13);
							}
							else if (text.EndsWith(".ex", StringComparison.OrdinalIgnoreCase))
							{
								text = text.Substring(0, 12);
							}
						}
						processInfo.processName = text;
					}
					hashtable[processInfo.processId] = processInfo;
					intPtr = (IntPtr)((long)intPtr + (long)Marshal.SizeOf(systemProcessInformation));
					int num3 = 0;
					while ((long)num3 < (long)((ulong)systemProcessInformation.NumberOfThreads))
					{
						NtProcessInfoHelper.SystemThreadInformation systemThreadInformation = new NtProcessInfoHelper.SystemThreadInformation();
						Marshal.PtrToStructure(intPtr, systemThreadInformation);
						ThreadInfo threadInfo = new ThreadInfo();
						threadInfo.processId = (int)systemThreadInformation.UniqueProcess;
						threadInfo.threadId = (int)systemThreadInformation.UniqueThread;
						threadInfo.basePriority = systemThreadInformation.BasePriority;
						threadInfo.currentPriority = systemThreadInformation.Priority;
						threadInfo.startAddress = systemThreadInformation.StartAddress;
						threadInfo.threadState = (ThreadState)systemThreadInformation.ThreadState;
						threadInfo.threadWaitReason = NtProcessManager.GetThreadWaitReason((int)systemThreadInformation.WaitReason);
						processInfo.threadInfoList.Add(threadInfo);
						intPtr = (IntPtr)((long)intPtr + (long)Marshal.SizeOf(systemThreadInformation));
						num3++;
					}
				}
				if (systemProcessInformation.NextEntryOffset == 0U)
				{
					break;
				}
				num += (long)((ulong)systemProcessInformation.NextEntryOffset);
			}
			ProcessInfo[] array = new ProcessInfo[hashtable.Values.Count];
			hashtable.Values.CopyTo(array, 0);
			return array;
		}

		// Token: 0x06003053 RID: 12371 RVA: 0x000DB4C4 File Offset: 0x000D96C4
		internal static string GetProcessShortName(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				return string.Empty;
			}
			int num = -1;
			int num2 = -1;
			for (int i = 0; i < name.Length; i++)
			{
				if (name[i] == '\\')
				{
					num = i;
				}
				else if (name[i] == '.')
				{
					num2 = i;
				}
			}
			if (num2 == -1)
			{
				num2 = name.Length - 1;
			}
			else
			{
				string text = name.Substring(num2);
				if (string.Equals(".exe", text, StringComparison.OrdinalIgnoreCase))
				{
					num2--;
				}
				else
				{
					num2 = name.Length - 1;
				}
			}
			if (num == -1)
			{
				num = 0;
			}
			else
			{
				num++;
			}
			return name.Substring(num, num2 - num + 1);
		}

		// Token: 0x04002885 RID: 10373
		private const int DefaultCachedBufferSize = 131072;

		// Token: 0x04002886 RID: 10374
		private static long[] CachedBuffer;

		// Token: 0x02000884 RID: 2180
		[StructLayout(LayoutKind.Sequential)]
		internal class SystemProcessInformation
		{
			// Token: 0x0400375D RID: 14173
			internal uint NextEntryOffset;

			// Token: 0x0400375E RID: 14174
			internal uint NumberOfThreads;

			// Token: 0x0400375F RID: 14175
			private long SpareLi1;

			// Token: 0x04003760 RID: 14176
			private long SpareLi2;

			// Token: 0x04003761 RID: 14177
			private long SpareLi3;

			// Token: 0x04003762 RID: 14178
			private long CreateTime;

			// Token: 0x04003763 RID: 14179
			private long UserTime;

			// Token: 0x04003764 RID: 14180
			private long KernelTime;

			// Token: 0x04003765 RID: 14181
			internal ushort NameLength;

			// Token: 0x04003766 RID: 14182
			internal ushort MaximumNameLength;

			// Token: 0x04003767 RID: 14183
			internal IntPtr NamePtr;

			// Token: 0x04003768 RID: 14184
			internal int BasePriority;

			// Token: 0x04003769 RID: 14185
			internal IntPtr UniqueProcessId;

			// Token: 0x0400376A RID: 14186
			internal IntPtr InheritedFromUniqueProcessId;

			// Token: 0x0400376B RID: 14187
			internal uint HandleCount;

			// Token: 0x0400376C RID: 14188
			internal uint SessionId;

			// Token: 0x0400376D RID: 14189
			internal UIntPtr PageDirectoryBase;

			// Token: 0x0400376E RID: 14190
			internal UIntPtr PeakVirtualSize;

			// Token: 0x0400376F RID: 14191
			internal UIntPtr VirtualSize;

			// Token: 0x04003770 RID: 14192
			internal uint PageFaultCount;

			// Token: 0x04003771 RID: 14193
			internal UIntPtr PeakWorkingSetSize;

			// Token: 0x04003772 RID: 14194
			internal UIntPtr WorkingSetSize;

			// Token: 0x04003773 RID: 14195
			internal UIntPtr QuotaPeakPagedPoolUsage;

			// Token: 0x04003774 RID: 14196
			internal UIntPtr QuotaPagedPoolUsage;

			// Token: 0x04003775 RID: 14197
			internal UIntPtr QuotaPeakNonPagedPoolUsage;

			// Token: 0x04003776 RID: 14198
			internal UIntPtr QuotaNonPagedPoolUsage;

			// Token: 0x04003777 RID: 14199
			internal UIntPtr PagefileUsage;

			// Token: 0x04003778 RID: 14200
			internal UIntPtr PeakPagefileUsage;

			// Token: 0x04003779 RID: 14201
			internal UIntPtr PrivatePageCount;

			// Token: 0x0400377A RID: 14202
			private long ReadOperationCount;

			// Token: 0x0400377B RID: 14203
			private long WriteOperationCount;

			// Token: 0x0400377C RID: 14204
			private long OtherOperationCount;

			// Token: 0x0400377D RID: 14205
			private long ReadTransferCount;

			// Token: 0x0400377E RID: 14206
			private long WriteTransferCount;

			// Token: 0x0400377F RID: 14207
			private long OtherTransferCount;
		}

		// Token: 0x02000885 RID: 2181
		[StructLayout(LayoutKind.Sequential)]
		internal class SystemThreadInformation
		{
			// Token: 0x04003780 RID: 14208
			private long KernelTime;

			// Token: 0x04003781 RID: 14209
			private long UserTime;

			// Token: 0x04003782 RID: 14210
			private long CreateTime;

			// Token: 0x04003783 RID: 14211
			private uint WaitTime;

			// Token: 0x04003784 RID: 14212
			internal IntPtr StartAddress;

			// Token: 0x04003785 RID: 14213
			internal IntPtr UniqueProcess;

			// Token: 0x04003786 RID: 14214
			internal IntPtr UniqueThread;

			// Token: 0x04003787 RID: 14215
			internal int Priority;

			// Token: 0x04003788 RID: 14216
			internal int BasePriority;

			// Token: 0x04003789 RID: 14217
			internal uint ContextSwitches;

			// Token: 0x0400378A RID: 14218
			internal uint ThreadState;

			// Token: 0x0400378B RID: 14219
			internal uint WaitReason;
		}
	}
}
