using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Google.Protobuf.Reflection;
using Google.Protobuf.WellKnownTypes;

namespace Google.Protobuf
{
	// Token: 0x0200001B RID: 27
	public sealed class JsonParser
	{
		// Token: 0x06000175 RID: 373 RVA: 0x00007A0B File Offset: 0x00005C0B
		private static void MergeWrapperField(JsonParser parser, IMessage message, JsonTokenizer tokenizer)
		{
			parser.MergeField(message, message.Descriptor.Fields[1], tokenizer);
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000176 RID: 374 RVA: 0x00007A26 File Offset: 0x00005C26
		public static JsonParser Default
		{
			get
			{
				return JsonParser.defaultInstance;
			}
		}

		// Token: 0x06000177 RID: 375 RVA: 0x00007A2D File Offset: 0x00005C2D
		public JsonParser(JsonParser.Settings settings)
		{
			this.settings = ProtoPreconditions.CheckNotNull<JsonParser.Settings>(settings, "settings");
		}

		// Token: 0x06000178 RID: 376 RVA: 0x00007A46 File Offset: 0x00005C46
		internal void Merge(IMessage message, string json)
		{
			this.Merge(message, new StringReader(json));
		}

		// Token: 0x06000179 RID: 377 RVA: 0x00007A58 File Offset: 0x00005C58
		internal void Merge(IMessage message, TextReader jsonReader)
		{
			JsonTokenizer jsonTokenizer = JsonTokenizer.FromTextReader(jsonReader);
			this.Merge(message, jsonTokenizer);
			if (jsonTokenizer.Next() != JsonToken.EndDocument)
			{
				throw new InvalidProtocolBufferException("Expected end of JSON after object");
			}
		}

		// Token: 0x0600017A RID: 378 RVA: 0x00007A8C File Offset: 0x00005C8C
		private void Merge(IMessage message, JsonTokenizer tokenizer)
		{
			if (tokenizer.ObjectDepth > this.settings.RecursionLimit)
			{
				throw InvalidProtocolBufferException.JsonRecursionLimitExceeded();
			}
			Action<JsonParser, IMessage, JsonTokenizer> action;
			if (message.Descriptor.IsWellKnownType && JsonParser.WellKnownTypeHandlers.TryGetValue(message.Descriptor.FullName, out action))
			{
				action(this, message, tokenizer);
				return;
			}
			JsonToken jsonToken = tokenizer.Next();
			if (jsonToken.Type != JsonToken.TokenType.StartObject)
			{
				throw new InvalidProtocolBufferException("Expected an object");
			}
			IDictionary<string, FieldDescriptor> dictionary = message.Descriptor.Fields.ByJsonName();
			HashSet<OneofDescriptor> hashSet = null;
			string stringValue;
			FieldDescriptor fieldDescriptor;
			for (;;)
			{
				jsonToken = tokenizer.Next();
				if (jsonToken.Type == JsonToken.TokenType.EndObject)
				{
					break;
				}
				if (jsonToken.Type != JsonToken.TokenType.Name)
				{
					goto Block_6;
				}
				stringValue = jsonToken.StringValue;
				if (dictionary.TryGetValue(stringValue, out fieldDescriptor))
				{
					if (fieldDescriptor.ContainingOneof != null)
					{
						if (hashSet == null)
						{
							hashSet = new HashSet<OneofDescriptor>();
						}
						if (!hashSet.Add(fieldDescriptor.ContainingOneof))
						{
							goto Block_10;
						}
					}
					this.MergeField(message, fieldDescriptor, tokenizer);
				}
				else
				{
					if (!this.settings.IgnoreUnknownFields)
					{
						goto IL_012E;
					}
					tokenizer.SkipValue();
				}
			}
			return;
			Block_6:
			throw new InvalidOperationException("Unexpected token type " + jsonToken.Type.ToString());
			Block_10:
			throw new InvalidProtocolBufferException("Multiple values specified for oneof " + fieldDescriptor.ContainingOneof.Name);
			IL_012E:
			throw new InvalidProtocolBufferException("Unknown field: " + stringValue);
		}

		// Token: 0x0600017B RID: 379 RVA: 0x00007BD8 File Offset: 0x00005DD8
		private void MergeField(IMessage message, FieldDescriptor field, JsonTokenizer tokenizer)
		{
			JsonToken jsonToken = tokenizer.Next();
			if (jsonToken.Type == JsonToken.TokenType.Null && (field.IsMap || field.IsRepeated || !JsonParser.IsGoogleProtobufValueField(field)))
			{
				field.Accessor.Clear(message);
				return;
			}
			tokenizer.PushBack(jsonToken);
			if (field.IsMap)
			{
				this.MergeMapField(message, field, tokenizer);
				return;
			}
			if (field.IsRepeated)
			{
				this.MergeRepeatedField(message, field, tokenizer);
				return;
			}
			object obj = this.ParseSingleValue(field, tokenizer);
			field.Accessor.SetValue(message, obj);
		}

		// Token: 0x0600017C RID: 380 RVA: 0x00007C5C File Offset: 0x00005E5C
		private void MergeRepeatedField(IMessage message, FieldDescriptor field, JsonTokenizer tokenizer)
		{
			JsonToken jsonToken = tokenizer.Next();
			if (jsonToken.Type != JsonToken.TokenType.StartArray)
			{
				throw new InvalidProtocolBufferException("Repeated field value was not an array. Token type: " + jsonToken.Type.ToString());
			}
			IList list = (IList)field.Accessor.GetValue(message);
			for (;;)
			{
				jsonToken = tokenizer.Next();
				if (jsonToken.Type == JsonToken.TokenType.EndArray)
				{
					break;
				}
				tokenizer.PushBack(jsonToken);
				object obj = this.ParseSingleValue(field, tokenizer);
				if (obj == null)
				{
					goto Block_3;
				}
				list.Add(obj);
			}
			return;
			Block_3:
			throw new InvalidProtocolBufferException("Repeated field elements cannot be null");
		}

		// Token: 0x0600017D RID: 381 RVA: 0x00007CE8 File Offset: 0x00005EE8
		private void MergeMapField(IMessage message, FieldDescriptor field, JsonTokenizer tokenizer)
		{
			JsonToken jsonToken = tokenizer.Next();
			if (jsonToken.Type != JsonToken.TokenType.StartObject)
			{
				throw new InvalidProtocolBufferException("Expected an object to populate a map");
			}
			MessageDescriptor messageType = field.MessageType;
			FieldDescriptor fieldDescriptor = messageType.FindFieldByNumber(1);
			FieldDescriptor fieldDescriptor2 = messageType.FindFieldByNumber(2);
			if (fieldDescriptor == null || fieldDescriptor2 == null)
			{
				throw new InvalidProtocolBufferException("Invalid map field: " + field.FullName);
			}
			IDictionary dictionary = (IDictionary)field.Accessor.GetValue(message);
			for (;;)
			{
				jsonToken = tokenizer.Next();
				if (jsonToken.Type == JsonToken.TokenType.EndObject)
				{
					break;
				}
				object obj = JsonParser.ParseMapKey(fieldDescriptor, jsonToken.StringValue);
				object obj2 = this.ParseSingleValue(fieldDescriptor2, tokenizer);
				if (obj2 == null)
				{
					goto Block_4;
				}
				dictionary[obj] = obj2;
			}
			return;
			Block_4:
			throw new InvalidProtocolBufferException("Map values must not be null");
		}

		// Token: 0x0600017E RID: 382 RVA: 0x00007D96 File Offset: 0x00005F96
		private static bool IsGoogleProtobufValueField(FieldDescriptor field)
		{
			return field.FieldType == FieldType.Message && field.MessageType.FullName == Value.Descriptor.FullName;
		}

		// Token: 0x0600017F RID: 383 RVA: 0x00007DC0 File Offset: 0x00005FC0
		private object ParseSingleValue(FieldDescriptor field, JsonTokenizer tokenizer)
		{
			JsonToken jsonToken = tokenizer.Next();
			if (jsonToken.Type != JsonToken.TokenType.Null)
			{
				FieldType fieldType = field.FieldType;
				if (fieldType == FieldType.Message)
				{
					if (!field.MessageType.IsWrapperType)
					{
						tokenizer.PushBack(jsonToken);
						IMessage message = JsonParser.NewMessageForField(field);
						this.Merge(message, tokenizer);
						return message;
					}
					field = field.MessageType.Fields[1];
					fieldType = field.FieldType;
				}
				switch (jsonToken.Type)
				{
				case JsonToken.TokenType.Null:
					throw new NotImplementedException("Haven't worked out what to do for null yet");
				case JsonToken.TokenType.False:
				case JsonToken.TokenType.True:
					if (fieldType == FieldType.Bool)
					{
						return jsonToken.Type == JsonToken.TokenType.True;
					}
					break;
				case JsonToken.TokenType.StringValue:
					return JsonParser.ParseSingleStringValue(field, jsonToken.StringValue);
				case JsonToken.TokenType.Number:
					return JsonParser.ParseSingleNumberValue(field, jsonToken);
				}
				throw new InvalidProtocolBufferException("Unsupported JSON token type " + jsonToken.Type.ToString() + " for field type " + fieldType.ToString());
			}
			if (JsonParser.IsGoogleProtobufValueField(field))
			{
				return Value.ForNull();
			}
			return null;
		}

		// Token: 0x06000180 RID: 384 RVA: 0x00007EC5 File Offset: 0x000060C5
		public T Parse<T>(string json) where T : IMessage, new()
		{
			ProtoPreconditions.CheckNotNull<string>(json, "json");
			return this.Parse<T>(new StringReader(json));
		}

		// Token: 0x06000181 RID: 385 RVA: 0x00007EE0 File Offset: 0x000060E0
		public T Parse<T>(TextReader jsonReader) where T : IMessage, new()
		{
			ProtoPreconditions.CheckNotNull<TextReader>(jsonReader, "jsonReader");
			T t = new T();
			this.Merge(t, jsonReader);
			return t;
		}

		// Token: 0x06000182 RID: 386 RVA: 0x00007F0D File Offset: 0x0000610D
		public IMessage Parse(string json, MessageDescriptor descriptor)
		{
			ProtoPreconditions.CheckNotNull<string>(json, "json");
			ProtoPreconditions.CheckNotNull<MessageDescriptor>(descriptor, "descriptor");
			return this.Parse(new StringReader(json), descriptor);
		}

		// Token: 0x06000183 RID: 387 RVA: 0x00007F34 File Offset: 0x00006134
		public IMessage Parse(TextReader jsonReader, MessageDescriptor descriptor)
		{
			ProtoPreconditions.CheckNotNull<TextReader>(jsonReader, "jsonReader");
			ProtoPreconditions.CheckNotNull<MessageDescriptor>(descriptor, "descriptor");
			IMessage message = descriptor.Parser.CreateTemplate();
			this.Merge(message, jsonReader);
			return message;
		}

		// Token: 0x06000184 RID: 388 RVA: 0x00007F70 File Offset: 0x00006170
		private void MergeStructValue(IMessage message, JsonTokenizer tokenizer)
		{
			JsonToken jsonToken = tokenizer.Next();
			MessageDescriptor.FieldCollection fields = message.Descriptor.Fields;
			switch (jsonToken.Type)
			{
			case JsonToken.TokenType.Null:
				fields[1].Accessor.SetValue(message, 0);
				return;
			case JsonToken.TokenType.False:
			case JsonToken.TokenType.True:
				fields[4].Accessor.SetValue(message, jsonToken.Type == JsonToken.TokenType.True);
				return;
			case JsonToken.TokenType.StringValue:
				fields[3].Accessor.SetValue(message, jsonToken.StringValue);
				return;
			case JsonToken.TokenType.Number:
				fields[2].Accessor.SetValue(message, jsonToken.NumberValue);
				return;
			case JsonToken.TokenType.StartObject:
			{
				FieldDescriptor fieldDescriptor = fields[5];
				IMessage message2 = JsonParser.NewMessageForField(fieldDescriptor);
				tokenizer.PushBack(jsonToken);
				this.Merge(message2, tokenizer);
				fieldDescriptor.Accessor.SetValue(message, message2);
				return;
			}
			case JsonToken.TokenType.StartArray:
			{
				FieldDescriptor fieldDescriptor2 = fields[6];
				IMessage message3 = JsonParser.NewMessageForField(fieldDescriptor2);
				tokenizer.PushBack(jsonToken);
				this.Merge(message3, tokenizer);
				fieldDescriptor2.Accessor.SetValue(message, message3);
				return;
			}
			}
			throw new InvalidOperationException("Unexpected token type: " + jsonToken.Type.ToString());
		}

		// Token: 0x06000185 RID: 389 RVA: 0x000080B4 File Offset: 0x000062B4
		private void MergeStruct(IMessage message, JsonTokenizer tokenizer)
		{
			JsonToken jsonToken = tokenizer.Next();
			if (jsonToken.Type != JsonToken.TokenType.StartObject)
			{
				throw new InvalidProtocolBufferException("Expected object value for Struct");
			}
			tokenizer.PushBack(jsonToken);
			FieldDescriptor fieldDescriptor = message.Descriptor.Fields[1];
			this.MergeMapField(message, fieldDescriptor, tokenizer);
		}

		// Token: 0x06000186 RID: 390 RVA: 0x00008100 File Offset: 0x00006300
		private void MergeAny(IMessage message, JsonTokenizer tokenizer)
		{
			List<JsonToken> list = new List<JsonToken>();
			JsonToken jsonToken = tokenizer.Next();
			if (jsonToken.Type != JsonToken.TokenType.StartObject)
			{
				throw new InvalidProtocolBufferException("Expected object value for Any");
			}
			int objectDepth = tokenizer.ObjectDepth;
			while (jsonToken.Type != JsonToken.TokenType.Name || jsonToken.StringValue != "@type" || tokenizer.ObjectDepth != objectDepth)
			{
				list.Add(jsonToken);
				jsonToken = tokenizer.Next();
				if (tokenizer.ObjectDepth < objectDepth)
				{
					throw new InvalidProtocolBufferException("Any message with no @type");
				}
			}
			jsonToken = tokenizer.Next();
			if (jsonToken.Type != JsonToken.TokenType.StringValue)
			{
				throw new InvalidProtocolBufferException("Expected string value for Any.@type");
			}
			string stringValue = jsonToken.StringValue;
			string typeName = Any.GetTypeName(stringValue);
			MessageDescriptor messageDescriptor = this.settings.TypeRegistry.Find(typeName);
			if (messageDescriptor == null)
			{
				throw new InvalidOperationException("Type registry has no descriptor for type name '" + typeName + "'");
			}
			JsonTokenizer jsonTokenizer = JsonTokenizer.FromReplayedTokens(list, tokenizer);
			IMessage message2 = messageDescriptor.Parser.CreateTemplate();
			if (messageDescriptor.IsWellKnownType)
			{
				this.MergeWellKnownTypeAnyBody(message2, jsonTokenizer);
			}
			else
			{
				this.Merge(message2, jsonTokenizer);
			}
			ByteString byteString = message2.ToByteString();
			message.Descriptor.Fields[1].Accessor.SetValue(message, stringValue);
			message.Descriptor.Fields[2].Accessor.SetValue(message, byteString);
		}

		// Token: 0x06000187 RID: 391 RVA: 0x0000824C File Offset: 0x0000644C
		private void MergeWellKnownTypeAnyBody(IMessage body, JsonTokenizer tokenizer)
		{
			JsonToken jsonToken = tokenizer.Next();
			jsonToken = tokenizer.Next();
			if (jsonToken.Type != JsonToken.TokenType.Name || jsonToken.StringValue != "value")
			{
				throw new InvalidProtocolBufferException("Expected 'value' property for well-known type Any body");
			}
			this.Merge(body, tokenizer);
			jsonToken = tokenizer.Next();
			if (jsonToken.Type != JsonToken.TokenType.EndObject)
			{
				throw new InvalidProtocolBufferException("Expected end-object token after @type/value for well-known type");
			}
		}

		// Token: 0x06000188 RID: 392 RVA: 0x000082B0 File Offset: 0x000064B0
		private static object ParseMapKey(FieldDescriptor field, string keyText)
		{
			switch (field.FieldType)
			{
			case FieldType.Int64:
			case FieldType.SFixed64:
			case FieldType.SInt64:
				return JsonParser.ParseNumericString<long>(keyText, new Func<string, NumberStyles, IFormatProvider, long>(long.Parse));
			case FieldType.UInt64:
			case FieldType.Fixed64:
				return JsonParser.ParseNumericString<ulong>(keyText, new Func<string, NumberStyles, IFormatProvider, ulong>(ulong.Parse));
			case FieldType.Int32:
			case FieldType.SFixed32:
			case FieldType.SInt32:
				return JsonParser.ParseNumericString<int>(keyText, new Func<string, NumberStyles, IFormatProvider, int>(int.Parse));
			case FieldType.Fixed32:
			case FieldType.UInt32:
				return JsonParser.ParseNumericString<uint>(keyText, new Func<string, NumberStyles, IFormatProvider, uint>(uint.Parse));
			case FieldType.Bool:
				if (keyText == "true")
				{
					return true;
				}
				if (keyText == "false")
				{
					return false;
				}
				throw new InvalidProtocolBufferException("Invalid string for bool map key: " + keyText);
			case FieldType.String:
				return keyText;
			}
			throw new InvalidProtocolBufferException("Invalid field type for map: " + field.FieldType.ToString());
		}

		// Token: 0x06000189 RID: 393 RVA: 0x000083CC File Offset: 0x000065CC
		private static object ParseSingleNumberValue(FieldDescriptor field, JsonToken token)
		{
			double numberValue = token.NumberValue;
			checked
			{
				try
				{
					switch (field.FieldType)
					{
					case FieldType.Double:
						return numberValue;
					case FieldType.Float:
						if (double.IsNaN(numberValue))
						{
							return float.NaN;
						}
						if (numberValue <= 3.4028234663852886E+38 && numberValue >= -3.4028234663852886E+38)
						{
							return (float)numberValue;
						}
						if (double.IsPositiveInfinity(numberValue))
						{
							return float.PositiveInfinity;
						}
						if (double.IsNegativeInfinity(numberValue))
						{
							return float.NegativeInfinity;
						}
						throw new InvalidProtocolBufferException(string.Format("Value out of range: {0}", numberValue));
					case FieldType.Int64:
					case FieldType.SFixed64:
					case FieldType.SInt64:
						JsonParser.CheckInteger(numberValue);
						return (long)numberValue;
					case FieldType.UInt64:
					case FieldType.Fixed64:
						JsonParser.CheckInteger(numberValue);
						return (ulong)numberValue;
					case FieldType.Int32:
					case FieldType.SFixed32:
					case FieldType.SInt32:
						JsonParser.CheckInteger(numberValue);
						return (int)numberValue;
					case FieldType.Fixed32:
					case FieldType.UInt32:
						JsonParser.CheckInteger(numberValue);
						return (uint)numberValue;
					case FieldType.Enum:
						JsonParser.CheckInteger(numberValue);
						return (int)numberValue;
					}
					throw new InvalidProtocolBufferException(string.Format("Unsupported conversion from JSON number for field type {0}", field.FieldType));
				}
				catch (OverflowException)
				{
					throw new InvalidProtocolBufferException(string.Format("Value out of range: {0}", numberValue));
				}
				object obj;
				return obj;
			}
		}

		// Token: 0x0600018A RID: 394 RVA: 0x0000856C File Offset: 0x0000676C
		private static void CheckInteger(double value)
		{
			if (double.IsInfinity(value) || double.IsNaN(value))
			{
				throw new InvalidProtocolBufferException(string.Format("Value not an integer: {0}", value));
			}
			if (value != Math.Floor(value))
			{
				throw new InvalidProtocolBufferException(string.Format("Value not an integer: {0}", value));
			}
		}

		// Token: 0x0600018B RID: 395 RVA: 0x000085C0 File Offset: 0x000067C0
		private static object ParseSingleStringValue(FieldDescriptor field, string text)
		{
			switch (field.FieldType)
			{
			case FieldType.Double:
			{
				double num = JsonParser.ParseNumericString<double>(text, new Func<string, NumberStyles, IFormatProvider, double>(double.Parse));
				JsonParser.ValidateInfinityAndNan(text, double.IsPositiveInfinity(num), double.IsNegativeInfinity(num), double.IsNaN(num));
				return num;
			}
			case FieldType.Float:
			{
				float num2 = JsonParser.ParseNumericString<float>(text, new Func<string, NumberStyles, IFormatProvider, float>(float.Parse));
				JsonParser.ValidateInfinityAndNan(text, float.IsPositiveInfinity(num2), float.IsNegativeInfinity(num2), float.IsNaN(num2));
				return num2;
			}
			case FieldType.Int64:
			case FieldType.SFixed64:
			case FieldType.SInt64:
				return JsonParser.ParseNumericString<long>(text, new Func<string, NumberStyles, IFormatProvider, long>(long.Parse));
			case FieldType.UInt64:
			case FieldType.Fixed64:
				return JsonParser.ParseNumericString<ulong>(text, new Func<string, NumberStyles, IFormatProvider, ulong>(ulong.Parse));
			case FieldType.Int32:
			case FieldType.SFixed32:
			case FieldType.SInt32:
				break;
			case FieldType.Fixed32:
			case FieldType.UInt32:
				return JsonParser.ParseNumericString<uint>(text, new Func<string, NumberStyles, IFormatProvider, uint>(uint.Parse));
			case FieldType.Bool:
			case FieldType.Group:
			case FieldType.Message:
				goto IL_016E;
			case FieldType.String:
				return text;
			case FieldType.Bytes:
				try
				{
					return ByteString.FromBase64(text);
				}
				catch (FormatException ex)
				{
					throw InvalidProtocolBufferException.InvalidBase64(ex);
				}
				break;
			case FieldType.Enum:
			{
				EnumValueDescriptor enumValueDescriptor = field.EnumType.FindValueByName(text);
				if (enumValueDescriptor == null)
				{
					throw new InvalidProtocolBufferException("Invalid enum value: " + text + " for enum type: " + field.EnumType.FullName);
				}
				return enumValueDescriptor.Number;
			}
			default:
				goto IL_016E;
			}
			return JsonParser.ParseNumericString<int>(text, new Func<string, NumberStyles, IFormatProvider, int>(int.Parse));
			IL_016E:
			throw new InvalidProtocolBufferException(string.Format("Unsupported conversion from JSON string for field type {0}", field.FieldType));
		}

		// Token: 0x0600018C RID: 396 RVA: 0x00008768 File Offset: 0x00006968
		private static IMessage NewMessageForField(FieldDescriptor field)
		{
			return field.MessageType.Parser.CreateTemplate();
		}

		// Token: 0x0600018D RID: 397 RVA: 0x0000877C File Offset: 0x0000697C
		private static T ParseNumericString<T>(string text, Func<string, NumberStyles, IFormatProvider, T> parser)
		{
			if (text.StartsWith("+"))
			{
				throw new InvalidProtocolBufferException("Invalid numeric value: " + text);
			}
			if (text.StartsWith("0") && text.Length > 1)
			{
				if (text[1] >= '0' && text[1] <= '9')
				{
					throw new InvalidProtocolBufferException("Invalid numeric value: " + text);
				}
			}
			else if (text.StartsWith("-0") && text.Length > 2 && text[2] >= '0' && text[2] <= '9')
			{
				throw new InvalidProtocolBufferException("Invalid numeric value: " + text);
			}
			T t;
			try
			{
				t = parser(text, NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent, CultureInfo.InvariantCulture);
			}
			catch (FormatException)
			{
				throw new InvalidProtocolBufferException("Invalid numeric value for type: " + text);
			}
			catch (OverflowException)
			{
				throw new InvalidProtocolBufferException("Value out of range: " + text);
			}
			return t;
		}

		// Token: 0x0600018E RID: 398 RVA: 0x00008878 File Offset: 0x00006A78
		private static void ValidateInfinityAndNan(string text, bool isPositiveInfinity, bool isNegativeInfinity, bool isNaN)
		{
			if ((isPositiveInfinity && text != "Infinity") || (isNegativeInfinity && text != "-Infinity") || (isNaN && text != "NaN"))
			{
				throw new InvalidProtocolBufferException("Invalid numeric value: " + text);
			}
		}

		// Token: 0x0600018F RID: 399 RVA: 0x000088C8 File Offset: 0x00006AC8
		private static void MergeTimestamp(IMessage message, JsonToken token)
		{
			if (token.Type != JsonToken.TokenType.StringValue)
			{
				throw new InvalidProtocolBufferException("Expected string value for Timestamp");
			}
			Match match = JsonParser.TimestampRegex.Match(token.StringValue);
			if (!match.Success)
			{
				throw new InvalidProtocolBufferException("Invalid Timestamp value: " + token.StringValue);
			}
			string value = match.Groups["datetime"].Value;
			string value2 = match.Groups["subseconds"].Value;
			string value3 = match.Groups["offset"].Value;
			try
			{
				Timestamp timestamp = Timestamp.FromDateTime(DateTime.ParseExact(value, "yyyy-MM-dd'T'HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal));
				int num = 0;
				if (value2 != "")
				{
					num = int.Parse(value2.Substring(1), CultureInfo.InvariantCulture) * JsonParser.SubsecondScalingFactors[value2.Length];
				}
				int num2 = 0;
				if (value3 != "Z")
				{
					int num3 = ((value3[0] == '-') ? 1 : (-1));
					int num4 = int.Parse(value3.Substring(1, 2), CultureInfo.InvariantCulture);
					int num5 = int.Parse(value3.Substring(4, 2));
					int num6 = num4 * 60 + num5;
					if (num6 > 1080)
					{
						throw new InvalidProtocolBufferException("Invalid Timestamp value: " + token.StringValue);
					}
					if (num6 == 0 && num3 == 1)
					{
						throw new InvalidProtocolBufferException("Invalid Timestamp value: " + token.StringValue);
					}
					num2 = num3 * num6 * 60;
				}
				if (num2 < 0 && num > 0)
				{
					num2++;
					num -= 1000000000;
				}
				if (num2 != 0 || num != 0)
				{
					timestamp += new Duration
					{
						Nanos = num,
						Seconds = (long)num2
					};
					if (timestamp.Seconds < -62135596800L || timestamp.Seconds > 253402300799L)
					{
						throw new InvalidProtocolBufferException("Invalid Timestamp value: " + token.StringValue);
					}
				}
				message.Descriptor.Fields[1].Accessor.SetValue(message, timestamp.Seconds);
				message.Descriptor.Fields[2].Accessor.SetValue(message, timestamp.Nanos);
			}
			catch (FormatException)
			{
				throw new InvalidProtocolBufferException("Invalid Timestamp value: " + token.StringValue);
			}
		}

		// Token: 0x06000190 RID: 400 RVA: 0x00008B30 File Offset: 0x00006D30
		private static void MergeDuration(IMessage message, JsonToken token)
		{
			if (token.Type != JsonToken.TokenType.StringValue)
			{
				throw new InvalidProtocolBufferException("Expected string value for Duration");
			}
			Match match = JsonParser.DurationRegex.Match(token.StringValue);
			if (!match.Success)
			{
				throw new InvalidProtocolBufferException("Invalid Duration value: " + token.StringValue);
			}
			string value = match.Groups["sign"].Value;
			string value2 = match.Groups["int"].Value;
			if (value2[0] == '0' && value2.Length > 1)
			{
				throw new InvalidProtocolBufferException("Invalid Duration value: " + token.StringValue);
			}
			string value3 = match.Groups["subseconds"].Value;
			int num = ((value == "-") ? (-1) : 1);
			try
			{
				long num2 = long.Parse(value2, CultureInfo.InvariantCulture) * (long)num;
				int num3 = 0;
				if (value3 != "")
				{
					num3 = int.Parse(value3.Substring(1)) * JsonParser.SubsecondScalingFactors[value3.Length] * num;
				}
				if (!Duration.IsNormalized(num2, num3))
				{
					throw new InvalidProtocolBufferException("Invalid Duration value: " + token.StringValue);
				}
				message.Descriptor.Fields[1].Accessor.SetValue(message, num2);
				message.Descriptor.Fields[2].Accessor.SetValue(message, num3);
			}
			catch (FormatException)
			{
				throw new InvalidProtocolBufferException("Invalid Duration value: " + token.StringValue);
			}
		}

		// Token: 0x06000191 RID: 401 RVA: 0x00008CCC File Offset: 0x00006ECC
		private static void MergeFieldMask(IMessage message, JsonToken token)
		{
			if (token.Type != JsonToken.TokenType.StringValue)
			{
				throw new InvalidProtocolBufferException("Expected string value for FieldMask");
			}
			string[] array = token.StringValue.Split(JsonParser.FieldMaskPathSeparators, StringSplitOptions.RemoveEmptyEntries);
			IList list = (IList)message.Descriptor.Fields[1].Accessor.GetValue(message);
			foreach (string text in array)
			{
				list.Add(JsonParser.ToSnakeCase(text));
			}
		}

		// Token: 0x06000192 RID: 402 RVA: 0x00008D40 File Offset: 0x00006F40
		private static string ToSnakeCase(string text)
		{
			StringBuilder stringBuilder = new StringBuilder(text.Length * 2);
			bool flag = false;
			bool flag2 = false;
			for (int i = 0; i < text.Length; i++)
			{
				char c = text[i];
				if (c >= 'A' && c <= 'Z')
				{
					if (flag && (flag2 || (i + 1 < text.Length && text[i + 1] >= 'a' && text[i + 1] <= 'z')))
					{
						stringBuilder.Append('_');
					}
					stringBuilder.Append(c + 'a' - 'A');
					flag = true;
					flag2 = false;
				}
				else
				{
					stringBuilder.Append(c);
					if (c == '_')
					{
						throw new InvalidProtocolBufferException("Invalid field mask: " + text);
					}
					flag = true;
					flag2 = true;
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04000048 RID: 72
		private static readonly Regex TimestampRegex = new Regex("^(?<datetime>[0-9]{4}-[01][0-9]-[0-3][0-9]T[012][0-9]:[0-5][0-9]:[0-5][0-9])(?<subseconds>\\.[0-9]{1,9})?(?<offset>(Z|[+-][0-1][0-9]:[0-5][0-9]))$", FrameworkPortability.CompiledRegexWhereAvailable);

		// Token: 0x04000049 RID: 73
		private static readonly Regex DurationRegex = new Regex("^(?<sign>-)?(?<int>[0-9]{1,12})(?<subseconds>\\.[0-9]{1,9})?s$", FrameworkPortability.CompiledRegexWhereAvailable);

		// Token: 0x0400004A RID: 74
		private static readonly int[] SubsecondScalingFactors = new int[]
		{
			0, 100000000, 100000000, 10000000, 1000000, 100000, 10000, 1000, 100, 10,
			1
		};

		// Token: 0x0400004B RID: 75
		private static readonly char[] FieldMaskPathSeparators = new char[] { ',' };

		// Token: 0x0400004C RID: 76
		private static readonly JsonParser defaultInstance = new JsonParser(JsonParser.Settings.Default);

		// Token: 0x0400004D RID: 77
		private static readonly Dictionary<string, Action<JsonParser, IMessage, JsonTokenizer>> WellKnownTypeHandlers = new Dictionary<string, Action<JsonParser, IMessage, JsonTokenizer>>
		{
			{
				Timestamp.Descriptor.FullName,
				delegate(JsonParser parser, IMessage message, JsonTokenizer tokenizer)
				{
					JsonParser.MergeTimestamp(message, tokenizer.Next());
				}
			},
			{
				Duration.Descriptor.FullName,
				delegate(JsonParser parser, IMessage message, JsonTokenizer tokenizer)
				{
					JsonParser.MergeDuration(message, tokenizer.Next());
				}
			},
			{
				Value.Descriptor.FullName,
				delegate(JsonParser parser, IMessage message, JsonTokenizer tokenizer)
				{
					parser.MergeStructValue(message, tokenizer);
				}
			},
			{
				ListValue.Descriptor.FullName,
				delegate(JsonParser parser, IMessage message, JsonTokenizer tokenizer)
				{
					parser.MergeRepeatedField(message, message.Descriptor.Fields[1], tokenizer);
				}
			},
			{
				Struct.Descriptor.FullName,
				delegate(JsonParser parser, IMessage message, JsonTokenizer tokenizer)
				{
					parser.MergeStruct(message, tokenizer);
				}
			},
			{
				Any.Descriptor.FullName,
				delegate(JsonParser parser, IMessage message, JsonTokenizer tokenizer)
				{
					parser.MergeAny(message, tokenizer);
				}
			},
			{
				FieldMask.Descriptor.FullName,
				delegate(JsonParser parser, IMessage message, JsonTokenizer tokenizer)
				{
					JsonParser.MergeFieldMask(message, tokenizer.Next());
				}
			},
			{
				Int32Value.Descriptor.FullName,
				new Action<JsonParser, IMessage, JsonTokenizer>(JsonParser.MergeWrapperField)
			},
			{
				Int64Value.Descriptor.FullName,
				new Action<JsonParser, IMessage, JsonTokenizer>(JsonParser.MergeWrapperField)
			},
			{
				UInt32Value.Descriptor.FullName,
				new Action<JsonParser, IMessage, JsonTokenizer>(JsonParser.MergeWrapperField)
			},
			{
				UInt64Value.Descriptor.FullName,
				new Action<JsonParser, IMessage, JsonTokenizer>(JsonParser.MergeWrapperField)
			},
			{
				FloatValue.Descriptor.FullName,
				new Action<JsonParser, IMessage, JsonTokenizer>(JsonParser.MergeWrapperField)
			},
			{
				DoubleValue.Descriptor.FullName,
				new Action<JsonParser, IMessage, JsonTokenizer>(JsonParser.MergeWrapperField)
			},
			{
				BytesValue.Descriptor.FullName,
				new Action<JsonParser, IMessage, JsonTokenizer>(JsonParser.MergeWrapperField)
			},
			{
				StringValue.Descriptor.FullName,
				new Action<JsonParser, IMessage, JsonTokenizer>(JsonParser.MergeWrapperField)
			},
			{
				BoolValue.Descriptor.FullName,
				new Action<JsonParser, IMessage, JsonTokenizer>(JsonParser.MergeWrapperField)
			}
		};

		// Token: 0x0400004E RID: 78
		private readonly JsonParser.Settings settings;

		// Token: 0x020000A3 RID: 163
		public sealed class Settings
		{
			// Token: 0x17000265 RID: 613
			// (get) Token: 0x0600092C RID: 2348 RVA: 0x0001F3AF File Offset: 0x0001D5AF
			public static JsonParser.Settings Default { get; } = new JsonParser.Settings(100);

			// Token: 0x17000266 RID: 614
			// (get) Token: 0x0600092E RID: 2350 RVA: 0x0001F3C4 File Offset: 0x0001D5C4
			public int RecursionLimit { get; }

			// Token: 0x17000267 RID: 615
			// (get) Token: 0x0600092F RID: 2351 RVA: 0x0001F3CC File Offset: 0x0001D5CC
			public TypeRegistry TypeRegistry { get; }

			// Token: 0x17000268 RID: 616
			// (get) Token: 0x06000930 RID: 2352 RVA: 0x0001F3D4 File Offset: 0x0001D5D4
			public bool IgnoreUnknownFields { get; }

			// Token: 0x06000931 RID: 2353 RVA: 0x0001F3DC File Offset: 0x0001D5DC
			private Settings(int recursionLimit, TypeRegistry typeRegistry, bool ignoreUnknownFields)
			{
				this.RecursionLimit = recursionLimit;
				this.TypeRegistry = ProtoPreconditions.CheckNotNull<TypeRegistry>(typeRegistry, "typeRegistry");
				this.IgnoreUnknownFields = ignoreUnknownFields;
			}

			// Token: 0x06000932 RID: 2354 RVA: 0x0001F403 File Offset: 0x0001D603
			public Settings(int recursionLimit)
				: this(recursionLimit, TypeRegistry.Empty)
			{
			}

			// Token: 0x06000933 RID: 2355 RVA: 0x0001F411 File Offset: 0x0001D611
			public Settings(int recursionLimit, TypeRegistry typeRegistry)
				: this(recursionLimit, typeRegistry, false)
			{
			}

			// Token: 0x06000934 RID: 2356 RVA: 0x0001F41C File Offset: 0x0001D61C
			public JsonParser.Settings WithIgnoreUnknownFields(bool ignoreUnknownFields)
			{
				return new JsonParser.Settings(this.RecursionLimit, this.TypeRegistry, ignoreUnknownFields);
			}

			// Token: 0x06000935 RID: 2357 RVA: 0x0001F430 File Offset: 0x0001D630
			public JsonParser.Settings WithRecursionLimit(int recursionLimit)
			{
				return new JsonParser.Settings(recursionLimit, this.TypeRegistry, this.IgnoreUnknownFields);
			}

			// Token: 0x06000936 RID: 2358 RVA: 0x0001F444 File Offset: 0x0001D644
			public JsonParser.Settings WithTypeRegistry(TypeRegistry typeRegistry)
			{
				return new JsonParser.Settings(this.RecursionLimit, ProtoPreconditions.CheckNotNull<TypeRegistry>(typeRegistry, "typeRegistry"), this.IgnoreUnknownFields);
			}
		}
	}
}
