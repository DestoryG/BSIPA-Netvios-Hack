using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.CSharp.RuntimeBinder.Syntax;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x02000074 RID: 116
	internal sealed class BSYMMGR
	{
		// Token: 0x060003F9 RID: 1017 RVA: 0x00017778 File Offset: 0x00015978
		public BSYMMGR(NameManager nameMgr)
		{
			this.m_nameTable = nameMgr;
			this.tableGlobal = new SYMTBL();
			this._symFactory = new SymFactory(this.tableGlobal);
			this.tableTypeArrays = new Dictionary<BSYMMGR.TypeArrayKey, TypeArray>();
			this._rootNS = this._symFactory.CreateNamespace(this.m_nameTable.Lookup(""), null);
			this.GetNsAid(this._rootNS);
			for (int i = 0; i < 48; i++)
			{
				NamespaceSymbol namespaceSymbol = this.GetRootNS();
				string name = PredefinedTypeFacts.GetName((PredefinedType)i);
				string text;
				for (int j = 0; j < name.Length; j += text.Length + 1)
				{
					int num = name.IndexOf('.', j);
					if (num == -1)
					{
						break;
					}
					text = ((num > j) ? name.Substring(j, num - j) : name.Substring(j));
					Name name2 = this.GetNameManager().Add(text);
					namespaceSymbol = (this.LookupGlobalSymCore(name2, namespaceSymbol, symbmask_t.MASK_NamespaceSymbol) as NamespaceSymbol) ?? this._symFactory.CreateNamespace(name2, namespaceSymbol);
				}
			}
		}

		// Token: 0x060003FA RID: 1018 RVA: 0x0001787F File Offset: 0x00015A7F
		public NameManager GetNameManager()
		{
			return this.m_nameTable;
		}

		// Token: 0x060003FB RID: 1019 RVA: 0x00017887 File Offset: 0x00015A87
		public SYMTBL GetSymbolTable()
		{
			return this.tableGlobal;
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x0001788F File Offset: 0x00015A8F
		public static TypeArray EmptyTypeArray()
		{
			return BSYMMGR.s_taEmpty;
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x00017896 File Offset: 0x00015A96
		public AssemblyQualifiedNamespaceSymbol GetRootNsAid()
		{
			return this.GetNsAid(this._rootNS);
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x000178A4 File Offset: 0x00015AA4
		public NamespaceSymbol GetRootNS()
		{
			return this._rootNS;
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x000178AC File Offset: 0x00015AAC
		public BetterType CompareTypes(TypeArray ta1, TypeArray ta2)
		{
			if (ta1 == ta2)
			{
				return BetterType.Same;
			}
			if (ta1.Count == ta2.Count)
			{
				BetterType betterType = BetterType.Neither;
				int i = 0;
				IL_00F6:
				while (i < ta1.Count)
				{
					CType ctype = ta1[i];
					CType ctype2 = ta2[i];
					BetterType betterType2 = BetterType.Neither;
					while (ctype.GetTypeKind() == ctype2.GetTypeKind())
					{
						switch (ctype.GetTypeKind())
						{
						case TypeKind.TK_AggregateType:
							betterType2 = this.CompareTypes(((AggregateType)ctype).GetTypeArgsAll(), ((AggregateType)ctype2).GetTypeArgsAll());
							break;
						case TypeKind.TK_ArrayType:
						case TypeKind.TK_PointerType:
						case TypeKind.TK_ParameterModifierType:
						case TypeKind.TK_NullableType:
							ctype = ctype.GetBaseOrParameterOrElementType();
							ctype2 = ctype2.GetBaseOrParameterOrElementType();
							continue;
						}
						IL_00D5:
						if (betterType2 == BetterType.Right || betterType2 == BetterType.Left)
						{
							if (betterType == BetterType.Same || betterType == BetterType.Neither)
							{
								betterType = betterType2;
							}
							else if (betterType2 != betterType)
							{
								return BetterType.Neither;
							}
						}
						i++;
						goto IL_00F6;
					}
					if (ctype is TypeParameterType)
					{
						betterType2 = BetterType.Right;
						goto IL_00D5;
					}
					if (ctype2 is TypeParameterType)
					{
						betterType2 = BetterType.Left;
						goto IL_00D5;
					}
					goto IL_00D5;
				}
				return betterType;
			}
			if (ta1.Count <= ta2.Count)
			{
				return BetterType.Right;
			}
			return BetterType.Left;
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x000179BC File Offset: 0x00015BBC
		public SymFactory GetSymFactory()
		{
			return this._symFactory;
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x000179C4 File Offset: 0x00015BC4
		private void InitPreLoad()
		{
			for (int i = 0; i < 48; i++)
			{
				NamespaceSymbol namespaceSymbol = this.GetRootNS();
				string name = PredefinedTypeFacts.GetName((PredefinedType)i);
				string text;
				for (int j = 0; j < name.Length; j += text.Length + 1)
				{
					int num = name.IndexOf('.', j);
					if (num == -1)
					{
						break;
					}
					text = ((num > j) ? name.Substring(j, num - j) : name.Substring(j));
					Name name2 = this.GetNameManager().Add(text);
					namespaceSymbol = (this.LookupGlobalSymCore(name2, namespaceSymbol, symbmask_t.MASK_NamespaceSymbol) as NamespaceSymbol) ?? this._symFactory.CreateNamespace(name2, namespaceSymbol);
				}
			}
		}

		// Token: 0x06000402 RID: 1026 RVA: 0x00017A68 File Offset: 0x00015C68
		public Symbol LookupGlobalSymCore(Name name, ParentSymbol parent, symbmask_t kindmask)
		{
			return this.tableGlobal.LookupSym(name, parent, kindmask);
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x00017A78 File Offset: 0x00015C78
		public Symbol LookupAggMember(Name name, AggregateSymbol agg, symbmask_t mask)
		{
			return this.tableGlobal.LookupSym(name, agg, mask);
		}

		// Token: 0x06000404 RID: 1028 RVA: 0x00017A88 File Offset: 0x00015C88
		public static Symbol LookupNextSym(Symbol sym, ParentSymbol parent, symbmask_t kindmask)
		{
			for (sym = sym.nextSameName; sym != null; sym = sym.nextSameName)
			{
				if ((kindmask & sym.mask()) > (symbmask_t)0L)
				{
					return sym;
				}
			}
			return null;
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x00017AB0 File Offset: 0x00015CB0
		public Name GetNameFromPtrs(object u1, object u2)
		{
			if (u2 != null)
			{
				return this.m_nameTable.Add(string.Format(CultureInfo.InvariantCulture, "{0:X}-{1:X}", u1.GetHashCode(), u2.GetHashCode()));
			}
			return this.m_nameTable.Add(string.Format(CultureInfo.InvariantCulture, "{0:X}", u1.GetHashCode()));
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x00017B18 File Offset: 0x00015D18
		private AssemblyQualifiedNamespaceSymbol GetNsAid(NamespaceSymbol ns)
		{
			Name nameFromPtrs = this.GetNameFromPtrs(0, 0);
			return (this.LookupGlobalSymCore(nameFromPtrs, ns, symbmask_t.MASK_AssemblyQualifiedNamespaceSymbol) as AssemblyQualifiedNamespaceSymbol) ?? this._symFactory.CreateNamespaceAid(nameFromPtrs, ns);
		}

		// Token: 0x06000407 RID: 1031 RVA: 0x00017B58 File Offset: 0x00015D58
		public TypeArray AllocParams(int ctype, CType[] prgtype)
		{
			if (ctype == 0)
			{
				return BSYMMGR.s_taEmpty;
			}
			return this.AllocParams(prgtype);
		}

		// Token: 0x06000408 RID: 1032 RVA: 0x00017B6C File Offset: 0x00015D6C
		public TypeArray AllocParams(int ctype, TypeArray array, int offset)
		{
			Array items = array.Items;
			CType[] array2 = new CType[ctype];
			Array.ConstrainedCopy(items, offset, array2, 0, ctype);
			return this.AllocParams(array2);
		}

		// Token: 0x06000409 RID: 1033 RVA: 0x00017B98 File Offset: 0x00015D98
		public TypeArray AllocParams(params CType[] types)
		{
			if (types == null || types.Length == 0)
			{
				return BSYMMGR.s_taEmpty;
			}
			BSYMMGR.TypeArrayKey typeArrayKey = new BSYMMGR.TypeArrayKey(types);
			TypeArray typeArray;
			if (!this.tableTypeArrays.TryGetValue(typeArrayKey, out typeArray))
			{
				typeArray = new TypeArray(types);
				this.tableTypeArrays.Add(typeArrayKey, typeArray);
			}
			return typeArray;
		}

		// Token: 0x0600040A RID: 1034 RVA: 0x00017BE0 File Offset: 0x00015DE0
		private TypeArray ConcatParams(CType[] prgtype1, CType[] prgtype2)
		{
			CType[] array = new CType[prgtype1.Length + prgtype2.Length];
			Array.Copy(prgtype1, 0, array, 0, prgtype1.Length);
			Array.Copy(prgtype2, 0, array, prgtype1.Length, prgtype2.Length);
			return this.AllocParams(array);
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x00017C1B File Offset: 0x00015E1B
		public TypeArray ConcatParams(TypeArray pta1, TypeArray pta2)
		{
			return this.ConcatParams(pta1.Items, pta2.Items);
		}

		// Token: 0x040004FD RID: 1277
		public PropertySymbol propNubValue;

		// Token: 0x040004FE RID: 1278
		public MethodSymbol methNubCtor;

		// Token: 0x040004FF RID: 1279
		private readonly SymFactory _symFactory;

		// Token: 0x04000500 RID: 1280
		private readonly NamespaceSymbol _rootNS;

		// Token: 0x04000501 RID: 1281
		private NameManager m_nameTable;

		// Token: 0x04000502 RID: 1282
		private SYMTBL tableGlobal;

		// Token: 0x04000503 RID: 1283
		private Dictionary<BSYMMGR.TypeArrayKey, TypeArray> tableTypeArrays;

		// Token: 0x04000504 RID: 1284
		private static readonly TypeArray s_taEmpty = new TypeArray(Array.Empty<CType>());

		// Token: 0x020000EE RID: 238
		private struct TypeArrayKey : IEquatable<BSYMMGR.TypeArrayKey>
		{
			// Token: 0x06000747 RID: 1863 RVA: 0x000238C8 File Offset: 0x00021AC8
			public TypeArrayKey(CType[] types)
			{
				this._types = types;
				this._hashCode = 0;
				int i = 0;
				int num = types.Length;
				while (i < num)
				{
					this._hashCode ^= types[i].GetHashCode();
					i++;
				}
			}

			// Token: 0x06000748 RID: 1864 RVA: 0x00023908 File Offset: 0x00021B08
			public bool Equals(BSYMMGR.TypeArrayKey other)
			{
				CType[] types = this._types;
				CType[] types2 = other._types;
				if (types2 == types)
				{
					return true;
				}
				if (other._hashCode != this._hashCode || types2.Length != types.Length)
				{
					return false;
				}
				for (int i = 0; i < types.Length; i++)
				{
					if (!types[i].Equals(types2[i]))
					{
						return false;
					}
				}
				return true;
			}

			// Token: 0x06000749 RID: 1865 RVA: 0x0002395F File Offset: 0x00021B5F
			public override bool Equals(object obj)
			{
				return obj is BSYMMGR.TypeArrayKey && this.Equals((BSYMMGR.TypeArrayKey)obj);
			}

			// Token: 0x0600074A RID: 1866 RVA: 0x00023977 File Offset: 0x00021B77
			public override int GetHashCode()
			{
				return this._hashCode;
			}

			// Token: 0x04000710 RID: 1808
			private readonly CType[] _types;

			// Token: 0x04000711 RID: 1809
			private readonly int _hashCode;
		}
	}
}
