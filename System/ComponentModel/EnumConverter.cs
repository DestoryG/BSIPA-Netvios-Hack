using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x0200054E RID: 1358
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class EnumConverter : TypeConverter
	{
		// Token: 0x060032F1 RID: 13041 RVA: 0x000E2A4D File Offset: 0x000E0C4D
		public EnumConverter(Type type)
		{
			this.type = type;
		}

		// Token: 0x17000C74 RID: 3188
		// (get) Token: 0x060032F2 RID: 13042 RVA: 0x000E2A5C File Offset: 0x000E0C5C
		protected Type EnumType
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x17000C75 RID: 3189
		// (get) Token: 0x060032F3 RID: 13043 RVA: 0x000E2A64 File Offset: 0x000E0C64
		// (set) Token: 0x060032F4 RID: 13044 RVA: 0x000E2A6C File Offset: 0x000E0C6C
		protected TypeConverter.StandardValuesCollection Values
		{
			get
			{
				return this.values;
			}
			set
			{
				this.values = value;
			}
		}

		// Token: 0x060032F5 RID: 13045 RVA: 0x000E2A75 File Offset: 0x000E0C75
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || sourceType == typeof(Enum[]) || base.CanConvertFrom(context, sourceType);
		}

		// Token: 0x060032F6 RID: 13046 RVA: 0x000E2AA5 File Offset: 0x000E0CA5
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || destinationType == typeof(Enum[]) || base.CanConvertTo(context, destinationType);
		}

		// Token: 0x17000C76 RID: 3190
		// (get) Token: 0x060032F7 RID: 13047 RVA: 0x000E2AD5 File Offset: 0x000E0CD5
		protected virtual IComparer Comparer
		{
			get
			{
				return InvariantComparer.Default;
			}
		}

		// Token: 0x060032F8 RID: 13048 RVA: 0x000E2ADC File Offset: 0x000E0CDC
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is string)
			{
				try
				{
					string text = (string)value;
					if (text.IndexOf(',') != -1)
					{
						long num = 0L;
						string[] array = text.Split(new char[] { ',' });
						foreach (string text2 in array)
						{
							num |= Convert.ToInt64((Enum)Enum.Parse(this.type, text2, true), culture);
						}
						return Enum.ToObject(this.type, num);
					}
					return Enum.Parse(this.type, text, true);
				}
				catch (Exception ex)
				{
					throw new FormatException(SR.GetString("ConvertInvalidPrimitive", new object[]
					{
						(string)value,
						this.type.Name
					}), ex);
				}
			}
			if (value is Enum[])
			{
				long num2 = 0L;
				foreach (Enum @enum in (Enum[])value)
				{
					num2 |= Convert.ToInt64(@enum, culture);
				}
				return Enum.ToObject(this.type, num2);
			}
			return base.ConvertFrom(context, culture, value);
		}

		// Token: 0x060032F9 RID: 13049 RVA: 0x000E2C10 File Offset: 0x000E0E10
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (destinationType == typeof(string) && value != null)
			{
				Type underlyingType = Enum.GetUnderlyingType(this.type);
				if (value is IConvertible && value.GetType() != underlyingType)
				{
					value = ((IConvertible)value).ToType(underlyingType, culture);
				}
				if (!this.type.IsDefined(typeof(FlagsAttribute), false) && !Enum.IsDefined(this.type, value))
				{
					throw new ArgumentException(SR.GetString("EnumConverterInvalidValue", new object[]
					{
						value.ToString(),
						this.type.Name
					}));
				}
				return Enum.Format(this.type, value, "G");
			}
			else
			{
				if (destinationType == typeof(InstanceDescriptor) && value != null)
				{
					string text = base.ConvertToInvariantString(context, value);
					if (this.type.IsDefined(typeof(FlagsAttribute), false) && text.IndexOf(',') != -1)
					{
						Type underlyingType2 = Enum.GetUnderlyingType(this.type);
						if (value is IConvertible)
						{
							object obj = ((IConvertible)value).ToType(underlyingType2, culture);
							MethodInfo method = typeof(Enum).GetMethod("ToObject", new Type[]
							{
								typeof(Type),
								underlyingType2
							});
							if (method != null)
							{
								return new InstanceDescriptor(method, new object[] { this.type, obj });
							}
						}
					}
					else
					{
						FieldInfo field = this.type.GetField(text);
						if (field != null)
						{
							return new InstanceDescriptor(field, null);
						}
					}
				}
				if (!(destinationType == typeof(Enum[])) || value == null)
				{
					return base.ConvertTo(context, culture, value, destinationType);
				}
				if (this.type.IsDefined(typeof(FlagsAttribute), false))
				{
					List<Enum> list = new List<Enum>();
					Array array = Enum.GetValues(this.type);
					long[] array2 = new long[array.Length];
					for (int i = 0; i < array.Length; i++)
					{
						array2[i] = Convert.ToInt64((Enum)array.GetValue(i), culture);
					}
					long num = Convert.ToInt64((Enum)value, culture);
					bool flag = true;
					while (flag)
					{
						flag = false;
						foreach (long num2 in array2)
						{
							if ((num2 != 0L && (num2 & num) == num2) || num2 == num)
							{
								list.Add((Enum)Enum.ToObject(this.type, num2));
								flag = true;
								num &= ~num2;
								break;
							}
						}
						if (num == 0L)
						{
							break;
						}
					}
					if (!flag && num != 0L)
					{
						list.Add((Enum)Enum.ToObject(this.type, num));
					}
					return list.ToArray();
				}
				return new Enum[] { (Enum)Enum.ToObject(this.type, value) };
			}
		}

		// Token: 0x060032FA RID: 13050 RVA: 0x000E2F14 File Offset: 0x000E1114
		public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			if (this.values == null)
			{
				Type reflectionType = TypeDescriptor.GetReflectionType(this.type);
				if (reflectionType == null)
				{
					reflectionType = this.type;
				}
				FieldInfo[] fields = reflectionType.GetFields(BindingFlags.Static | BindingFlags.Public);
				ArrayList arrayList = null;
				if (fields != null && fields.Length != 0)
				{
					arrayList = new ArrayList(fields.Length);
				}
				if (arrayList != null)
				{
					foreach (FieldInfo fieldInfo in fields)
					{
						BrowsableAttribute browsableAttribute = null;
						foreach (Attribute attribute in fieldInfo.GetCustomAttributes(typeof(BrowsableAttribute), false))
						{
							browsableAttribute = attribute as BrowsableAttribute;
						}
						if (browsableAttribute == null || browsableAttribute.Browsable)
						{
							object obj = null;
							try
							{
								if (fieldInfo.Name != null)
								{
									obj = Enum.Parse(this.type, fieldInfo.Name);
								}
							}
							catch (ArgumentException)
							{
							}
							if (obj != null)
							{
								arrayList.Add(obj);
							}
						}
					}
					IComparer comparer = this.Comparer;
					if (comparer != null)
					{
						arrayList.Sort(comparer);
					}
				}
				Array array2 = ((arrayList != null) ? arrayList.ToArray() : null);
				this.values = new TypeConverter.StandardValuesCollection(array2);
			}
			return this.values;
		}

		// Token: 0x060032FB RID: 13051 RVA: 0x000E3050 File Offset: 0x000E1250
		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return !this.type.IsDefined(typeof(FlagsAttribute), false);
		}

		// Token: 0x060032FC RID: 13052 RVA: 0x000E306B File Offset: 0x000E126B
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		// Token: 0x060032FD RID: 13053 RVA: 0x000E306E File Offset: 0x000E126E
		public override bool IsValid(ITypeDescriptorContext context, object value)
		{
			return Enum.IsDefined(this.type, value);
		}

		// Token: 0x040029A0 RID: 10656
		private TypeConverter.StandardValuesCollection values;

		// Token: 0x040029A1 RID: 10657
		private Type type;
	}
}
