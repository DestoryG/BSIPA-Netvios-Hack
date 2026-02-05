using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace BS_Utils.Properties
{
	// Token: 0x02000009 RID: 9
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
	[DebuggerNonUserCode]
	[CompilerGenerated]
	internal class Resources
	{
		// Token: 0x0600009F RID: 159 RVA: 0x000022A0 File Offset: 0x000004A0
		internal Resources()
		{
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x060000A0 RID: 160 RVA: 0x0000427C File Offset: 0x0000247C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (Resources.resourceMan == null)
				{
					ResourceManager resourceManager = new ResourceManager("BS_Utils.Properties.Resources", typeof(Resources).Assembly);
					Resources.resourceMan = resourceManager;
				}
				return Resources.resourceMan;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x060000A1 RID: 161 RVA: 0x000042B5 File Offset: 0x000024B5
		// (set) Token: 0x060000A2 RID: 162 RVA: 0x000042BC File Offset: 0x000024BC
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return Resources.resourceCulture;
			}
			set
			{
				Resources.resourceCulture = value;
			}
		}

		// Token: 0x04000032 RID: 50
		private static ResourceManager resourceMan;

		// Token: 0x04000033 RID: 51
		private static CultureInfo resourceCulture;
	}
}
