using System;
using System.Collections.Generic;
using BeatSaberMarkupLanguage.Parser;
using HMUI;
using IPA.Logging;
using UnityEngine;

namespace BeatSaberMarkupLanguage.TypeHandlers
{
	// Token: 0x02000029 RID: 41
	[ComponentHandler(typeof(ModalView))]
	public class ModalViewHandler : TypeHandler
	{
		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060000F0 RID: 240 RVA: 0x00006C8C File Offset: 0x00004E8C
		public override Dictionary<string, string[]> Props
		{
			get
			{
				return new Dictionary<string, string[]>
				{
					{
						"showEvent",
						new string[] { "show-event" }
					},
					{
						"hideEvent",
						new string[] { "hide-event" }
					},
					{
						"clickOffCloses",
						new string[] { "click-off-closes", "clickerino-offerino-closerino" }
					},
					{
						"moveToCenter",
						new string[] { "move-to-center" }
					}
				};
			}
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00006D0C File Offset: 0x00004F0C
		public override void HandleType(BSMLParser.ComponentTypeWithData componentType, BSMLParserParams parserParams)
		{
			try
			{
				ModalView modalView = componentType.component as ModalView;
				Transform originalParent = modalView.transform.parent;
				bool moveToCenter = true;
				string text;
				if (componentType.data.TryGetValue("moveToCenter", out text))
				{
					moveToCenter = bool.Parse(text);
				}
				string text2;
				if (componentType.data.TryGetValue("showEvent", out text2))
				{
					parserParams.AddEvent(text2, delegate
					{
						modalView.Show(true, moveToCenter, null);
					});
				}
				string text3;
				if (componentType.data.TryGetValue("hideEvent", out text3))
				{
					parserParams.AddEvent(text3, delegate
					{
						modalView.Hide(true, delegate
						{
							modalView.transform.SetParent(originalParent, true);
						});
					});
				}
				string text4;
				if (componentType.data.TryGetValue("clickOffCloses", out text4) && Parse.Bool(text4))
				{
					modalView._blockerClickedEvent += delegate
					{
						modalView.Hide(true, delegate
						{
							modalView.transform.SetParent(originalParent, true);
						});
					};
				}
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
	}
}
