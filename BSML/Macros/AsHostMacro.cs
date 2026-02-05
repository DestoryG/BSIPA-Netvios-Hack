using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using BeatSaberMarkupLanguage.Parser;
using UnityEngine;

namespace BeatSaberMarkupLanguage.Macros
{
	// Token: 0x02000089 RID: 137
	public class AsHostMacro : BSMLMacro
	{
		// Token: 0x170000BF RID: 191
		// (get) Token: 0x060002AF RID: 687 RVA: 0x0000D844 File Offset: 0x0000BA44
		public override string[] Aliases
		{
			get
			{
				return new string[] { "as-host" };
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x060002B0 RID: 688 RVA: 0x0000D854 File Offset: 0x0000BA54
		public override Dictionary<string, string[]> Props
		{
			get
			{
				return new Dictionary<string, string[]> { 
				{
					"host",
					new string[] { "host" }
				} };
			}
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x0000D884 File Offset: 0x0000BA84
		public override void Execute(XmlNode node, GameObject parent, Dictionary<string, string> data, BSMLParserParams parserParams, out IEnumerable<BSMLParser.ComponentTypeWithData> components)
		{
			components = Enumerable.Empty<BSMLParser.ComponentTypeWithData>();
			string text;
			if (data.TryGetValue("host", out text))
			{
				BSMLValue bsmlvalue;
				if (!parserParams.values.TryGetValue(text, out bsmlvalue))
				{
					throw new Exception("host '" + text + "' not found");
				}
				PersistentSingleton<BSMLParser>.instance.Parse(node, parent, bsmlvalue.GetValue());
			}
		}
	}
}
