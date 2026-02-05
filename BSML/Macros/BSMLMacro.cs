using System;
using System.Collections.Generic;
using System.Xml;
using BeatSaberMarkupLanguage.Parser;
using UnityEngine;

namespace BeatSaberMarkupLanguage.Macros
{
	// Token: 0x0200008A RID: 138
	public abstract class BSMLMacro
	{
		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x060002B3 RID: 691
		public abstract string[] Aliases { get; }

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x060002B4 RID: 692 RVA: 0x0000D8EA File Offset: 0x0000BAEA
		public Dictionary<string, string[]> CachedProps
		{
			get
			{
				if (this.cachedProps == null)
				{
					this.cachedProps = this.Props;
				}
				return this.cachedProps;
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x060002B5 RID: 693
		public abstract Dictionary<string, string[]> Props { get; }

		// Token: 0x060002B6 RID: 694
		public abstract void Execute(XmlNode node, GameObject parent, Dictionary<string, string> data, BSMLParserParams parserParams, out IEnumerable<BSMLParser.ComponentTypeWithData> childComponentTypes);

		// Token: 0x04000099 RID: 153
		private Dictionary<string, string[]> cachedProps;
	}
}
