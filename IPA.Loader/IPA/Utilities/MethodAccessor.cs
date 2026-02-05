using System;
using System.Linq;
using System.Reflection;
using IPA.Utilities.Async;

namespace IPA.Utilities
{
	/// <summary>
	/// A type containing utilities for calling non-public methods on an object.
	/// </summary>
	/// <typeparam name="T">the type to find the methods on</typeparam>
	/// <typeparam name="TDelegate">the delegate type to create, and to use as a signature to search for</typeparam>
	// Token: 0x02000014 RID: 20
	public static class MethodAccessor<T, TDelegate> where TDelegate : Delegate
	{
		// Token: 0x0600003A RID: 58 RVA: 0x00002D48 File Offset: 0x00000F48
		static MethodAccessor()
		{
			Type firstType = AccessorDelegateInfo<TDelegate>.Parameters.First<ParameterInfo>().ParameterType;
			if (typeof(T).IsValueType)
			{
				if (!firstType.IsByRef)
				{
					throw new InvalidOperationException("First parameter of a method accessor to a value type is not byref");
				}
				firstType = firstType.GetElementType();
			}
			if (!typeof(T).IsAssignableFrom(firstType))
			{
				throw new InvalidOperationException("First parameter of a method accessor is not assignable to the method owning type");
			}
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00002DB8 File Offset: 0x00000FB8
		private static TDelegate MakeDelegate(string name)
		{
			ParameterInfo[] delParams = AccessorDelegateInfo<TDelegate>.Parameters;
			MethodInfo method = typeof(T).GetMethod(name, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, (from p in delParams.Skip(1)
				select p.ParameterType).ToArray<Type>(), Array.Empty<ParameterModifier>());
			if (method == null)
			{
				throw new MissingMethodException(typeof(T).FullName, name);
			}
			if (!AccessorDelegateInfo<TDelegate>.Invoke.ReturnType.IsAssignableFrom(method.ReturnType))
			{
				throw new ArgumentException(string.Format("The method found returns a type incompatable with the return type of {0}", typeof(TDelegate)));
			}
			return (TDelegate)((object)Delegate.CreateDelegate(AccessorDelegateInfo<TDelegate>.Type, method, true));
		}

		/// <summary>
		/// Gets a delegate to the named method with the signature specified by <typeparamref name="TDelegate" />.
		/// </summary>
		/// <param name="name">the name of the method to get</param>
		/// <returns>a delegate that can call the specified method</returns>
		/// <exception cref="T:System.MissingMethodException">if <paramref name="name" /> does not represent the name of a method with the given signature</exception>
		/// <exception cref="T:System.ArgumentException">if the method found returns a type incompatable with the return type of <typeparamref name="TDelegate" /></exception>
		// Token: 0x0600003C RID: 60 RVA: 0x00002E75 File Offset: 0x00001075
		public static TDelegate GetDelegate(string name)
		{
			return MethodAccessor<T, TDelegate>.methods.GetOrAdd(name, new Func<string, TDelegate>(MethodAccessor<T, TDelegate>.MakeDelegate));
		}

		// Token: 0x04000018 RID: 24
		private static readonly SingleCreationValueCache<string, TDelegate> methods = new SingleCreationValueCache<string, TDelegate>();
	}
}
