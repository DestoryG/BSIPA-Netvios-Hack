using System;
using IPA.Config.Data;

namespace IPA.Config.Stores
{
	/// <summary>
	/// A strongly-typed <see cref="T:IPA.Config.Stores.IValueConverter" />.
	/// </summary>
	/// <typeparam name="T">the type of object to handle</typeparam>
	/// <seealso cref="T:IPA.Config.Stores.IValueConverter" />
	// Token: 0x02000067 RID: 103
	public abstract class ValueConverter<T> : IValueConverter
	{
		/// <summary>
		/// Converts the given object to a <see cref="T:IPA.Config.Data.Value" />.
		/// </summary>
		/// <param name="obj">the object to convert</param>
		/// <param name="parent">the owning object of <paramref name="obj" /></param>
		/// <returns>a representation of <paramref name="obj" /> as a <see cref="T:IPA.Config.Data.Value" /> structure</returns>
		/// <seealso cref="M:IPA.Config.Stores.IValueConverter.ToValue(System.Object,System.Object)" />
		// Token: 0x06000319 RID: 793
		public abstract Value ToValue(T obj, object parent);

		/// <summary>
		/// Converts the given <see cref="T:IPA.Config.Data.Value" /> to the object type handled by this converter.
		/// </summary>
		/// <param name="value">the <see cref="T:IPA.Config.Data.Value" /> to deserialize</param>
		/// <param name="parent">the object that will own the result</param>
		/// <returns>the deserialized object</returns>
		/// <seealso cref="M:IPA.Config.Stores.IValueConverter.FromValue(IPA.Config.Data.Value,System.Object)" />
		// Token: 0x0600031A RID: 794
		public abstract T FromValue(Value value, object parent);

		// Token: 0x0600031B RID: 795 RVA: 0x0001235A File Offset: 0x0001055A
		Value IValueConverter.ToValue(object obj, object parent)
		{
			return this.ToValue((T)((object)obj), parent);
		}

		// Token: 0x0600031C RID: 796 RVA: 0x00012369 File Offset: 0x00010569
		object IValueConverter.FromValue(Value value, object parent)
		{
			return this.FromValue(value, parent);
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x0600031D RID: 797 RVA: 0x00012378 File Offset: 0x00010578
		Type IValueConverter.Type
		{
			get
			{
				return typeof(T);
			}
		}
	}
}
