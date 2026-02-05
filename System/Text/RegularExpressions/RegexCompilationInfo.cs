using System;
using System.Runtime.Serialization;

namespace System.Text.RegularExpressions
{
	// Token: 0x02000691 RID: 1681
	[Serializable]
	public class RegexCompilationInfo
	{
		// Token: 0x06003E28 RID: 15912 RVA: 0x001007A1 File Offset: 0x000FE9A1
		[OnDeserializing]
		private void InitMatchTimeoutDefaultForOldVersionDeserialization(StreamingContext unusedContext)
		{
			this.matchTimeout = Regex.DefaultMatchTimeout;
		}

		// Token: 0x06003E29 RID: 15913 RVA: 0x001007AE File Offset: 0x000FE9AE
		public RegexCompilationInfo(string pattern, RegexOptions options, string name, string fullnamespace, bool ispublic)
			: this(pattern, options, name, fullnamespace, ispublic, Regex.DefaultMatchTimeout)
		{
		}

		// Token: 0x06003E2A RID: 15914 RVA: 0x001007C2 File Offset: 0x000FE9C2
		public RegexCompilationInfo(string pattern, RegexOptions options, string name, string fullnamespace, bool ispublic, TimeSpan matchTimeout)
		{
			this.Pattern = pattern;
			this.Name = name;
			this.Namespace = fullnamespace;
			this.options = options;
			this.isPublic = ispublic;
			this.MatchTimeout = matchTimeout;
		}

		// Token: 0x17000EB6 RID: 3766
		// (get) Token: 0x06003E2B RID: 15915 RVA: 0x001007F7 File Offset: 0x000FE9F7
		// (set) Token: 0x06003E2C RID: 15916 RVA: 0x001007FF File Offset: 0x000FE9FF
		public string Pattern
		{
			get
			{
				return this.pattern;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.pattern = value;
			}
		}

		// Token: 0x17000EB7 RID: 3767
		// (get) Token: 0x06003E2D RID: 15917 RVA: 0x00100816 File Offset: 0x000FEA16
		// (set) Token: 0x06003E2E RID: 15918 RVA: 0x0010081E File Offset: 0x000FEA1E
		public RegexOptions Options
		{
			get
			{
				return this.options;
			}
			set
			{
				this.options = value;
			}
		}

		// Token: 0x17000EB8 RID: 3768
		// (get) Token: 0x06003E2F RID: 15919 RVA: 0x00100827 File Offset: 0x000FEA27
		// (set) Token: 0x06003E30 RID: 15920 RVA: 0x00100830 File Offset: 0x000FEA30
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (value.Length == 0)
				{
					throw new ArgumentException(SR.GetString("InvalidNullEmptyArgument", new object[] { "value" }), "value");
				}
				this.name = value;
			}
		}

		// Token: 0x17000EB9 RID: 3769
		// (get) Token: 0x06003E31 RID: 15921 RVA: 0x0010087D File Offset: 0x000FEA7D
		// (set) Token: 0x06003E32 RID: 15922 RVA: 0x00100885 File Offset: 0x000FEA85
		public string Namespace
		{
			get
			{
				return this.nspace;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.nspace = value;
			}
		}

		// Token: 0x17000EBA RID: 3770
		// (get) Token: 0x06003E33 RID: 15923 RVA: 0x0010089C File Offset: 0x000FEA9C
		// (set) Token: 0x06003E34 RID: 15924 RVA: 0x001008A4 File Offset: 0x000FEAA4
		public bool IsPublic
		{
			get
			{
				return this.isPublic;
			}
			set
			{
				this.isPublic = value;
			}
		}

		// Token: 0x17000EBB RID: 3771
		// (get) Token: 0x06003E35 RID: 15925 RVA: 0x001008AD File Offset: 0x000FEAAD
		// (set) Token: 0x06003E36 RID: 15926 RVA: 0x001008B5 File Offset: 0x000FEAB5
		public TimeSpan MatchTimeout
		{
			get
			{
				return this.matchTimeout;
			}
			set
			{
				Regex.ValidateMatchTimeout(value);
				this.matchTimeout = value;
			}
		}

		// Token: 0x04002D64 RID: 11620
		private string pattern;

		// Token: 0x04002D65 RID: 11621
		private RegexOptions options;

		// Token: 0x04002D66 RID: 11622
		private string name;

		// Token: 0x04002D67 RID: 11623
		private string nspace;

		// Token: 0x04002D68 RID: 11624
		private bool isPublic;

		// Token: 0x04002D69 RID: 11625
		[OptionalField(VersionAdded = 2)]
		private TimeSpan matchTimeout;
	}
}
