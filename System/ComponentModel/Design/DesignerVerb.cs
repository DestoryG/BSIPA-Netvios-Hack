using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text.RegularExpressions;

namespace System.ComponentModel.Design
{
	// Token: 0x020005D8 RID: 1496
	[ComVisible(true)]
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public class DesignerVerb : MenuCommand
	{
		// Token: 0x06003798 RID: 14232 RVA: 0x000F061F File Offset: 0x000EE81F
		public DesignerVerb(string text, EventHandler handler)
			: base(handler, StandardCommands.VerbFirst)
		{
			this.Properties["Text"] = ((text == null) ? null : Regex.Replace(text, "\\(\\&.\\)", ""));
		}

		// Token: 0x06003799 RID: 14233 RVA: 0x000F0653 File Offset: 0x000EE853
		public DesignerVerb(string text, EventHandler handler, CommandID startCommandID)
			: base(handler, startCommandID)
		{
			this.Properties["Text"] = ((text == null) ? null : Regex.Replace(text, "\\(\\&.\\)", ""));
		}

		// Token: 0x17000D60 RID: 3424
		// (get) Token: 0x0600379A RID: 14234 RVA: 0x000F0684 File Offset: 0x000EE884
		// (set) Token: 0x0600379B RID: 14235 RVA: 0x000F06B1 File Offset: 0x000EE8B1
		public string Description
		{
			get
			{
				object obj = this.Properties["Description"];
				if (obj == null)
				{
					return string.Empty;
				}
				return (string)obj;
			}
			set
			{
				this.Properties["Description"] = value;
			}
		}

		// Token: 0x17000D61 RID: 3425
		// (get) Token: 0x0600379C RID: 14236 RVA: 0x000F06C4 File Offset: 0x000EE8C4
		public string Text
		{
			get
			{
				object obj = this.Properties["Text"];
				if (obj == null)
				{
					return string.Empty;
				}
				return (string)obj;
			}
		}

		// Token: 0x0600379D RID: 14237 RVA: 0x000F06F1 File Offset: 0x000EE8F1
		public override string ToString()
		{
			return this.Text + " : " + base.ToString();
		}
	}
}
