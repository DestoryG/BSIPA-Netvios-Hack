using System;

namespace Microsoft.CSharp.RuntimeBinder
{
	// Token: 0x02000019 RID: 25
	internal static class Error
	{
		// Token: 0x060000B4 RID: 180 RVA: 0x000033A5 File Offset: 0x000015A5
		internal static Exception InternalCompilerError()
		{
			return new RuntimeBinderInternalCompilerException("An unexpected exception occurred while binding a dynamic operation");
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x000033B1 File Offset: 0x000015B1
		internal static Exception BindRequireArguments()
		{
			return new ArgumentException("Cannot bind call with no calling object");
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x000033BD File Offset: 0x000015BD
		internal static Exception BindCallFailedOverloadResolution()
		{
			return new RuntimeBinderException("Overload resolution failed");
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x000033C9 File Offset: 0x000015C9
		internal static Exception BindBinaryOperatorRequireTwoArguments()
		{
			return new ArgumentException("Binary operators must be invoked with two arguments");
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x000033D5 File Offset: 0x000015D5
		internal static Exception BindUnaryOperatorRequireOneArgument()
		{
			return new ArgumentException("Unary operators must be invoked with one argument");
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x000033E1 File Offset: 0x000015E1
		internal static Exception BindBinaryAssignmentRequireTwoArguments()
		{
			return new ArgumentException("Binary operators cannot be invoked with one argument");
		}

		// Token: 0x060000BA RID: 186 RVA: 0x000033ED File Offset: 0x000015ED
		internal static Exception BindPropertyFailedMethodGroup(object p0)
		{
			return new RuntimeBinderException(SR.Format("The name '{0}' is bound to a method and cannot be used like a property", p0));
		}

		// Token: 0x060000BB RID: 187 RVA: 0x000033FF File Offset: 0x000015FF
		internal static Exception BindPropertyFailedEvent(object p0)
		{
			return new RuntimeBinderException(SR.Format("The event '{0}' can only appear on the left hand side of +", p0));
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00003411 File Offset: 0x00001611
		internal static Exception BindInvokeFailedNonDelegate()
		{
			return new RuntimeBinderException("Cannot invoke a non-delegate type");
		}

		// Token: 0x060000BD RID: 189 RVA: 0x0000341D File Offset: 0x0000161D
		internal static Exception BindStaticRequiresType(string paramName)
		{
			return new ArgumentException("The first argument to dynamically-bound static or constructor call must be a Type", paramName);
		}

		// Token: 0x060000BE RID: 190 RVA: 0x0000342A File Offset: 0x0000162A
		internal static Exception BindBinaryAssignmentFailedNullReference()
		{
			return new RuntimeBinderException("Cannot perform member assignment on a null reference");
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00003436 File Offset: 0x00001636
		internal static Exception NullReferenceOnMemberException()
		{
			return new RuntimeBinderException("Cannot perform runtime binding on a null reference");
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00003442 File Offset: 0x00001642
		internal static Exception BindCallToConditionalMethod(object p0)
		{
			return new RuntimeBinderException(SR.Format("Cannot dynamically invoke method '{0}' because it has a Conditional attribute", p0));
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00003454 File Offset: 0x00001654
		internal static Exception BindToVoidMethodButExpectResult()
		{
			return new RuntimeBinderException("Cannot implicitly convert type 'void' to 'object'");
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00003460 File Offset: 0x00001660
		internal static Exception ArgumentNull(string paramName)
		{
			return new ArgumentNullException(paramName);
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00003468 File Offset: 0x00001668
		internal static Exception DynamicArgumentNeedsValue(string paramName)
		{
			return new ArgumentException("The runtime binder cannot bind a metaobject without a value", paramName);
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00003475 File Offset: 0x00001675
		internal static Exception BindingNameCollision()
		{
			return new RuntimeBinderException("More than one type in the binding has the same full name.");
		}
	}
}
