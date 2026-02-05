using System;
using System.Collections;
using System.ComponentModel.Design.Serialization;
using System.Configuration;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x020005B2 RID: 1458
	[ComVisible(true)]
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class TypeConverter
	{
		// Token: 0x17000D36 RID: 3382
		// (get) Token: 0x06003635 RID: 13877 RVA: 0x000EC6C0 File Offset: 0x000EA8C0
		private static bool UseCompatibleTypeConversion
		{
			get
			{
				if (TypeConverter.firstLoadAppSetting)
				{
					object obj = TypeConverter.loadAppSettingLock;
					lock (obj)
					{
						if (TypeConverter.firstLoadAppSetting)
						{
							string text = ConfigurationManager.AppSettings["UseCompatibleTypeConverterBehavior"];
							try
							{
								if (!string.IsNullOrEmpty(text))
								{
									TypeConverter.useCompatibleTypeConversion = bool.Parse(text.Trim());
								}
							}
							catch
							{
								TypeConverter.useCompatibleTypeConversion = false;
							}
							TypeConverter.firstLoadAppSetting = false;
						}
					}
				}
				return TypeConverter.useCompatibleTypeConversion;
			}
		}

		// Token: 0x06003636 RID: 13878 RVA: 0x000EC760 File Offset: 0x000EA960
		public bool CanConvertFrom(Type sourceType)
		{
			return this.CanConvertFrom(null, sourceType);
		}

		// Token: 0x06003637 RID: 13879 RVA: 0x000EC76A File Offset: 0x000EA96A
		public virtual bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(InstanceDescriptor);
		}

		// Token: 0x06003638 RID: 13880 RVA: 0x000EC781 File Offset: 0x000EA981
		public bool CanConvertTo(Type destinationType)
		{
			return this.CanConvertTo(null, destinationType);
		}

		// Token: 0x06003639 RID: 13881 RVA: 0x000EC78B File Offset: 0x000EA98B
		public virtual bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(string);
		}

		// Token: 0x0600363A RID: 13882 RVA: 0x000EC79D File Offset: 0x000EA99D
		public object ConvertFrom(object value)
		{
			return this.ConvertFrom(null, CultureInfo.CurrentCulture, value);
		}

		// Token: 0x0600363B RID: 13883 RVA: 0x000EC7AC File Offset: 0x000EA9AC
		public virtual object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			InstanceDescriptor instanceDescriptor = value as InstanceDescriptor;
			if (instanceDescriptor != null)
			{
				return instanceDescriptor.Invoke();
			}
			throw this.GetConvertFromException(value);
		}

		// Token: 0x0600363C RID: 13884 RVA: 0x000EC7D1 File Offset: 0x000EA9D1
		public object ConvertFromInvariantString(string text)
		{
			return this.ConvertFromString(null, CultureInfo.InvariantCulture, text);
		}

		// Token: 0x0600363D RID: 13885 RVA: 0x000EC7E0 File Offset: 0x000EA9E0
		public object ConvertFromInvariantString(ITypeDescriptorContext context, string text)
		{
			return this.ConvertFromString(context, CultureInfo.InvariantCulture, text);
		}

		// Token: 0x0600363E RID: 13886 RVA: 0x000EC7EF File Offset: 0x000EA9EF
		public object ConvertFromString(string text)
		{
			return this.ConvertFrom(null, null, text);
		}

		// Token: 0x0600363F RID: 13887 RVA: 0x000EC7FA File Offset: 0x000EA9FA
		public object ConvertFromString(ITypeDescriptorContext context, string text)
		{
			return this.ConvertFrom(context, CultureInfo.CurrentCulture, text);
		}

		// Token: 0x06003640 RID: 13888 RVA: 0x000EC809 File Offset: 0x000EAA09
		public object ConvertFromString(ITypeDescriptorContext context, CultureInfo culture, string text)
		{
			return this.ConvertFrom(context, culture, text);
		}

		// Token: 0x06003641 RID: 13889 RVA: 0x000EC814 File Offset: 0x000EAA14
		public object ConvertTo(object value, Type destinationType)
		{
			return this.ConvertTo(null, null, value, destinationType);
		}

		// Token: 0x06003642 RID: 13890 RVA: 0x000EC820 File Offset: 0x000EAA20
		public virtual object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (!(destinationType == typeof(string)))
			{
				throw this.GetConvertToException(value, destinationType);
			}
			if (value == null)
			{
				return string.Empty;
			}
			if (culture != null && culture != CultureInfo.CurrentCulture)
			{
				IFormattable formattable = value as IFormattable;
				if (formattable != null)
				{
					return formattable.ToString(null, culture);
				}
			}
			return value.ToString();
		}

		// Token: 0x06003643 RID: 13891 RVA: 0x000EC88C File Offset: 0x000EAA8C
		public string ConvertToInvariantString(object value)
		{
			return this.ConvertToString(null, CultureInfo.InvariantCulture, value);
		}

		// Token: 0x06003644 RID: 13892 RVA: 0x000EC89B File Offset: 0x000EAA9B
		public string ConvertToInvariantString(ITypeDescriptorContext context, object value)
		{
			return this.ConvertToString(context, CultureInfo.InvariantCulture, value);
		}

		// Token: 0x06003645 RID: 13893 RVA: 0x000EC8AA File Offset: 0x000EAAAA
		public string ConvertToString(object value)
		{
			return (string)this.ConvertTo(null, CultureInfo.CurrentCulture, value, typeof(string));
		}

		// Token: 0x06003646 RID: 13894 RVA: 0x000EC8C8 File Offset: 0x000EAAC8
		public string ConvertToString(ITypeDescriptorContext context, object value)
		{
			return (string)this.ConvertTo(context, CultureInfo.CurrentCulture, value, typeof(string));
		}

		// Token: 0x06003647 RID: 13895 RVA: 0x000EC8E6 File Offset: 0x000EAAE6
		public string ConvertToString(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			return (string)this.ConvertTo(context, culture, value, typeof(string));
		}

		// Token: 0x06003648 RID: 13896 RVA: 0x000EC900 File Offset: 0x000EAB00
		public object CreateInstance(IDictionary propertyValues)
		{
			return this.CreateInstance(null, propertyValues);
		}

		// Token: 0x06003649 RID: 13897 RVA: 0x000EC90A File Offset: 0x000EAB0A
		public virtual object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
		{
			return null;
		}

		// Token: 0x0600364A RID: 13898 RVA: 0x000EC910 File Offset: 0x000EAB10
		protected Exception GetConvertFromException(object value)
		{
			string text;
			if (value == null)
			{
				text = SR.GetString("ToStringNull");
			}
			else
			{
				text = value.GetType().FullName;
			}
			throw new NotSupportedException(SR.GetString("ConvertFromException", new object[]
			{
				base.GetType().Name,
				text
			}));
		}

		// Token: 0x0600364B RID: 13899 RVA: 0x000EC960 File Offset: 0x000EAB60
		protected Exception GetConvertToException(object value, Type destinationType)
		{
			string text;
			if (value == null)
			{
				text = SR.GetString("ToStringNull");
			}
			else
			{
				text = value.GetType().FullName;
			}
			throw new NotSupportedException(SR.GetString("ConvertToException", new object[]
			{
				base.GetType().Name,
				text,
				destinationType.FullName
			}));
		}

		// Token: 0x0600364C RID: 13900 RVA: 0x000EC9B9 File Offset: 0x000EABB9
		public bool GetCreateInstanceSupported()
		{
			return this.GetCreateInstanceSupported(null);
		}

		// Token: 0x0600364D RID: 13901 RVA: 0x000EC9C2 File Offset: 0x000EABC2
		public virtual bool GetCreateInstanceSupported(ITypeDescriptorContext context)
		{
			return false;
		}

		// Token: 0x0600364E RID: 13902 RVA: 0x000EC9C5 File Offset: 0x000EABC5
		public PropertyDescriptorCollection GetProperties(object value)
		{
			return this.GetProperties(null, value);
		}

		// Token: 0x0600364F RID: 13903 RVA: 0x000EC9CF File Offset: 0x000EABCF
		public PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value)
		{
			return this.GetProperties(context, value, new Attribute[] { BrowsableAttribute.Yes });
		}

		// Token: 0x06003650 RID: 13904 RVA: 0x000EC9E7 File Offset: 0x000EABE7
		public virtual PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
		{
			return null;
		}

		// Token: 0x06003651 RID: 13905 RVA: 0x000EC9EA File Offset: 0x000EABEA
		public bool GetPropertiesSupported()
		{
			return this.GetPropertiesSupported(null);
		}

		// Token: 0x06003652 RID: 13906 RVA: 0x000EC9F3 File Offset: 0x000EABF3
		public virtual bool GetPropertiesSupported(ITypeDescriptorContext context)
		{
			return false;
		}

		// Token: 0x06003653 RID: 13907 RVA: 0x000EC9F6 File Offset: 0x000EABF6
		public ICollection GetStandardValues()
		{
			return this.GetStandardValues(null);
		}

		// Token: 0x06003654 RID: 13908 RVA: 0x000EC9FF File Offset: 0x000EABFF
		public virtual TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			return null;
		}

		// Token: 0x06003655 RID: 13909 RVA: 0x000ECA02 File Offset: 0x000EAC02
		public bool GetStandardValuesExclusive()
		{
			return this.GetStandardValuesExclusive(null);
		}

		// Token: 0x06003656 RID: 13910 RVA: 0x000ECA0B File Offset: 0x000EAC0B
		public virtual bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return false;
		}

		// Token: 0x06003657 RID: 13911 RVA: 0x000ECA0E File Offset: 0x000EAC0E
		public bool GetStandardValuesSupported()
		{
			return this.GetStandardValuesSupported(null);
		}

		// Token: 0x06003658 RID: 13912 RVA: 0x000ECA17 File Offset: 0x000EAC17
		public virtual bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return false;
		}

		// Token: 0x06003659 RID: 13913 RVA: 0x000ECA1A File Offset: 0x000EAC1A
		public bool IsValid(object value)
		{
			return this.IsValid(null, value);
		}

		// Token: 0x0600365A RID: 13914 RVA: 0x000ECA24 File Offset: 0x000EAC24
		public virtual bool IsValid(ITypeDescriptorContext context, object value)
		{
			if (TypeConverter.UseCompatibleTypeConversion)
			{
				return true;
			}
			bool flag = true;
			try
			{
				if (value == null || this.CanConvertFrom(context, value.GetType()))
				{
					this.ConvertFrom(context, CultureInfo.InvariantCulture, value);
				}
				else
				{
					flag = false;
				}
			}
			catch
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x0600365B RID: 13915 RVA: 0x000ECA78 File Offset: 0x000EAC78
		protected PropertyDescriptorCollection SortProperties(PropertyDescriptorCollection props, string[] names)
		{
			props.Sort(names);
			return props;
		}

		// Token: 0x04002A97 RID: 10903
		private const string s_UseCompatibleTypeConverterBehavior = "UseCompatibleTypeConverterBehavior";

		// Token: 0x04002A98 RID: 10904
		private static volatile bool useCompatibleTypeConversion = false;

		// Token: 0x04002A99 RID: 10905
		private static volatile bool firstLoadAppSetting = true;

		// Token: 0x04002A9A RID: 10906
		private static object loadAppSettingLock = new object();

		// Token: 0x0200089E RID: 2206
		protected abstract class SimplePropertyDescriptor : PropertyDescriptor
		{
			// Token: 0x060045BC RID: 17852 RVA: 0x00123CC6 File Offset: 0x00121EC6
			protected SimplePropertyDescriptor(Type componentType, string name, Type propertyType)
				: this(componentType, name, propertyType, new Attribute[0])
			{
			}

			// Token: 0x060045BD RID: 17853 RVA: 0x00123CD7 File Offset: 0x00121ED7
			protected SimplePropertyDescriptor(Type componentType, string name, Type propertyType, Attribute[] attributes)
				: base(name, attributes)
			{
				this.componentType = componentType;
				this.propertyType = propertyType;
			}

			// Token: 0x17000FC5 RID: 4037
			// (get) Token: 0x060045BE RID: 17854 RVA: 0x00123CF0 File Offset: 0x00121EF0
			public override Type ComponentType
			{
				get
				{
					return this.componentType;
				}
			}

			// Token: 0x17000FC6 RID: 4038
			// (get) Token: 0x060045BF RID: 17855 RVA: 0x00123CF8 File Offset: 0x00121EF8
			public override bool IsReadOnly
			{
				get
				{
					return this.Attributes.Contains(ReadOnlyAttribute.Yes);
				}
			}

			// Token: 0x17000FC7 RID: 4039
			// (get) Token: 0x060045C0 RID: 17856 RVA: 0x00123D0A File Offset: 0x00121F0A
			public override Type PropertyType
			{
				get
				{
					return this.propertyType;
				}
			}

			// Token: 0x060045C1 RID: 17857 RVA: 0x00123D14 File Offset: 0x00121F14
			public override bool CanResetValue(object component)
			{
				DefaultValueAttribute defaultValueAttribute = (DefaultValueAttribute)this.Attributes[typeof(DefaultValueAttribute)];
				return defaultValueAttribute != null && defaultValueAttribute.Value.Equals(this.GetValue(component));
			}

			// Token: 0x060045C2 RID: 17858 RVA: 0x00123D54 File Offset: 0x00121F54
			public override void ResetValue(object component)
			{
				DefaultValueAttribute defaultValueAttribute = (DefaultValueAttribute)this.Attributes[typeof(DefaultValueAttribute)];
				if (defaultValueAttribute != null)
				{
					this.SetValue(component, defaultValueAttribute.Value);
				}
			}

			// Token: 0x060045C3 RID: 17859 RVA: 0x00123D8C File Offset: 0x00121F8C
			public override bool ShouldSerializeValue(object component)
			{
				return false;
			}

			// Token: 0x040037E6 RID: 14310
			private Type componentType;

			// Token: 0x040037E7 RID: 14311
			private Type propertyType;
		}

		// Token: 0x0200089F RID: 2207
		public class StandardValuesCollection : ICollection, IEnumerable
		{
			// Token: 0x060045C4 RID: 17860 RVA: 0x00123D90 File Offset: 0x00121F90
			public StandardValuesCollection(ICollection values)
			{
				if (values == null)
				{
					values = new object[0];
				}
				Array array = values as Array;
				if (array != null)
				{
					this.valueArray = array;
				}
				this.values = values;
			}

			// Token: 0x17000FC8 RID: 4040
			// (get) Token: 0x060045C5 RID: 17861 RVA: 0x00123DC6 File Offset: 0x00121FC6
			public int Count
			{
				get
				{
					if (this.valueArray != null)
					{
						return this.valueArray.Length;
					}
					return this.values.Count;
				}
			}

			// Token: 0x17000FC9 RID: 4041
			public object this[int index]
			{
				get
				{
					if (this.valueArray != null)
					{
						return this.valueArray.GetValue(index);
					}
					IList list = this.values as IList;
					if (list != null)
					{
						return list[index];
					}
					this.valueArray = new object[this.values.Count];
					this.values.CopyTo(this.valueArray, 0);
					return this.valueArray.GetValue(index);
				}
			}

			// Token: 0x060045C7 RID: 17863 RVA: 0x00123E55 File Offset: 0x00122055
			public void CopyTo(Array array, int index)
			{
				this.values.CopyTo(array, index);
			}

			// Token: 0x060045C8 RID: 17864 RVA: 0x00123E64 File Offset: 0x00122064
			public IEnumerator GetEnumerator()
			{
				return this.values.GetEnumerator();
			}

			// Token: 0x17000FCA RID: 4042
			// (get) Token: 0x060045C9 RID: 17865 RVA: 0x00123E71 File Offset: 0x00122071
			int ICollection.Count
			{
				get
				{
					return this.Count;
				}
			}

			// Token: 0x17000FCB RID: 4043
			// (get) Token: 0x060045CA RID: 17866 RVA: 0x00123E79 File Offset: 0x00122079
			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17000FCC RID: 4044
			// (get) Token: 0x060045CB RID: 17867 RVA: 0x00123E7C File Offset: 0x0012207C
			object ICollection.SyncRoot
			{
				get
				{
					return null;
				}
			}

			// Token: 0x060045CC RID: 17868 RVA: 0x00123E7F File Offset: 0x0012207F
			void ICollection.CopyTo(Array array, int index)
			{
				this.CopyTo(array, index);
			}

			// Token: 0x060045CD RID: 17869 RVA: 0x00123E89 File Offset: 0x00122089
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			// Token: 0x040037E8 RID: 14312
			private ICollection values;

			// Token: 0x040037E9 RID: 14313
			private Array valueArray;
		}
	}
}
