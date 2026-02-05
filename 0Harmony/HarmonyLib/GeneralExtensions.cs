using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace HarmonyLib
{
	// Token: 0x02000099 RID: 153
	public static class GeneralExtensions
	{
		// Token: 0x060002E9 RID: 745 RVA: 0x0000E6F4 File Offset: 0x0000C8F4
		public static string Join<T>(this IEnumerable<T> enumeration, Func<T, string> converter = null, string delimiter = ", ")
		{
			if (converter == null)
			{
				converter = (T t) => t.ToString();
			}
			return enumeration.Aggregate("", (string prev, T curr) => prev + ((prev.Length > 0) ? delimiter : "") + converter(curr));
		}

		// Token: 0x060002EA RID: 746 RVA: 0x0000E75C File Offset: 0x0000C95C
		public static string Description(this Type[] parameters)
		{
			if (parameters == null)
			{
				return "NULL";
			}
			return "(" + parameters.Join((Type p) => p.FullDescription(), ", ") + ")";
		}

		// Token: 0x060002EB RID: 747 RVA: 0x0000E7AC File Offset: 0x0000C9AC
		public static string FullDescription(this Type type)
		{
			if (type == null)
			{
				return "null";
			}
			string text = type.Namespace;
			if (!string.IsNullOrEmpty(text))
			{
				text += ".";
			}
			string text2 = text + type.Name;
			if (type.IsGenericType)
			{
				text2 += "<";
				Type[] genericArguments = type.GetGenericArguments();
				for (int i = 0; i < genericArguments.Length; i++)
				{
					if (!text2.EndsWith("<", StringComparison.Ordinal))
					{
						text2 += ", ";
					}
					text2 += genericArguments[i].FullDescription();
				}
				text2 += ">";
			}
			return text2;
		}

		// Token: 0x060002EC RID: 748 RVA: 0x0000E850 File Offset: 0x0000CA50
		public static string FullDescription(this MethodBase member)
		{
			if (member == null)
			{
				return "null";
			}
			Type[] array = member.GetParameters().Types();
			Type returnedType = AccessTools.GetReturnedType(member);
			List<string> list = new List<string>();
			if (member.IsStatic)
			{
				list.Add("static");
			}
			if (member.IsAbstract)
			{
				list.Add("abstract");
			}
			if (member.IsVirtual)
			{
				list.Add("virtual");
			}
			list.Add(returnedType.FullDescription());
			if (member.DeclaringType != null)
			{
				list.Add(member.DeclaringType.FullDescription());
			}
			list.Add(member.Name + array.Description());
			return list.ToArray().Join(null, " ");
		}

		// Token: 0x060002ED RID: 749 RVA: 0x0000E911 File Offset: 0x0000CB11
		public static Type[] Types(this ParameterInfo[] pinfo)
		{
			return pinfo.Select((ParameterInfo pi) => pi.ParameterType).ToArray<Type>();
		}

		// Token: 0x060002EE RID: 750 RVA: 0x0000E940 File Offset: 0x0000CB40
		public static T GetValueSafe<S, T>(this Dictionary<S, T> dictionary, S key)
		{
			T t;
			if (dictionary.TryGetValue(key, out t))
			{
				return t;
			}
			return default(T);
		}

		// Token: 0x060002EF RID: 751 RVA: 0x0000E964 File Offset: 0x0000CB64
		public static T GetTypedValue<T>(this Dictionary<string, object> dictionary, string key)
		{
			object obj;
			if (dictionary.TryGetValue(key, out obj) && obj is T)
			{
				return (T)((object)obj);
			}
			return default(T);
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x0000E994 File Offset: 0x0000CB94
		public static string ToLiteral(this string input, string quoteChar = "\"")
		{
			StringBuilder stringBuilder = new StringBuilder(input.Length + 2);
			stringBuilder.Append(quoteChar);
			int i = 0;
			while (i < input.Length)
			{
				char c = input[i];
				if (c <= '"')
				{
					switch (c)
					{
					case '\0':
						stringBuilder.Append("\\0");
						break;
					case '\u0001':
					case '\u0002':
					case '\u0003':
					case '\u0004':
					case '\u0005':
					case '\u0006':
						goto IL_012C;
					case '\a':
						stringBuilder.Append("\\a");
						break;
					case '\b':
						stringBuilder.Append("\\b");
						break;
					case '\t':
						stringBuilder.Append("\\t");
						break;
					case '\n':
						stringBuilder.Append("\\n");
						break;
					case '\v':
						stringBuilder.Append("\\v");
						break;
					case '\f':
						stringBuilder.Append("\\f");
						break;
					case '\r':
						stringBuilder.Append("\\r");
						break;
					default:
						if (c != '"')
						{
							goto IL_012C;
						}
						stringBuilder.Append("\\\"");
						break;
					}
				}
				else if (c != '\'')
				{
					if (c != '\\')
					{
						goto IL_012C;
					}
					stringBuilder.Append("\\\\");
				}
				else
				{
					stringBuilder.Append("\\'");
				}
				IL_0162:
				i++;
				continue;
				IL_012C:
				if (c >= ' ' && c <= '~')
				{
					stringBuilder.Append(c);
					goto IL_0162;
				}
				stringBuilder.Append("\\u");
				StringBuilder stringBuilder2 = stringBuilder;
				int num = (int)c;
				stringBuilder2.Append(num.ToString("x4"));
				goto IL_0162;
			}
			stringBuilder.Append(quoteChar);
			return stringBuilder.ToString();
		}
	}
}
