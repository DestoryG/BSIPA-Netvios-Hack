using System;
using System.Collections.Generic;
using System.Dynamic;
using Microsoft.CSharp.RuntimeBinder.Semantics;

namespace Microsoft.CSharp.RuntimeBinder
{
	// Token: 0x02000013 RID: 19
	internal sealed class CSharpInvokeConstructorBinder : DynamicMetaObjectBinder, ICSharpInvokeOrInvokeMemberBinder, ICSharpBinder
	{
		// Token: 0x17000029 RID: 41
		// (get) Token: 0x0600006B RID: 107 RVA: 0x00002EB9 File Offset: 0x000010B9
		public BindingFlag BindingFlags
		{
			get
			{
				return (BindingFlag)0;
			}
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00002EBC File Offset: 0x000010BC
		public Expr DispatchPayload(RuntimeBinder runtimeBinder, ArgumentObject[] arguments, LocalVariableSymbol[] locals)
		{
			return runtimeBinder.DispatchPayload(this, arguments, locals);
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00002EC7 File Offset: 0x000010C7
		public void PopulateSymbolTableWithName(SymbolTable symbolTable, Type callingType, ArgumentObject[] arguments)
		{
			RuntimeBinder.PopulateSymbolTableWithPayloadInformation(symbolTable, this, callingType, arguments);
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x0600006E RID: 110 RVA: 0x00002ED2 File Offset: 0x000010D2
		public bool IsBinderThatCanHaveRefReceiver
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x0600006F RID: 111 RVA: 0x00002ED5 File Offset: 0x000010D5
		public CSharpCallFlags Flags { get; }

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000070 RID: 112 RVA: 0x00002EDD File Offset: 0x000010DD
		public Type CallingContext { get; }

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000071 RID: 113 RVA: 0x00002EE5 File Offset: 0x000010E5
		public bool IsChecked
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00002EE8 File Offset: 0x000010E8
		CSharpArgumentInfo ICSharpBinder.GetArgumentInfo(int index)
		{
			return this._argumentInfo[index];
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000073 RID: 115 RVA: 0x00002EF2 File Offset: 0x000010F2
		public bool StaticCall
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000074 RID: 116 RVA: 0x00002EF5 File Offset: 0x000010F5
		public Type[] TypeArguments
		{
			get
			{
				return Array.Empty<Type>();
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000075 RID: 117 RVA: 0x00002EFC File Offset: 0x000010FC
		public string Name
		{
			get
			{
				return ".ctor";
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000076 RID: 118 RVA: 0x00002F03 File Offset: 0x00001103
		bool ICSharpInvokeOrInvokeMemberBinder.ResultDiscarded
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00002F06 File Offset: 0x00001106
		public CSharpInvokeConstructorBinder(CSharpCallFlags flags, Type callingContext, IEnumerable<CSharpArgumentInfo> argumentInfo)
		{
			this.Flags = flags;
			this.CallingContext = callingContext;
			this._argumentInfo = BinderHelper.ToArray<CSharpArgumentInfo>(argumentInfo);
			this._binder = RuntimeBinder.GetInstance();
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00002F33 File Offset: 0x00001133
		public override DynamicMetaObject Bind(DynamicMetaObject target, DynamicMetaObject[] args)
		{
			BinderHelper.ValidateBindArgument(target, "target");
			BinderHelper.ValidateBindArgument(args, "args");
			return BinderHelper.Bind(this, this._binder, BinderHelper.Cons<DynamicMetaObject>(target, args), this._argumentInfo, null);
		}

		// Token: 0x040000C9 RID: 201
		private readonly CSharpArgumentInfo[] _argumentInfo;

		// Token: 0x040000CA RID: 202
		private readonly RuntimeBinder _binder;
	}
}
