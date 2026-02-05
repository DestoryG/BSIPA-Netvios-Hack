using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x020005BB RID: 1467
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	[Serializable]
	public class WarningException : SystemException
	{
		// Token: 0x060036FD RID: 14077 RVA: 0x000EF6B1 File Offset: 0x000ED8B1
		public WarningException()
			: this(null, null, null)
		{
		}

		// Token: 0x060036FE RID: 14078 RVA: 0x000EF6BC File Offset: 0x000ED8BC
		public WarningException(string message)
			: this(message, null, null)
		{
		}

		// Token: 0x060036FF RID: 14079 RVA: 0x000EF6C7 File Offset: 0x000ED8C7
		public WarningException(string message, string helpUrl)
			: this(message, helpUrl, null)
		{
		}

		// Token: 0x06003700 RID: 14080 RVA: 0x000EF6D2 File Offset: 0x000ED8D2
		public WarningException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		// Token: 0x06003701 RID: 14081 RVA: 0x000EF6DC File Offset: 0x000ED8DC
		public WarningException(string message, string helpUrl, string helpTopic)
			: base(message)
		{
			this.helpUrl = helpUrl;
			this.helpTopic = helpTopic;
		}

		// Token: 0x06003702 RID: 14082 RVA: 0x000EF6F4 File Offset: 0x000ED8F4
		protected WarningException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			this.helpUrl = (string)info.GetValue("helpUrl", typeof(string));
			this.helpTopic = (string)info.GetValue("helpTopic", typeof(string));
		}

		// Token: 0x17000D40 RID: 3392
		// (get) Token: 0x06003703 RID: 14083 RVA: 0x000EF749 File Offset: 0x000ED949
		public string HelpUrl
		{
			get
			{
				return this.helpUrl;
			}
		}

		// Token: 0x17000D41 RID: 3393
		// (get) Token: 0x06003704 RID: 14084 RVA: 0x000EF751 File Offset: 0x000ED951
		public string HelpTopic
		{
			get
			{
				return this.helpTopic;
			}
		}

		// Token: 0x06003705 RID: 14085 RVA: 0x000EF759 File Offset: 0x000ED959
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			info.AddValue("helpUrl", this.helpUrl);
			info.AddValue("helpTopic", this.helpTopic);
			base.GetObjectData(info, context);
		}

		// Token: 0x04002AB4 RID: 10932
		private readonly string helpUrl;

		// Token: 0x04002AB5 RID: 10933
		private readonly string helpTopic;
	}
}
