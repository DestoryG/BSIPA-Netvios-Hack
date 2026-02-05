using System;
using System.Diagnostics;

namespace System.IO.Compression
{
	// Token: 0x0200041A RID: 1050
	internal class CompressionTracingSwitch : Switch
	{
		// Token: 0x0600275E RID: 10078 RVA: 0x000B57A3 File Offset: 0x000B39A3
		internal CompressionTracingSwitch(string displayName, string description)
			: base(displayName, description)
		{
		}

		// Token: 0x170009C2 RID: 2498
		// (get) Token: 0x0600275F RID: 10079 RVA: 0x000B57AD File Offset: 0x000B39AD
		public static bool Verbose
		{
			get
			{
				return CompressionTracingSwitch.tracingSwitch.SwitchSetting >= 2;
			}
		}

		// Token: 0x170009C3 RID: 2499
		// (get) Token: 0x06002760 RID: 10080 RVA: 0x000B57BF File Offset: 0x000B39BF
		public static bool Informational
		{
			get
			{
				return CompressionTracingSwitch.tracingSwitch.SwitchSetting >= 1;
			}
		}

		// Token: 0x0400215D RID: 8541
		internal static readonly CompressionTracingSwitch tracingSwitch = new CompressionTracingSwitch("CompressionSwitch", "Compression Library Tracing Switch");
	}
}
