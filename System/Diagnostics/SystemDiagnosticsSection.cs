using System;
using System.Configuration;

namespace System.Diagnostics
{
	// Token: 0x020004AB RID: 1195
	internal class SystemDiagnosticsSection : ConfigurationSection
	{
		// Token: 0x06002C36 RID: 11318 RVA: 0x000C7570 File Offset: 0x000C5770
		static SystemDiagnosticsSection()
		{
			SystemDiagnosticsSection._properties.Add(SystemDiagnosticsSection._propAssert);
			SystemDiagnosticsSection._properties.Add(SystemDiagnosticsSection._propPerfCounters);
			SystemDiagnosticsSection._properties.Add(SystemDiagnosticsSection._propSources);
			SystemDiagnosticsSection._properties.Add(SystemDiagnosticsSection._propSharedListeners);
			SystemDiagnosticsSection._properties.Add(SystemDiagnosticsSection._propSwitches);
			SystemDiagnosticsSection._properties.Add(SystemDiagnosticsSection._propTrace);
		}

		// Token: 0x17000AB7 RID: 2743
		// (get) Token: 0x06002C37 RID: 11319 RVA: 0x000C769B File Offset: 0x000C589B
		[ConfigurationProperty("assert")]
		public AssertSection Assert
		{
			get
			{
				return (AssertSection)base[SystemDiagnosticsSection._propAssert];
			}
		}

		// Token: 0x17000AB8 RID: 2744
		// (get) Token: 0x06002C38 RID: 11320 RVA: 0x000C76AD File Offset: 0x000C58AD
		[ConfigurationProperty("performanceCounters")]
		public PerfCounterSection PerfCounters
		{
			get
			{
				return (PerfCounterSection)base[SystemDiagnosticsSection._propPerfCounters];
			}
		}

		// Token: 0x17000AB9 RID: 2745
		// (get) Token: 0x06002C39 RID: 11321 RVA: 0x000C76BF File Offset: 0x000C58BF
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return SystemDiagnosticsSection._properties;
			}
		}

		// Token: 0x17000ABA RID: 2746
		// (get) Token: 0x06002C3A RID: 11322 RVA: 0x000C76C6 File Offset: 0x000C58C6
		[ConfigurationProperty("sources")]
		public SourceElementsCollection Sources
		{
			get
			{
				return (SourceElementsCollection)base[SystemDiagnosticsSection._propSources];
			}
		}

		// Token: 0x17000ABB RID: 2747
		// (get) Token: 0x06002C3B RID: 11323 RVA: 0x000C76D8 File Offset: 0x000C58D8
		[ConfigurationProperty("sharedListeners")]
		public ListenerElementsCollection SharedListeners
		{
			get
			{
				return (ListenerElementsCollection)base[SystemDiagnosticsSection._propSharedListeners];
			}
		}

		// Token: 0x17000ABC RID: 2748
		// (get) Token: 0x06002C3C RID: 11324 RVA: 0x000C76EA File Offset: 0x000C58EA
		[ConfigurationProperty("switches")]
		public SwitchElementsCollection Switches
		{
			get
			{
				return (SwitchElementsCollection)base[SystemDiagnosticsSection._propSwitches];
			}
		}

		// Token: 0x17000ABD RID: 2749
		// (get) Token: 0x06002C3D RID: 11325 RVA: 0x000C76FC File Offset: 0x000C58FC
		[ConfigurationProperty("trace")]
		public TraceSection Trace
		{
			get
			{
				return (TraceSection)base[SystemDiagnosticsSection._propTrace];
			}
		}

		// Token: 0x06002C3E RID: 11326 RVA: 0x000C770E File Offset: 0x000C590E
		protected override void InitializeDefault()
		{
			this.Trace.Listeners.InitializeDefaultInternal();
		}

		// Token: 0x040026B8 RID: 9912
		private static readonly ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x040026B9 RID: 9913
		private static readonly ConfigurationProperty _propAssert = new ConfigurationProperty("assert", typeof(AssertSection), new AssertSection(), ConfigurationPropertyOptions.None);

		// Token: 0x040026BA RID: 9914
		private static readonly ConfigurationProperty _propPerfCounters = new ConfigurationProperty("performanceCounters", typeof(PerfCounterSection), new PerfCounterSection(), ConfigurationPropertyOptions.None);

		// Token: 0x040026BB RID: 9915
		private static readonly ConfigurationProperty _propSources = new ConfigurationProperty("sources", typeof(SourceElementsCollection), new SourceElementsCollection(), ConfigurationPropertyOptions.None);

		// Token: 0x040026BC RID: 9916
		private static readonly ConfigurationProperty _propSharedListeners = new ConfigurationProperty("sharedListeners", typeof(SharedListenerElementsCollection), new SharedListenerElementsCollection(), ConfigurationPropertyOptions.None);

		// Token: 0x040026BD RID: 9917
		private static readonly ConfigurationProperty _propSwitches = new ConfigurationProperty("switches", typeof(SwitchElementsCollection), new SwitchElementsCollection(), ConfigurationPropertyOptions.None);

		// Token: 0x040026BE RID: 9918
		private static readonly ConfigurationProperty _propTrace = new ConfigurationProperty("trace", typeof(TraceSection), new TraceSection(), ConfigurationPropertyOptions.None);
	}
}
