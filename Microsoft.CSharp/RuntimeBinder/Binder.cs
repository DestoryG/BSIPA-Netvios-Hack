using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Microsoft.CSharp.RuntimeBinder
{
	// Token: 0x02000006 RID: 6
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static class Binder
	{
		// Token: 0x0600000B RID: 11 RVA: 0x000020D0 File Offset: 0x000002D0
		public static CallSiteBinder BinaryOperation(CSharpBinderFlags flags, ExpressionType operation, Type context, IEnumerable<CSharpArgumentInfo> argumentInfo)
		{
			bool flag = (flags & CSharpBinderFlags.CheckedContext) > CSharpBinderFlags.None;
			bool flag2 = (flags & CSharpBinderFlags.BinaryOperationLogical) > CSharpBinderFlags.None;
			CSharpBinaryOperationFlags csharpBinaryOperationFlags = CSharpBinaryOperationFlags.None;
			if (flag2)
			{
				csharpBinaryOperationFlags |= CSharpBinaryOperationFlags.LogicalOperation;
			}
			return new CSharpBinaryOperationBinder(operation, flag, csharpBinaryOperationFlags, context, argumentInfo);
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000020FC File Offset: 0x000002FC
		public static CallSiteBinder Convert(CSharpBinderFlags flags, Type type, Type context)
		{
			CSharpConversionKind csharpConversionKind = (((flags & CSharpBinderFlags.ConvertExplicit) != CSharpBinderFlags.None) ? CSharpConversionKind.ExplicitConversion : (((flags & CSharpBinderFlags.ConvertArrayIndex) != CSharpBinderFlags.None) ? CSharpConversionKind.ArrayCreationConversion : CSharpConversionKind.ImplicitConversion));
			bool flag = (flags & CSharpBinderFlags.CheckedContext) > CSharpBinderFlags.None;
			return new CSharpConvertBinder(type, csharpConversionKind, flag, context);
		}

		// Token: 0x0600000D RID: 13 RVA: 0x0000212D File Offset: 0x0000032D
		public static CallSiteBinder GetIndex(CSharpBinderFlags flags, Type context, IEnumerable<CSharpArgumentInfo> argumentInfo)
		{
			return new CSharpGetIndexBinder(context, argumentInfo);
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002138 File Offset: 0x00000338
		public static CallSiteBinder GetMember(CSharpBinderFlags flags, string name, Type context, IEnumerable<CSharpArgumentInfo> argumentInfo)
		{
			bool flag = (flags & CSharpBinderFlags.ResultIndexed) > CSharpBinderFlags.None;
			return new CSharpGetMemberBinder(name, flag, context, argumentInfo);
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002158 File Offset: 0x00000358
		public static CallSiteBinder Invoke(CSharpBinderFlags flags, Type context, IEnumerable<CSharpArgumentInfo> argumentInfo)
		{
			bool flag = (flags & CSharpBinderFlags.ResultDiscarded) > CSharpBinderFlags.None;
			CSharpCallFlags csharpCallFlags = CSharpCallFlags.None;
			if (flag)
			{
				csharpCallFlags |= CSharpCallFlags.ResultDiscarded;
			}
			return new CSharpInvokeBinder(csharpCallFlags, context, argumentInfo);
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002180 File Offset: 0x00000380
		public static CallSiteBinder InvokeMember(CSharpBinderFlags flags, string name, IEnumerable<Type> typeArguments, Type context, IEnumerable<CSharpArgumentInfo> argumentInfo)
		{
			bool flag = (flags & CSharpBinderFlags.InvokeSimpleName) > CSharpBinderFlags.None;
			bool flag2 = (flags & CSharpBinderFlags.InvokeSpecialName) > CSharpBinderFlags.None;
			bool flag3 = (flags & CSharpBinderFlags.ResultDiscarded) > CSharpBinderFlags.None;
			CSharpCallFlags csharpCallFlags = CSharpCallFlags.None;
			if (flag)
			{
				csharpCallFlags |= CSharpCallFlags.SimpleNameCall;
			}
			if (flag2)
			{
				csharpCallFlags |= CSharpCallFlags.EventHookup;
			}
			if (flag3)
			{
				csharpCallFlags |= CSharpCallFlags.ResultDiscarded;
			}
			return new CSharpInvokeMemberBinder(csharpCallFlags, name, context, typeArguments, argumentInfo);
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000021C6 File Offset: 0x000003C6
		public static CallSiteBinder InvokeConstructor(CSharpBinderFlags flags, Type context, IEnumerable<CSharpArgumentInfo> argumentInfo)
		{
			return new CSharpInvokeConstructorBinder(CSharpCallFlags.None, context, argumentInfo);
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000021D0 File Offset: 0x000003D0
		public static CallSiteBinder IsEvent(CSharpBinderFlags flags, string name, Type context)
		{
			return new CSharpIsEventBinder(name, context);
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000021DC File Offset: 0x000003DC
		public static CallSiteBinder SetIndex(CSharpBinderFlags flags, Type context, IEnumerable<CSharpArgumentInfo> argumentInfo)
		{
			bool flag = (flags & CSharpBinderFlags.ValueFromCompoundAssignment) > CSharpBinderFlags.None;
			bool flag2 = (flags & CSharpBinderFlags.CheckedContext) > CSharpBinderFlags.None;
			return new CSharpSetIndexBinder(flag, flag2, context, argumentInfo);
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002204 File Offset: 0x00000404
		public static CallSiteBinder SetMember(CSharpBinderFlags flags, string name, Type context, IEnumerable<CSharpArgumentInfo> argumentInfo)
		{
			bool flag = (flags & CSharpBinderFlags.ValueFromCompoundAssignment) > CSharpBinderFlags.None;
			bool flag2 = (flags & CSharpBinderFlags.CheckedContext) > CSharpBinderFlags.None;
			return new CSharpSetMemberBinder(name, flag, flag2, context, argumentInfo);
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002230 File Offset: 0x00000430
		public static CallSiteBinder UnaryOperation(CSharpBinderFlags flags, ExpressionType operation, Type context, IEnumerable<CSharpArgumentInfo> argumentInfo)
		{
			bool flag = (flags & CSharpBinderFlags.CheckedContext) > CSharpBinderFlags.None;
			return new CSharpUnaryOperationBinder(operation, flag, context, argumentInfo);
		}
	}
}
