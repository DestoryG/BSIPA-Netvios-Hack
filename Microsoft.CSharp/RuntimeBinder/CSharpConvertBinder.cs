using System;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using Microsoft.CSharp.RuntimeBinder.Semantics;

namespace Microsoft.CSharp.RuntimeBinder
{
	// Token: 0x0200000F RID: 15
	internal sealed class CSharpConvertBinder : ConvertBinder, ICSharpBinder
	{
		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600003C RID: 60 RVA: 0x00002BAF File Offset: 0x00000DAF
		[ExcludeFromCodeCoverage]
		public string Name
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600003D RID: 61 RVA: 0x00002BB2 File Offset: 0x00000DB2
		public BindingFlag BindingFlags
		{
			get
			{
				return (BindingFlag)0;
			}
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002BB5 File Offset: 0x00000DB5
		public Expr DispatchPayload(RuntimeBinder runtimeBinder, ArgumentObject[] arguments, LocalVariableSymbol[] locals)
		{
			if (!base.Explicit)
			{
				return runtimeBinder.BindImplicitConversion(arguments, base.Type, locals, this.ConversionKind == CSharpConversionKind.ArrayCreationConversion);
			}
			return runtimeBinder.BindExplicitConversion(arguments, base.Type, locals);
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002BE5 File Offset: 0x00000DE5
		public void PopulateSymbolTableWithName(SymbolTable symbolTable, Type callingType, ArgumentObject[] arguments)
		{
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000040 RID: 64 RVA: 0x00002BE7 File Offset: 0x00000DE7
		public bool IsBinderThatCanHaveRefReceiver
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00002BEA File Offset: 0x00000DEA
		CSharpArgumentInfo ICSharpBinder.GetArgumentInfo(int index)
		{
			return CSharpArgumentInfo.None;
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000042 RID: 66 RVA: 0x00002BF1 File Offset: 0x00000DF1
		private CSharpConversionKind ConversionKind { get; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000043 RID: 67 RVA: 0x00002BF9 File Offset: 0x00000DF9
		public bool IsChecked { get; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000044 RID: 68 RVA: 0x00002C01 File Offset: 0x00000E01
		public Type CallingContext { get; }

		// Token: 0x06000045 RID: 69 RVA: 0x00002C09 File Offset: 0x00000E09
		public CSharpConvertBinder(Type type, CSharpConversionKind conversionKind, bool isChecked, Type callingContext)
			: base(type, conversionKind == CSharpConversionKind.ExplicitConversion)
		{
			this.ConversionKind = conversionKind;
			this.IsChecked = isChecked;
			this.CallingContext = callingContext;
			this._binder = RuntimeBinder.GetInstance();
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00002C37 File Offset: 0x00000E37
		public override DynamicMetaObject FallbackConvert(DynamicMetaObject target, DynamicMetaObject errorSuggestion)
		{
			BinderHelper.ValidateBindArgument(target, "target");
			return BinderHelper.Bind(this, this._binder, new DynamicMetaObject[] { target }, null, errorSuggestion);
		}

		// Token: 0x040000BB RID: 187
		private readonly RuntimeBinder _binder;
	}
}
