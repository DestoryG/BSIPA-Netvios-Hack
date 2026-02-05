using System;
using IPA.Config.Data;
using IPA.Config.Stores;
using IPA.Config.Stores.Converters;

namespace IPA.Utilities
{
	/// <summary>
	/// A <see cref="T:IPA.Config.Stores.ValueConverter`1" /> for <see cref="T:IPA.Utilities.AlmostVersion" />s.
	/// </summary>
	// Token: 0x02000017 RID: 23
	public sealed class AlmostVersionConverter : ValueConverter<AlmostVersion>
	{
		/// <summary>
		/// Converts a <see cref="T:IPA.Config.Data.Text" /> node into an <see cref="T:IPA.Utilities.AlmostVersion" />.
		/// </summary>
		/// <param name="value">the <see cref="T:IPA.Config.Data.Text" /> node to convert</param>
		/// <param name="parent">the owner of the new object</param>
		/// <returns></returns>
		// Token: 0x06000063 RID: 99 RVA: 0x00003410 File Offset: 0x00001610
		public override AlmostVersion FromValue(Value value, object parent)
		{
			return new AlmostVersion(Converter<string>.Default.FromValue(value, parent));
		}

		/// <summary>
		/// Converts an <see cref="T:IPA.Utilities.AlmostVersion" /> to a <see cref="T:IPA.Config.Data.Text" /> node.
		/// </summary>
		/// <param name="obj">the <see cref="T:IPA.Utilities.AlmostVersion" /> to convert</param>
		/// <param name="parent">the parent of <paramref name="obj" /></param>
		/// <returns>a <see cref="T:IPA.Config.Data.Text" /> node representing <paramref name="obj" /></returns>
		// Token: 0x06000064 RID: 100 RVA: 0x00003423 File Offset: 0x00001623
		public override Value ToValue(AlmostVersion obj, object parent)
		{
			return Value.From(obj.ToString());
		}
	}
}
