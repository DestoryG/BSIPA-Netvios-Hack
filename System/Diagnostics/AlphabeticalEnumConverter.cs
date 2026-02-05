using System;
using System.Collections;
using System.ComponentModel;

namespace System.Diagnostics
{
	// Token: 0x020004BD RID: 1213
	internal class AlphabeticalEnumConverter : EnumConverter
	{
		// Token: 0x06002D50 RID: 11600 RVA: 0x000CBFFF File Offset: 0x000CA1FF
		public AlphabeticalEnumConverter(Type type)
			: base(type)
		{
		}

		// Token: 0x06002D51 RID: 11601 RVA: 0x000CC008 File Offset: 0x000CA208
		public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			if (base.Values == null)
			{
				Array values = Enum.GetValues(base.EnumType);
				object[] array = new object[values.Length];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = this.ConvertTo(context, null, values.GetValue(i), typeof(string));
				}
				Array.Sort(array, values, 0, values.Length, global::System.Collections.Comparer.Default);
				base.Values = new TypeConverter.StandardValuesCollection(values);
			}
			return base.Values;
		}
	}
}
