using System;
using System.Configuration;
using System.Xml;

namespace System.CodeDom.Compiler
{
	// Token: 0x0200066E RID: 1646
	internal class CodeDomConfigurationHandler : IConfigurationSectionHandler
	{
		// Token: 0x06003B9B RID: 15259 RVA: 0x000F6938 File Offset: 0x000F4B38
		internal CodeDomConfigurationHandler()
		{
		}

		// Token: 0x06003B9C RID: 15260 RVA: 0x000F6940 File Offset: 0x000F4B40
		public virtual object Create(object inheritedObject, object configContextObj, XmlNode node)
		{
			return CodeDomCompilationConfiguration.SectionHandler.CreateStatic(inheritedObject, node);
		}
	}
}
