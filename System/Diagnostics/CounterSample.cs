using System;

namespace System.Diagnostics
{
	// Token: 0x020004C2 RID: 1218
	public struct CounterSample
	{
		// Token: 0x06002D7A RID: 11642 RVA: 0x000CC77D File Offset: 0x000CA97D
		public CounterSample(long rawValue, long baseValue, long counterFrequency, long systemFrequency, long timeStamp, long timeStamp100nSec, PerformanceCounterType counterType)
		{
			this.rawValue = rawValue;
			this.baseValue = baseValue;
			this.timeStamp = timeStamp;
			this.counterFrequency = counterFrequency;
			this.counterType = counterType;
			this.timeStamp100nSec = timeStamp100nSec;
			this.systemFrequency = systemFrequency;
			this.counterTimeStamp = 0L;
		}

		// Token: 0x06002D7B RID: 11643 RVA: 0x000CC7BC File Offset: 0x000CA9BC
		public CounterSample(long rawValue, long baseValue, long counterFrequency, long systemFrequency, long timeStamp, long timeStamp100nSec, PerformanceCounterType counterType, long counterTimeStamp)
		{
			this.rawValue = rawValue;
			this.baseValue = baseValue;
			this.timeStamp = timeStamp;
			this.counterFrequency = counterFrequency;
			this.counterType = counterType;
			this.timeStamp100nSec = timeStamp100nSec;
			this.systemFrequency = systemFrequency;
			this.counterTimeStamp = counterTimeStamp;
		}

		// Token: 0x17000AF9 RID: 2809
		// (get) Token: 0x06002D7C RID: 11644 RVA: 0x000CC7FB File Offset: 0x000CA9FB
		public long RawValue
		{
			get
			{
				return this.rawValue;
			}
		}

		// Token: 0x17000AFA RID: 2810
		// (get) Token: 0x06002D7D RID: 11645 RVA: 0x000CC803 File Offset: 0x000CAA03
		internal ulong UnsignedRawValue
		{
			get
			{
				return (ulong)this.rawValue;
			}
		}

		// Token: 0x17000AFB RID: 2811
		// (get) Token: 0x06002D7E RID: 11646 RVA: 0x000CC80B File Offset: 0x000CAA0B
		public long BaseValue
		{
			get
			{
				return this.baseValue;
			}
		}

		// Token: 0x17000AFC RID: 2812
		// (get) Token: 0x06002D7F RID: 11647 RVA: 0x000CC813 File Offset: 0x000CAA13
		public long SystemFrequency
		{
			get
			{
				return this.systemFrequency;
			}
		}

		// Token: 0x17000AFD RID: 2813
		// (get) Token: 0x06002D80 RID: 11648 RVA: 0x000CC81B File Offset: 0x000CAA1B
		public long CounterFrequency
		{
			get
			{
				return this.counterFrequency;
			}
		}

		// Token: 0x17000AFE RID: 2814
		// (get) Token: 0x06002D81 RID: 11649 RVA: 0x000CC823 File Offset: 0x000CAA23
		public long CounterTimeStamp
		{
			get
			{
				return this.counterTimeStamp;
			}
		}

		// Token: 0x17000AFF RID: 2815
		// (get) Token: 0x06002D82 RID: 11650 RVA: 0x000CC82B File Offset: 0x000CAA2B
		public long TimeStamp
		{
			get
			{
				return this.timeStamp;
			}
		}

		// Token: 0x17000B00 RID: 2816
		// (get) Token: 0x06002D83 RID: 11651 RVA: 0x000CC833 File Offset: 0x000CAA33
		public long TimeStamp100nSec
		{
			get
			{
				return this.timeStamp100nSec;
			}
		}

		// Token: 0x17000B01 RID: 2817
		// (get) Token: 0x06002D84 RID: 11652 RVA: 0x000CC83B File Offset: 0x000CAA3B
		public PerformanceCounterType CounterType
		{
			get
			{
				return this.counterType;
			}
		}

		// Token: 0x06002D85 RID: 11653 RVA: 0x000CC843 File Offset: 0x000CAA43
		public static float Calculate(CounterSample counterSample)
		{
			return CounterSampleCalculator.ComputeCounterValue(counterSample);
		}

		// Token: 0x06002D86 RID: 11654 RVA: 0x000CC84B File Offset: 0x000CAA4B
		public static float Calculate(CounterSample counterSample, CounterSample nextCounterSample)
		{
			return CounterSampleCalculator.ComputeCounterValue(counterSample, nextCounterSample);
		}

		// Token: 0x06002D87 RID: 11655 RVA: 0x000CC854 File Offset: 0x000CAA54
		public override bool Equals(object o)
		{
			return o is CounterSample && this.Equals((CounterSample)o);
		}

		// Token: 0x06002D88 RID: 11656 RVA: 0x000CC86C File Offset: 0x000CAA6C
		public bool Equals(CounterSample sample)
		{
			return this.rawValue == sample.rawValue && this.baseValue == sample.baseValue && this.timeStamp == sample.timeStamp && this.counterFrequency == sample.counterFrequency && this.counterType == sample.counterType && this.timeStamp100nSec == sample.timeStamp100nSec && this.systemFrequency == sample.systemFrequency && this.counterTimeStamp == sample.counterTimeStamp;
		}

		// Token: 0x06002D89 RID: 11657 RVA: 0x000CC8EB File Offset: 0x000CAAEB
		public override int GetHashCode()
		{
			return this.rawValue.GetHashCode();
		}

		// Token: 0x06002D8A RID: 11658 RVA: 0x000CC8F8 File Offset: 0x000CAAF8
		public static bool operator ==(CounterSample a, CounterSample b)
		{
			return a.Equals(b);
		}

		// Token: 0x06002D8B RID: 11659 RVA: 0x000CC902 File Offset: 0x000CAB02
		public static bool operator !=(CounterSample a, CounterSample b)
		{
			return !a.Equals(b);
		}

		// Token: 0x0400271D RID: 10013
		private long rawValue;

		// Token: 0x0400271E RID: 10014
		private long baseValue;

		// Token: 0x0400271F RID: 10015
		private long timeStamp;

		// Token: 0x04002720 RID: 10016
		private long counterFrequency;

		// Token: 0x04002721 RID: 10017
		private PerformanceCounterType counterType;

		// Token: 0x04002722 RID: 10018
		private long timeStamp100nSec;

		// Token: 0x04002723 RID: 10019
		private long systemFrequency;

		// Token: 0x04002724 RID: 10020
		private long counterTimeStamp;

		// Token: 0x04002725 RID: 10021
		public static CounterSample Empty = new CounterSample(0L, 0L, 0L, 0L, 0L, 0L, PerformanceCounterType.NumberOfItems32);
	}
}
