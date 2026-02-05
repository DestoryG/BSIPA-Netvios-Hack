using System;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x02000089 RID: 137
	internal sealed class ExpressionIterator
	{
		// Token: 0x06000493 RID: 1171 RVA: 0x000183F1 File Offset: 0x000165F1
		public ExpressionIterator(Expr pExpr)
		{
			this.Init(pExpr);
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x00018400 File Offset: 0x00016600
		public bool AtEnd()
		{
			return this._pCurrent == null && this._pList == null;
		}

		// Token: 0x06000495 RID: 1173 RVA: 0x00018415 File Offset: 0x00016615
		public Expr Current()
		{
			return this._pCurrent;
		}

		// Token: 0x06000496 RID: 1174 RVA: 0x0001841D File Offset: 0x0001661D
		public void MoveNext()
		{
			if (this.AtEnd())
			{
				return;
			}
			if (this._pList == null)
			{
				this._pCurrent = null;
				return;
			}
			this.Init(this._pList.OptionalNextListNode);
		}

		// Token: 0x06000497 RID: 1175 RVA: 0x0001844C File Offset: 0x0001664C
		public static int Count(Expr pExpr)
		{
			int num = 0;
			ExpressionIterator expressionIterator = new ExpressionIterator(pExpr);
			while (!expressionIterator.AtEnd())
			{
				num++;
				expressionIterator.MoveNext();
			}
			return num;
		}

		// Token: 0x06000498 RID: 1176 RVA: 0x00018478 File Offset: 0x00016678
		private void Init(Expr pExpr)
		{
			if (pExpr == null)
			{
				this._pList = null;
				this._pCurrent = null;
				return;
			}
			ExprList exprList;
			if ((exprList = pExpr as ExprList) != null)
			{
				this._pList = exprList;
				this._pCurrent = exprList.OptionalElement;
				return;
			}
			this._pList = null;
			this._pCurrent = pExpr;
		}

		// Token: 0x04000543 RID: 1347
		private ExprList _pList;

		// Token: 0x04000544 RID: 1348
		private Expr _pCurrent;
	}
}
