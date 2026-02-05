using System;

namespace System.Diagnostics
{
	// Token: 0x020004D9 RID: 1241
	public class InstanceData
	{
		// Token: 0x06002EDA RID: 11994 RVA: 0x000D2889 File Offset: 0x000D0A89
		public InstanceData(string instanceName, CounterSample sample)
		{
			this.instanceName = instanceName;
			this.sample = sample;
		}

		// Token: 0x17000B64 RID: 2916
		// (get) Token: 0x06002EDB RID: 11995 RVA: 0x000D289F File Offset: 0x000D0A9F
		public string InstanceName
		{
			get
			{
				return this.instanceName;
			}
		}

		// Token: 0x17000B65 RID: 2917
		// (get) Token: 0x06002EDC RID: 11996 RVA: 0x000D28A7 File Offset: 0x000D0AA7
		public CounterSample Sample
		{
			get
			{
				return this.sample;
			}
		}

		// Token: 0x17000B66 RID: 2918
		// (get) Token: 0x06002EDD RID: 11997 RVA: 0x000D28AF File Offset: 0x000D0AAF
		public long RawValue
		{
			get
			{
				return this.sample.RawValue;
			}
		}

		// Token: 0x04002795 RID: 10133
		private string instanceName;

		// Token: 0x04002796 RID: 10134
		private CounterSample sample;
	}
}
