using System;
using System.Collections.Generic;

namespace Mono.Cecil.Cil
{
	// Token: 0x02000111 RID: 273
	internal static class PdbGuidMapping
	{
		// Token: 0x06000AB5 RID: 2741 RVA: 0x00023704 File Offset: 0x00021904
		static PdbGuidMapping()
		{
			PdbGuidMapping.AddMapping(DocumentLanguage.C, new Guid("63a08714-fc37-11d2-904c-00c04fa302a1"));
			PdbGuidMapping.AddMapping(DocumentLanguage.Cpp, new Guid("3a12d0b7-c26c-11d0-b442-00a0244a1dd2"));
			PdbGuidMapping.AddMapping(DocumentLanguage.CSharp, new Guid("3f5162f8-07c6-11d3-9053-00c04fa302a1"));
			PdbGuidMapping.AddMapping(DocumentLanguage.Basic, new Guid("3a12d0b8-c26c-11d0-b442-00a0244a1dd2"));
			PdbGuidMapping.AddMapping(DocumentLanguage.Java, new Guid("3a12d0b4-c26c-11d0-b442-00a0244a1dd2"));
			PdbGuidMapping.AddMapping(DocumentLanguage.Cobol, new Guid("af046cd1-d0e1-11d2-977c-00a0c9b4d50c"));
			PdbGuidMapping.AddMapping(DocumentLanguage.Pascal, new Guid("af046cd2-d0e1-11d2-977c-00a0c9b4d50c"));
			PdbGuidMapping.AddMapping(DocumentLanguage.Cil, new Guid("af046cd3-d0e1-11d2-977c-00a0c9b4d50c"));
			PdbGuidMapping.AddMapping(DocumentLanguage.JScript, new Guid("3a12d0b6-c26c-11d0-b442-00a0244a1dd2"));
			PdbGuidMapping.AddMapping(DocumentLanguage.Smc, new Guid("0d9b9f7b-6611-11d3-bd2a-0000f80849bd"));
			PdbGuidMapping.AddMapping(DocumentLanguage.MCpp, new Guid("4b35fde8-07c6-11d3-9053-00c04fa302a1"));
			PdbGuidMapping.AddMapping(DocumentLanguage.FSharp, new Guid("ab4f38c9-b6e6-43ba-be3b-58080b2ccce3"));
		}

		// Token: 0x06000AB6 RID: 2742 RVA: 0x00023834 File Offset: 0x00021A34
		private static void AddMapping(DocumentLanguage language, Guid guid)
		{
			PdbGuidMapping.guid_language.Add(guid, language);
			PdbGuidMapping.language_guid.Add(language, guid);
		}

		// Token: 0x06000AB7 RID: 2743 RVA: 0x0002384E File Offset: 0x00021A4E
		public static DocumentType ToType(this Guid guid)
		{
			if (guid == PdbGuidMapping.type_text)
			{
				return DocumentType.Text;
			}
			return DocumentType.Other;
		}

		// Token: 0x06000AB8 RID: 2744 RVA: 0x00023860 File Offset: 0x00021A60
		public static Guid ToGuid(this DocumentType type)
		{
			if (type == DocumentType.Text)
			{
				return PdbGuidMapping.type_text;
			}
			return default(Guid);
		}

		// Token: 0x06000AB9 RID: 2745 RVA: 0x00023880 File Offset: 0x00021A80
		public static DocumentHashAlgorithm ToHashAlgorithm(this Guid guid)
		{
			if (guid == PdbGuidMapping.hash_md5)
			{
				return DocumentHashAlgorithm.MD5;
			}
			if (guid == PdbGuidMapping.hash_sha1)
			{
				return DocumentHashAlgorithm.SHA1;
			}
			if (guid == PdbGuidMapping.hash_sha256)
			{
				return DocumentHashAlgorithm.SHA256;
			}
			return DocumentHashAlgorithm.None;
		}

		// Token: 0x06000ABA RID: 2746 RVA: 0x000238B0 File Offset: 0x00021AB0
		public static Guid ToGuid(this DocumentHashAlgorithm hash_algo)
		{
			if (hash_algo == DocumentHashAlgorithm.MD5)
			{
				return PdbGuidMapping.hash_md5;
			}
			if (hash_algo == DocumentHashAlgorithm.SHA1)
			{
				return PdbGuidMapping.hash_sha1;
			}
			if (hash_algo == DocumentHashAlgorithm.SHA256)
			{
				return PdbGuidMapping.hash_sha256;
			}
			return default(Guid);
		}

		// Token: 0x06000ABB RID: 2747 RVA: 0x000238E4 File Offset: 0x00021AE4
		public static DocumentLanguage ToLanguage(this Guid guid)
		{
			DocumentLanguage documentLanguage;
			if (!PdbGuidMapping.guid_language.TryGetValue(guid, out documentLanguage))
			{
				return DocumentLanguage.Other;
			}
			return documentLanguage;
		}

		// Token: 0x06000ABC RID: 2748 RVA: 0x00023904 File Offset: 0x00021B04
		public static Guid ToGuid(this DocumentLanguage language)
		{
			Guid guid;
			if (!PdbGuidMapping.language_guid.TryGetValue(language, out guid))
			{
				return default(Guid);
			}
			return guid;
		}

		// Token: 0x06000ABD RID: 2749 RVA: 0x0002392B File Offset: 0x00021B2B
		public static DocumentLanguageVendor ToVendor(this Guid guid)
		{
			if (guid == PdbGuidMapping.vendor_ms)
			{
				return DocumentLanguageVendor.Microsoft;
			}
			return DocumentLanguageVendor.Other;
		}

		// Token: 0x06000ABE RID: 2750 RVA: 0x00023940 File Offset: 0x00021B40
		public static Guid ToGuid(this DocumentLanguageVendor vendor)
		{
			if (vendor == DocumentLanguageVendor.Microsoft)
			{
				return PdbGuidMapping.vendor_ms;
			}
			return default(Guid);
		}

		// Token: 0x04000681 RID: 1665
		private static readonly Dictionary<Guid, DocumentLanguage> guid_language = new Dictionary<Guid, DocumentLanguage>();

		// Token: 0x04000682 RID: 1666
		private static readonly Dictionary<DocumentLanguage, Guid> language_guid = new Dictionary<DocumentLanguage, Guid>();

		// Token: 0x04000683 RID: 1667
		private static readonly Guid type_text = new Guid("5a869d0b-6611-11d3-bd2a-0000f80849bd");

		// Token: 0x04000684 RID: 1668
		private static readonly Guid hash_md5 = new Guid("406ea660-64cf-4c82-b6f0-42d48172a799");

		// Token: 0x04000685 RID: 1669
		private static readonly Guid hash_sha1 = new Guid("ff1816ec-aa5e-4d10-87f7-6f4963833460");

		// Token: 0x04000686 RID: 1670
		private static readonly Guid hash_sha256 = new Guid("8829d00f-11b8-4213-878b-770e8597ac16");

		// Token: 0x04000687 RID: 1671
		private static readonly Guid vendor_ms = new Guid("994b45c4-e6e9-11d2-903f-00c04fa302a1");
	}
}
