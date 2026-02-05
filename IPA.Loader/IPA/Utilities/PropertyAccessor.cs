using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using IPA.Utilities.Async;

namespace IPA.Utilities
{
	/// <summary>
	/// A type containing utilities for accessing non-public properties of an object.
	/// </summary>
	/// <typeparam name="T">the type that the properties are on</typeparam>
	/// <typeparam name="U">the type of the property to access</typeparam>
	// Token: 0x02000012 RID: 18
	public static class PropertyAccessor<T, U>
	{
		// Token: 0x0600002F RID: 47 RVA: 0x00002A38 File Offset: 0x00000C38
		private static global::System.ValueTuple<PropertyAccessor<T, U>.Getter, PropertyAccessor<T, U>.Setter> MakeAccessors(string propName)
		{
			PropertyInfo property = typeof(T).GetProperty(propName, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (property == null)
			{
				throw new MissingMemberException(typeof(T).Name, propName);
			}
			if (property.PropertyType != typeof(U))
			{
				throw new ArgumentException(string.Format("Property '{0}' on {1} is not of type {2}", propName, typeof(T), typeof(U)));
			}
			MethodInfo getM = property.GetGetMethod(true);
			MethodInfo setM = property.GetSetMethod(true);
			PropertyAccessor<T, U>.Getter getter = null;
			PropertyAccessor<T, U>.Setter setter = null;
			if (typeof(T).IsValueType)
			{
				if (getM != null)
				{
					getter = (PropertyAccessor<T, U>.Getter)Delegate.CreateDelegate(typeof(PropertyAccessor<T, U>.Getter), getM);
				}
				if (setM != null)
				{
					setter = (PropertyAccessor<T, U>.Setter)Delegate.CreateDelegate(typeof(PropertyAccessor<T, U>.Setter), setM);
				}
			}
			else
			{
				if (getM != null)
				{
					DynamicMethod dynamicMethod = new DynamicMethod("<>_get__" + propName, typeof(U), new Type[] { typeof(T).MakeByRefType() }, typeof(PropertyAccessor<T, U>), true);
					ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
					ilgenerator.Emit(OpCodes.Ldarg_0);
					ilgenerator.Emit(OpCodes.Ldind_Ref);
					ilgenerator.Emit(OpCodes.Tailcall);
					ilgenerator.Emit(getM.IsVirtual ? OpCodes.Callvirt : OpCodes.Call, getM);
					ilgenerator.Emit(OpCodes.Ret);
					getter = (PropertyAccessor<T, U>.Getter)dynamicMethod.CreateDelegate(typeof(PropertyAccessor<T, U>.Getter));
				}
				if (setM != null)
				{
					DynamicMethod dynamicMethod2 = new DynamicMethod("<>_set__" + propName, typeof(void), new Type[]
					{
						typeof(T).MakeByRefType(),
						typeof(U)
					}, typeof(PropertyAccessor<T, U>), true);
					ILGenerator ilgenerator2 = dynamicMethod2.GetILGenerator();
					ilgenerator2.Emit(OpCodes.Ldarg_0);
					ilgenerator2.Emit(OpCodes.Ldind_Ref);
					ilgenerator2.Emit(OpCodes.Ldarg_1);
					ilgenerator2.Emit(OpCodes.Tailcall);
					ilgenerator2.Emit(setM.IsVirtual ? OpCodes.Callvirt : OpCodes.Call, setM);
					ilgenerator2.Emit(OpCodes.Ret);
					setter = (PropertyAccessor<T, U>.Setter)dynamicMethod2.CreateDelegate(typeof(PropertyAccessor<T, U>.Setter));
				}
			}
			return new global::System.ValueTuple<PropertyAccessor<T, U>.Getter, PropertyAccessor<T, U>.Setter>(getter, setter);
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002C8F File Offset: 0x00000E8F
		[return: global::System.Runtime.CompilerServices.TupleElementNames(new string[] { "get", "set" })]
		private static global::System.ValueTuple<PropertyAccessor<T, U>.Getter, PropertyAccessor<T, U>.Setter> GetAccessors(string propName)
		{
			return PropertyAccessor<T, U>.props.GetOrAdd(propName, new Func<string, global::System.ValueTuple<PropertyAccessor<T, U>.Getter, PropertyAccessor<T, U>.Setter>>(PropertyAccessor<T, U>.MakeAccessors));
		}

		/// <summary>
		/// Gets a <see cref="T:IPA.Utilities.PropertyAccessor`2.Getter" /> for the property identified by <paramref name="name" />.
		/// </summary>
		/// <param name="name">the name of the property</param>
		/// <returns>a <see cref="T:IPA.Utilities.PropertyAccessor`2.Getter" /> that can access that property</returns>
		/// <exception cref="T:System.MissingMemberException">if the property does not exist</exception>
		// Token: 0x06000031 RID: 49 RVA: 0x00002CA8 File Offset: 0x00000EA8
		public static PropertyAccessor<T, U>.Getter GetGetter(string name)
		{
			return PropertyAccessor<T, U>.GetAccessors(name).Item1;
		}

		/// <summary>
		/// Gets a <see cref="T:IPA.Utilities.PropertyAccessor`2.Setter" /> for the property identified by <paramref name="name" />.
		/// </summary>
		/// <param name="name">the name of the property</param>
		/// <returns>a <see cref="T:IPA.Utilities.PropertyAccessor`2.Setter" /> that can access that property</returns>
		/// <exception cref="T:System.MissingMemberException">if the property does not exist</exception>
		// Token: 0x06000032 RID: 50 RVA: 0x00002CB5 File Offset: 0x00000EB5
		public static PropertyAccessor<T, U>.Setter GetSetter(string name)
		{
			return PropertyAccessor<T, U>.GetAccessors(name).Item2;
		}

		/// <summary>
		/// Gets the value of the property identified by <paramref name="name" /> on <paramref name="obj" />.
		/// </summary>
		/// <remarks>
		/// The only reason to use this over <see cref="M:IPA.Utilities.PropertyAccessor`2.Get(`0,System.String)" /> is if you are using a value type because 
		/// it avoids a copy.
		/// </remarks>
		/// <param name="obj">the instance to access</param>
		/// <param name="name">the name of the property</param>
		/// <returns>the value of the property</returns>
		/// <exception cref="T:System.MissingMemberException">if the property does not exist</exception>
		/// <seealso cref="M:IPA.Utilities.PropertyAccessor`2.Get(`0,System.String)" />
		/// <seealso cref="M:IPA.Utilities.PropertyAccessor`2.GetGetter(System.String)" />
		// Token: 0x06000033 RID: 51 RVA: 0x00002CC2 File Offset: 0x00000EC2
		public static U Get(ref T obj, string name)
		{
			return PropertyAccessor<T, U>.GetGetter(name)(ref obj);
		}

		/// <summary>
		/// Gets the value of the property identified by <paramref name="name" /> on <paramref name="obj" />.
		/// </summary>
		/// <param name="obj">the instance to access</param>
		/// <param name="name">the name of the property</param>
		/// <returns>the value of the property</returns>
		/// <exception cref="T:System.MissingMemberException">if the property does not exist</exception>
		/// <seealso cref="M:IPA.Utilities.PropertyAccessor`2.Get(`0@,System.String)" />
		/// <seealso cref="M:IPA.Utilities.PropertyAccessor`2.GetGetter(System.String)" />
		// Token: 0x06000034 RID: 52 RVA: 0x00002CD0 File Offset: 0x00000ED0
		public static U Get(T obj, string name)
		{
			return PropertyAccessor<T, U>.GetGetter(name)(ref obj);
		}

		/// <summary>
		/// Sets the value of the property identified by <paramref name="name" /> on <paramref name="obj" />.
		/// </summary>
		/// <remarks>
		/// This overload must be used for value types.
		/// </remarks>
		/// <param name="obj">the instance to access</param>
		/// <param name="name">the name of the property</param>
		/// <param name="val">the new value of the property</param>
		/// <exception cref="T:System.MissingMemberException">if the property does not exist</exception>
		/// <seealso cref="M:IPA.Utilities.PropertyAccessor`2.Set(`0,System.String,`1)" />
		/// <seealso cref="M:IPA.Utilities.PropertyAccessor`2.GetSetter(System.String)" />
		// Token: 0x06000035 RID: 53 RVA: 0x00002CDF File Offset: 0x00000EDF
		public static void Set(ref T obj, string name, U val)
		{
			PropertyAccessor<T, U>.GetSetter(name)(ref obj, val);
		}

		/// <summary>
		/// Sets the value of the property identified by <paramref name="name" /> on <paramref name="obj" />.
		/// </summary>
		/// <remarks>
		/// This overload cannot be safely used for value types. Use <see cref="M:IPA.Utilities.PropertyAccessor`2.Set(`0@,System.String,`1)" /> instead.
		/// </remarks>
		/// <param name="obj">the instance to access</param>
		/// <param name="name">the name of the property</param>
		/// <param name="val">the new value of the property</param>
		/// <exception cref="T:System.MissingMemberException">if the property does not exist</exception>
		/// <seealso cref="M:IPA.Utilities.PropertyAccessor`2.Set(`0@,System.String,`1)" />
		/// <seealso cref="M:IPA.Utilities.PropertyAccessor`2.GetSetter(System.String)" />
		// Token: 0x06000036 RID: 54 RVA: 0x00002CEE File Offset: 0x00000EEE
		public static void Set(T obj, string name, U val)
		{
			PropertyAccessor<T, U>.GetSetter(name)(ref obj, val);
		}

		// Token: 0x04000014 RID: 20
		[global::System.Runtime.CompilerServices.TupleElementNames(new string[] { "get", "set" })]
		private static readonly SingleCreationValueCache<string, global::System.ValueTuple<PropertyAccessor<T, U>.Getter, PropertyAccessor<T, U>.Setter>> props = new SingleCreationValueCache<string, global::System.ValueTuple<PropertyAccessor<T, U>.Getter, PropertyAccessor<T, U>.Setter>>();

		/// <summary>
		/// A getter for a property.
		/// </summary>
		/// <param name="obj">the object it is a member of</param>
		/// <returns>the value of the property</returns>
		// Token: 0x020000B8 RID: 184
		// (Invoke) Token: 0x06000484 RID: 1156
		public delegate U Getter(ref T obj);

		/// <summary>
		/// A setter for a property.
		/// </summary>
		/// <param name="obj">the object it is a member of</param>
		/// <param name="val">the new property value</param>
		// Token: 0x020000B9 RID: 185
		// (Invoke) Token: 0x06000488 RID: 1160
		public delegate void Setter(ref T obj, U val);
	}
}
