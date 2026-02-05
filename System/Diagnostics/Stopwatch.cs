using System;
using Microsoft.Win32;

namespace System.Diagnostics
{
	// Token: 0x02000507 RID: 1287
	[global::__DynamicallyInvokable]
	public class Stopwatch
	{
		// Token: 0x060030F0 RID: 12528 RVA: 0x000DE5EC File Offset: 0x000DC7EC
		static Stopwatch()
		{
			if (!SafeNativeMethods.QueryPerformanceFrequency(out Stopwatch.Frequency))
			{
				Stopwatch.IsHighResolution = false;
				Stopwatch.Frequency = 10000000L;
				Stopwatch.tickFrequency = 1.0;
				return;
			}
			Stopwatch.IsHighResolution = true;
			Stopwatch.tickFrequency = 10000000.0;
			Stopwatch.tickFrequency /= (double)Stopwatch.Frequency;
		}

		// Token: 0x060030F1 RID: 12529 RVA: 0x000DE64C File Offset: 0x000DC84C
		[global::__DynamicallyInvokable]
		public Stopwatch()
		{
			this.Reset();
		}

		// Token: 0x060030F2 RID: 12530 RVA: 0x000DE65A File Offset: 0x000DC85A
		[global::__DynamicallyInvokable]
		public void Start()
		{
			if (!this.isRunning)
			{
				this.startTimeStamp = Stopwatch.GetTimestamp();
				this.isRunning = true;
			}
		}

		// Token: 0x060030F3 RID: 12531 RVA: 0x000DE678 File Offset: 0x000DC878
		[global::__DynamicallyInvokable]
		public static Stopwatch StartNew()
		{
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			return stopwatch;
		}

		// Token: 0x060030F4 RID: 12532 RVA: 0x000DE694 File Offset: 0x000DC894
		[global::__DynamicallyInvokable]
		public void Stop()
		{
			if (this.isRunning)
			{
				long timestamp = Stopwatch.GetTimestamp();
				long num = timestamp - this.startTimeStamp;
				this.elapsed += num;
				this.isRunning = false;
				if (this.elapsed < 0L)
				{
					this.elapsed = 0L;
				}
			}
		}

		// Token: 0x060030F5 RID: 12533 RVA: 0x000DE6DF File Offset: 0x000DC8DF
		[global::__DynamicallyInvokable]
		public void Reset()
		{
			this.elapsed = 0L;
			this.isRunning = false;
			this.startTimeStamp = 0L;
		}

		// Token: 0x060030F6 RID: 12534 RVA: 0x000DE6F8 File Offset: 0x000DC8F8
		[global::__DynamicallyInvokable]
		public void Restart()
		{
			this.elapsed = 0L;
			this.startTimeStamp = Stopwatch.GetTimestamp();
			this.isRunning = true;
		}

		// Token: 0x17000BFA RID: 3066
		// (get) Token: 0x060030F7 RID: 12535 RVA: 0x000DE714 File Offset: 0x000DC914
		[global::__DynamicallyInvokable]
		public bool IsRunning
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.isRunning;
			}
		}

		// Token: 0x17000BFB RID: 3067
		// (get) Token: 0x060030F8 RID: 12536 RVA: 0x000DE71C File Offset: 0x000DC91C
		[global::__DynamicallyInvokable]
		public TimeSpan Elapsed
		{
			[global::__DynamicallyInvokable]
			get
			{
				return new TimeSpan(this.GetElapsedDateTimeTicks());
			}
		}

		// Token: 0x17000BFC RID: 3068
		// (get) Token: 0x060030F9 RID: 12537 RVA: 0x000DE729 File Offset: 0x000DC929
		[global::__DynamicallyInvokable]
		public long ElapsedMilliseconds
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.GetElapsedDateTimeTicks() / 10000L;
			}
		}

		// Token: 0x17000BFD RID: 3069
		// (get) Token: 0x060030FA RID: 12538 RVA: 0x000DE738 File Offset: 0x000DC938
		[global::__DynamicallyInvokable]
		public long ElapsedTicks
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.GetRawElapsedTicks();
			}
		}

		// Token: 0x060030FB RID: 12539 RVA: 0x000DE740 File Offset: 0x000DC940
		[global::__DynamicallyInvokable]
		public static long GetTimestamp()
		{
			if (Stopwatch.IsHighResolution)
			{
				long num = 0L;
				SafeNativeMethods.QueryPerformanceCounter(out num);
				return num;
			}
			return DateTime.UtcNow.Ticks;
		}

		// Token: 0x060030FC RID: 12540 RVA: 0x000DE770 File Offset: 0x000DC970
		private long GetRawElapsedTicks()
		{
			long num = this.elapsed;
			if (this.isRunning)
			{
				long timestamp = Stopwatch.GetTimestamp();
				long num2 = timestamp - this.startTimeStamp;
				num += num2;
			}
			return num;
		}

		// Token: 0x060030FD RID: 12541 RVA: 0x000DE7A0 File Offset: 0x000DC9A0
		private long GetElapsedDateTimeTicks()
		{
			long rawElapsedTicks = this.GetRawElapsedTicks();
			if (Stopwatch.IsHighResolution)
			{
				double num = (double)rawElapsedTicks;
				num *= Stopwatch.tickFrequency;
				return (long)num;
			}
			return rawElapsedTicks;
		}

		// Token: 0x040028D1 RID: 10449
		private const long TicksPerMillisecond = 10000L;

		// Token: 0x040028D2 RID: 10450
		private const long TicksPerSecond = 10000000L;

		// Token: 0x040028D3 RID: 10451
		private long elapsed;

		// Token: 0x040028D4 RID: 10452
		private long startTimeStamp;

		// Token: 0x040028D5 RID: 10453
		private bool isRunning;

		// Token: 0x040028D6 RID: 10454
		[global::__DynamicallyInvokable]
		public static readonly long Frequency;

		// Token: 0x040028D7 RID: 10455
		[global::__DynamicallyInvokable]
		public static readonly bool IsHighResolution;

		// Token: 0x040028D8 RID: 10456
		private static readonly double tickFrequency;
	}
}
