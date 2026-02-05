using System;
using System.Globalization;

namespace System.Collections.Specialized
{
	// Token: 0x020003AF RID: 943
	[Serializable]
	internal class CompatibleComparer : IEqualityComparer
	{
		// Token: 0x0600234B RID: 9035 RVA: 0x000A7549 File Offset: 0x000A5749
		internal CompatibleComparer(IComparer comparer, IHashCodeProvider hashCodeProvider)
		{
			this._comparer = comparer;
			this._hcp = hashCodeProvider;
		}

		// Token: 0x0600234C RID: 9036 RVA: 0x000A7560 File Offset: 0x000A5760
		public bool Equals(object a, object b)
		{
			if (a == b)
			{
				return true;
			}
			if (a == null || b == null)
			{
				return false;
			}
			try
			{
				if (this._comparer != null)
				{
					return this._comparer.Compare(a, b) == 0;
				}
				IComparable comparable = a as IComparable;
				if (comparable != null)
				{
					return comparable.CompareTo(b) == 0;
				}
			}
			catch (ArgumentException)
			{
				return false;
			}
			return a.Equals(b);
		}

		// Token: 0x0600234D RID: 9037 RVA: 0x000A75D0 File Offset: 0x000A57D0
		public int GetHashCode(object obj)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			if (this._hcp != null)
			{
				return this._hcp.GetHashCode(obj);
			}
			return obj.GetHashCode();
		}

		// Token: 0x170008F1 RID: 2289
		// (get) Token: 0x0600234E RID: 9038 RVA: 0x000A75FB File Offset: 0x000A57FB
		public IComparer Comparer
		{
			get
			{
				return this._comparer;
			}
		}

		// Token: 0x170008F2 RID: 2290
		// (get) Token: 0x0600234F RID: 9039 RVA: 0x000A7603 File Offset: 0x000A5803
		public IHashCodeProvider HashCodeProvider
		{
			get
			{
				return this._hcp;
			}
		}

		// Token: 0x170008F3 RID: 2291
		// (get) Token: 0x06002350 RID: 9040 RVA: 0x000A760B File Offset: 0x000A580B
		public static IComparer DefaultComparer
		{
			get
			{
				if (CompatibleComparer.defaultComparer == null)
				{
					CompatibleComparer.defaultComparer = new CaseInsensitiveComparer(CultureInfo.InvariantCulture);
				}
				return CompatibleComparer.defaultComparer;
			}
		}

		// Token: 0x170008F4 RID: 2292
		// (get) Token: 0x06002351 RID: 9041 RVA: 0x000A762E File Offset: 0x000A582E
		public static IHashCodeProvider DefaultHashCodeProvider
		{
			get
			{
				if (CompatibleComparer.defaultHashProvider == null)
				{
					CompatibleComparer.defaultHashProvider = new CaseInsensitiveHashCodeProvider(CultureInfo.InvariantCulture);
				}
				return CompatibleComparer.defaultHashProvider;
			}
		}

		// Token: 0x04001FCA RID: 8138
		private IComparer _comparer;

		// Token: 0x04001FCB RID: 8139
		private static volatile IComparer defaultComparer;

		// Token: 0x04001FCC RID: 8140
		private IHashCodeProvider _hcp;

		// Token: 0x04001FCD RID: 8141
		private static volatile IHashCodeProvider defaultHashProvider;
	}
}
