using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Xml;

namespace System.Runtime.Serialization
{
	// Token: 0x020000DA RID: 218
	public static class XPathQueryGenerator
	{
		// Token: 0x06000C27 RID: 3111 RVA: 0x0003454D File Offset: 0x0003274D
		public static string CreateFromDataContractSerializer(Type type, MemberInfo[] pathToMember, out XmlNamespaceManager namespaces)
		{
			return XPathQueryGenerator.CreateFromDataContractSerializer(type, pathToMember, null, out namespaces);
		}

		// Token: 0x06000C28 RID: 3112 RVA: 0x00034558 File Offset: 0x00032758
		public static string CreateFromDataContractSerializer(Type type, MemberInfo[] pathToMember, StringBuilder rootElementXpath, out XmlNamespaceManager namespaces)
		{
			if (type == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("type"));
			}
			if (pathToMember == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("pathToMember"));
			}
			DataContract dataContract = DataContract.GetDataContract(type);
			XPathQueryGenerator.ExportContext exportContext;
			if (rootElementXpath == null)
			{
				exportContext = new XPathQueryGenerator.ExportContext(dataContract);
			}
			else
			{
				exportContext = new XPathQueryGenerator.ExportContext(rootElementXpath);
			}
			for (int i = 0; i < pathToMember.Length; i++)
			{
				dataContract = XPathQueryGenerator.ProcessDataContract(dataContract, exportContext, pathToMember[i]);
			}
			namespaces = exportContext.Namespaces;
			return exportContext.XPath;
		}

		// Token: 0x06000C29 RID: 3113 RVA: 0x000345D2 File Offset: 0x000327D2
		private static DataContract ProcessDataContract(DataContract contract, XPathQueryGenerator.ExportContext context, MemberInfo memberNode)
		{
			if (contract is ClassDataContract)
			{
				return XPathQueryGenerator.ProcessClassDataContract((ClassDataContract)contract, context, memberNode);
			}
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("The path to member was not found for XPath query generator.")));
		}

		// Token: 0x06000C2A RID: 3114 RVA: 0x00034600 File Offset: 0x00032800
		private static DataContract ProcessClassDataContract(ClassDataContract contract, XPathQueryGenerator.ExportContext context, MemberInfo memberNode)
		{
			string text = context.SetNamespace(contract.Namespace.Value);
			foreach (DataMember dataMember in XPathQueryGenerator.GetDataMembers(contract))
			{
				if (dataMember.MemberInfo.Name == memberNode.Name && dataMember.MemberInfo.DeclaringType.IsAssignableFrom(memberNode.DeclaringType))
				{
					context.WriteChildToContext(dataMember, text);
					return dataMember.MemberTypeContract;
				}
			}
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("The path to member was not found for XPath query generator.")));
		}

		// Token: 0x06000C2B RID: 3115 RVA: 0x000346B0 File Offset: 0x000328B0
		private static IEnumerable<DataMember> GetDataMembers(ClassDataContract contract)
		{
			if (contract.BaseContract != null)
			{
				foreach (DataMember dataMember in XPathQueryGenerator.GetDataMembers(contract.BaseContract))
				{
					yield return dataMember;
				}
				IEnumerator<DataMember> enumerator = null;
			}
			if (contract.Members != null)
			{
				foreach (DataMember dataMember2 in contract.Members)
				{
					yield return dataMember2;
				}
				List<DataMember>.Enumerator enumerator2 = default(List<DataMember>.Enumerator);
			}
			yield break;
			yield break;
		}

		// Token: 0x040004F9 RID: 1273
		private const string XPathSeparator = "/";

		// Token: 0x040004FA RID: 1274
		private const string NsSeparator = ":";

		// Token: 0x02000179 RID: 377
		private class ExportContext
		{
			// Token: 0x060014C3 RID: 5315 RVA: 0x00054290 File Offset: 0x00052490
			public ExportContext(DataContract rootContract)
			{
				this.namespaces = new XmlNamespaceManager(new NameTable());
				string text = this.SetNamespace(rootContract.TopLevelElementNamespace.Value);
				this.xPathBuilder = new StringBuilder("/" + text + ":" + rootContract.TopLevelElementName.Value);
			}

			// Token: 0x060014C4 RID: 5316 RVA: 0x000542EB File Offset: 0x000524EB
			public ExportContext(StringBuilder rootContractXPath)
			{
				this.namespaces = new XmlNamespaceManager(new NameTable());
				this.xPathBuilder = rootContractXPath;
			}

			// Token: 0x060014C5 RID: 5317 RVA: 0x0005430A File Offset: 0x0005250A
			public void WriteChildToContext(DataMember contextMember, string prefix)
			{
				this.xPathBuilder.Append("/" + prefix + ":" + contextMember.Name);
			}

			// Token: 0x17000452 RID: 1106
			// (get) Token: 0x060014C6 RID: 5318 RVA: 0x0005432E File Offset: 0x0005252E
			public XmlNamespaceManager Namespaces
			{
				get
				{
					return this.namespaces;
				}
			}

			// Token: 0x17000453 RID: 1107
			// (get) Token: 0x060014C7 RID: 5319 RVA: 0x00054336 File Offset: 0x00052536
			public string XPath
			{
				get
				{
					return this.xPathBuilder.ToString();
				}
			}

			// Token: 0x060014C8 RID: 5320 RVA: 0x00054344 File Offset: 0x00052544
			public string SetNamespace(string ns)
			{
				string text = this.namespaces.LookupPrefix(ns);
				if (text == null || text.Length == 0)
				{
					string text2 = "xg";
					int num = this.nextPrefix;
					this.nextPrefix = num + 1;
					text = text2 + num.ToString(NumberFormatInfo.InvariantInfo);
					this.Namespaces.AddNamespace(text, ns);
				}
				return text;
			}

			// Token: 0x04000A25 RID: 2597
			private XmlNamespaceManager namespaces;

			// Token: 0x04000A26 RID: 2598
			private int nextPrefix;

			// Token: 0x04000A27 RID: 2599
			private StringBuilder xPathBuilder;
		}
	}
}
