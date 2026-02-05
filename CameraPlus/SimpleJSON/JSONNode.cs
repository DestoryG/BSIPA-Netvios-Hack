using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace CameraPlus.SimpleJSON
{
	// Token: 0x02000015 RID: 21
	public abstract class JSONNode
	{
		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000095 RID: 149
		public abstract JSONNodeType Tag { get; }

		// Token: 0x17000014 RID: 20
		public virtual JSONNode this[int aIndex]
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x17000015 RID: 21
		public virtual JSONNode this[string aKey]
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600009A RID: 154 RVA: 0x00007FA0 File Offset: 0x000061A0
		// (set) Token: 0x0600009B RID: 155 RVA: 0x00003561 File Offset: 0x00001761
		public virtual string Value
		{
			get
			{
				return "";
			}
			set
			{
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600009C RID: 156 RVA: 0x00007FA7 File Offset: 0x000061A7
		public virtual int Count
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600009D RID: 157 RVA: 0x00007FA7 File Offset: 0x000061A7
		public virtual bool IsNumber
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600009E RID: 158 RVA: 0x00007FA7 File Offset: 0x000061A7
		public virtual bool IsString
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600009F RID: 159 RVA: 0x00007FA7 File Offset: 0x000061A7
		public virtual bool IsBoolean
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060000A0 RID: 160 RVA: 0x00007FA7 File Offset: 0x000061A7
		public virtual bool IsNull
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060000A1 RID: 161 RVA: 0x00007FA7 File Offset: 0x000061A7
		public virtual bool IsArray
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060000A2 RID: 162 RVA: 0x00007FA7 File Offset: 0x000061A7
		public virtual bool IsObject
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060000A3 RID: 163 RVA: 0x00007FA7 File Offset: 0x000061A7
		// (set) Token: 0x060000A4 RID: 164 RVA: 0x00003561 File Offset: 0x00001761
		public virtual bool Inline
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00003561 File Offset: 0x00001761
		public virtual void Add(string aKey, JSONNode aItem)
		{
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00007FAA File Offset: 0x000061AA
		public virtual void Add(JSONNode aItem)
		{
			this.Add("", aItem);
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00007F9D File Offset: 0x0000619D
		public virtual JSONNode Remove(string aKey)
		{
			return null;
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00007F9D File Offset: 0x0000619D
		public virtual JSONNode Remove(int aIndex)
		{
			return null;
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00007FB8 File Offset: 0x000061B8
		public virtual JSONNode Remove(JSONNode aNode)
		{
			return aNode;
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060000AA RID: 170 RVA: 0x00007FBB File Offset: 0x000061BB
		public virtual IEnumerable<JSONNode> Children
		{
			get
			{
				yield break;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060000AB RID: 171 RVA: 0x00007FC4 File Offset: 0x000061C4
		public IEnumerable<JSONNode> DeepChildren
		{
			get
			{
				foreach (JSONNode jsonnode in this.Children)
				{
					foreach (JSONNode jsonnode2 in jsonnode.DeepChildren)
					{
						yield return jsonnode2;
					}
					IEnumerator<JSONNode> enumerator2 = null;
				}
				IEnumerator<JSONNode> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00007FD4 File Offset: 0x000061D4
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			this.WriteToStringBuilder(stringBuilder, 0, 0, JSONTextMode.Compact);
			return stringBuilder.ToString();
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00007FF8 File Offset: 0x000061F8
		public virtual string ToString(int aIndent)
		{
			StringBuilder stringBuilder = new StringBuilder();
			this.WriteToStringBuilder(stringBuilder, 0, aIndent, JSONTextMode.Indent);
			return stringBuilder.ToString();
		}

		// Token: 0x060000AE RID: 174
		internal abstract void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode);

		// Token: 0x060000AF RID: 175
		public abstract JSONNode.Enumerator GetEnumerator();

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060000B0 RID: 176 RVA: 0x0000801B File Offset: 0x0000621B
		public IEnumerable<KeyValuePair<string, JSONNode>> Linq
		{
			get
			{
				return new JSONNode.LinqEnumerator(this);
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060000B1 RID: 177 RVA: 0x00008023 File Offset: 0x00006223
		public JSONNode.KeyEnumerator Keys
		{
			get
			{
				return new JSONNode.KeyEnumerator(this.GetEnumerator());
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000B2 RID: 178 RVA: 0x00008030 File Offset: 0x00006230
		public JSONNode.ValueEnumerator Values
		{
			get
			{
				return new JSONNode.ValueEnumerator(this.GetEnumerator());
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000B3 RID: 179 RVA: 0x00008040 File Offset: 0x00006240
		// (set) Token: 0x060000B4 RID: 180 RVA: 0x00008071 File Offset: 0x00006271
		public virtual double AsDouble
		{
			get
			{
				double num = 0.0;
				if (double.TryParse(this.Value, out num))
				{
					return num;
				}
				return 0.0;
			}
			set
			{
				this.Value = value.ToString();
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000B5 RID: 181 RVA: 0x00008080 File Offset: 0x00006280
		// (set) Token: 0x060000B6 RID: 182 RVA: 0x00008089 File Offset: 0x00006289
		public virtual int AsInt
		{
			get
			{
				return (int)this.AsDouble;
			}
			set
			{
				this.AsDouble = (double)value;
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000B7 RID: 183 RVA: 0x00008093 File Offset: 0x00006293
		// (set) Token: 0x060000B8 RID: 184 RVA: 0x00008089 File Offset: 0x00006289
		public virtual float AsFloat
		{
			get
			{
				return (float)this.AsDouble;
			}
			set
			{
				this.AsDouble = (double)value;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000B9 RID: 185 RVA: 0x0000809C File Offset: 0x0000629C
		// (set) Token: 0x060000BA RID: 186 RVA: 0x000080CA File Offset: 0x000062CA
		public virtual bool AsBool
		{
			get
			{
				bool flag = false;
				if (bool.TryParse(this.Value, out flag))
				{
					return flag;
				}
				return !string.IsNullOrEmpty(this.Value);
			}
			set
			{
				this.Value = (value ? "true" : "false");
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000BB RID: 187 RVA: 0x000080E1 File Offset: 0x000062E1
		public virtual JSONArray AsArray
		{
			get
			{
				return this as JSONArray;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000BC RID: 188 RVA: 0x000080E9 File Offset: 0x000062E9
		public virtual JSONObject AsObject
		{
			get
			{
				return this as JSONObject;
			}
		}

		// Token: 0x060000BD RID: 189 RVA: 0x000080F1 File Offset: 0x000062F1
		public static implicit operator JSONNode(string s)
		{
			return new JSONString(s);
		}

		// Token: 0x060000BE RID: 190 RVA: 0x000080F9 File Offset: 0x000062F9
		public static implicit operator string(JSONNode d)
		{
			if (!(d == null))
			{
				return d.Value;
			}
			return null;
		}

		// Token: 0x060000BF RID: 191 RVA: 0x0000810C File Offset: 0x0000630C
		public static implicit operator JSONNode(double n)
		{
			return new JSONNumber(n);
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00008114 File Offset: 0x00006314
		public static implicit operator double(JSONNode d)
		{
			if (!(d == null))
			{
				return d.AsDouble;
			}
			return 0.0;
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x0000812F File Offset: 0x0000632F
		public static implicit operator JSONNode(float n)
		{
			return new JSONNumber((double)n);
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00008138 File Offset: 0x00006338
		public static implicit operator float(JSONNode d)
		{
			if (!(d == null))
			{
				return d.AsFloat;
			}
			return 0f;
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x0000812F File Offset: 0x0000632F
		public static implicit operator JSONNode(int n)
		{
			return new JSONNumber((double)n);
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x0000814F File Offset: 0x0000634F
		public static implicit operator int(JSONNode d)
		{
			if (!(d == null))
			{
				return d.AsInt;
			}
			return 0;
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00008162 File Offset: 0x00006362
		public static implicit operator JSONNode(bool b)
		{
			return new JSONBool(b);
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x0000816A File Offset: 0x0000636A
		public static implicit operator bool(JSONNode d)
		{
			return !(d == null) && d.AsBool;
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x0000817D File Offset: 0x0000637D
		public static implicit operator JSONNode(KeyValuePair<string, JSONNode> aKeyValue)
		{
			return aKeyValue.Value;
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00008188 File Offset: 0x00006388
		public static bool operator ==(JSONNode a, object b)
		{
			if (a == b)
			{
				return true;
			}
			bool flag = a is JSONNull || a == null || a is JSONLazyCreator;
			bool flag2 = b is JSONNull || b == null || b is JSONLazyCreator;
			return (flag && flag2) || (!flag && a.Equals(b));
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x000081DE File Offset: 0x000063DE
		public static bool operator !=(JSONNode a, object b)
		{
			return !(a == b);
		}

		// Token: 0x060000CA RID: 202 RVA: 0x000081EA File Offset: 0x000063EA
		public override bool Equals(object obj)
		{
			return this == obj;
		}

		// Token: 0x060000CB RID: 203 RVA: 0x000081F0 File Offset: 0x000063F0
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000CC RID: 204 RVA: 0x000081F8 File Offset: 0x000063F8
		internal static StringBuilder EscapeBuilder
		{
			get
			{
				if (JSONNode.m_EscapeBuilder == null)
				{
					JSONNode.m_EscapeBuilder = new StringBuilder();
				}
				return JSONNode.m_EscapeBuilder;
			}
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00008210 File Offset: 0x00006410
		internal static string Escape(string aText)
		{
			StringBuilder escapeBuilder = JSONNode.EscapeBuilder;
			escapeBuilder.Length = 0;
			if (escapeBuilder.Capacity < aText.Length + aText.Length / 10)
			{
				escapeBuilder.Capacity = aText.Length + aText.Length / 10;
			}
			int i = 0;
			while (i < aText.Length)
			{
				char c = aText[i];
				switch (c)
				{
				case '\b':
					escapeBuilder.Append("\\b");
					break;
				case '\t':
					escapeBuilder.Append("\\t");
					break;
				case '\n':
					escapeBuilder.Append("\\n");
					break;
				case '\v':
					goto IL_00E2;
				case '\f':
					escapeBuilder.Append("\\f");
					break;
				case '\r':
					escapeBuilder.Append("\\r");
					break;
				default:
					if (c != '"')
					{
						if (c != '\\')
						{
							goto IL_00E2;
						}
						escapeBuilder.Append("\\\\");
					}
					else
					{
						escapeBuilder.Append("\\\"");
					}
					break;
				}
				IL_0121:
				i++;
				continue;
				IL_00E2:
				if (c < ' ' || (JSONNode.forceASCII && c > '\u007f'))
				{
					ushort num = (ushort)c;
					escapeBuilder.Append("\\u").Append(num.ToString("X4"));
					goto IL_0121;
				}
				escapeBuilder.Append(c);
				goto IL_0121;
			}
			string text = escapeBuilder.ToString();
			escapeBuilder.Length = 0;
			return text;
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00008360 File Offset: 0x00006560
		private static void ParseElement(JSONNode ctx, string token, string tokenName, bool quoted)
		{
			if (quoted)
			{
				ctx.Add(tokenName, token);
				return;
			}
			string text = token.ToLower();
			if (text == "false" || text == "true")
			{
				ctx.Add(tokenName, text == "true");
				return;
			}
			if (text == "null")
			{
				ctx.Add(tokenName, null);
				return;
			}
			double num;
			if (double.TryParse(token, out num))
			{
				ctx.Add(tokenName, num);
				return;
			}
			ctx.Add(tokenName, token);
		}

		// Token: 0x060000CF RID: 207 RVA: 0x000083F4 File Offset: 0x000065F4
		public static JSONNode Parse(string aJSON)
		{
			Stack<JSONNode> stack = new Stack<JSONNode>();
			JSONNode jsonnode = null;
			int i = 0;
			StringBuilder stringBuilder = new StringBuilder();
			string text = "";
			bool flag = false;
			bool flag2 = false;
			while (i < aJSON.Length)
			{
				char c = aJSON[i];
				if (c <= ',')
				{
					if (c <= ' ')
					{
						switch (c)
						{
						case '\t':
							break;
						case '\n':
						case '\r':
							goto IL_033E;
						case '\v':
						case '\f':
							goto IL_0330;
						default:
							if (c != ' ')
							{
								goto IL_0330;
							}
							break;
						}
						if (flag)
						{
							stringBuilder.Append(aJSON[i]);
						}
					}
					else if (c != '"')
					{
						if (c != ',')
						{
							goto IL_0330;
						}
						if (flag)
						{
							stringBuilder.Append(aJSON[i]);
						}
						else
						{
							if (stringBuilder.Length > 0 || flag2)
							{
								JSONNode.ParseElement(jsonnode, stringBuilder.ToString(), text, flag2);
							}
							text = "";
							stringBuilder.Length = 0;
							flag2 = false;
						}
					}
					else
					{
						flag = !flag;
						flag2 = flag2 || flag;
					}
				}
				else
				{
					if (c <= ']')
					{
						if (c != ':')
						{
							switch (c)
							{
							case '[':
								if (flag)
								{
									stringBuilder.Append(aJSON[i]);
									goto IL_033E;
								}
								stack.Push(new JSONArray());
								if (jsonnode != null)
								{
									jsonnode.Add(text, stack.Peek());
								}
								text = "";
								stringBuilder.Length = 0;
								jsonnode = stack.Peek();
								goto IL_033E;
							case '\\':
								i++;
								if (flag)
								{
									char c2 = aJSON[i];
									if (c2 <= 'f')
									{
										if (c2 == 'b')
										{
											stringBuilder.Append('\b');
											goto IL_033E;
										}
										if (c2 == 'f')
										{
											stringBuilder.Append('\f');
											goto IL_033E;
										}
									}
									else
									{
										if (c2 == 'n')
										{
											stringBuilder.Append('\n');
											goto IL_033E;
										}
										switch (c2)
										{
										case 'r':
											stringBuilder.Append('\r');
											goto IL_033E;
										case 't':
											stringBuilder.Append('\t');
											goto IL_033E;
										case 'u':
										{
											string text2 = aJSON.Substring(i + 1, 4);
											stringBuilder.Append((char)int.Parse(text2, NumberStyles.AllowHexSpecifier));
											i += 4;
											goto IL_033E;
										}
										}
									}
									stringBuilder.Append(c2);
									goto IL_033E;
								}
								goto IL_033E;
							case ']':
								break;
							default:
								goto IL_0330;
							}
						}
						else
						{
							if (flag)
							{
								stringBuilder.Append(aJSON[i]);
								goto IL_033E;
							}
							text = stringBuilder.ToString();
							stringBuilder.Length = 0;
							flag2 = false;
							goto IL_033E;
						}
					}
					else if (c != '{')
					{
						if (c != '}')
						{
							goto IL_0330;
						}
					}
					else
					{
						if (flag)
						{
							stringBuilder.Append(aJSON[i]);
							goto IL_033E;
						}
						stack.Push(new JSONObject());
						if (jsonnode != null)
						{
							jsonnode.Add(text, stack.Peek());
						}
						text = "";
						stringBuilder.Length = 0;
						jsonnode = stack.Peek();
						goto IL_033E;
					}
					if (flag)
					{
						stringBuilder.Append(aJSON[i]);
					}
					else
					{
						if (stack.Count == 0)
						{
							throw new Exception("JSON Parse: Too many closing brackets");
						}
						stack.Pop();
						if (stringBuilder.Length > 0 || flag2)
						{
							JSONNode.ParseElement(jsonnode, stringBuilder.ToString(), text, flag2);
							flag2 = false;
						}
						text = "";
						stringBuilder.Length = 0;
						if (stack.Count > 0)
						{
							jsonnode = stack.Peek();
						}
					}
				}
				IL_033E:
				i++;
				continue;
				IL_0330:
				stringBuilder.Append(aJSON[i]);
				goto IL_033E;
			}
			if (flag)
			{
				throw new Exception("JSON Parse: Quotation marks seems to be messed up.");
			}
			return jsonnode;
		}

		// Token: 0x040000A9 RID: 169
		public static bool forceASCII;

		// Token: 0x040000AA RID: 170
		[ThreadStatic]
		private static StringBuilder m_EscapeBuilder;

		// Token: 0x0200002D RID: 45
		public struct Enumerator
		{
			// Token: 0x1700005B RID: 91
			// (get) Token: 0x0600016E RID: 366 RVA: 0x00009E5B File Offset: 0x0000805B
			public bool IsValid
			{
				get
				{
					return this.type > JSONNode.Enumerator.Type.None;
				}
			}

			// Token: 0x0600016F RID: 367 RVA: 0x00009E66 File Offset: 0x00008066
			public Enumerator(List<JSONNode>.Enumerator aArrayEnum)
			{
				this.type = JSONNode.Enumerator.Type.Array;
				this.m_Object = default(Dictionary<string, JSONNode>.Enumerator);
				this.m_Array = aArrayEnum;
			}

			// Token: 0x06000170 RID: 368 RVA: 0x00009E82 File Offset: 0x00008082
			public Enumerator(Dictionary<string, JSONNode>.Enumerator aDictEnum)
			{
				this.type = JSONNode.Enumerator.Type.Object;
				this.m_Object = aDictEnum;
				this.m_Array = default(List<JSONNode>.Enumerator);
			}

			// Token: 0x1700005C RID: 92
			// (get) Token: 0x06000171 RID: 369 RVA: 0x00009EA0 File Offset: 0x000080A0
			public KeyValuePair<string, JSONNode> Current
			{
				get
				{
					if (this.type == JSONNode.Enumerator.Type.Array)
					{
						return new KeyValuePair<string, JSONNode>(string.Empty, this.m_Array.Current);
					}
					if (this.type == JSONNode.Enumerator.Type.Object)
					{
						return this.m_Object.Current;
					}
					return new KeyValuePair<string, JSONNode>(string.Empty, null);
				}
			}

			// Token: 0x06000172 RID: 370 RVA: 0x00009EEC File Offset: 0x000080EC
			public bool MoveNext()
			{
				if (this.type == JSONNode.Enumerator.Type.Array)
				{
					return this.m_Array.MoveNext();
				}
				return this.type == JSONNode.Enumerator.Type.Object && this.m_Object.MoveNext();
			}

			// Token: 0x040000E4 RID: 228
			private JSONNode.Enumerator.Type type;

			// Token: 0x040000E5 RID: 229
			private Dictionary<string, JSONNode>.Enumerator m_Object;

			// Token: 0x040000E6 RID: 230
			private List<JSONNode>.Enumerator m_Array;

			// Token: 0x02000036 RID: 54
			private enum Type
			{
				// Token: 0x04000100 RID: 256
				None,
				// Token: 0x04000101 RID: 257
				Array,
				// Token: 0x04000102 RID: 258
				Object
			}
		}

		// Token: 0x0200002E RID: 46
		public struct ValueEnumerator
		{
			// Token: 0x06000173 RID: 371 RVA: 0x00009F19 File Offset: 0x00008119
			public ValueEnumerator(List<JSONNode>.Enumerator aArrayEnum)
			{
				this = new JSONNode.ValueEnumerator(new JSONNode.Enumerator(aArrayEnum));
			}

			// Token: 0x06000174 RID: 372 RVA: 0x00009F27 File Offset: 0x00008127
			public ValueEnumerator(Dictionary<string, JSONNode>.Enumerator aDictEnum)
			{
				this = new JSONNode.ValueEnumerator(new JSONNode.Enumerator(aDictEnum));
			}

			// Token: 0x06000175 RID: 373 RVA: 0x00009F35 File Offset: 0x00008135
			public ValueEnumerator(JSONNode.Enumerator aEnumerator)
			{
				this.m_Enumerator = aEnumerator;
			}

			// Token: 0x1700005D RID: 93
			// (get) Token: 0x06000176 RID: 374 RVA: 0x00009F40 File Offset: 0x00008140
			public JSONNode Current
			{
				get
				{
					KeyValuePair<string, JSONNode> keyValuePair = this.m_Enumerator.Current;
					return keyValuePair.Value;
				}
			}

			// Token: 0x06000177 RID: 375 RVA: 0x00009F60 File Offset: 0x00008160
			public bool MoveNext()
			{
				return this.m_Enumerator.MoveNext();
			}

			// Token: 0x06000178 RID: 376 RVA: 0x00009F6D File Offset: 0x0000816D
			public JSONNode.ValueEnumerator GetEnumerator()
			{
				return this;
			}

			// Token: 0x040000E7 RID: 231
			private JSONNode.Enumerator m_Enumerator;
		}

		// Token: 0x0200002F RID: 47
		public struct KeyEnumerator
		{
			// Token: 0x06000179 RID: 377 RVA: 0x00009F75 File Offset: 0x00008175
			public KeyEnumerator(List<JSONNode>.Enumerator aArrayEnum)
			{
				this = new JSONNode.KeyEnumerator(new JSONNode.Enumerator(aArrayEnum));
			}

			// Token: 0x0600017A RID: 378 RVA: 0x00009F83 File Offset: 0x00008183
			public KeyEnumerator(Dictionary<string, JSONNode>.Enumerator aDictEnum)
			{
				this = new JSONNode.KeyEnumerator(new JSONNode.Enumerator(aDictEnum));
			}

			// Token: 0x0600017B RID: 379 RVA: 0x00009F91 File Offset: 0x00008191
			public KeyEnumerator(JSONNode.Enumerator aEnumerator)
			{
				this.m_Enumerator = aEnumerator;
			}

			// Token: 0x1700005E RID: 94
			// (get) Token: 0x0600017C RID: 380 RVA: 0x00009F9C File Offset: 0x0000819C
			public JSONNode Current
			{
				get
				{
					KeyValuePair<string, JSONNode> keyValuePair = this.m_Enumerator.Current;
					return keyValuePair.Key;
				}
			}

			// Token: 0x0600017D RID: 381 RVA: 0x00009FC1 File Offset: 0x000081C1
			public bool MoveNext()
			{
				return this.m_Enumerator.MoveNext();
			}

			// Token: 0x0600017E RID: 382 RVA: 0x00009FCE File Offset: 0x000081CE
			public JSONNode.KeyEnumerator GetEnumerator()
			{
				return this;
			}

			// Token: 0x040000E8 RID: 232
			private JSONNode.Enumerator m_Enumerator;
		}

		// Token: 0x02000030 RID: 48
		public class LinqEnumerator : IEnumerator<KeyValuePair<string, JSONNode>>, IDisposable, IEnumerator, IEnumerable<KeyValuePair<string, JSONNode>>, IEnumerable
		{
			// Token: 0x0600017F RID: 383 RVA: 0x00009FD6 File Offset: 0x000081D6
			internal LinqEnumerator(JSONNode aNode)
			{
				this.m_Node = aNode;
				if (this.m_Node != null)
				{
					this.m_Enumerator = this.m_Node.GetEnumerator();
				}
			}

			// Token: 0x1700005F RID: 95
			// (get) Token: 0x06000180 RID: 384 RVA: 0x0000A004 File Offset: 0x00008204
			public KeyValuePair<string, JSONNode> Current
			{
				get
				{
					return this.m_Enumerator.Current;
				}
			}

			// Token: 0x17000060 RID: 96
			// (get) Token: 0x06000181 RID: 385 RVA: 0x0000A011 File Offset: 0x00008211
			object IEnumerator.Current
			{
				get
				{
					return this.m_Enumerator.Current;
				}
			}

			// Token: 0x06000182 RID: 386 RVA: 0x0000A023 File Offset: 0x00008223
			public bool MoveNext()
			{
				return this.m_Enumerator.MoveNext();
			}

			// Token: 0x06000183 RID: 387 RVA: 0x0000A030 File Offset: 0x00008230
			public void Dispose()
			{
				this.m_Node = null;
				this.m_Enumerator = default(JSONNode.Enumerator);
			}

			// Token: 0x06000184 RID: 388 RVA: 0x0000A045 File Offset: 0x00008245
			public IEnumerator<KeyValuePair<string, JSONNode>> GetEnumerator()
			{
				return new JSONNode.LinqEnumerator(this.m_Node);
			}

			// Token: 0x06000185 RID: 389 RVA: 0x0000A052 File Offset: 0x00008252
			public void Reset()
			{
				if (this.m_Node != null)
				{
					this.m_Enumerator = this.m_Node.GetEnumerator();
				}
			}

			// Token: 0x06000186 RID: 390 RVA: 0x0000A045 File Offset: 0x00008245
			IEnumerator IEnumerable.GetEnumerator()
			{
				return new JSONNode.LinqEnumerator(this.m_Node);
			}

			// Token: 0x040000E9 RID: 233
			private JSONNode m_Node;

			// Token: 0x040000EA RID: 234
			private JSONNode.Enumerator m_Enumerator;
		}
	}
}
