using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Google.Protobuf.Collections;

namespace Google.Protobuf.Reflection
{
	// Token: 0x0200004E RID: 78
	public sealed class CustomOptions
	{
		// Token: 0x0600045C RID: 1116 RVA: 0x00011322 File Offset: 0x0000F522
		internal CustomOptions(IDictionary<int, IExtensionValue> values)
		{
			this.values = values;
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x00011331 File Offset: 0x0000F531
		public bool TryGetBool(int field, out bool value)
		{
			return this.TryGetPrimitiveValue<bool>(field, out value);
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x0001133B File Offset: 0x0000F53B
		public bool TryGetInt32(int field, out int value)
		{
			return this.TryGetPrimitiveValue<int>(field, out value);
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x00011345 File Offset: 0x0000F545
		public bool TryGetInt64(int field, out long value)
		{
			return this.TryGetPrimitiveValue<long>(field, out value);
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x0001134F File Offset: 0x0000F54F
		public bool TryGetFixed32(int field, out uint value)
		{
			return this.TryGetUInt32(field, out value);
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x00011359 File Offset: 0x0000F559
		public bool TryGetFixed64(int field, out ulong value)
		{
			return this.TryGetUInt64(field, out value);
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x00011363 File Offset: 0x0000F563
		public bool TryGetSFixed32(int field, out int value)
		{
			return this.TryGetInt32(field, out value);
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x0001136D File Offset: 0x0000F56D
		public bool TryGetSFixed64(int field, out long value)
		{
			return this.TryGetInt64(field, out value);
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x00011377 File Offset: 0x0000F577
		public bool TryGetSInt32(int field, out int value)
		{
			return this.TryGetPrimitiveValue<int>(field, out value);
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x00011381 File Offset: 0x0000F581
		public bool TryGetSInt64(int field, out long value)
		{
			return this.TryGetPrimitiveValue<long>(field, out value);
		}

		// Token: 0x06000466 RID: 1126 RVA: 0x0001138B File Offset: 0x0000F58B
		public bool TryGetUInt32(int field, out uint value)
		{
			return this.TryGetPrimitiveValue<uint>(field, out value);
		}

		// Token: 0x06000467 RID: 1127 RVA: 0x00011395 File Offset: 0x0000F595
		public bool TryGetUInt64(int field, out ulong value)
		{
			return this.TryGetPrimitiveValue<ulong>(field, out value);
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x0001139F File Offset: 0x0000F59F
		public bool TryGetFloat(int field, out float value)
		{
			return this.TryGetPrimitiveValue<float>(field, out value);
		}

		// Token: 0x06000469 RID: 1129 RVA: 0x000113A9 File Offset: 0x0000F5A9
		public bool TryGetDouble(int field, out double value)
		{
			return this.TryGetPrimitiveValue<double>(field, out value);
		}

		// Token: 0x0600046A RID: 1130 RVA: 0x000113B3 File Offset: 0x0000F5B3
		public bool TryGetString(int field, out string value)
		{
			return this.TryGetPrimitiveValue<string>(field, out value);
		}

		// Token: 0x0600046B RID: 1131 RVA: 0x000113BD File Offset: 0x0000F5BD
		public bool TryGetBytes(int field, out ByteString value)
		{
			return this.TryGetPrimitiveValue<ByteString>(field, out value);
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x000113C8 File Offset: 0x0000F5C8
		public bool TryGetMessage<T>(int field, out T value) where T : class, IMessage, new()
		{
			if (this.values == null)
			{
				value = default(T);
				return false;
			}
			IExtensionValue extensionValue;
			if (this.values.TryGetValue(field, out extensionValue))
			{
				if (extensionValue is ExtensionValue<T>)
				{
					ByteString byteString = (extensionValue as ExtensionValue<T>).GetValue().ToByteString();
					value = new T();
					value.MergeFrom(byteString);
					return true;
				}
				if (extensionValue is RepeatedExtensionValue<T>)
				{
					RepeatedExtensionValue<T> repeatedExtensionValue = extensionValue as RepeatedExtensionValue<T>;
					value = (from v in repeatedExtensionValue.GetValue()
						select v.ToByteString()).Aggregate(new T(), delegate(T t, ByteString b)
					{
						t.MergeFrom(b);
						return t;
					});
					return true;
				}
			}
			value = default(T);
			return false;
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x000114A8 File Offset: 0x0000F6A8
		private bool TryGetPrimitiveValue<T>(int field, out T value)
		{
			if (this.values == null)
			{
				value = default(T);
				return false;
			}
			IExtensionValue extensionValue;
			if (this.values.TryGetValue(field, out extensionValue))
			{
				if (extensionValue is ExtensionValue<T>)
				{
					ExtensionValue<T> extensionValue2 = extensionValue as ExtensionValue<T>;
					value = extensionValue2.GetValue();
					return true;
				}
				if (extensionValue is RepeatedExtensionValue<T>)
				{
					RepeatedExtensionValue<T> repeatedExtensionValue = extensionValue as RepeatedExtensionValue<T>;
					if (repeatedExtensionValue.GetValue().Count != 0)
					{
						RepeatedField<T> value2 = repeatedExtensionValue.GetValue();
						value = value2[value2.Count - 1];
						return true;
					}
				}
				else
				{
					Type type = extensionValue.GetType();
					if (type.GetGenericTypeDefinition() == typeof(ExtensionValue<>))
					{
						TypeInfo typeInfo = type.GetTypeInfo();
						Type[] genericTypeArguments = typeInfo.GenericTypeArguments;
						if (genericTypeArguments.Length == 1 && genericTypeArguments[0].GetTypeInfo().IsEnum)
						{
							value = (T)((object)typeInfo.GetDeclaredMethod("GetValue").Invoke(extensionValue, CustomOptions.EmptyParameters));
							return true;
						}
					}
					else if (type.GetGenericTypeDefinition() == typeof(RepeatedExtensionValue<>))
					{
						TypeInfo typeInfo2 = type.GetTypeInfo();
						Type[] genericTypeArguments2 = typeInfo2.GenericTypeArguments;
						if (genericTypeArguments2.Length == 1 && genericTypeArguments2[0].GetTypeInfo().IsEnum)
						{
							IList list = (IList)typeInfo2.GetDeclaredMethod("GetValue").Invoke(extensionValue, CustomOptions.EmptyParameters);
							if (list.Count != 0)
							{
								value = (T)((object)list[list.Count - 1]);
								return true;
							}
						}
					}
				}
			}
			value = default(T);
			return false;
		}

		// Token: 0x04000150 RID: 336
		private static readonly object[] EmptyParameters = new object[0];

		// Token: 0x04000151 RID: 337
		private readonly IDictionary<int, IExtensionValue> values;
	}
}
