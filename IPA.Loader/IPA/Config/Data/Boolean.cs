using System;

namespace IPA.Config.Data
{
	/// <summary>
	/// A <see cref="P:IPA.Config.Data.Boolean.Value" /> representing a boolean value.
	/// </summary>
	// Token: 0x02000099 RID: 153
	public sealed class Boolean : Value
	{
		/// <summary>
		/// The actual value fo this <see cref="T:IPA.Config.Data.Boolean" /> object.
		/// </summary>
		// Token: 0x170000AC RID: 172
		// (get) Token: 0x060003D4 RID: 980 RVA: 0x00013613 File Offset: 0x00011813
		// (set) Token: 0x060003D5 RID: 981 RVA: 0x0001361B File Offset: 0x0001181B
		public bool Value { get; set; }

		/// <summary>
		/// Converts this <see cref="T:IPA.Config.Data.Value" /> into a human-readable format.
		/// </summary>
		/// <returns>the result of <c>Value.ToString().ToLower()</c></returns>
		// Token: 0x060003D6 RID: 982 RVA: 0x00013624 File Offset: 0x00011824
		public override string ToString()
		{
			return this.Value.ToString().ToLower();
		}
	}
}
