using System;

namespace IPA.Old
{
	/// <inheritdoc cref="T:IPA.Old.IPlugin" />
	/// <summary>
	/// An enhanced version of the standard IPA plugin.
	/// </summary>
	// Token: 0x02000026 RID: 38
	[Obsolete("When building plugins for Beat Saber, use IPA.IEnhancedPlugin")]
	public interface IEnhancedPlugin : IPlugin
	{
		/// <summary>
		/// Gets a list of executables this plugin should be executed on (without the file ending)
		/// </summary>
		/// <example>{ "PlayClub", "PlayClubStudio" }</example>
		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060000D8 RID: 216
		string[] Filter { get; }

		/// <summary>
		/// Called after Update.
		/// </summary>
		// Token: 0x060000D9 RID: 217
		void OnLateUpdate();
	}
}
