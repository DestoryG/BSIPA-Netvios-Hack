using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Microsoft.CSharp.RuntimeBinder
{
	// Token: 0x02000007 RID: 7
	internal static class BinderHelper
	{
		// Token: 0x06000016 RID: 22 RVA: 0x00002250 File Offset: 0x00000450
		internal static DynamicMetaObject Bind(DynamicMetaObjectBinder action, RuntimeBinder binder, DynamicMetaObject[] args, IEnumerable<CSharpArgumentInfo> arginfos, DynamicMetaObject onBindingError)
		{
			Expression[] array = new Expression[args.Length];
			BindingRestrictions bindingRestrictions = BindingRestrictions.Empty;
			ICSharpInvokeOrInvokeMemberBinder icsharpInvokeOrInvokeMemberBinder = action as ICSharpInvokeOrInvokeMemberBinder;
			ParameterExpression parameterExpression = null;
			IEnumerator<CSharpArgumentInfo> enumerator = (arginfos ?? Array.Empty<CSharpArgumentInfo>()).GetEnumerator();
			for (int i = 0; i < args.Length; i++)
			{
				DynamicMetaObject dynamicMetaObject = args[i];
				CSharpArgumentInfo csharpArgumentInfo = (enumerator.MoveNext() ? enumerator.Current : null);
				if (i == 0 && BinderHelper.IsIncrementOrDecrementActionOnLocal(action))
				{
					object value = dynamicMetaObject.Value;
					parameterExpression = Expression.Variable((value != null) ? value.GetType() : typeof(object), "t0");
					array[0] = parameterExpression;
				}
				else
				{
					array[i] = dynamicMetaObject.Expression;
				}
				BindingRestrictions bindingRestrictions2 = BinderHelper.DeduceArgumentRestriction(i, icsharpInvokeOrInvokeMemberBinder, dynamicMetaObject, csharpArgumentInfo);
				bindingRestrictions = bindingRestrictions.Merge(bindingRestrictions2);
				if (csharpArgumentInfo != null && csharpArgumentInfo.LiteralConstant)
				{
					if (dynamicMetaObject.Value is double && double.IsNaN((double)dynamicMetaObject.Value))
					{
						MethodInfo methodInfo;
						if ((methodInfo = BinderHelper.s_DoubleIsNaN) == null)
						{
							methodInfo = (BinderHelper.s_DoubleIsNaN = typeof(double).GetMethod("IsNaN"));
						}
						MethodInfo methodInfo2 = methodInfo;
						Expression expression = Expression.Call(null, methodInfo2, new Expression[] { dynamicMetaObject.Expression });
						bindingRestrictions = bindingRestrictions.Merge(BindingRestrictions.GetExpressionRestriction(expression));
					}
					else if (dynamicMetaObject.Value is float && float.IsNaN((float)dynamicMetaObject.Value))
					{
						MethodInfo methodInfo3;
						if ((methodInfo3 = BinderHelper.s_SingleIsNaN) == null)
						{
							methodInfo3 = (BinderHelper.s_SingleIsNaN = typeof(float).GetMethod("IsNaN"));
						}
						MethodInfo methodInfo4 = methodInfo3;
						Expression expression2 = Expression.Call(null, methodInfo4, new Expression[] { dynamicMetaObject.Expression });
						bindingRestrictions = bindingRestrictions.Merge(BindingRestrictions.GetExpressionRestriction(expression2));
					}
					else
					{
						bindingRestrictions2 = BindingRestrictions.GetExpressionRestriction(Expression.Equal(dynamicMetaObject.Expression, Expression.Constant(dynamicMetaObject.Value, dynamicMetaObject.Expression.Type)));
						bindingRestrictions = bindingRestrictions.Merge(bindingRestrictions2);
					}
				}
			}
			DynamicMetaObject dynamicMetaObject3;
			try
			{
				DynamicMetaObject dynamicMetaObject2;
				Expression expression3 = binder.Bind(action, array, args, out dynamicMetaObject2);
				if (dynamicMetaObject2 != null)
				{
					expression3 = BinderHelper.ConvertResult(dynamicMetaObject2.Expression, action);
					bindingRestrictions = dynamicMetaObject2.Restrictions.Merge(bindingRestrictions);
					dynamicMetaObject3 = new DynamicMetaObject(expression3, bindingRestrictions);
				}
				else
				{
					if (parameterExpression != null)
					{
						DynamicMetaObject dynamicMetaObject4 = args[0];
						expression3 = Expression.Block(new ParameterExpression[] { parameterExpression }, new Expression[]
						{
							Expression.Assign(parameterExpression, Expression.Convert(dynamicMetaObject4.Expression, dynamicMetaObject4.Value.GetType())),
							expression3,
							Expression.Assign(dynamicMetaObject4.Expression, Expression.Convert(parameterExpression, dynamicMetaObject4.Expression.Type))
						});
					}
					expression3 = BinderHelper.ConvertResult(expression3, action);
					dynamicMetaObject3 = new DynamicMetaObject(expression3, bindingRestrictions);
				}
			}
			catch (RuntimeBinderException ex)
			{
				if (onBindingError != null)
				{
					dynamicMetaObject3 = onBindingError;
				}
				else
				{
					dynamicMetaObject3 = new DynamicMetaObject(Expression.Throw(Expression.New(typeof(RuntimeBinderException).GetConstructor(new Type[] { typeof(string) }), new Expression[] { Expression.Constant(ex.Message) }), BinderHelper.GetTypeForErrorMetaObject(action, args)), bindingRestrictions);
				}
			}
			return dynamicMetaObject3;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x0000256C File Offset: 0x0000076C
		public static void ValidateBindArgument(DynamicMetaObject argument, string paramName)
		{
			if (argument == null)
			{
				throw Error.ArgumentNull(paramName);
			}
			if (!argument.HasValue)
			{
				throw Error.DynamicArgumentNeedsValue(paramName);
			}
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002588 File Offset: 0x00000788
		public static void ValidateBindArgument(DynamicMetaObject[] arguments, string paramName)
		{
			if (arguments != null)
			{
				for (int num = 0; num != arguments.Length; num++)
				{
					BinderHelper.ValidateBindArgument(arguments[num], string.Format("{0}[{1}]", paramName, num));
				}
			}
		}

		// Token: 0x06000019 RID: 25 RVA: 0x000025BF File Offset: 0x000007BF
		private static bool IsTypeOfStaticCall(int parameterIndex, ICSharpInvokeOrInvokeMemberBinder callPayload)
		{
			return parameterIndex == 0 && callPayload != null && callPayload.StaticCall;
		}

		// Token: 0x0600001A RID: 26 RVA: 0x000025CF File Offset: 0x000007CF
		private static bool IsComObject(object obj)
		{
			return obj != null && Marshal.IsComObject(obj);
		}

		// Token: 0x0600001B RID: 27 RVA: 0x000025DC File Offset: 0x000007DC
		private static bool IsTransparentProxy(object obj)
		{
			return false;
		}

		// Token: 0x0600001C RID: 28 RVA: 0x000025DF File Offset: 0x000007DF
		private static bool IsDynamicallyTypedRuntimeProxy(DynamicMetaObject argument, CSharpArgumentInfo info)
		{
			return info != null && !info.UseCompileTimeType && (BinderHelper.IsComObject(argument.Value) || BinderHelper.IsTransparentProxy(argument.Value));
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002608 File Offset: 0x00000808
		private static BindingRestrictions DeduceArgumentRestriction(int parameterIndex, ICSharpInvokeOrInvokeMemberBinder callPayload, DynamicMetaObject argument, CSharpArgumentInfo info)
		{
			if (argument.Value != null && !BinderHelper.IsTypeOfStaticCall(parameterIndex, callPayload) && !BinderHelper.IsDynamicallyTypedRuntimeProxy(argument, info))
			{
				return BindingRestrictions.GetTypeRestriction(argument.Expression, argument.RuntimeType);
			}
			return BindingRestrictions.GetInstanceRestriction(argument.Expression, argument.Value);
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002658 File Offset: 0x00000858
		private static Expression ConvertResult(Expression binding, DynamicMetaObjectBinder action)
		{
			if (action is CSharpInvokeConstructorBinder)
			{
				return binding;
			}
			if (binding.Type == typeof(void))
			{
				ICSharpInvokeOrInvokeMemberBinder icsharpInvokeOrInvokeMemberBinder = action as ICSharpInvokeOrInvokeMemberBinder;
				if (icsharpInvokeOrInvokeMemberBinder != null && icsharpInvokeOrInvokeMemberBinder.ResultDiscarded)
				{
					return Expression.Block(binding, Expression.Default(action.ReturnType));
				}
				throw Error.BindToVoidMethodButExpectResult();
			}
			else
			{
				if (binding.Type.IsValueType && !action.ReturnType.IsValueType)
				{
					return Expression.Convert(binding, action.ReturnType);
				}
				return binding;
			}
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000026D8 File Offset: 0x000008D8
		private static Type GetTypeForErrorMetaObject(DynamicMetaObjectBinder action, DynamicMetaObject[] args)
		{
			if (action is CSharpInvokeConstructorBinder)
			{
				return args[0].Value as Type;
			}
			return action.ReturnType;
		}

		// Token: 0x06000020 RID: 32 RVA: 0x000026F8 File Offset: 0x000008F8
		private static bool IsIncrementOrDecrementActionOnLocal(DynamicMetaObjectBinder action)
		{
			CSharpUnaryOperationBinder csharpUnaryOperationBinder = action as CSharpUnaryOperationBinder;
			return csharpUnaryOperationBinder != null && (csharpUnaryOperationBinder.Operation == ExpressionType.Increment || csharpUnaryOperationBinder.Operation == ExpressionType.Decrement);
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002728 File Offset: 0x00000928
		internal static T[] Cons<T>(T sourceHead, T[] sourceTail)
		{
			if (sourceTail == null || sourceTail.Length != 0)
			{
				T[] array = new T[sourceTail.Length + 1];
				array[0] = sourceHead;
				sourceTail.CopyTo(array, 1);
				return array;
			}
			return new T[] { sourceHead };
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002770 File Offset: 0x00000970
		internal static T[] Cons<T>(T sourceHead, T[] sourceMiddle, T sourceLast)
		{
			if (sourceMiddle == null || sourceMiddle.Length != 0)
			{
				T[] array = new T[sourceMiddle.Length + 2];
				array[0] = sourceHead;
				array[array.Length - 1] = sourceLast;
				sourceMiddle.CopyTo(array, 1);
				return array;
			}
			return new T[] { sourceHead, sourceLast };
		}

		// Token: 0x06000023 RID: 35 RVA: 0x000027C9 File Offset: 0x000009C9
		internal static T[] ToArray<T>(IEnumerable<T> source)
		{
			if (source != null)
			{
				return source.ToArray<T>();
			}
			return Array.Empty<T>();
		}

		// Token: 0x06000024 RID: 36 RVA: 0x000027DC File Offset: 0x000009DC
		internal static CallInfo CreateCallInfo(IEnumerable<CSharpArgumentInfo> argInfos, int discard)
		{
			int num = 0;
			List<string> list = new List<string>();
			foreach (CSharpArgumentInfo csharpArgumentInfo in argInfos)
			{
				if (csharpArgumentInfo.NamedArgument)
				{
					list.Add(csharpArgumentInfo.Name);
				}
				num++;
			}
			return new CallInfo(num - discard, list);
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002848 File Offset: 0x00000A48
		internal static string GetCLROperatorName(this ExpressionType p)
		{
			if (p <= ExpressionType.Subtract)
			{
				if (p == ExpressionType.Add)
				{
					return "op_Addition";
				}
				if (p == ExpressionType.And)
				{
					return "op_BitwiseAnd";
				}
				switch (p)
				{
				case ExpressionType.Divide:
					return "op_Division";
				case ExpressionType.Equal:
					return "op_Equality";
				case ExpressionType.ExclusiveOr:
					return "op_ExclusiveOr";
				case ExpressionType.GreaterThan:
					return "op_GreaterThan";
				case ExpressionType.GreaterThanOrEqual:
					return "op_GreaterThanOrEqual";
				case ExpressionType.LeftShift:
					return "op_LeftShift";
				case ExpressionType.LessThan:
					return "op_LessThan";
				case ExpressionType.LessThanOrEqual:
					return "op_LessThanOrEqual";
				case ExpressionType.Modulo:
					return "op_Modulus";
				case ExpressionType.Multiply:
					return "op_Multiply";
				case ExpressionType.Negate:
					return "op_UnaryNegation";
				case ExpressionType.UnaryPlus:
					return "op_UnaryPlus";
				case ExpressionType.Not:
					return "op_LogicalNot";
				case ExpressionType.NotEqual:
					return "op_Inequality";
				case ExpressionType.Or:
					return "op_BitwiseOr";
				case ExpressionType.RightShift:
					return "op_RightShift";
				case ExpressionType.Subtract:
					return "op_Subtraction";
				}
			}
			else
			{
				if (p == ExpressionType.Decrement)
				{
					return "op_Decrement";
				}
				switch (p)
				{
				case ExpressionType.Increment:
					return "op_Increment";
				case ExpressionType.Index:
				case ExpressionType.Label:
				case ExpressionType.RuntimeVariables:
				case ExpressionType.Loop:
				case ExpressionType.Switch:
				case ExpressionType.Throw:
				case ExpressionType.Try:
				case ExpressionType.Unbox:
				case ExpressionType.PowerAssign:
					break;
				case ExpressionType.AddAssign:
					return "op_Addition";
				case ExpressionType.AndAssign:
					return "op_BitwiseAnd";
				case ExpressionType.DivideAssign:
					return "op_Division";
				case ExpressionType.ExclusiveOrAssign:
					return "op_ExclusiveOr";
				case ExpressionType.LeftShiftAssign:
					return "op_LeftShift";
				case ExpressionType.ModuloAssign:
					return "op_Modulus";
				case ExpressionType.MultiplyAssign:
					return "op_Multiply";
				case ExpressionType.OrAssign:
					return "op_BitwiseOr";
				case ExpressionType.RightShiftAssign:
					return "op_RightShift";
				case ExpressionType.SubtractAssign:
					return "op_Subtraction";
				default:
					switch (p)
					{
					case ExpressionType.OnesComplement:
						return "op_OnesComplement";
					case ExpressionType.IsTrue:
						return "op_True";
					case ExpressionType.IsFalse:
						return "op_False";
					}
					break;
				}
			}
			return null;
		}

		// Token: 0x0400008E RID: 142
		private static MethodInfo s_DoubleIsNaN;

		// Token: 0x0400008F RID: 143
		private static MethodInfo s_SingleIsNaN;
	}
}
