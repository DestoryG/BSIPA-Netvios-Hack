using System;

namespace IPA.Loader
{
	/// <summary>
	/// An enum that represents several categories of ignore reasons that the loader may encounter.
	/// </summary>
	/// <seealso cref="T:IPA.Loader.IgnoreReason" />
	// Token: 0x02000046 RID: 70
	public enum Reason
	{
		/// <summary>
		/// An error was thrown either loading plugin information fomr disk, or when initializing the plugin.
		/// </summary>
		/// <remarks>
		/// When this is the set <see cref="T:IPA.Loader.Reason" /> in an <see cref="T:IPA.Loader.IgnoreReason" /> structure, the member
		/// <see cref="P:IPA.Loader.IgnoreReason.Error" /> will contain the thrown exception.
		/// </remarks>
		// Token: 0x040000AC RID: 172
		Error,
		/// <summary>
		/// The plugin this reason is associated with has the same ID as another plugin whose information was
		/// already loaded.
		/// </summary>
		/// <remarks>
		/// When this is the set <see cref="T:IPA.Loader.Reason" /> in an <see cref="T:IPA.Loader.IgnoreReason" /> structure, the member
		/// <see cref="P:IPA.Loader.IgnoreReason.RelatedTo" /> will contain the metadata of the already loaded plugin.
		/// </remarks>
		// Token: 0x040000AD RID: 173
		Duplicate,
		/// <summary>
		/// The plugin this reason is associated with conflicts with another already loaded plugin.
		/// </summary>
		/// <remarks>
		/// When this is the set <see cref="T:IPA.Loader.Reason" /> in an <see cref="T:IPA.Loader.IgnoreReason" /> structure, the member
		/// <see cref="P:IPA.Loader.IgnoreReason.RelatedTo" /> will contain the metadata of the plugin it conflicts with.
		/// </remarks>
		// Token: 0x040000AE RID: 174
		Conflict,
		/// <summary>
		/// The plugin this reason is assiciated with is missing a dependency.
		/// </summary>
		/// <remarks>
		/// Since this is only given when a dependency is missing, <see cref="P:IPA.Loader.IgnoreReason.RelatedTo" /> will
		/// not be set.
		/// </remarks>
		// Token: 0x040000AF RID: 175
		Dependency,
		/// <summary>
		/// The plugin this reason is associated with was released for a game update, but is still considered
		/// present for the purposes of updating.
		/// </summary>
		// Token: 0x040000B0 RID: 176
		Released,
		/// <summary>
		/// The plugin this reason is associated with was denied from loading by a <see cref="T:IPA.Loader.Features.Feature" />
		/// that it marks.
		/// </summary>
		// Token: 0x040000B1 RID: 177
		Feature,
		/// <summary>
		/// The plugin this reason is assoicated with is unsupported.
		/// </summary>
		/// <remarks>
		/// Currently, there is no path in the loader that emits this <see cref="T:IPA.Loader.Reason" />, however there may
		/// be in the future.
		/// </remarks>
		// Token: 0x040000B2 RID: 178
		Unsupported,
		/// <summary>
		/// One of the files that a plugin declared in its manifest is missing.
		/// </summary>
		// Token: 0x040000B3 RID: 179
		MissingFiles
	}
}
