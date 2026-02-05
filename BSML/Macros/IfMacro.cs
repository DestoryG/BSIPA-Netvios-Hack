using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using BeatSaberMarkupLanguage.Parser;
using UnityEngine;

namespace BeatSaberMarkupLanguage.Macros
{
	// Token: 0x0200008D RID: 141
	public class IfMacro : BSMLMacro
	{
		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x060002C0 RID: 704 RVA: 0x0000DB08 File Offset: 0x0000BD08
		public override string[] Aliases
		{
			get
			{
				return new string[] { "if" };
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x060002C1 RID: 705 RVA: 0x0000DB18 File Offset: 0x0000BD18
		public override Dictionary<string, string[]> Props
		{
			get
			{
				return new Dictionary<string, string[]> { 
				{
					"value",
					new string[] { "bool", "value" }
				} };
			}
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x0000DB50 File Offset: 0x0000BD50
		public override void Execute(XmlNode node, GameObject parent, Dictionary<string, string> data, BSMLParserParams parserParams, out IEnumerable<BSMLParser.ComponentTypeWithData> components)
		{
			components = Enumerable.Empty<BSMLParser.ComponentTypeWithData>();
			string text;
			if (data.TryGetValue("value", out text))
			{
				bool flag = false;
				if (text.StartsWith("!"))
				{
					flag = true;
					text = text.Substring(1);
				}
				BSMLValue bsmlvalue;
				if (!parserParams.values.TryGetValue(text, out bsmlvalue))
				{
					throw new Exception("value '" + text + "' not found");
				}
				if ((bool)bsmlvalue.GetValue() != flag)
				{
					foreach (object obj in node.ChildNodes)
					{
						XmlNode xmlNode = (XmlNode)obj;
						IEnumerable<BSMLParser.ComponentTypeWithData> enumerable;
						PersistentSingleton<BSMLParser>.instance.HandleNode(xmlNode, parent, parserParams, out enumerable);
						components = components.Concat(enumerable);
					}
				}
			}
		}
	}
}
