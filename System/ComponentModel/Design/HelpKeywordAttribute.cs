using System;

namespace System.ComponentModel.Design
{
	// Token: 0x020005E1 RID: 1505
	[AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = false)]
	[Serializable]
	public sealed class HelpKeywordAttribute : Attribute
	{
		// Token: 0x060037CC RID: 14284 RVA: 0x000F0D9D File Offset: 0x000EEF9D
		public HelpKeywordAttribute()
		{
		}

		// Token: 0x060037CD RID: 14285 RVA: 0x000F0DA5 File Offset: 0x000EEFA5
		public HelpKeywordAttribute(string keyword)
		{
			if (keyword == null)
			{
				throw new ArgumentNullException("keyword");
			}
			this.contextKeyword = keyword;
		}

		// Token: 0x060037CE RID: 14286 RVA: 0x000F0DC2 File Offset: 0x000EEFC2
		public HelpKeywordAttribute(Type t)
		{
			if (t == null)
			{
				throw new ArgumentNullException("t");
			}
			this.contextKeyword = t.FullName;
		}

		// Token: 0x17000D6A RID: 3434
		// (get) Token: 0x060037CF RID: 14287 RVA: 0x000F0DEA File Offset: 0x000EEFEA
		public string HelpKeyword
		{
			get
			{
				return this.contextKeyword;
			}
		}

		// Token: 0x060037D0 RID: 14288 RVA: 0x000F0DF2 File Offset: 0x000EEFF2
		public override bool Equals(object obj)
		{
			return obj == this || (obj != null && obj is HelpKeywordAttribute && ((HelpKeywordAttribute)obj).HelpKeyword == this.HelpKeyword);
		}

		// Token: 0x060037D1 RID: 14289 RVA: 0x000F0E1D File Offset: 0x000EF01D
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x060037D2 RID: 14290 RVA: 0x000F0E25 File Offset: 0x000EF025
		public override bool IsDefaultAttribute()
		{
			return this.Equals(HelpKeywordAttribute.Default);
		}

		// Token: 0x04002AFF RID: 11007
		public static readonly HelpKeywordAttribute Default = new HelpKeywordAttribute();

		// Token: 0x04002B00 RID: 11008
		private string contextKeyword;
	}
}
