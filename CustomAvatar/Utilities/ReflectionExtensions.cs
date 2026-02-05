using System;
using System.Reflection;

namespace CustomAvatar.Utilities
{
	// Token: 0x0200001F RID: 31
	internal static class ReflectionExtensions
	{
		// Token: 0x06000068 RID: 104 RVA: 0x00004324 File Offset: 0x00002524
		internal static TResult GetPrivateField<TSubject, TResult>(this TSubject obj, string fieldName)
		{
			bool flag = obj == null;
			if (flag)
			{
				throw new ArgumentNullException("obj");
			}
			FieldInfo field = typeof(TSubject).GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
			bool flag2 = field == null;
			if (flag2)
			{
				throw new InvalidOperationException("Field \"" + fieldName + "\" does not exist on " + typeof(TSubject).FullName);
			}
			return (TResult)((object)field.GetValue(obj));
		}

		// Token: 0x06000069 RID: 105 RVA: 0x000043A4 File Offset: 0x000025A4
		internal static void SetPrivateField<TSubject>(this TSubject obj, string fieldName, object value)
		{
			bool flag = obj == null;
			if (flag)
			{
				throw new ArgumentNullException("obj");
			}
			FieldInfo field = typeof(TSubject).GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
			bool flag2 = field == null;
			if (flag2)
			{
				throw new InvalidOperationException("Field \"" + fieldName + "\" does not exist on " + typeof(TSubject).FullName);
			}
			field.SetValue(obj, value);
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00004420 File Offset: 0x00002620
		internal static void InvokePrivateMethod<TSubject>(this TSubject obj, string methodName, params object[] args)
		{
			bool flag = obj == null;
			if (flag)
			{
				throw new ArgumentNullException("obj");
			}
			MethodInfo method = typeof(TSubject).GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);
			bool flag2 = method == null;
			if (flag2)
			{
				throw new InvalidOperationException("Method \"" + methodName + "\" does not exist on " + typeof(TSubject).FullName);
			}
			method.Invoke(obj, args);
		}

		// Token: 0x0600006B RID: 107 RVA: 0x0000449C File Offset: 0x0000269C
		internal static TDelegate CreatePrivateMethodDelegate<TDelegate>(this Type type, string methodName) where TDelegate : Delegate
		{
			bool flag = type == null;
			if (flag)
			{
				throw new ArgumentNullException("type");
			}
			MethodInfo method = type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);
			bool flag2 = method == null;
			if (flag2)
			{
				throw new InvalidOperationException("Method \"" + methodName + "\" does not exist on " + type.FullName);
			}
			return (TDelegate)((object)Delegate.CreateDelegate(typeof(TDelegate), method));
		}
	}
}
