using System;
using System.Threading;

namespace System.Runtime.InteropServices
{
	// Token: 0x020003DA RID: 986
	[global::__DynamicallyInvokable]
	public sealed class HandleCollector
	{
		// Token: 0x060025EC RID: 9708 RVA: 0x000B0161 File Offset: 0x000AE361
		[global::__DynamicallyInvokable]
		public HandleCollector(string name, int initialThreshold)
			: this(name, initialThreshold, int.MaxValue)
		{
		}

		// Token: 0x060025ED RID: 9709 RVA: 0x000B0170 File Offset: 0x000AE370
		[global::__DynamicallyInvokable]
		public HandleCollector(string name, int initialThreshold, int maximumThreshold)
		{
			if (initialThreshold < 0)
			{
				throw new ArgumentOutOfRangeException("initialThreshold", SR.GetString("ArgumentOutOfRange_NeedNonNegNumRequired"));
			}
			if (maximumThreshold < 0)
			{
				throw new ArgumentOutOfRangeException("maximumThreshold", SR.GetString("ArgumentOutOfRange_NeedNonNegNumRequired"));
			}
			if (initialThreshold > maximumThreshold)
			{
				throw new ArgumentException(SR.GetString("Argument_InvalidThreshold"));
			}
			if (name != null)
			{
				this.name = name;
			}
			else
			{
				this.name = string.Empty;
			}
			this.initialThreshold = initialThreshold;
			this.maximumThreshold = maximumThreshold;
			this.threshold = initialThreshold;
			this.handleCount = 0;
		}

		// Token: 0x1700096F RID: 2415
		// (get) Token: 0x060025EE RID: 9710 RVA: 0x000B0208 File Offset: 0x000AE408
		[global::__DynamicallyInvokable]
		public int Count
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.handleCount;
			}
		}

		// Token: 0x17000970 RID: 2416
		// (get) Token: 0x060025EF RID: 9711 RVA: 0x000B0210 File Offset: 0x000AE410
		[global::__DynamicallyInvokable]
		public int InitialThreshold
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.initialThreshold;
			}
		}

		// Token: 0x17000971 RID: 2417
		// (get) Token: 0x060025F0 RID: 9712 RVA: 0x000B0218 File Offset: 0x000AE418
		[global::__DynamicallyInvokable]
		public int MaximumThreshold
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.maximumThreshold;
			}
		}

		// Token: 0x17000972 RID: 2418
		// (get) Token: 0x060025F1 RID: 9713 RVA: 0x000B0220 File Offset: 0x000AE420
		[global::__DynamicallyInvokable]
		public string Name
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.name;
			}
		}

		// Token: 0x060025F2 RID: 9714 RVA: 0x000B0228 File Offset: 0x000AE428
		[global::__DynamicallyInvokable]
		public void Add()
		{
			int num = -1;
			Interlocked.Increment(ref this.handleCount);
			if (this.handleCount < 0)
			{
				throw new InvalidOperationException(SR.GetString("InvalidOperation_HCCountOverflow"));
			}
			if (this.handleCount > this.threshold)
			{
				lock (this)
				{
					this.threshold = this.handleCount + this.handleCount / 10;
					num = this.gc_gen;
					if (this.gc_gen < 2)
					{
						this.gc_gen++;
					}
				}
			}
			if (num >= 0 && (num == 0 || this.gc_counts[num] == GC.CollectionCount(num)))
			{
				GC.Collect(num);
				Thread.Sleep(10 * num);
			}
			for (int i = 1; i < 3; i++)
			{
				this.gc_counts[i] = GC.CollectionCount(i);
			}
		}

		// Token: 0x060025F3 RID: 9715 RVA: 0x000B0308 File Offset: 0x000AE508
		[global::__DynamicallyInvokable]
		public void Remove()
		{
			Interlocked.Decrement(ref this.handleCount);
			if (this.handleCount < 0)
			{
				throw new InvalidOperationException(SR.GetString("InvalidOperation_HCCountOverflow"));
			}
			int num = this.handleCount + this.handleCount / 10;
			if (num < this.threshold - this.threshold / 10)
			{
				lock (this)
				{
					if (num > this.initialThreshold)
					{
						this.threshold = num;
					}
					else
					{
						this.threshold = this.initialThreshold;
					}
					this.gc_gen = 0;
				}
			}
			for (int i = 1; i < 3; i++)
			{
				this.gc_counts[i] = GC.CollectionCount(i);
			}
		}

		// Token: 0x04002070 RID: 8304
		private const int deltaPercent = 10;

		// Token: 0x04002071 RID: 8305
		private string name;

		// Token: 0x04002072 RID: 8306
		private int initialThreshold;

		// Token: 0x04002073 RID: 8307
		private int maximumThreshold;

		// Token: 0x04002074 RID: 8308
		private int threshold;

		// Token: 0x04002075 RID: 8309
		private int handleCount;

		// Token: 0x04002076 RID: 8310
		private int[] gc_counts = new int[3];

		// Token: 0x04002077 RID: 8311
		private int gc_gen;
	}
}
