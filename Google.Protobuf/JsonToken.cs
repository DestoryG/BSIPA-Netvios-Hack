using System;

namespace Google.Protobuf
{
	// Token: 0x0200001C RID: 28
	internal sealed class JsonToken : IEquatable<JsonToken>
	{
		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000194 RID: 404 RVA: 0x00009051 File Offset: 0x00007251
		internal static JsonToken Null
		{
			get
			{
				return JsonToken._null;
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000195 RID: 405 RVA: 0x00009058 File Offset: 0x00007258
		internal static JsonToken False
		{
			get
			{
				return JsonToken._false;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000196 RID: 406 RVA: 0x0000905F File Offset: 0x0000725F
		internal static JsonToken True
		{
			get
			{
				return JsonToken._true;
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000197 RID: 407 RVA: 0x00009066 File Offset: 0x00007266
		internal static JsonToken StartObject
		{
			get
			{
				return JsonToken.startObject;
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000198 RID: 408 RVA: 0x0000906D File Offset: 0x0000726D
		internal static JsonToken EndObject
		{
			get
			{
				return JsonToken.endObject;
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000199 RID: 409 RVA: 0x00009074 File Offset: 0x00007274
		internal static JsonToken StartArray
		{
			get
			{
				return JsonToken.startArray;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600019A RID: 410 RVA: 0x0000907B File Offset: 0x0000727B
		internal static JsonToken EndArray
		{
			get
			{
				return JsonToken.endArray;
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600019B RID: 411 RVA: 0x00009082 File Offset: 0x00007282
		internal static JsonToken EndDocument
		{
			get
			{
				return JsonToken.endDocument;
			}
		}

		// Token: 0x0600019C RID: 412 RVA: 0x00009089 File Offset: 0x00007289
		internal static JsonToken Name(string name)
		{
			return new JsonToken(JsonToken.TokenType.Name, name, 0.0);
		}

		// Token: 0x0600019D RID: 413 RVA: 0x0000909B File Offset: 0x0000729B
		internal static JsonToken Value(string value)
		{
			return new JsonToken(JsonToken.TokenType.StringValue, value, 0.0);
		}

		// Token: 0x0600019E RID: 414 RVA: 0x000090AD File Offset: 0x000072AD
		internal static JsonToken Value(double value)
		{
			return new JsonToken(JsonToken.TokenType.Number, null, value);
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600019F RID: 415 RVA: 0x000090B7 File Offset: 0x000072B7
		internal JsonToken.TokenType Type
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060001A0 RID: 416 RVA: 0x000090BF File Offset: 0x000072BF
		internal string StringValue
		{
			get
			{
				return this.stringValue;
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060001A1 RID: 417 RVA: 0x000090C7 File Offset: 0x000072C7
		internal double NumberValue
		{
			get
			{
				return this.numberValue;
			}
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x000090CF File Offset: 0x000072CF
		private JsonToken(JsonToken.TokenType type, string stringValue = null, double numberValue = 0.0)
		{
			this.type = type;
			this.stringValue = stringValue;
			this.numberValue = numberValue;
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x000090EC File Offset: 0x000072EC
		public override bool Equals(object obj)
		{
			return this.Equals(obj as JsonToken);
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x000090FC File Offset: 0x000072FC
		public override int GetHashCode()
		{
			return ((((int)(((JsonToken.TokenType)17 * (JsonToken.TokenType)31 + (int)this.type) * (JsonToken.TokenType)31)).ToString() + this.stringValue == null) ? 0 : this.stringValue.GetHashCode()) * 31 + this.numberValue.GetHashCode();
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x00009150 File Offset: 0x00007350
		public override string ToString()
		{
			switch (this.type)
			{
			case JsonToken.TokenType.Null:
				return "null";
			case JsonToken.TokenType.False:
				return "false";
			case JsonToken.TokenType.True:
				return "true";
			case JsonToken.TokenType.StringValue:
				return "value (" + this.stringValue + ")";
			case JsonToken.TokenType.Number:
				return "number (" + this.numberValue.ToString() + ")";
			case JsonToken.TokenType.Name:
				return "name (" + this.stringValue + ")";
			case JsonToken.TokenType.StartObject:
				return "start-object";
			case JsonToken.TokenType.EndObject:
				return "end-object";
			case JsonToken.TokenType.StartArray:
				return "start-array";
			case JsonToken.TokenType.EndArray:
				return "end-array";
			case JsonToken.TokenType.EndDocument:
				return "end-document";
			default:
				throw new InvalidOperationException("Token is of unknown type " + this.type.ToString());
			}
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x00009238 File Offset: 0x00007438
		public bool Equals(JsonToken other)
		{
			return other != null && (other.type == this.type && other.stringValue == this.stringValue) && other.numberValue.Equals(this.numberValue);
		}

		// Token: 0x0400004F RID: 79
		private static readonly JsonToken _true = new JsonToken(JsonToken.TokenType.True, null, 0.0);

		// Token: 0x04000050 RID: 80
		private static readonly JsonToken _false = new JsonToken(JsonToken.TokenType.False, null, 0.0);

		// Token: 0x04000051 RID: 81
		private static readonly JsonToken _null = new JsonToken(JsonToken.TokenType.Null, null, 0.0);

		// Token: 0x04000052 RID: 82
		private static readonly JsonToken startObject = new JsonToken(JsonToken.TokenType.StartObject, null, 0.0);

		// Token: 0x04000053 RID: 83
		private static readonly JsonToken endObject = new JsonToken(JsonToken.TokenType.EndObject, null, 0.0);

		// Token: 0x04000054 RID: 84
		private static readonly JsonToken startArray = new JsonToken(JsonToken.TokenType.StartArray, null, 0.0);

		// Token: 0x04000055 RID: 85
		private static readonly JsonToken endArray = new JsonToken(JsonToken.TokenType.EndArray, null, 0.0);

		// Token: 0x04000056 RID: 86
		private static readonly JsonToken endDocument = new JsonToken(JsonToken.TokenType.EndDocument, null, 0.0);

		// Token: 0x04000057 RID: 87
		private readonly JsonToken.TokenType type;

		// Token: 0x04000058 RID: 88
		private readonly string stringValue;

		// Token: 0x04000059 RID: 89
		private readonly double numberValue;

		// Token: 0x020000A5 RID: 165
		internal enum TokenType
		{
			// Token: 0x040003A7 RID: 935
			Null,
			// Token: 0x040003A8 RID: 936
			False,
			// Token: 0x040003A9 RID: 937
			True,
			// Token: 0x040003AA RID: 938
			StringValue,
			// Token: 0x040003AB RID: 939
			Number,
			// Token: 0x040003AC RID: 940
			Name,
			// Token: 0x040003AD RID: 941
			StartObject,
			// Token: 0x040003AE RID: 942
			EndObject,
			// Token: 0x040003AF RID: 943
			StartArray,
			// Token: 0x040003B0 RID: 944
			EndArray,
			// Token: 0x040003B1 RID: 945
			EndDocument
		}
	}
}
