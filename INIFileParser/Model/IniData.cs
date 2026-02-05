using System;
using IniParser.Model.Configuration;
using IniParser.Model.Formatting;

namespace IniParser.Model
{
	// Token: 0x02000008 RID: 8
	public class IniData : ICloneable
	{
		// Token: 0x06000037 RID: 55 RVA: 0x00002BC2 File Offset: 0x00000DC2
		public IniData()
			: this(new SectionDataCollection())
		{
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002BD1 File Offset: 0x00000DD1
		public IniData(SectionDataCollection sdc)
		{
			this._sections = (SectionDataCollection)sdc.Clone();
			this.Global = new KeyDataCollection();
			this.SectionKeySeparator = '.';
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002C01 File Offset: 0x00000E01
		public IniData(IniData ori)
			: this(ori.Sections)
		{
			this.Global = (KeyDataCollection)ori.Global.Clone();
			this.Configuration = ori.Configuration.Clone();
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600003A RID: 58 RVA: 0x00002C3C File Offset: 0x00000E3C
		// (set) Token: 0x0600003B RID: 59 RVA: 0x00002C6C File Offset: 0x00000E6C
		public IniParserConfiguration Configuration
		{
			get
			{
				bool flag = this._configuration == null;
				if (flag)
				{
					this._configuration = new IniParserConfiguration();
				}
				return this._configuration;
			}
			set
			{
				this._configuration = value.Clone();
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600003C RID: 60 RVA: 0x00002C7B File Offset: 0x00000E7B
		// (set) Token: 0x0600003D RID: 61 RVA: 0x00002C83 File Offset: 0x00000E83
		public KeyDataCollection Global { get; protected set; }

		// Token: 0x1700000C RID: 12
		public KeyDataCollection this[string sectionName]
		{
			get
			{
				bool flag = !this._sections.ContainsSection(sectionName);
				if (flag)
				{
					bool allowCreateSectionsOnFly = this.Configuration.AllowCreateSectionsOnFly;
					if (!allowCreateSectionsOnFly)
					{
						return null;
					}
					this._sections.AddSection(sectionName);
				}
				return this._sections[sectionName];
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600003F RID: 63 RVA: 0x00002CE0 File Offset: 0x00000EE0
		// (set) Token: 0x06000040 RID: 64 RVA: 0x00002CF8 File Offset: 0x00000EF8
		public SectionDataCollection Sections
		{
			get
			{
				return this._sections;
			}
			set
			{
				this._sections = value;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000041 RID: 65 RVA: 0x00002D02 File Offset: 0x00000F02
		// (set) Token: 0x06000042 RID: 66 RVA: 0x00002D0A File Offset: 0x00000F0A
		public char SectionKeySeparator { get; set; }

		// Token: 0x06000043 RID: 67 RVA: 0x00002D14 File Offset: 0x00000F14
		public override string ToString()
		{
			return this.ToString(new DefaultIniDataFormatter(this.Configuration));
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00002D38 File Offset: 0x00000F38
		public virtual string ToString(IIniDataFormatter formatter)
		{
			return formatter.IniDataToString(this);
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00002D54 File Offset: 0x00000F54
		public object Clone()
		{
			return new IniData(this);
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00002D6C File Offset: 0x00000F6C
		public void ClearAllComments()
		{
			this.Global.ClearComments();
			foreach (SectionData sectionData in this.Sections)
			{
				sectionData.ClearComments();
			}
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00002DCC File Offset: 0x00000FCC
		public void Merge(IniData toMergeIniData)
		{
			bool flag = toMergeIniData == null;
			if (!flag)
			{
				this.Global.Merge(toMergeIniData.Global);
				this.Sections.Merge(toMergeIniData.Sections);
			}
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00002E08 File Offset: 0x00001008
		public bool TryGetKey(string key, out string value)
		{
			value = string.Empty;
			bool flag = string.IsNullOrEmpty(key);
			bool flag2;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				string[] array = key.Split(new char[] { this.SectionKeySeparator });
				int num = array.Length - 1;
				bool flag3 = num > 1;
				if (flag3)
				{
					throw new ArgumentException("key contains multiple separators", "key");
				}
				bool flag4 = num == 0;
				if (flag4)
				{
					bool flag5 = !this.Global.ContainsKey(key);
					if (flag5)
					{
						flag2 = false;
					}
					else
					{
						value = this.Global[key];
						flag2 = true;
					}
				}
				else
				{
					string text = array[0];
					key = array[1];
					bool flag6 = !this._sections.ContainsSection(text);
					if (flag6)
					{
						flag2 = false;
					}
					else
					{
						KeyDataCollection keyDataCollection = this._sections[text];
						bool flag7 = !keyDataCollection.ContainsKey(key);
						if (flag7)
						{
							flag2 = false;
						}
						else
						{
							value = keyDataCollection[key];
							flag2 = true;
						}
					}
				}
			}
			return flag2;
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00002EF4 File Offset: 0x000010F4
		public string GetKey(string key)
		{
			string text;
			return this.TryGetKey(key, out text) ? text : null;
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00002F18 File Offset: 0x00001118
		private void MergeSection(SectionData otherSection)
		{
			bool flag = !this.Sections.ContainsSection(otherSection.SectionName);
			if (flag)
			{
				this.Sections.AddSection(otherSection.SectionName);
			}
			this.Sections.GetSectionData(otherSection.SectionName).Merge(otherSection);
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00002F6C File Offset: 0x0000116C
		private void MergeGlobal(KeyDataCollection globals)
		{
			foreach (KeyData keyData in globals)
			{
				this.Global[keyData.KeyName] = keyData.Value;
			}
		}

		// Token: 0x0400000A RID: 10
		private SectionDataCollection _sections;

		// Token: 0x0400000D RID: 13
		private IniParserConfiguration _configuration;
	}
}
