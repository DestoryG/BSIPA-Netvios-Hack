using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.Linq.Expressions;
using Microsoft.CSharp.RuntimeBinder.Semantics;

namespace Microsoft.CSharp.RuntimeBinder
{
	// Token: 0x02000018 RID: 24
	internal sealed class CSharpUnaryOperationBinder : UnaryOperationBinder, ICSharpBinder
	{
		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060000AA RID: 170 RVA: 0x000032FE File Offset: 0x000014FE
		[ExcludeFromCodeCoverage]
		public string Name
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060000AB RID: 171 RVA: 0x00003301 File Offset: 0x00001501
		public BindingFlag BindingFlags
		{
			get
			{
				return (BindingFlag)0;
			}
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00003304 File Offset: 0x00001504
		public Expr DispatchPayload(RuntimeBinder runtimeBinder, ArgumentObject[] arguments, LocalVariableSymbol[] locals)
		{
			return runtimeBinder.BindUnaryOperation(this, arguments, locals);
		}

		// Token: 0x060000AD RID: 173 RVA: 0x0000330F File Offset: 0x0000150F
		public void PopulateSymbolTableWithName(SymbolTable symbolTable, Type callingType, ArgumentObject[] arguments)
		{
			symbolTable.PopulateSymbolTableWithName(base.Operation.GetCLROperatorName(), null, arguments[0].Type);
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060000AE RID: 174 RVA: 0x0000332F File Offset: 0x0000152F
		public bool IsBinderThatCanHaveRefReceiver
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060000AF RID: 175 RVA: 0x00003332 File Offset: 0x00001532
		public bool IsChecked { get; }

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060000B0 RID: 176 RVA: 0x0000333A File Offset: 0x0000153A
		public Type CallingContext { get; }

		// Token: 0x060000B1 RID: 177 RVA: 0x00003342 File Offset: 0x00001542
		CSharpArgumentInfo ICSharpBinder.GetArgumentInfo(int index)
		{
			return this._argumentInfo[index];
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x0000334C File Offset: 0x0000154C
		public CSharpUnaryOperationBinder(ExpressionType operation, bool isChecked, Type callingContext, IEnumerable<CSharpArgumentInfo> argumentInfo)
			: base(operation)
		{
			this.IsChecked = isChecked;
			this.CallingContext = callingContext;
			this._argumentInfo = BinderHelper.ToArray<CSharpArgumentInfo>(argumentInfo);
			this._binder = RuntimeBinder.GetInstance();
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x0000337B File Offset: 0x0000157B
		public override DynamicMetaObject FallbackUnaryOperation(DynamicMetaObject target, DynamicMetaObject errorSuggestion)
		{
			BinderHelper.ValidateBindArgument(target, "target");
			return BinderHelper.Bind(this, this._binder, new DynamicMetaObject[] { target }, this._argumentInfo, errorSuggestion);
		}

		// Token: 0x040000DF RID: 223
		private readonly CSharpArgumentInfo[] _argumentInfo;

		// Token: 0x040000E0 RID: 224
		private readonly RuntimeBinder _binder;
	}
}
