using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.Linq.Expressions;
using Microsoft.CSharp.RuntimeBinder.Semantics;

namespace Microsoft.CSharp.RuntimeBinder
{
	// Token: 0x0200000A RID: 10
	internal sealed class CSharpBinaryOperationBinder : BinaryOperationBinder, ICSharpBinder
	{
		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000031 RID: 49 RVA: 0x00002AC2 File Offset: 0x00000CC2
		[ExcludeFromCodeCoverage]
		public string Name
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000032 RID: 50 RVA: 0x00002AC5 File Offset: 0x00000CC5
		public BindingFlag BindingFlags
		{
			get
			{
				return (BindingFlag)0;
			}
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00002AC8 File Offset: 0x00000CC8
		public Expr DispatchPayload(RuntimeBinder runtimeBinder, ArgumentObject[] arguments, LocalVariableSymbol[] locals)
		{
			return runtimeBinder.BindBinaryOperation(this, arguments, locals);
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002AD4 File Offset: 0x00000CD4
		public void PopulateSymbolTableWithName(SymbolTable symbolTable, Type callingType, ArgumentObject[] arguments)
		{
			string clroperatorName = base.Operation.GetCLROperatorName();
			symbolTable.PopulateSymbolTableWithName(clroperatorName, null, arguments[0].Type);
			symbolTable.PopulateSymbolTableWithName(clroperatorName, null, arguments[1].Type);
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000035 RID: 53 RVA: 0x00002B15 File Offset: 0x00000D15
		public bool IsBinderThatCanHaveRefReceiver
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000036 RID: 54 RVA: 0x00002B18 File Offset: 0x00000D18
		public bool IsChecked { get; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000037 RID: 55 RVA: 0x00002B20 File Offset: 0x00000D20
		internal bool IsLogicalOperation
		{
			get
			{
				return (this._binopFlags & CSharpBinaryOperationFlags.LogicalOperation) > CSharpBinaryOperationFlags.None;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000038 RID: 56 RVA: 0x00002B2D File Offset: 0x00000D2D
		public Type CallingContext { get; }

		// Token: 0x06000039 RID: 57 RVA: 0x00002B35 File Offset: 0x00000D35
		CSharpArgumentInfo ICSharpBinder.GetArgumentInfo(int index)
		{
			return this._argumentInfo[index];
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002B3F File Offset: 0x00000D3F
		public CSharpBinaryOperationBinder(ExpressionType operation, bool isChecked, CSharpBinaryOperationFlags binaryOperationFlags, Type callingContext, IEnumerable<CSharpArgumentInfo> argumentInfo)
			: base(operation)
		{
			this.IsChecked = isChecked;
			this._binopFlags = binaryOperationFlags;
			this.CallingContext = callingContext;
			this._argumentInfo = BinderHelper.ToArray<CSharpArgumentInfo>(argumentInfo);
			this._binder = RuntimeBinder.GetInstance();
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00002B76 File Offset: 0x00000D76
		public override DynamicMetaObject FallbackBinaryOperation(DynamicMetaObject target, DynamicMetaObject arg, DynamicMetaObject errorSuggestion)
		{
			BinderHelper.ValidateBindArgument(target, "target");
			BinderHelper.ValidateBindArgument(arg, "arg");
			return BinderHelper.Bind(this, this._binder, new DynamicMetaObject[] { target, arg }, this._argumentInfo, errorSuggestion);
		}

		// Token: 0x0400009C RID: 156
		private readonly CSharpBinaryOperationFlags _binopFlags;

		// Token: 0x0400009E RID: 158
		private readonly CSharpArgumentInfo[] _argumentInfo;

		// Token: 0x0400009F RID: 159
		private readonly RuntimeBinder _binder;
	}
}
