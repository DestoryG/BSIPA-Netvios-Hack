using System;
using System.Collections.Generic;
using System.Dynamic;
using Microsoft.CSharp.RuntimeBinder.Semantics;

namespace Microsoft.CSharp.RuntimeBinder
{
	// Token: 0x02000017 RID: 23
	internal sealed class CSharpSetMemberBinder : SetMemberBinder, ICSharpBinder
	{
		// Token: 0x17000046 RID: 70
		// (get) Token: 0x0600009F RID: 159 RVA: 0x00003237 File Offset: 0x00001437
		public BindingFlag BindingFlags
		{
			get
			{
				return (BindingFlag)0;
			}
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x0000323A File Offset: 0x0000143A
		public Expr DispatchPayload(RuntimeBinder runtimeBinder, ArgumentObject[] arguments, LocalVariableSymbol[] locals)
		{
			return runtimeBinder.BindAssignment(this, arguments, locals);
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00003245 File Offset: 0x00001445
		public void PopulateSymbolTableWithName(SymbolTable symbolTable, Type callingType, ArgumentObject[] arguments)
		{
			symbolTable.PopulateSymbolTableWithName(base.Name, null, arguments[0].Type);
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060000A2 RID: 162 RVA: 0x00003260 File Offset: 0x00001460
		public bool IsBinderThatCanHaveRefReceiver
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060000A3 RID: 163 RVA: 0x00003263 File Offset: 0x00001463
		internal bool IsCompoundAssignment { get; }

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060000A4 RID: 164 RVA: 0x0000326B File Offset: 0x0000146B
		public bool IsChecked { get; }

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060000A5 RID: 165 RVA: 0x00003273 File Offset: 0x00001473
		public Type CallingContext { get; }

		// Token: 0x060000A6 RID: 166 RVA: 0x0000327B File Offset: 0x0000147B
		CSharpArgumentInfo ICSharpBinder.GetArgumentInfo(int index)
		{
			return this._argumentInfo[index];
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00003285 File Offset: 0x00001485
		public CSharpSetMemberBinder(string name, bool isCompoundAssignment, bool isChecked, Type callingContext, IEnumerable<CSharpArgumentInfo> argumentInfo)
			: base(name, false)
		{
			this.IsCompoundAssignment = isCompoundAssignment;
			this.IsChecked = isChecked;
			this.CallingContext = callingContext;
			this._argumentInfo = BinderHelper.ToArray<CSharpArgumentInfo>(argumentInfo);
			this._binder = RuntimeBinder.GetInstance();
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x000032BD File Offset: 0x000014BD
		public override DynamicMetaObject FallbackSetMember(DynamicMetaObject target, DynamicMetaObject value, DynamicMetaObject errorSuggestion)
		{
			BinderHelper.ValidateBindArgument(target, "target");
			BinderHelper.ValidateBindArgument(value, "value");
			return BinderHelper.Bind(this, this._binder, new DynamicMetaObject[] { target, value }, this._argumentInfo, errorSuggestion);
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x000032F6 File Offset: 0x000014F6
		string ICSharpBinder.get_Name()
		{
			return base.Name;
		}

		// Token: 0x040000DB RID: 219
		private readonly CSharpArgumentInfo[] _argumentInfo;

		// Token: 0x040000DC RID: 220
		private readonly RuntimeBinder _binder;
	}
}
