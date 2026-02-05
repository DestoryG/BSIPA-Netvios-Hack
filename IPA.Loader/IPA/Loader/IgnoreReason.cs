using System;
using System.Collections.Generic;

namespace IPA.Loader
{
	/// <summary>
	/// A structure describing the reason that a plugin was ignored.
	/// </summary>
	// Token: 0x02000047 RID: 71
	public struct IgnoreReason
	{
		/// <summary>
		/// Gets the ignore reason, as represented by the <see cref="T:IPA.Loader.Reason" /> enum.
		/// </summary>
		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060001D0 RID: 464 RVA: 0x0000A5B8 File Offset: 0x000087B8
		public readonly Reason Reason { get; }

		/// <summary>
		/// Gets the textual description of the particular ignore reason. This will typically
		/// include details about why the plugin was ignored, if it is present.
		/// </summary>
		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060001D1 RID: 465 RVA: 0x0000A5C0 File Offset: 0x000087C0
		// (set) Token: 0x060001D2 RID: 466 RVA: 0x0000A5C8 File Offset: 0x000087C8
		public string ReasonText { readonly get; internal set; }

		/// <summary>
		/// Gets the <see cref="T:System.Exception" /> that caused this plugin to be ignored, if any.
		/// </summary>
		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060001D3 RID: 467 RVA: 0x0000A5D1 File Offset: 0x000087D1
		// (set) Token: 0x060001D4 RID: 468 RVA: 0x0000A5D9 File Offset: 0x000087D9
		public Exception Error { readonly get; internal set; }

		/// <summary>
		/// Gets the metadata of the plugin that this ignore was related to, if any.
		/// </summary>
		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060001D5 RID: 469 RVA: 0x0000A5E2 File Offset: 0x000087E2
		// (set) Token: 0x060001D6 RID: 470 RVA: 0x0000A5EA File Offset: 0x000087EA
		public PluginMetadata RelatedTo { readonly get; internal set; }

		/// <summary>
		/// Initializes an <see cref="T:IPA.Loader.IgnoreReason" /> with the provided data.
		/// </summary>
		/// <param name="reason">the <see cref="T:IPA.Loader.Reason" /> enum value that describes this reason</param>
		/// <param name="reasonText">the textual description of this ignore reason, if any</param>
		/// <param name="error">the <see cref="T:System.Exception" /> that caused this <see cref="T:IPA.Loader.IgnoreReason" />, if any</param>
		/// <param name="relatedTo">the <see cref="T:IPA.Loader.PluginMetadata" /> this reason is related to, if any</param>
		// Token: 0x060001D7 RID: 471 RVA: 0x0000A5F3 File Offset: 0x000087F3
		public IgnoreReason(Reason reason, string reasonText = null, Exception error = null, PluginMetadata relatedTo = null)
		{
			this.Reason = reason;
			this.ReasonText = reasonText;
			this.Error = error;
			this.RelatedTo = relatedTo;
		}

		/// <inheritdoc />
		// Token: 0x060001D8 RID: 472 RVA: 0x0000A614 File Offset: 0x00008814
		public override bool Equals(object obj)
		{
			if (obj is IgnoreReason)
			{
				IgnoreReason ir = (IgnoreReason)obj;
				return this.Equals(ir);
			}
			return false;
		}

		/// <summary>
		/// Compares this <see cref="T:IPA.Loader.IgnoreReason" /> with <paramref name="other" /> for equality.
		/// </summary>
		/// <param name="other">the reason to compare to</param>
		/// <returns><see langword="true" /> if the two reasons compare equal, <see langword="false" /> otherwise</returns>
		// Token: 0x060001D9 RID: 473 RVA: 0x0000A63C File Offset: 0x0000883C
		public bool Equals(IgnoreReason other)
		{
			return this.Reason == other.Reason && this.ReasonText == other.ReasonText && this.Error == other.Error && this.RelatedTo == other.RelatedTo;
		}

		/// <inheritdoc />
		// Token: 0x060001DA RID: 474 RVA: 0x0000A68C File Offset: 0x0000888C
		public override int GetHashCode()
		{
			return (((778404373 * -1521134295 + this.Reason.GetHashCode()) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.ReasonText)) * -1521134295 + EqualityComparer<Exception>.Default.GetHashCode(this.Error)) * -1521134295 + EqualityComparer<PluginMetadata>.Default.GetHashCode(this.RelatedTo);
		}

		/// <summary>
		/// Checks if two <see cref="T:IPA.Loader.IgnoreReason" />s are equal.
		/// </summary>
		/// <param name="left">the first <see cref="T:IPA.Loader.IgnoreReason" /> to compare</param>
		/// <param name="right">the second <see cref="T:IPA.Loader.IgnoreReason" /> to compare</param>
		/// <returns><see langword="true" /> if the two reasons compare equal, <see langword="false" /> otherwise</returns>
		// Token: 0x060001DB RID: 475 RVA: 0x0000A6FE File Offset: 0x000088FE
		public static bool operator ==(IgnoreReason left, IgnoreReason right)
		{
			return left.Equals(right);
		}

		/// <summary>
		/// Checks if two <see cref="T:IPA.Loader.IgnoreReason" />s are not equal.
		/// </summary>
		/// <param name="left">the first <see cref="T:IPA.Loader.IgnoreReason" /> to compare</param>
		/// <param name="right">the second <see cref="T:IPA.Loader.IgnoreReason" /> to compare</param>
		/// <returns><see langword="true" /> if the two reasons are not equal, <see langword="false" /> otherwise</returns>
		// Token: 0x060001DC RID: 476 RVA: 0x0000A708 File Offset: 0x00008908
		public static bool operator !=(IgnoreReason left, IgnoreReason right)
		{
			return !(left == right);
		}
	}
}
