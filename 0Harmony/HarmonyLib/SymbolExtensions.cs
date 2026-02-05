using System;
using System.Linq.Expressions;
using System.Reflection;

namespace HarmonyLib
{
	// Token: 0x020000A0 RID: 160
	public static class SymbolExtensions
	{
		// Token: 0x0600031C RID: 796 RVA: 0x0000F8A8 File Offset: 0x0000DAA8
		public static MethodInfo GetMethodInfo(Expression<Action> expression)
		{
			return SymbolExtensions.GetMethodInfo(expression);
		}

		// Token: 0x0600031D RID: 797 RVA: 0x0000F8A8 File Offset: 0x0000DAA8
		public static MethodInfo GetMethodInfo<T>(Expression<Action<T>> expression)
		{
			return SymbolExtensions.GetMethodInfo(expression);
		}

		// Token: 0x0600031E RID: 798 RVA: 0x0000F8A8 File Offset: 0x0000DAA8
		public static MethodInfo GetMethodInfo<T, TResult>(Expression<Func<T, TResult>> expression)
		{
			return SymbolExtensions.GetMethodInfo(expression);
		}

		// Token: 0x0600031F RID: 799 RVA: 0x0000F8B0 File Offset: 0x0000DAB0
		public static MethodInfo GetMethodInfo(LambdaExpression expression)
		{
			MethodCallExpression methodCallExpression = expression.Body as MethodCallExpression;
			if (methodCallExpression == null)
			{
				throw new ArgumentException("Invalid Expression. Expression should consist of a Method call only.");
			}
			MethodInfo method = methodCallExpression.Method;
			if (method == null)
			{
				throw new Exception(string.Format("Cannot find method for expression {0}", expression));
			}
			return method;
		}
	}
}
