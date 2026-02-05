using System;
using System.Reflection;

namespace System.Runtime.Serialization
{
	// Token: 0x020000F3 RID: 243
	internal static class CodeInterpreter
	{
		// Token: 0x06000F2D RID: 3885 RVA: 0x0003E024 File Offset: 0x0003C224
		internal static object ConvertValue(object arg, Type source, Type target)
		{
			return CodeInterpreter.InternalConvert(arg, source, target, false);
		}

		// Token: 0x06000F2E RID: 3886 RVA: 0x0003E02F File Offset: 0x0003C22F
		private static bool CanConvert(TypeCode typeCode)
		{
			return typeCode - TypeCode.Boolean <= 11;
		}

		// Token: 0x06000F2F RID: 3887 RVA: 0x0003E03C File Offset: 0x0003C23C
		private static object InternalConvert(object arg, Type source, Type target, bool isAddress)
		{
			if (target == source)
			{
				return arg;
			}
			if (target.IsValueType)
			{
				if (source.IsValueType)
				{
					if (!CodeInterpreter.CanConvert(Type.GetTypeCode(target)))
					{
						throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("An internal error has occurred. No conversion is possible to '{0}' - error generating code for serialization.", new object[] { DataContract.GetClrTypeFullName(target) })));
					}
					return target;
				}
				else
				{
					if (source.IsAssignableFrom(target))
					{
						return arg;
					}
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("An internal error has occurred. '{0}' is not assignable from '{1}' - error generating code for serialization.", new object[]
					{
						DataContract.GetClrTypeFullName(target),
						DataContract.GetClrTypeFullName(source)
					})));
				}
			}
			else
			{
				if (target.IsAssignableFrom(source))
				{
					return arg;
				}
				if (source.IsAssignableFrom(target))
				{
					return arg;
				}
				if (target.IsInterface || source.IsInterface)
				{
					return arg;
				}
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("An internal error has occurred. '{0}' is not assignable from '{1}' - error generating code for serialization.", new object[]
				{
					DataContract.GetClrTypeFullName(target),
					DataContract.GetClrTypeFullName(source)
				})));
			}
		}

		// Token: 0x06000F30 RID: 3888 RVA: 0x0003E124 File Offset: 0x0003C324
		public static object GetMember(MemberInfo memberInfo, object instance)
		{
			PropertyInfo propertyInfo = memberInfo as PropertyInfo;
			if (propertyInfo != null)
			{
				return propertyInfo.GetValue(instance);
			}
			return ((FieldInfo)memberInfo).GetValue(instance);
		}

		// Token: 0x06000F31 RID: 3889 RVA: 0x0003E158 File Offset: 0x0003C358
		public static void SetMember(MemberInfo memberInfo, object instance, object value)
		{
			PropertyInfo propertyInfo = memberInfo as PropertyInfo;
			if (propertyInfo != null)
			{
				propertyInfo.SetValue(instance, value);
				return;
			}
			((FieldInfo)memberInfo).SetValue(instance, value);
		}
	}
}
