using System;
using System.Collections.Specialized;

namespace System.Net
{
	// Token: 0x02000142 RID: 322
	internal class KnownHttpVerb
	{
		// Token: 0x06000B5D RID: 2909 RVA: 0x0003DFA4 File Offset: 0x0003C1A4
		internal KnownHttpVerb(string name, bool requireContentBody, bool contentBodyNotAllowed, bool connectRequest, bool expectNoContentResponse)
		{
			this.Name = name;
			this.RequireContentBody = requireContentBody;
			this.ContentBodyNotAllowed = contentBodyNotAllowed;
			this.ConnectRequest = connectRequest;
			this.ExpectNoContentResponse = expectNoContentResponse;
		}

		// Token: 0x06000B5E RID: 2910 RVA: 0x0003DFD4 File Offset: 0x0003C1D4
		static KnownHttpVerb()
		{
			KnownHttpVerb.NamedHeaders[KnownHttpVerb.Get.Name] = KnownHttpVerb.Get;
			KnownHttpVerb.NamedHeaders[KnownHttpVerb.Connect.Name] = KnownHttpVerb.Connect;
			KnownHttpVerb.NamedHeaders[KnownHttpVerb.Head.Name] = KnownHttpVerb.Head;
			KnownHttpVerb.NamedHeaders[KnownHttpVerb.Put.Name] = KnownHttpVerb.Put;
			KnownHttpVerb.NamedHeaders[KnownHttpVerb.Post.Name] = KnownHttpVerb.Post;
			KnownHttpVerb.NamedHeaders[KnownHttpVerb.MkCol.Name] = KnownHttpVerb.MkCol;
		}

		// Token: 0x06000B5F RID: 2911 RVA: 0x0003E0F8 File Offset: 0x0003C2F8
		public bool Equals(KnownHttpVerb verb)
		{
			return this == verb || string.Compare(this.Name, verb.Name, StringComparison.OrdinalIgnoreCase) == 0;
		}

		// Token: 0x06000B60 RID: 2912 RVA: 0x0003E118 File Offset: 0x0003C318
		public static KnownHttpVerb Parse(string name)
		{
			KnownHttpVerb knownHttpVerb = KnownHttpVerb.NamedHeaders[name] as KnownHttpVerb;
			if (knownHttpVerb == null)
			{
				knownHttpVerb = new KnownHttpVerb(name, false, false, false, false);
			}
			return knownHttpVerb;
		}

		// Token: 0x040010C8 RID: 4296
		internal string Name;

		// Token: 0x040010C9 RID: 4297
		internal bool RequireContentBody;

		// Token: 0x040010CA RID: 4298
		internal bool ContentBodyNotAllowed;

		// Token: 0x040010CB RID: 4299
		internal bool ConnectRequest;

		// Token: 0x040010CC RID: 4300
		internal bool ExpectNoContentResponse;

		// Token: 0x040010CD RID: 4301
		private static ListDictionary NamedHeaders = new ListDictionary(CaseInsensitiveAscii.StaticInstance);

		// Token: 0x040010CE RID: 4302
		internal static KnownHttpVerb Get = new KnownHttpVerb("GET", false, true, false, false);

		// Token: 0x040010CF RID: 4303
		internal static KnownHttpVerb Connect = new KnownHttpVerb("CONNECT", false, true, true, false);

		// Token: 0x040010D0 RID: 4304
		internal static KnownHttpVerb Head = new KnownHttpVerb("HEAD", false, true, false, true);

		// Token: 0x040010D1 RID: 4305
		internal static KnownHttpVerb Put = new KnownHttpVerb("PUT", true, false, false, false);

		// Token: 0x040010D2 RID: 4306
		internal static KnownHttpVerb Post = new KnownHttpVerb("POST", true, false, false, false);

		// Token: 0x040010D3 RID: 4307
		internal static KnownHttpVerb MkCol = new KnownHttpVerb("MKCOL", false, false, false, false);
	}
}
