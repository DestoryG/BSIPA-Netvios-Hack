using System;
using Microsoft.CSharp.RuntimeBinder.Errors;
using Microsoft.CSharp.RuntimeBinder.Syntax;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x0200005D RID: 93
	internal sealed class CSemanticChecker
	{
		// Token: 0x06000328 RID: 808 RVA: 0x00015E60 File Offset: 0x00014060
		public void CheckForStaticClass(Symbol symCtx, CType CType, ErrorCode err)
		{
			if (CType.isStaticClass())
			{
				throw this.ReportStaticClassError(symCtx, CType, err);
			}
		}

		// Token: 0x06000329 RID: 809 RVA: 0x00015E74 File Offset: 0x00014074
		public ACCESSERROR CheckAccess2(Symbol symCheck, AggregateType atsCheck, Symbol symWhere, CType typeThru)
		{
			ACCESSERROR accesserror = this.CheckAccessCore(symCheck, atsCheck, symWhere, typeThru);
			if (ACCESSERROR.ACCESSERROR_NOERROR != accesserror)
			{
				return accesserror;
			}
			CType ctype = symCheck.getType();
			if (ctype == null)
			{
				return ACCESSERROR.ACCESSERROR_NOERROR;
			}
			if (atsCheck.GetTypeArgsAll().Count > 0)
			{
				ctype = this.SymbolLoader.GetTypeManager().SubstType(ctype, atsCheck);
			}
			if (!this.CheckTypeAccess(ctype, symWhere))
			{
				return ACCESSERROR.ACCESSERROR_NOACCESS;
			}
			return ACCESSERROR.ACCESSERROR_NOERROR;
		}

		// Token: 0x0600032A RID: 810 RVA: 0x00015ED0 File Offset: 0x000140D0
		public bool CheckTypeAccess(CType type, Symbol symWhere)
		{
			type = type.GetNakedType(true);
			AggregateType aggregateType;
			if ((aggregateType = type as AggregateType) == null)
			{
				return true;
			}
			while (ACCESSERROR.ACCESSERROR_NOERROR == this.CheckAccessCore(aggregateType.GetOwningAggregate(), aggregateType.outerType, symWhere, null))
			{
				aggregateType = aggregateType.outerType;
				if (aggregateType == null)
				{
					TypeArray typeArgsAll = ((AggregateType)type).GetTypeArgsAll();
					for (int i = 0; i < typeArgsAll.Count; i++)
					{
						if (!this.CheckTypeAccess(typeArgsAll[i], symWhere))
						{
							return false;
						}
					}
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600032B RID: 811 RVA: 0x00015F45 File Offset: 0x00014145
		public RuntimeBinderException ReportStaticClassError(Symbol symCtx, CType CType, ErrorCode err)
		{
			ErrorHandling errorContext = this.ErrorContext;
			ErrArg[] array;
			if (symCtx == null)
			{
				(array = new ErrArg[1])[0] = CType;
			}
			else
			{
				ErrArg[] array2 = new ErrArg[2];
				array2[0] = CType;
				array = array2;
				array2[1] = new ErrArg(symCtx);
			}
			return errorContext.Error(err, array);
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x0600032C RID: 812 RVA: 0x00015F7F File Offset: 0x0001417F
		public SymbolLoader SymbolLoader { get; } = new SymbolLoader();

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x0600032D RID: 813 RVA: 0x00015F87 File Offset: 0x00014187
		public ErrorHandling ErrorContext
		{
			get
			{
				return this.SymbolLoader.ErrorContext;
			}
		}

		// Token: 0x0600032E RID: 814 RVA: 0x00015F94 File Offset: 0x00014194
		public NameManager GetNameManager()
		{
			return this.SymbolLoader.GetNameManager();
		}

		// Token: 0x0600032F RID: 815 RVA: 0x00015FA1 File Offset: 0x000141A1
		public TypeManager GetTypeManager()
		{
			return this.SymbolLoader.GetTypeManager();
		}

		// Token: 0x06000330 RID: 816 RVA: 0x00015FAE File Offset: 0x000141AE
		public BSYMMGR getBSymmgr()
		{
			return this.SymbolLoader.getBSymmgr();
		}

		// Token: 0x06000331 RID: 817 RVA: 0x00015FBB File Offset: 0x000141BB
		public SymFactory GetGlobalSymbolFactory()
		{
			return this.SymbolLoader.GetGlobalSymbolFactory();
		}

		// Token: 0x06000332 RID: 818 RVA: 0x00015FC8 File Offset: 0x000141C8
		public PredefinedTypes getPredefTypes()
		{
			return this.SymbolLoader.GetPredefindTypes();
		}

		// Token: 0x06000333 RID: 819 RVA: 0x00015FD8 File Offset: 0x000141D8
		private ACCESSERROR CheckAccessCore(Symbol symCheck, AggregateType atsCheck, Symbol symWhere, CType typeThru)
		{
			switch (symCheck.GetAccess())
			{
			case ACCESS.ACC_UNKNOWN:
				return ACCESSERROR.ACCESSERROR_NOACCESS;
			case ACCESS.ACC_PRIVATE:
			case ACCESS.ACC_PROTECTED:
				if (symWhere == null)
				{
					return ACCESSERROR.ACCESSERROR_NOACCESS;
				}
				break;
			case ACCESS.ACC_INTERNAL:
			case ACCESS.ACC_INTERNALPROTECTED:
				if (symWhere == null)
				{
					return ACCESSERROR.ACCESSERROR_NOACCESS;
				}
				if (symWhere.SameAssemOrFriend(symCheck))
				{
					return ACCESSERROR.ACCESSERROR_NOERROR;
				}
				if (symCheck.GetAccess() == ACCESS.ACC_INTERNAL)
				{
					return ACCESSERROR.ACCESSERROR_NOACCESS;
				}
				break;
			case ACCESS.ACC_PUBLIC:
				return ACCESSERROR.ACCESSERROR_NOERROR;
			default:
				throw Error.InternalCompilerError();
			}
			AggregateSymbol aggregateSymbol = null;
			for (Symbol symbol = symWhere; symbol != null; symbol = symbol.parent)
			{
				AggregateSymbol aggregateSymbol2;
				if ((aggregateSymbol2 = symbol as AggregateSymbol) != null)
				{
					aggregateSymbol = aggregateSymbol2;
					break;
				}
				AggregateDeclaration aggregateDeclaration;
				if ((aggregateDeclaration = symbol as AggregateDeclaration) != null)
				{
					aggregateSymbol = aggregateDeclaration.Agg();
					break;
				}
			}
			if (aggregateSymbol == null)
			{
				return ACCESSERROR.ACCESSERROR_NOACCESS;
			}
			AggregateSymbol aggregateSymbol3 = symCheck.parent as AggregateSymbol;
			for (AggregateSymbol aggregateSymbol4 = aggregateSymbol; aggregateSymbol4 != null; aggregateSymbol4 = aggregateSymbol4.GetOuterAgg())
			{
				if (aggregateSymbol4 == aggregateSymbol3)
				{
					return ACCESSERROR.ACCESSERROR_NOERROR;
				}
			}
			if (symCheck.GetAccess() == ACCESS.ACC_PRIVATE)
			{
				return ACCESSERROR.ACCESSERROR_NOACCESS;
			}
			AggregateType aggregateType = null;
			if (typeThru != null && !symCheck.isStatic)
			{
				aggregateType = this.SymbolLoader.GetAggTypeSym(typeThru);
			}
			bool flag = false;
			for (AggregateSymbol aggregateSymbol5 = aggregateSymbol; aggregateSymbol5 != null; aggregateSymbol5 = aggregateSymbol5.GetOuterAgg())
			{
				if (aggregateSymbol5.FindBaseAgg(aggregateSymbol3))
				{
					flag = true;
					if (aggregateType == null || aggregateType.getAggregate().FindBaseAgg(aggregateSymbol5))
					{
						return ACCESSERROR.ACCESSERROR_NOERROR;
					}
				}
			}
			if (!flag)
			{
				return ACCESSERROR.ACCESSERROR_NOACCESS;
			}
			if (aggregateType != null)
			{
				return ACCESSERROR.ACCESSERROR_NOACCESSTHRU;
			}
			return ACCESSERROR.ACCESSERROR_NOACCESS;
		}

		// Token: 0x06000334 RID: 820 RVA: 0x00016101 File Offset: 0x00014301
		public static bool CheckBogus(Symbol sym)
		{
			PropertySymbol propertySymbol = sym as PropertySymbol;
			return propertySymbol != null && propertySymbol.Bogus;
		}

		// Token: 0x06000335 RID: 821 RVA: 0x00016114 File Offset: 0x00014314
		public RuntimeBinderException ReportAccessError(SymWithType swtBad, Symbol symWhere, CType typeQual)
		{
			if (this.CheckAccess2(swtBad.Sym, swtBad.GetType(), symWhere, typeQual) != ACCESSERROR.ACCESSERROR_NOACCESSTHRU)
			{
				return this.ErrorContext.Error(ErrorCode.ERR_BadAccess, new ErrArg[] { swtBad });
			}
			return this.ErrorContext.Error(ErrorCode.ERR_BadProtectedAccess, new ErrArg[] { swtBad, typeQual, symWhere });
		}

		// Token: 0x06000336 RID: 822 RVA: 0x00016186 File Offset: 0x00014386
		public bool CheckAccess(Symbol symCheck, AggregateType atsCheck, Symbol symWhere, CType typeThru)
		{
			return this.CheckAccess2(symCheck, atsCheck, symWhere, typeThru) == ACCESSERROR.ACCESSERROR_NOERROR;
		}
	}
}
