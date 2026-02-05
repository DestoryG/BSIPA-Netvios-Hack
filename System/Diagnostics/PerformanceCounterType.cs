using System;
using System.ComponentModel;

namespace System.Diagnostics
{
	// Token: 0x020004EE RID: 1262
	[TypeConverter(typeof(AlphabeticalEnumConverter))]
	public enum PerformanceCounterType
	{
		// Token: 0x04002808 RID: 10248
		NumberOfItems32 = 65536,
		// Token: 0x04002809 RID: 10249
		NumberOfItems64 = 65792,
		// Token: 0x0400280A RID: 10250
		NumberOfItemsHEX32 = 0,
		// Token: 0x0400280B RID: 10251
		NumberOfItemsHEX64 = 256,
		// Token: 0x0400280C RID: 10252
		RateOfCountsPerSecond32 = 272696320,
		// Token: 0x0400280D RID: 10253
		RateOfCountsPerSecond64 = 272696576,
		// Token: 0x0400280E RID: 10254
		CountPerTimeInterval32 = 4523008,
		// Token: 0x0400280F RID: 10255
		CountPerTimeInterval64 = 4523264,
		// Token: 0x04002810 RID: 10256
		RawFraction = 537003008,
		// Token: 0x04002811 RID: 10257
		RawBase = 1073939459,
		// Token: 0x04002812 RID: 10258
		AverageTimer32 = 805438464,
		// Token: 0x04002813 RID: 10259
		AverageBase = 1073939458,
		// Token: 0x04002814 RID: 10260
		AverageCount64 = 1073874176,
		// Token: 0x04002815 RID: 10261
		SampleFraction = 549585920,
		// Token: 0x04002816 RID: 10262
		SampleCounter = 4260864,
		// Token: 0x04002817 RID: 10263
		SampleBase = 1073939457,
		// Token: 0x04002818 RID: 10264
		CounterTimer = 541132032,
		// Token: 0x04002819 RID: 10265
		CounterTimerInverse = 557909248,
		// Token: 0x0400281A RID: 10266
		Timer100Ns = 542180608,
		// Token: 0x0400281B RID: 10267
		Timer100NsInverse = 558957824,
		// Token: 0x0400281C RID: 10268
		ElapsedTime = 807666944,
		// Token: 0x0400281D RID: 10269
		CounterMultiTimer = 574686464,
		// Token: 0x0400281E RID: 10270
		CounterMultiTimerInverse = 591463680,
		// Token: 0x0400281F RID: 10271
		CounterMultiTimer100Ns = 575735040,
		// Token: 0x04002820 RID: 10272
		CounterMultiTimer100NsInverse = 592512256,
		// Token: 0x04002821 RID: 10273
		CounterMultiBase = 1107494144,
		// Token: 0x04002822 RID: 10274
		CounterDelta32 = 4195328,
		// Token: 0x04002823 RID: 10275
		CounterDelta64 = 4195584
	}
}
