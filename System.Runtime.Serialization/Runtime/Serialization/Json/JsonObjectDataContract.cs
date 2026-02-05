using System;
using System.Globalization;
using System.Xml;

namespace System.Runtime.Serialization.Json
{
	// Token: 0x0200010F RID: 271
	internal class JsonObjectDataContract : JsonDataContract
	{
		// Token: 0x06001034 RID: 4148 RVA: 0x0004218A File Offset: 0x0004038A
		public JsonObjectDataContract(DataContract traditionalDataContract)
			: base(traditionalDataContract)
		{
		}

		// Token: 0x06001035 RID: 4149 RVA: 0x00042194 File Offset: 0x00040394
		public override object ReadJsonValueCore(XmlReaderDelegator jsonReader, XmlObjectSerializerReadContextComplexJson context)
		{
			string attribute = jsonReader.GetAttribute("type");
			uint num = <PrivateImplementationDetails>.ComputeStringHash(attribute);
			object obj;
			if (num <= 467038368U)
			{
				if (num != 0U)
				{
					if (num != 398550328U)
					{
						if (num != 467038368U)
						{
							goto IL_011C;
						}
						if (!(attribute == "number"))
						{
							goto IL_011C;
						}
						obj = JsonObjectDataContract.ParseJsonNumber(jsonReader.ReadElementContentAsString());
						goto IL_013B;
					}
					else if (!(attribute == "string"))
					{
						goto IL_011C;
					}
				}
				else if (attribute != null)
				{
					goto IL_011C;
				}
				obj = jsonReader.ReadElementContentAsString();
				goto IL_013B;
			}
			if (num <= 1996966820U)
			{
				if (num != 1710517951U)
				{
					if (num == 1996966820U)
					{
						if (attribute == "null")
						{
							jsonReader.Skip();
							obj = null;
							goto IL_013B;
						}
					}
				}
				else if (attribute == "boolean")
				{
					obj = jsonReader.ReadElementContentAsBoolean();
					goto IL_013B;
				}
			}
			else if (num != 2321067302U)
			{
				if (num == 3099987130U)
				{
					if (attribute == "object")
					{
						jsonReader.Skip();
						obj = new object();
						goto IL_013B;
					}
				}
			}
			else if (attribute == "array")
			{
				return DataContractJsonSerializer.ReadJsonValue(DataContract.GetDataContract(Globals.TypeOfObjectArray), jsonReader, context);
			}
			IL_011C:
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("Unexpected attribute value '{0}'.", new object[] { attribute })));
			IL_013B:
			if (context != null)
			{
				context.AddNewObject(obj);
			}
			return obj;
		}

		// Token: 0x06001036 RID: 4150 RVA: 0x000422E7 File Offset: 0x000404E7
		public override void WriteJsonValueCore(XmlWriterDelegator jsonWriter, object obj, XmlObjectSerializerWriteContextComplexJson context, RuntimeTypeHandle declaredTypeHandle)
		{
			jsonWriter.WriteAttributeString(null, "type", null, "object");
		}

		// Token: 0x06001037 RID: 4151 RVA: 0x000422FC File Offset: 0x000404FC
		internal static object ParseJsonNumber(string value, out TypeCode objectTypeCode)
		{
			if (value == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("The value '{0}' cannot be parsed as the type '{1}'.", new object[]
				{
					value,
					Globals.TypeOfInt
				})));
			}
			if (value.IndexOfAny(JsonGlobals.floatingPointCharacters) == -1)
			{
				int num;
				if (int.TryParse(value, NumberStyles.Float, NumberFormatInfo.InvariantInfo, out num))
				{
					objectTypeCode = TypeCode.Int32;
					return num;
				}
				long num2;
				if (long.TryParse(value, NumberStyles.Float, NumberFormatInfo.InvariantInfo, out num2))
				{
					objectTypeCode = TypeCode.Int64;
					return num2;
				}
			}
			decimal num3;
			if (decimal.TryParse(value, NumberStyles.Float, NumberFormatInfo.InvariantInfo, out num3))
			{
				objectTypeCode = TypeCode.Decimal;
				if (num3 == 0m)
				{
					double num4 = XmlConverter.ToDouble(value);
					if (num4 != 0.0)
					{
						objectTypeCode = TypeCode.Double;
						return num4;
					}
				}
				return num3;
			}
			objectTypeCode = TypeCode.Double;
			return XmlConverter.ToDouble(value);
		}

		// Token: 0x06001038 RID: 4152 RVA: 0x000423D8 File Offset: 0x000405D8
		private static object ParseJsonNumber(string value)
		{
			TypeCode typeCode;
			return JsonObjectDataContract.ParseJsonNumber(value, out typeCode);
		}
	}
}
