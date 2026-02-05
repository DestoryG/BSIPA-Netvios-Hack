using System;
using Microsoft.CSharp.RuntimeBinder.Errors;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x02000053 RID: 83
	internal sealed class CNullable
	{
		// Token: 0x060002FE RID: 766 RVA: 0x00014BF9 File Offset: 0x00012DF9
		private SymbolLoader GetSymbolLoader()
		{
			return this._pSymbolLoader;
		}

		// Token: 0x060002FF RID: 767 RVA: 0x00014C01 File Offset: 0x00012E01
		private ExprFactory GetExprFactory()
		{
			return this._exprFactory;
		}

		// Token: 0x06000300 RID: 768 RVA: 0x00014C09 File Offset: 0x00012E09
		private ErrorHandling GetErrorContext()
		{
			return this._pErrorContext;
		}

		// Token: 0x06000301 RID: 769 RVA: 0x00014C14 File Offset: 0x00012E14
		private static bool IsNullableConstructor(Expr expr, out ExprCall call)
		{
			ExprCall exprCall;
			if ((exprCall = expr as ExprCall) != null && exprCall.MemberGroup.OptionalObject == null)
			{
				MethodSymbol methodSymbol = exprCall.MethWithInst.Meth();
				if (methodSymbol != null && methodSymbol.IsNullableConstructor())
				{
					call = exprCall;
					return true;
				}
			}
			call = null;
			return false;
		}

		// Token: 0x06000302 RID: 770 RVA: 0x00014C58 File Offset: 0x00012E58
		public static Expr StripNullableConstructor(Expr pExpr)
		{
			ExprCall exprCall;
			while (CNullable.IsNullableConstructor(pExpr, out exprCall))
			{
				pExpr = exprCall.OptionalArguments;
			}
			return pExpr;
		}

		// Token: 0x06000303 RID: 771 RVA: 0x00014C7C File Offset: 0x00012E7C
		public Expr BindValue(Expr exprSrc)
		{
			ExprCall exprCall;
			if (CNullable.IsNullableConstructor(exprSrc, out exprCall))
			{
				return exprCall.OptionalArguments;
			}
			NullableType nullableType = (NullableType)exprSrc.Type;
			CType underlyingType = nullableType.GetUnderlyingType();
			AggregateType ats = nullableType.GetAts();
			PropertySymbol propertySymbol = this.GetSymbolLoader().getBSymmgr().propNubValue;
			if (propertySymbol == null)
			{
				propertySymbol = this.GetSymbolLoader().getPredefinedMembers().GetProperty(PREDEFPROP.PP_G_OPTIONAL_VALUE);
				this.GetSymbolLoader().getBSymmgr().propNubValue = propertySymbol;
			}
			PropWithType propWithType = new PropWithType(propertySymbol, ats);
			MethPropWithInst methPropWithInst = new MethPropWithInst(propertySymbol, ats);
			ExprMemberGroup exprMemberGroup = this.GetExprFactory().CreateMemGroup(exprSrc, methPropWithInst);
			return this.GetExprFactory().CreateProperty(underlyingType, null, null, exprMemberGroup, propWithType, null);
		}

		// Token: 0x06000304 RID: 772 RVA: 0x00014D20 File Offset: 0x00012F20
		public ExprCall BindNew(Expr pExprSrc)
		{
			NullableType nullable = this.GetSymbolLoader().GetTypeManager().GetNullable(pExprSrc.Type);
			AggregateType ats = nullable.GetAts();
			MethodSymbol methodSymbol = this.GetSymbolLoader().getBSymmgr().methNubCtor;
			if (methodSymbol == null)
			{
				methodSymbol = this.GetSymbolLoader().getPredefinedMembers().GetMethod(PREDEFMETH.PM_G_OPTIONAL_CTOR);
				this.GetSymbolLoader().getBSymmgr().methNubCtor = methodSymbol;
			}
			MethWithInst methWithInst = new MethWithInst(methodSymbol, ats, BSYMMGR.EmptyTypeArray());
			ExprMemberGroup exprMemberGroup = this.GetExprFactory().CreateMemGroup(null, methWithInst);
			return this.GetExprFactory().CreateCall(EXPRFLAG.EXF_LITERALCONST | EXPRFLAG.EXF_CANTBENULL, nullable, pExprSrc, exprMemberGroup, methWithInst);
		}

		// Token: 0x06000305 RID: 773 RVA: 0x00014DB4 File Offset: 0x00012FB4
		public CNullable(SymbolLoader symbolLoader, ErrorHandling errorContext, ExprFactory exprFactory)
		{
			this._pSymbolLoader = symbolLoader;
			this._pErrorContext = errorContext;
			this._exprFactory = exprFactory;
		}

		// Token: 0x040003F9 RID: 1017
		private readonly SymbolLoader _pSymbolLoader;

		// Token: 0x040003FA RID: 1018
		private readonly ExprFactory _exprFactory;

		// Token: 0x040003FB RID: 1019
		private readonly ErrorHandling _pErrorContext;
	}
}
