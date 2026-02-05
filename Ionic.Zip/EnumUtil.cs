using System;
using System.ComponentModel;
using System.Reflection;

namespace Ionic
{
	// Token: 0x0200001A RID: 26
	internal sealed class EnumUtil
	{
		// Token: 0x060000D1 RID: 209 RVA: 0x00004F36 File Offset: 0x00003136
		private EnumUtil()
		{
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00004F40 File Offset: 0x00003140
		internal static string GetDescription(Enum value)
		{
			FieldInfo field = value.GetType().GetField(value.ToString());
			DescriptionAttribute[] array = (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), false);
			if (array.Length > 0)
			{
				return array[0].Description;
			}
			return value.ToString();
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00004F8B File Offset: 0x0000318B
		internal static object Parse(Type enumType, string stringRepresentation)
		{
			return EnumUtil.Parse(enumType, stringRepresentation, false);
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00004F98 File Offset: 0x00003198
		internal static object Parse(Type enumType, string stringRepresentation, bool ignoreCase)
		{
			if (ignoreCase)
			{
				stringRepresentation = stringRepresentation.ToLower();
			}
			foreach (object obj in Enum.GetValues(enumType))
			{
				Enum @enum = (Enum)obj;
				string text = EnumUtil.GetDescription(@enum);
				if (ignoreCase)
				{
					text = text.ToLower();
				}
				if (text == stringRepresentation)
				{
					return @enum;
				}
			}
			return Enum.Parse(enumType, stringRepresentation, ignoreCase);
		}
	}
}
