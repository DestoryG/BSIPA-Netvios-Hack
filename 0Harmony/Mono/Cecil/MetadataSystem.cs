using System;
using System.Collections.Generic;
using System.Threading;
using Mono.Cecil.Cil;
using Mono.Cecil.Metadata;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x0200013D RID: 317
	internal sealed class MetadataSystem
	{
		// Token: 0x060008B5 RID: 2229 RVA: 0x00022388 File Offset: 0x00020588
		private static void InitializePrimitives()
		{
			Dictionary<string, Row<ElementType, bool>> dictionary = new Dictionary<string, Row<ElementType, bool>>(18, StringComparer.Ordinal)
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
			Interlocked.CompareExchange<Dictionary<string, Row<ElementType, bool>>>(ref MetadataSystem.primitive_value_types, dictionary, null);
		}

		// Token: 0x060008B6 RID: 2230 RVA: 0x00022500 File Offset: 0x00020700
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

		// Token: 0x060008B7 RID: 2231 RVA: 0x00022558 File Offset: 0x00020758
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

		// Token: 0x060008B8 RID: 2232 RVA: 0x00022591 File Offset: 0x00020791
		private static bool TryGetPrimitiveData(TypeReference type, out Row<ElementType, bool> primitive_data)
		{
			if (MetadataSystem.primitive_value_types == null)
			{
				MetadataSystem.InitializePrimitives();
			}
			return MetadataSystem.primitive_value_types.TryGetValue(type.Name, out primitive_data);
		}

		// Token: 0x060008B9 RID: 2233 RVA: 0x000225B0 File Offset: 0x000207B0
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
				this.GenericConstraints = new Dictionary<uint, Collection<Row<uint, MetadataToken>>>(0);
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

		// Token: 0x060008BA RID: 2234 RVA: 0x0002274F File Offset: 0x0002094F
		public AssemblyNameReference GetAssemblyNameReference(uint rid)
		{
			if (rid < 1U || (ulong)rid > (ulong)((long)this.AssemblyReferences.Length))
			{
				return null;
			}
			return this.AssemblyReferences[(int)(rid - 1U)];
		}

		// Token: 0x060008BB RID: 2235 RVA: 0x0002276E File Offset: 0x0002096E
		public TypeDefinition GetTypeDefinition(uint rid)
		{
			if (rid < 1U || (ulong)rid > (ulong)((long)this.Types.Length))
			{
				return null;
			}
			return this.Types[(int)(rid - 1U)];
		}

		// Token: 0x060008BC RID: 2236 RVA: 0x0002278D File Offset: 0x0002098D
		public void AddTypeDefinition(TypeDefinition type)
		{
			this.Types[(int)(type.token.RID - 1U)] = type;
		}

		// Token: 0x060008BD RID: 2237 RVA: 0x000227A4 File Offset: 0x000209A4
		public TypeReference GetTypeReference(uint rid)
		{
			if (rid < 1U || (ulong)rid > (ulong)((long)this.TypeReferences.Length))
			{
				return null;
			}
			return this.TypeReferences[(int)(rid - 1U)];
		}

		// Token: 0x060008BE RID: 2238 RVA: 0x000227C3 File Offset: 0x000209C3
		public void AddTypeReference(TypeReference type)
		{
			this.TypeReferences[(int)(type.token.RID - 1U)] = type;
		}

		// Token: 0x060008BF RID: 2239 RVA: 0x000227DA File Offset: 0x000209DA
		public FieldDefinition GetFieldDefinition(uint rid)
		{
			if (rid < 1U || (ulong)rid > (ulong)((long)this.Fields.Length))
			{
				return null;
			}
			return this.Fields[(int)(rid - 1U)];
		}

		// Token: 0x060008C0 RID: 2240 RVA: 0x000227F9 File Offset: 0x000209F9
		public void AddFieldDefinition(FieldDefinition field)
		{
			this.Fields[(int)(field.token.RID - 1U)] = field;
		}

		// Token: 0x060008C1 RID: 2241 RVA: 0x00022810 File Offset: 0x00020A10
		public MethodDefinition GetMethodDefinition(uint rid)
		{
			if (rid < 1U || (ulong)rid > (ulong)((long)this.Methods.Length))
			{
				return null;
			}
			return this.Methods[(int)(rid - 1U)];
		}

		// Token: 0x060008C2 RID: 2242 RVA: 0x0002282F File Offset: 0x00020A2F
		public void AddMethodDefinition(MethodDefinition method)
		{
			this.Methods[(int)(method.token.RID - 1U)] = method;
		}

		// Token: 0x060008C3 RID: 2243 RVA: 0x00022846 File Offset: 0x00020A46
		public MemberReference GetMemberReference(uint rid)
		{
			if (rid < 1U || (ulong)rid > (ulong)((long)this.MemberReferences.Length))
			{
				return null;
			}
			return this.MemberReferences[(int)(rid - 1U)];
		}

		// Token: 0x060008C4 RID: 2244 RVA: 0x00022865 File Offset: 0x00020A65
		public void AddMemberReference(MemberReference member)
		{
			this.MemberReferences[(int)(member.token.RID - 1U)] = member;
		}

		// Token: 0x060008C5 RID: 2245 RVA: 0x0002287C File Offset: 0x00020A7C
		public bool TryGetNestedTypeMapping(TypeDefinition type, out Collection<uint> mapping)
		{
			return this.NestedTypes.TryGetValue(type.token.RID, out mapping);
		}

		// Token: 0x060008C6 RID: 2246 RVA: 0x00022895 File Offset: 0x00020A95
		public void SetNestedTypeMapping(uint type_rid, Collection<uint> mapping)
		{
			this.NestedTypes[type_rid] = mapping;
		}

		// Token: 0x060008C7 RID: 2247 RVA: 0x000228A4 File Offset: 0x00020AA4
		public void RemoveNestedTypeMapping(TypeDefinition type)
		{
			this.NestedTypes.Remove(type.token.RID);
		}

		// Token: 0x060008C8 RID: 2248 RVA: 0x000228BD File Offset: 0x00020ABD
		public bool TryGetReverseNestedTypeMapping(TypeDefinition type, out uint declaring)
		{
			return this.ReverseNestedTypes.TryGetValue(type.token.RID, out declaring);
		}

		// Token: 0x060008C9 RID: 2249 RVA: 0x000228D6 File Offset: 0x00020AD6
		public void SetReverseNestedTypeMapping(uint nested, uint declaring)
		{
			this.ReverseNestedTypes[nested] = declaring;
		}

		// Token: 0x060008CA RID: 2250 RVA: 0x000228E5 File Offset: 0x00020AE5
		public void RemoveReverseNestedTypeMapping(TypeDefinition type)
		{
			this.ReverseNestedTypes.Remove(type.token.RID);
		}

		// Token: 0x060008CB RID: 2251 RVA: 0x000228FE File Offset: 0x00020AFE
		public bool TryGetInterfaceMapping(TypeDefinition type, out Collection<Row<uint, MetadataToken>> mapping)
		{
			return this.Interfaces.TryGetValue(type.token.RID, out mapping);
		}

		// Token: 0x060008CC RID: 2252 RVA: 0x00022917 File Offset: 0x00020B17
		public void SetInterfaceMapping(uint type_rid, Collection<Row<uint, MetadataToken>> mapping)
		{
			this.Interfaces[type_rid] = mapping;
		}

		// Token: 0x060008CD RID: 2253 RVA: 0x00022926 File Offset: 0x00020B26
		public void RemoveInterfaceMapping(TypeDefinition type)
		{
			this.Interfaces.Remove(type.token.RID);
		}

		// Token: 0x060008CE RID: 2254 RVA: 0x0002293F File Offset: 0x00020B3F
		public void AddPropertiesRange(uint type_rid, Range range)
		{
			this.Properties.Add(type_rid, range);
		}

		// Token: 0x060008CF RID: 2255 RVA: 0x0002294E File Offset: 0x00020B4E
		public bool TryGetPropertiesRange(TypeDefinition type, out Range range)
		{
			return this.Properties.TryGetValue(type.token.RID, out range);
		}

		// Token: 0x060008D0 RID: 2256 RVA: 0x00022967 File Offset: 0x00020B67
		public void RemovePropertiesRange(TypeDefinition type)
		{
			this.Properties.Remove(type.token.RID);
		}

		// Token: 0x060008D1 RID: 2257 RVA: 0x00022980 File Offset: 0x00020B80
		public void AddEventsRange(uint type_rid, Range range)
		{
			this.Events.Add(type_rid, range);
		}

		// Token: 0x060008D2 RID: 2258 RVA: 0x0002298F File Offset: 0x00020B8F
		public bool TryGetEventsRange(TypeDefinition type, out Range range)
		{
			return this.Events.TryGetValue(type.token.RID, out range);
		}

		// Token: 0x060008D3 RID: 2259 RVA: 0x000229A8 File Offset: 0x00020BA8
		public void RemoveEventsRange(TypeDefinition type)
		{
			this.Events.Remove(type.token.RID);
		}

		// Token: 0x060008D4 RID: 2260 RVA: 0x000229C1 File Offset: 0x00020BC1
		public bool TryGetGenericParameterRanges(IGenericParameterProvider owner, out Range[] ranges)
		{
			return this.GenericParameters.TryGetValue(owner.MetadataToken, out ranges);
		}

		// Token: 0x060008D5 RID: 2261 RVA: 0x000229D5 File Offset: 0x00020BD5
		public void RemoveGenericParameterRange(IGenericParameterProvider owner)
		{
			this.GenericParameters.Remove(owner.MetadataToken);
		}

		// Token: 0x060008D6 RID: 2262 RVA: 0x000229E9 File Offset: 0x00020BE9
		public bool TryGetCustomAttributeRanges(ICustomAttributeProvider owner, out Range[] ranges)
		{
			return this.CustomAttributes.TryGetValue(owner.MetadataToken, out ranges);
		}

		// Token: 0x060008D7 RID: 2263 RVA: 0x000229FD File Offset: 0x00020BFD
		public void RemoveCustomAttributeRange(ICustomAttributeProvider owner)
		{
			this.CustomAttributes.Remove(owner.MetadataToken);
		}

		// Token: 0x060008D8 RID: 2264 RVA: 0x00022A11 File Offset: 0x00020C11
		public bool TryGetSecurityDeclarationRanges(ISecurityDeclarationProvider owner, out Range[] ranges)
		{
			return this.SecurityDeclarations.TryGetValue(owner.MetadataToken, out ranges);
		}

		// Token: 0x060008D9 RID: 2265 RVA: 0x00022A25 File Offset: 0x00020C25
		public void RemoveSecurityDeclarationRange(ISecurityDeclarationProvider owner)
		{
			this.SecurityDeclarations.Remove(owner.MetadataToken);
		}

		// Token: 0x060008DA RID: 2266 RVA: 0x00022A39 File Offset: 0x00020C39
		public bool TryGetGenericConstraintMapping(GenericParameter generic_parameter, out Collection<Row<uint, MetadataToken>> mapping)
		{
			return this.GenericConstraints.TryGetValue(generic_parameter.token.RID, out mapping);
		}

		// Token: 0x060008DB RID: 2267 RVA: 0x00022A52 File Offset: 0x00020C52
		public void SetGenericConstraintMapping(uint gp_rid, Collection<Row<uint, MetadataToken>> mapping)
		{
			this.GenericConstraints[gp_rid] = mapping;
		}

		// Token: 0x060008DC RID: 2268 RVA: 0x00022A61 File Offset: 0x00020C61
		public void RemoveGenericConstraintMapping(GenericParameter generic_parameter)
		{
			this.GenericConstraints.Remove(generic_parameter.token.RID);
		}

		// Token: 0x060008DD RID: 2269 RVA: 0x00022A7A File Offset: 0x00020C7A
		public bool TryGetOverrideMapping(MethodDefinition method, out Collection<MetadataToken> mapping)
		{
			return this.Overrides.TryGetValue(method.token.RID, out mapping);
		}

		// Token: 0x060008DE RID: 2270 RVA: 0x00022A93 File Offset: 0x00020C93
		public void SetOverrideMapping(uint rid, Collection<MetadataToken> mapping)
		{
			this.Overrides[rid] = mapping;
		}

		// Token: 0x060008DF RID: 2271 RVA: 0x00022AA2 File Offset: 0x00020CA2
		public void RemoveOverrideMapping(MethodDefinition method)
		{
			this.Overrides.Remove(method.token.RID);
		}

		// Token: 0x060008E0 RID: 2272 RVA: 0x00022ABB File Offset: 0x00020CBB
		public Document GetDocument(uint rid)
		{
			if (rid < 1U || (ulong)rid > (ulong)((long)this.Documents.Length))
			{
				return null;
			}
			return this.Documents[(int)(rid - 1U)];
		}

		// Token: 0x060008E1 RID: 2273 RVA: 0x00022ADC File Offset: 0x00020CDC
		public bool TryGetLocalScopes(MethodDefinition method, out Collection<Row<uint, Range, Range, uint, uint, uint>> scopes)
		{
			return this.LocalScopes.TryGetValue(method.MetadataToken.RID, out scopes);
		}

		// Token: 0x060008E2 RID: 2274 RVA: 0x00022B03 File Offset: 0x00020D03
		public void SetLocalScopes(uint method_rid, Collection<Row<uint, Range, Range, uint, uint, uint>> records)
		{
			this.LocalScopes[method_rid] = records;
		}

		// Token: 0x060008E3 RID: 2275 RVA: 0x00022B12 File Offset: 0x00020D12
		public ImportDebugInformation GetImportScope(uint rid)
		{
			if (rid < 1U || (ulong)rid > (ulong)((long)this.ImportScopes.Length))
			{
				return null;
			}
			return this.ImportScopes[(int)(rid - 1U)];
		}

		// Token: 0x060008E4 RID: 2276 RVA: 0x00022B34 File Offset: 0x00020D34
		public bool TryGetStateMachineKickOffMethod(MethodDefinition method, out uint rid)
		{
			return this.StateMachineMethods.TryGetValue(method.MetadataToken.RID, out rid);
		}

		// Token: 0x060008E5 RID: 2277 RVA: 0x00022B5B File Offset: 0x00020D5B
		public TypeDefinition GetFieldDeclaringType(uint field_rid)
		{
			return MetadataSystem.BinaryRangeSearch(this.Types, field_rid, true);
		}

		// Token: 0x060008E6 RID: 2278 RVA: 0x00022B6A File Offset: 0x00020D6A
		public TypeDefinition GetMethodDeclaringType(uint method_rid)
		{
			return MetadataSystem.BinaryRangeSearch(this.Types, method_rid, false);
		}

		// Token: 0x060008E7 RID: 2279 RVA: 0x00022B7C File Offset: 0x00020D7C
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

		// Token: 0x0400031C RID: 796
		internal AssemblyNameReference[] AssemblyReferences;

		// Token: 0x0400031D RID: 797
		internal ModuleReference[] ModuleReferences;

		// Token: 0x0400031E RID: 798
		internal TypeDefinition[] Types;

		// Token: 0x0400031F RID: 799
		internal TypeReference[] TypeReferences;

		// Token: 0x04000320 RID: 800
		internal FieldDefinition[] Fields;

		// Token: 0x04000321 RID: 801
		internal MethodDefinition[] Methods;

		// Token: 0x04000322 RID: 802
		internal MemberReference[] MemberReferences;

		// Token: 0x04000323 RID: 803
		internal Dictionary<uint, Collection<uint>> NestedTypes;

		// Token: 0x04000324 RID: 804
		internal Dictionary<uint, uint> ReverseNestedTypes;

		// Token: 0x04000325 RID: 805
		internal Dictionary<uint, Collection<Row<uint, MetadataToken>>> Interfaces;

		// Token: 0x04000326 RID: 806
		internal Dictionary<uint, Row<ushort, uint>> ClassLayouts;

		// Token: 0x04000327 RID: 807
		internal Dictionary<uint, uint> FieldLayouts;

		// Token: 0x04000328 RID: 808
		internal Dictionary<uint, uint> FieldRVAs;

		// Token: 0x04000329 RID: 809
		internal Dictionary<MetadataToken, uint> FieldMarshals;

		// Token: 0x0400032A RID: 810
		internal Dictionary<MetadataToken, Row<ElementType, uint>> Constants;

		// Token: 0x0400032B RID: 811
		internal Dictionary<uint, Collection<MetadataToken>> Overrides;

		// Token: 0x0400032C RID: 812
		internal Dictionary<MetadataToken, Range[]> CustomAttributes;

		// Token: 0x0400032D RID: 813
		internal Dictionary<MetadataToken, Range[]> SecurityDeclarations;

		// Token: 0x0400032E RID: 814
		internal Dictionary<uint, Range> Events;

		// Token: 0x0400032F RID: 815
		internal Dictionary<uint, Range> Properties;

		// Token: 0x04000330 RID: 816
		internal Dictionary<uint, Row<MethodSemanticsAttributes, MetadataToken>> Semantics;

		// Token: 0x04000331 RID: 817
		internal Dictionary<uint, Row<PInvokeAttributes, uint, uint>> PInvokes;

		// Token: 0x04000332 RID: 818
		internal Dictionary<MetadataToken, Range[]> GenericParameters;

		// Token: 0x04000333 RID: 819
		internal Dictionary<uint, Collection<Row<uint, MetadataToken>>> GenericConstraints;

		// Token: 0x04000334 RID: 820
		internal Document[] Documents;

		// Token: 0x04000335 RID: 821
		internal Dictionary<uint, Collection<Row<uint, Range, Range, uint, uint, uint>>> LocalScopes;

		// Token: 0x04000336 RID: 822
		internal ImportDebugInformation[] ImportScopes;

		// Token: 0x04000337 RID: 823
		internal Dictionary<uint, uint> StateMachineMethods;

		// Token: 0x04000338 RID: 824
		internal Dictionary<MetadataToken, Row<Guid, uint, uint>[]> CustomDebugInformations;

		// Token: 0x04000339 RID: 825
		private static Dictionary<string, Row<ElementType, bool>> primitive_value_types;
	}
}
