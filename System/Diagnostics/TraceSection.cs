using System;
using System.Configuration;

namespace System.Diagnostics
{
	// Token: 0x020004B6 RID: 1206
	internal class TraceSection : ConfigurationElement
	{
		// Token: 0x06002CFC RID: 11516 RVA: 0x000C9F5C File Offset: 0x000C815C
		static TraceSection()
		{
			TraceSection._properties.Add(TraceSection._propListeners);
			TraceSection._properties.Add(TraceSection._propAutoFlush);
			TraceSection._properties.Add(TraceSection._propIndentSize);
			TraceSection._properties.Add(TraceSection._propUseGlobalLock);
		}

		// Token: 0x17000AE2 RID: 2786
		// (get) Token: 0x06002CFD RID: 11517 RVA: 0x000CA02E File Offset: 0x000C822E
		[ConfigurationProperty("autoflush", DefaultValue = false)]
		public bool AutoFlush
		{
			get
			{
				return (bool)base[TraceSection._propAutoFlush];
			}
		}

		// Token: 0x17000AE3 RID: 2787
		// (get) Token: 0x06002CFE RID: 11518 RVA: 0x000CA040 File Offset: 0x000C8240
		[ConfigurationProperty("indentsize", DefaultValue = 4)]
		public int IndentSize
		{
			get
			{
				return (int)base[TraceSection._propIndentSize];
			}
		}

		// Token: 0x17000AE4 RID: 2788
		// (get) Token: 0x06002CFF RID: 11519 RVA: 0x000CA052 File Offset: 0x000C8252
		[ConfigurationProperty("listeners")]
		public ListenerElementsCollection Listeners
		{
			get
			{
				return (ListenerElementsCollection)base[TraceSection._propListeners];
			}
		}

		// Token: 0x17000AE5 RID: 2789
		// (get) Token: 0x06002D00 RID: 11520 RVA: 0x000CA064 File Offset: 0x000C8264
		[ConfigurationProperty("useGlobalLock", DefaultValue = true)]
		public bool UseGlobalLock
		{
			get
			{
				return (bool)base[TraceSection._propUseGlobalLock];
			}
		}

		// Token: 0x17000AE6 RID: 2790
		// (get) Token: 0x06002D01 RID: 11521 RVA: 0x000CA076 File Offset: 0x000C8276
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return TraceSection._properties;
			}
		}

		// Token: 0x040026F3 RID: 9971
		private static readonly ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x040026F4 RID: 9972
		private static readonly ConfigurationProperty _propListeners = new ConfigurationProperty("listeners", typeof(ListenerElementsCollection), new ListenerElementsCollection(), ConfigurationPropertyOptions.None);

		// Token: 0x040026F5 RID: 9973
		private static readonly ConfigurationProperty _propAutoFlush = new ConfigurationProperty("autoflush", typeof(bool), false, ConfigurationPropertyOptions.None);

		// Token: 0x040026F6 RID: 9974
		private static readonly ConfigurationProperty _propIndentSize = new ConfigurationProperty("indentsize", typeof(int), 4, ConfigurationPropertyOptions.None);

		// Token: 0x040026F7 RID: 9975
		private static readonly ConfigurationProperty _propUseGlobalLock = new ConfigurationProperty("useGlobalLock", typeof(bool), true, ConfigurationPropertyOptions.None);
	}
}
