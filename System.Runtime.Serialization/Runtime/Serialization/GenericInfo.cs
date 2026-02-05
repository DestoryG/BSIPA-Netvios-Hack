using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace System.Runtime.Serialization
{
	// Token: 0x02000070 RID: 112
	internal class GenericInfo : IGenericNameProvider
	{
		// Token: 0x0600087F RID: 2175 RVA: 0x00027DE2 File Offset: 0x00025FE2
		internal GenericInfo(XmlQualifiedName stableName, string genericTypeName)
		{
			this.stableName = stableName;
			this.genericTypeName = genericTypeName;
			this.nestedParamCounts = new List<int>();
			this.nestedParamCounts.Add(0);
		}

		// Token: 0x06000880 RID: 2176 RVA: 0x00027E0F File Offset: 0x0002600F
		internal void Add(GenericInfo actualParamInfo)
		{
			if (this.paramGenericInfos == null)
			{
				this.paramGenericInfos = new List<GenericInfo>();
			}
			this.paramGenericInfos.Add(actualParamInfo);
		}

		// Token: 0x06000881 RID: 2177 RVA: 0x00027E30 File Offset: 0x00026030
		internal void AddToLevel(int level, int count)
		{
			if (level >= this.nestedParamCounts.Count)
			{
				do
				{
					this.nestedParamCounts.Add((level == this.nestedParamCounts.Count) ? count : 0);
				}
				while (level >= this.nestedParamCounts.Count);
				return;
			}
			this.nestedParamCounts[level] = this.nestedParamCounts[level] + count;
		}

		// Token: 0x06000882 RID: 2178 RVA: 0x00027E91 File Offset: 0x00026091
		internal XmlQualifiedName GetExpandedStableName()
		{
			if (this.paramGenericInfos == null)
			{
				return this.stableName;
			}
			return new XmlQualifiedName(DataContract.EncodeLocalName(DataContract.ExpandGenericParameters(XmlConvert.DecodeName(this.stableName.Name), this)), this.stableName.Namespace);
		}

		// Token: 0x06000883 RID: 2179 RVA: 0x00027ECD File Offset: 0x000260CD
		internal string GetStableNamespace()
		{
			return this.stableName.Namespace;
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x06000884 RID: 2180 RVA: 0x00027EDA File Offset: 0x000260DA
		internal XmlQualifiedName StableName
		{
			get
			{
				return this.stableName;
			}
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x06000885 RID: 2181 RVA: 0x00027EE2 File Offset: 0x000260E2
		internal IList<GenericInfo> Parameters
		{
			get
			{
				return this.paramGenericInfos;
			}
		}

		// Token: 0x06000886 RID: 2182 RVA: 0x00027EEA File Offset: 0x000260EA
		public int GetParameterCount()
		{
			return this.paramGenericInfos.Count;
		}

		// Token: 0x06000887 RID: 2183 RVA: 0x00027EF7 File Offset: 0x000260F7
		public IList<int> GetNestedParameterCounts()
		{
			return this.nestedParamCounts;
		}

		// Token: 0x06000888 RID: 2184 RVA: 0x00027EFF File Offset: 0x000260FF
		public string GetParameterName(int paramIndex)
		{
			return this.paramGenericInfos[paramIndex].GetExpandedStableName().Name;
		}

		// Token: 0x06000889 RID: 2185 RVA: 0x00027F18 File Offset: 0x00026118
		public string GetNamespaces()
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < this.paramGenericInfos.Count; i++)
			{
				stringBuilder.Append(" ").Append(this.paramGenericInfos[i].GetStableNamespace());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600088A RID: 2186 RVA: 0x00027F69 File Offset: 0x00026169
		public string GetGenericTypeName()
		{
			return this.genericTypeName;
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x0600088B RID: 2187 RVA: 0x00027F74 File Offset: 0x00026174
		public bool ParametersFromBuiltInNamespaces
		{
			get
			{
				bool flag = true;
				int num = 0;
				while (num < this.paramGenericInfos.Count && flag)
				{
					flag = DataContract.IsBuiltInNamespace(this.paramGenericInfos[num].GetStableNamespace());
					num++;
				}
				return flag;
			}
		}

		// Token: 0x0400031B RID: 795
		private string genericTypeName;

		// Token: 0x0400031C RID: 796
		private XmlQualifiedName stableName;

		// Token: 0x0400031D RID: 797
		private List<GenericInfo> paramGenericInfos;

		// Token: 0x0400031E RID: 798
		private List<int> nestedParamCounts;
	}
}
