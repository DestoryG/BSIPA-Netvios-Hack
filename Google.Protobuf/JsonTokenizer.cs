using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace Google.Protobuf
{
	// Token: 0x0200001D RID: 29
	internal abstract class JsonTokenizer
	{
		// Token: 0x060001A8 RID: 424 RVA: 0x0000933B File Offset: 0x0000753B
		internal static JsonTokenizer FromTextReader(TextReader reader)
		{
			return new JsonTokenizer.JsonTextTokenizer(reader);
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x00009343 File Offset: 0x00007543
		internal static JsonTokenizer FromReplayedTokens(IList<JsonToken> tokens, JsonTokenizer continuation)
		{
			return new JsonTokenizer.JsonReplayTokenizer(tokens, continuation);
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060001AA RID: 426 RVA: 0x0000934C File Offset: 0x0000754C
		// (set) Token: 0x060001AB RID: 427 RVA: 0x00009354 File Offset: 0x00007554
		internal int ObjectDepth { get; private set; }

		// Token: 0x060001AC RID: 428 RVA: 0x00009360 File Offset: 0x00007560
		internal void PushBack(JsonToken token)
		{
			if (this.bufferedToken != null)
			{
				throw new InvalidOperationException("Can't push back twice");
			}
			this.bufferedToken = token;
			if (token.Type == JsonToken.TokenType.StartObject)
			{
				int num = this.ObjectDepth;
				this.ObjectDepth = num - 1;
				return;
			}
			if (token.Type == JsonToken.TokenType.EndObject)
			{
				int num = this.ObjectDepth;
				this.ObjectDepth = num + 1;
			}
		}

		// Token: 0x060001AD RID: 429 RVA: 0x000093BC File Offset: 0x000075BC
		internal JsonToken Next()
		{
			JsonToken jsonToken;
			if (this.bufferedToken != null)
			{
				jsonToken = this.bufferedToken;
				this.bufferedToken = null;
			}
			else
			{
				jsonToken = this.NextImpl();
			}
			if (jsonToken.Type == JsonToken.TokenType.StartObject)
			{
				int num = this.ObjectDepth;
				this.ObjectDepth = num + 1;
			}
			else if (jsonToken.Type == JsonToken.TokenType.EndObject)
			{
				int num = this.ObjectDepth;
				this.ObjectDepth = num - 1;
			}
			return jsonToken;
		}

		// Token: 0x060001AE RID: 430
		protected abstract JsonToken NextImpl();

		// Token: 0x060001AF RID: 431 RVA: 0x00009420 File Offset: 0x00007620
		internal void SkipValue()
		{
			int num = 0;
			do
			{
				switch (this.Next().Type)
				{
				case JsonToken.TokenType.StartObject:
				case JsonToken.TokenType.StartArray:
					num++;
					break;
				case JsonToken.TokenType.EndObject:
				case JsonToken.TokenType.EndArray:
					num--;
					break;
				}
			}
			while (num != 0);
		}

		// Token: 0x0400005A RID: 90
		private JsonToken bufferedToken;

		// Token: 0x020000A6 RID: 166
		private class JsonReplayTokenizer : JsonTokenizer
		{
			// Token: 0x06000940 RID: 2368 RVA: 0x0001F4D9 File Offset: 0x0001D6D9
			internal JsonReplayTokenizer(IList<JsonToken> tokens, JsonTokenizer nextTokenizer)
			{
				this.tokens = tokens;
				this.nextTokenizer = nextTokenizer;
			}

			// Token: 0x06000941 RID: 2369 RVA: 0x0001F4F0 File Offset: 0x0001D6F0
			protected override JsonToken NextImpl()
			{
				if (this.nextTokenIndex >= this.tokens.Count)
				{
					return this.nextTokenizer.Next();
				}
				IList<JsonToken> list = this.tokens;
				int num = this.nextTokenIndex;
				this.nextTokenIndex = num + 1;
				return list[num];
			}

			// Token: 0x040003B2 RID: 946
			private readonly IList<JsonToken> tokens;

			// Token: 0x040003B3 RID: 947
			private readonly JsonTokenizer nextTokenizer;

			// Token: 0x040003B4 RID: 948
			private int nextTokenIndex;
		}

		// Token: 0x020000A7 RID: 167
		private sealed class JsonTextTokenizer : JsonTokenizer
		{
			// Token: 0x06000942 RID: 2370 RVA: 0x0001F538 File Offset: 0x0001D738
			internal JsonTextTokenizer(TextReader reader)
			{
				this.reader = new JsonTokenizer.JsonTextTokenizer.PushBackReader(reader);
				this.state = JsonTokenizer.JsonTextTokenizer.State.StartOfDocument;
				this.containerStack.Push(JsonTokenizer.JsonTextTokenizer.ContainerType.Document);
			}

			// Token: 0x06000943 RID: 2371 RVA: 0x0001F56C File Offset: 0x0001D76C
			protected override JsonToken NextImpl()
			{
				if (this.state == JsonTokenizer.JsonTextTokenizer.State.ReaderExhausted)
				{
					throw new InvalidOperationException("Next() called after end of document");
				}
				char? c;
				char value;
				for (;;)
				{
					c = this.reader.Read();
					if (c == null)
					{
						break;
					}
					value = c.Value;
					if (value > ']')
					{
						goto IL_0106;
					}
					if (value > ':')
					{
						goto IL_00F1;
					}
					switch (value)
					{
					case '\t':
					case '\n':
					case '\r':
						break;
					case '\v':
					case '\f':
						goto IL_02BB;
					default:
						switch (value)
						{
						case ' ':
							continue;
						case '"':
							goto IL_0188;
						case ',':
							this.ValidateState(JsonTokenizer.JsonTextTokenizer.State.ObjectAfterProperty | JsonTokenizer.JsonTextTokenizer.State.ArrayAfterValue, "Invalid state to read a comma: ");
							this.state = ((this.state == JsonTokenizer.JsonTextTokenizer.State.ObjectAfterProperty) ? JsonTokenizer.JsonTextTokenizer.State.ObjectAfterComma : JsonTokenizer.JsonTextTokenizer.State.ArrayAfterComma);
							continue;
						case '-':
						case '0':
						case '1':
						case '2':
						case '3':
						case '4':
						case '5':
						case '6':
						case '7':
						case '8':
						case '9':
							goto IL_029D;
						case ':':
							this.ValidateState(JsonTokenizer.JsonTextTokenizer.State.ObjectBeforeColon, "Invalid state to read a colon: ");
							this.state = JsonTokenizer.JsonTextTokenizer.State.ObjectAfterColon;
							continue;
						}
						goto Block_6;
					}
				}
				this.ValidateState(JsonTokenizer.JsonTextTokenizer.State.ExpectedEndOfDocument, "Unexpected end of document in state: ");
				this.state = JsonTokenizer.JsonTextTokenizer.State.ReaderExhausted;
				return JsonToken.EndDocument;
				Block_6:
				goto IL_02BB;
				IL_00F1:
				if (value == '[')
				{
					this.ValidateState(JsonTokenizer.JsonTextTokenizer.ValueStates, "Invalid state to read an open square bracket: ");
					this.state = JsonTokenizer.JsonTextTokenizer.State.ArrayStart;
					this.containerStack.Push(JsonTokenizer.JsonTextTokenizer.ContainerType.Array);
					return JsonToken.StartArray;
				}
				if (value != ']')
				{
					goto IL_02BB;
				}
				this.ValidateState(JsonTokenizer.JsonTextTokenizer.State.ArrayStart | JsonTokenizer.JsonTextTokenizer.State.ArrayAfterValue, "Invalid state to read a close square bracket: ");
				this.PopContainer();
				return JsonToken.EndArray;
				IL_0106:
				if (value <= 'n')
				{
					if (value == 'f')
					{
						this.ConsumeLiteral("false");
						this.ValidateAndModifyStateForValue("Invalid state to read a false literal: ");
						return JsonToken.False;
					}
					if (value != 'n')
					{
						goto IL_02BB;
					}
					this.ConsumeLiteral("null");
					this.ValidateAndModifyStateForValue("Invalid state to read a null literal: ");
					return JsonToken.Null;
				}
				else
				{
					if (value == 't')
					{
						this.ConsumeLiteral("true");
						this.ValidateAndModifyStateForValue("Invalid state to read a true literal: ");
						return JsonToken.True;
					}
					if (value == '{')
					{
						this.ValidateState(JsonTokenizer.JsonTextTokenizer.ValueStates, "Invalid state to read an open brace: ");
						this.state = JsonTokenizer.JsonTextTokenizer.State.ObjectStart;
						this.containerStack.Push(JsonTokenizer.JsonTextTokenizer.ContainerType.Object);
						return JsonToken.StartObject;
					}
					if (value != '}')
					{
						goto IL_02BB;
					}
					this.ValidateState(JsonTokenizer.JsonTextTokenizer.State.ObjectStart | JsonTokenizer.JsonTextTokenizer.State.ObjectAfterProperty, "Invalid state to read a close brace: ");
					this.PopContainer();
					return JsonToken.EndObject;
				}
				IL_0188:
				string text = this.ReadString();
				if ((this.state & (JsonTokenizer.JsonTextTokenizer.State.ObjectStart | JsonTokenizer.JsonTextTokenizer.State.ObjectAfterComma)) != (JsonTokenizer.JsonTextTokenizer.State)0)
				{
					this.state = JsonTokenizer.JsonTextTokenizer.State.ObjectBeforeColon;
					return JsonToken.Name(text);
				}
				this.ValidateAndModifyStateForValue("Invalid state to read a double quote: ");
				return JsonToken.Value(text);
				IL_029D:
				double num = this.ReadNumber(c.Value);
				this.ValidateAndModifyStateForValue("Invalid state to read a number token: ");
				return JsonToken.Value(num);
				IL_02BB:
				throw new InvalidJsonException("Invalid first character of token: " + c.Value.ToString());
			}

			// Token: 0x06000944 RID: 2372 RVA: 0x0001F852 File Offset: 0x0001DA52
			private void ValidateState(JsonTokenizer.JsonTextTokenizer.State validStates, string errorPrefix)
			{
				if ((validStates & this.state) == (JsonTokenizer.JsonTextTokenizer.State)0)
				{
					throw this.reader.CreateException(errorPrefix + this.state.ToString());
				}
			}

			// Token: 0x06000945 RID: 2373 RVA: 0x0001F884 File Offset: 0x0001DA84
			private string ReadString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				bool flag = false;
				char c;
				for (;;)
				{
					c = this.reader.ReadOrFail("Unexpected end of text while reading string");
					if (c < ' ')
					{
						break;
					}
					if (c == '"')
					{
						goto Block_2;
					}
					if (c == '\\')
					{
						c = this.ReadEscapedCharacter();
					}
					if (flag != char.IsLowSurrogate(c))
					{
						goto Block_5;
					}
					flag = char.IsHighSurrogate(c);
					stringBuilder.Append(c);
				}
				throw this.reader.CreateException(string.Format(CultureInfo.InvariantCulture, "Invalid character in string literal: U+{0:x4}", (int)c));
				Block_2:
				if (flag)
				{
					throw this.reader.CreateException("Invalid use of surrogate pair code units");
				}
				return stringBuilder.ToString();
				Block_5:
				throw this.reader.CreateException("Invalid use of surrogate pair code units");
			}

			// Token: 0x06000946 RID: 2374 RVA: 0x0001F92C File Offset: 0x0001DB2C
			private char ReadEscapedCharacter()
			{
				char c = this.reader.ReadOrFail("Unexpected end of text while reading character escape sequence");
				if (c <= '\\')
				{
					if (c == '"')
					{
						return '"';
					}
					if (c == '/')
					{
						return '/';
					}
					if (c == '\\')
					{
						return '\\';
					}
				}
				else if (c <= 'f')
				{
					if (c == 'b')
					{
						return '\b';
					}
					if (c == 'f')
					{
						return '\f';
					}
				}
				else
				{
					if (c == 'n')
					{
						return '\n';
					}
					switch (c)
					{
					case 'r':
						return '\r';
					case 't':
						return '\t';
					case 'u':
						return this.ReadUnicodeEscape();
					}
				}
				throw this.reader.CreateException(string.Format(CultureInfo.InvariantCulture, "Invalid character in character escape sequence: U+{0:x4}", (int)c));
			}

			// Token: 0x06000947 RID: 2375 RVA: 0x0001F9D0 File Offset: 0x0001DBD0
			private char ReadUnicodeEscape()
			{
				int num = 0;
				for (int i = 0; i < 4; i++)
				{
					char c = this.reader.ReadOrFail("Unexpected end of text while reading Unicode escape sequence");
					int num2;
					if (c >= '0' && c <= '9')
					{
						num2 = (int)(c - '0');
					}
					else if (c >= 'a' && c <= 'f')
					{
						num2 = (int)(c - 'a' + '\n');
					}
					else
					{
						if (c < 'A' || c > 'F')
						{
							throw this.reader.CreateException(string.Format(CultureInfo.InvariantCulture, "Invalid character in character escape sequence: U+{0:x4}", (int)c));
						}
						num2 = (int)(c - 'A' + '\n');
					}
					num = (num << 4) + num2;
				}
				return (char)num;
			}

			// Token: 0x06000948 RID: 2376 RVA: 0x0001FA60 File Offset: 0x0001DC60
			private void ConsumeLiteral(string text)
			{
				for (int i = 1; i < text.Length; i++)
				{
					char? c = this.reader.Read();
					if (c == null)
					{
						throw this.reader.CreateException("Unexpected end of text while reading literal token " + text);
					}
					if (c.Value != text[i])
					{
						throw this.reader.CreateException("Unexpected character while reading literal token " + text);
					}
				}
			}

			// Token: 0x06000949 RID: 2377 RVA: 0x0001FAD4 File Offset: 0x0001DCD4
			private double ReadNumber(char initialCharacter)
			{
				StringBuilder stringBuilder = new StringBuilder();
				if (initialCharacter == '-')
				{
					stringBuilder.Append("-");
				}
				else
				{
					this.reader.PushBack(initialCharacter);
				}
				char? c = this.ReadInt(stringBuilder);
				char? c2 = c;
				int? num = ((c2 != null) ? new int?((int)c2.GetValueOrDefault()) : null);
				int num2 = 46;
				if ((num.GetValueOrDefault() == num2) & (num != null))
				{
					c = this.ReadFrac(stringBuilder);
				}
				c2 = c;
				num = ((c2 != null) ? new int?((int)c2.GetValueOrDefault()) : null);
				num2 = 101;
				if (!((num.GetValueOrDefault() == num2) & (num != null)))
				{
					c2 = c;
					num = ((c2 != null) ? new int?((int)c2.GetValueOrDefault()) : null);
					num2 = 69;
					if (!((num.GetValueOrDefault() == num2) & (num != null)))
					{
						goto IL_00F1;
					}
				}
				c = this.ReadExp(stringBuilder);
				IL_00F1:
				if (c != null)
				{
					this.reader.PushBack(c.Value);
				}
				double num3;
				try
				{
					num3 = double.Parse(stringBuilder.ToString(), NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent, CultureInfo.InvariantCulture);
				}
				catch (OverflowException)
				{
					JsonTokenizer.JsonTextTokenizer.PushBackReader pushBackReader = this.reader;
					string text = "Numeric value out of range: ";
					StringBuilder stringBuilder2 = stringBuilder;
					throw pushBackReader.CreateException(text + ((stringBuilder2 != null) ? stringBuilder2.ToString() : null));
				}
				return num3;
			}

			// Token: 0x0600094A RID: 2378 RVA: 0x0001FC40 File Offset: 0x0001DE40
			private char? ReadInt(StringBuilder builder)
			{
				char c = this.reader.ReadOrFail("Invalid numeric literal");
				if (c < '0' || c > '9')
				{
					throw this.reader.CreateException("Invalid numeric literal");
				}
				builder.Append(c);
				int num;
				char? c2 = this.ConsumeDigits(builder, out num);
				if (c == '0' && num != 0)
				{
					throw this.reader.CreateException("Invalid numeric literal: leading 0 for non-zero value.");
				}
				return c2;
			}

			// Token: 0x0600094B RID: 2379 RVA: 0x0001FCA4 File Offset: 0x0001DEA4
			private char? ReadFrac(StringBuilder builder)
			{
				builder.Append('.');
				int num;
				char? c = this.ConsumeDigits(builder, out num);
				if (num == 0)
				{
					throw this.reader.CreateException("Invalid numeric literal: fraction with no trailing digits");
				}
				return c;
			}

			// Token: 0x0600094C RID: 2380 RVA: 0x0001FCD8 File Offset: 0x0001DED8
			private char? ReadExp(StringBuilder builder)
			{
				builder.Append('E');
				char? c = this.reader.Read();
				if (c == null)
				{
					throw this.reader.CreateException("Invalid numeric literal: exponent with no trailing digits");
				}
				char? c2 = c;
				int? num = ((c2 != null) ? new int?((int)c2.GetValueOrDefault()) : null);
				int num2 = 45;
				if (!((num.GetValueOrDefault() == num2) & (num != null)))
				{
					c2 = c;
					num = ((c2 != null) ? new int?((int)c2.GetValueOrDefault()) : null);
					num2 = 43;
					if (!((num.GetValueOrDefault() == num2) & (num != null)))
					{
						this.reader.PushBack(c.Value);
						goto IL_00C9;
					}
				}
				builder.Append(c.Value);
				IL_00C9:
				int num3;
				c = this.ConsumeDigits(builder, out num3);
				if (num3 == 0)
				{
					throw this.reader.CreateException("Invalid numeric literal: exponent without value");
				}
				return c;
			}

			// Token: 0x0600094D RID: 2381 RVA: 0x0001FDD0 File Offset: 0x0001DFD0
			private char? ConsumeDigits(StringBuilder builder, out int count)
			{
				count = 0;
				char? c;
				for (;;)
				{
					c = this.reader.Read();
					if (c == null || c.Value < '0' || c.Value > '9')
					{
						break;
					}
					count++;
					builder.Append(c.Value);
				}
				return c;
			}

			// Token: 0x0600094E RID: 2382 RVA: 0x0001FE24 File Offset: 0x0001E024
			private void ValidateAndModifyStateForValue(string errorPrefix)
			{
				this.ValidateState(JsonTokenizer.JsonTextTokenizer.ValueStates, errorPrefix);
				JsonTokenizer.JsonTextTokenizer.State state = this.state;
				if (state <= JsonTokenizer.JsonTextTokenizer.State.ObjectAfterColon)
				{
					if (state == JsonTokenizer.JsonTextTokenizer.State.StartOfDocument)
					{
						this.state = JsonTokenizer.JsonTextTokenizer.State.ExpectedEndOfDocument;
						return;
					}
					if (state == JsonTokenizer.JsonTextTokenizer.State.ObjectAfterColon)
					{
						this.state = JsonTokenizer.JsonTextTokenizer.State.ObjectAfterProperty;
						return;
					}
				}
				else if (state == JsonTokenizer.JsonTextTokenizer.State.ArrayStart || state == JsonTokenizer.JsonTextTokenizer.State.ArrayAfterComma)
				{
					this.state = JsonTokenizer.JsonTextTokenizer.State.ArrayAfterValue;
					return;
				}
				throw new InvalidOperationException("ValidateAndModifyStateForValue does not handle all value states (and should)");
			}

			// Token: 0x0600094F RID: 2383 RVA: 0x0001FE90 File Offset: 0x0001E090
			private void PopContainer()
			{
				this.containerStack.Pop();
				JsonTokenizer.JsonTextTokenizer.ContainerType containerType = this.containerStack.Peek();
				switch (containerType)
				{
				case JsonTokenizer.JsonTextTokenizer.ContainerType.Document:
					this.state = JsonTokenizer.JsonTextTokenizer.State.ExpectedEndOfDocument;
					return;
				case JsonTokenizer.JsonTextTokenizer.ContainerType.Object:
					this.state = JsonTokenizer.JsonTextTokenizer.State.ObjectAfterProperty;
					return;
				case JsonTokenizer.JsonTextTokenizer.ContainerType.Array:
					this.state = JsonTokenizer.JsonTextTokenizer.State.ArrayAfterValue;
					return;
				default:
					throw new InvalidOperationException("Unexpected container type: " + containerType.ToString());
				}
			}

			// Token: 0x040003B5 RID: 949
			private static readonly JsonTokenizer.JsonTextTokenizer.State ValueStates = JsonTokenizer.JsonTextTokenizer.State.StartOfDocument | JsonTokenizer.JsonTextTokenizer.State.ObjectAfterColon | JsonTokenizer.JsonTextTokenizer.State.ArrayStart | JsonTokenizer.JsonTextTokenizer.State.ArrayAfterComma;

			// Token: 0x040003B6 RID: 950
			private readonly Stack<JsonTokenizer.JsonTextTokenizer.ContainerType> containerStack = new Stack<JsonTokenizer.JsonTextTokenizer.ContainerType>();

			// Token: 0x040003B7 RID: 951
			private readonly JsonTokenizer.JsonTextTokenizer.PushBackReader reader;

			// Token: 0x040003B8 RID: 952
			private JsonTokenizer.JsonTextTokenizer.State state;

			// Token: 0x0200010E RID: 270
			private enum ContainerType
			{
				// Token: 0x04000445 RID: 1093
				Document,
				// Token: 0x04000446 RID: 1094
				Object,
				// Token: 0x04000447 RID: 1095
				Array
			}

			// Token: 0x0200010F RID: 271
			[Flags]
			private enum State
			{
				// Token: 0x04000449 RID: 1097
				StartOfDocument = 1,
				// Token: 0x0400044A RID: 1098
				ExpectedEndOfDocument = 2,
				// Token: 0x0400044B RID: 1099
				ReaderExhausted = 4,
				// Token: 0x0400044C RID: 1100
				ObjectStart = 8,
				// Token: 0x0400044D RID: 1101
				ObjectBeforeColon = 16,
				// Token: 0x0400044E RID: 1102
				ObjectAfterColon = 32,
				// Token: 0x0400044F RID: 1103
				ObjectAfterProperty = 64,
				// Token: 0x04000450 RID: 1104
				ObjectAfterComma = 128,
				// Token: 0x04000451 RID: 1105
				ArrayStart = 256,
				// Token: 0x04000452 RID: 1106
				ArrayAfterValue = 512,
				// Token: 0x04000453 RID: 1107
				ArrayAfterComma = 1024
			}

			// Token: 0x02000110 RID: 272
			private class PushBackReader
			{
				// Token: 0x06000A80 RID: 2688 RVA: 0x00021445 File Offset: 0x0001F645
				internal PushBackReader(TextReader reader)
				{
					this.reader = reader;
				}

				// Token: 0x06000A81 RID: 2689 RVA: 0x00021454 File Offset: 0x0001F654
				internal char? Read()
				{
					if (this.nextChar != null)
					{
						char? c = this.nextChar;
						this.nextChar = null;
						return c;
					}
					int num = this.reader.Read();
					if (num != -1)
					{
						return new char?((char)num);
					}
					return null;
				}

				// Token: 0x06000A82 RID: 2690 RVA: 0x000214A4 File Offset: 0x0001F6A4
				internal char ReadOrFail(string messageOnFailure)
				{
					char? c = this.Read();
					if (c == null)
					{
						throw this.CreateException(messageOnFailure);
					}
					return c.Value;
				}

				// Token: 0x06000A83 RID: 2691 RVA: 0x000214D0 File Offset: 0x0001F6D0
				internal void PushBack(char c)
				{
					if (this.nextChar != null)
					{
						throw new InvalidOperationException("Cannot push back when already buffering a character");
					}
					this.nextChar = new char?(c);
				}

				// Token: 0x06000A84 RID: 2692 RVA: 0x000214F6 File Offset: 0x0001F6F6
				internal InvalidJsonException CreateException(string message)
				{
					return new InvalidJsonException(message);
				}

				// Token: 0x04000454 RID: 1108
				private readonly TextReader reader;

				// Token: 0x04000455 RID: 1109
				private char? nextChar;
			}
		}
	}
}
