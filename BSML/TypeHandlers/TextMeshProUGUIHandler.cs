using System;
using System.Collections.Generic;
using IPA.Logging;
using TMPro;
using UnityEngine;

namespace BeatSaberMarkupLanguage.TypeHandlers
{
	// Token: 0x02000031 RID: 49
	[ComponentHandler(typeof(TextMeshProUGUI))]
	public class TextMeshProUGUIHandler : TypeHandler<TextMeshProUGUI>
	{
		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000112 RID: 274 RVA: 0x000079F0 File Offset: 0x00005BF0
		public override Dictionary<string, string[]> Props
		{
			get
			{
				return new Dictionary<string, string[]>
				{
					{
						"text",
						new string[] { "text" }
					},
					{
						"fontSize",
						new string[] { "font-size" }
					},
					{
						"color",
						new string[] { "font-color", "color" }
					},
					{
						"faceColor",
						new string[] { "face-color" }
					},
					{
						"outlineColor",
						new string[] { "outline-color" }
					},
					{
						"outlineWidth",
						new string[] { "outline-width" }
					},
					{
						"richText",
						new string[] { "rich-text" }
					},
					{
						"alignment",
						new string[] { "font-align", "align" }
					},
					{
						"overflowMode",
						new string[] { "overflow-mode" }
					},
					{
						"wordWrapping",
						new string[] { "word-wrapping" }
					},
					{
						"bold",
						new string[] { "bold" }
					},
					{
						"italics",
						new string[] { "italics" }
					},
					{
						"underlined",
						new string[] { "underlined" }
					},
					{
						"strikethrough",
						new string[] { "strikethrough" }
					},
					{
						"allUppercase",
						new string[] { "all-uppercase" }
					}
				};
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000113 RID: 275 RVA: 0x00007B8C File Offset: 0x00005D8C
		public override Dictionary<string, Action<TextMeshProUGUI, string>> Setters
		{
			get
			{
				Dictionary<string, Action<TextMeshProUGUI, string>> dictionary = new Dictionary<string, Action<TextMeshProUGUI, string>>();
				dictionary.Add("text", delegate(TextMeshProUGUI textMesh, string value)
				{
					textMesh.text = value;
				});
				dictionary.Add("fontSize", delegate(TextMeshProUGUI textMesh, string value)
				{
					textMesh.fontSize = Parse.Float(value);
				});
				dictionary.Add("color", delegate(TextMeshProUGUI textMesh, string value)
				{
					textMesh.color = TextMeshProUGUIHandler.GetColor(value);
				});
				dictionary.Add("faceColor", delegate(TextMeshProUGUI textMesh, string value)
				{
					textMesh.faceColor = TextMeshProUGUIHandler.GetColor(value);
				});
				dictionary.Add("outlineColor", delegate(TextMeshProUGUI textMesh, string value)
				{
					textMesh.outlineColor = TextMeshProUGUIHandler.GetColor(value);
				});
				dictionary.Add("outlineWidth", delegate(TextMeshProUGUI textMesh, string value)
				{
					textMesh.outlineWidth = Parse.Float(value);
				});
				dictionary.Add("richText", delegate(TextMeshProUGUI textMesh, string value)
				{
					textMesh.richText = Parse.Bool(value);
				});
				dictionary.Add("alignment", delegate(TextMeshProUGUI textMesh, string value)
				{
					textMesh.alignment = (TextAlignmentOptions)Enum.Parse(typeof(TextAlignmentOptions), value);
				});
				dictionary.Add("overflowMode", delegate(TextMeshProUGUI textMesh, string value)
				{
					textMesh.overflowMode = (TextOverflowModes)Enum.Parse(typeof(TextOverflowModes), value);
				});
				dictionary.Add("wordWrapping", delegate(TextMeshProUGUI textMesh, string value)
				{
					textMesh.enableWordWrapping = Parse.Bool(value);
				});
				dictionary.Add("bold", delegate(TextMeshProUGUI textMesh, string value)
				{
					textMesh.fontStyle = TextMeshProUGUIHandler.SetStyle(textMesh.fontStyle, 1, value);
				});
				dictionary.Add("italics", delegate(TextMeshProUGUI textMesh, string value)
				{
					textMesh.fontStyle = TextMeshProUGUIHandler.SetStyle(textMesh.fontStyle, 2, value);
				});
				dictionary.Add("underlined", delegate(TextMeshProUGUI textMesh, string value)
				{
					textMesh.fontStyle = TextMeshProUGUIHandler.SetStyle(textMesh.fontStyle, 4, value);
				});
				dictionary.Add("strikethrough", delegate(TextMeshProUGUI textMesh, string value)
				{
					textMesh.fontStyle = TextMeshProUGUIHandler.SetStyle(textMesh.fontStyle, 64, value);
				});
				dictionary.Add("allUppercase", delegate(TextMeshProUGUI textMesh, string value)
				{
					textMesh.fontStyle = TextMeshProUGUIHandler.SetStyle(textMesh.fontStyle, 16, value);
				});
				return dictionary;
			}
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00007E14 File Offset: 0x00006014
		private static FontStyles SetStyle(FontStyles existing, FontStyles modifyStyle, string flag)
		{
			if (Parse.Bool(flag))
			{
				return existing |= modifyStyle;
			}
			return existing &= ~modifyStyle;
		}

		// Token: 0x06000115 RID: 277 RVA: 0x00007E2C File Offset: 0x0000602C
		private static Color GetColor(string colorStr)
		{
			Color color;
			if (ColorUtility.TryParseHtmlString(colorStr, ref color))
			{
				return color;
			}
			Logger log = Logger.log;
			if (log != null)
			{
				log.Warn("Color " + colorStr + ", is not a valid color.");
			}
			return Color.white;
		}
	}
}
