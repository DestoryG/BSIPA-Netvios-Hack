using System;

namespace IPA.Utilities
{
	/// <summary>
	/// Utilities to create <see cref="T:IPA.Utilities.Ref`1" /> using type inference.
	/// </summary>
	// Token: 0x0200001A RID: 26
	public static class Ref
	{
		/// <summary>
		/// Creates a <see cref="T:IPA.Utilities.Ref`1" />.
		/// </summary>
		/// <typeparam name="T">the type to reference.</typeparam>
		/// <param name="val">the default value.</param>
		/// <returns>the new <see cref="T:IPA.Utilities.Ref`1" />.</returns>
		// Token: 0x06000076 RID: 118 RVA: 0x00003698 File Offset: 0x00001898
		public static Ref<T> Create<T>(T val)
		{
			return new Ref<T>(val);
		}
	}
}
