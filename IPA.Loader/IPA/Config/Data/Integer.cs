using System;

namespace IPA.Config.Data
{
	/// <summary>
	/// A <see cref="P:IPA.Config.Data.Integer.Value" /> representing an integer. This may hold a <see cref="T:System.Int64" />'s 
	/// worth of data.
	/// </summary>
	// Token: 0x02000097 RID: 151
	public sealed class Integer : Value
	{
		/// <summary>
		/// The actual value of the <see cref="T:IPA.Config.Data.Integer" /> object.
		/// </summary>
		// Token: 0x170000AA RID: 170
		// (get) Token: 0x060003CA RID: 970 RVA: 0x00013584 File Offset: 0x00011784
		// (set) Token: 0x060003CB RID: 971 RVA: 0x0001358C File Offset: 0x0001178C
		public long Value { get; set; }

		/// <summary>
		/// Coerces this <see cref="T:IPA.Config.Data.Integer" /> into a <see cref="T:IPA.Config.Data.FloatingPoint" />.
		/// </summary>
		/// <returns>a <see cref="T:IPA.Config.Data.FloatingPoint" /> representing the closest approximation of <see cref="P:IPA.Config.Data.Integer.Value" /></returns>
		// Token: 0x060003CC RID: 972 RVA: 0x00013595 File Offset: 0x00011795
		public FloatingPoint AsFloat()
		{
			return IPA.Config.Data.Value.Float(this.Value);
		}

		/// <summary>
		/// Converts this <see cref="T:IPA.Config.Data.Value" /> into a human-readable format.
		/// </summary>
		/// <returns>the result of <c>Value.ToString()</c></returns>
		// Token: 0x060003CD RID: 973 RVA: 0x000135A8 File Offset: 0x000117A8
		public override string ToString()
		{
			return this.Value.ToString();
		}
	}
}
