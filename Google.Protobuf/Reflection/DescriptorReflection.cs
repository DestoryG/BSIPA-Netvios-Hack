using System;

namespace Google.Protobuf.Reflection
{
	// Token: 0x0200004F RID: 79
	public static class DescriptorReflection
	{
		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x0600046F RID: 1135 RVA: 0x0001163F File Offset: 0x0000F83F
		public static FileDescriptor Descriptor
		{
			get
			{
				return DescriptorReflection.descriptor;
			}
		}

		// Token: 0x04000152 RID: 338
		private static FileDescriptor descriptor = FileDescriptor.FromGeneratedCode(Convert.FromBase64String(string.Concat(new string[]
		{
			"CiBnb29nbGUvcHJvdG9idWYvZGVzY3JpcHRvci5wcm90bxIPZ29vZ2xlLnBy", "b3RvYnVmIkcKEUZpbGVEZXNjcmlwdG9yU2V0EjIKBGZpbGUYASADKAsyJC5n", "b29nbGUucHJvdG9idWYuRmlsZURlc2NyaXB0b3JQcm90byLbAwoTRmlsZURl", "c2NyaXB0b3JQcm90bxIMCgRuYW1lGAEgASgJEg8KB3BhY2thZ2UYAiABKAkS", "EgoKZGVwZW5kZW5jeRgDIAMoCRIZChFwdWJsaWNfZGVwZW5kZW5jeRgKIAMo", "BRIXCg93ZWFrX2RlcGVuZGVuY3kYCyADKAUSNgoMbWVzc2FnZV90eXBlGAQg", "AygLMiAuZ29vZ2xlLnByb3RvYnVmLkRlc2NyaXB0b3JQcm90bxI3CgllbnVt", "X3R5cGUYBSADKAsyJC5nb29nbGUucHJvdG9idWYuRW51bURlc2NyaXB0b3JQ", "cm90bxI4CgdzZXJ2aWNlGAYgAygLMicuZ29vZ2xlLnByb3RvYnVmLlNlcnZp", "Y2VEZXNjcmlwdG9yUHJvdG8SOAoJZXh0ZW5zaW9uGAcgAygLMiUuZ29vZ2xl",
			"LnByb3RvYnVmLkZpZWxkRGVzY3JpcHRvclByb3RvEi0KB29wdGlvbnMYCCAB", "KAsyHC5nb29nbGUucHJvdG9idWYuRmlsZU9wdGlvbnMSOQoQc291cmNlX2Nv", "ZGVfaW5mbxgJIAEoCzIfLmdvb2dsZS5wcm90b2J1Zi5Tb3VyY2VDb2RlSW5m", "bxIOCgZzeW50YXgYDCABKAkiqQUKD0Rlc2NyaXB0b3JQcm90bxIMCgRuYW1l", "GAEgASgJEjQKBWZpZWxkGAIgAygLMiUuZ29vZ2xlLnByb3RvYnVmLkZpZWxk", "RGVzY3JpcHRvclByb3RvEjgKCWV4dGVuc2lvbhgGIAMoCzIlLmdvb2dsZS5w", "cm90b2J1Zi5GaWVsZERlc2NyaXB0b3JQcm90bxI1CgtuZXN0ZWRfdHlwZRgD", "IAMoCzIgLmdvb2dsZS5wcm90b2J1Zi5EZXNjcmlwdG9yUHJvdG8SNwoJZW51", "bV90eXBlGAQgAygLMiQuZ29vZ2xlLnByb3RvYnVmLkVudW1EZXNjcmlwdG9y", "UHJvdG8SSAoPZXh0ZW5zaW9uX3JhbmdlGAUgAygLMi8uZ29vZ2xlLnByb3Rv",
			"YnVmLkRlc2NyaXB0b3JQcm90by5FeHRlbnNpb25SYW5nZRI5CgpvbmVvZl9k", "ZWNsGAggAygLMiUuZ29vZ2xlLnByb3RvYnVmLk9uZW9mRGVzY3JpcHRvclBy", "b3RvEjAKB29wdGlvbnMYByABKAsyHy5nb29nbGUucHJvdG9idWYuTWVzc2Fn", "ZU9wdGlvbnMSRgoOcmVzZXJ2ZWRfcmFuZ2UYCSADKAsyLi5nb29nbGUucHJv", "dG9idWYuRGVzY3JpcHRvclByb3RvLlJlc2VydmVkUmFuZ2USFQoNcmVzZXJ2", "ZWRfbmFtZRgKIAMoCRplCg5FeHRlbnNpb25SYW5nZRINCgVzdGFydBgBIAEo", "BRILCgNlbmQYAiABKAUSNwoHb3B0aW9ucxgDIAEoCzImLmdvb2dsZS5wcm90", "b2J1Zi5FeHRlbnNpb25SYW5nZU9wdGlvbnMaKwoNUmVzZXJ2ZWRSYW5nZRIN", "CgVzdGFydBgBIAEoBRILCgNlbmQYAiABKAUiZwoVRXh0ZW5zaW9uUmFuZ2VP", "cHRpb25zEkMKFHVuaW50ZXJwcmV0ZWRfb3B0aW9uGOcHIAMoCzIkLmdvb2ds",
			"ZS5wcm90b2J1Zi5VbmludGVycHJldGVkT3B0aW9uKgkI6AcQgICAgAIi1QUK", "FEZpZWxkRGVzY3JpcHRvclByb3RvEgwKBG5hbWUYASABKAkSDgoGbnVtYmVy", "GAMgASgFEjoKBWxhYmVsGAQgASgOMisuZ29vZ2xlLnByb3RvYnVmLkZpZWxk", "RGVzY3JpcHRvclByb3RvLkxhYmVsEjgKBHR5cGUYBSABKA4yKi5nb29nbGUu", "cHJvdG9idWYuRmllbGREZXNjcmlwdG9yUHJvdG8uVHlwZRIRCgl0eXBlX25h", "bWUYBiABKAkSEAoIZXh0ZW5kZWUYAiABKAkSFQoNZGVmYXVsdF92YWx1ZRgH", "IAEoCRITCgtvbmVvZl9pbmRleBgJIAEoBRIRCglqc29uX25hbWUYCiABKAkS", "LgoHb3B0aW9ucxgIIAEoCzIdLmdvb2dsZS5wcm90b2J1Zi5GaWVsZE9wdGlv", "bnMSFwoPcHJvdG8zX29wdGlvbmFsGBEgASgIIrYCCgRUeXBlEg8KC1RZUEVf", "RE9VQkxFEAESDgoKVFlQRV9GTE9BVBACEg4KClRZUEVfSU5UNjQQAxIPCgtU",
			"WVBFX1VJTlQ2NBAEEg4KClRZUEVfSU5UMzIQBRIQCgxUWVBFX0ZJWEVENjQQ", "BhIQCgxUWVBFX0ZJWEVEMzIQBxINCglUWVBFX0JPT0wQCBIPCgtUWVBFX1NU", "UklORxAJEg4KClRZUEVfR1JPVVAQChIQCgxUWVBFX01FU1NBR0UQCxIOCgpU", "WVBFX0JZVEVTEAwSDwoLVFlQRV9VSU5UMzIQDRINCglUWVBFX0VOVU0QDhIR", "Cg1UWVBFX1NGSVhFRDMyEA8SEQoNVFlQRV9TRklYRUQ2NBAQEg8KC1RZUEVf", "U0lOVDMyEBESDwoLVFlQRV9TSU5UNjQQEiJDCgVMYWJlbBISCg5MQUJFTF9P", "UFRJT05BTBABEhIKDkxBQkVMX1JFUVVJUkVEEAISEgoOTEFCRUxfUkVQRUFU", "RUQQAyJUChRPbmVvZkRlc2NyaXB0b3JQcm90bxIMCgRuYW1lGAEgASgJEi4K", "B29wdGlvbnMYAiABKAsyHS5nb29nbGUucHJvdG9idWYuT25lb2ZPcHRpb25z", "IqQCChNFbnVtRGVzY3JpcHRvclByb3RvEgwKBG5hbWUYASABKAkSOAoFdmFs",
			"dWUYAiADKAsyKS5nb29nbGUucHJvdG9idWYuRW51bVZhbHVlRGVzY3JpcHRv", "clByb3RvEi0KB29wdGlvbnMYAyABKAsyHC5nb29nbGUucHJvdG9idWYuRW51", "bU9wdGlvbnMSTgoOcmVzZXJ2ZWRfcmFuZ2UYBCADKAsyNi5nb29nbGUucHJv", "dG9idWYuRW51bURlc2NyaXB0b3JQcm90by5FbnVtUmVzZXJ2ZWRSYW5nZRIV", "Cg1yZXNlcnZlZF9uYW1lGAUgAygJGi8KEUVudW1SZXNlcnZlZFJhbmdlEg0K", "BXN0YXJ0GAEgASgFEgsKA2VuZBgCIAEoBSJsChhFbnVtVmFsdWVEZXNjcmlw", "dG9yUHJvdG8SDAoEbmFtZRgBIAEoCRIOCgZudW1iZXIYAiABKAUSMgoHb3B0", "aW9ucxgDIAEoCzIhLmdvb2dsZS5wcm90b2J1Zi5FbnVtVmFsdWVPcHRpb25z", "IpABChZTZXJ2aWNlRGVzY3JpcHRvclByb3RvEgwKBG5hbWUYASABKAkSNgoG", "bWV0aG9kGAIgAygLMiYuZ29vZ2xlLnByb3RvYnVmLk1ldGhvZERlc2NyaXB0",
			"b3JQcm90bxIwCgdvcHRpb25zGAMgASgLMh8uZ29vZ2xlLnByb3RvYnVmLlNl", "cnZpY2VPcHRpb25zIsEBChVNZXRob2REZXNjcmlwdG9yUHJvdG8SDAoEbmFt", "ZRgBIAEoCRISCgppbnB1dF90eXBlGAIgASgJEhMKC291dHB1dF90eXBlGAMg", "ASgJEi8KB29wdGlvbnMYBCABKAsyHi5nb29nbGUucHJvdG9idWYuTWV0aG9k", "T3B0aW9ucxIfChBjbGllbnRfc3RyZWFtaW5nGAUgASgIOgVmYWxzZRIfChBz", "ZXJ2ZXJfc3RyZWFtaW5nGAYgASgIOgVmYWxzZSKlBgoLRmlsZU9wdGlvbnMS", "FAoMamF2YV9wYWNrYWdlGAEgASgJEhwKFGphdmFfb3V0ZXJfY2xhc3NuYW1l", "GAggASgJEiIKE2phdmFfbXVsdGlwbGVfZmlsZXMYCiABKAg6BWZhbHNlEikK", "HWphdmFfZ2VuZXJhdGVfZXF1YWxzX2FuZF9oYXNoGBQgASgIQgIYARIlChZq", "YXZhX3N0cmluZ19jaGVja191dGY4GBsgASgIOgVmYWxzZRJGCgxvcHRpbWl6",
			"ZV9mb3IYCSABKA4yKS5nb29nbGUucHJvdG9idWYuRmlsZU9wdGlvbnMuT3B0", "aW1pemVNb2RlOgVTUEVFRBISCgpnb19wYWNrYWdlGAsgASgJEiIKE2NjX2dl", "bmVyaWNfc2VydmljZXMYECABKAg6BWZhbHNlEiQKFWphdmFfZ2VuZXJpY19z", "ZXJ2aWNlcxgRIAEoCDoFZmFsc2USIgoTcHlfZ2VuZXJpY19zZXJ2aWNlcxgS", "IAEoCDoFZmFsc2USIwoUcGhwX2dlbmVyaWNfc2VydmljZXMYKiABKAg6BWZh", "bHNlEhkKCmRlcHJlY2F0ZWQYFyABKAg6BWZhbHNlEh4KEGNjX2VuYWJsZV9h", "cmVuYXMYHyABKAg6BHRydWUSGQoRb2JqY19jbGFzc19wcmVmaXgYJCABKAkS", "GAoQY3NoYXJwX25hbWVzcGFjZRglIAEoCRIUCgxzd2lmdF9wcmVmaXgYJyAB", "KAkSGAoQcGhwX2NsYXNzX3ByZWZpeBgoIAEoCRIVCg1waHBfbmFtZXNwYWNl", "GCkgASgJEh4KFnBocF9tZXRhZGF0YV9uYW1lc3BhY2UYLCABKAkSFAoMcnVi",
			"eV9wYWNrYWdlGC0gASgJEkMKFHVuaW50ZXJwcmV0ZWRfb3B0aW9uGOcHIAMo", "CzIkLmdvb2dsZS5wcm90b2J1Zi5VbmludGVycHJldGVkT3B0aW9uIjoKDE9w", "dGltaXplTW9kZRIJCgVTUEVFRBABEg0KCUNPREVfU0laRRACEhAKDExJVEVf", "UlVOVElNRRADKgkI6AcQgICAgAJKBAgmECci8gEKDk1lc3NhZ2VPcHRpb25z", "EiYKF21lc3NhZ2Vfc2V0X3dpcmVfZm9ybWF0GAEgASgIOgVmYWxzZRIuCh9u", "b19zdGFuZGFyZF9kZXNjcmlwdG9yX2FjY2Vzc29yGAIgASgIOgVmYWxzZRIZ", "CgpkZXByZWNhdGVkGAMgASgIOgVmYWxzZRIRCgltYXBfZW50cnkYByABKAgS", "QwoUdW5pbnRlcnByZXRlZF9vcHRpb24Y5wcgAygLMiQuZ29vZ2xlLnByb3Rv", "YnVmLlVuaW50ZXJwcmV0ZWRPcHRpb24qCQjoBxCAgICAAkoECAgQCUoECAkQ", "CiKeAwoMRmllbGRPcHRpb25zEjoKBWN0eXBlGAEgASgOMiMuZ29vZ2xlLnBy",
			"b3RvYnVmLkZpZWxkT3B0aW9ucy5DVHlwZToGU1RSSU5HEg4KBnBhY2tlZBgC", "IAEoCBI/CgZqc3R5cGUYBiABKA4yJC5nb29nbGUucHJvdG9idWYuRmllbGRP", "cHRpb25zLkpTVHlwZToJSlNfTk9STUFMEhMKBGxhenkYBSABKAg6BWZhbHNl", "EhkKCmRlcHJlY2F0ZWQYAyABKAg6BWZhbHNlEhMKBHdlYWsYCiABKAg6BWZh", "bHNlEkMKFHVuaW50ZXJwcmV0ZWRfb3B0aW9uGOcHIAMoCzIkLmdvb2dsZS5w", "cm90b2J1Zi5VbmludGVycHJldGVkT3B0aW9uIi8KBUNUeXBlEgoKBlNUUklO", "RxAAEggKBENPUkQQARIQCgxTVFJJTkdfUElFQ0UQAiI1CgZKU1R5cGUSDQoJ", "SlNfTk9STUFMEAASDQoJSlNfU1RSSU5HEAESDQoJSlNfTlVNQkVSEAIqCQjo", "BxCAgICAAkoECAQQBSJeCgxPbmVvZk9wdGlvbnMSQwoUdW5pbnRlcnByZXRl", "ZF9vcHRpb24Y5wcgAygLMiQuZ29vZ2xlLnByb3RvYnVmLlVuaW50ZXJwcmV0",
			"ZWRPcHRpb24qCQjoBxCAgICAAiKTAQoLRW51bU9wdGlvbnMSEwoLYWxsb3df", "YWxpYXMYAiABKAgSGQoKZGVwcmVjYXRlZBgDIAEoCDoFZmFsc2USQwoUdW5p", "bnRlcnByZXRlZF9vcHRpb24Y5wcgAygLMiQuZ29vZ2xlLnByb3RvYnVmLlVu", "aW50ZXJwcmV0ZWRPcHRpb24qCQjoBxCAgICAAkoECAUQBiJ9ChBFbnVtVmFs", "dWVPcHRpb25zEhkKCmRlcHJlY2F0ZWQYASABKAg6BWZhbHNlEkMKFHVuaW50", "ZXJwcmV0ZWRfb3B0aW9uGOcHIAMoCzIkLmdvb2dsZS5wcm90b2J1Zi5Vbmlu", "dGVycHJldGVkT3B0aW9uKgkI6AcQgICAgAIiewoOU2VydmljZU9wdGlvbnMS", "GQoKZGVwcmVjYXRlZBghIAEoCDoFZmFsc2USQwoUdW5pbnRlcnByZXRlZF9v", "cHRpb24Y5wcgAygLMiQuZ29vZ2xlLnByb3RvYnVmLlVuaW50ZXJwcmV0ZWRP", "cHRpb24qCQjoBxCAgICAAiKtAgoNTWV0aG9kT3B0aW9ucxIZCgpkZXByZWNh",
			"dGVkGCEgASgIOgVmYWxzZRJfChFpZGVtcG90ZW5jeV9sZXZlbBgiIAEoDjIv", "Lmdvb2dsZS5wcm90b2J1Zi5NZXRob2RPcHRpb25zLklkZW1wb3RlbmN5TGV2", "ZWw6E0lERU1QT1RFTkNZX1VOS05PV04SQwoUdW5pbnRlcnByZXRlZF9vcHRp", "b24Y5wcgAygLMiQuZ29vZ2xlLnByb3RvYnVmLlVuaW50ZXJwcmV0ZWRPcHRp", "b24iUAoQSWRlbXBvdGVuY3lMZXZlbBIXChNJREVNUE9URU5DWV9VTktOT1dO", "EAASEwoPTk9fU0lERV9FRkZFQ1RTEAESDgoKSURFTVBPVEVOVBACKgkI6AcQ", "gICAgAIingIKE1VuaW50ZXJwcmV0ZWRPcHRpb24SOwoEbmFtZRgCIAMoCzIt", "Lmdvb2dsZS5wcm90b2J1Zi5VbmludGVycHJldGVkT3B0aW9uLk5hbWVQYXJ0", "EhgKEGlkZW50aWZpZXJfdmFsdWUYAyABKAkSGgoScG9zaXRpdmVfaW50X3Zh", "bHVlGAQgASgEEhoKEm5lZ2F0aXZlX2ludF92YWx1ZRgFIAEoAxIUCgxkb3Vi",
			"bGVfdmFsdWUYBiABKAESFAoMc3RyaW5nX3ZhbHVlGAcgASgMEhcKD2FnZ3Jl", "Z2F0ZV92YWx1ZRgIIAEoCRozCghOYW1lUGFydBIRCgluYW1lX3BhcnQYASAC", "KAkSFAoMaXNfZXh0ZW5zaW9uGAIgAigIItUBCg5Tb3VyY2VDb2RlSW5mbxI6", "Cghsb2NhdGlvbhgBIAMoCzIoLmdvb2dsZS5wcm90b2J1Zi5Tb3VyY2VDb2Rl", "SW5mby5Mb2NhdGlvbhqGAQoITG9jYXRpb24SEAoEcGF0aBgBIAMoBUICEAES", "EAoEc3BhbhgCIAMoBUICEAESGAoQbGVhZGluZ19jb21tZW50cxgDIAEoCRIZ", "ChF0cmFpbGluZ19jb21tZW50cxgEIAEoCRIhChlsZWFkaW5nX2RldGFjaGVk", "X2NvbW1lbnRzGAYgAygJIqcBChFHZW5lcmF0ZWRDb2RlSW5mbxJBCgphbm5v", "dGF0aW9uGAEgAygLMi0uZ29vZ2xlLnByb3RvYnVmLkdlbmVyYXRlZENvZGVJ", "bmZvLkFubm90YXRpb24aTwoKQW5ub3RhdGlvbhIQCgRwYXRoGAEgAygFQgIQ",
			"ARITCgtzb3VyY2VfZmlsZRgCIAEoCRINCgViZWdpbhgDIAEoBRILCgNlbmQY", "BCABKAVCjwEKE2NvbS5nb29nbGUucHJvdG9idWZCEERlc2NyaXB0b3JQcm90", "b3NIAVo+Z2l0aHViLmNvbS9nb2xhbmcvcHJvdG9idWYvcHJvdG9jLWdlbi1n", "by9kZXNjcmlwdG9yO2Rlc2NyaXB0b3L4AQGiAgNHUEKqAhpHb29nbGUuUHJv", "dG9idWYuUmVmbGVjdGlvbg=="
		})), new FileDescriptor[0], new GeneratedClrTypeInfo(null, null, new GeneratedClrTypeInfo[]
		{
			new GeneratedClrTypeInfo(typeof(FileDescriptorSet), FileDescriptorSet.Parser, new string[] { "File" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(FileDescriptorProto), FileDescriptorProto.Parser, new string[]
			{
				"Name", "Package", "Dependency", "PublicDependency", "WeakDependency", "MessageType", "EnumType", "Service", "Extension", "Options",
				"SourceCodeInfo", "Syntax"
			}, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(DescriptorProto), DescriptorProto.Parser, new string[] { "Name", "Field", "Extension", "NestedType", "EnumType", "ExtensionRange", "OneofDecl", "Options", "ReservedRange", "ReservedName" }, null, null, null, new GeneratedClrTypeInfo[]
			{
				new GeneratedClrTypeInfo(typeof(DescriptorProto.Types.ExtensionRange), DescriptorProto.Types.ExtensionRange.Parser, new string[] { "Start", "End", "Options" }, null, null, null, null),
				new GeneratedClrTypeInfo(typeof(DescriptorProto.Types.ReservedRange), DescriptorProto.Types.ReservedRange.Parser, new string[] { "Start", "End" }, null, null, null, null)
			}),
			new GeneratedClrTypeInfo(typeof(ExtensionRangeOptions), ExtensionRangeOptions.Parser, new string[] { "UninterpretedOption" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(FieldDescriptorProto), FieldDescriptorProto.Parser, new string[]
			{
				"Name", "Number", "Label", "Type", "TypeName", "Extendee", "DefaultValue", "OneofIndex", "JsonName", "Options",
				"Proto3Optional"
			}, null, new Type[]
			{
				typeof(FieldDescriptorProto.Types.Type),
				typeof(FieldDescriptorProto.Types.Label)
			}, null, null),
			new GeneratedClrTypeInfo(typeof(OneofDescriptorProto), OneofDescriptorProto.Parser, new string[] { "Name", "Options" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(EnumDescriptorProto), EnumDescriptorProto.Parser, new string[] { "Name", "Value", "Options", "ReservedRange", "ReservedName" }, null, null, null, new GeneratedClrTypeInfo[]
			{
				new GeneratedClrTypeInfo(typeof(EnumDescriptorProto.Types.EnumReservedRange), EnumDescriptorProto.Types.EnumReservedRange.Parser, new string[] { "Start", "End" }, null, null, null, null)
			}),
			new GeneratedClrTypeInfo(typeof(EnumValueDescriptorProto), EnumValueDescriptorProto.Parser, new string[] { "Name", "Number", "Options" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(ServiceDescriptorProto), ServiceDescriptorProto.Parser, new string[] { "Name", "Method", "Options" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(MethodDescriptorProto), MethodDescriptorProto.Parser, new string[] { "Name", "InputType", "OutputType", "Options", "ClientStreaming", "ServerStreaming" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(FileOptions), FileOptions.Parser, new string[]
			{
				"JavaPackage", "JavaOuterClassname", "JavaMultipleFiles", "JavaGenerateEqualsAndHash", "JavaStringCheckUtf8", "OptimizeFor", "GoPackage", "CcGenericServices", "JavaGenericServices", "PyGenericServices",
				"PhpGenericServices", "Deprecated", "CcEnableArenas", "ObjcClassPrefix", "CsharpNamespace", "SwiftPrefix", "PhpClassPrefix", "PhpNamespace", "PhpMetadataNamespace", "RubyPackage",
				"UninterpretedOption"
			}, null, new Type[] { typeof(FileOptions.Types.OptimizeMode) }, null, null),
			new GeneratedClrTypeInfo(typeof(MessageOptions), MessageOptions.Parser, new string[] { "MessageSetWireFormat", "NoStandardDescriptorAccessor", "Deprecated", "MapEntry", "UninterpretedOption" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(FieldOptions), FieldOptions.Parser, new string[] { "Ctype", "Packed", "Jstype", "Lazy", "Deprecated", "Weak", "UninterpretedOption" }, null, new Type[]
			{
				typeof(FieldOptions.Types.CType),
				typeof(FieldOptions.Types.JSType)
			}, null, null),
			new GeneratedClrTypeInfo(typeof(OneofOptions), OneofOptions.Parser, new string[] { "UninterpretedOption" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(EnumOptions), EnumOptions.Parser, new string[] { "AllowAlias", "Deprecated", "UninterpretedOption" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(EnumValueOptions), EnumValueOptions.Parser, new string[] { "Deprecated", "UninterpretedOption" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(ServiceOptions), ServiceOptions.Parser, new string[] { "Deprecated", "UninterpretedOption" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(MethodOptions), MethodOptions.Parser, new string[] { "Deprecated", "IdempotencyLevel", "UninterpretedOption" }, null, new Type[] { typeof(MethodOptions.Types.IdempotencyLevel) }, null, null),
			new GeneratedClrTypeInfo(typeof(UninterpretedOption), UninterpretedOption.Parser, new string[] { "Name", "IdentifierValue", "PositiveIntValue", "NegativeIntValue", "DoubleValue", "StringValue", "AggregateValue" }, null, null, null, new GeneratedClrTypeInfo[]
			{
				new GeneratedClrTypeInfo(typeof(UninterpretedOption.Types.NamePart), UninterpretedOption.Types.NamePart.Parser, new string[] { "NamePart_", "IsExtension" }, null, null, null, null)
			}),
			new GeneratedClrTypeInfo(typeof(SourceCodeInfo), SourceCodeInfo.Parser, new string[] { "Location" }, null, null, null, new GeneratedClrTypeInfo[]
			{
				new GeneratedClrTypeInfo(typeof(SourceCodeInfo.Types.Location), SourceCodeInfo.Types.Location.Parser, new string[] { "Path", "Span", "LeadingComments", "TrailingComments", "LeadingDetachedComments" }, null, null, null, null)
			}),
			new GeneratedClrTypeInfo(typeof(GeneratedCodeInfo), GeneratedCodeInfo.Parser, new string[] { "Annotation" }, null, null, null, new GeneratedClrTypeInfo[]
			{
				new GeneratedClrTypeInfo(typeof(GeneratedCodeInfo.Types.Annotation), GeneratedCodeInfo.Types.Annotation.Parser, new string[] { "Path", "SourceFile", "Begin", "End" }, null, null, null, null)
			})
		}));
	}
}
