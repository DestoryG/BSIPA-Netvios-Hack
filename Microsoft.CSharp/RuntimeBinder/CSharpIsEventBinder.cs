using System;
using System.Dynamic;
using Microsoft.CSharp.RuntimeBinder.Semantics;

namespace Microsoft.CSharp.RuntimeBinder
{
	// Token: 0x02000015 RID: 21
	internal sealed class CSharpIsEventBinder : DynamicMetaObjectBinder, ICSharpBinder
	{
		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000089 RID: 137 RVA: 0x0000309E File Offset: 0x0000129E
		public BindingFlag BindingFlags
		{
			get
			{
				return (BindingFlag)0;
			}
		}

		// Token: 0x0600008A RID: 138 RVA: 0x000030A1 File Offset: 0x000012A1
		public Expr DispatchPayload(RuntimeBinder runtimeBinder, ArgumentObject[] arguments, LocalVariableSymbol[] locals)
		{
			return runtimeBinder.BindIsEvent(this, arguments, locals);
		}

		// Token: 0x0600008B RID: 139 RVA: 0x000030AC File Offset: 0x000012AC
		public void PopulateSymbolTableWithName(SymbolTable symbolTable, Type callingType, ArgumentObject[] arguments)
		{
			symbolTable.PopulateSymbolTableWithName(this.Name, null, arguments[0].Info.IsStaticType ? (arguments[0].Value as Type) : arguments[0].Type);
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x0600008C RID: 140 RVA: 0x000030F8 File Offset: 0x000012F8
		public bool IsBinderThatCanHaveRefReceiver
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600008D RID: 141 RVA: 0x000030FB File Offset: 0x000012FB
		CSharpArgumentInfo ICSharpBinder.GetArgumentInfo(int index)
		{
			return CSharpArgumentInfo.None;
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x0600008E RID: 142 RVA: 0x00003102 File Offset: 0x00001302
		public string Name { get; }

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x0600008F RID: 143 RVA: 0x0000310A File Offset: 0x0000130A
		public Type CallingContext { get; }

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000090 RID: 144 RVA: 0x00003112 File Offset: 0x00001312
		public bool IsChecked
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00003115 File Offset: 0x00001315
		public CSharpIsEventBinder(string name, Type callingContext)
		{
			this.Name = name;
			this.CallingContext = callingContext;
			this._binder = RuntimeBinder.GetInstance();
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000092 RID: 146 RVA: 0x00003136 File Offset: 0x00001336
		public override Type ReturnType
		{
			get
			{
				return typeof(bool);
			}
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00003142 File Offset: 0x00001342
		public override DynamicMetaObject Bind(DynamicMetaObject target, DynamicMetaObject[] args)
		{
			BinderHelper.ValidateBindArgument(target, "target");
			return BinderHelper.Bind(this, this._binder, new DynamicMetaObject[] { target }, null, null);
		}

		// Token: 0x040000D2 RID: 210
		private readonly RuntimeBinder _binder;
	}
}
