using System;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using Mono.Cecil.Metadata;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x02000086 RID: 134
	internal sealed class MetadataSystem
	{
		// Token: 0x06000523 RID: 1315 RVA: 0x00013B5C File Offset: 0x00011D5C
		private static void InitializePrimitives()
		{
			MetadataSystem.primitive_value_types = new Dictionary<string, Row<ElementType, bool>>(18, StringComparer.Ordinal)
			{
				{
					"Void",
					new Row<ElementType, bool>(ElementType.Void, false)
				},
				{
					"Boolean",
					new Row<ElementType, bool>(ElementType.Boolean, true)
				},
				{
					"Char",
					new Row<ElementType, bool>(ElementType.Char, true)
				},
				{
					"SByte",
					new Row<ElementType, bool>(ElementType.I1, true)
				},
				{
					"Byte",
					new Row<ElementType, bool>(ElementType.U1, true)
				},
				{
					"Int16",
					new Row<ElementType, bool>(ElementType.I2, true)
				},
				{
					"UInt16",
					new Row<ElementType, bool>(ElementType.U2, true)
				},
				{
					"Int32",
					new Row<ElementType, bool>(ElementType.I4, true)
				},
				{
					"UInt32",
					new Row<ElementType, bool>(ElementType.U4, true)
				},
				{
					"Int64",
					new Row<ElementType, bool>(ElementType.I8, true)
				},
				{
					"UInt64",
					new Row<ElementType, bool>(ElementType.U8, true)
				},
				{
					"Single",
					new Row<ElementType, bool>(ElementType.R4, true)
				},
				{
					"Double",
					new Row<ElementType, bool>(ElementType.R8, true)
				},
				{
					"String",
					new Row<ElementType, bool>(ElementType.String, false)
				},
				{
					"TypedReference",
					new Row<ElementType, bool>(ElementType.TypedByRef, false)
				},
				{
					"IntPtr",
					new Row<ElementType, bool>(ElementType.I, true)
				},
				{
					"UIntPtr",
					new Row<ElementType, bool>(ElementType.U, true)
				},
				{
					"Object",
					new Row<ElementType, bool>(ElementType.Object, false)
				}
			};
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x00013CC8 File Offset: 0x00011EC8
		public static void TryProcessPrimitiveTypeReference(TypeReference type)
		{
			if (type.Namespace != "System")
			{
				return;
			}
			IMetadataScope scope = type.scope;
			if (scope == null || scope.MetadataScopeType != MetadataScopeType.AssemblyNameReference)
			{
				return;
			}
			Row<ElementType, bool> row;
			if (!MetadataSystem.TryGetPrimitiveData(type, out row))
			{
				return;
			}
			type.etype = row.Col1;
			type.IsValueType = row.Col2;
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x00013D20 File Offset: 0x00011F20
		public static bool TryGetPrimitiveElementType(TypeDefinition type, out ElementType etype)
		{
			etype = ElementType.None;
			if (type.Namespace != "System")
			{
				return false;
			}
			Row<ElementType, bool> row;
			if (MetadataSystem.TryGetPrimitiveData(type, out row))
			{
				etype = row.Col1;
				return true;
			}
			return false;
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x00013D59 File Offset: 0x00011F59
		private static bool TryGetPrimitiveData(TypeReference type, out Row<ElementType, bool> primitive_data)
		{
			if (MetadataSystem.primitive_value_types == null)
			{
				MetadataSystem.InitializePrimitives();
			}
			return MetadataSystem.primitive_value_types.TryGetValue(type.Name, out primitive_data);
		}

		// Token: 0x06000527 RID: 1319 RVA: 0x00013D78 File Offset: 0x00011F78
		public void Clear()
		{
			if (this.NestedTypes != null)
			{
				this.NestedTypes = new Dictionary<uint, Collection<uint>>(0);
			}
			if (this.ReverseNestedTypes != null)
			{
				this.ReverseNestedTypes = new Dictionary<uint, uint>(0);
			}
			if (this.Interfaces != null)
			{
				this.Interfaces = new Dictionary<uint, Collection<Row<uint, MetadataToken>>>(0);
			}
			if (this.ClassLayouts != null)
			{
				this.ClassLayouts = new Dictionary<uint, Row<ushort, uint>>(0);
			}
			if (this.FieldLayouts != null)
			{
				this.FieldLayouts = new Dictionary<uint, uint>(0);
			}
			if (this.FieldRVAs != null)
			{
				this.FieldRVAs = new Dictionary<uint, uint>(0);
			}
			if (this.FieldMarshals != null)
			{
				this.FieldMarshals = new Dictionary<MetadataToken, uint>(0);
			}
			if (this.Constants != null)
			{
				this.Constants = new Dictionary<MetadataToken, Row<ElementType, uint>>(0);
			}
			if (this.Overrides != null)
			{
				this.Overrides = new Dictionary<uint, Collection<MetadataToken>>(0);
			}
			if (this.CustomAttributes != null)
			{
				this.CustomAttributes = new Dictionary<MetadataToken, Range[]>(0);
			}
			if (this.SecurityDeclarations != null)
			{
				this.SecurityDeclarations = new Dictionary<MetadataToken, Range[]>(0);
			}
			if (this.Events != null)
			{
				this.Events = new Dictionary<uint, Range>(0);
			}
			if (this.Properties != null)
			{
				this.Properties = new Dictionary<uint, Range>(0);
			}
			if (this.Semantics != null)
			{
				this.Semantics = new Dictionary<uint, Row<MethodSemanticsAttributes, MetadataToken>>(0);
			}
			if (this.PInvokes != null)
			{
				this.PInvokes = new Dictionary<uint, Row<PInvokeAttributes, uint, uint>>(0);
			}
			if (this.GenericParameters != null)
			{
				this.GenericParameters = new Dictionary<MetadataToken, Range[]>(0);
			}
			if (this.GenericConstraints != null)
			{
				this.GenericConstraints = new Dictionary<uint, Collection<MetadataToken>>(0);
			}
			this.Documents = Empty<Document>.Array;
			this.ImportScopes = Empty<ImportDebugInformation>.Array;
			if (this.LocalScopes != null)
			{
				this.LocalScopes = new Dictionary<uint, Collection<Row<uint, Range, Range, uint, uint, uint>>>(0);
			}
			if (this.StateMachineMethods != null)
			{
				this.StateMachineMethods = new Dictionary<uint, uint>(0);
			}
		}

		// Token: 0x06000528 RID: 1320 RVA: 0x00013F17 File Offset: 0x00012117
		public AssemblyNameReference GetAssemblyNameReference(uint rid)
		{
			if (rid < 1U || (ulong)rid > (ulong)((long)this.AssemblyReferences.Length))
			{
				return null;
			}
			return this.AssemblyReferences[(int)(rid - 1U)];
		}

		// Token: 0x06000529 RID: 1321 RVA: 0x00013F36 File Offset: 0x00012136
		public TypeDefinition GetTypeDefinition(uint rid)
		{
			if (rid < 1U || (ulong)rid > (ulong)((long)this.Types.Length))
			{
				return null;
			}
			return this.Types[(int)(rid - 1U)];
		}

		// Token: 0x0600052A RID: 1322 RVA: 0x00013F55 File Offset: 0x00012155
		public void AddTypeDefinition(TypeDefinition type)
		{
			this.Types[(int)(type.token.RID - 1U)] = type;
		}

		// Token: 0x0600052B RID: 1323 RVA: 0x00013F6C File Offset: 0x0001216C
		public TypeReference GetTypeReference(uint rid)
		{
			if (rid < 1U || (ulong)rid > (ulong)((long)this.TypeReferences.Length))
			{
				return null;
			}
			return this.TypeReferences[(int)(rid - 1U)];
		}

		// Token: 0x0600052C RID: 1324 RVA: 0x00013F8B File Offset: 0x0001218B
		public void AddTypeReference(TypeReference type)
		{
			this.TypeReferences[(int)(type.token.RID - 1U)] = type;
		}

		// Token: 0x0600052D RID: 1325 RVA: 0x00013FA2 File Offset: 0x000121A2
		public FieldDefinition GetFieldDefinition(uint rid)
		{
			if (rid < 1U || (ulong)rid > (ulong)((long)this.Fields.Length))
			{
				return null;
			}
			return this.Fields[(int)(rid - 1U)];
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x00013FC1 File Offset: 0x000121C1
		public void AddFieldDefinition(FieldDefinition field)
		{
			this.Fields[(int)(field.token.RID - 1U)] = field;
		}

		// Token: 0x0600052F RID: 1327 RVA: 0x00013FD8 File Offset: 0x000121D8
		public MethodDefinition GetMethodDefinition(uint rid)
		{
			if (rid < 1U || (ulong)rid > (ulong)((long)this.Methods.Length))
			{
				return null;
			}
			return this.Methods[(int)(rid - 1U)];
		}

		// Token: 0x06000530 RID: 1328 RVA: 0x00013FF7 File Offset: 0x000121F7
		public void AddMethodDefinition(MethodDefinition method)
		{
			this.Methods[(int)(method.token.RID - 1U)] = method;
		}

		// Token: 0x06000531 RID: 1329 RVA: 0x0001400E File Offset: 0x0001220E
		public MemberReference GetMemberReference(uint rid)
		{
			if (rid < 1U || (ulong)rid > (ulong)((long)this.MemberReferences.Length))
			{
				return null;
			}
			return this.MemberReferences[(int)(rid - 1U)];
		}

		// Token: 0x06000532 RID: 1330 RVA: 0x0001402D File Offset: 0x0001222D
		public void AddMemberReference(MemberReference member)
		{
			this.MemberReferences[(int)(member.token.RID - 1U)] = member;
		}

		// Token: 0x06000533 RID: 1331 RVA: 0x00014044 File Offset: 0x00012244
		public bool TryGetNestedTypeMapping(TypeDefinition type, out Collection<uint> mapping)
		{
			return this.NestedTypes.TryGetValue(type.token.RID, out mapping);
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x0001405D File Offset: 0x0001225D
		public void SetNestedTypeMapping(uint type_rid, Collection<uint> mapping)
		{
			this.NestedTypes[type_rid] = mapping;
		}

		// Token: 0x06000535 RID: 1333 RVA: 0x0001406C File Offset: 0x0001226C
		public void RemoveNestedTypeMapping(TypeDefinition type)
		{
			this.NestedTypes.Remove(type.token.RID);
		}

		// Token: 0x06000536 RID: 1334 RVA: 0x00014085 File Offset: 0x00012285
		public bool TryGetReverseNestedTypeMapping(TypeDefinition type, out uint declaring)
		{
			return this.ReverseNestedTypes.TryGetValue(type.token.RID, out declaring);
		}

		// Token: 0x06000537 RID: 1335 RVA: 0x0001409E File Offset: 0x0001229E
		public void SetReverseNestedTypeMapping(uint nested, uint declaring)
		{
			this.ReverseNestedTypes[nested] = declaring;
		}

		// Token: 0x06000538 RID: 1336 RVA: 0x000140AD File Offset: 0x000122AD
		public void RemoveReverseNestedTypeMapping(TypeDefinition type)
		{
			this.ReverseNestedTypes.Remove(type.token.RID);
		}

		// Token: 0x06000539 RID: 1337 RVA: 0x000140C6 File Offset: 0x000122C6
		public bool TryGetInterfaceMapping(TypeDefinition type, out Collection<Row<uint, MetadataToken>> mapping)
		{
			return this.Interfaces.TryGetValue(type.token.RID, out mapping);
		}

		// Token: 0x0600053A RID: 1338 RVA: 0x000140DF File Offset: 0x000122DF
		public void SetInterfaceMapping(uint type_rid, Collection<Row<uint, MetadataToken>> mapping)
		{
			this.Interfaces[type_rid] = mapping;
		}

		// Token: 0x0600053B RID: 1339 RVA: 0x000140EE File Offset: 0x000122EE
		public void RemoveInterfaceMapping(TypeDefinition type)
		{
			this.Interfaces.Remove(type.token.RID);
		}

		// Token: 0x0600053C RID: 1340 RVA: 0x00014107 File Offset: 0x00012307
		public void AddPropertiesRange(uint type_rid, Range range)
		{
			this.Properties.Add(type_rid, range);
		}

		// Token: 0x0600053D RID: 1341 RVA: 0x00014116 File Offset: 0x00012316
		public bool TryGetPropertiesRange(TypeDefinition type, out Range range)
		{
			return this.Properties.TryGetValue(type.token.RID, out range);
		}

		// Token: 0x0600053E RID: 1342 RVA: 0x0001412F File Offset: 0x0001232F
		public void RemovePropertiesRange(TypeDefinition type)
		{
			this.Properties.Remove(type.token.RID);
		}

		// Token: 0x0600053F RID: 1343 RVA: 0x00014148 File Offset: 0x00012348
		public void AddEventsRange(uint type_rid, Range range)
		{
			this.Events.Add(type_rid, range);
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x00014157 File Offset: 0x00012357
		public bool TryGetEventsRange(TypeDefinition type, out Range range)
		{
			return this.Events.TryGetValue(type.token.RID, out range);
		}

		// Token: 0x06000541 RID: 1345 RVA: 0x00014170 File Offset: 0x00012370
		public void RemoveEventsRange(TypeDefinition type)
		{
			this.Events.Remove(type.token.RID);
		}

		// Token: 0x06000542 RID: 1346 RVA: 0x00014189 File Offset: 0x00012389
		public bool TryGetGenericParameterRanges(IGenericParameterProvider owner, out Range[] ranges)
		{
			return this.GenericParameters.TryGetValue(owner.MetadataToken, out ranges);
		}

		// Token: 0x06000543 RID: 1347 RVA: 0x0001419D File Offset: 0x0001239D
		public void RemoveGenericParameterRange(IGenericParameterProvider owner)
		{
			this.GenericParameters.Remove(owner.MetadataToken);
		}

		// Token: 0x06000544 RID: 1348 RVA: 0x000141B1 File Offset: 0x000123B1
		public bool TryGetCustomAttributeRanges(ICustomAttributeProvider owner, out Range[] ranges)
		{
			return this.CustomAttributes.TryGetValue(owner.MetadataToken, out ranges);
		}

		// Token: 0x06000545 RID: 1349 RVA: 0x000141C5 File Offset: 0x000123C5
		public void RemoveCustomAttributeRange(ICustomAttributeProvider owner)
		{
			this.CustomAttributes.Remove(owner.MetadataToken);
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x000141D9 File Offset: 0x000123D9
		public bool TryGetSecurityDeclarationRanges(ISecurityDeclarationProvider owner, out Range[] ranges)
		{
			return this.SecurityDeclarations.TryGetValue(owner.MetadataToken, out ranges);
		}

		// Token: 0x06000547 RID: 1351 RVA: 0x000141ED File Offset: 0x000123ED
		public void RemoveSecurityDeclarationRange(ISecurityDeclarationProvider owner)
		{
			this.SecurityDeclarations.Remove(owner.MetadataToken);
		}

		// Token: 0x06000548 RID: 1352 RVA: 0x00014201 File Offset: 0x00012401
		public bool TryGetGenericConstraintMapping(GenericParameter generic_parameter, out Collection<MetadataToken> mapping)
		{
			return this.GenericConstraints.TryGetValue(generic_parameter.token.RID, out mapping);
		}

		// Token: 0x06000549 RID: 1353 RVA: 0x0001421A File Offset: 0x0001241A
		public void SetGenericConstraintMapping(uint gp_rid, Collection<MetadataToken> mapping)
		{
			this.GenericConstraints[gp_rid] = mapping;
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x00014229 File Offset: 0x00012429
		public void RemoveGenericConstraintMapping(GenericParameter generic_parameter)
		{
			this.GenericConstraints.Remove(generic_parameter.token.RID);
		}

		// Token: 0x0600054B RID: 1355 RVA: 0x00014242 File Offset: 0x00012442
		public bool TryGetOverrideMapping(MethodDefinition method, out Collection<MetadataToken> mapping)
		{
			return this.Overrides.TryGetValue(method.token.RID, out mapping);
		}

		// Token: 0x0600054C RID: 1356 RVA: 0x0001425B File Offset: 0x0001245B
		public void SetOverrideMapping(uint rid, Collection<MetadataToken> mapping)
		{
			this.Overrides[rid] = mapping;
		}

		// Token: 0x0600054D RID: 1357 RVA: 0x0001426A File Offset: 0x0001246A
		public void RemoveOverrideMapping(MethodDefinition method)
		{
			this.Overrides.Remove(method.token.RID);
		}

		// Token: 0x0600054E RID: 1358 RVA: 0x00014283 File Offset: 0x00012483
		public Document GetDocument(uint rid)
		{
			if (rid < 1U || (ulong)rid > (ulong)((long)this.Documents.Length))
			{
				return null;
			}
			return this.Documents[(int)(rid - 1U)];
		}

		// Token: 0x0600054F RID: 1359 RVA: 0x000142A4 File Offset: 0x000124A4
		public bool TryGetLocalScopes(MethodDefinition method, out Collection<Row<uint, Range, Range, uint, uint, uint>> scopes)
		{
			return this.LocalScopes.TryGetValue(method.MetadataToken.RID, out scopes);
		}

		// Token: 0x06000550 RID: 1360 RVA: 0x000142CB File Offset: 0x000124CB
		public void SetLocalScopes(uint method_rid, Collection<Row<uint, Range, Range, uint, uint, uint>> records)
		{
			this.LocalScopes[method_rid] = records;
		}

		// Token: 0x06000551 RID: 1361 RVA: 0x000142DA File Offset: 0x000124DA
		public ImportDebugInformation GetImportScope(uint rid)
		{
			if (rid < 1U || (ulong)rid > (ulong)((long)this.ImportScopes.Length))
			{
				return null;
			}
			return this.ImportScopes[(int)(rid - 1U)];
		}

		// Token: 0x06000552 RID: 1362 RVA: 0x000142FC File Offset: 0x000124FC
		public bool TryGetStateMachineKickOffMethod(MethodDefinition method, out uint rid)
		{
			return this.StateMachineMethods.TryGetValue(method.MetadataToken.RID, out rid);
		}

		// Token: 0x06000553 RID: 1363 RVA: 0x00014323 File Offset: 0x00012523
		public TypeDefinition GetFieldDeclaringType(uint field_rid)
		{
			return MetadataSystem.BinaryRangeSearch(this.Types, field_rid, true);
		}

		// Token: 0x06000554 RID: 1364 RVA: 0x00014332 File Offset: 0x00012532
		public TypeDefinition GetMethodDeclaringType(uint method_rid)
		{
			return MetadataSystem.BinaryRangeSearch(this.Types, method_rid, false);
		}

		// Token: 0x06000555 RID: 1365 RVA: 0x00014344 File Offset: 0x00012544
		private static TypeDefinition BinaryRangeSearch(TypeDefinition[] types, uint rid, bool field)
		{
			int i = 0;
			int num = types.Length - 1;
			while (i <= num)
			{
				int num2 = i + (num - i) / 2;
				TypeDefinition typeDefinition = types[num2];
				Range range = (field ? typeDefinition.fields_range : typeDefinition.methods_range);
				if (rid < range.Start)
				{
					num = num2 - 1;
				}
				else
				{
					if (rid < range.Start + range.Length)
					{
						return typeDefinition;
					}
					i = num2 + 1;
				}
			}
			return null;
		}

		// Token: 0x04000102 RID: 258
		internal AssemblyNameReference[] AssemblyReferences;

		// Token: 0x04000103 RID: 259
		internal ModuleReference[] ModuleReferences;

		// Token: 0x04000104 RID: 260
		internal TypeDefinition[] Types;

		// Token: 0x04000105 RID: 261
		internal TypeReference[] TypeReferences;

		// Token: 0x04000106 RID: 262
		internal FieldDefinition[] Fields;

		// Token: 0x04000107 RID: 263
		internal MethodDefinition[] Methods;

		// Token: 0x04000108 RID: 264
		internal MemberReference[] MemberReferences;

		// Token: 0x04000109 RID: 265
		internal Dictionary<uint, Collection<uint>> NestedTypes;

		// Token: 0x0400010A RID: 266
		internal Dictionary<uint, uint> ReverseNestedTypes;

		// Token: 0x0400010B RID: 267
		internal Dictionary<uint, Collection<Row<uint, MetadataToken>>> Interfaces;

		// Token: 0x0400010C RID: 268
		internal Dictionary<uint, Row<ushort, uint>> ClassLayouts;

		// Token: 0x0400010D RID: 269
		internal Dictionary<uint, uint> FieldLayouts;

		// Token: 0x0400010E RID: 270
		internal Dictionary<uint, uint> FieldRVAs;

		// Token: 0x0400010F RID: 271
		internal Dictionary<MetadataToken, uint> FieldMarshals;

		// Token: 0x04000110 RID: 272
		internal Dictionary<MetadataToken, Row<ElementType, uint>> Constants;

		// Token: 0x04000111 RID: 273
		internal Dictionary<uint, Collection<MetadataToken>> Overrides;

		// Token: 0x04000112 RID: 274
		internal Dictionary<MetadataToken, Range[]> CustomAttributes;

		// Token: 0x04000113 RID: 275
		internal Dictionary<MetadataToken, Range[]> SecurityDeclarations;

		// Token: 0x04000114 RID: 276
		internal Dictionary<uint, Range> Events;

		// Token: 0x04000115 RID: 277
		internal Dictionary<uint, Range> Properties;

		// Token: 0x04000116 RID: 278
		internal Dictionary<uint, Row<MethodSemanticsAttributes, MetadataToken>> Semantics;

		// Token: 0x04000117 RID: 279
		internal Dictionary<uint, Row<PInvokeAttributes, uint, uint>> PInvokes;

		// Token: 0x04000118 RID: 280
		internal Dictionary<MetadataToken, Range[]> GenericParameters;

		// Token: 0x04000119 RID: 281
		internal Dictionary<uint, Collection<MetadataToken>> GenericConstraints;

		// Token: 0x0400011A RID: 282
		internal Document[] Documents;

		// Token: 0x0400011B RID: 283
		internal Dictionary<uint, Collection<Row<uint, Range, Range, uint, uint, uint>>> LocalScopes;

		// Token: 0x0400011C RID: 284
		internal ImportDebugInformation[] ImportScopes;

		// Token: 0x0400011D RID: 285
		internal Dictionary<uint, uint> StateMachineMethods;

		// Token: 0x0400011E RID: 286
		internal Dictionary<MetadataToken, Row<Guid, uint, uint>[]> CustomDebugInformations;

		// Token: 0x0400011F RID: 287
		private static Dictionary<string, Row<ElementType, bool>> primitive_value_types;
	}
}
