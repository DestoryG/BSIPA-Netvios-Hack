using System;
using System.ComponentModel;

namespace System.Diagnostics
{
	// Token: 0x020004C0 RID: 1216
	[TypeConverter("System.Diagnostics.Design.CounterCreationDataConverter, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[Serializable]
	public class CounterCreationData
	{
		// Token: 0x06002D64 RID: 11620 RVA: 0x000CC564 File Offset: 0x000CA764
		public CounterCreationData()
		{
		}

		// Token: 0x06002D65 RID: 11621 RVA: 0x000CC58D File Offset: 0x000CA78D
		public CounterCreationData(string counterName, string counterHelp, PerformanceCounterType counterType)
		{
			this.CounterType = counterType;
			this.CounterName = counterName;
			this.CounterHelp = counterHelp;
		}

		// Token: 0x17000AF5 RID: 2805
		// (get) Token: 0x06002D66 RID: 11622 RVA: 0x000CC5CB File Offset: 0x000CA7CB
		// (set) Token: 0x06002D67 RID: 11623 RVA: 0x000CC5D3 File Offset: 0x000CA7D3
		[DefaultValue(PerformanceCounterType.NumberOfItems32)]
		[MonitoringDescription("CounterType")]
		public PerformanceCounterType CounterType
		{
			get
			{
				return this.counterType;
			}
			set
			{
				if (!Enum.IsDefined(typeof(PerformanceCounterType), value))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(PerformanceCounterType));
				}
				this.counterType = value;
			}
		}

		// Token: 0x17000AF6 RID: 2806
		// (get) Token: 0x06002D68 RID: 11624 RVA: 0x000CC609 File Offset: 0x000CA809
		// (set) Token: 0x06002D69 RID: 11625 RVA: 0x000CC611 File Offset: 0x000CA811
		[DefaultValue("")]
		[MonitoringDescription("CounterName")]
		[TypeConverter("System.Diagnostics.Design.StringValueConverter, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		public string CounterName
		{
			get
			{
				return this.counterName;
			}
			set
			{
				PerformanceCounterCategory.CheckValidCounter(value);
				this.counterName = value;
			}
		}

		// Token: 0x17000AF7 RID: 2807
		// (get) Token: 0x06002D6A RID: 11626 RVA: 0x000CC620 File Offset: 0x000CA820
		// (set) Token: 0x06002D6B RID: 11627 RVA: 0x000CC628 File Offset: 0x000CA828
		[DefaultValue("")]
		[MonitoringDescription("CounterHelp")]
		public string CounterHelp
		{
			get
			{
				return this.counterHelp;
			}
			set
			{
				PerformanceCounterCategory.CheckValidHelp(value);
				this.counterHelp = value;
			}
		}

		// Token: 0x0400271A RID: 10010
		private PerformanceCounterType counterType = PerformanceCounterType.NumberOfItems32;

		// Token: 0x0400271B RID: 10011
		private string counterName = string.Empty;

		// Token: 0x0400271C RID: 10012
		private string counterHelp = string.Empty;
	}
}
