using System;
using System.Collections;
using System.Collections.Generic;

namespace IniParser.Model
{
	// Token: 0x0200000C RID: 12
	public class SectionDataCollection : ICloneable, IEnumerable<SectionData>, IEnumerable
	{
		// Token: 0x0600007A RID: 122 RVA: 0x0000385D File Offset: 0x00001A5D
		public SectionDataCollection()
			: this(EqualityComparer<string>.Default)
		{
		}

		// Token: 0x0600007B RID: 123 RVA: 0x0000386C File Offset: 0x00001A6C
		public SectionDataCollection(IEqualityComparer<string> searchComparer)
		{
			this._searchComparer = searchComparer;
			this._sectionData = new Dictionary<string, SectionData>(this._searchComparer);
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00003890 File Offset: 0x00001A90
		public SectionDataCollection(SectionDataCollection ori, IEqualityComparer<string> searchComparer)
		{
			this._searchComparer = searchComparer ?? EqualityComparer<string>.Default;
			this._sectionData = new Dictionary<string, SectionData>(this._searchComparer);
			foreach (SectionData sectionData in ori)
			{
				this._sectionData.Add(sectionData.SectionName, (SectionData)sectionData.Clone());
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600007D RID: 125 RVA: 0x0000391C File Offset: 0x00001B1C
		public int Count
		{
			get
			{
				return this._sectionData.Count;
			}
		}

		// Token: 0x1700001A RID: 26
		public KeyDataCollection this[string sectionName]
		{
			get
			{
				bool flag = this._sectionData.ContainsKey(sectionName);
				KeyDataCollection keyDataCollection;
				if (flag)
				{
					keyDataCollection = this._sectionData[sectionName].Keys;
				}
				else
				{
					keyDataCollection = null;
				}
				return keyDataCollection;
			}
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00003974 File Offset: 0x00001B74
		public bool AddSection(string keyName)
		{
			bool flag = !this.ContainsSection(keyName);
			bool flag2;
			if (flag)
			{
				this._sectionData.Add(keyName, new SectionData(keyName, this._searchComparer));
				flag2 = true;
			}
			else
			{
				flag2 = false;
			}
			return flag2;
		}

		// Token: 0x06000080 RID: 128 RVA: 0x000039B4 File Offset: 0x00001BB4
		public void Add(SectionData data)
		{
			bool flag = this.ContainsSection(data.SectionName);
			if (flag)
			{
				this.SetSectionData(data.SectionName, new SectionData(data, this._searchComparer));
			}
			else
			{
				this._sectionData.Add(data.SectionName, new SectionData(data, this._searchComparer));
			}
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00003A0F File Offset: 0x00001C0F
		public void Clear()
		{
			this._sectionData.Clear();
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00003A20 File Offset: 0x00001C20
		public bool ContainsSection(string keyName)
		{
			return this._sectionData.ContainsKey(keyName);
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00003A40 File Offset: 0x00001C40
		public SectionData GetSectionData(string sectionName)
		{
			bool flag = this._sectionData.ContainsKey(sectionName);
			SectionData sectionData;
			if (flag)
			{
				sectionData = this._sectionData[sectionName];
			}
			else
			{
				sectionData = null;
			}
			return sectionData;
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00003A74 File Offset: 0x00001C74
		public void Merge(SectionDataCollection sectionsToMerge)
		{
			foreach (SectionData sectionData in sectionsToMerge)
			{
				SectionData sectionData2 = this.GetSectionData(sectionData.SectionName);
				bool flag = sectionData2 == null;
				if (flag)
				{
					this.AddSection(sectionData.SectionName);
				}
				this[sectionData.SectionName].Merge(sectionData.Keys);
			}
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00003AF8 File Offset: 0x00001CF8
		public void SetSectionData(string sectionName, SectionData data)
		{
			bool flag = data != null;
			if (flag)
			{
				this._sectionData[sectionName] = data;
			}
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00003B1C File Offset: 0x00001D1C
		public bool RemoveSection(string keyName)
		{
			return this._sectionData.Remove(keyName);
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00003B3A File Offset: 0x00001D3A
		public IEnumerator<SectionData> GetEnumerator()
		{
			foreach (string sectionName in this._sectionData.Keys)
			{
				yield return this._sectionData[sectionName];
				sectionName = null;
			}
			Dictionary<string, SectionData>.KeyCollection.Enumerator enumerator = default(Dictionary<string, SectionData>.KeyCollection.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00003B4C File Offset: 0x00001D4C
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00003B64 File Offset: 0x00001D64
		public object Clone()
		{
			return new SectionDataCollection(this, this._searchComparer);
		}

		// Token: 0x04000018 RID: 24
		private IEqualityComparer<string> _searchComparer;

		// Token: 0x04000019 RID: 25
		private readonly Dictionary<string, SectionData> _sectionData;
	}
}
