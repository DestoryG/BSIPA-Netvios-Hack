using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace SimpleJSON
{
	// Token: 0x02000004 RID: 4
	public abstract class JSONNode
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1
		public abstract JSONNodeType Tag { get; }

		// Token: 0x17000002 RID: 2
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

		// Token: 0x17000003 RID: 3
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

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000006 RID: 6 RVA: 0x00002055 File Offset: 0x00000255
		// (set) Token: 0x06000007 RID: 7 RVA: 0x00002053 File Offset: 0x00000253
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

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000008 RID: 8 RVA: 0x0000205C File Offset: 0x0000025C
		public virtual int Count
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000009 RID: 9 RVA: 0x0000205C File Offset: 0x0000025C
		public virtual bool IsNumber
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600000A RID: 10 RVA: 0x0000205C File Offset: 0x0000025C
		public virtual bool IsString
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600000B RID: 11 RVA: 0x0000205C File Offset: 0x0000025C
		public virtual bool IsBoolean
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600000C RID: 12 RVA: 0x0000205C File Offset: 0x0000025C
		public virtual bool IsNull
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600000D RID: 13 RVA: 0x0000205C File Offset: 0x0000025C
		public virtual bool IsArray
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600000E RID: 14 RVA: 0x0000205C File Offset: 0x0000025C
		public virtual bool IsObject
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600000F RID: 15 RVA: 0x0000205C File Offset: 0x0000025C
		// (set) Token: 0x06000010 RID: 16 RVA: 0x00002053 File Offset: 0x00000253
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

		// Token: 0x06000011 RID: 17 RVA: 0x00002053 File Offset: 0x00000253
		public virtual void Add(string aKey, JSONNode aItem)
		{
		}

		// Token: 0x06000012 RID: 18 RVA: 0x0000205F File Offset: 0x0000025F
		public virtual void Add(JSONNode aItem)
		{
			this.Add("", aItem);
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002050 File Offset: 0x00000250
		public virtual JSONNode Remove(string aKey)
		{
			return null;
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002050 File Offset: 0x00000250
		public virtual JSONNode Remove(int aIndex)
		{
			return null;
		}

		// Token: 0x06000015 RID: 21 RVA: 0x0000206D File Offset: 0x0000026D
		public virtual JSONNode Remove(JSONNode aNode)
		{
			return aNode;
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000016 RID: 22 RVA: 0x00002070 File Offset: 0x00000270
		public virtual IEnumerable<JSONNode> Children
		{
			get
			{
				yield break;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000017 RID: 23 RVA: 0x00002079 File Offset: 0x00000279
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

		// Token: 0x06000018 RID: 24 RVA: 0x0000208C File Offset: 0x0000028C
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			this.WriteToStringBuilder(stringBuilder, 0, 0, JSONTextMode.Compact);
			return stringBuilder.ToString();
		}

		// Token: 0x06000019 RID: 25 RVA: 0x000020B0 File Offset: 0x000002B0
		public virtual string ToString(int aIndent)
		{
			StringBuilder stringBuilder = new StringBuilder();
			this.WriteToStringBuilder(stringBuilder, 0, aIndent, JSONTextMode.Indent);
			return stringBuilder.ToString();
		}

		// Token: 0x0600001A RID: 26
		internal abstract void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode);

		// Token: 0x0600001B RID: 27
		public abstract JSONNode.Enumerator GetEnumerator();

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600001C RID: 28 RVA: 0x000020D3 File Offset: 0x000002D3
		public IEnumerable<KeyValuePair<string, JSONNode>> Linq
		{
			get
			{
				return new JSONNode.LinqEnumerator(this);
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600001D RID: 29 RVA: 0x000020DB File Offset: 0x000002DB
		public JSONNode.KeyEnumerator Keys
		{
			get
			{
				return new JSONNode.KeyEnumerator(this.GetEnumerator());
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600001E RID: 30 RVA: 0x000020E8 File Offset: 0x000002E8
		public JSONNode.ValueEnumerator Values
		{
			get
			{
				return new JSONNode.ValueEnumerator(this.GetEnumerator());
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600001F RID: 31 RVA: 0x000020F8 File Offset: 0x000002F8
		// (set) Token: 0x06000020 RID: 32 RVA: 0x00002129 File Offset: 0x00000329
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

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000021 RID: 33 RVA: 0x00002138 File Offset: 0x00000338
		// (set) Token: 0x06000022 RID: 34 RVA: 0x00002141 File Offset: 0x00000341
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

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000023 RID: 35 RVA: 0x0000214B File Offset: 0x0000034B
		// (set) Token: 0x06000024 RID: 36 RVA: 0x00002141 File Offset: 0x00000341
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

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000025 RID: 37 RVA: 0x00002154 File Offset: 0x00000354
		// (set) Token: 0x06000026 RID: 38 RVA: 0x00002182 File Offset: 0x00000382
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

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000027 RID: 39 RVA: 0x00002199 File Offset: 0x00000399
		public virtual JSONArray AsArray
		{
			get
			{
				return this as JSONArray;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000028 RID: 40 RVA: 0x000021A1 File Offset: 0x000003A1
		public virtual JSONObject AsObject
		{
			get
			{
				return this as JSONObject;
			}
		}

		// Token: 0x06000029 RID: 41 RVA: 0x000021A9 File Offset: 0x000003A9
		public static implicit operator JSONNode(string s)
		{
			return new JSONString(s);
		}

		// Token: 0x0600002A RID: 42 RVA: 0x000021B1 File Offset: 0x000003B1
		public static implicit operator string(JSONNode d)
		{
			if (!(d == null))
			{
				return d.Value;
			}
			return null;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x000021C4 File Offset: 0x000003C4
		public static implicit operator JSONNode(double n)
		{
			return new JSONNumber(n);
		}

		// Token: 0x0600002C RID: 44 RVA: 0x000021CC File Offset: 0x000003CC
		public static implicit operator double(JSONNode d)
		{
			if (!(d == null))
			{
				return d.AsDouble;
			}
			return 0.0;
		}

		// Token: 0x0600002D RID: 45 RVA: 0x000021E7 File Offset: 0x000003E7
		public static implicit operator JSONNode(float n)
		{
			return new JSONNumber((double)n);
		}

		// Token: 0x0600002E RID: 46 RVA: 0x000021F0 File Offset: 0x000003F0
		public static implicit operator float(JSONNode d)
		{
			if (!(d == null))
			{
				return d.AsFloat;
			}
			return 0f;
		}

		// Token: 0x0600002F RID: 47 RVA: 0x000021E7 File Offset: 0x000003E7
		public static implicit operator JSONNode(int n)
		{
			return new JSONNumber((double)n);
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002207 File Offset: 0x00000407
		public static implicit operator int(JSONNode d)
		{
			if (!(d == null))
			{
				return d.AsInt;
			}
			return 0;
		}

		// Token: 0x06000031 RID: 49 RVA: 0x0000221A File Offset: 0x0000041A
		public static implicit operator JSONNode(bool b)
		{
			return new JSONBool(b);
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002222 File Offset: 0x00000422
		public static implicit operator bool(JSONNode d)
		{
			return !(d == null) && d.AsBool;
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00002235 File Offset: 0x00000435
		public static implicit operator JSONNode(KeyValuePair<string, JSONNode> aKeyValue)
		{
			return aKeyValue.Value;
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002240 File Offset: 0x00000440
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

		// Token: 0x06000035 RID: 53 RVA: 0x00002296 File Offset: 0x00000496
		public static bool operator !=(JSONNode a, object b)
		{
			return !(a == b);
		}

		// Token: 0x06000036 RID: 54 RVA: 0x000022A2 File Offset: 0x000004A2
		public override bool Equals(object obj)
		{
			return this == obj;
		}

		// Token: 0x06000037 RID: 55 RVA: 0x000022A8 File Offset: 0x000004A8
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000038 RID: 56 RVA: 0x000022B0 File Offset: 0x000004B0
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

		// Token: 0x06000039 RID: 57 RVA: 0x000022C8 File Offset: 0x000004C8
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

		// Token: 0x0600003A RID: 58 RVA: 0x00002418 File Offset: 0x00000618
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

		// Token: 0x0600003B RID: 59 RVA: 0x000024AC File Offset: 0x000006AC
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

		// Token: 0x0400000D RID: 13
		public static bool forceASCII;

		// Token: 0x0400000E RID: 14
		[ThreadStatic]
		private static StringBuilder m_EscapeBuilder;

		// Token: 0x02000025 RID: 37
		public struct Enumerator
		{
			// Token: 0x1700004D RID: 77
			// (get) Token: 0x0600016C RID: 364 RVA: 0x00006778 File Offset: 0x00004978
			public bool IsValid
			{
				get
				{
					return this.type > JSONNode.Enumerator.Type.None;
				}
			}

			// Token: 0x0600016D RID: 365 RVA: 0x00006783 File Offset: 0x00004983
			public Enumerator(List<JSONNode>.Enumerator aArrayEnum)
			{
				this.type = JSONNode.Enumerator.Type.Array;
				this.m_Object = default(Dictionary<string, JSONNode>.Enumerator);
				this.m_Array = aArrayEnum;
			}

			// Token: 0x0600016E RID: 366 RVA: 0x0000679F File Offset: 0x0000499F
			public Enumerator(Dictionary<string, JSONNode>.Enumerator aDictEnum)
			{
				this.type = JSONNode.Enumerator.Type.Object;
				this.m_Object = aDictEnum;
				this.m_Array = default(List<JSONNode>.Enumerator);
			}

			// Token: 0x1700004E RID: 78
			// (get) Token: 0x0600016F RID: 367 RVA: 0x000067BC File Offset: 0x000049BC
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

			// Token: 0x06000170 RID: 368 RVA: 0x00006808 File Offset: 0x00004A08
			public bool MoveNext()
			{
				if (this.type == JSONNode.Enumerator.Type.Array)
				{
					return this.m_Array.MoveNext();
				}
				return this.type == JSONNode.Enumerator.Type.Object && this.m_Object.MoveNext();
			}

			// Token: 0x0400009D RID: 157
			private JSONNode.Enumerator.Type type;

			// Token: 0x0400009E RID: 158
			private Dictionary<string, JSONNode>.Enumerator m_Object;

			// Token: 0x0400009F RID: 159
			private List<JSONNode>.Enumerator m_Array;

			// Token: 0x02000052 RID: 82
			private enum Type
			{
				// Token: 0x04000170 RID: 368
				None,
				// Token: 0x04000171 RID: 369
				Array,
				// Token: 0x04000172 RID: 370
				Object
			}
		}

		// Token: 0x02000026 RID: 38
		public struct ValueEnumerator
		{
			// Token: 0x06000171 RID: 369 RVA: 0x00006835 File Offset: 0x00004A35
			public ValueEnumerator(List<JSONNode>.Enumerator aArrayEnum)
			{
				this = new JSONNode.ValueEnumerator(new JSONNode.Enumerator(aArrayEnum));
			}

			// Token: 0x06000172 RID: 370 RVA: 0x00006843 File Offset: 0x00004A43
			public ValueEnumerator(Dictionary<string, JSONNode>.Enumerator aDictEnum)
			{
				this = new JSONNode.ValueEnumerator(new JSONNode.Enumerator(aDictEnum));
			}

			// Token: 0x06000173 RID: 371 RVA: 0x00006851 File Offset: 0x00004A51
			public ValueEnumerator(JSONNode.Enumerator aEnumerator)
			{
				this.m_Enumerator = aEnumerator;
			}

			// Token: 0x1700004F RID: 79
			// (get) Token: 0x06000174 RID: 372 RVA: 0x0000685C File Offset: 0x00004A5C
			public JSONNode Current
			{
				get
				{
					KeyValuePair<string, JSONNode> keyValuePair = this.m_Enumerator.Current;
					return keyValuePair.Value;
				}
			}

			// Token: 0x06000175 RID: 373 RVA: 0x0000687C File Offset: 0x00004A7C
			public bool MoveNext()
			{
				return this.m_Enumerator.MoveNext();
			}

			// Token: 0x06000176 RID: 374 RVA: 0x00006889 File Offset: 0x00004A89
			public JSONNode.ValueEnumerator GetEnumerator()
			{
				return this;
			}

			// Token: 0x040000A0 RID: 160
			private JSONNode.Enumerator m_Enumerator;
		}

		// Token: 0x02000027 RID: 39
		public struct KeyEnumerator
		{
			// Token: 0x06000177 RID: 375 RVA: 0x00006891 File Offset: 0x00004A91
			public KeyEnumerator(List<JSONNode>.Enumerator aArrayEnum)
			{
				this = new JSONNode.KeyEnumerator(new JSONNode.Enumerator(aArrayEnum));
			}

			// Token: 0x06000178 RID: 376 RVA: 0x0000689F File Offset: 0x00004A9F
			public KeyEnumerator(Dictionary<string, JSONNode>.Enumerator aDictEnum)
			{
				this = new JSONNode.KeyEnumerator(new JSONNode.Enumerator(aDictEnum));
			}

			// Token: 0x06000179 RID: 377 RVA: 0x000068AD File Offset: 0x00004AAD
			public KeyEnumerator(JSONNode.Enumerator aEnumerator)
			{
				this.m_Enumerator = aEnumerator;
			}

			// Token: 0x17000050 RID: 80
			// (get) Token: 0x0600017A RID: 378 RVA: 0x000068B8 File Offset: 0x00004AB8
			public JSONNode Current
			{
				get
				{
					KeyValuePair<string, JSONNode> keyValuePair = this.m_Enumerator.Current;
					return keyValuePair.Key;
				}
			}

			// Token: 0x0600017B RID: 379 RVA: 0x000068DD File Offset: 0x00004ADD
			public bool MoveNext()
			{
				return this.m_Enumerator.MoveNext();
			}

			// Token: 0x0600017C RID: 380 RVA: 0x000068EA File Offset: 0x00004AEA
			public JSONNode.KeyEnumerator GetEnumerator()
			{
				return this;
			}

			// Token: 0x040000A1 RID: 161
			private JSONNode.Enumerator m_Enumerator;
		}

		// Token: 0x02000028 RID: 40
		public class LinqEnumerator : IEnumerator<KeyValuePair<string, JSONNode>>, IDisposable, IEnumerator, IEnumerable<KeyValuePair<string, JSONNode>>, IEnumerable
		{
			// Token: 0x0600017D RID: 381 RVA: 0x000068F2 File Offset: 0x00004AF2
			internal LinqEnumerator(JSONNode aNode)
			{
				this.m_Node = aNode;
				if (this.m_Node != null)
				{
					this.m_Enumerator = this.m_Node.GetEnumerator();
				}
			}

			// Token: 0x17000051 RID: 81
			// (get) Token: 0x0600017E RID: 382 RVA: 0x00006920 File Offset: 0x00004B20
			public KeyValuePair<string, JSONNode> Current
			{
				get
				{
					return this.m_Enumerator.Current;
				}
			}

			// Token: 0x17000052 RID: 82
			// (get) Token: 0x0600017F RID: 383 RVA: 0x0000692D File Offset: 0x00004B2D
			object IEnumerator.Current
			{
				get
				{
					return this.m_Enumerator.Current;
				}
			}

			// Token: 0x06000180 RID: 384 RVA: 0x0000693F File Offset: 0x00004B3F
			public bool MoveNext()
			{
				return this.m_Enumerator.MoveNext();
			}

			// Token: 0x06000181 RID: 385 RVA: 0x0000694C File Offset: 0x00004B4C
			public void Dispose()
			{
				this.m_Node = null;
				this.m_Enumerator = default(JSONNode.Enumerator);
			}

			// Token: 0x06000182 RID: 386 RVA: 0x00006961 File Offset: 0x00004B61
			public IEnumerator<KeyValuePair<string, JSONNode>> GetEnumerator()
			{
				return new JSONNode.LinqEnumerator(this.m_Node);
			}

			// Token: 0x06000183 RID: 387 RVA: 0x0000696E File Offset: 0x00004B6E
			public void Reset()
			{
				if (this.m_Node != null)
				{
					this.m_Enumerator = this.m_Node.GetEnumerator();
				}
			}

			// Token: 0x06000184 RID: 388 RVA: 0x00006961 File Offset: 0x00004B61
			IEnumerator IEnumerable.GetEnumerator()
			{
				return new JSONNode.LinqEnumerator(this.m_Node);
			}

			// Token: 0x040000A2 RID: 162
			private JSONNode m_Node;

			// Token: 0x040000A3 RID: 163
			private JSONNode.Enumerator m_Enumerator;
		}
	}
}
