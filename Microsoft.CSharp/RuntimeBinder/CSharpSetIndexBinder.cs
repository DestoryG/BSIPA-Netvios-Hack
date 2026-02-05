using System;
using System.Collections.Generic;
using System.Dynamic;
using Microsoft.CSharp.RuntimeBinder.Semantics;

namespace Microsoft.CSharp.RuntimeBinder
{
	// Token: 0x02000016 RID: 22
	internal sealed class CSharpSetIndexBinder : SetIndexBinder, ICSharpBinder
	{
		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000094 RID: 148 RVA: 0x00003167 File Offset: 0x00001367
		public string Name
		{
			get
			{
				return "$Item$";
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000095 RID: 149 RVA: 0x0000316E File Offset: 0x0000136E
		public BindingFlag BindingFlags
		{
			get
			{
				return (BindingFlag)0;
			}
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00003171 File Offset: 0x00001371
		public Expr DispatchPayload(RuntimeBinder runtimeBinder, ArgumentObject[] arguments, LocalVariableSymbol[] locals)
		{
			return runtimeBinder.BindAssignment(this, arguments, locals);
		}

		// Token: 0x06000097 RID: 151 RVA: 0x0000317C File Offset: 0x0000137C
		public void PopulateSymbolTableWithName(SymbolTable symbolTable, Type callingType, ArgumentObject[] arguments)
		{
			symbolTable.PopulateSymbolTableWithName("$Item$", null, arguments[0].Type);
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000098 RID: 152 RVA: 0x00003196 File Offset: 0x00001396
		public bool IsBinderThatCanHaveRefReceiver
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000099 RID: 153 RVA: 0x00003199 File Offset: 0x00001399
		internal bool IsCompoundAssignment { get; }

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x0600009A RID: 154 RVA: 0x000031A1 File Offset: 0x000013A1
		public bool IsChecked { get; }

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x0600009B RID: 155 RVA: 0x000031A9 File Offset: 0x000013A9
		public Type CallingContext { get; }

		// Token: 0x0600009C RID: 156 RVA: 0x000031B1 File Offset: 0x000013B1
		CSharpArgumentInfo ICSharpBinder.GetArgumentInfo(int index)
		{
			return this._argumentInfo[index];
		}

		// Token: 0x0600009D RID: 157 RVA: 0x000031BB File Offset: 0x000013BB
		public CSharpSetIndexBinder(bool isCompoundAssignment, bool isChecked, Type callingContext, IEnumerable<CSharpArgumentInfo> argumentInfo)
			: base(BinderHelper.CreateCallInfo(argumentInfo, 2))
		{
			this.IsCompoundAssignment = isCompoundAssignment;
			this.IsChecked = isChecked;
			this.CallingContext = callingContext;
			this._argumentInfo = BinderHelper.ToArray<CSharpArgumentInfo>(argumentInfo);
			this._binder = RuntimeBinder.GetInstance();
		}

		// Token: 0x0600009E RID: 158 RVA: 0x000031F8 File Offset: 0x000013F8
		public override DynamicMetaObject FallbackSetIndex(DynamicMetaObject target, DynamicMetaObject[] indexes, DynamicMetaObject value, DynamicMetaObject errorSuggestion)
		{
			BinderHelper.ValidateBindArgument(target, "target");
			BinderHelper.ValidateBindArgument(indexes, "indexes");
			BinderHelper.ValidateBindArgument(value, "value");
			return BinderHelper.Bind(this, this._binder, BinderHelper.Cons<DynamicMetaObject>(target, indexes, value), this._argumentInfo, errorSuggestion);
		}

		// Token: 0x040000D6 RID: 214
		private readonly CSharpArgumentInfo[] _argumentInfo;

		// Token: 0x040000D7 RID: 215
		private readonly RuntimeBinder _binder;
	}
}
