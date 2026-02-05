using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Google.Protobuf.Reflection;
using Google.Protobuf.WellKnownTypes;

namespace Google.Protobuf
{
	// Token: 0x0200001A RID: 26
	public sealed class JsonFormatter
	{
		// Token: 0x17000026 RID: 38
		// (get) Token: 0x0600015B RID: 347 RVA: 0x000063B3 File Offset: 0x000045B3
		public static JsonFormatter Default { get; } = new JsonFormatter(JsonFormatter.Settings.Default);

		// Token: 0x0600015C RID: 348 RVA: 0x000063BC File Offset: 0x000045BC
		static JsonFormatter()
		{
			for (int i = 0; i < JsonFormatter.CommonRepresentations.Length; i++)
			{
				if (JsonFormatter.CommonRepresentations[i] == "")
				{
					JsonFormatter.CommonRepresentations[i] = ((char)i).ToString();
				}
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x0600015D RID: 349 RVA: 0x00006A23 File Offset: 0x00004C23
		private bool DiagnosticOnly
		{
			get
			{
				return this == JsonFormatter.diagnosticFormatter;
			}
		}

		// Token: 0x0600015E RID: 350 RVA: 0x00006A2D File Offset: 0x00004C2D
		public JsonFormatter(JsonFormatter.Settings settings)
		{
			this.settings = ProtoPreconditions.CheckNotNull<JsonFormatter.Settings>(settings, "settings");
		}

		// Token: 0x0600015F RID: 351 RVA: 0x00006A48 File Offset: 0x00004C48
		public string Format(IMessage message)
		{
			StringWriter stringWriter = new StringWriter();
			this.Format(message, stringWriter);
			return stringWriter.ToString();
		}

		// Token: 0x06000160 RID: 352 RVA: 0x00006A69 File Offset: 0x00004C69
		public void Format(IMessage message, TextWriter writer)
		{
			ProtoPreconditions.CheckNotNull<IMessage>(message, "message");
			ProtoPreconditions.CheckNotNull<TextWriter>(writer, "writer");
			if (message.Descriptor.IsWellKnownType)
			{
				this.WriteWellKnownTypeValue(writer, message.Descriptor, message);
				return;
			}
			this.WriteMessage(writer, message);
		}

		// Token: 0x06000161 RID: 353 RVA: 0x00006AA7 File Offset: 0x00004CA7
		public static string ToDiagnosticString(IMessage message)
		{
			ProtoPreconditions.CheckNotNull<IMessage>(message, "message");
			return JsonFormatter.diagnosticFormatter.Format(message);
		}

		// Token: 0x06000162 RID: 354 RVA: 0x00006AC0 File Offset: 0x00004CC0
		private void WriteMessage(TextWriter writer, IMessage message)
		{
			if (message == null)
			{
				JsonFormatter.WriteNull(writer);
				return;
			}
			if (this.DiagnosticOnly)
			{
				ICustomDiagnosticMessage customDiagnosticMessage = message as ICustomDiagnosticMessage;
				if (customDiagnosticMessage != null)
				{
					writer.Write(customDiagnosticMessage.ToDiagnosticString());
					return;
				}
			}
			writer.Write("{ ");
			writer.Write(this.WriteMessageFields(writer, message, false) ? " }" : "}");
		}

		// Token: 0x06000163 RID: 355 RVA: 0x00006B20 File Offset: 0x00004D20
		private bool WriteMessageFields(TextWriter writer, IMessage message, bool assumeFirstFieldWritten)
		{
			MessageDescriptor.FieldCollection fields = message.Descriptor.Fields;
			bool flag = !assumeFirstFieldWritten;
			foreach (FieldDescriptor fieldDescriptor in fields.InFieldNumberOrder())
			{
				IFieldAccessor accessor = fieldDescriptor.Accessor;
				if (fieldDescriptor.ContainingOneof == null || fieldDescriptor.ContainingOneof.Accessor.GetCaseFieldDescriptor(message) == fieldDescriptor)
				{
					object value = accessor.GetValue(message);
					if (fieldDescriptor.ContainingOneof != null || this.settings.FormatDefaultValues || !JsonFormatter.IsDefaultValue(accessor, value))
					{
						if (!flag)
						{
							writer.Write(", ");
						}
						JsonFormatter.WriteString(writer, accessor.Descriptor.JsonName);
						writer.Write(": ");
						this.WriteValue(writer, value);
						flag = false;
					}
				}
			}
			return !flag;
		}

		// Token: 0x06000164 RID: 356 RVA: 0x00006C00 File Offset: 0x00004E00
		internal static string ToJsonName(string name)
		{
			StringBuilder stringBuilder = new StringBuilder(name.Length);
			bool flag = false;
			foreach (char c in name)
			{
				if (c == '_')
				{
					flag = true;
				}
				else if (flag)
				{
					stringBuilder.Append(char.ToUpperInvariant(c));
					flag = false;
				}
				else
				{
					stringBuilder.Append(c);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000165 RID: 357 RVA: 0x00006C68 File Offset: 0x00004E68
		internal static string FromJsonName(string name)
		{
			StringBuilder stringBuilder = new StringBuilder(name.Length);
			foreach (char c in name)
			{
				if (char.IsUpper(c))
				{
					stringBuilder.Append('_');
					stringBuilder.Append(char.ToLowerInvariant(c));
				}
				else
				{
					stringBuilder.Append(c);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000166 RID: 358 RVA: 0x00006CCA File Offset: 0x00004ECA
		private static void WriteNull(TextWriter writer)
		{
			writer.Write("null");
		}

		// Token: 0x06000167 RID: 359 RVA: 0x00006CD8 File Offset: 0x00004ED8
		private static bool IsDefaultValue(IFieldAccessor accessor, object value)
		{
			if (accessor.Descriptor.IsMap)
			{
				return ((IDictionary)value).Count == 0;
			}
			if (accessor.Descriptor.IsRepeated)
			{
				return ((IList)value).Count == 0;
			}
			switch (accessor.Descriptor.FieldType)
			{
			case FieldType.Double:
				return (double)value == 0.0;
			case FieldType.Float:
				return (float)value == 0f;
			case FieldType.Int64:
			case FieldType.SFixed64:
			case FieldType.SInt64:
				return (long)value == 0L;
			case FieldType.UInt64:
			case FieldType.Fixed64:
				return (ulong)value == 0UL;
			case FieldType.Int32:
			case FieldType.SFixed32:
			case FieldType.SInt32:
			case FieldType.Enum:
				return (int)value == 0;
			case FieldType.Fixed32:
			case FieldType.UInt32:
				return (uint)value == 0U;
			case FieldType.Bool:
				return !(bool)value;
			case FieldType.String:
				return (string)value == "";
			case FieldType.Group:
			case FieldType.Message:
				return value == null;
			case FieldType.Bytes:
				return (ByteString)value == ByteString.Empty;
			default:
				throw new ArgumentException("Invalid field type");
			}
		}

		// Token: 0x06000168 RID: 360 RVA: 0x00006E00 File Offset: 0x00005000
		public void WriteValue(TextWriter writer, object value)
		{
			if (value == null)
			{
				JsonFormatter.WriteNull(writer);
				return;
			}
			if (value is bool)
			{
				writer.Write(((bool)value) ? "true" : "false");
				return;
			}
			if (value is ByteString)
			{
				writer.Write('"');
				writer.Write(((ByteString)value).ToBase64());
				writer.Write('"');
				return;
			}
			if (value is string)
			{
				JsonFormatter.WriteString(writer, (string)value);
				return;
			}
			if (value is IDictionary)
			{
				this.WriteDictionary(writer, (IDictionary)value);
				return;
			}
			if (value is IList)
			{
				this.WriteList(writer, (IList)value);
				return;
			}
			if (value is int || value is uint)
			{
				IFormattable formattable = (IFormattable)value;
				writer.Write(formattable.ToString("d", CultureInfo.InvariantCulture));
				return;
			}
			if (value is long || value is ulong)
			{
				writer.Write('"');
				IFormattable formattable2 = (IFormattable)value;
				writer.Write(formattable2.ToString("d", CultureInfo.InvariantCulture));
				writer.Write('"');
				return;
			}
			if (value is Enum)
			{
				if (this.settings.FormatEnumsAsIntegers)
				{
					this.WriteValue(writer, (int)value);
					return;
				}
				string originalName = JsonFormatter.OriginalEnumValueHelper.GetOriginalName(value);
				if (originalName != null)
				{
					JsonFormatter.WriteString(writer, originalName);
					return;
				}
				this.WriteValue(writer, (int)value);
				return;
			}
			else if (value is float || value is double)
			{
				string text = ((IFormattable)value).ToString("r", CultureInfo.InvariantCulture);
				if (text == "NaN" || text == "Infinity" || text == "-Infinity")
				{
					writer.Write('"');
					writer.Write(text);
					writer.Write('"');
					return;
				}
				writer.Write(text);
				return;
			}
			else
			{
				if (value is IMessage)
				{
					this.Format((IMessage)value, writer);
					return;
				}
				string text2 = "Unable to format value of type ";
				Type type = value.GetType();
				throw new ArgumentException(text2 + ((type != null) ? type.ToString() : null));
			}
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00007004 File Offset: 0x00005204
		private void WriteWellKnownTypeValue(TextWriter writer, MessageDescriptor descriptor, object value)
		{
			if (value == null)
			{
				JsonFormatter.WriteNull(writer);
				return;
			}
			if (descriptor.IsWrapperType)
			{
				if (value is IMessage)
				{
					IMessage message = (IMessage)value;
					value = message.Descriptor.Fields[1].Accessor.GetValue(message);
				}
				this.WriteValue(writer, value);
				return;
			}
			if (descriptor.FullName == Timestamp.Descriptor.FullName)
			{
				this.WriteTimestamp(writer, (IMessage)value);
				return;
			}
			if (descriptor.FullName == Duration.Descriptor.FullName)
			{
				this.WriteDuration(writer, (IMessage)value);
				return;
			}
			if (descriptor.FullName == FieldMask.Descriptor.FullName)
			{
				this.WriteFieldMask(writer, (IMessage)value);
				return;
			}
			if (descriptor.FullName == Struct.Descriptor.FullName)
			{
				this.WriteStruct(writer, (IMessage)value);
				return;
			}
			if (descriptor.FullName == ListValue.Descriptor.FullName)
			{
				IFieldAccessor accessor = descriptor.Fields[1].Accessor;
				this.WriteList(writer, (IList)accessor.GetValue((IMessage)value));
				return;
			}
			if (descriptor.FullName == Value.Descriptor.FullName)
			{
				this.WriteStructFieldValue(writer, (IMessage)value);
				return;
			}
			if (descriptor.FullName == Any.Descriptor.FullName)
			{
				this.WriteAny(writer, (IMessage)value);
				return;
			}
			this.WriteMessage(writer, (IMessage)value);
		}

		// Token: 0x0600016A RID: 362 RVA: 0x00007188 File Offset: 0x00005388
		private void WriteTimestamp(TextWriter writer, IMessage value)
		{
			int num = (int)value.Descriptor.Fields[2].Accessor.GetValue(value);
			long num2 = (long)value.Descriptor.Fields[1].Accessor.GetValue(value);
			writer.Write(Timestamp.ToJson(num2, num, this.DiagnosticOnly));
		}

		// Token: 0x0600016B RID: 363 RVA: 0x000071EC File Offset: 0x000053EC
		private void WriteDuration(TextWriter writer, IMessage value)
		{
			int num = (int)value.Descriptor.Fields[2].Accessor.GetValue(value);
			long num2 = (long)value.Descriptor.Fields[1].Accessor.GetValue(value);
			writer.Write(Duration.ToJson(num2, num, this.DiagnosticOnly));
		}

		// Token: 0x0600016C RID: 364 RVA: 0x00007250 File Offset: 0x00005450
		private void WriteFieldMask(TextWriter writer, IMessage value)
		{
			IList<string> list = (IList<string>)value.Descriptor.Fields[1].Accessor.GetValue(value);
			writer.Write(FieldMask.ToJson(list, this.DiagnosticOnly));
		}

		// Token: 0x0600016D RID: 365 RVA: 0x00007294 File Offset: 0x00005494
		private void WriteAny(TextWriter writer, IMessage value)
		{
			if (this.DiagnosticOnly)
			{
				this.WriteDiagnosticOnlyAny(writer, value);
				return;
			}
			string text = (string)value.Descriptor.Fields[1].Accessor.GetValue(value);
			ByteString byteString = (ByteString)value.Descriptor.Fields[2].Accessor.GetValue(value);
			string typeName = Any.GetTypeName(text);
			MessageDescriptor messageDescriptor = this.settings.TypeRegistry.Find(typeName);
			if (messageDescriptor == null)
			{
				throw new InvalidOperationException("Type registry has no descriptor for type name '" + typeName + "'");
			}
			IMessage message = messageDescriptor.Parser.ParseFrom(byteString);
			writer.Write("{ ");
			JsonFormatter.WriteString(writer, "@type");
			writer.Write(": ");
			JsonFormatter.WriteString(writer, text);
			if (messageDescriptor.IsWellKnownType)
			{
				writer.Write(", ");
				JsonFormatter.WriteString(writer, "value");
				writer.Write(": ");
				this.WriteWellKnownTypeValue(writer, messageDescriptor, message);
			}
			else
			{
				this.WriteMessageFields(writer, message, true);
			}
			writer.Write(" }");
		}

		// Token: 0x0600016E RID: 366 RVA: 0x000073AC File Offset: 0x000055AC
		private void WriteDiagnosticOnlyAny(TextWriter writer, IMessage value)
		{
			string text = (string)value.Descriptor.Fields[1].Accessor.GetValue(value);
			ByteString byteString = (ByteString)value.Descriptor.Fields[2].Accessor.GetValue(value);
			writer.Write("{ ");
			JsonFormatter.WriteString(writer, "@type");
			writer.Write(": ");
			JsonFormatter.WriteString(writer, text);
			writer.Write(", ");
			JsonFormatter.WriteString(writer, "@value");
			writer.Write(": ");
			writer.Write('"');
			writer.Write(byteString.ToBase64());
			writer.Write('"');
			writer.Write(" }");
		}

		// Token: 0x0600016F RID: 367 RVA: 0x00007470 File Offset: 0x00005670
		private void WriteStruct(TextWriter writer, IMessage message)
		{
			writer.Write("{ ");
			IDictionary dictionary = (IDictionary)message.Descriptor.Fields[1].Accessor.GetValue(message);
			bool flag = true;
			foreach (object obj in dictionary)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				string text = (string)dictionaryEntry.Key;
				IMessage message2 = (IMessage)dictionaryEntry.Value;
				if (string.IsNullOrEmpty(text) || message2 == null)
				{
					throw new InvalidOperationException("Struct fields cannot have an empty key or a null value.");
				}
				if (!flag)
				{
					writer.Write(", ");
				}
				JsonFormatter.WriteString(writer, text);
				writer.Write(": ");
				this.WriteStructFieldValue(writer, message2);
				flag = false;
			}
			writer.Write(flag ? "}" : " }");
		}

		// Token: 0x06000170 RID: 368 RVA: 0x00007560 File Offset: 0x00005760
		private void WriteStructFieldValue(TextWriter writer, IMessage message)
		{
			FieldDescriptor caseFieldDescriptor = message.Descriptor.Oneofs[0].Accessor.GetCaseFieldDescriptor(message);
			if (caseFieldDescriptor == null)
			{
				throw new InvalidOperationException("Value message must contain a value for the oneof.");
			}
			object value = caseFieldDescriptor.Accessor.GetValue(message);
			switch (caseFieldDescriptor.FieldNumber)
			{
			case 1:
				JsonFormatter.WriteNull(writer);
				return;
			case 2:
			case 3:
			case 4:
				this.WriteValue(writer, value);
				return;
			case 5:
			case 6:
			{
				IMessage message2 = (IMessage)caseFieldDescriptor.Accessor.GetValue(message);
				this.WriteWellKnownTypeValue(writer, message2.Descriptor, message2);
				return;
			}
			default:
				throw new InvalidOperationException("Unexpected case in struct field: " + caseFieldDescriptor.FieldNumber.ToString());
			}
		}

		// Token: 0x06000171 RID: 369 RVA: 0x00007620 File Offset: 0x00005820
		internal void WriteList(TextWriter writer, IList list)
		{
			writer.Write("[ ");
			bool flag = true;
			foreach (object obj in list)
			{
				if (!flag)
				{
					writer.Write(", ");
				}
				this.WriteValue(writer, obj);
				flag = false;
			}
			writer.Write(flag ? "]" : " ]");
		}

		// Token: 0x06000172 RID: 370 RVA: 0x000076A4 File Offset: 0x000058A4
		internal void WriteDictionary(TextWriter writer, IDictionary dictionary)
		{
			writer.Write("{ ");
			bool flag = true;
			foreach (object obj in dictionary)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				if (!flag)
				{
					writer.Write(", ");
				}
				string text;
				if (dictionaryEntry.Key is string)
				{
					text = (string)dictionaryEntry.Key;
				}
				else if (dictionaryEntry.Key is bool)
				{
					text = (((bool)dictionaryEntry.Key) ? "true" : "false");
				}
				else if (dictionaryEntry.Key is int || ((dictionaryEntry.Key is uint) | (dictionaryEntry.Key is long)) || dictionaryEntry.Key is ulong)
				{
					text = ((IFormattable)dictionaryEntry.Key).ToString("d", CultureInfo.InvariantCulture);
				}
				else
				{
					if (dictionaryEntry.Key == null)
					{
						throw new ArgumentException("Dictionary has entry with null key");
					}
					string text2 = "Unhandled dictionary key type: ";
					Type type = dictionaryEntry.Key.GetType();
					throw new ArgumentException(text2 + ((type != null) ? type.ToString() : null));
				}
				JsonFormatter.WriteString(writer, text);
				writer.Write(": ");
				this.WriteValue(writer, dictionaryEntry.Value);
				flag = false;
			}
			writer.Write(flag ? "}" : " }");
		}

		// Token: 0x06000173 RID: 371 RVA: 0x00007840 File Offset: 0x00005A40
		internal static void WriteString(TextWriter writer, string text)
		{
			writer.Write('"');
			for (int i = 0; i < text.Length; i++)
			{
				char c = text[i];
				if (c < '\u00a0')
				{
					writer.Write(JsonFormatter.CommonRepresentations[(int)c]);
				}
				else if (char.IsHighSurrogate(c))
				{
					i++;
					if (i == text.Length || !char.IsLowSurrogate(text[i]))
					{
						throw new ArgumentException("String contains low surrogate not followed by high surrogate");
					}
					JsonFormatter.HexEncodeUtf16CodeUnit(writer, c);
					JsonFormatter.HexEncodeUtf16CodeUnit(writer, text[i]);
				}
				else
				{
					if (char.IsLowSurrogate(c))
					{
						throw new ArgumentException("String contains high surrogate not preceded by low surrogate");
					}
					uint num = (uint)c;
					if (num <= 1807U)
					{
						if (num != 173U && num != 1757U && num != 1807U)
						{
							goto IL_00D4;
						}
					}
					else if (num - 6068U > 1U && num != 65279U && num - 65529U > 2U)
					{
						goto IL_00D4;
					}
					JsonFormatter.HexEncodeUtf16CodeUnit(writer, c);
					goto IL_0134;
					IL_00D4:
					if ((c >= '\u0600' && c <= '\u0603') || (c >= '\u200b' && c <= '\u200f') || (c >= '\u2028' && c <= '\u202e') || (c >= '\u2060' && c <= '\u2064') || (c >= '\u206a' && c <= '\u206f'))
					{
						JsonFormatter.HexEncodeUtf16CodeUnit(writer, c);
					}
					else
					{
						writer.Write(c);
					}
				}
				IL_0134:;
			}
			writer.Write('"');
		}

		// Token: 0x06000174 RID: 372 RVA: 0x0000799C File Offset: 0x00005B9C
		private static void HexEncodeUtf16CodeUnit(TextWriter writer, char c)
		{
			writer.Write("\\u");
			writer.Write("0123456789abcdef"[(int)((c >> 12) & '\u000f')]);
			writer.Write("0123456789abcdef"[(int)((c >> 8) & '\u000f')]);
			writer.Write("0123456789abcdef"[(int)((c >> 4) & '\u000f')]);
			writer.Write("0123456789abcdef"[(int)(c & '\u000f')]);
		}

		// Token: 0x0400003D RID: 61
		internal const string AnyTypeUrlField = "@type";

		// Token: 0x0400003E RID: 62
		internal const string AnyDiagnosticValueField = "@value";

		// Token: 0x0400003F RID: 63
		internal const string AnyWellKnownTypeValueField = "value";

		// Token: 0x04000040 RID: 64
		private const string TypeUrlPrefix = "type.googleapis.com";

		// Token: 0x04000041 RID: 65
		private const string NameValueSeparator = ": ";

		// Token: 0x04000042 RID: 66
		private const string PropertySeparator = ", ";

		// Token: 0x04000044 RID: 68
		private static readonly JsonFormatter diagnosticFormatter = new JsonFormatter(JsonFormatter.Settings.Default);

		// Token: 0x04000045 RID: 69
		private static readonly string[] CommonRepresentations = new string[]
		{
			"\\u0000", "\\u0001", "\\u0002", "\\u0003", "\\u0004", "\\u0005", "\\u0006", "\\u0007", "\\b", "\\t",
			"\\n", "\\u000b", "\\f", "\\r", "\\u000e", "\\u000f", "\\u0010", "\\u0011", "\\u0012", "\\u0013",
			"\\u0014", "\\u0015", "\\u0016", "\\u0017", "\\u0018", "\\u0019", "\\u001a", "\\u001b", "\\u001c", "\\u001d",
			"\\u001e", "\\u001f", "", "", "\\\"", "", "", "", "", "",
			"", "", "", "", "", "", "", "", "", "",
			"", "", "", "", "", "", "", "", "", "",
			"\\u003c", "", "\\u003e", "", "", "", "", "", "", "",
			"", "", "", "", "", "", "", "", "", "",
			"", "", "", "", "", "", "", "", "", "",
			"", "", "\\\\", "", "", "", "", "", "", "",
			"", "", "", "", "", "", "", "", "", "",
			"", "", "", "", "", "", "", "", "", "",
			"", "", "", "", "", "", "", "\\u007f", "\\u0080", "\\u0081",
			"\\u0082", "\\u0083", "\\u0084", "\\u0085", "\\u0086", "\\u0087", "\\u0088", "\\u0089", "\\u008a", "\\u008b",
			"\\u008c", "\\u008d", "\\u008e", "\\u008f", "\\u0090", "\\u0091", "\\u0092", "\\u0093", "\\u0094", "\\u0095",
			"\\u0096", "\\u0097", "\\u0098", "\\u0099", "\\u009a", "\\u009b", "\\u009c", "\\u009d", "\\u009e", "\\u009f"
		};

		// Token: 0x04000046 RID: 70
		private readonly JsonFormatter.Settings settings;

		// Token: 0x04000047 RID: 71
		private const string Hex = "0123456789abcdef";

		// Token: 0x020000A1 RID: 161
		public sealed class Settings
		{
			// Token: 0x17000261 RID: 609
			// (get) Token: 0x0600091E RID: 2334 RVA: 0x0001F1E7 File Offset: 0x0001D3E7
			public static JsonFormatter.Settings Default { get; } = new JsonFormatter.Settings(false);

			// Token: 0x17000262 RID: 610
			// (get) Token: 0x06000920 RID: 2336 RVA: 0x0001F1FB File Offset: 0x0001D3FB
			public bool FormatDefaultValues { get; }

			// Token: 0x17000263 RID: 611
			// (get) Token: 0x06000921 RID: 2337 RVA: 0x0001F203 File Offset: 0x0001D403
			public TypeRegistry TypeRegistry { get; }

			// Token: 0x17000264 RID: 612
			// (get) Token: 0x06000922 RID: 2338 RVA: 0x0001F20B File Offset: 0x0001D40B
			public bool FormatEnumsAsIntegers { get; }

			// Token: 0x06000923 RID: 2339 RVA: 0x0001F213 File Offset: 0x0001D413
			public Settings(bool formatDefaultValues)
				: this(formatDefaultValues, TypeRegistry.Empty)
			{
			}

			// Token: 0x06000924 RID: 2340 RVA: 0x0001F221 File Offset: 0x0001D421
			public Settings(bool formatDefaultValues, TypeRegistry typeRegistry)
				: this(formatDefaultValues, typeRegistry, false)
			{
			}

			// Token: 0x06000925 RID: 2341 RVA: 0x0001F22C File Offset: 0x0001D42C
			private Settings(bool formatDefaultValues, TypeRegistry typeRegistry, bool formatEnumsAsIntegers)
			{
				this.FormatDefaultValues = formatDefaultValues;
				this.TypeRegistry = typeRegistry ?? TypeRegistry.Empty;
				this.FormatEnumsAsIntegers = formatEnumsAsIntegers;
			}

			// Token: 0x06000926 RID: 2342 RVA: 0x0001F252 File Offset: 0x0001D452
			public JsonFormatter.Settings WithFormatDefaultValues(bool formatDefaultValues)
			{
				return new JsonFormatter.Settings(formatDefaultValues, this.TypeRegistry, this.FormatEnumsAsIntegers);
			}

			// Token: 0x06000927 RID: 2343 RVA: 0x0001F266 File Offset: 0x0001D466
			public JsonFormatter.Settings WithTypeRegistry(TypeRegistry typeRegistry)
			{
				return new JsonFormatter.Settings(this.FormatDefaultValues, typeRegistry, this.FormatEnumsAsIntegers);
			}

			// Token: 0x06000928 RID: 2344 RVA: 0x0001F27A File Offset: 0x0001D47A
			public JsonFormatter.Settings WithFormatEnumsAsIntegers(bool formatEnumsAsIntegers)
			{
				return new JsonFormatter.Settings(this.FormatDefaultValues, this.TypeRegistry, formatEnumsAsIntegers);
			}
		}

		// Token: 0x020000A2 RID: 162
		private static class OriginalEnumValueHelper
		{
			// Token: 0x06000929 RID: 2345 RVA: 0x0001F290 File Offset: 0x0001D490
			internal static string GetOriginalName(object value)
			{
				Type type = value.GetType();
				Dictionary<Type, Dictionary<object, string>> dictionary = JsonFormatter.OriginalEnumValueHelper.dictionaries;
				Dictionary<object, string> nameMapping;
				lock (dictionary)
				{
					if (!JsonFormatter.OriginalEnumValueHelper.dictionaries.TryGetValue(type, out nameMapping))
					{
						nameMapping = JsonFormatter.OriginalEnumValueHelper.GetNameMapping(type);
						JsonFormatter.OriginalEnumValueHelper.dictionaries[type] = nameMapping;
					}
				}
				string text;
				nameMapping.TryGetValue(value, out text);
				return text;
			}

			// Token: 0x0600092A RID: 2346 RVA: 0x0001F300 File Offset: 0x0001D500
			private static Dictionary<object, string> GetNameMapping(Type enumType)
			{
				return enumType.GetTypeInfo().DeclaredFields.Where((FieldInfo f) => f.IsStatic).Where(delegate(FieldInfo f)
				{
					OriginalNameAttribute originalNameAttribute = f.GetCustomAttributes<OriginalNameAttribute>().FirstOrDefault<OriginalNameAttribute>();
					return originalNameAttribute == null || originalNameAttribute.PreferredAlias;
				}).ToDictionary((FieldInfo f) => f.GetValue(null), delegate(FieldInfo f)
				{
					OriginalNameAttribute originalNameAttribute2 = f.GetCustomAttributes<OriginalNameAttribute>().FirstOrDefault<OriginalNameAttribute>();
					return ((originalNameAttribute2 != null) ? originalNameAttribute2.Name : null) ?? f.Name;
				});
			}

			// Token: 0x040003A0 RID: 928
			private static readonly Dictionary<Type, Dictionary<object, string>> dictionaries = new Dictionary<Type, Dictionary<object, string>>();
		}
	}
}
