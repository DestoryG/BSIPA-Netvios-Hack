using System;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x0200056D RID: 1389
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public abstract class InstanceCreationEditor
	{
		// Token: 0x17000CA6 RID: 3238
		// (get) Token: 0x060033B7 RID: 13239 RVA: 0x000E3D04 File Offset: 0x000E1F04
		public virtual string Text
		{
			get
			{
				return SR.GetString("InstanceCreationEditorDefaultText");
			}
		}

		// Token: 0x060033B8 RID: 13240
		public abstract object CreateInstance(ITypeDescriptorContext context, Type instanceType);
	}
}
