using System;
using System.Collections;
using System.Globalization;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace System.Diagnostics
{
	// Token: 0x020004E6 RID: 1254
	internal class CategorySample
	{
		// Token: 0x06002F6A RID: 12138 RVA: 0x000D5EC0 File Offset: 0x000D40C0
		internal unsafe CategorySample(byte[] data, CategoryEntry entry, PerformanceCounterLib library)
		{
			this.entry = entry;
			this.library = library;
			int nameIndex = entry.NameIndex;
			Microsoft.Win32.NativeMethods.PERF_DATA_BLOCK perf_DATA_BLOCK = new Microsoft.Win32.NativeMethods.PERF_DATA_BLOCK();
			fixed (byte[] array = data)
			{
				byte* ptr;
				if (data == null || array.Length == 0)
				{
					ptr = null;
				}
				else
				{
					ptr = &array[0];
				}
				IntPtr intPtr = new IntPtr((void*)ptr);
				IntPtr intPtr2 = intPtr;
				Microsoft.Win32.NativeMethods.PERF_DATA_BLOCK.ValidateBeforeRead(intPtr, data.Length, intPtr2);
				Marshal.PtrToStructure(intPtr2, perf_DATA_BLOCK);
				perf_DATA_BLOCK.Validate(data.Length - (int)((long)intPtr2 - (long)intPtr));
				this.SystemFrequency = perf_DATA_BLOCK.PerfFreq;
				this.TimeStamp = perf_DATA_BLOCK.PerfTime;
				this.TimeStamp100nSec = perf_DATA_BLOCK.PerfTime100nSec;
				intPtr2 = (IntPtr)((long)intPtr2 + (long)perf_DATA_BLOCK.HeaderLength);
				int numObjectTypes = perf_DATA_BLOCK.NumObjectTypes;
				if (numObjectTypes == 0)
				{
					this.CounterTable = new Hashtable();
					this.InstanceNameTable = new Hashtable(StringComparer.OrdinalIgnoreCase);
					return;
				}
				Microsoft.Win32.NativeMethods.PERF_OBJECT_TYPE perf_OBJECT_TYPE = null;
				bool flag = false;
				for (int i = 0; i < numObjectTypes; i++)
				{
					Microsoft.Win32.NativeMethods.PERF_OBJECT_TYPE.ValidateBeforeRead(intPtr, data.Length, intPtr2);
					perf_OBJECT_TYPE = new Microsoft.Win32.NativeMethods.PERF_OBJECT_TYPE();
					Marshal.PtrToStructure(intPtr2, perf_OBJECT_TYPE);
					perf_OBJECT_TYPE.Validate(data.Length - (int)((long)intPtr2 - (long)intPtr));
					if (perf_OBJECT_TYPE.ObjectNameTitleIndex == nameIndex)
					{
						flag = true;
						break;
					}
					intPtr2 = (IntPtr)((long)intPtr2 + (long)perf_OBJECT_TYPE.TotalByteLength);
				}
				if (!flag)
				{
					throw new InvalidOperationException(SR.GetString("CantReadCategoryIndex", new object[] { nameIndex.ToString(CultureInfo.CurrentCulture) }));
				}
				this.CounterFrequency = perf_OBJECT_TYPE.PerfFreq;
				this.CounterTimeStamp = perf_OBJECT_TYPE.PerfTime;
				int numCounters = perf_OBJECT_TYPE.NumCounters;
				int numInstances = perf_OBJECT_TYPE.NumInstances;
				if (numInstances == -1)
				{
					this.IsMultiInstance = false;
				}
				else
				{
					this.IsMultiInstance = true;
				}
				intPtr2 = (IntPtr)((long)intPtr2 + (long)perf_OBJECT_TYPE.HeaderLength);
				CounterDefinitionSample[] array2 = new CounterDefinitionSample[numCounters];
				this.CounterTable = new Hashtable(numCounters);
				for (int j = 0; j < array2.Length; j++)
				{
					Microsoft.Win32.NativeMethods.PERF_COUNTER_DEFINITION.ValidateBeforeRead(intPtr, data.Length, intPtr2);
					Microsoft.Win32.NativeMethods.PERF_COUNTER_DEFINITION perf_COUNTER_DEFINITION = new Microsoft.Win32.NativeMethods.PERF_COUNTER_DEFINITION();
					Marshal.PtrToStructure(intPtr2, perf_COUNTER_DEFINITION);
					perf_COUNTER_DEFINITION.Validate(data.Length - (int)((long)intPtr2 - (long)intPtr));
					array2[j] = new CounterDefinitionSample(perf_COUNTER_DEFINITION, this, numInstances);
					intPtr2 = (IntPtr)((long)intPtr2 + (long)perf_COUNTER_DEFINITION.ByteLength);
					int counterType = array2[j].CounterType;
					if (!PerformanceCounterLib.IsBaseCounter(counterType))
					{
						if (counterType != 1073742336)
						{
							this.CounterTable[array2[j].NameIndex] = array2[j];
						}
					}
					else if (j > 0)
					{
						array2[j - 1].BaseCounterDefinitionSample = array2[j];
					}
				}
				if (!this.IsMultiInstance)
				{
					this.InstanceNameTable = new Hashtable(1, StringComparer.OrdinalIgnoreCase);
					this.InstanceNameTable["systemdiagnosticsperfcounterlibsingleinstance"] = 0;
					for (int k = 0; k < array2.Length; k++)
					{
						array2[k].SetInstanceValue(0, intPtr2);
					}
				}
				else
				{
					string[] array3 = null;
					this.InstanceNameTable = new Hashtable(numInstances, StringComparer.OrdinalIgnoreCase);
					for (int l = 0; l < numInstances; l++)
					{
						Microsoft.Win32.NativeMethods.PERF_INSTANCE_DEFINITION.ValidateBeforeRead(intPtr, data.Length, intPtr2);
						Microsoft.Win32.NativeMethods.PERF_INSTANCE_DEFINITION perf_INSTANCE_DEFINITION = new Microsoft.Win32.NativeMethods.PERF_INSTANCE_DEFINITION();
						Marshal.PtrToStructure(intPtr2, perf_INSTANCE_DEFINITION);
						perf_INSTANCE_DEFINITION.Validate(data.Length - (int)((long)intPtr2 - (long)intPtr));
						if (perf_INSTANCE_DEFINITION.ParentObjectTitleIndex > 0 && array3 == null)
						{
							array3 = this.GetInstanceNamesFromIndex(perf_INSTANCE_DEFINITION.ParentObjectTitleIndex);
						}
						string text;
						if (array3 != null && perf_INSTANCE_DEFINITION.ParentObjectInstance >= 0 && perf_INSTANCE_DEFINITION.ParentObjectInstance < array3.Length - 1)
						{
							text = array3[perf_INSTANCE_DEFINITION.ParentObjectInstance] + "/" + Marshal.PtrToStringUni((IntPtr)((long)intPtr2 + (long)perf_INSTANCE_DEFINITION.NameOffset));
						}
						else
						{
							text = Marshal.PtrToStringUni((IntPtr)((long)intPtr2 + (long)perf_INSTANCE_DEFINITION.NameOffset));
						}
						string text2 = text;
						int num = 1;
						while (this.InstanceNameTable.ContainsKey(text2))
						{
							text2 = text + "#" + num.ToString(CultureInfo.InvariantCulture);
							num++;
						}
						this.InstanceNameTable[text2] = l;
						intPtr2 = (IntPtr)((long)intPtr2 + (long)perf_INSTANCE_DEFINITION.ByteLength);
						for (int m = 0; m < array2.Length; m++)
						{
							array2[m].SetInstanceValue(l, intPtr2);
						}
						Microsoft.Win32.NativeMethods.PERF_COUNTER_BLOCK.ValidateBeforeRead(intPtr, data.Length, intPtr2);
						Microsoft.Win32.NativeMethods.PERF_COUNTER_BLOCK perf_COUNTER_BLOCK = new Microsoft.Win32.NativeMethods.PERF_COUNTER_BLOCK();
						Marshal.PtrToStructure(intPtr2, perf_COUNTER_BLOCK);
						perf_COUNTER_BLOCK.Validate(data.Length - (int)((long)intPtr2 - (long)intPtr));
						intPtr2 = (IntPtr)((long)intPtr2 + (long)perf_COUNTER_BLOCK.ByteLength);
					}
				}
			}
		}

		// Token: 0x06002F6B RID: 12139 RVA: 0x000D6390 File Offset: 0x000D4590
		internal unsafe string[] GetInstanceNamesFromIndex(int categoryIndex)
		{
			byte[] performanceData = this.library.GetPerformanceData(categoryIndex.ToString(CultureInfo.InvariantCulture));
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
			perf_DATA_BLOCK.Validate(performanceData.Length - (int)((long)intPtr2 - (long)intPtr));
			intPtr2 = (IntPtr)((long)intPtr2 + (long)perf_DATA_BLOCK.HeaderLength);
			int numObjectTypes = perf_DATA_BLOCK.NumObjectTypes;
			Microsoft.Win32.NativeMethods.PERF_OBJECT_TYPE perf_OBJECT_TYPE = null;
			bool flag = false;
			for (int i = 0; i < numObjectTypes; i++)
			{
				Microsoft.Win32.NativeMethods.PERF_OBJECT_TYPE.ValidateBeforeRead(intPtr, performanceData.Length, intPtr2);
				perf_OBJECT_TYPE = new Microsoft.Win32.NativeMethods.PERF_OBJECT_TYPE();
				Marshal.PtrToStructure(intPtr2, perf_OBJECT_TYPE);
				perf_OBJECT_TYPE.Validate(performanceData.Length - (int)((long)intPtr2 - (long)intPtr));
				if (perf_OBJECT_TYPE.ObjectNameTitleIndex == categoryIndex)
				{
					flag = true;
					break;
				}
				intPtr2 = (IntPtr)((long)intPtr2 + (long)perf_OBJECT_TYPE.TotalByteLength);
			}
			if (!flag)
			{
				return new string[0];
			}
			int numCounters = perf_OBJECT_TYPE.NumCounters;
			int numInstances = perf_OBJECT_TYPE.NumInstances;
			intPtr2 = (IntPtr)((long)intPtr2 + (long)perf_OBJECT_TYPE.HeaderLength);
			if (numInstances == -1)
			{
				return new string[0];
			}
			CounterDefinitionSample[] array2 = new CounterDefinitionSample[numCounters];
			for (int j = 0; j < array2.Length; j++)
			{
				Microsoft.Win32.NativeMethods.PERF_COUNTER_DEFINITION.ValidateBeforeRead(intPtr, performanceData.Length, intPtr2);
				Microsoft.Win32.NativeMethods.PERF_COUNTER_DEFINITION perf_COUNTER_DEFINITION = new Microsoft.Win32.NativeMethods.PERF_COUNTER_DEFINITION();
				Marshal.PtrToStructure(intPtr2, perf_COUNTER_DEFINITION);
				perf_COUNTER_DEFINITION.Validate(performanceData.Length - (int)((long)intPtr2 - (long)intPtr));
				intPtr2 = (IntPtr)((long)intPtr2 + (long)perf_COUNTER_DEFINITION.ByteLength);
			}
			string[] array3 = new string[numInstances];
			for (int k = 0; k < numInstances; k++)
			{
				Microsoft.Win32.NativeMethods.PERF_INSTANCE_DEFINITION.ValidateBeforeRead(intPtr, performanceData.Length, intPtr2);
				Microsoft.Win32.NativeMethods.PERF_INSTANCE_DEFINITION perf_INSTANCE_DEFINITION = new Microsoft.Win32.NativeMethods.PERF_INSTANCE_DEFINITION();
				Marshal.PtrToStructure(intPtr2, perf_INSTANCE_DEFINITION);
				perf_INSTANCE_DEFINITION.Validate(performanceData.Length - (int)((long)intPtr2 - (long)intPtr));
				array3[k] = Marshal.PtrToStringUni((IntPtr)((long)intPtr2 + (long)perf_INSTANCE_DEFINITION.NameOffset));
				intPtr2 = (IntPtr)((long)intPtr2 + (long)perf_INSTANCE_DEFINITION.ByteLength);
				Microsoft.Win32.NativeMethods.PERF_COUNTER_BLOCK.ValidateBeforeRead(intPtr, performanceData.Length, intPtr2);
				Microsoft.Win32.NativeMethods.PERF_COUNTER_BLOCK perf_COUNTER_BLOCK = new Microsoft.Win32.NativeMethods.PERF_COUNTER_BLOCK();
				Marshal.PtrToStructure(intPtr2, perf_COUNTER_BLOCK);
				perf_COUNTER_BLOCK.Validate(performanceData.Length - (int)((long)intPtr2 - (long)intPtr));
				intPtr2 = (IntPtr)((long)intPtr2 + (long)perf_COUNTER_BLOCK.ByteLength);
			}
			return array3;
		}

		// Token: 0x06002F6C RID: 12140 RVA: 0x000D6620 File Offset: 0x000D4820
		internal CounterDefinitionSample GetCounterDefinitionSample(string counter)
		{
			int i = 0;
			while (i < this.entry.CounterIndexes.Length)
			{
				int num = this.entry.CounterIndexes[i];
				string text = (string)this.library.NameTable[num];
				if (text != null && string.Compare(text, counter, StringComparison.OrdinalIgnoreCase) == 0)
				{
					CounterDefinitionSample counterDefinitionSample = (CounterDefinitionSample)this.CounterTable[num];
					if (counterDefinitionSample == null)
					{
						foreach (object obj in this.CounterTable.Values)
						{
							CounterDefinitionSample counterDefinitionSample2 = (CounterDefinitionSample)obj;
							if (counterDefinitionSample2.BaseCounterDefinitionSample != null && counterDefinitionSample2.BaseCounterDefinitionSample.NameIndex == num)
							{
								return counterDefinitionSample2.BaseCounterDefinitionSample;
							}
						}
						throw new InvalidOperationException(SR.GetString("CounterLayout"));
					}
					return counterDefinitionSample;
				}
				else
				{
					i++;
				}
			}
			throw new InvalidOperationException(SR.GetString("CantReadCounter", new object[] { counter }));
		}

		// Token: 0x06002F6D RID: 12141 RVA: 0x000D6748 File Offset: 0x000D4948
		internal InstanceDataCollectionCollection ReadCategory()
		{
			InstanceDataCollectionCollection instanceDataCollectionCollection = new InstanceDataCollectionCollection();
			for (int i = 0; i < this.entry.CounterIndexes.Length; i++)
			{
				int num = this.entry.CounterIndexes[i];
				string text = (string)this.library.NameTable[num];
				if (text != null && text != string.Empty)
				{
					CounterDefinitionSample counterDefinitionSample = (CounterDefinitionSample)this.CounterTable[num];
					if (counterDefinitionSample != null)
					{
						instanceDataCollectionCollection.Add(text, counterDefinitionSample.ReadInstanceData(text));
					}
				}
			}
			return instanceDataCollectionCollection;
		}

		// Token: 0x040027E7 RID: 10215
		internal readonly long SystemFrequency;

		// Token: 0x040027E8 RID: 10216
		internal readonly long TimeStamp;

		// Token: 0x040027E9 RID: 10217
		internal readonly long TimeStamp100nSec;

		// Token: 0x040027EA RID: 10218
		internal readonly long CounterFrequency;

		// Token: 0x040027EB RID: 10219
		internal readonly long CounterTimeStamp;

		// Token: 0x040027EC RID: 10220
		internal Hashtable CounterTable;

		// Token: 0x040027ED RID: 10221
		internal Hashtable InstanceNameTable;

		// Token: 0x040027EE RID: 10222
		internal bool IsMultiInstance;

		// Token: 0x040027EF RID: 10223
		private CategoryEntry entry;

		// Token: 0x040027F0 RID: 10224
		private PerformanceCounterLib library;
	}
}
