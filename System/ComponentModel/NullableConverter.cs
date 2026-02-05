using System;
using System.Collections;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x02000594 RID: 1428
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class NullableConverter : TypeConverter
	{
		// Token: 0x06003503 RID: 13571 RVA: 0x000E7428 File Offset: 0x000E5628
		public NullableConverter(Type type)
		{
			this.nullableType = type;
			this.simpleType = Nullable.GetUnderlyingType(type);
			if (this.simpleType == null)
			{
				throw new ArgumentException(SR.GetString("NullableConverterBadCtorArg"), "type");
			}
			this.simpleTypeConverter = TypeDescriptor.GetConverter(this.simpleType);
		}

		// Token: 0x06003504 RID: 13572 RVA: 0x000E7482 File Offset: 0x000E5682
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (sourceType == this.simpleType)
			{
				return true;
			}
			if (this.simpleTypeConverter != null)
			{
				return this.simpleTypeConverter.CanConvertFrom(context, sourceType);
			}
			return base.CanConvertFrom(context, sourceType);
		}

		// Token: 0x06003505 RID: 13573 RVA: 0x000E74B4 File Offset: 0x000E56B4
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value == null || value.GetType() == this.simpleType)
			{
				return value;
			}
			if (value is string && string.IsNullOrEmpty(value as string))
			{
				return null;
			}
			if (this.simpleTypeConverter != null)
			{
				return this.simpleTypeConverter.ConvertFrom(context, culture, value);
			}
			return base.ConvertFrom(context, culture, value);
		}

		// Token: 0x06003506 RID: 13574 RVA: 0x000E7514 File Offset: 0x000E5714
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if (destinationType == this.simpleType)
			{
				return true;
			}
			if (destinationType == typeof(InstanceDescriptor))
			{
				return true;
			}
			if (this.simpleTypeConverter != null)
			{
				return this.simpleTypeConverter.CanConvertTo(context, destinationType);
			}
			return base.CanConvertTo(context, destinationType);
		}

		// Token: 0x06003507 RID: 13575 RVA: 0x000E7564 File Offset: 0x000E5764
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (destinationType == this.simpleType && this.nullableType.IsInstanceOfType(value))
			{
				return value;
			}
			if (destinationType == typeof(InstanceDescriptor))
			{
				ConstructorInfo constructor = this.nullableType.GetConstructor(new Type[] { this.simpleType });
				return new InstanceDescriptor(constructor, new object[] { value }, true);
			}
			if (value == null)
			{
				if (destinationType == typeof(string))
				{
					return string.Empty;
				}
			}
			else if (this.simpleTypeConverter != null)
			{
				return this.simpleTypeConverter.ConvertTo(context, culture, value, destinationType);
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		// Token: 0x06003508 RID: 13576 RVA: 0x000E7628 File Offset: 0x000E5828
		public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
		{
			if (this.simpleTypeConverter != null)
			{
				return this.simpleTypeConverter.CreateInstance(context, propertyValues);
			}
			return base.CreateInstance(context, propertyValues);
		}

		// Token: 0x06003509 RID: 13577 RVA: 0x000E7655 File Offset: 0x000E5855
		public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
		{
			if (this.simpleTypeConverter != null)
			{
				return this.simpleTypeConverter.GetCreateInstanceSupported(context);
			}
			return base.GetCreateInstanceSupported(context);
		}

		// Token: 0x0600350A RID: 13578 RVA: 0x000E7674 File Offset: 0x000E5874
		public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
		{
			if (this.simpleTypeConverter != null)
			{
				return this.simpleTypeConverter.GetProperties(context, value, attributes);
			}
			return base.GetProperties(context, value, attributes);
		}

		// Token: 0x0600350B RID: 13579 RVA: 0x000E76A3 File Offset: 0x000E58A3
		public override bool GetPropertiesSupported(ITypeDescriptorContext context)
		{
			if (this.simpleTypeConverter != null)
			{
				return this.simpleTypeConverter.GetPropertiesSupported(context);
			}
			return base.GetPropertiesSupported(context);
		}

		// Token: 0x0600350C RID: 13580 RVA: 0x000E76C4 File Offset: 0x000E58C4
		public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			if (this.simpleTypeConverter != null)
			{
				TypeConverter.StandardValuesCollection standardValues = this.simpleTypeConverter.GetStandardValues(context);
				if (this.GetStandardValuesSupported(context) && standardValues != null)
				{
					object[] array = new object[standardValues.Count + 1];
					int num = 0;
					array[num++] = null;
					foreach (object obj in standardValues)
					{
						array[num++] = obj;
					}
					return new TypeConverter.StandardValuesCollection(array);
				}
			}
			return base.GetStandardValues(context);
		}

		// Token: 0x0600350D RID: 13581 RVA: 0x000E7760 File Offset: 0x000E5960
		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			if (this.simpleTypeConverter != null)
			{
				return this.simpleTypeConverter.GetStandardValuesExclusive(context);
			}
			return base.GetStandardValuesExclusive(context);
		}

		// Token: 0x0600350E RID: 13582 RVA: 0x000E777E File Offset: 0x000E597E
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			if (this.simpleTypeConverter != null)
			{
				return this.simpleTypeConverter.GetStandardValuesSupported(context);
			}
			return base.GetStandardValuesSupported(context);
		}

		// Token: 0x0600350F RID: 13583 RVA: 0x000E779C File Offset: 0x000E599C
		public override bool IsValid(ITypeDescriptorContext context, object value)
		{
			if (this.simpleTypeConverter != null)
			{
				return value == null || this.simpleTypeConverter.IsValid(context, value);
			}
			return base.IsValid(context, value);
		}

		// Token: 0x17000CF5 RID: 3317
		// (get) Token: 0x06003510 RID: 13584 RVA: 0x000E77CE File Offset: 0x000E59CE
		public Type NullableType
		{
			get
			{
				return this.nullableType;
			}
		}

		// Token: 0x17000CF6 RID: 3318
		// (get) Token: 0x06003511 RID: 13585 RVA: 0x000E77D6 File Offset: 0x000E59D6
		public Type UnderlyingType
		{
			get
			{
				return this.simpleType;
			}
		}

		// Token: 0x17000CF7 RID: 3319
		// (get) Token: 0x06003512 RID: 13586 RVA: 0x000E77DE File Offset: 0x000E59DE
		public TypeConverter UnderlyingTypeConverter
		{
			get
			{
				return this.simpleTypeConverter;
			}
		}

		// Token: 0x04002A2A RID: 10794
		private Type nullableType;

		// Token: 0x04002A2B RID: 10795
		private Type simpleType;

		// Token: 0x04002A2C RID: 10796
		private TypeConverter simpleTypeConverter;
	}
}
