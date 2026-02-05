using System;
using System.Collections.Generic;

namespace IniParser.Model
{
	// Token: 0x0200000B RID: 11
	public class SectionData : ICloneable
	{
		// Token: 0x0600006A RID: 106 RVA: 0x000035BC File Offset: 0x000017BC
		public SectionData(string sectionName)
			: this(sectionName, EqualityComparer<string>.Default)
		{
		}

		// Token: 0x0600006B RID: 107 RVA: 0x000035CC File Offset: 0x000017CC
		public SectionData(string sectionName, IEqualityComparer<string> searchComparer)
		{
			this._trailingComments = new List<string>();
			base..ctor();
			this._searchComparer = searchComparer;
			bool flag = string.IsNullOrEmpty(sectionName);
			if (flag)
			{
				throw new ArgumentException("section name can not be empty");
			}
			this._leadingComments = new List<string>();
			this._keyDataCollection = new KeyDataCollection(this._searchComparer);
			this.SectionName = sectionName;
		}

		// Token: 0x0600006C RID: 108 RVA: 0x0000362C File Offset: 0x0000182C
		public SectionData(SectionData ori, IEqualityComparer<string> searchComparer = null)
		{
			this._trailingComments = new List<string>();
			base..ctor();
			this.SectionName = ori.SectionName;
			this._searchComparer = searchComparer;
			this._leadingComments = new List<string>(ori._leadingComments);
			this._keyDataCollection = new KeyDataCollection(ori._keyDataCollection, searchComparer ?? ori._searchComparer);
		}

		// Token: 0x0600006D RID: 109 RVA: 0x0000368D File Offset: 0x0000188D
		public void ClearComments()
		{
			this.LeadingComments.Clear();
			this.TrailingComments.Clear();
			this.Keys.ClearComments();
		}

		// Token: 0x0600006E RID: 110 RVA: 0x000036B4 File Offset: 0x000018B4
		public void ClearKeyData()
		{
			this.Keys.RemoveAllKeys();
		}

		// Token: 0x0600006F RID: 111 RVA: 0x000036C4 File Offset: 0x000018C4
		public void Merge(SectionData toMergeSection)
		{
			foreach (string text in toMergeSection.LeadingComments)
			{
				this.LeadingComments.Add(text);
			}
			this.Keys.Merge(toMergeSection.Keys);
			foreach (string text2 in toMergeSection.TrailingComments)
			{
				this.TrailingComments.Add(text2);
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000070 RID: 112 RVA: 0x0000377C File Offset: 0x0000197C
		// (set) Token: 0x06000071 RID: 113 RVA: 0x00003794 File Offset: 0x00001994
		public string SectionName
		{
			get
			{
				return this._sectionName;
			}
			set
			{
				bool flag = !string.IsNullOrEmpty(value);
				if (flag)
				{
					this._sectionName = value;
				}
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000072 RID: 114 RVA: 0x000037B8 File Offset: 0x000019B8
		// (set) Token: 0x06000073 RID: 115 RVA: 0x000037D0 File Offset: 0x000019D0
		[Obsolete("Do not use this property, use property Comments instead")]
		public List<string> LeadingComments
		{
			get
			{
				return this._leadingComments;
			}
			internal set
			{
				this._leadingComments = new List<string>(value);
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000074 RID: 116 RVA: 0x000037E0 File Offset: 0x000019E0
		public List<string> Comments
		{
			get
			{
				return this._leadingComments;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000075 RID: 117 RVA: 0x000037F8 File Offset: 0x000019F8
		// (set) Token: 0x06000076 RID: 118 RVA: 0x00003810 File Offset: 0x00001A10
		[Obsolete("Do not use this property, use property Comments instead")]
		public List<string> TrailingComments
		{
			get
			{
				return this._trailingComments;
			}
			internal set
			{
				this._trailingComments = new List<string>(value);
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000077 RID: 119 RVA: 0x00003820 File Offset: 0x00001A20
		// (set) Token: 0x06000078 RID: 120 RVA: 0x00003838 File Offset: 0x00001A38
		public KeyDataCollection Keys
		{
			get
			{
				return this._keyDataCollection;
			}
			set
			{
				this._keyDataCollection = value;
			}
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00003844 File Offset: 0x00001A44
		public object Clone()
		{
			return new SectionData(this, null);
		}

		// Token: 0x04000013 RID: 19
		private IEqualityComparer<string> _searchComparer;

		// Token: 0x04000014 RID: 20
		private List<string> _leadingComments;

		// Token: 0x04000015 RID: 21
		private List<string> _trailingComments;

		// Token: 0x04000016 RID: 22
		private KeyDataCollection _keyDataCollection;

		// Token: 0x04000017 RID: 23
		private string _sectionName;
	}
}
