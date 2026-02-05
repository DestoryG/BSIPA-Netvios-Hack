using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using IPA.Config.Data;
using IPA.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IPA.Config.Providers
{
	// Token: 0x0200009B RID: 155
	internal class JsonConfigProvider : IConfigProvider
	{
		// Token: 0x060003E8 RID: 1000 RVA: 0x00013744 File Offset: 0x00011944
		public static void RegisterConfig()
		{
			Config.Register<JsonConfigProvider>();
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x060003E9 RID: 1001 RVA: 0x0001374B File Offset: 0x0001194B
		public string Extension
		{
			get
			{
				return "json";
			}
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x00013754 File Offset: 0x00011954
		public Value Load(FileInfo file)
		{
			if (!file.Exists)
			{
				return Value.Null();
			}
			Value value;
			try
			{
				JToken jtok;
				using (StreamReader sreader = new StreamReader(file.OpenRead()))
				{
					using (JsonTextReader jreader = new JsonTextReader(sreader))
					{
						jtok = JToken.ReadFrom(jreader);
					}
				}
				value = this.VisitToValue(jtok);
			}
			catch (Exception e)
			{
				Logger.config.Error("Error reading JSON file " + file.FullName + "; ignoring");
				Logger.config.Error(e);
				value = Value.Null();
			}
			return value;
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x0001380C File Offset: 0x00011A0C
		private Value VisitToValue(JToken tok)
		{
			if (tok == null)
			{
				return Value.Null();
			}
			switch (tok.Type)
			{
			case JTokenType.Object:
				return Value.From((tok as IEnumerable<KeyValuePair<string, JToken>>).Select((KeyValuePair<string, JToken> kvp) => new KeyValuePair<string, Value>(kvp.Key, this.VisitToValue(kvp.Value))));
			case JTokenType.Array:
				return Value.From((tok as JArray).Select(new Func<JToken, Value>(this.VisitToValue)));
			case JTokenType.Constructor:
				Logger.config.Warn("Found JTokenType.Constructor");
				break;
			case JTokenType.Property:
				Logger.config.Warn("Found JTokenType.Property");
				break;
			case JTokenType.Comment:
				Logger.config.Warn("Found JTokenType.Comment");
				break;
			case JTokenType.Integer:
			{
				object val = (tok as JValue).Value;
				if (val is long)
				{
					long i = (long)val;
					return Value.Integer(i);
				}
				if (val is ulong)
				{
					ulong u = (ulong)val;
					return Value.Integer((long)u);
				}
				return Value.Integer(0L);
			}
			case JTokenType.Float:
			{
				object val = (tok as JValue).Value;
				if (val is decimal)
				{
					decimal dec = (decimal)val;
					return Value.Float(dec);
				}
				if (val is double)
				{
					double dou = (double)val;
					return Value.Float((decimal)dou);
				}
				if (val is float)
				{
					float flo = (float)val;
					return Value.Float((decimal)flo);
				}
				return Value.Float(0m);
			}
			case JTokenType.String:
			{
				object val = (tok as JValue).Value;
				string s = val as string;
				if (s != null)
				{
					return Value.Text(s);
				}
				if (val is char)
				{
					return Value.Text(((char)val).ToString() ?? "");
				}
				return Value.Text(string.Empty);
			}
			case JTokenType.Boolean:
				return Value.Bool(((tok as JValue).Value as bool?).GetValueOrDefault());
			case JTokenType.Null:
				break;
			case JTokenType.Undefined:
				Logger.config.Warn("Found JTokenType.Undefined");
				break;
			case JTokenType.Date:
			{
				object val = (tok as JValue).Value;
				if (val is DateTime)
				{
					return Value.Text(((DateTime)val).ToString());
				}
				if (val is DateTimeOffset)
				{
					return Value.Text(((DateTimeOffset)val).ToString());
				}
				return Value.Text("Unknown Date-type token");
			}
			case JTokenType.Raw:
				return this.VisitToValue(JToken.Parse((tok as JRaw).Value as string));
			case JTokenType.Bytes:
				Logger.config.Warn("Found JTokenType.Bytes");
				break;
			case JTokenType.Guid:
			{
				object val = (tok as JValue).Value;
				if (val is Guid)
				{
					return Value.Text(((Guid)val).ToString());
				}
				return Value.Text("Unknown Guid-type token");
			}
			case JTokenType.Uri:
			{
				object val = (tok as JValue).Value;
				Uri ur = val as Uri;
				if (ur != null)
				{
					return Value.Text(ur.ToString());
				}
				return Value.Text("Unknown Uri-type token");
			}
			case JTokenType.TimeSpan:
			{
				object val = (tok as JValue).Value;
				if (val is TimeSpan)
				{
					return Value.Text(((TimeSpan)val).ToString());
				}
				return Value.Text("Unknown TimeSpan-type token");
			}
			default:
				throw new ArgumentException("Unknown JTokenType in parameter");
			}
			return Value.Null();
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x00013B4C File Offset: 0x00011D4C
		public void Store(Value value, FileInfo file)
		{
			if (!file.Directory.Exists)
			{
				file.Directory.Create();
			}
			try
			{
				JToken tok = this.VisitToToken(value);
				using (StreamWriter swriter = new StreamWriter(file.Open(FileMode.Create, FileAccess.Write)))
				{
					using (JsonTextWriter jwriter = new JsonTextWriter(swriter)
					{
						Formatting = Formatting.Indented
					})
					{
						tok.WriteTo(jwriter, Array.Empty<JsonConverter>());
					}
				}
			}
			catch (Exception e)
			{
				Logger.config.Error("Error serializing value for " + file.FullName);
				Logger.config.Error(e);
			}
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x00013C0C File Offset: 0x00011E0C
		private JToken VisitToToken(Value val)
		{
			Text t = val as Text;
			if (t != null)
			{
				return new JValue(t.Value);
			}
			IPA.Config.Data.Boolean b = val as IPA.Config.Data.Boolean;
			if (b != null)
			{
				return new JValue(b.Value);
			}
			Integer i = val as Integer;
			if (i != null)
			{
				return new JValue(i.Value);
			}
			FloatingPoint f = val as FloatingPoint;
			if (f != null)
			{
				return new JValue(f.Value);
			}
			List j = val as List;
			if (j != null)
			{
				JArray jarr = new JArray();
				foreach (JToken tok in j.Select(new Func<Value, JToken>(this.VisitToToken)))
				{
					jarr.Add(tok);
				}
				return jarr;
			}
			Map k = val as Map;
			if (k != null)
			{
				JObject jobj = new JObject();
				foreach (KeyValuePair<string, Value> kvp in k)
				{
					jobj.Add(kvp.Key, this.VisitToToken(kvp.Value));
				}
				return jobj;
			}
			if (val != null)
			{
				throw new ArgumentException("Unsupported subtype of Value");
			}
			return JValue.CreateNull();
		}
	}
}
