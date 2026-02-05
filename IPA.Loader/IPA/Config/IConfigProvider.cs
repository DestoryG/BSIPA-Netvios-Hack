using System;
using System.IO;
using IPA.Config.Data;

namespace IPA.Config
{
	/// <summary>
	/// An interface for configuration providers.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Implementers must provide a default constructor. Do not assume that <see cref="T:System.IO.File" /> will ever be set for a given object.
	/// </para>
	/// <para>
	/// Implementers are expected to preserve the typing of values passed to <see cref="M:IPA.Config.IConfigProvider.Store(IPA.Config.Data.Value,System.IO.FileInfo)" /> when returned from <see cref="M:IPA.Config.IConfigProvider.Load(System.IO.FileInfo)" />.
	/// The only exceptions to this are the numeric types, <see cref="T:IPA.Config.Data.Integer" /> and <see cref="T:IPA.Config.Data.FloatingPoint" />, since they can be coerced
	/// to each other with <see cref="M:IPA.Config.Data.Integer.AsFloat" /> and <see cref="M:IPA.Config.Data.FloatingPoint.AsInteger" /> respectively. The provider <i>should</i>
	/// however store and recover <see cref="T:IPA.Config.Data.Integer" /> with as much precision as is possible. For example, a JSON provider may decide to
	/// decode all numbers that have an integral value, even if they were originally <see cref="T:IPA.Config.Data.FloatingPoint" />, as <see cref="T:IPA.Config.Data.Integer" />.
	/// This is reasonable, as <see cref="T:IPA.Config.Data.Integer" /> is more precise, particularly with larger values, than <see cref="T:IPA.Config.Data.FloatingPoint" />.
	/// </para>
	/// </remarks>
	// Token: 0x0200005D RID: 93
	public interface IConfigProvider
	{
		/// <summary>
		/// Gets the extension <i>without</i> a dot to use for files handled by this provider.
		/// </summary>
		/// <remarks>
		/// This must work immediately, and is used to generate the <see cref="T:System.IO.FileInfo" /> used to set
		/// <see cref="T:System.IO.File" />.
		/// </remarks>
		// Token: 0x17000084 RID: 132
		// (get) Token: 0x060002A0 RID: 672
		string Extension { get; }

		/// <summary>
		/// Stores the <see cref="T:IPA.Config.Data.Value" /> given to disk in the format specified.
		/// </summary>
		/// <param name="value">the <see cref="T:IPA.Config.Data.Value" /> to store</param>
		/// <param name="file">the file to write to</param>
		// Token: 0x060002A1 RID: 673
		void Store(Value value, FileInfo file);

		/// <summary>
		/// Loads a <see cref="T:IPA.Config.Data.Value" /> from disk in whatever format this provider provides
		/// and returns it.
		/// </summary>
		/// <param name="file">the file to read from</param>
		/// <returns>the <see cref="T:IPA.Config.Data.Value" /> loaded</returns>
		// Token: 0x060002A2 RID: 674
		Value Load(FileInfo file);
	}
}
