using System;

namespace IPA.Config.Data
{
	/// <summary>
	/// A <see cref="P:IPA.Config.Data.Text.Value" /> representing a piece of text. The only reason this is not named 
	/// String is so that it doesn't conflict with <see cref="T:System.String" />.
	/// </summary>
	// Token: 0x02000096 RID: 150
	public sealed class Text : Value
	{
		/// <summary>
		/// The actual value of this <see cref="T:IPA.Config.Data.Text" /> object.
		/// </summary>
		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x060003C6 RID: 966 RVA: 0x00013554 File Offset: 0x00011754
		// (set) Token: 0x060003C7 RID: 967 RVA: 0x0001355C File Offset: 0x0001175C
		public string Value { get; set; }

		/// <summary>
		/// Converts this <see cref="T:IPA.Config.Data.Value" /> into a human-readable format.
		/// </summary>
		/// <returns>a quoted, unescaped string form of <see cref="P:IPA.Config.Data.Text.Value" /></returns>
		// Token: 0x060003C8 RID: 968 RVA: 0x00013565 File Offset: 0x00011765
		public override string ToString()
		{
			return "\"" + this.Value + "\"";
		}
	}
}
