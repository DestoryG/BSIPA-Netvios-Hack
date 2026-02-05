using System;
using System.Collections.Generic;
using System.Dynamic;
using Microsoft.CSharp.RuntimeBinder.Semantics;

namespace Microsoft.CSharp.RuntimeBinder
{
	// Token: 0x02000014 RID: 20
	internal sealed class CSharpInvokeMemberBinder : InvokeMemberBinder, ICSharpInvokeOrInvokeMemberBinder, ICSharpBinder
	{
		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000079 RID: 121 RVA: 0x00002F65 File Offset: 0x00001165
		public BindingFlag BindingFlags
		{
			get
			{
				return (BindingFlag)0;
			}
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00002F68 File Offset: 0x00001168
		public Expr DispatchPayload(RuntimeBinder runtimeBinder, ArgumentObject[] arguments, LocalVariableSymbol[] locals)
		{
			return runtimeBinder.DispatchPayload(this, arguments, locals);
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00002F73 File Offset: 0x00001173
		public void PopulateSymbolTableWithName(SymbolTable symbolTable, Type callingType, ArgumentObject[] arguments)
		{
			RuntimeBinder.PopulateSymbolTableWithPayloadInformation(symbolTable, this, callingType, arguments);
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x0600007C RID: 124 RVA: 0x00002F7E File Offset: 0x0000117E
		public bool IsBinderThatCanHaveRefReceiver
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x0600007D RID: 125 RVA: 0x00002F81 File Offset: 0x00001181
		bool ICSharpInvokeOrInvokeMemberBinder.StaticCall
		{
			get
			{
				CSharpArgumentInfo csharpArgumentInfo = this._argumentInfo[0];
				return csharpArgumentInfo != null && csharpArgumentInfo.IsStaticType;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x0600007E RID: 126 RVA: 0x00002F96 File Offset: 0x00001196
		public CSharpCallFlags Flags { get; }

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x0600007F RID: 127 RVA: 0x00002F9E File Offset: 0x0000119E
		public Type CallingContext { get; }

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000080 RID: 128 RVA: 0x00002FA6 File Offset: 0x000011A6
		public bool IsChecked
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000081 RID: 129 RVA: 0x00002FA9 File Offset: 0x000011A9
		public Type[] TypeArguments { get; }

		// Token: 0x06000082 RID: 130 RVA: 0x00002FB1 File Offset: 0x000011B1
		public CSharpArgumentInfo GetArgumentInfo(int index)
		{
			return this._argumentInfo[index];
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00002FBC File Offset: 0x000011BC
		public CSharpArgumentInfo[] ArgumentInfoArray()
		{
			CSharpArgumentInfo[] array = new CSharpArgumentInfo[this._argumentInfo.Length];
			this._argumentInfo.CopyTo(array, 0);
			return array;
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000084 RID: 132 RVA: 0x00002FE5 File Offset: 0x000011E5
		bool ICSharpInvokeOrInvokeMemberBinder.ResultDiscarded
		{
			get
			{
				return (this.Flags & CSharpCallFlags.ResultDiscarded) > CSharpCallFlags.None;
			}
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00002FF4 File Offset: 0x000011F4
		public CSharpInvokeMemberBinder(CSharpCallFlags flags, string name, Type callingContext, IEnumerable<Type> typeArguments, IEnumerable<CSharpArgumentInfo> argumentInfo)
			: base(name, false, BinderHelper.CreateCallInfo(argumentInfo, 1))
		{
			this.Flags = flags;
			this.CallingContext = callingContext;
			this.TypeArguments = BinderHelper.ToArray<Type>(typeArguments);
			this._argumentInfo = BinderHelper.ToArray<CSharpArgumentInfo>(argumentInfo);
			this._binder = RuntimeBinder.GetInstance();
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00003044 File Offset: 0x00001244
		public override DynamicMetaObject FallbackInvokeMember(DynamicMetaObject target, DynamicMetaObject[] args, DynamicMetaObject errorSuggestion)
		{
			BinderHelper.ValidateBindArgument(target, "target");
			BinderHelper.ValidateBindArgument(args, "args");
			return BinderHelper.Bind(this, this._binder, BinderHelper.Cons<DynamicMetaObject>(target, args), this._argumentInfo, errorSuggestion);
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00003076 File Offset: 0x00001276
		public override DynamicMetaObject FallbackInvoke(DynamicMetaObject target, DynamicMetaObject[] args, DynamicMetaObject errorSuggestion)
		{
			return new CSharpInvokeBinder(this.Flags, this.CallingContext, this._argumentInfo).Defer(target, args);
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00003096 File Offset: 0x00001296
		string ICSharpBinder.get_Name()
		{
			return base.Name;
		}

		// Token: 0x040000CE RID: 206
		private readonly CSharpArgumentInfo[] _argumentInfo;

		// Token: 0x040000CF RID: 207
		private readonly RuntimeBinder _binder;
	}
}
