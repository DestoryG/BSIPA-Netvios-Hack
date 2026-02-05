using System;
using System.Collections.Generic;
using BeatSaberMarkupLanguage.Parser;

namespace BeatSaberMarkupLanguage.TypeHandlers
{
	// Token: 0x02000034 RID: 52
	public abstract class TypeHandler
	{
		// Token: 0x17000056 RID: 86
		// (get) Token: 0x0600011D RID: 285 RVA: 0x0000800B File Offset: 0x0000620B
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

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x0600011E RID: 286
		public abstract Dictionary<string, string[]> Props { get; }

		// Token: 0x0600011F RID: 287
		public abstract void HandleType(BSMLParser.ComponentTypeWithData componentType, BSMLParserParams parserParams);

		// Token: 0x06000120 RID: 288 RVA: 0x0000263A File Offset: 0x0000083A
		public virtual void HandleTypeAfterChildren(BSMLParser.ComponentTypeWithData componentType, BSMLParserParams parserParams)
		{
		}

		// Token: 0x06000121 RID: 289 RVA: 0x0000263A File Offset: 0x0000083A
		public virtual void HandleTypeAfterParse(BSMLParser.ComponentTypeWithData componentType, BSMLParserParams parserParams)
		{
		}

		// Token: 0x04000033 RID: 51
		private Dictionary<string, string[]> cachedProps;
	}
}
