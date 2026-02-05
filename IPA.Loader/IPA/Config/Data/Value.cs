using System;
using System.Collections.Generic;

namespace IPA.Config.Data
{
	/// <summary>
	/// A base value type for config data abstract representations, to be serialized with an
	/// <see cref="T:IPA.Config.IConfigProvider" />. If a <see cref="T:IPA.Config.Data.Value" /> is <see langword="null" />, then
	/// that represents just that: a <c>null</c> in whatever serialization is being used.
	/// Also contains factory functions for all derived types.
	/// </summary>
	// Token: 0x0200009A RID: 154
	public abstract class Value
	{
		/// <summary>
		/// Converts this <see cref="T:IPA.Config.Data.Value" /> into a human-readable format.
		/// </summary>
		/// <returns>a human-readable string containing the value provided</returns>
		// Token: 0x060003D8 RID: 984
		public abstract override string ToString();

		/// <summary>
		/// Creates a Null <see cref="T:IPA.Config.Data.Value" />.
		/// </summary>
		/// <returns><see langword="null" /></returns>
		// Token: 0x060003D9 RID: 985 RVA: 0x0001364C File Offset: 0x0001184C
		public static Value Null()
		{
			return null;
		}

		/// <summary>
		/// Creates an empty <see cref="M:IPA.Config.Data.Value.List" />.
		/// </summary>
		/// <returns>an empty <see cref="M:IPA.Config.Data.Value.List" /></returns>
		/// <seealso cref="M:IPA.Config.Data.Value.From(System.Collections.Generic.IEnumerable{IPA.Config.Data.Value})" />
		// Token: 0x060003DA RID: 986 RVA: 0x0001364F File Offset: 0x0001184F
		public static List List()
		{
			return new List();
		}

		/// <summary>
		/// Creates an empty <see cref="M:IPA.Config.Data.Value.Map" />.
		/// </summary>
		/// <returns>an empty <see cref="M:IPA.Config.Data.Value.Map" /></returns>
		/// <seealso cref="M:IPA.Config.Data.Value.From(System.Collections.Generic.IDictionary{System.String,IPA.Config.Data.Value})" />
		/// <seealso cref="M:IPA.Config.Data.Value.From(System.Collections.Generic.IEnumerable{System.Collections.Generic.KeyValuePair{System.String,IPA.Config.Data.Value}})" />
		// Token: 0x060003DB RID: 987 RVA: 0x00013656 File Offset: 0x00011856
		public static Map Map()
		{
			return new Map();
		}

		/// <summary>
		/// Creates a new <see cref="T:IPA.Config.Data.Value" /> representing a <see cref="T:System.String" />.
		/// </summary>
		/// <param name="val">the value to wrap</param>
		/// <returns>a <see cref="T:IPA.Config.Data.Text" /> wrapping <paramref name="val" /></returns>
		/// <seealso cref="M:IPA.Config.Data.Value.Text(System.String)" />
		// Token: 0x060003DC RID: 988 RVA: 0x0001365D File Offset: 0x0001185D
		public static Text From(string val)
		{
			return Value.Text(val);
		}

		/// <summary>
		/// Creates a new <see cref="T:IPA.Config.Data.Text" /> object wrapping a <see cref="T:System.String" />.
		/// </summary>
		/// <param name="val">the value to wrap</param>
		/// <returns>a <see cref="T:IPA.Config.Data.Text" /> wrapping <paramref name="val" /></returns>
		/// <seealso cref="M:IPA.Config.Data.Value.From(System.String)" />
		// Token: 0x060003DD RID: 989 RVA: 0x00013665 File Offset: 0x00011865
		public static Text Text(string val)
		{
			if (val != null)
			{
				return new Text
				{
					Value = val
				};
			}
			return null;
		}

		/// <summary>
		/// Creates a new <see cref="T:IPA.Config.Data.Value" /> wrapping a <see cref="T:System.Int64" />.
		/// </summary>
		/// <param name="val">the value to wrap</param>
		/// <returns>a <see cref="T:IPA.Config.Data.Integer" /> wrapping <paramref name="val" /></returns>
		/// <seealso cref="M:IPA.Config.Data.Value.Integer(System.Int64)" />
		// Token: 0x060003DE RID: 990 RVA: 0x00013678 File Offset: 0x00011878
		public static Integer From(long val)
		{
			return Value.Integer(val);
		}

		/// <summary>
		/// Creates a new <see cref="T:IPA.Config.Data.Integer" /> wrapping a <see cref="T:System.Int64" />.
		/// </summary>
		/// <param name="val">the value to wrap</param>
		/// <returns>a <see cref="T:IPA.Config.Data.Integer" /> wrapping <paramref name="val" /></returns>
		/// <seealso cref="M:IPA.Config.Data.Value.From(System.Int64)" />
		// Token: 0x060003DF RID: 991 RVA: 0x00013680 File Offset: 0x00011880
		public static Integer Integer(long val)
		{
			return new Integer
			{
				Value = val
			};
		}

		/// <summary>
		/// Creates a new <see cref="T:IPA.Config.Data.Value" /> wrapping a <see cref="T:System.Double" />.
		/// </summary>
		/// <param name="val">the value to wrap</param>
		/// <returns>a <see cref="T:IPA.Config.Data.FloatingPoint" /> wrapping <paramref name="val" /></returns>
		/// <seealso cref="M:IPA.Config.Data.Value.Float(System.Decimal)" />
		// Token: 0x060003E0 RID: 992 RVA: 0x0001368E File Offset: 0x0001188E
		public static FloatingPoint From(decimal val)
		{
			return Value.Float(val);
		}

		/// <summary>
		/// Creates a new <see cref="T:IPA.Config.Data.FloatingPoint" /> wrapping a <see cref="T:System.Decimal" />.
		/// </summary>
		/// <param name="val">the value to wrap</param>
		/// <returns>a <see cref="T:IPA.Config.Data.FloatingPoint" /> wrapping <paramref name="val" /></returns>
		/// <seealso cref="M:IPA.Config.Data.Value.From(System.Decimal)" />
		// Token: 0x060003E1 RID: 993 RVA: 0x00013696 File Offset: 0x00011896
		public static FloatingPoint Float(decimal val)
		{
			return new FloatingPoint
			{
				Value = val
			};
		}

		/// <summary>
		/// Creates a new <see cref="T:IPA.Config.Data.Value" /> wrapping a <see cref="T:System.Boolean" />.
		/// </summary>
		/// <param name="val">the  value to wrap</param>
		/// <returns>a <see cref="T:IPA.Config.Data.Boolean" /> wrapping <paramref name="val" /></returns>
		/// <seealso cref="M:IPA.Config.Data.Value.Bool(System.Boolean)" />
		// Token: 0x060003E2 RID: 994 RVA: 0x000136A4 File Offset: 0x000118A4
		public static Boolean From(bool val)
		{
			return Value.Bool(val);
		}

		/// <summary>
		/// Creates a new <see cref="T:IPA.Config.Data.Boolean" /> wrapping a <see cref="T:System.Boolean" />.
		/// </summary>
		/// <param name="val">the value to wrap</param>
		/// <returns>a <see cref="T:IPA.Config.Data.Boolean" /> wrapping <paramref name="val" /></returns>
		/// <seealso cref="M:IPA.Config.Data.Value.From(System.Boolean)" />
		// Token: 0x060003E3 RID: 995 RVA: 0x000136AC File Offset: 0x000118AC
		public static Boolean Bool(bool val)
		{
			return new Boolean
			{
				Value = val
			};
		}

		/// <summary>
		/// Creates a new <see cref="T:IPA.Config.Data.List" /> holding the content of an <see cref="T:System.Collections.Generic.IEnumerable`1" />
		/// of <see cref="T:IPA.Config.Data.Value" />.
		/// </summary>
		/// <param name="vals">the <see cref="T:IPA.Config.Data.Value" />s to initialize the <see cref="T:IPA.Config.Data.List" /> with</param>
		/// <returns>a <see cref="T:IPA.Config.Data.List" /> containing the content of <paramref name="vals" /></returns>
		/// <seealso cref="M:IPA.Config.Data.Value.List" />
		// Token: 0x060003E4 RID: 996 RVA: 0x000136BA File Offset: 0x000118BA
		public static List From(IEnumerable<Value> vals)
		{
			if (vals == null)
			{
				return null;
			}
			List list = Value.List();
			list.AddRange(vals);
			return list;
		}

		/// <summary>
		/// Creates a new <see cref="T:IPA.Config.Data.Map" /> holding the content of an <see cref="T:System.Collections.Generic.IDictionary`2" />
		/// of <see cref="T:System.String" /> to <see cref="T:IPA.Config.Data.Value" />.
		/// </summary>
		/// <param name="vals">the dictionary of <see cref="T:IPA.Config.Data.Value" />s to initialize the <see cref="T:IPA.Config.Data.Map" /> wtih</param>
		/// <returns>a <see cref="T:IPA.Config.Data.Map" /> containing the content of <paramref name="vals" /></returns>
		/// <seealso cref="M:IPA.Config.Data.Value.Map" />
		/// <seealso cref="M:IPA.Config.Data.Value.From(System.Collections.Generic.IEnumerable{System.Collections.Generic.KeyValuePair{System.String,IPA.Config.Data.Value}})" />
		// Token: 0x060003E5 RID: 997 RVA: 0x000136CD File Offset: 0x000118CD
		public static Map From(IDictionary<string, Value> vals)
		{
			return Value.From(vals);
		}

		/// <summary>
		/// Creates a new <see cref="T:IPA.Config.Data.Map" /> holding the content of an <see cref="T:System.Collections.Generic.IEnumerable`1" />
		/// of <see cref="T:System.Collections.Generic.KeyValuePair`2" /> of <see cref="T:System.String" /> to <see cref="T:IPA.Config.Data.Value" />.
		/// </summary>
		/// <param name="vals">the enumerable of <see cref="T:System.Collections.Generic.KeyValuePair`2" /> of name to <see cref="T:IPA.Config.Data.Value" /></param>
		/// <returns>a <see cref="T:IPA.Config.Data.Map" /> containing the content of <paramref name="vals" /></returns>
		/// <seealso cref="M:IPA.Config.Data.Value.Map" />
		/// <seealso cref="M:IPA.Config.Data.Value.From(System.Collections.Generic.IDictionary{System.String,IPA.Config.Data.Value})" />
		// Token: 0x060003E6 RID: 998 RVA: 0x000136D8 File Offset: 0x000118D8
		public static Map From(IEnumerable<KeyValuePair<string, Value>> vals)
		{
			if (vals == null)
			{
				return null;
			}
			Map i = Value.Map();
			foreach (KeyValuePair<string, Value> v in vals)
			{
				i.Add(v.Key, v.Value);
			}
			return i;
		}
	}
}
