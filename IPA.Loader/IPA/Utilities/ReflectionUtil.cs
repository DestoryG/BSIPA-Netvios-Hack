using System;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;

namespace IPA.Utilities
{
	/// <summary>
	/// A utility class providing reflection helper methods.
	/// </summary>
	// Token: 0x0200001D RID: 29
	public static class ReflectionUtil
	{
		/// <summary>
		/// Sets a field on the target object, as gotten from <typeparamref name="T" />.
		/// </summary>
		/// <typeparam name="T">the type to get the field from</typeparam>
		/// <typeparam name="U">the type of the field to set</typeparam>
		/// <param name="obj">the object instance</param>
		/// <param name="fieldName">the field to set</param>
		/// <param name="value">the value to set it to</param>
		/// <exception cref="T:System.MissingFieldException">if <paramref name="fieldName" /> does not exist on <typeparamref name="T" /></exception>
		/// <seealso cref="M:IPA.Utilities.FieldAccessor`2.Set(`0@,System.String,`1)" />
		// Token: 0x06000083 RID: 131 RVA: 0x00003819 File Offset: 0x00001A19
		public static void SetField<T, U>(this T obj, string fieldName, U value)
		{
			FieldAccessor<T, U>.Set(ref obj, fieldName, value);
		}

		/// <summary>
		/// Gets the value of a field.
		/// </summary>
		/// <typeparam name="T">the type to get the field from</typeparam>
		/// <typeparam name="U">the type of the field (result casted)</typeparam>
		/// <param name="obj">the object instance to pull from</param>
		/// <param name="fieldName">the name of the field to read</param>
		/// <returns>the value of the field</returns>
		/// <exception cref="T:System.MissingFieldException">if <paramref name="fieldName" /> does not exist on <typeparamref name="T" /></exception>
		/// <seealso cref="M:IPA.Utilities.FieldAccessor`2.Get(`0@,System.String)" />
		// Token: 0x06000084 RID: 132 RVA: 0x00003824 File Offset: 0x00001A24
		public static U GetField<U, T>(this T obj, string fieldName)
		{
			return FieldAccessor<T, U>.Get(ref obj, fieldName);
		}

		/// <summary>
		/// Sets a property on the target object, as gotten from <typeparamref name="T" />.
		/// </summary>
		/// <typeparam name="T">the type to get the property from</typeparam>
		/// <typeparam name="U">the type of the property to set</typeparam>
		/// <param name="obj">the object instance</param>
		/// <param name="propertyName">the property to set</param>
		/// <param name="value">the value to set it to</param>
		/// <exception cref="T:System.MissingMemberException">if <paramref name="propertyName" /> does not exist on <typeparamref name="T" /></exception>
		/// <seealso cref="M:IPA.Utilities.PropertyAccessor`2.Set(`0@,System.String,`1)" />
		// Token: 0x06000085 RID: 133 RVA: 0x0000382E File Offset: 0x00001A2E
		public static void SetProperty<T, U>(this T obj, string propertyName, U value)
		{
			PropertyAccessor<T, U>.Set(ref obj, propertyName, value);
		}

		/// <summary>
		/// Gets a property on the target object, as gotten from <typeparamref name="T" />.
		/// </summary>
		/// <typeparam name="T">the type to get the property from</typeparam>
		/// <typeparam name="U">the type of the property to get</typeparam>
		/// <param name="obj">the object instance</param>
		/// <param name="propertyName">the property to get</param>
		/// <returns>the value of the property</returns>
		/// <exception cref="T:System.MissingMemberException">if <paramref name="propertyName" /> does not exist on <typeparamref name="T" /></exception>
		/// <seealso cref="M:IPA.Utilities.PropertyAccessor`2.Get(`0@,System.String)" />
		// Token: 0x06000086 RID: 134 RVA: 0x00003839 File Offset: 0x00001A39
		public static U GetProperty<U, T>(this T obj, string propertyName)
		{
			return PropertyAccessor<T, U>.Get(ref obj, propertyName);
		}

		/// <summary>
		/// Invokes a method from <typeparamref name="T" /> on an object.
		/// </summary>
		/// <typeparam name="U">the type that the method returns</typeparam>
		/// <typeparam name="T">the type to search for the method on</typeparam>
		/// <param name="obj">the object instance</param>
		/// <param name="methodName">the method's name</param>
		/// <param name="args">the method arguments</param>
		/// <returns>the return value</returns>
		/// <exception cref="T:System.MissingMethodException">if <paramref name="methodName" /> does not exist on <typeparamref name="T" /></exception>
		// Token: 0x06000087 RID: 135 RVA: 0x00003844 File Offset: 0x00001A44
		public static U InvokeMethod<U, T>(this T obj, string methodName, params object[] args)
		{
			MethodInfo method = typeof(T).GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (method == null)
			{
				throw new MissingMethodException("Method " + methodName + " does not exist", "methodName");
			}
			return (U)((object)((method != null) ? method.Invoke(obj, args) : null));
		}

		/// <summary>
		/// Copies a component <paramref name="original" /> to a component of <paramref name="overridingType" /> on the destination <see cref="T:UnityEngine.GameObject" />.
		/// </summary>
		/// <param name="original">the original component</param>
		/// <param name="overridingType">the new component's type</param>
		/// <param name="destination">the destination GameObject</param>
		/// <param name="originalTypeOverride">overrides the source component type (for example, to a superclass)</param>
		/// <returns>the copied component</returns>
		// Token: 0x06000088 RID: 136 RVA: 0x000038A0 File Offset: 0x00001AA0
		public static Component CopyComponent(this Component original, Type overridingType, GameObject destination, Type originalTypeOverride = null)
		{
			Component copy = destination.AddComponent(overridingType);
			Type type = originalTypeOverride ?? original.GetType();
			while (type != typeof(MonoBehaviour))
			{
				ReflectionUtil.CopyForType(type, original, copy);
				type = ((type != null) ? type.BaseType : null);
			}
			return copy;
		}

		/// <summary>
		/// A generic version of <see cref="M:IPA.Utilities.ReflectionUtil.CopyComponent(UnityEngine.Component,System.Type,UnityEngine.GameObject,System.Type)" />. 
		/// </summary>
		/// <seealso cref="M:IPA.Utilities.ReflectionUtil.CopyComponent(UnityEngine.Component,System.Type,UnityEngine.GameObject,System.Type)" />
		/// <typeparam name="T">the overriding type</typeparam>
		/// <param name="original">the original component</param>
		/// <param name="destination">the destination game object</param>
		/// <param name="originalTypeOverride">overrides the source component type (for example, to a superclass)</param>
		/// <returns>the copied component</returns>
		// Token: 0x06000089 RID: 137 RVA: 0x000038EC File Offset: 0x00001AEC
		public static T CopyComponent<T>(this Component original, GameObject destination, Type originalTypeOverride = null) where T : Component
		{
			T copy = destination.AddComponent<T>();
			Type type = originalTypeOverride ?? original.GetType();
			while (type != typeof(MonoBehaviour))
			{
				ReflectionUtil.CopyForType(type, original, copy);
				type = ((type != null) ? type.BaseType : null);
			}
			return copy;
		}

		// Token: 0x0600008A RID: 138 RVA: 0x0000393C File Offset: 0x00001B3C
		private static void CopyForType(Type type, Component source, Component destination)
		{
			foreach (FieldInfo fi in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
			{
				fi.SetValue(destination, fi.GetValue(source));
			}
		}

		// Token: 0x0400002B RID: 43
		internal static readonly FieldInfo DynamicMethodReturnType = typeof(DynamicMethod).GetField("returnType", BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.NonPublic);
	}
}
