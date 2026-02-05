using System;
using System.Globalization;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x0200050F RID: 1295
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class ArrayConverter : CollectionConverter
	{
		// Token: 0x06003115 RID: 12565 RVA: 0x000DE94C File Offset: 0x000DCB4C
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (destinationType == typeof(string) && value is Array)
			{
				return SR.GetString("ArrayConverterText", new object[] { value.GetType().Name });
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		// Token: 0x06003116 RID: 12566 RVA: 0x000DE9B4 File Offset: 0x000DCBB4
		public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
		{
			PropertyDescriptor[] array = null;
			if (value.GetType().IsArray)
			{
				Array array2 = (Array)value;
				int length = array2.GetLength(0);
				array = new PropertyDescriptor[length];
				Type type = value.GetType();
				Type elementType = type.GetElementType();
				for (int i = 0; i < length; i++)
				{
					array[i] = new ArrayConverter.ArrayPropertyDescriptor(type, elementType, i);
				}
			}
			return new PropertyDescriptorCollection(array);
		}

		// Token: 0x06003117 RID: 12567 RVA: 0x000DEA19 File Offset: 0x000DCC19
		public override bool GetPropertiesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		// Token: 0x0200088E RID: 2190
		private class ArrayPropertyDescriptor : TypeConverter.SimplePropertyDescriptor
		{
			// Token: 0x0600457F RID: 17791 RVA: 0x00121C20 File Offset: 0x0011FE20
			public ArrayPropertyDescriptor(Type arrayType, Type elementType, int index)
				: base(arrayType, "[" + index.ToString() + "]", elementType, null)
			{
				this.index = index;
			}

			// Token: 0x06004580 RID: 17792 RVA: 0x00121C48 File Offset: 0x0011FE48
			public override object GetValue(object instance)
			{
				if (instance is Array)
				{
					Array array = (Array)instance;
					if (array.GetLength(0) > this.index)
					{
						return array.GetValue(this.index);
					}
				}
				return null;
			}

			// Token: 0x06004581 RID: 17793 RVA: 0x00121C84 File Offset: 0x0011FE84
			public override void SetValue(object instance, object value)
			{
				if (instance is Array)
				{
					Array array = (Array)instance;
					if (array.GetLength(0) > this.index)
					{
						array.SetValue(value, this.index);
					}
					this.OnValueChanged(instance, EventArgs.Empty);
				}
			}

			// Token: 0x040037B6 RID: 14262
			private int index;
		}
	}
}
