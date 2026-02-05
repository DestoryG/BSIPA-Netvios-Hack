using System;
using System.Collections.Generic;
using BeatSaberMarkupLanguage.Parser;
using IPA.Logging;
using UnityEngine.UI;

namespace BeatSaberMarkupLanguage.TypeHandlers
{
	// Token: 0x02000017 RID: 23
	[ComponentHandler(typeof(Button))]
	public class ButtonHandler : TypeHandler<Button>
	{
		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000B1 RID: 177 RVA: 0x00004F3C File Offset: 0x0000313C
		public override Dictionary<string, string[]> Props
		{
			get
			{
				return new Dictionary<string, string[]>
				{
					{
						"onClick",
						new string[] { "on-click" }
					},
					{
						"clickEvent",
						new string[] { "click-event", "event-click" }
					},
					{
						"interactable",
						new string[] { "interactable" }
					}
				};
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000B2 RID: 178 RVA: 0x00004FA3 File Offset: 0x000031A3
		public override Dictionary<string, Action<Button, string>> Setters
		{
			get
			{
				return new Dictionary<string, Action<Button, string>> { 
				{
					"interactable",
					new Action<Button, string>(ButtonHandler.SetInteractable)
				} };
			}
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00004FC4 File Offset: 0x000031C4
		public override void HandleType(BSMLParser.ComponentTypeWithData componentType, BSMLParserParams parserParams)
		{
			try
			{
				Button button = componentType.component as Button;
				string onClick;
				if (componentType.data.TryGetValue("onClick", out onClick))
				{
					button.onClick.AddListener(delegate
					{
						BSMLAction bsmlaction;
						if (!parserParams.actions.TryGetValue(onClick, out bsmlaction))
						{
							throw new Exception("on-click action '" + onClick + "' not found");
						}
						bsmlaction.Invoke(Array.Empty<object>());
					});
				}
				string clickEvent;
				if (componentType.data.TryGetValue("clickEvent", out clickEvent))
				{
					button.onClick.AddListener(delegate
					{
						parserParams.EmitEvent(clickEvent);
					});
				}
				base.HandleType(componentType, parserParams);
			}
			catch (Exception ex)
			{
				Logger log = Logger.log;
				if (log != null)
				{
					log.Error(ex);
				}
			}
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x0000507C File Offset: 0x0000327C
		public static void SetInteractable(Button button, string flag)
		{
			button.interactable = Parse.Bool(flag);
		}
	}
}
