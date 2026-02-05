using System;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x02000552 RID: 1362
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class ExpandableObjectConverter : TypeConverter
	{
		// Token: 0x06003338 RID: 13112 RVA: 0x000E382A File Offset: 0x000E1A2A
		public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
		{
			return TypeDescriptor.GetProperties(value, attributes);
		}

		// Token: 0x06003339 RID: 13113 RVA: 0x000E3833 File Offset: 0x000E1A33
		public override bool GetPropertiesSupported(ITypeDescriptorContext context)
		{
			return true;
		}
	}
}
