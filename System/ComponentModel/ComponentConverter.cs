using System;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x0200052D RID: 1325
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class ComponentConverter : ReferenceConverter
	{
		// Token: 0x06003216 RID: 12822 RVA: 0x000E0842 File Offset: 0x000DEA42
		public ComponentConverter(Type type)
			: base(type)
		{
		}

		// Token: 0x06003217 RID: 12823 RVA: 0x000E084B File Offset: 0x000DEA4B
		public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
		{
			return TypeDescriptor.GetProperties(value, attributes);
		}

		// Token: 0x06003218 RID: 12824 RVA: 0x000E0854 File Offset: 0x000DEA54
		public override bool GetPropertiesSupported(ITypeDescriptorContext context)
		{
			return true;
		}
	}
}
