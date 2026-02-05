using System;
using System.Collections.Generic;
using System.Dynamic;
using Microsoft.CSharp.RuntimeBinder.Semantics;

namespace Microsoft.CSharp.RuntimeBinder
{
	// Token: 0x02000012 RID: 18
	internal sealed class CSharpInvokeBinder : InvokeBinder, ICSharpInvokeOrInvokeMemberBinder, ICSharpBinder
	{
		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600005D RID: 93 RVA: 0x00002DE4 File Offset: 0x00000FE4
		public BindingFlag BindingFlags
		{
			get
			{
				return (BindingFlag)0;
			}
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00002DE7 File Offset: 0x00000FE7
		public Expr DispatchPayload(RuntimeBinder runtimeBinder, ArgumentObject[] arguments, LocalVariableSymbol[] locals)
		{
			return runtimeBinder.DispatchPayload(this, arguments, locals);
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00002DF2 File Offset: 0x00000FF2
		public void PopulateSymbolTableWithName(SymbolTable symbolTable, Type callingType, ArgumentObject[] arguments)
		{
			RuntimeBinder.PopulateSymbolTableWithPayloadInformation(symbolTable, this, callingType, arguments);
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000060 RID: 96 RVA: 0x00002DFD File Offset: 0x00000FFD
		public bool IsBinderThatCanHaveRefReceiver
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000061 RID: 97 RVA: 0x00002E00 File Offset: 0x00001000
		bool ICSharpInvokeOrInvokeMemberBinder.StaticCall
		{
			get
			{
				return this._argumentInfo[0] != null && this._argumentInfo[0].IsStaticType;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000062 RID: 98 RVA: 0x00002E1B File Offset: 0x0000101B
		string ICSharpBinder.Name
		{
			get
			{
				return "Invoke";
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000063 RID: 99 RVA: 0x00002E22 File Offset: 0x00001022
		Type[] ICSharpInvokeOrInvokeMemberBinder.TypeArguments
		{
			get
			{
				return Array.Empty<Type>();
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000064 RID: 100 RVA: 0x00002E29 File Offset: 0x00001029
		CSharpCallFlags ICSharpInvokeOrInvokeMemberBinder.Flags
		{
			get
			{
				return this._flags;
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000065 RID: 101 RVA: 0x00002E31 File Offset: 0x00001031
		public Type CallingContext { get; }

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000066 RID: 102 RVA: 0x00002E39 File Offset: 0x00001039
		public bool IsChecked
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00002E3C File Offset: 0x0000103C
		CSharpArgumentInfo ICSharpBinder.GetArgumentInfo(int index)
		{
			return this._argumentInfo[index];
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000068 RID: 104 RVA: 0x00002E46 File Offset: 0x00001046
		bool ICSharpInvokeOrInvokeMemberBinder.ResultDiscarded
		{
			get
			{
				return (this._flags & CSharpCallFlags.ResultDiscarded) > CSharpCallFlags.None;
			}
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00002E53 File Offset: 0x00001053
		public CSharpInvokeBinder(CSharpCallFlags flags, Type callingContext, IEnumerable<CSharpArgumentInfo> argumentInfo)
			: base(BinderHelper.CreateCallInfo(argumentInfo, 1))
		{
			this._flags = flags;
			this.CallingContext = callingContext;
			this._argumentInfo = BinderHelper.ToArray<CSharpArgumentInfo>(argumentInfo);
			this._binder = RuntimeBinder.GetInstance();
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00002E87 File Offset: 0x00001087
		public override DynamicMetaObject FallbackInvoke(DynamicMetaObject target, DynamicMetaObject[] args, DynamicMetaObject errorSuggestion)
		{
			BinderHelper.ValidateBindArgument(target, "target");
			BinderHelper.ValidateBindArgument(args, "args");
			return BinderHelper.Bind(this, this._binder, BinderHelper.Cons<DynamicMetaObject>(target, args), this._argumentInfo, errorSuggestion);
		}

		// Token: 0x040000C3 RID: 195
		private readonly CSharpCallFlags _flags;

		// Token: 0x040000C5 RID: 197
		private readonly CSharpArgumentInfo[] _argumentInfo;

		// Token: 0x040000C6 RID: 198
		private readonly RuntimeBinder _binder;
	}
}
