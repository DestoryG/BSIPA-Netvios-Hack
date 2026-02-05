using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace System.Security.Authentication.ExtendedProtection
{
	// Token: 0x02000443 RID: 1091
	public class ExtendedProtectionPolicyTypeConverter : TypeConverter
	{
		// Token: 0x0600288D RID: 10381 RVA: 0x000BA199 File Offset: 0x000B8399
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);
		}

		// Token: 0x0600288E RID: 10382 RVA: 0x000BA1B8 File Offset: 0x000B83B8
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == typeof(InstanceDescriptor))
			{
				ExtendedProtectionPolicy extendedProtectionPolicy = value as ExtendedProtectionPolicy;
				if (extendedProtectionPolicy != null)
				{
					Type[] array;
					object[] array2;
					if (extendedProtectionPolicy.PolicyEnforcement == PolicyEnforcement.Never)
					{
						array = new Type[] { typeof(PolicyEnforcement) };
						array2 = new object[] { PolicyEnforcement.Never };
					}
					else
					{
						array = new Type[]
						{
							typeof(PolicyEnforcement),
							typeof(ProtectionScenario),
							typeof(ICollection)
						};
						object[] array3 = null;
						if (extendedProtectionPolicy.CustomServiceNames != null && extendedProtectionPolicy.CustomServiceNames.Count > 0)
						{
							array3 = new object[extendedProtectionPolicy.CustomServiceNames.Count];
							((ICollection)extendedProtectionPolicy.CustomServiceNames).CopyTo(array3, 0);
						}
						array2 = new object[] { extendedProtectionPolicy.PolicyEnforcement, extendedProtectionPolicy.ProtectionScenario, array3 };
					}
					ConstructorInfo constructor = typeof(ExtendedProtectionPolicy).GetConstructor(array);
					return new InstanceDescriptor(constructor, array2);
				}
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}
