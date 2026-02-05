using System;
using System.Reflection;
using System.Reflection.Emit;
using IPA.Utilities.Async;

namespace IPA.Utilities
{
	/// <summary>
	/// A type containing utilities for accessing non-public fields of objects.
	/// </summary>
	/// <typeparam name="T">the type that the fields are on</typeparam>
	/// <typeparam name="U">the type of the field to access</typeparam>
	/// <seealso cref="T:IPA.Utilities.PropertyAccessor`2" />
	// Token: 0x02000011 RID: 17
	public static class FieldAccessor<T, U>
	{
		// Token: 0x06000027 RID: 39 RVA: 0x000028B0 File Offset: 0x00000AB0
		private static FieldAccessor<T, U>.Accessor MakeAccessor(string fieldName)
		{
			FieldInfo field = typeof(T).GetField(fieldName, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (field == null)
			{
				throw new MissingFieldException(typeof(T).Name, fieldName);
			}
			if (field.FieldType != typeof(U))
			{
				throw new ArgumentException(string.Format("Field '{0}' not of type {1}", fieldName, typeof(U)));
			}
			DynamicMethod dyn = new DynamicMethod("<>_accessor__" + fieldName, typeof(U), new Type[] { typeof(T).MakeByRefType() }, typeof(FieldAccessor<T, U>), true);
			ReflectionUtil.DynamicMethodReturnType.SetValue(dyn, typeof(U).MakeByRefType());
			ILGenerator il = dyn.GetILGenerator();
			il.Emit(OpCodes.Ldarg_0);
			if (!typeof(T).IsValueType)
			{
				il.Emit(OpCodes.Ldind_Ref);
			}
			il.Emit(OpCodes.Ldflda, field);
			il.Emit(OpCodes.Ret);
			return (FieldAccessor<T, U>.Accessor)dyn.CreateDelegate(typeof(FieldAccessor<T, U>.Accessor));
		}

		/// <summary>
		/// Gets an <see cref="T:IPA.Utilities.FieldAccessor`2.Accessor" /> for the field named <paramref name="name" /> on <typeparamref name="T" />.
		/// </summary>
		/// <param name="name">the field name</param>
		/// <returns>an accessor for the field</returns>
		/// <exception cref="T:System.MissingFieldException">if the field does not exist on <typeparamref name="T" /></exception>
		// Token: 0x06000028 RID: 40 RVA: 0x000029D3 File Offset: 0x00000BD3
		public static FieldAccessor<T, U>.Accessor GetAccessor(string name)
		{
			return FieldAccessor<T, U>.accessors.GetOrAdd(name, new Func<string, FieldAccessor<T, U>.Accessor>(FieldAccessor<T, U>.MakeAccessor));
		}

		/// <summary>
		/// Accesses a field for an object by name.
		/// </summary>
		/// <param name="obj">the object to access the field of</param>
		/// <param name="name">the name of the field to access</param>
		/// <returns>a reference to the object at the field</returns>
		/// <exception cref="T:System.MissingFieldException">if the field does not exist on <typeparamref name="T" /></exception>
		/// <seealso cref="M:IPA.Utilities.FieldAccessor`2.GetAccessor(System.String)" />
		// Token: 0x06000029 RID: 41 RVA: 0x000029EC File Offset: 0x00000BEC
		public static ref U Access(ref T obj, string name)
		{
			return FieldAccessor<T, U>.GetAccessor(name)(ref obj);
		}

		/// <summary>
		/// Gets the value of a field of an object by name.
		/// </summary>
		/// <remarks>
		/// The only good reason to use this over <see cref="M:IPA.Utilities.FieldAccessor`2.Get(`0,System.String)" /> is when you are working with a value type,
		/// as it prevents a copy.
		/// </remarks>
		/// <param name="obj">the object to access the field of</param>
		/// <param name="name">the name of the field to access</param>
		/// <returns>the value of the field</returns>
		/// <exception cref="T:System.MissingFieldException">if the field does not exist on <typeparamref name="T" /></exception>
		/// <seealso cref="M:IPA.Utilities.FieldAccessor`2.Get(`0,System.String)" />
		/// <seealso cref="M:IPA.Utilities.FieldAccessor`2.Access(`0@,System.String)" />
		/// <seealso cref="M:IPA.Utilities.FieldAccessor`2.GetAccessor(System.String)" />
		// Token: 0x0600002A RID: 42 RVA: 0x000029FA File Offset: 0x00000BFA
		public unsafe static U Get(ref T obj, string name)
		{
			return *FieldAccessor<T, U>.Access(ref obj, name);
		}

		/// <summary>
		/// Gets the value of a field of an object by name.
		/// </summary>
		/// <param name="obj">the object to access the field of</param>
		/// <param name="name">the name of the field to access</param>
		/// <returns>the value of the field</returns>
		/// <exception cref="T:System.MissingFieldException">if the field does not exist on <typeparamref name="T" /></exception>
		/// <seealso cref="M:IPA.Utilities.FieldAccessor`2.Get(`0@,System.String)" />
		/// <seealso cref="M:IPA.Utilities.FieldAccessor`2.Access(`0@,System.String)" />
		/// <seealso cref="M:IPA.Utilities.FieldAccessor`2.GetAccessor(System.String)" />
		// Token: 0x0600002B RID: 43 RVA: 0x00002A08 File Offset: 0x00000C08
		public static U Get(T obj, string name)
		{
			return FieldAccessor<T, U>.Get(ref obj, name);
		}

		/// <summary>
		/// Sets the value of a field for an object by name.
		/// </summary>
		/// <remarks>
		/// This overload must be used for value types.
		/// </remarks>
		/// <param name="obj">the object to set the field of</param>
		/// <param name="name">the name of the field</param>
		/// <param name="value">the value to set it to</param>
		/// <exception cref="T:System.MissingFieldException">if the field does not exist on <typeparamref name="T" /></exception>
		/// <seealso cref="M:IPA.Utilities.FieldAccessor`2.Set(`0,System.String,`1)" />
		/// <seealso cref="M:IPA.Utilities.FieldAccessor`2.Access(`0@,System.String)" />
		/// <seealso cref="M:IPA.Utilities.FieldAccessor`2.GetAccessor(System.String)" />
		// Token: 0x0600002C RID: 44 RVA: 0x00002A12 File Offset: 0x00000C12
		public unsafe static void Set(ref T obj, string name, U value)
		{
			*FieldAccessor<T, U>.Access(ref obj, name) = value;
		}

		/// <summary>
		/// Sets the value of a field for an object by name.
		/// </summary>
		/// <remarks>
		/// This overload cannot be safely used for value types. Use <see cref="M:IPA.Utilities.FieldAccessor`2.Set(`0@,System.String,`1)" /> instead.
		/// </remarks>
		/// <param name="obj">the object to set the field of</param>
		/// <param name="name">the name of the field</param>
		/// <param name="value">the value to set it to</param>
		/// <exception cref="T:System.MissingFieldException">if the field does not exist on <typeparamref name="T" /></exception>
		/// <seealso cref="M:IPA.Utilities.FieldAccessor`2.Set(`0@,System.String,`1)" />
		/// <seealso cref="M:IPA.Utilities.FieldAccessor`2.Access(`0@,System.String)" />
		/// <seealso cref="M:IPA.Utilities.FieldAccessor`2.GetAccessor(System.String)" />
		// Token: 0x0600002D RID: 45 RVA: 0x00002A21 File Offset: 0x00000C21
		public static void Set(T obj, string name, U value)
		{
			FieldAccessor<T, U>.Set(ref obj, name, value);
		}

		// Token: 0x04000013 RID: 19
		private static readonly SingleCreationValueCache<string, FieldAccessor<T, U>.Accessor> accessors = new SingleCreationValueCache<string, FieldAccessor<T, U>.Accessor>();

		/// <summary>
		/// A delegate for a field accessor taking a <typeparamref name="T" /> ref and returning a <typeparamref name="U" /> ref.
		/// </summary>
		/// <param name="obj">the object to access the field of</param>
		/// <returns>a reference to the field's value</returns>
		// Token: 0x020000B7 RID: 183
		// (Invoke) Token: 0x06000480 RID: 1152
		public delegate ref U Accessor(ref T obj);
	}
}
