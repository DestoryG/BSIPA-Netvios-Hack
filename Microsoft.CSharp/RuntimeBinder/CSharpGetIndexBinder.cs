using System;
using System.Collections.Generic;
using System.Dynamic;
using Microsoft.CSharp.RuntimeBinder.Semantics;

namespace Microsoft.CSharp.RuntimeBinder
{
	// Token: 0x02000010 RID: 16
	internal sealed class CSharpGetIndexBinder : GetIndexBinder, ICSharpBinder
	{
		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000047 RID: 71 RVA: 0x00002C5C File Offset: 0x00000E5C
		public string Name
		{
			get
			{
				return "$Item$";
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000048 RID: 72 RVA: 0x00002C63 File Offset: 0x00000E63
		public BindingFlag BindingFlags
		{
			get
			{
				return BindingFlag.BIND_RVALUEREQUIRED;
			}
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00002C68 File Offset: 0x00000E68
		public Expr DispatchPayload(RuntimeBinder runtimeBinder, ArgumentObject[] arguments, LocalVariableSymbol[] locals)
		{
			Expr expr = runtimeBinder.CreateArgumentListEXPR(arguments, locals, 1, arguments.Length);
			return runtimeBinder.BindProperty(this, arguments[0], locals[0], expr);
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00002C94 File Offset: 0x00000E94
		public void PopulateSymbolTableWithName(SymbolTable symbolTable, Type callingType, ArgumentObject[] arguments)
		{
			symbolTable.PopulateSymbolTableWithName("$Item$", null, arguments[0].Type);
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600004B RID: 75 RVA: 0x00002CAE File Offset: 0x00000EAE
		public bool IsBinderThatCanHaveRefReceiver
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600004C RID: 76 RVA: 0x00002CB1 File Offset: 0x00000EB1
		public Type CallingContext { get; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600004D RID: 77 RVA: 0x00002CB9 File Offset: 0x00000EB9
		public bool IsChecked
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00002CBC File Offset: 0x00000EBC
		CSharpArgumentInfo ICSharpBinder.GetArgumentInfo(int index)
		{
			return this._argumentInfo[index];
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00002CC6 File Offset: 0x00000EC6
		public CSharpGetIndexBinder(Type callingContext, IEnumerable<CSharpArgumentInfo> argumentInfo)
			: base(BinderHelper.CreateCallInfo(argumentInfo, 1))
		{
			this.CallingContext = callingContext;
			this._argumentInfo = BinderHelper.ToArray<CSharpArgumentInfo>(argumentInfo);
			this._binder = RuntimeBinder.GetInstance();
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00002CF3 File Offset: 0x00000EF3
		public override DynamicMetaObject FallbackGetIndex(DynamicMetaObject target, DynamicMetaObject[] indexes, DynamicMetaObject errorSuggestion)
		{
			BinderHelper.ValidateBindArgument(target, "target");
			BinderHelper.ValidateBindArgument(indexes, "indexes");
			return BinderHelper.Bind(this, this._binder, BinderHelper.Cons<DynamicMetaObject>(target, indexes), this._argumentInfo, errorSuggestion);
		}

		// Token: 0x040000BD RID: 189
		private readonly CSharpArgumentInfo[] _argumentInfo;

		// Token: 0x040000BE RID: 190
		private readonly RuntimeBinder _binder;
	}
}
