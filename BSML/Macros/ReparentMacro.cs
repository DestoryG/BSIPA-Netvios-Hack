using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using BeatSaberMarkupLanguage.Parser;
using UnityEngine;

namespace BeatSaberMarkupLanguage.Macros
{
	// Token: 0x0200008E RID: 142
	public class ReparentMacro : BSMLMacro
	{
		// Token: 0x170000CA RID: 202
		// (get) Token: 0x060002C4 RID: 708 RVA: 0x0000DC2C File Offset: 0x0000BE2C
		public override string[] Aliases
		{
			get
			{
				return new string[] { "reparent" };
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x060002C5 RID: 709 RVA: 0x0000DC3C File Offset: 0x0000BE3C
		public override Dictionary<string, string[]> Props
		{
			get
			{
				return new Dictionary<string, string[]>
				{
					{
						"transform",
						new string[] { "transform" }
					},
					{
						"transforms",
						new string[] { "transforms" }
					}
				};
			}
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x0000DC84 File Offset: 0x0000BE84
		public override void Execute(XmlNode node, GameObject parent, Dictionary<string, string> data, BSMLParserParams parserParams, out IEnumerable<BSMLParser.ComponentTypeWithData> components)
		{
			components = Enumerable.Empty<BSMLParser.ComponentTypeWithData>();
			string text;
			if (data.TryGetValue("transform", out text))
			{
				BSMLValue bsmlvalue;
				if (!parserParams.values.TryGetValue(text, out bsmlvalue))
				{
					throw new Exception("transform '" + text + "' not found");
				}
				(bsmlvalue.GetValue() as Transform).SetParent(parent.transform, false);
			}
			string text2;
			if (data.TryGetValue("transforms", out text2))
			{
				BSMLValue bsmlvalue2;
				if (!parserParams.values.TryGetValue(text2, out bsmlvalue2))
				{
					throw new Exception("transform list '" + text2 + "' not found");
				}
				foreach (Transform transform in (bsmlvalue2.GetValue() as List<Transform>))
				{
					transform.SetParent(parent.transform, false);
				}
			}
		}
	}
}
