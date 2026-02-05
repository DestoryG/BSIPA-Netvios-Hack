using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace BeatSaberMultiplayer.Helper
{
	// Token: 0x0200007C RID: 124
	public abstract class JSONNode
	{
		// Token: 0x1700023D RID: 573
		// (get) Token: 0x0600087F RID: 2175
		public abstract JSONNodeType Tag { get; }

		// Token: 0x1700023E RID: 574
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

		// Token: 0x1700023F RID: 575
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

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x06000884 RID: 2180 RVA: 0x000246BA File Offset: 0x000228BA
		// (set) Token: 0x06000885 RID: 2181 RVA: 0x000196A0 File Offset: 0x000178A0
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

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x06000886 RID: 2182 RVA: 0x000246C1 File Offset: 0x000228C1
		public virtual int Count
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x06000887 RID: 2183 RVA: 0x000246C1 File Offset: 0x000228C1
		public virtual bool IsNumber
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x06000888 RID: 2184 RVA: 0x000246C1 File Offset: 0x000228C1
		public virtual bool IsString
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x06000889 RID: 2185 RVA: 0x000246C1 File Offset: 0x000228C1
		public virtual bool IsBoolean
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x0600088A RID: 2186 RVA: 0x000246C1 File Offset: 0x000228C1
		public virtual bool IsNull
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x0600088B RID: 2187 RVA: 0x000246C1 File Offset: 0x000228C1
		public virtual bool IsArray
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000247 RID: 583
		// (get) Token: 0x0600088C RID: 2188 RVA: 0x000246C1 File Offset: 0x000228C1
		public virtual bool IsObject
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000248 RID: 584
		// (get) Token: 0x0600088D RID: 2189 RVA: 0x000246C1 File Offset: 0x000228C1
		// (set) Token: 0x0600088E RID: 2190 RVA: 0x000196A0 File Offset: 0x000178A0
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

		// Token: 0x0600088F RID: 2191 RVA: 0x000196A0 File Offset: 0x000178A0
		public virtual void Add(string aKey, JSONNode aItem)
		{
		}

		// Token: 0x06000890 RID: 2192 RVA: 0x000246C4 File Offset: 0x000228C4
		public virtual void Add(JSONNode aItem)
		{
			this.Add("", aItem);
		}

		// Token: 0x06000891 RID: 2193 RVA: 0x000246B7 File Offset: 0x000228B7
		public virtual JSONNode Remove(string aKey)
		{
			return null;
		}

		// Token: 0x06000892 RID: 2194 RVA: 0x000246B7 File Offset: 0x000228B7
		public virtual JSONNode Remove(int aIndex)
		{
			return null;
		}

		// Token: 0x06000893 RID: 2195 RVA: 0x000246D2 File Offset: 0x000228D2
		public virtual JSONNode Remove(JSONNode aNode)
		{
			return aNode;
		}

		// Token: 0x06000894 RID: 2196 RVA: 0x000246B7 File Offset: 0x000228B7
		public virtual JSONNode Clone()
		{
			return null;
		}

		// Token: 0x17000249 RID: 585
		// (get) Token: 0x06000895 RID: 2197 RVA: 0x000246D5 File Offset: 0x000228D5
		public virtual IEnumerable<JSONNode> Children
		{
			get
			{
				yield break;
			}
		}

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x06000896 RID: 2198 RVA: 0x000246DE File Offset: 0x000228DE
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

		// Token: 0x06000897 RID: 2199 RVA: 0x000246C1 File Offset: 0x000228C1
		public virtual bool HasKey(string aKey)
		{
			return false;
		}

		// Token: 0x06000898 RID: 2200 RVA: 0x000246EE File Offset: 0x000228EE
		public virtual JSONNode GetValueOrDefault(string aKey, JSONNode aDefault)
		{
			return aDefault;
		}

		// Token: 0x06000899 RID: 2201 RVA: 0x000246F4 File Offset: 0x000228F4
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			this.WriteToStringBuilder(stringBuilder, 0, 0, JSONTextMode.Compact);
			return stringBuilder.ToString();
		}

		// Token: 0x0600089A RID: 2202 RVA: 0x00024718 File Offset: 0x00022918
		public virtual string ToString(int aIndent)
		{
			StringBuilder stringBuilder = new StringBuilder();
			this.WriteToStringBuilder(stringBuilder, 0, aIndent, JSONTextMode.Indent);
			return stringBuilder.ToString();
		}

		// Token: 0x0600089B RID: 2203
		internal abstract void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode);

		// Token: 0x0600089C RID: 2204
		public abstract JSONNode.Enumerator GetEnumerator();

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x0600089D RID: 2205 RVA: 0x0002473B File Offset: 0x0002293B
		public IEnumerable<KeyValuePair<string, JSONNode>> Linq
		{
			get
			{
				return new JSONNode.LinqEnumerator(this);
			}
		}

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x0600089E RID: 2206 RVA: 0x00024743 File Offset: 0x00022943
		public JSONNode.KeyEnumerator Keys
		{
			get
			{
				return new JSONNode.KeyEnumerator(this.GetEnumerator());
			}
		}

		// Token: 0x1700024D RID: 589
		// (get) Token: 0x0600089F RID: 2207 RVA: 0x00024750 File Offset: 0x00022950
		public JSONNode.ValueEnumerator Values
		{
			get
			{
				return new JSONNode.ValueEnumerator(this.GetEnumerator());
			}
		}

		// Token: 0x1700024E RID: 590
		// (get) Token: 0x060008A0 RID: 2208 RVA: 0x00024760 File Offset: 0x00022960
		// (set) Token: 0x060008A1 RID: 2209 RVA: 0x0002479B File Offset: 0x0002299B
		public virtual double AsDouble
		{
			get
			{
				double num = 0.0;
				if (double.TryParse(this.Value, NumberStyles.Float, CultureInfo.InvariantCulture, out num))
				{
					return num;
				}
				return 0.0;
			}
			set
			{
				this.Value = value.ToString(CultureInfo.InvariantCulture);
			}
		}

		// Token: 0x1700024F RID: 591
		// (get) Token: 0x060008A2 RID: 2210 RVA: 0x000247AF File Offset: 0x000229AF
		// (set) Token: 0x060008A3 RID: 2211 RVA: 0x000247B8 File Offset: 0x000229B8
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

		// Token: 0x17000250 RID: 592
		// (get) Token: 0x060008A4 RID: 2212 RVA: 0x000247C2 File Offset: 0x000229C2
		// (set) Token: 0x060008A5 RID: 2213 RVA: 0x000247B8 File Offset: 0x000229B8
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

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x060008A6 RID: 2214 RVA: 0x000247CC File Offset: 0x000229CC
		// (set) Token: 0x060008A7 RID: 2215 RVA: 0x000247FA File Offset: 0x000229FA
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

		// Token: 0x17000252 RID: 594
		// (get) Token: 0x060008A8 RID: 2216 RVA: 0x00024814 File Offset: 0x00022A14
		// (set) Token: 0x060008A9 RID: 2217 RVA: 0x00024837 File Offset: 0x00022A37
		public virtual long AsLong
		{
			get
			{
				long num = 0L;
				if (long.TryParse(this.Value, out num))
				{
					return num;
				}
				return 0L;
			}
			set
			{
				this.Value = value.ToString();
			}
		}

		// Token: 0x17000253 RID: 595
		// (get) Token: 0x060008AA RID: 2218 RVA: 0x00024846 File Offset: 0x00022A46
		public virtual JSONArray AsArray
		{
			get
			{
				return this as JSONArray;
			}
		}

		// Token: 0x17000254 RID: 596
		// (get) Token: 0x060008AB RID: 2219 RVA: 0x0002484E File Offset: 0x00022A4E
		public virtual JSONObject AsObject
		{
			get
			{
				return this as JSONObject;
			}
		}

		// Token: 0x060008AC RID: 2220 RVA: 0x00024856 File Offset: 0x00022A56
		public static implicit operator JSONNode(string s)
		{
			return new JSONString(s);
		}

		// Token: 0x060008AD RID: 2221 RVA: 0x0002485E File Offset: 0x00022A5E
		public static implicit operator string(JSONNode d)
		{
			if (!(d == null))
			{
				return d.Value;
			}
			return null;
		}

		// Token: 0x060008AE RID: 2222 RVA: 0x00024871 File Offset: 0x00022A71
		public static implicit operator JSONNode(double n)
		{
			return new JSONNumber(n);
		}

		// Token: 0x060008AF RID: 2223 RVA: 0x00024879 File Offset: 0x00022A79
		public static implicit operator double(JSONNode d)
		{
			if (!(d == null))
			{
				return d.AsDouble;
			}
			return 0.0;
		}

		// Token: 0x060008B0 RID: 2224 RVA: 0x00024894 File Offset: 0x00022A94
		public static implicit operator JSONNode(float n)
		{
			return new JSONNumber((double)n);
		}

		// Token: 0x060008B1 RID: 2225 RVA: 0x0002489D File Offset: 0x00022A9D
		public static implicit operator float(JSONNode d)
		{
			if (!(d == null))
			{
				return d.AsFloat;
			}
			return 0f;
		}

		// Token: 0x060008B2 RID: 2226 RVA: 0x00024894 File Offset: 0x00022A94
		public static implicit operator JSONNode(int n)
		{
			return new JSONNumber((double)n);
		}

		// Token: 0x060008B3 RID: 2227 RVA: 0x000248B4 File Offset: 0x00022AB4
		public static implicit operator int(JSONNode d)
		{
			if (!(d == null))
			{
				return d.AsInt;
			}
			return 0;
		}

		// Token: 0x060008B4 RID: 2228 RVA: 0x000248C7 File Offset: 0x00022AC7
		public static implicit operator JSONNode(long n)
		{
			if (JSONNode.longAsString)
			{
				return new JSONString(n.ToString());
			}
			return new JSONNumber((double)n);
		}

		// Token: 0x060008B5 RID: 2229 RVA: 0x000248E4 File Offset: 0x00022AE4
		public static implicit operator long(JSONNode d)
		{
			if (!(d == null))
			{
				return d.AsLong;
			}
			return 0L;
		}

		// Token: 0x060008B6 RID: 2230 RVA: 0x000248F8 File Offset: 0x00022AF8
		public static implicit operator JSONNode(bool b)
		{
			return new JSONBool(b);
		}

		// Token: 0x060008B7 RID: 2231 RVA: 0x00024900 File Offset: 0x00022B00
		public static implicit operator bool(JSONNode d)
		{
			return !(d == null) && d.AsBool;
		}

		// Token: 0x060008B8 RID: 2232 RVA: 0x00024913 File Offset: 0x00022B13
		public static implicit operator JSONNode(KeyValuePair<string, JSONNode> aKeyValue)
		{
			return aKeyValue.Value;
		}

		// Token: 0x060008B9 RID: 2233 RVA: 0x0002491C File Offset: 0x00022B1C
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

		// Token: 0x060008BA RID: 2234 RVA: 0x00024972 File Offset: 0x00022B72
		public static bool operator !=(JSONNode a, object b)
		{
			return !(a == b);
		}

		// Token: 0x060008BB RID: 2235 RVA: 0x0002497E File Offset: 0x00022B7E
		public override bool Equals(object obj)
		{
			return this == obj;
		}

		// Token: 0x060008BC RID: 2236 RVA: 0x00024984 File Offset: 0x00022B84
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x17000255 RID: 597
		// (get) Token: 0x060008BD RID: 2237 RVA: 0x0002498C File Offset: 0x00022B8C
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

		// Token: 0x060008BE RID: 2238 RVA: 0x000249A4 File Offset: 0x00022BA4
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

		// Token: 0x060008BF RID: 2239 RVA: 0x00024AF4 File Offset: 0x00022CF4
		private static JSONNode ParseElement(string token, bool quoted)
		{
			if (quoted)
			{
				return token;
			}
			string text = token.ToLower();
			if (text == "false" || text == "true")
			{
				return text == "true";
			}
			if (text == "null")
			{
				return JSONNull.CreateOrGet();
			}
			double num;
			if (double.TryParse(token, NumberStyles.Float, CultureInfo.InvariantCulture, out num))
			{
				return num;
			}
			return token;
		}

		// Token: 0x060008C0 RID: 2240 RVA: 0x00024B74 File Offset: 0x00022D74
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
				if (c <= '/')
				{
					if (c <= ' ')
					{
						switch (c)
						{
						case '\t':
							break;
						case '\n':
						case '\r':
							goto IL_03C7;
						case '\v':
						case '\f':
							goto IL_03B9;
						default:
							if (c != ' ')
							{
								goto IL_03B9;
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
							if (c != '/')
							{
								goto IL_03B9;
							}
							if (JSONNode.allowLineComments && !flag && i + 1 < aJSON.Length && aJSON[i + 1] == '/')
							{
								while (++i < aJSON.Length && aJSON[i] != '\n')
								{
									if (aJSON[i] == '\r')
									{
										break;
									}
								}
							}
							else
							{
								stringBuilder.Append(aJSON[i]);
							}
						}
						else if (flag)
						{
							stringBuilder.Append(aJSON[i]);
						}
						else
						{
							if (stringBuilder.Length > 0 || flag2)
							{
								jsonnode.Add(text, JSONNode.ParseElement(stringBuilder.ToString(), flag2));
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
									goto IL_03C7;
								}
								stack.Push(new JSONArray());
								if (jsonnode != null)
								{
									jsonnode.Add(text, stack.Peek());
								}
								text = "";
								stringBuilder.Length = 0;
								jsonnode = stack.Peek();
								goto IL_03C7;
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
											goto IL_03C7;
										}
										if (c2 == 'f')
										{
											stringBuilder.Append('\f');
											goto IL_03C7;
										}
									}
									else
									{
										if (c2 == 'n')
										{
											stringBuilder.Append('\n');
											goto IL_03C7;
										}
										switch (c2)
										{
										case 'r':
											stringBuilder.Append('\r');
											goto IL_03C7;
										case 't':
											stringBuilder.Append('\t');
											goto IL_03C7;
										case 'u':
										{
											string text2 = aJSON.Substring(i + 1, 4);
											stringBuilder.Append((char)int.Parse(text2, NumberStyles.AllowHexSpecifier));
											i += 4;
											goto IL_03C7;
										}
										}
									}
									stringBuilder.Append(c2);
									goto IL_03C7;
								}
								goto IL_03C7;
							case ']':
								break;
							default:
								goto IL_03B9;
							}
						}
						else
						{
							if (flag)
							{
								stringBuilder.Append(aJSON[i]);
								goto IL_03C7;
							}
							text = stringBuilder.ToString();
							stringBuilder.Length = 0;
							flag2 = false;
							goto IL_03C7;
						}
					}
					else if (c != '{')
					{
						if (c != '}')
						{
							if (c != '\ufeff')
							{
								goto IL_03B9;
							}
							goto IL_03C7;
						}
					}
					else
					{
						if (flag)
						{
							stringBuilder.Append(aJSON[i]);
							goto IL_03C7;
						}
						stack.Push(new JSONObject());
						if (jsonnode != null)
						{
							jsonnode.Add(text, stack.Peek());
						}
						text = "";
						stringBuilder.Length = 0;
						jsonnode = stack.Peek();
						goto IL_03C7;
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
							jsonnode.Add(text, JSONNode.ParseElement(stringBuilder.ToString(), flag2));
						}
						flag2 = false;
						text = "";
						stringBuilder.Length = 0;
						if (stack.Count > 0)
						{
							jsonnode = stack.Peek();
						}
					}
				}
				IL_03C7:
				i++;
				continue;
				IL_03B9:
				stringBuilder.Append(aJSON[i]);
				goto IL_03C7;
			}
			if (flag)
			{
				throw new Exception("JSON Parse: Quotation marks seems to be messed up.");
			}
			if (jsonnode == null)
			{
				return JSONNode.ParseElement(stringBuilder.ToString(), flag2);
			}
			return jsonnode;
		}

		// Token: 0x0400045C RID: 1116
		public static bool forceASCII = false;

		// Token: 0x0400045D RID: 1117
		public static bool longAsString = false;

		// Token: 0x0400045E RID: 1118
		public static bool allowLineComments = true;

		// Token: 0x0400045F RID: 1119
		[ThreadStatic]
		private static StringBuilder m_EscapeBuilder;

		// Token: 0x02000106 RID: 262
		public struct Enumerator
		{
			// Token: 0x170002A8 RID: 680
			// (get) Token: 0x06000B10 RID: 2832 RVA: 0x0002938C File Offset: 0x0002758C
			public bool IsValid
			{
				get
				{
					return this.type > JSONNode.Enumerator.Type.None;
				}
			}

			// Token: 0x06000B11 RID: 2833 RVA: 0x00029397 File Offset: 0x00027597
			public Enumerator(List<JSONNode>.Enumerator aArrayEnum)
			{
				this.type = JSONNode.Enumerator.Type.Array;
				this.m_Object = default(Dictionary<string, JSONNode>.Enumerator);
				this.m_Array = aArrayEnum;
			}

			// Token: 0x06000B12 RID: 2834 RVA: 0x000293B3 File Offset: 0x000275B3
			public Enumerator(Dictionary<string, JSONNode>.Enumerator aDictEnum)
			{
				this.type = JSONNode.Enumerator.Type.Object;
				this.m_Object = aDictEnum;
				this.m_Array = default(List<JSONNode>.Enumerator);
			}

			// Token: 0x170002A9 RID: 681
			// (get) Token: 0x06000B13 RID: 2835 RVA: 0x000293D0 File Offset: 0x000275D0
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

			// Token: 0x06000B14 RID: 2836 RVA: 0x0002941C File Offset: 0x0002761C
			public bool MoveNext()
			{
				if (this.type == JSONNode.Enumerator.Type.Array)
				{
					return this.m_Array.MoveNext();
				}
				return this.type == JSONNode.Enumerator.Type.Object && this.m_Object.MoveNext();
			}

			// Token: 0x04000607 RID: 1543
			private JSONNode.Enumerator.Type type;

			// Token: 0x04000608 RID: 1544
			private Dictionary<string, JSONNode>.Enumerator m_Object;

			// Token: 0x04000609 RID: 1545
			private List<JSONNode>.Enumerator m_Array;

			// Token: 0x0200011F RID: 287
			private enum Type
			{
				// Token: 0x04000657 RID: 1623
				None,
				// Token: 0x04000658 RID: 1624
				Array,
				// Token: 0x04000659 RID: 1625
				Object
			}
		}

		// Token: 0x02000107 RID: 263
		public struct ValueEnumerator
		{
			// Token: 0x06000B15 RID: 2837 RVA: 0x00029449 File Offset: 0x00027649
			public ValueEnumerator(List<JSONNode>.Enumerator aArrayEnum)
			{
				this = new JSONNode.ValueEnumerator(new JSONNode.Enumerator(aArrayEnum));
			}

			// Token: 0x06000B16 RID: 2838 RVA: 0x00029457 File Offset: 0x00027657
			public ValueEnumerator(Dictionary<string, JSONNode>.Enumerator aDictEnum)
			{
				this = new JSONNode.ValueEnumerator(new JSONNode.Enumerator(aDictEnum));
			}

			// Token: 0x06000B17 RID: 2839 RVA: 0x00029465 File Offset: 0x00027665
			public ValueEnumerator(JSONNode.Enumerator aEnumerator)
			{
				this.m_Enumerator = aEnumerator;
			}

			// Token: 0x170002AA RID: 682
			// (get) Token: 0x06000B18 RID: 2840 RVA: 0x00029470 File Offset: 0x00027670
			public JSONNode Current
			{
				get
				{
					KeyValuePair<string, JSONNode> keyValuePair = this.m_Enumerator.Current;
					return keyValuePair.Value;
				}
			}

			// Token: 0x06000B19 RID: 2841 RVA: 0x00029490 File Offset: 0x00027690
			public bool MoveNext()
			{
				return this.m_Enumerator.MoveNext();
			}

			// Token: 0x06000B1A RID: 2842 RVA: 0x0002949D File Offset: 0x0002769D
			public JSONNode.ValueEnumerator GetEnumerator()
			{
				return this;
			}

			// Token: 0x0400060A RID: 1546
			private JSONNode.Enumerator m_Enumerator;
		}

		// Token: 0x02000108 RID: 264
		public struct KeyEnumerator
		{
			// Token: 0x06000B1B RID: 2843 RVA: 0x000294A5 File Offset: 0x000276A5
			public KeyEnumerator(List<JSONNode>.Enumerator aArrayEnum)
			{
				this = new JSONNode.KeyEnumerator(new JSONNode.Enumerator(aArrayEnum));
			}

			// Token: 0x06000B1C RID: 2844 RVA: 0x000294B3 File Offset: 0x000276B3
			public KeyEnumerator(Dictionary<string, JSONNode>.Enumerator aDictEnum)
			{
				this = new JSONNode.KeyEnumerator(new JSONNode.Enumerator(aDictEnum));
			}

			// Token: 0x06000B1D RID: 2845 RVA: 0x000294C1 File Offset: 0x000276C1
			public KeyEnumerator(JSONNode.Enumerator aEnumerator)
			{
				this.m_Enumerator = aEnumerator;
			}

			// Token: 0x170002AB RID: 683
			// (get) Token: 0x06000B1E RID: 2846 RVA: 0x000294CC File Offset: 0x000276CC
			public string Current
			{
				get
				{
					KeyValuePair<string, JSONNode> keyValuePair = this.m_Enumerator.Current;
					return keyValuePair.Key;
				}
			}

			// Token: 0x06000B1F RID: 2847 RVA: 0x000294EC File Offset: 0x000276EC
			public bool MoveNext()
			{
				return this.m_Enumerator.MoveNext();
			}

			// Token: 0x06000B20 RID: 2848 RVA: 0x000294F9 File Offset: 0x000276F9
			public JSONNode.KeyEnumerator GetEnumerator()
			{
				return this;
			}

			// Token: 0x0400060B RID: 1547
			private JSONNode.Enumerator m_Enumerator;
		}

		// Token: 0x02000109 RID: 265
		public class LinqEnumerator : IEnumerator<KeyValuePair<string, JSONNode>>, IDisposable, IEnumerator, IEnumerable<KeyValuePair<string, JSONNode>>, IEnumerable
		{
			// Token: 0x06000B21 RID: 2849 RVA: 0x00029501 File Offset: 0x00027701
			internal LinqEnumerator(JSONNode aNode)
			{
				this.m_Node = aNode;
				if (this.m_Node != null)
				{
					this.m_Enumerator = this.m_Node.GetEnumerator();
				}
			}

			// Token: 0x170002AC RID: 684
			// (get) Token: 0x06000B22 RID: 2850 RVA: 0x0002952F File Offset: 0x0002772F
			public KeyValuePair<string, JSONNode> Current
			{
				get
				{
					return this.m_Enumerator.Current;
				}
			}

			// Token: 0x170002AD RID: 685
			// (get) Token: 0x06000B23 RID: 2851 RVA: 0x0002953C File Offset: 0x0002773C
			object IEnumerator.Current
			{
				get
				{
					return this.m_Enumerator.Current;
				}
			}

			// Token: 0x06000B24 RID: 2852 RVA: 0x0002954E File Offset: 0x0002774E
			public bool MoveNext()
			{
				return this.m_Enumerator.MoveNext();
			}

			// Token: 0x06000B25 RID: 2853 RVA: 0x0002955B File Offset: 0x0002775B
			public void Dispose()
			{
				this.m_Node = null;
				this.m_Enumerator = default(JSONNode.Enumerator);
			}

			// Token: 0x06000B26 RID: 2854 RVA: 0x00029570 File Offset: 0x00027770
			public IEnumerator<KeyValuePair<string, JSONNode>> GetEnumerator()
			{
				return new JSONNode.LinqEnumerator(this.m_Node);
			}

			// Token: 0x06000B27 RID: 2855 RVA: 0x0002957D File Offset: 0x0002777D
			public void Reset()
			{
				if (this.m_Node != null)
				{
					this.m_Enumerator = this.m_Node.GetEnumerator();
				}
			}

			// Token: 0x06000B28 RID: 2856 RVA: 0x00029570 File Offset: 0x00027770
			IEnumerator IEnumerable.GetEnumerator()
			{
				return new JSONNode.LinqEnumerator(this.m_Node);
			}

			// Token: 0x0400060C RID: 1548
			private JSONNode m_Node;

			// Token: 0x0400060D RID: 1549
			private JSONNode.Enumerator m_Enumerator;
		}
	}
}
