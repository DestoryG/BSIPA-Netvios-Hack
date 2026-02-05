using System;
using System.Collections.Generic;
using System.Dynamic;
using Microsoft.CSharp.RuntimeBinder.Semantics;

namespace Microsoft.CSharp.RuntimeBinder
{
	// Token: 0x02000011 RID: 17
	internal sealed class CSharpGetMemberBinder : GetMemberBinder, IInvokeOnGetBinder, ICSharpBinder
	{
		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000051 RID: 81 RVA: 0x00002D25 File Offset: 0x00000F25
		public BindingFlag BindingFlags
		{
			get
			{
				return BindingFlag.BIND_RVALUEREQUIRED;
			}
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00002D28 File Offset: 0x00000F28
		public Expr DispatchPayload(RuntimeBinder runtimeBinder, ArgumentObject[] arguments, LocalVariableSymbol[] locals)
		{
			return runtimeBinder.BindProperty(this, arguments[0], locals[0], null);
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00002D3C File Offset: 0x00000F3C
		public void PopulateSymbolTableWithName(SymbolTable symbolTable, Type callingType, ArgumentObject[] arguments)
		{
			symbolTable.PopulateSymbolTableWithName(base.Name, null, arguments[0].Type);
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000054 RID: 84 RVA: 0x00002D57 File Offset: 0x00000F57
		public bool IsBinderThatCanHaveRefReceiver
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000055 RID: 85 RVA: 0x00002D5A File Offset: 0x00000F5A
		public Type CallingContext { get; }

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000056 RID: 86 RVA: 0x00002D62 File Offset: 0x00000F62
		public bool IsChecked
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00002D65 File Offset: 0x00000F65
		CSharpArgumentInfo ICSharpBinder.GetArgumentInfo(int index)
		{
			return this._argumentInfo[index];
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000058 RID: 88 RVA: 0x00002D6F File Offset: 0x00000F6F
		bool IInvokeOnGetBinder.InvokeOnGet
		{
			get
			{
				return !this.ResultIndexed;
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000059 RID: 89 RVA: 0x00002D7A File Offset: 0x00000F7A
		private bool ResultIndexed { get; }

		// Token: 0x0600005A RID: 90 RVA: 0x00002D82 File Offset: 0x00000F82
		public CSharpGetMemberBinder(string name, bool resultIndexed, Type callingContext, IEnumerable<CSharpArgumentInfo> argumentInfo)
			: base(name, false)
		{
			this.ResultIndexed = resultIndexed;
			this.CallingContext = callingContext;
			this._argumentInfo = BinderHelper.ToArray<CSharpArgumentInfo>(argumentInfo);
			this._binder = RuntimeBinder.GetInstance();
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00002DB2 File Offset: 0x00000FB2
		public override DynamicMetaObject FallbackGetMember(DynamicMetaObject target, DynamicMetaObject errorSuggestion)
		{
			BinderHelper.ValidateBindArgument(target, "target");
			return BinderHelper.Bind(this, this._binder, new DynamicMetaObject[] { target }, this._argumentInfo, errorSuggestion);
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00002DDC File Offset: 0x00000FDC
		string ICSharpBinder.get_Name()
		{
			return base.Name;
		}

		// Token: 0x040000C0 RID: 192
		private readonly CSharpArgumentInfo[] _argumentInfo;

		// Token: 0x040000C2 RID: 194
		private readonly RuntimeBinder _binder;
	}
}
