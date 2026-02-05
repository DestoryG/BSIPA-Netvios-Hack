using System;
using Microsoft.CSharp.RuntimeBinder.Syntax;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x02000091 RID: 145
	internal sealed class ExprMemberGroup : ExprWithType, IExprWithObject
	{
		// Token: 0x060004AD RID: 1197 RVA: 0x000185AC File Offset: 0x000167AC
		public ExprMemberGroup(CType type, EXPRFLAG flags, Name name, TypeArray typeArgs, SYMKIND symKind, CType parentType, MethodOrPropertySymbol pMPS, Expr optionalObject, CMemberLookupResults memberLookupResults)
			: base(ExpressionKind.MemberGroup, type)
		{
			base.Flags = flags;
			this.Name = name;
			this.TypeArgs = typeArgs ?? BSYMMGR.EmptyTypeArray();
			this.SymKind = symKind;
			this.ParentType = parentType;
			this.OptionalObject = optionalObject;
			this.MemberLookupResults = memberLookupResults;
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x060004AE RID: 1198 RVA: 0x00018601 File Offset: 0x00016801
		public Name Name { get; }

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x060004AF RID: 1199 RVA: 0x00018609 File Offset: 0x00016809
		public TypeArray TypeArgs { get; }

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x060004B0 RID: 1200 RVA: 0x00018611 File Offset: 0x00016811
		public SYMKIND SymKind { get; }

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x060004B1 RID: 1201 RVA: 0x00018619 File Offset: 0x00016819
		// (set) Token: 0x060004B2 RID: 1202 RVA: 0x00018621 File Offset: 0x00016821
		public Expr OptionalObject { get; set; }

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x060004B3 RID: 1203 RVA: 0x0001862A File Offset: 0x0001682A
		// (set) Token: 0x060004B4 RID: 1204 RVA: 0x00018632 File Offset: 0x00016832
		public Expr OptionalLHS { get; set; }

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x060004B5 RID: 1205 RVA: 0x0001863B File Offset: 0x0001683B
		public CMemberLookupResults MemberLookupResults { get; }

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x060004B6 RID: 1206 RVA: 0x00018643 File Offset: 0x00016843
		public CType ParentType { get; }

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x060004B7 RID: 1207 RVA: 0x0001864B File Offset: 0x0001684B
		public bool IsDelegate
		{
			get
			{
				return (base.Flags & EXPRFLAG.EXF_GOTONOTBLOCKED) > (EXPRFLAG)0;
			}
		}
	}
}
