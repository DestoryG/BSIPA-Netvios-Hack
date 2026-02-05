using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace BeatSaverSharp
{
	// Token: 0x02000009 RID: 9
	internal class HttpResponse
	{
		// Token: 0x06000039 RID: 57 RVA: 0x00002DE4 File Offset: 0x00000FE4
		internal HttpResponse(HttpResponseMessage resp, byte[] body)
		{
			this.StatusCode = resp.StatusCode;
			this.ReasonPhrase = resp.ReasonPhrase;
			this.Headers = resp.Headers;
			this.RequestMessage = resp.RequestMessage;
			this.IsSuccessStatusCode = resp.IsSuccessStatusCode;
			this.RateLimit = RateLimitInfo.FromHttp(resp);
			this._body = body;
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002E46 File Offset: 0x00001046
		public byte[] Bytes()
		{
			return this._body;
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00002E4E File Offset: 0x0000104E
		public string String()
		{
			return Encoding.UTF8.GetString(this._body);
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002E60 File Offset: 0x00001060
		public T JSON<T>()
		{
			T t;
			using (StringReader stringReader = new StringReader(this.String()))
			{
				using (JsonTextReader jsonTextReader = new JsonTextReader(stringReader))
				{
					t = Http.Serializer.Deserialize<T>(jsonTextReader);
				}
			}
			return t;
		}

		// Token: 0x04000012 RID: 18
		public readonly HttpStatusCode StatusCode;

		// Token: 0x04000013 RID: 19
		public readonly string ReasonPhrase;

		// Token: 0x04000014 RID: 20
		public readonly HttpResponseHeaders Headers;

		// Token: 0x04000015 RID: 21
		public readonly HttpRequestMessage RequestMessage;

		// Token: 0x04000016 RID: 22
		public readonly bool IsSuccessStatusCode;

		// Token: 0x04000017 RID: 23
		public readonly RateLimitInfo? RateLimit;

		// Token: 0x04000018 RID: 24
		private readonly byte[] _body;
	}
}
