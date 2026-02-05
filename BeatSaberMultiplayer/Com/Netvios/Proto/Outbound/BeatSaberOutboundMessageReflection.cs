using System;
using Google.Protobuf.Reflection;

namespace Com.Netvios.Proto.Outbound
{
	// Token: 0x02000007 RID: 7
	public static class BeatSaberOutboundMessageReflection
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000010 RID: 16 RVA: 0x000025B1 File Offset: 0x000007B1
		public static FileDescriptor Descriptor
		{
			get
			{
				return BeatSaberOutboundMessageReflection.descriptor;
			}
		}

		// Token: 0x04000030 RID: 48
		private static FileDescriptor descriptor = FileDescriptor.FromGeneratedCode(Convert.FromBase64String(string.Concat(new string[]
		{
			"Ch5CZWF0U2FiZXJPdXRib3VuZE1lc3NhZ2UucHJvdG8SGmNvbS5uZXR2aW9z", "LnByb3RvLm91dGJvdW5kGhZCZWF0U2FiZXJNZXNzYWdlLnByb3RvIv4PCg1C", "ZWF0U2FiZXJCb2R5EikKBHR5cGUYASABKA4yGy5jb20ubmV0dmlvcy5wcm90", "by5EYXRhVHlwZRIwCgRwaW5nGAIgASgLMiAuY29tLm5ldHZpb3MucHJvdG8u", "b3V0Ym91bmQuUGluZ0gAEjIKBWxvZ2luGAMgASgLMiEuY29tLm5ldHZpb3Mu", "cHJvdG8ub3V0Ym91bmQuTG9naW5IABIyCgVyZW5ldxgEIAEoCzIhLmNvbS5u", "ZXR2aW9zLnByb3RvLm91dGJvdW5kLlJlbmV3SAASNAoGbG9nb3V0GAUgASgL", "MiIuY29tLm5ldHZpb3MucHJvdG8ub3V0Ym91bmQuTG9nb3V0SAASOgoJZ2V0", "UGxheWVyGAYgASgLMiUuY29tLm5ldHZpb3MucHJvdG8ub3V0Ym91bmQuR2V0", "UGxheWVySAASOAoIc29uZ0xpc3QYByABKAsyJC5jb20ubmV0dmlvcy5wcm90",
			"by5vdXRib3VuZC5Tb25nTGlzdEgAEjgKCHJvb21MaXN0GAggASgLMiQuY29t", "Lm5ldHZpb3MucHJvdG8ub3V0Ym91bmQuUm9vbUxpc3RIABI2CgdnZXRSb29t", "GAkgASgLMiMuY29tLm5ldHZpb3MucHJvdG8ub3V0Ym91bmQuR2V0Um9vbUgA", "EjwKCmNyZWF0ZVJvb20YCiABKAsyJi5jb20ubmV0dmlvcy5wcm90by5vdXRi", "b3VuZC5DcmVhdGVSb29tSAASOAoIam9pblJvb20YCyABKAsyJC5jb20ubmV0", "dmlvcy5wcm90by5vdXRib3VuZC5Kb2luUm9vbUgAEjgKCGV4aXRSb29tGAwg", "ASgLMiQuY29tLm5ldHZpb3MucHJvdG8ub3V0Ym91bmQuRXhpdFJvb21IABJK", "ChFraWNrT3V0Um9vbVBsYXllchgNIAEoCzItLmNvbS5uZXR2aW9zLnByb3Rv", "Lm91dGJvdW5kLktpY2tPdXRSb29tUGxheWVySAASOgoJc3RhcnRHYW1lGA4g", "ASgLMiUuY29tLm5ldHZpb3MucHJvdG8ub3V0Ym91bmQuU3RhcnRHYW1lSAAS",
			"QgoNcm9vbUJyb2FkY2FzdBgPIAEoCzIpLmNvbS5uZXR2aW9zLnByb3RvLm91", "dGJvdW5kLlJvb21Ccm9hZGNhc3RIABJGCg9jaGFuZ2VSb29tT3duZXIYECAB", "KAsyKy5jb20ubmV0dmlvcy5wcm90by5vdXRib3VuZC5DaGFuZ2VSb29tT3du", "ZXJIABJKChFtb2RpZnlQZXJzb25hbENmZxgRIAEoCzItLmNvbS5uZXR2aW9z", "LnByb3RvLm91dGJvdW5kLk1vZGlmeVBlcnNvbmFsQ2ZnSAASQgoNbW9kaWZ5", "Um9vbUNmZxgSIAEoCzIpLmNvbS5uZXR2aW9zLnByb3RvLm91dGJvdW5kLk1v", "ZGlmeVJvb21DZmdIABJCCg1tb2RpZnlTb25nQ2ZnGBMgASgLMikuY29tLm5l", "dHZpb3MucHJvdG8ub3V0Ym91bmQuTW9kaWZ5U29uZ0NmZ0gAEkYKD3Jvb21T", "dWJtaXRTY29yZRgUIAEoCzIrLmNvbS5uZXR2aW9zLnByb3RvLm91dGJvdW5k", "LlJvb21TdWJtaXRTY29yZUgAEjoKCWZhc3RNYXRjaBgVIAEoCzIlLmNvbS5u",
			"ZXR2aW9zLnByb3RvLm91dGJvdW5kLkZhc3RNYXRjaEgAEjoKCWF1dG9NYXRj", "aBgWIAEoCzIlLmNvbS5uZXR2aW9zLnByb3RvLm91dGJvdW5kLkF1dG9NYXRj", "aEgAEkQKDm1vZGlmeU5pY2tuYW1lGBggASgLMiouY29tLm5ldHZpb3MucHJv", "dG8ub3V0Ym91bmQuTW9kaWZ5Tmlja25hbWVIABJGCg9raWNrZWRPdXROb3Rp", "Y2UYHiABKAsyKy5jb20ubmV0dmlvcy5wcm90by5vdXRib3VuZC5LaWNrZWRP", "dXROb3RpY2VIABJKChFyb29tVXBkYXRlZE5vdGljZRgfIAEoCzItLmNvbS5u", "ZXR2aW9zLnByb3RvLm91dGJvdW5kLlJvb21VcGRhdGVkTm90aWNlSAASTgoT", "a2lja2VkT3V0Um9vbU5vdGljZRggIAEoCzIvLmNvbS5uZXR2aW9zLnByb3Rv", "Lm91dGJvdW5kLktpY2tlZE91dFJvb21Ob3RpY2VIABJGCg9zdGFydEdhbWVO", "b3RpY2UYISABKAsyKy5jb20ubmV0dmlvcy5wcm90by5vdXRib3VuZC5TdGFy",
			"dEdhbWVOb3RpY2VIABJSChVyb29tU3VibWl0U2NvcmVOb3RpY2UYIiABKAsy", "MS5jb20ubmV0dmlvcy5wcm90by5vdXRib3VuZC5Sb29tU3VibWl0U2NvcmVO", "b3RpY2VIABJOChNyb29tQnJvYWRjYXN0Tm90aWNlGCMgASgLMi8uY29tLm5l", "dHZpb3MucHJvdG8ub3V0Ym91bmQuUm9vbUJyb2FkY2FzdE5vdGljZUgAEkYK", "D2F1dG9NYXRjaE5vdGljZRgkIAEoCzIrLmNvbS5uZXR2aW9zLnByb3RvLm91", "dGJvdW5kLkF1dG9NYXRjaE5vdGljZUgAEkoKEWhlYWRwaG9uZU9uTm90aWNl", "GCUgASgLMi0uY29tLm5ldHZpb3MucHJvdG8ub3V0Ym91bmQuSGVhZHBob25l", "T25Ob3RpY2VIAEIGCgRkYXRhIhgKBFBpbmcSEAoIc2VxdWVuY2UYASABKAUi", "YAoFTG9naW4SEwoLYXBwX2NoYW5uZWwYASABKAkSDQoFdG9rZW4YAiABKAkS", "EAoIbmlja25hbWUYAyABKAkSDgoGYXZhdGFyGAQgASgJEhEKCXBsYXllcl9p",
			"ZBgFIAEoAyJwCgVSZW5ldxITCgthcHBfY2hhbm5lbBgBIAEoCRINCgV0b2tl", "bhgCIAEoCRIQCghuaWNrbmFtZRgDIAEoCRIOCgZhdmF0YXIYBCABKAkSEQoJ", "cGxheWVyX2lkGAUgASgDEg4KBmV4cGlyZRgGIAEoBSIdCgZMb2dvdXQSEwoL", "YXBwX2NoYW5uZWwYASABKAkiZAoJR2V0UGxheWVyEhMKC2FwcF9jaGFubmVs", "GAEgASgJEg0KBXRva2VuGAIgASgJEhAKCG5pY2tuYW1lGAMgASgJEg4KBmF2", "YXRhchgEIAEoCRIRCglwbGF5ZXJfaWQYBSABKAMiaQoOTW9kaWZ5Tmlja25h", "bWUSEwoLYXBwX2NoYW5uZWwYASABKAkSDQoFdG9rZW4YAiABKAkSEAoIbmlj", "a25hbWUYAyABKAkSDgoGYXZhdGFyGAQgASgJEhEKCXBsYXllcl9pZBgFIAEo", "AyKAAQoIUm9vbUxpc3QSEwoLcGFnZV9udW1iZXIYASABKAUSEQoJcGFnZV9z", "aXplGAIgASgFEgwKBHNpemUYAyABKAUSDQoFY291bnQYBCABKAUSLwoFcm9v",
			"bXMYCiADKAsyIC5jb20ubmV0dmlvcy5wcm90by5vdXRib3VuZC5Sb29tIvwB", "CgdHZXRSb29tEg8KB3Jvb21faWQYASABKAkSEgoKcm9vbV9vd25lchgCIAEo", "AxIXCg9yb29tX293bmVyX25hbWUYAyABKAkSNAoHcm9vbUNmZxgHIAEoCzIj", "LmNvbS5uZXR2aW9zLnByb3RvLm91dGJvdW5kLlJvb21DZmcSNAoHc29uZ0Nm", "ZxgIIAEoCzIjLmNvbS5uZXR2aW9zLnByb3RvLm91dGJvdW5kLlNvbmdDZmcS", "NwoHcGxheWVycxgKIAMoCzImLmNvbS5uZXR2aW9zLnByb3RvLm91dGJvdW5k", "LlJvb21QbGF5ZXISDgoGc3RhdHVzGAwgASgJIpICCgpDcmVhdGVSb29tEg8K", "B3Jvb21faWQYASABKAkSEQoJcm9vbV9uYW1lGAIgASgJEhIKCnJvb21fb3du", "ZXIYAyABKAMSFwoPcm9vbV9vd25lcl9uYW1lGAQgASgJEjQKB3Jvb21DZmcY", "ByABKAsyIy5jb20ubmV0dmlvcy5wcm90by5vdXRib3VuZC5Sb29tQ2ZnEjQK",
			"B3NvbmdDZmcYCCABKAsyIy5jb20ubmV0dmlvcy5wcm90by5vdXRib3VuZC5T", "b25nQ2ZnEjcKB3BsYXllcnMYCiADKAsyJi5jb20ubmV0dmlvcy5wcm90by5v", "dXRib3VuZC5Sb29tUGxheWVyEg4KBnN0YXR1cxgMIAEoCSKQAgoISm9pblJv", "b20SDwoHcm9vbV9pZBgBIAEoCRIRCglyb29tX25hbWUYAiABKAkSEgoKcm9v", "bV9vd25lchgDIAEoAxIXCg9yb29tX293bmVyX25hbWUYBCABKAkSNAoHcm9v", "bUNmZxgHIAEoCzIjLmNvbS5uZXR2aW9zLnByb3RvLm91dGJvdW5kLlJvb21D", "ZmcSNAoHc29uZ0NmZxgIIAEoCzIjLmNvbS5uZXR2aW9zLnByb3RvLm91dGJv", "dW5kLlNvbmdDZmcSNwoHcGxheWVycxgKIAMoCzImLmNvbS5uZXR2aW9zLnBy", "b3RvLm91dGJvdW5kLlJvb21QbGF5ZXISDgoGc3RhdHVzGAwgASgJIhsKCEV4", "aXRSb29tEg8KB3Jvb21faWQYASABKAkiYAoRS2lja091dFJvb21QbGF5ZXIS",
			"DwoHcm9vbV9pZBgBIAEoCRIPCgdzdWNjZXNzGAIgASgIEg8KB21lc3NhZ2UY", "AyABKAkSGAoQdGFyZ2V0X3BsYXllcl9pZBgEIAEoAyI+CglTdGFydEdhbWUS", "DwoHcm9vbV9pZBgBIAEoCRIPCgdzdWNjZXNzGAIgASgIEg8KB21lc3NhZ2UY", "AyABKAkiXgoPQ2hhbmdlUm9vbU93bmVyEg8KB3Jvb21faWQYASABKAkSDwoH", "c3VjY2VzcxgCIAEoCBIPCgdtZXNzYWdlGAMgASgJEhgKEHRhcmdldF9wbGF5", "ZXJfaWQYBCABKAMiRgoRTW9kaWZ5UGVyc29uYWxDZmcSDwoHcm9vbV9pZBgB", "IAEoCRIPCgdzdWNjZXNzGAIgASgIEg8KB21lc3NhZ2UYAyABKAkiQgoNTW9k", "aWZ5Um9vbUNmZxIPCgdyb29tX2lkGAEgASgJEg8KB3N1Y2Nlc3MYAiABKAgS", "DwoHbWVzc2FnZRgDIAEoCSJCCg1Nb2RpZnlTb25nQ2ZnEg8KB3Jvb21faWQY", "ASABKAkSDwoHc3VjY2VzcxgCIAEoCBIPCgdtZXNzYWdlGAMgASgJIkQKD1Jv",
			"b21TdWJtaXRTY29yZRIPCgdyb29tX2lkGAEgASgJEg8KB3N1Y2Nlc3MYAiAB", "KAgSDwoHbWVzc2FnZRgDIAEoCSI/Cg1Sb29tQnJvYWRjYXN0Eg8KB3Jvb21f", "aWQYASABKAkSDAoEdHlwZRgCIAEoCRIPCgdjb250ZW50GAMgASgJIoABCghT", "b25nTGlzdBITCgtwYWdlX251bWJlchgBIAEoBRIRCglwYWdlX3NpemUYAiAB", "KAUSDAoEc2l6ZRgDIAEoBRINCgVjb3VudBgEIAEoBRIvCgVzb25ncxgKIAMo", "CzIgLmNvbS5uZXR2aW9zLnByb3RvLm91dGJvdW5kLlNvbmcikQIKCUZhc3RN", "YXRjaBIPCgdyb29tX2lkGAEgASgJEhEKCXJvb21fbmFtZRgCIAEoCRISCgpy", "b29tX293bmVyGAMgASgDEhcKD3Jvb21fb3duZXJfbmFtZRgEIAEoCRI0Cgdy", "b29tQ2ZnGAcgASgLMiMuY29tLm5ldHZpb3MucHJvdG8ub3V0Ym91bmQuUm9v", "bUNmZxI0Cgdzb25nQ2ZnGAggASgLMiMuY29tLm5ldHZpb3MucHJvdG8ub3V0",
			"Ym91bmQuU29uZ0NmZxI3CgdwbGF5ZXJzGAogAygLMiYuY29tLm5ldHZpb3Mu", "cHJvdG8ub3V0Ym91bmQuUm9vbVBsYXllchIOCgZzdGF0dXMYDCABKAkiHAoJ", "QXV0b01hdGNoEg8KB3N1Y2Nlc3MYASABKAgiPAoPS2lja2VkT3V0Tm90aWNl", "EhgKEHRhcmdldF9wbGF5ZXJfaWQYASABKAMSDwoHbWVzc2FnZRgCIAEoCSLK", "AgoRUm9vbVVwZGF0ZWROb3RpY2USDwoHcm9vbV9pZBgBIAEoCRIRCglyb29t", "X25hbWUYAiABKAkSEgoKcm9vbV9vd25lchgDIAEoAxIXCg9yb29tX293bmVy", "X25hbWUYBCABKAkSNAoHcm9vbUNmZxgHIAEoCzIjLmNvbS5uZXR2aW9zLnBy", "b3RvLm91dGJvdW5kLlJvb21DZmcSNAoHc29uZ0NmZxgIIAEoCzIjLmNvbS5u", "ZXR2aW9zLnByb3RvLm91dGJvdW5kLlNvbmdDZmcSNwoHcGxheWVycxgKIAMo", "CzImLmNvbS5uZXR2aW9zLnByb3RvLm91dGJvdW5kLlJvb21QbGF5ZXISDgoG",
			"c3RhdHVzGAwgASgJEi8KCmV2ZW50X3R5cGUYDyABKA4yGy5jb20ubmV0dmlv", "cy5wcm90by5EYXRhVHlwZSJRChNLaWNrZWRPdXRSb29tTm90aWNlEg8KB3Jv", "b21faWQYASABKAkSGAoQdGFyZ2V0X3BsYXllcl9pZBgCIAEoAxIPCgdtZXNz", "YWdlGAMgASgJIlgKD1N0YXJ0R2FtZU5vdGljZRIPCgdyb29tX2lkGAEgASgJ", "EjQKB3NvbmdDZmcYBSABKAsyIy5jb20ubmV0dmlvcy5wcm90by5vdXRib3Vu", "ZC5Tb25nQ2ZnIp4CChVSb29tU3VibWl0U2NvcmVOb3RpY2USEwoLYXBwX2No", "YW5uZWwYASABKAkSEQoJcGxheWVyX2lkGAIgASgDEhAKCG5pY2tuYW1lGAMg", "ASgJEg4KBmF2YXRhchgEIAEoCRI0Cgdzb25nQ2ZnGAUgASgLMiMuY29tLm5l", "dHZpb3MucHJvdG8ub3V0Ym91bmQuU29uZ0NmZxINCgV2YWxpZBgGIAEoCBIN", "CgVzY29yZRgHIAEoBRIMCgRyYW5rGAggASgJEhcKD3NvbmdfZGlkX2Zpbmlz",
			"aBgJIAEoCBIPCgdtZXNzYWdlGAogASgJEh4KFnJlc3VsdF9kaXNwbGF5X3Nl", "Y29uZHMYCyABKAUSDwoHcm9vbV9pZBgPIAEoCSJTChNSb29tQnJvYWRjYXN0", "Tm90aWNlEg8KB3Jvb21faWQYASABKAkSDAoEZnJvbRgCIAEoAxIMCgR0eXBl", "GAMgASgJEg8KB2NvbnRlbnQYBCABKAkilwIKD0F1dG9NYXRjaE5vdGljZRIP", "Cgdyb29tX2lkGAEgASgJEhEKCXJvb21fbmFtZRgCIAEoCRISCgpyb29tX293", "bmVyGAMgASgDEhcKD3Jvb21fb3duZXJfbmFtZRgEIAEoCRI0Cgdyb29tQ2Zn", "GAcgASgLMiMuY29tLm5ldHZpb3MucHJvdG8ub3V0Ym91bmQuUm9vbUNmZxI0", "Cgdzb25nQ2ZnGAggASgLMiMuY29tLm5ldHZpb3MucHJvdG8ub3V0Ym91bmQu", "U29uZ0NmZxI3CgdwbGF5ZXJzGAogAygLMiYuY29tLm5ldHZpb3MucHJvdG8u", "b3V0Ym91bmQuUm9vbVBsYXllchIOCgZzdGF0dXMYDCABKAkiOQoRSGVhZHBo",
			"b25lT25Ob3RpY2USDwoHcm9vbV9pZBgBIAEoCRITCgtjY19yZXNwb25zZRgF", "IAEoCSKzAQoKUm9vbVBsYXllchITCgthcHBfY2hhbm5lbBgBIAEoCRIRCglw", "bGF5ZXJfaWQYAiABKAMSEAoIbmlja25hbWUYAyABKAkSDgoGYXZhdGFyGAQg", "ASgJEg0KBXNjb3JlGAggASgFEjwKC3BlcnNvbmFsQ2ZnGAogASgLMicuY29t", "Lm5ldHZpb3MucHJvdG8ub3V0Ym91bmQuUGVyc29uYWxDZmcSDgoGc3RhdHVz", "GAwgASgJIvkBCgRSb29tEg8KB3Jvb21faWQYASABKAkSEgoKcm9vbV9vd25l", "chgCIAEoAxIXCg9yb29tX293bmVyX25hbWUYAyABKAkSNAoHcm9vbUNmZxgH", "IAEoCzIjLmNvbS5uZXR2aW9zLnByb3RvLm91dGJvdW5kLlJvb21DZmcSNAoH", "c29uZ0NmZxgIIAEoCzIjLmNvbS5uZXR2aW9zLnByb3RvLm91dGJvdW5kLlNv", "bmdDZmcSNwoHcGxheWVycxgKIAMoCzImLmNvbS5uZXR2aW9zLnByb3RvLm91",
			"dGJvdW5kLlJvb21QbGF5ZXISDgoGc3RhdHVzGAwgASgJIjoKC1BlcnNvbmFs", "Q2ZnEhQKDGhlYWRwaG9uZV9vbhgBIAEoCBIVCg1taWNyb3Bob25lX29uGAIg", "ASgIImUKB1Jvb21DZmcSEQoJcm9vbV9uYW1lGAEgASgJEhMKC21heF9wbGF5", "ZXJzGAIgASgFEhIKCmlzX3ByaXZhdGUYAyABKAgSHgoWcmVzdWx0X2Rpc3Bs", "YXlfc2Vjb25kcxgEIAEoBSKNAQoHU29uZ0NmZxIPCgdzb25nX2lkGAEgASgJ", "EgwKBG1vZGUYAiABKAkSEgoKZGlmZmljdWx0eRgDIAEoCRIRCglzb25nX25h", "bWUYBSABKAkSFgoOc29uZ19jb3Zlcl9pbWcYBiABKAkSFQoNc29uZ19kdXJh", "dGlvbhgHIAEoBRINCgVydWxlcxgKIAEoCSKTAQoEU29uZxIPCgdzb25nX2lk", "GAEgASgJEhEKCXNvbmdfbmFtZRgCIAEoCRINCgVhbGJ1bRgDIAEoCRIOCgZz", "aW5nZXIYBCABKAkSFAoMcHVibGlzaF9kYXRlGAUgASgDEhAKCGR1cmF0aW9u",
			"GAYgASgFEhMKC2NvdmVyX2ltYWdlGAcgASgJEgsKA3NlcRgIIAEoBUIzChlj", "b20ubmV0dmlvcy50Y3AucHJvdG8ub3V0QhZCZWF0U2FiZXJPdXRib3VuZFBy", "b3RvYgZwcm90bzM="
		})), new FileDescriptor[] { BeatSaberMessageReflection.Descriptor }, new GeneratedClrTypeInfo(null, null, new GeneratedClrTypeInfo[]
		{
			new GeneratedClrTypeInfo(typeof(BeatSaberBody), BeatSaberBody.Parser, new string[]
			{
				"Type", "Ping", "Login", "Renew", "Logout", "GetPlayer", "SongList", "RoomList", "GetRoom", "CreateRoom",
				"JoinRoom", "ExitRoom", "KickOutRoomPlayer", "StartGame", "RoomBroadcast", "ChangeRoomOwner", "ModifyPersonalCfg", "ModifyRoomCfg", "ModifySongCfg", "RoomSubmitScore",
				"FastMatch", "AutoMatch", "ModifyNickname", "KickedOutNotice", "RoomUpdatedNotice", "KickedOutRoomNotice", "StartGameNotice", "RoomSubmitScoreNotice", "RoomBroadcastNotice", "AutoMatchNotice",
				"HeadphoneOnNotice"
			}, new string[] { "Data" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(Ping), Ping.Parser, new string[] { "Sequence" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(Login), Login.Parser, new string[] { "AppChannel", "Token", "Nickname", "Avatar", "PlayerId" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(Renew), Renew.Parser, new string[] { "AppChannel", "Token", "Nickname", "Avatar", "PlayerId", "Expire" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(Logout), Logout.Parser, new string[] { "AppChannel" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(GetPlayer), GetPlayer.Parser, new string[] { "AppChannel", "Token", "Nickname", "Avatar", "PlayerId" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(ModifyNickname), ModifyNickname.Parser, new string[] { "AppChannel", "Token", "Nickname", "Avatar", "PlayerId" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(RoomList), RoomList.Parser, new string[] { "PageNumber", "PageSize", "Size", "Count", "Rooms" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(GetRoom), GetRoom.Parser, new string[] { "RoomId", "RoomOwner", "RoomOwnerName", "RoomCfg", "SongCfg", "Players", "Status" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(CreateRoom), CreateRoom.Parser, new string[] { "RoomId", "RoomName", "RoomOwner", "RoomOwnerName", "RoomCfg", "SongCfg", "Players", "Status" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(JoinRoom), JoinRoom.Parser, new string[] { "RoomId", "RoomName", "RoomOwner", "RoomOwnerName", "RoomCfg", "SongCfg", "Players", "Status" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(ExitRoom), ExitRoom.Parser, new string[] { "RoomId" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(KickOutRoomPlayer), KickOutRoomPlayer.Parser, new string[] { "RoomId", "Success", "Message", "TargetPlayerId" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(StartGame), StartGame.Parser, new string[] { "RoomId", "Success", "Message" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(ChangeRoomOwner), ChangeRoomOwner.Parser, new string[] { "RoomId", "Success", "Message", "TargetPlayerId" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(ModifyPersonalCfg), ModifyPersonalCfg.Parser, new string[] { "RoomId", "Success", "Message" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(ModifyRoomCfg), ModifyRoomCfg.Parser, new string[] { "RoomId", "Success", "Message" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(ModifySongCfg), ModifySongCfg.Parser, new string[] { "RoomId", "Success", "Message" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(RoomSubmitScore), RoomSubmitScore.Parser, new string[] { "RoomId", "Success", "Message" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(RoomBroadcast), RoomBroadcast.Parser, new string[] { "RoomId", "Type", "Content" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(SongList), SongList.Parser, new string[] { "PageNumber", "PageSize", "Size", "Count", "Songs" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(FastMatch), FastMatch.Parser, new string[] { "RoomId", "RoomName", "RoomOwner", "RoomOwnerName", "RoomCfg", "SongCfg", "Players", "Status" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(AutoMatch), AutoMatch.Parser, new string[] { "Success" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(KickedOutNotice), KickedOutNotice.Parser, new string[] { "TargetPlayerId", "Message" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(RoomUpdatedNotice), RoomUpdatedNotice.Parser, new string[] { "RoomId", "RoomName", "RoomOwner", "RoomOwnerName", "RoomCfg", "SongCfg", "Players", "Status", "EventType" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(KickedOutRoomNotice), KickedOutRoomNotice.Parser, new string[] { "RoomId", "TargetPlayerId", "Message" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(StartGameNotice), StartGameNotice.Parser, new string[] { "RoomId", "SongCfg" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(RoomSubmitScoreNotice), RoomSubmitScoreNotice.Parser, new string[]
			{
				"AppChannel", "PlayerId", "Nickname", "Avatar", "SongCfg", "Valid", "Score", "Rank", "SongDidFinish", "Message",
				"ResultDisplaySeconds", "RoomId"
			}, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(RoomBroadcastNotice), RoomBroadcastNotice.Parser, new string[] { "RoomId", "From", "Type", "Content" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(AutoMatchNotice), AutoMatchNotice.Parser, new string[] { "RoomId", "RoomName", "RoomOwner", "RoomOwnerName", "RoomCfg", "SongCfg", "Players", "Status" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(HeadphoneOnNotice), HeadphoneOnNotice.Parser, new string[] { "RoomId", "CcResponse" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(RoomPlayer), RoomPlayer.Parser, new string[] { "AppChannel", "PlayerId", "Nickname", "Avatar", "Score", "PersonalCfg", "Status" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(Room), Room.Parser, new string[] { "RoomId", "RoomOwner", "RoomOwnerName", "RoomCfg", "SongCfg", "Players", "Status" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(PersonalCfg), PersonalCfg.Parser, new string[] { "HeadphoneOn", "MicrophoneOn" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(RoomCfg), RoomCfg.Parser, new string[] { "RoomName", "MaxPlayers", "IsPrivate", "ResultDisplaySeconds" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(SongCfg), SongCfg.Parser, new string[] { "SongId", "Mode", "Difficulty", "SongName", "SongCoverImg", "SongDuration", "Rules" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(Song), Song.Parser, new string[] { "SongId", "SongName", "Album", "Singer", "PublishDate", "Duration", "CoverImage", "Seq" }, null, null, null, null)
		}));
	}
}
