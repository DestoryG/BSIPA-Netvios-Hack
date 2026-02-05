using System;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x020005B7 RID: 1463
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public abstract class TypeListConverter : TypeConverter
	{
		// Token: 0x060036E3 RID: 14051 RVA: 0x000EF3DD File Offset: 0x000ED5DD
		protected TypeListConverter(Type[] types)
		{
			this.types = types;
		}

		// Token: 0x060036E4 RID: 14052 RVA: 0x000EF3EC File Offset: 0x000ED5EC
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		// Token: 0x060036E5 RID: 14053 RVA: 0x000EF40A File Offset: 0x000ED60A
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);
		}

		// Token: 0x060036E6 RID: 14054 RVA: 0x000EF428 File Offset: 0x000ED628
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is string)
			{
				foreach (Type type in this.types)
				{
					if (value.Equals(type.FullName))
					{
						return type;
					}
				}
			}
			return base.ConvertFrom(context, culture, value);
		}

		// Token: 0x060036E7 RID: 14055 RVA: 0x000EF470 File Offset: 0x000ED670
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (!(destinationType == typeof(string)))
			{
				if (destinationType == typeof(InstanceDescriptor) && value is Type)
				{
					MethodInfo method = typeof(Type).GetMethod("GetType", new Type[] { typeof(string) });
					if (method != null)
					{
						return new InstanceDescriptor(method, new object[] { ((Type)value).AssemblyQualifiedName });
					}
				}
				return base.ConvertTo(context, culture, value, destinationType);
			}
			if (value == null)
			{
				return SR.GetString("toStringNone");
			}
			return ((Type)value).FullName;
		}

		// Token: 0x060036E8 RID: 14056 RVA: 0x000EF534 File Offset: 0x000ED734
		public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			if (this.values == null)
			{
				object[] array;
				if (this.types != null)
				{
					array = new object[this.types.Length];
					Array.Copy(this.types, array, this.types.Length);
				}
				else
				{
					array = null;
				}
				this.values = new TypeConverter.StandardValuesCollection(array);
			}
			return this.values;
		}

		// Token: 0x060036E9 RID: 14057 RVA: 0x000EF589 File Offset: 0x000ED789
		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return true;
		}

		// Token: 0x060036EA RID: 14058 RVA: 0x000EF58C File Offset: 0x000ED78C
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		// Token: 0x04002AB2 RID: 10930
		private Type[] types;

		// Token: 0x04002AB3 RID: 10931
		private TypeConverter.StandardValuesCollection values;
	}
}
