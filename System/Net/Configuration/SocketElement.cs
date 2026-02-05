using System;
using System.Configuration;
using System.Net.Sockets;

namespace System.Net.Configuration
{
	// Token: 0x02000348 RID: 840
	public sealed class SocketElement : ConfigurationElement
	{
		// Token: 0x06001E1F RID: 7711 RVA: 0x0008D618 File Offset: 0x0008B818
		public SocketElement()
		{
			this.properties.Add(this.alwaysUseCompletionPortsForAccept);
			this.properties.Add(this.alwaysUseCompletionPortsForConnect);
			this.properties.Add(this.ipProtectionLevel);
		}

		// Token: 0x06001E20 RID: 7712 RVA: 0x0008D6CC File Offset: 0x0008B8CC
		protected override void PostDeserialize()
		{
			if (base.EvaluationContext.IsMachineLevel)
			{
				return;
			}
			try
			{
				ExceptionHelper.UnrestrictedSocketPermission.Demand();
			}
			catch (Exception ex)
			{
				throw new ConfigurationErrorsException(SR.GetString("net_config_element_permission", new object[] { "socket" }), ex);
			}
		}

		// Token: 0x170007C9 RID: 1993
		// (get) Token: 0x06001E21 RID: 7713 RVA: 0x0008D724 File Offset: 0x0008B924
		// (set) Token: 0x06001E22 RID: 7714 RVA: 0x0008D737 File Offset: 0x0008B937
		[ConfigurationProperty("alwaysUseCompletionPortsForAccept", DefaultValue = false)]
		public bool AlwaysUseCompletionPortsForAccept
		{
			get
			{
				return (bool)base[this.alwaysUseCompletionPortsForAccept];
			}
			set
			{
				base[this.alwaysUseCompletionPortsForAccept] = value;
			}
		}

		// Token: 0x170007CA RID: 1994
		// (get) Token: 0x06001E23 RID: 7715 RVA: 0x0008D74B File Offset: 0x0008B94B
		// (set) Token: 0x06001E24 RID: 7716 RVA: 0x0008D75E File Offset: 0x0008B95E
		[ConfigurationProperty("alwaysUseCompletionPortsForConnect", DefaultValue = false)]
		public bool AlwaysUseCompletionPortsForConnect
		{
			get
			{
				return (bool)base[this.alwaysUseCompletionPortsForConnect];
			}
			set
			{
				base[this.alwaysUseCompletionPortsForConnect] = value;
			}
		}

		// Token: 0x170007CB RID: 1995
		// (get) Token: 0x06001E25 RID: 7717 RVA: 0x0008D772 File Offset: 0x0008B972
		// (set) Token: 0x06001E26 RID: 7718 RVA: 0x0008D785 File Offset: 0x0008B985
		[ConfigurationProperty("ipProtectionLevel", DefaultValue = IPProtectionLevel.Unspecified)]
		public IPProtectionLevel IPProtectionLevel
		{
			get
			{
				return (IPProtectionLevel)base[this.ipProtectionLevel];
			}
			set
			{
				base[this.ipProtectionLevel] = value;
			}
		}

		// Token: 0x170007CC RID: 1996
		// (get) Token: 0x06001E27 RID: 7719 RVA: 0x0008D799 File Offset: 0x0008B999
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x04001CA8 RID: 7336
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04001CA9 RID: 7337
		private readonly ConfigurationProperty alwaysUseCompletionPortsForConnect = new ConfigurationProperty("alwaysUseCompletionPortsForConnect", typeof(bool), false, ConfigurationPropertyOptions.None);

		// Token: 0x04001CAA RID: 7338
		private readonly ConfigurationProperty alwaysUseCompletionPortsForAccept = new ConfigurationProperty("alwaysUseCompletionPortsForAccept", typeof(bool), false, ConfigurationPropertyOptions.None);

		// Token: 0x04001CAB RID: 7339
		private readonly ConfigurationProperty ipProtectionLevel = new ConfigurationProperty("ipProtectionLevel", typeof(IPProtectionLevel), IPProtectionLevel.Unspecified, ConfigurationPropertyOptions.None);
	}
}
