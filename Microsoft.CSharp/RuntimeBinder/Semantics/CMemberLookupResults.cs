using System;
using Microsoft.CSharp.RuntimeBinder.Syntax;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x02000050 RID: 80
	internal class CMemberLookupResults
	{
		// Token: 0x1700007E RID: 126
		// (get) Token: 0x060002BE RID: 702 RVA: 0x0001333E File Offset: 0x0001153E
		private TypeArray ContainingTypes { get; }

		// Token: 0x060002BF RID: 703 RVA: 0x00013346 File Offset: 0x00011546
		public CMemberLookupResults(TypeArray containingTypes, Name name)
		{
			this._pName = name;
			this.ContainingTypes = containingTypes;
			if (this.ContainingTypes == null)
			{
				this.ContainingTypes = BSYMMGR.EmptyTypeArray();
			}
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x00013370 File Offset: 0x00011570
		public CMemberLookupResults.CMethodIterator GetMethodIterator(CSemanticChecker pChecker, SymbolLoader pSymLoader, CType pObject, CType pQualifyingType, AggregateDeclaration pContext, bool allowBogusAndInaccessible, bool allowExtensionMethods, int arity, EXPRFLAG flags, symbmask_t mask)
		{
			return new CMemberLookupResults.CMethodIterator(pChecker, pSymLoader, this._pName, this.ContainingTypes, pObject, pQualifyingType, pContext, allowBogusAndInaccessible, allowExtensionMethods, arity, flags, mask);
		}

		// Token: 0x040003D5 RID: 981
		private readonly Name _pName;

		// Token: 0x020000EB RID: 235
		public class CMethodIterator
		{
			// Token: 0x0600073B RID: 1851 RVA: 0x00023514 File Offset: 0x00021714
			public CMethodIterator(CSemanticChecker checker, SymbolLoader symLoader, Name name, TypeArray containingTypes, CType @object, CType qualifyingType, AggregateDeclaration context, bool allowBogusAndInaccessible, bool allowExtensionMethods, int arity, EXPRFLAG flags, symbmask_t mask)
			{
				this._pSemanticChecker = checker;
				this._pSymbolLoader = symLoader;
				this._pCurrentType = null;
				this._pCurrentSym = null;
				this._pName = name;
				this._pContainingTypes = containingTypes;
				this._pQualifyingType = qualifyingType;
				this._pContext = context;
				this._bAllowBogusAndInaccessible = allowBogusAndInaccessible;
				this._nArity = arity;
				this._flags = flags;
				this._mask = mask;
				this._nCurrentTypeCount = 0;
				this._bIsCheckingInstanceMethods = true;
				this._bAtEnd = false;
				this._bCurrentSymIsBogus = false;
				this._bCurrentSymIsInaccessible = false;
				this._bcanIncludeExtensionsInResults = allowExtensionMethods;
			}

			// Token: 0x0600073C RID: 1852 RVA: 0x000235AD File Offset: 0x000217AD
			public MethodOrPropertySymbol GetCurrentSymbol()
			{
				return this._pCurrentSym;
			}

			// Token: 0x0600073D RID: 1853 RVA: 0x000235B5 File Offset: 0x000217B5
			public AggregateType GetCurrentType()
			{
				return this._pCurrentType;
			}

			// Token: 0x0600073E RID: 1854 RVA: 0x000235BD File Offset: 0x000217BD
			public bool IsCurrentSymbolInaccessible()
			{
				return this._bCurrentSymIsInaccessible;
			}

			// Token: 0x0600073F RID: 1855 RVA: 0x000235C5 File Offset: 0x000217C5
			public bool IsCurrentSymbolBogus()
			{
				return this._bCurrentSymIsBogus;
			}

			// Token: 0x06000740 RID: 1856 RVA: 0x000235D0 File Offset: 0x000217D0
			public bool MoveNext(bool canIncludeExtensionsInResults)
			{
				if (this._bcanIncludeExtensionsInResults)
				{
					this._bcanIncludeExtensionsInResults = canIncludeExtensionsInResults;
				}
				if (this._bAtEnd)
				{
					return false;
				}
				if (this._pCurrentType == null)
				{
					if (this._pContainingTypes.Count == 0)
					{
						this._bIsCheckingInstanceMethods = false;
						this._bAtEnd = true;
						return false;
					}
					if (!this.FindNextTypeForInstanceMethods())
					{
						this._bAtEnd = true;
						return false;
					}
				}
				if (!this.FindNextMethod())
				{
					this._bAtEnd = true;
					return false;
				}
				return true;
			}

			// Token: 0x06000741 RID: 1857 RVA: 0x0002363E File Offset: 0x0002183E
			public bool AtEnd()
			{
				return this._pCurrentSym == null;
			}

			// Token: 0x06000742 RID: 1858 RVA: 0x00023649 File Offset: 0x00021849
			private CSemanticChecker GetSemanticChecker()
			{
				return this._pSemanticChecker;
			}

			// Token: 0x06000743 RID: 1859 RVA: 0x00023651 File Offset: 0x00021851
			private SymbolLoader GetSymbolLoader()
			{
				return this._pSymbolLoader;
			}

			// Token: 0x06000744 RID: 1860 RVA: 0x0002365C File Offset: 0x0002185C
			public bool CanUseCurrentSymbol()
			{
				this._bCurrentSymIsInaccessible = false;
				this._bCurrentSymIsBogus = false;
				if ((this._mask == symbmask_t.MASK_MethodSymbol && ((this._flags & EXPRFLAG.EXF_CTOR) == (EXPRFLAG)0 != !((MethodSymbol)this._pCurrentSym).IsConstructor() || (this._flags & EXPRFLAG.EXF_OPERATOR) == (EXPRFLAG)0 != !((MethodSymbol)this._pCurrentSym).isOperator)) || (this._mask == symbmask_t.MASK_PropertySymbol && !(this._pCurrentSym is IndexerSymbol)))
				{
					return false;
				}
				if (this._nArity > 0 && this._mask == symbmask_t.MASK_MethodSymbol && ((MethodSymbol)this._pCurrentSym).typeVars.Count != this._nArity)
				{
					return false;
				}
				if (!ExpressionBinder.IsMethPropCallable(this._pCurrentSym, (this._flags & EXPRFLAG.EXF_USERCALLABLE) > (EXPRFLAG)0))
				{
					return false;
				}
				if (!this.GetSemanticChecker().CheckAccess(this._pCurrentSym, this._pCurrentType, this._pContext, this._pQualifyingType))
				{
					if (!this._bAllowBogusAndInaccessible)
					{
						return false;
					}
					this._bCurrentSymIsInaccessible = true;
				}
				if (CSemanticChecker.CheckBogus(this._pCurrentSym))
				{
					if (!this._bAllowBogusAndInaccessible)
					{
						return false;
					}
					this._bCurrentSymIsBogus = true;
				}
				return this._bIsCheckingInstanceMethods;
			}

			// Token: 0x06000745 RID: 1861 RVA: 0x00023794 File Offset: 0x00021994
			private bool FindNextMethod()
			{
				for (;;)
				{
					if (this._pCurrentSym == null)
					{
						this._pCurrentSym = this.GetSymbolLoader().LookupAggMember(this._pName, this._pCurrentType.getAggregate(), this._mask) as MethodOrPropertySymbol;
					}
					else
					{
						this._pCurrentSym = this.GetSymbolLoader().LookupNextSym(this._pCurrentSym, this._pCurrentType.getAggregate(), this._mask) as MethodOrPropertySymbol;
					}
					if (this._pCurrentSym != null)
					{
						return true;
					}
					if (this._bIsCheckingInstanceMethods)
					{
						if (!this.FindNextTypeForInstanceMethods() && this._bcanIncludeExtensionsInResults)
						{
							this._bIsCheckingInstanceMethods = false;
						}
						else if (this._pCurrentType == null && !this._bcanIncludeExtensionsInResults)
						{
							break;
						}
					}
				}
				return false;
			}

			// Token: 0x06000746 RID: 1862 RVA: 0x0002384C File Offset: 0x00021A4C
			private bool FindNextTypeForInstanceMethods()
			{
				if (this._pContainingTypes.Count > 0)
				{
					if (this._nCurrentTypeCount >= this._pContainingTypes.Count)
					{
						this._pCurrentType = null;
					}
					else
					{
						TypeArray pContainingTypes = this._pContainingTypes;
						int nCurrentTypeCount = this._nCurrentTypeCount;
						this._nCurrentTypeCount = nCurrentTypeCount + 1;
						this._pCurrentType = pContainingTypes[nCurrentTypeCount] as AggregateType;
					}
				}
				else
				{
					this._pCurrentType = this._pCurrentType.GetBaseClass();
				}
				return this._pCurrentType != null;
			}

			// Token: 0x040006F3 RID: 1779
			private SymbolLoader _pSymbolLoader;

			// Token: 0x040006F4 RID: 1780
			private CSemanticChecker _pSemanticChecker;

			// Token: 0x040006F5 RID: 1781
			private AggregateType _pCurrentType;

			// Token: 0x040006F6 RID: 1782
			private MethodOrPropertySymbol _pCurrentSym;

			// Token: 0x040006F7 RID: 1783
			private AggregateDeclaration _pContext;

			// Token: 0x040006F8 RID: 1784
			private TypeArray _pContainingTypes;

			// Token: 0x040006F9 RID: 1785
			private CType _pQualifyingType;

			// Token: 0x040006FA RID: 1786
			private Name _pName;

			// Token: 0x040006FB RID: 1787
			private int _nArity;

			// Token: 0x040006FC RID: 1788
			private symbmask_t _mask;

			// Token: 0x040006FD RID: 1789
			private EXPRFLAG _flags;

			// Token: 0x040006FE RID: 1790
			private int _nCurrentTypeCount;

			// Token: 0x040006FF RID: 1791
			private bool _bIsCheckingInstanceMethods;

			// Token: 0x04000700 RID: 1792
			private bool _bAtEnd;

			// Token: 0x04000701 RID: 1793
			private bool _bAllowBogusAndInaccessible;

			// Token: 0x04000702 RID: 1794
			private bool _bCurrentSymIsBogus;

			// Token: 0x04000703 RID: 1795
			private bool _bCurrentSymIsInaccessible;

			// Token: 0x04000704 RID: 1796
			private bool _bcanIncludeExtensionsInResults;
		}
	}
}
