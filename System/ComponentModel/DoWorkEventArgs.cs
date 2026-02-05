using System;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x02000549 RID: 1353
	[global::__DynamicallyInvokable]
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class DoWorkEventArgs : CancelEventArgs
	{
		// Token: 0x060032DB RID: 13019 RVA: 0x000E2881 File Offset: 0x000E0A81
		[global::__DynamicallyInvokable]
		public DoWorkEventArgs(object argument)
		{
			this.argument = argument;
		}

		// Token: 0x17000C6E RID: 3182
		// (get) Token: 0x060032DC RID: 13020 RVA: 0x000E2890 File Offset: 0x000E0A90
		[SRDescription("BackgroundWorker_DoWorkEventArgs_Argument")]
		[global::__DynamicallyInvokable]
		public object Argument
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.argument;
			}
		}

		// Token: 0x17000C6F RID: 3183
		// (get) Token: 0x060032DD RID: 13021 RVA: 0x000E2898 File Offset: 0x000E0A98
		// (set) Token: 0x060032DE RID: 13022 RVA: 0x000E28A0 File Offset: 0x000E0AA0
		[SRDescription("BackgroundWorker_DoWorkEventArgs_Result")]
		[global::__DynamicallyInvokable]
		public object Result
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.result;
			}
			[global::__DynamicallyInvokable]
			set
			{
				this.result = value;
			}
		}

		// Token: 0x04002996 RID: 10646
		private object result;

		// Token: 0x04002997 RID: 10647
		private object argument;
	}
}
