using System;

namespace IPA.Config.Data
{
	/// <summary>
	/// A <see cref="P:IPA.Config.Data.FloatingPoint.Value" /> representing a floating point value. This may hold a 
	/// <see cref="T:System.Decimal" />'s  worth of data.
	/// </summary>
	// Token: 0x02000098 RID: 152
	public sealed class FloatingPoint : Value
	{
		/// <summary>
		/// The actual value fo this <see cref="T:IPA.Config.Data.FloatingPoint" /> object.
		/// </summary>
		// Token: 0x170000AB RID: 171
		// (get) Token: 0x060003CF RID: 975 RVA: 0x000135CB File Offset: 0x000117CB
		// (set) Token: 0x060003D0 RID: 976 RVA: 0x000135D3 File Offset: 0x000117D3
		public decimal Value { get; set; }

		/// <summary>
		/// Coerces this <see cref="T:IPA.Config.Data.FloatingPoint" /> into an <see cref="T:IPA.Config.Data.Integer" />.
		/// </summary>
		/// <returns>a <see cref="T:IPA.Config.Data.Integer" /> representing the closest approximation of <see cref="P:IPA.Config.Data.FloatingPoint.Value" /></returns>
		// Token: 0x060003D1 RID: 977 RVA: 0x000135DC File Offset: 0x000117DC
		public Integer AsInteger()
		{
			return IPA.Config.Data.Value.Integer((long)this.Value);
		}

		/// <summary>
		/// Converts this <see cref="T:IPA.Config.Data.Value" /> into a human-readable format.
		/// </summary>
		/// <returns>the result of <c>Value.ToString()</c></returns>
		// Token: 0x060003D2 RID: 978 RVA: 0x000135F0 File Offset: 0x000117F0
		public override string ToString()
		{
			return this.Value.ToString();
		}
	}
}
