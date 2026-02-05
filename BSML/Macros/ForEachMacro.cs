using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using BeatSaberMarkupLanguage.Parser;
using UnityEngine;

namespace BeatSaberMarkupLanguage.Macros
{
	// Token: 0x0200008C RID: 140
	public class ForEachMacro : BSMLMacro
	{
		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x060002BC RID: 700 RVA: 0x0000D9DD File Offset: 0x0000BBDD
		public override string[] Aliases
		{
			get
			{
				return new string[] { "for-each" };
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x060002BD RID: 701 RVA: 0x0000D9F0 File Offset: 0x0000BBF0
		public override Dictionary<string, string[]> Props
		{
			get
			{
				return new Dictionary<string, string[]>
				{
					{
						"hosts",
						new string[] { "hosts", "items" }
					},
					{
						"passTags",
						new string[] { "pass-back-tags" }
					}
				};
			}
		}

		// Token: 0x060002BE RID: 702 RVA: 0x0000DA40 File Offset: 0x0000BC40
		public override void Execute(XmlNode node, GameObject parent, Dictionary<string, string> data, BSMLParserParams parserParams, out IEnumerable<BSMLParser.ComponentTypeWithData> components)
		{
			components = Enumerable.Empty<BSMLParser.ComponentTypeWithData>();
			string text;
			if (data.TryGetValue("hosts", out text))
			{
				BSMLValue bsmlvalue;
				if (!parserParams.values.TryGetValue(text, out bsmlvalue))
				{
					throw new Exception("host list '" + text + "' not found");
				}
				bool flag = false;
				string text2;
				if (data.TryGetValue("passTags", out text2))
				{
					flag = Parse.Bool(text2);
				}
				foreach (object obj in (bsmlvalue.GetValue() as List<object>))
				{
					BSMLParserParams bsmlparserParams = PersistentSingleton<BSMLParser>.instance.Parse(node, parent, obj);
					if (flag)
					{
						bsmlparserParams.PassTaggedObjects(parserParams);
					}
				}
			}
		}
	}
}
