using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using BeatSaberMarkupLanguage.Parser;
using UnityEngine;

namespace BeatSaberMarkupLanguage.Macros
{
	// Token: 0x0200008B RID: 139
	public class DefineMacro : BSMLMacro
	{
		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x060002B8 RID: 696 RVA: 0x0000D906 File Offset: 0x0000BB06
		public override string[] Aliases
		{
			get
			{
				return new string[] { "define" };
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x060002B9 RID: 697 RVA: 0x0000D918 File Offset: 0x0000BB18
		public override Dictionary<string, string[]> Props
		{
			get
			{
				return new Dictionary<string, string[]>
				{
					{
						"name",
						new string[] { "name", "id" }
					},
					{
						"value",
						new string[] { "value" }
					}
				};
			}
		}

		// Token: 0x060002BA RID: 698 RVA: 0x0000D968 File Offset: 0x0000BB68
		public override void Execute(XmlNode node, GameObject parent, Dictionary<string, string> data, BSMLParserParams parserParams, out IEnumerable<BSMLParser.ComponentTypeWithData> components)
		{
			components = Enumerable.Empty<BSMLParser.ComponentTypeWithData>();
			string text;
			if (!data.TryGetValue("name", out text))
			{
				throw new Exception("define macro must have an id");
			}
			string text2;
			if (!data.TryGetValue("value", out text2))
			{
				throw new Exception("define macro must have a value");
			}
			BSMLValue bsmlvalue;
			if (parserParams.values.TryGetValue(text, out bsmlvalue))
			{
				bsmlvalue.SetValue(text2);
				return;
			}
			parserParams.values.Add(text, new BSMLStringValue(text2));
		}
	}
}
