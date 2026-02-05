using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace System.Runtime.Serialization
{
	// Token: 0x0200006F RID: 111
	internal class GenericNameProvider : IGenericNameProvider
	{
		// Token: 0x06000876 RID: 2166 RVA: 0x00027C68 File Offset: 0x00025E68
		internal GenericNameProvider(Type type)
		{
			string clrTypeFullName = DataContract.GetClrTypeFullName(type.GetGenericTypeDefinition());
			object[] genericArguments = type.GetGenericArguments();
			this..ctor(clrTypeFullName, genericArguments);
		}

		// Token: 0x06000877 RID: 2167 RVA: 0x00027C90 File Offset: 0x00025E90
		internal GenericNameProvider(string genericTypeName, object[] genericParams)
		{
			this.genericTypeName = genericTypeName;
			this.genericParams = new object[genericParams.Length];
			genericParams.CopyTo(this.genericParams, 0);
			string text;
			string text2;
			DataContract.GetClrNameAndNamespace(genericTypeName, out text, out text2);
			this.nestedParamCounts = DataContract.GetDataContractNameForGenericName(text, null);
		}

		// Token: 0x06000878 RID: 2168 RVA: 0x00027CDC File Offset: 0x00025EDC
		public int GetParameterCount()
		{
			return this.genericParams.Length;
		}

		// Token: 0x06000879 RID: 2169 RVA: 0x00027CE6 File Offset: 0x00025EE6
		public IList<int> GetNestedParameterCounts()
		{
			return this.nestedParamCounts;
		}

		// Token: 0x0600087A RID: 2170 RVA: 0x00027CEE File Offset: 0x00025EEE
		public string GetParameterName(int paramIndex)
		{
			return this.GetStableName(paramIndex).Name;
		}

		// Token: 0x0600087B RID: 2171 RVA: 0x00027CFC File Offset: 0x00025EFC
		public string GetNamespaces()
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < this.GetParameterCount(); i++)
			{
				stringBuilder.Append(" ").Append(this.GetStableName(i).Namespace);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600087C RID: 2172 RVA: 0x00027D43 File Offset: 0x00025F43
		public string GetGenericTypeName()
		{
			return this.genericTypeName;
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x0600087D RID: 2173 RVA: 0x00027D4C File Offset: 0x00025F4C
		public bool ParametersFromBuiltInNamespaces
		{
			get
			{
				bool flag = true;
				int num = 0;
				while (num < this.GetParameterCount() && flag)
				{
					flag = DataContract.IsBuiltInNamespace(this.GetStableName(num).Namespace);
					num++;
				}
				return flag;
			}
		}

		// Token: 0x0600087E RID: 2174 RVA: 0x00027D84 File Offset: 0x00025F84
		private XmlQualifiedName GetStableName(int i)
		{
			object obj = this.genericParams[i];
			XmlQualifiedName xmlQualifiedName = obj as XmlQualifiedName;
			if (xmlQualifiedName == null)
			{
				Type type = obj as Type;
				if (type != null)
				{
					xmlQualifiedName = (this.genericParams[i] = DataContract.GetStableName(type));
				}
				else
				{
					xmlQualifiedName = (this.genericParams[i] = ((DataContract)obj).StableName);
				}
			}
			return xmlQualifiedName;
		}

		// Token: 0x04000318 RID: 792
		private string genericTypeName;

		// Token: 0x04000319 RID: 793
		private object[] genericParams;

		// Token: 0x0400031A RID: 794
		private IList<int> nestedParamCounts;
	}
}
