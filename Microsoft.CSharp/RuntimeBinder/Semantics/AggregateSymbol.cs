using System;
using System.Reflection;
using Microsoft.CSharp.RuntimeBinder.Syntax;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x02000060 RID: 96
	internal class AggregateSymbol : NamespaceOrAggregateSymbol
	{
		// Token: 0x06000340 RID: 832 RVA: 0x000162B2 File Offset: 0x000144B2
		public AggregateSymbol GetBaseAgg()
		{
			AggregateType pBaseClass = this._pBaseClass;
			if (pBaseClass == null)
			{
				return null;
			}
			return pBaseClass.getAggregate();
		}

		// Token: 0x06000341 RID: 833 RVA: 0x000162C8 File Offset: 0x000144C8
		public AggregateType getThisType()
		{
			if (this._atsInst == null)
			{
				AggregateType aggregateType = (this.isNested() ? this.GetOuterAgg().getThisType() : null);
				this._atsInst = this._pTypeManager.GetAggregate(this, aggregateType, this.GetTypeVars());
			}
			return this._atsInst;
		}

		// Token: 0x06000342 RID: 834 RVA: 0x00016314 File Offset: 0x00014514
		public bool FindBaseAgg(AggregateSymbol agg)
		{
			for (AggregateSymbol aggregateSymbol = this; aggregateSymbol != null; aggregateSymbol = aggregateSymbol.GetBaseAgg())
			{
				if (aggregateSymbol == agg)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000343 RID: 835 RVA: 0x00016336 File Offset: 0x00014536
		public NamespaceOrAggregateSymbol Parent
		{
			get
			{
				return this.parent as NamespaceOrAggregateSymbol;
			}
		}

		// Token: 0x06000344 RID: 836 RVA: 0x00016343 File Offset: 0x00014543
		public bool isNested()
		{
			return this.parent is AggregateSymbol;
		}

		// Token: 0x06000345 RID: 837 RVA: 0x00016353 File Offset: 0x00014553
		public AggregateSymbol GetOuterAgg()
		{
			return this.parent as AggregateSymbol;
		}

		// Token: 0x06000346 RID: 838 RVA: 0x00016360 File Offset: 0x00014560
		public bool isPredefAgg(PredefinedType pt)
		{
			return this._isPredefined && this._iPredef == pt;
		}

		// Token: 0x06000347 RID: 839 RVA: 0x00016375 File Offset: 0x00014575
		public AggKindEnum AggKind()
		{
			return this._aggKind;
		}

		// Token: 0x06000348 RID: 840 RVA: 0x0001637D File Offset: 0x0001457D
		public void SetAggKind(AggKindEnum aggKind)
		{
			this._aggKind = aggKind;
			if (aggKind == AggKindEnum.Interface)
			{
				this.SetAbstract(true);
			}
		}

		// Token: 0x06000349 RID: 841 RVA: 0x00016391 File Offset: 0x00014591
		public bool IsClass()
		{
			return this.AggKind() == AggKindEnum.Class;
		}

		// Token: 0x0600034A RID: 842 RVA: 0x0001639C File Offset: 0x0001459C
		public bool IsDelegate()
		{
			return this.AggKind() == AggKindEnum.Delegate;
		}

		// Token: 0x0600034B RID: 843 RVA: 0x000163A7 File Offset: 0x000145A7
		public bool IsInterface()
		{
			return this.AggKind() == AggKindEnum.Interface;
		}

		// Token: 0x0600034C RID: 844 RVA: 0x000163B2 File Offset: 0x000145B2
		public bool IsStruct()
		{
			return this.AggKind() == AggKindEnum.Struct;
		}

		// Token: 0x0600034D RID: 845 RVA: 0x000163BD File Offset: 0x000145BD
		public bool IsEnum()
		{
			return this.AggKind() == AggKindEnum.Enum;
		}

		// Token: 0x0600034E RID: 846 RVA: 0x000163C8 File Offset: 0x000145C8
		public bool IsValueType()
		{
			return this.AggKind() == AggKindEnum.Struct || this.AggKind() == AggKindEnum.Enum;
		}

		// Token: 0x0600034F RID: 847 RVA: 0x000163DE File Offset: 0x000145DE
		public bool IsRefType()
		{
			return this.AggKind() == AggKindEnum.Class || this.AggKind() == AggKindEnum.Interface || this.AggKind() == AggKindEnum.Delegate;
		}

		// Token: 0x06000350 RID: 848 RVA: 0x000163FD File Offset: 0x000145FD
		public bool IsStatic()
		{
			return this._isAbstract && this._isSealed;
		}

		// Token: 0x06000351 RID: 849 RVA: 0x0001640F File Offset: 0x0001460F
		public bool IsAbstract()
		{
			return this._isAbstract;
		}

		// Token: 0x06000352 RID: 850 RVA: 0x00016417 File Offset: 0x00014617
		public void SetAbstract(bool @abstract)
		{
			this._isAbstract = @abstract;
		}

		// Token: 0x06000353 RID: 851 RVA: 0x00016420 File Offset: 0x00014620
		public bool IsPredefined()
		{
			return this._isPredefined;
		}

		// Token: 0x06000354 RID: 852 RVA: 0x00016428 File Offset: 0x00014628
		public void SetPredefined(bool predefined)
		{
			this._isPredefined = predefined;
		}

		// Token: 0x06000355 RID: 853 RVA: 0x00016431 File Offset: 0x00014631
		public PredefinedType GetPredefType()
		{
			return this._iPredef;
		}

		// Token: 0x06000356 RID: 854 RVA: 0x00016439 File Offset: 0x00014639
		public void SetPredefType(PredefinedType predef)
		{
			this._iPredef = predef;
		}

		// Token: 0x06000357 RID: 855 RVA: 0x00016442 File Offset: 0x00014642
		public bool IsSealed()
		{
			return this._isSealed;
		}

		// Token: 0x06000358 RID: 856 RVA: 0x0001644A File Offset: 0x0001464A
		public void SetSealed(bool @sealed)
		{
			this._isSealed = @sealed;
		}

		// Token: 0x06000359 RID: 857 RVA: 0x00016454 File Offset: 0x00014654
		public bool HasConversion(SymbolLoader pLoader)
		{
			pLoader.RuntimeBinderSymbolTable.AddConversionsForType(this.AssociatedSystemType);
			if (this._hasConversion == null)
			{
				this._hasConversion = new bool?(this.GetBaseAgg() != null && this.GetBaseAgg().HasConversion(pLoader));
			}
			return this._hasConversion.Value;
		}

		// Token: 0x0600035A RID: 858 RVA: 0x000164AC File Offset: 0x000146AC
		public void SetHasConversion()
		{
			this._hasConversion = new bool?(true);
		}

		// Token: 0x0600035B RID: 859 RVA: 0x000164BA File Offset: 0x000146BA
		public bool HasPubNoArgCtor()
		{
			return this._hasPubNoArgCtor;
		}

		// Token: 0x0600035C RID: 860 RVA: 0x000164C2 File Offset: 0x000146C2
		public void SetHasPubNoArgCtor(bool hasPubNoArgCtor)
		{
			this._hasPubNoArgCtor = hasPubNoArgCtor;
		}

		// Token: 0x0600035D RID: 861 RVA: 0x000164CB File Offset: 0x000146CB
		public bool IsSkipUDOps()
		{
			return this._isSkipUDOps;
		}

		// Token: 0x0600035E RID: 862 RVA: 0x000164D3 File Offset: 0x000146D3
		public void SetSkipUDOps(bool skipUDOps)
		{
			this._isSkipUDOps = skipUDOps;
		}

		// Token: 0x0600035F RID: 863 RVA: 0x000164DC File Offset: 0x000146DC
		public TypeArray GetTypeVars()
		{
			return this._typeVarsThis;
		}

		// Token: 0x06000360 RID: 864 RVA: 0x000164E4 File Offset: 0x000146E4
		public void SetTypeVars(TypeArray typeVars)
		{
			if (typeVars == null)
			{
				this._typeVarsThis = null;
				this._typeVarsAll = null;
				return;
			}
			TypeArray typeArray;
			if (this.GetOuterAgg() != null)
			{
				typeArray = this.GetOuterAgg().GetTypeVarsAll();
			}
			else
			{
				typeArray = BSYMMGR.EmptyTypeArray();
			}
			this._typeVarsThis = typeVars;
			this._typeVarsAll = this._pTypeManager.ConcatenateTypeArrays(typeArray, typeVars);
		}

		// Token: 0x06000361 RID: 865 RVA: 0x00016539 File Offset: 0x00014739
		public TypeArray GetTypeVarsAll()
		{
			return this._typeVarsAll;
		}

		// Token: 0x06000362 RID: 866 RVA: 0x00016541 File Offset: 0x00014741
		public AggregateType GetBaseClass()
		{
			return this._pBaseClass;
		}

		// Token: 0x06000363 RID: 867 RVA: 0x00016549 File Offset: 0x00014749
		public void SetBaseClass(AggregateType baseClass)
		{
			this._pBaseClass = baseClass;
		}

		// Token: 0x06000364 RID: 868 RVA: 0x00016552 File Offset: 0x00014752
		public AggregateType GetUnderlyingType()
		{
			return this._pUnderlyingType;
		}

		// Token: 0x06000365 RID: 869 RVA: 0x0001655A File Offset: 0x0001475A
		public void SetUnderlyingType(AggregateType underlyingType)
		{
			this._pUnderlyingType = underlyingType;
		}

		// Token: 0x06000366 RID: 870 RVA: 0x00016563 File Offset: 0x00014763
		public TypeArray GetIfaces()
		{
			return this._ifaces;
		}

		// Token: 0x06000367 RID: 871 RVA: 0x0001656B File Offset: 0x0001476B
		public void SetIfaces(TypeArray ifaces)
		{
			this._ifaces = ifaces;
		}

		// Token: 0x06000368 RID: 872 RVA: 0x00016574 File Offset: 0x00014774
		public TypeArray GetIfacesAll()
		{
			return this._ifacesAll;
		}

		// Token: 0x06000369 RID: 873 RVA: 0x0001657C File Offset: 0x0001477C
		public void SetIfacesAll(TypeArray ifacesAll)
		{
			this._ifacesAll = ifacesAll;
		}

		// Token: 0x0600036A RID: 874 RVA: 0x00016585 File Offset: 0x00014785
		public TypeManager GetTypeManager()
		{
			return this._pTypeManager;
		}

		// Token: 0x0600036B RID: 875 RVA: 0x0001658D File Offset: 0x0001478D
		public void SetTypeManager(TypeManager typeManager)
		{
			this._pTypeManager = typeManager;
		}

		// Token: 0x0600036C RID: 876 RVA: 0x00016596 File Offset: 0x00014796
		public MethodSymbol GetFirstUDConversion()
		{
			return this._pConvFirst;
		}

		// Token: 0x0600036D RID: 877 RVA: 0x0001659E File Offset: 0x0001479E
		public void SetFirstUDConversion(MethodSymbol conv)
		{
			this._pConvFirst = conv;
		}

		// Token: 0x0600036E RID: 878 RVA: 0x000165A7 File Offset: 0x000147A7
		public bool InternalsVisibleTo(Assembly assembly)
		{
			return this._pTypeManager.InternalsVisibleTo(this.AssociatedAssembly, assembly);
		}

		// Token: 0x04000489 RID: 1161
		public Type AssociatedSystemType;

		// Token: 0x0400048A RID: 1162
		public Assembly AssociatedAssembly;

		// Token: 0x0400048B RID: 1163
		private AggregateType _atsInst;

		// Token: 0x0400048C RID: 1164
		private AggregateType _pBaseClass;

		// Token: 0x0400048D RID: 1165
		private AggregateType _pUnderlyingType;

		// Token: 0x0400048E RID: 1166
		private TypeArray _ifaces;

		// Token: 0x0400048F RID: 1167
		private TypeArray _ifacesAll;

		// Token: 0x04000490 RID: 1168
		private TypeArray _typeVarsThis;

		// Token: 0x04000491 RID: 1169
		private TypeArray _typeVarsAll;

		// Token: 0x04000492 RID: 1170
		private TypeManager _pTypeManager;

		// Token: 0x04000493 RID: 1171
		private MethodSymbol _pConvFirst;

		// Token: 0x04000494 RID: 1172
		private AggKindEnum _aggKind;

		// Token: 0x04000495 RID: 1173
		private bool _isPredefined;

		// Token: 0x04000496 RID: 1174
		private PredefinedType _iPredef;

		// Token: 0x04000497 RID: 1175
		private bool _isAbstract;

		// Token: 0x04000498 RID: 1176
		private bool _isSealed;

		// Token: 0x04000499 RID: 1177
		private bool _hasPubNoArgCtor;

		// Token: 0x0400049A RID: 1178
		private bool _isSkipUDOps;

		// Token: 0x0400049B RID: 1179
		private bool? _hasConversion;
	}
}
