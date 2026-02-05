using System;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace System.Diagnostics
{
	// Token: 0x020004E7 RID: 1255
	internal class CounterDefinitionSample
	{
		// Token: 0x06002F6E RID: 12142 RVA: 0x000D67DC File Offset: 0x000D49DC
		internal CounterDefinitionSample(Microsoft.Win32.NativeMethods.PERF_COUNTER_DEFINITION perfCounter, CategorySample categorySample, int instanceNumber)
		{
			this.NameIndex = perfCounter.CounterNameTitleIndex;
			this.CounterType = perfCounter.CounterType;
			this.offset = perfCounter.CounterOffset;
			this.size = perfCounter.CounterSize;
			if (instanceNumber == -1)
			{
				this.instanceValues = new long[1];
			}
			else
			{
				this.instanceValues = new long[instanceNumber];
			}
			this.categorySample = categorySample;
		}

		// Token: 0x06002F6F RID: 12143 RVA: 0x000D6844 File Offset: 0x000D4A44
		private long ReadValue(IntPtr pointer)
		{
			if (this.size == 4)
			{
				return (long)((ulong)Marshal.ReadInt32((IntPtr)((long)pointer + (long)this.offset)));
			}
			if (this.size == 8)
			{
				return Marshal.ReadInt64((IntPtr)((long)pointer + (long)this.offset));
			}
			return -1L;
		}

		// Token: 0x06002F70 RID: 12144 RVA: 0x000D6898 File Offset: 0x000D4A98
		internal CounterSample GetInstanceValue(string instanceName)
		{
			if (!this.categorySample.InstanceNameTable.ContainsKey(instanceName))
			{
				if (instanceName.Length > 127)
				{
					instanceName = instanceName.Substring(0, 127);
				}
				if (!this.categorySample.InstanceNameTable.ContainsKey(instanceName))
				{
					throw new InvalidOperationException(SR.GetString("CantReadInstance", new object[] { instanceName }));
				}
			}
			int num = (int)this.categorySample.InstanceNameTable[instanceName];
			long num2 = this.instanceValues[num];
			long num3 = 0L;
			if (this.BaseCounterDefinitionSample != null)
			{
				CategorySample categorySample = this.BaseCounterDefinitionSample.categorySample;
				int num4 = (int)categorySample.InstanceNameTable[instanceName];
				num3 = this.BaseCounterDefinitionSample.instanceValues[num4];
			}
			return new CounterSample(num2, num3, this.categorySample.CounterFrequency, this.categorySample.SystemFrequency, this.categorySample.TimeStamp, this.categorySample.TimeStamp100nSec, (PerformanceCounterType)this.CounterType, this.categorySample.CounterTimeStamp);
		}

		// Token: 0x06002F71 RID: 12145 RVA: 0x000D6998 File Offset: 0x000D4B98
		internal InstanceDataCollection ReadInstanceData(string counterName)
		{
			InstanceDataCollection instanceDataCollection = new InstanceDataCollection(counterName);
			string[] array = new string[this.categorySample.InstanceNameTable.Count];
			this.categorySample.InstanceNameTable.Keys.CopyTo(array, 0);
			int[] array2 = new int[this.categorySample.InstanceNameTable.Count];
			this.categorySample.InstanceNameTable.Values.CopyTo(array2, 0);
			for (int i = 0; i < array.Length; i++)
			{
				long num = 0L;
				if (this.BaseCounterDefinitionSample != null)
				{
					CategorySample categorySample = this.BaseCounterDefinitionSample.categorySample;
					int num2 = (int)categorySample.InstanceNameTable[array[i]];
					num = this.BaseCounterDefinitionSample.instanceValues[num2];
				}
				CounterSample counterSample = new CounterSample(this.instanceValues[array2[i]], num, this.categorySample.CounterFrequency, this.categorySample.SystemFrequency, this.categorySample.TimeStamp, this.categorySample.TimeStamp100nSec, (PerformanceCounterType)this.CounterType, this.categorySample.CounterTimeStamp);
				instanceDataCollection.Add(array[i], new InstanceData(array[i], counterSample));
			}
			return instanceDataCollection;
		}

		// Token: 0x06002F72 RID: 12146 RVA: 0x000D6AC0 File Offset: 0x000D4CC0
		internal CounterSample GetSingleValue()
		{
			long num = this.instanceValues[0];
			long num2 = 0L;
			if (this.BaseCounterDefinitionSample != null)
			{
				num2 = this.BaseCounterDefinitionSample.instanceValues[0];
			}
			return new CounterSample(num, num2, this.categorySample.CounterFrequency, this.categorySample.SystemFrequency, this.categorySample.TimeStamp, this.categorySample.TimeStamp100nSec, (PerformanceCounterType)this.CounterType, this.categorySample.CounterTimeStamp);
		}

		// Token: 0x06002F73 RID: 12147 RVA: 0x000D6B34 File Offset: 0x000D4D34
		internal void SetInstanceValue(int index, IntPtr dataRef)
		{
			long num = this.ReadValue(dataRef);
			this.instanceValues[index] = num;
		}

		// Token: 0x040027F1 RID: 10225
		internal readonly int NameIndex;

		// Token: 0x040027F2 RID: 10226
		internal readonly int CounterType;

		// Token: 0x040027F3 RID: 10227
		internal CounterDefinitionSample BaseCounterDefinitionSample;

		// Token: 0x040027F4 RID: 10228
		private readonly int size;

		// Token: 0x040027F5 RID: 10229
		private readonly int offset;

		// Token: 0x040027F6 RID: 10230
		private long[] instanceValues;

		// Token: 0x040027F7 RID: 10231
		private CategorySample categorySample;
	}
}
