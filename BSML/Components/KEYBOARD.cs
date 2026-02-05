using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using HMUI;
using IPA.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BeatSaberMarkupLanguage.Components
{
	// Token: 0x020000A2 RID: 162
	public class KEYBOARD
	{
		// Token: 0x14000005 RID: 5
		// (add) Token: 0x0600033E RID: 830 RVA: 0x0000F8A8 File Offset: 0x0000DAA8
		// (remove) Token: 0x0600033F RID: 831 RVA: 0x0000F8E0 File Offset: 0x0000DAE0
		public event Action<string> EnterPressed;

		// Token: 0x170000E1 RID: 225
		public KEYBOARD.KEY this[string index]
		{
			get
			{
				foreach (KEYBOARD.KEY key in this.keys)
				{
					if (key.name == index)
					{
						return key;
					}
				}
				return this.dummy;
			}
		}

		// Token: 0x06000341 RID: 833 RVA: 0x0000F980 File Offset: 0x0000DB80
		public void SetButtonType(string ButtonName = "KeyboardButton")
		{
			this.BaseButton = Resources.FindObjectsOfTypeAll<Button>().First((Button x) => x.name == ButtonName);
			if (this.BaseButton == null)
			{
				this.BaseButton = Resources.FindObjectsOfTypeAll<Button>().First((Button x) => x.name == "KeyboardButton");
			}
		}

		// Token: 0x06000342 RID: 834 RVA: 0x0000F9F4 File Offset: 0x0000DBF4
		public void SetValue(string keylabel, string value)
		{
			foreach (KEYBOARD.KEY key in this.keys)
			{
				if (key.name == keylabel)
				{
					key.value = value;
				}
			}
		}

		// Token: 0x06000343 RID: 835 RVA: 0x0000FA58 File Offset: 0x0000DC58
		public void SetAction(string keyname, Action<KEYBOARD.KEY> action)
		{
			foreach (KEYBOARD.KEY key in this.keys)
			{
				if (key.name == keyname)
				{
					key.keyaction = action;
				}
			}
		}

		// Token: 0x06000344 RID: 836 RVA: 0x0000FABC File Offset: 0x0000DCBC
		private KEYBOARD.KEY AddKey(string keylabel, float width = 12f, float height = 10f, int color = 16777215)
		{
			Vector2 vector = this.currentposition;
			Color white = Color.white;
			white.r = (float)(color & 255) / 255f;
			white.g = (float)((color >> 8) & 255) / 255f;
			white.b = (float)((color >> 16) & 255) / 255f;
			KEYBOARD.KEY key = new KEYBOARD.KEY(this, vector, keylabel, width, height, white);
			this.keys.Add(key);
			return key;
		}

		// Token: 0x06000345 RID: 837 RVA: 0x0000FB36 File Offset: 0x0000DD36
		private KEYBOARD.KEY AddKey(string keylabel, string Shifted, float width = 12f, float height = 10f)
		{
			KEYBOARD.KEY key = this.AddKey(keylabel, width, 10f, 16777215);
			key.shifted = Shifted;
			return key;
		}

		// Token: 0x06000346 RID: 838 RVA: 0x0000FB54 File Offset: 0x0000DD54
		private void EmitKey(ref float spacing, ref float Width, ref string Label, ref string Key, ref bool space, ref string newvalue, ref float height, ref int color)
		{
			this.currentposition.x = this.currentposition.x + spacing;
			if (Label != "")
			{
				this.AddKey(Label, Width, height, color).Set(newvalue);
			}
			else if (Key != "")
			{
				this.AddKey(Key[0].ToString(), Key[1].ToString(), 12f, 10f).Set(newvalue);
			}
			spacing = 0f;
			Width = this.buttonwidth;
			height = 10f;
			Label = "";
			Key = "";
			newvalue = "";
			color = 16777215;
			space = false;
		}

		// Token: 0x06000347 RID: 839 RVA: 0x0000FC20 File Offset: 0x0000DE20
		private bool ReadFloat(ref string data, ref int Position, ref float result)
		{
			if (Position >= data.Length)
			{
				return false;
			}
			int num = Position;
			while (Position < data.Length)
			{
				char c = data[Position];
				if ((c < '0' || c > '9') && c != '+' && c != '-' && c != '.')
				{
					break;
				}
				Position++;
			}
			if (float.TryParse(data.Substring(num, Position - num), out result))
			{
				return true;
			}
			Position = num;
			return false;
		}

		// Token: 0x06000348 RID: 840 RVA: 0x0000FC90 File Offset: 0x0000DE90
		public KEYBOARD AddKeys(string Keyboard, float scale = 0.5f)
		{
			this.scale = scale;
			bool flag = true;
			float num = this.padding;
			float num2 = this.buttonwidth;
			float num3 = 10f;
			string text = "";
			string text2 = "";
			string text3 = "";
			int num4 = 16777215;
			int i = 0;
			try
			{
				while (i < Keyboard.Length)
				{
					char c = Keyboard[i];
					if (c <= '\'')
					{
						if (c <= '\r')
						{
							if (c != '\n')
							{
								if (c != '\r')
								{
									goto IL_033D;
								}
								flag = true;
							}
							else
							{
								this.EmitKey(ref num, ref num2, ref text, ref text2, ref flag, ref text3, ref num3, ref num4);
								flag = true;
								this.NextRow(0f);
							}
						}
						else if (c != ' ')
						{
							if (c == '#')
							{
								i++;
								num4 = int.Parse(Keyboard.Substring(i, 6), NumberStyles.HexNumber);
								i += 6;
								continue;
							}
							if (c != '\'')
							{
								goto IL_033D;
							}
							i++;
							int num5 = i;
							while (i < Keyboard.Length && Keyboard[i] != '\'')
							{
								i++;
							}
							text3 = Keyboard.Substring(num5, i - num5);
						}
						else
						{
							flag = true;
						}
					}
					else if (c <= '/')
					{
						if (c != '(')
						{
							if (c != '/')
							{
								goto IL_033D;
							}
							i++;
							float num6 = 0f;
							if (this.ReadFloat(ref Keyboard, ref i, ref num6))
							{
								if (i < Keyboard.Length && Keyboard[i] == ',')
								{
									i++;
									this.ReadFloat(ref Keyboard, ref i, ref num3);
								}
								if (flag)
								{
									if (text != "" || text2 != "")
									{
										this.EmitKey(ref num, ref num2, ref text, ref text2, ref flag, ref text3, ref num3, ref num4);
									}
									num = num6;
									continue;
								}
								num2 = num6;
								continue;
							}
						}
						else
						{
							this.EmitKey(ref num, ref num2, ref text, ref text2, ref flag, ref text3, ref num3, ref num4);
							i++;
							text2 = Keyboard.Substring(i, 2);
							i += 2;
							flag = false;
						}
					}
					else if (c != '@')
					{
						if (c == 'S')
						{
							this.EmitKey(ref num, ref num2, ref text, ref text2, ref flag, ref text3, ref num3, ref num4);
							i++;
							this.ReadFloat(ref Keyboard, ref i, ref this.scale);
							continue;
						}
						if (c != '[')
						{
							goto IL_033D;
						}
						this.EmitKey(ref num, ref num2, ref text, ref text2, ref flag, ref text3, ref num3, ref num4);
						flag = false;
						i++;
						int num7 = i;
						while (i < Keyboard.Length && Keyboard[i] != ']')
						{
							i++;
						}
						text = Keyboard.Substring(num7, i - num7);
					}
					else
					{
						this.EmitKey(ref num, ref num2, ref text, ref text2, ref flag, ref text3, ref num3, ref num4);
						i++;
						if (!this.ReadFloat(ref Keyboard, ref i, ref this.currentposition.x))
						{
							continue;
						}
						this.baseposition.x = this.currentposition.x;
						if (i < Keyboard.Length && Keyboard[i] == ',')
						{
							i++;
							this.ReadFloat(ref Keyboard, ref i, ref this.currentposition.y);
							this.baseposition.y = this.currentposition.y;
							continue;
						}
						continue;
					}
					i++;
					continue;
					IL_033D:
					return this;
				}
				this.EmitKey(ref num, ref num2, ref text, ref text2, ref flag, ref text3, ref num3, ref num4);
			}
			catch (Exception ex)
			{
				Logger.log.Error(string.Format("Unable to parse keyboard at position {0} : [{1}]", i, Keyboard));
				Logger.log.Error(ex);
			}
			return this;
		}

		// Token: 0x06000349 RID: 841 RVA: 0x00010058 File Offset: 0x0000E258
		public KEYBOARD DefaultActions()
		{
			this.SetAction("CLEAR", new Action<KEYBOARD.KEY>(this.Clear));
			this.SetAction("ENTER", new Action<KEYBOARD.KEY>(this.Enter));
			this.SetAction("<--", new Action<KEYBOARD.KEY>(this.Backspace));
			this.SetAction("SHIFT", new Action<KEYBOARD.KEY>(this.SHIFT));
			this.SetAction("CAPS", new Action<KEYBOARD.KEY>(this.CAPS));
			return this;
		}

		// Token: 0x0600034A RID: 842 RVA: 0x000100DC File Offset: 0x0000E2DC
		public KEYBOARD(RectTransform container, string DefaultKeyboard = "[CLEAR]/20\n(`~) (1!) (2@) (3#) (4$) (5%) (6^) (7&) (8*) (9() (0)) (-_) (=+) [<--]/15\n[TAB]/15'\\t' (qQ) (wW) (eE) (rR) (tT) (yY) (uU) (iI) (oO) (pP) ([{) (]}) (\\|)\n[CAPS]/20 (aA) (sS) (dD) (fF) (gG) (hH) (jJ) (kK) (lL) (;:) ('\") [ENTER]/20,22#20A0D0\n[SHIFT]/25 (zZ) (xX) (cC) (vV) (bB) (nN) (mM) (,<) (.>) (/?)  \n/23 (!!) (@@) [SPACE]/40' ' (##) (__)", bool EnableInputField = true, float x = 0f, float y = 0f)
		{
			this.EnableInputField = EnableInputField;
			this.container = container;
			this.baseposition = new Vector2(-50f + x, 23f + y);
			this.currentposition = this.baseposition;
			this.SetButtonType("KeyboardButton");
			this.KeyboardText = BeatSaberUI.CreateText(container, "", new Vector2(0f, 15f));
			this.KeyboardText.fontSize = 6f;
			this.KeyboardText.color = Color.white;
			this.KeyboardText.alignment = 514;
			this.KeyboardText.enableWordWrapping = false;
			this.KeyboardText.text = "";
			this.KeyboardText.enabled = this.EnableInputField;
			this.KeyboardCursor = BeatSaberUI.CreateText(container, "|", new Vector2(0f, 0f));
			this.KeyboardCursor.fontSize = 6f;
			this.KeyboardCursor.color = new Color(0.6f, 0.8f, 1f);
			this.KeyboardCursor.alignment = 513;
			this.KeyboardCursor.enableWordWrapping = false;
			this.KeyboardCursor.enabled = this.EnableInputField;
			this.DrawCursor();
			if (DefaultKeyboard != "")
			{
				this.AddKeys(DefaultKeyboard, 0.5f);
				this.DefaultActions();
			}
		}

		// Token: 0x0600034B RID: 843 RVA: 0x00010290 File Offset: 0x0000E490
		public KEYBOARD NextRow(float adjustx = 0f)
		{
			this.currentposition.y = this.currentposition.y - (float)((this.currentposition.x == this.baseposition.x) ? 3 : 6);
			this.currentposition.x = this.baseposition.x;
			return this;
		}

		// Token: 0x0600034C RID: 844 RVA: 0x000102E0 File Offset: 0x0000E4E0
		public KEYBOARD SetScale(float scale)
		{
			this.scale = scale;
			return this;
		}

		// Token: 0x0600034D RID: 845 RVA: 0x000102EA File Offset: 0x0000E4EA
		public void Clear(KEYBOARD.KEY key)
		{
			key.kb.KeyboardText.text = "";
		}

		// Token: 0x0600034E RID: 846 RVA: 0x00010304 File Offset: 0x0000E504
		public void Enter(KEYBOARD.KEY key)
		{
			string text = key.kb.KeyboardText.text;
			Action<string> enterPressed = this.EnterPressed;
			if (enterPressed != null)
			{
				enterPressed(text);
			}
			key.kb.KeyboardText.text = "";
		}

		// Token: 0x0600034F RID: 847 RVA: 0x0001034C File Offset: 0x0000E54C
		private void Backspace(KEYBOARD.KEY key)
		{
			int length = key.kb.KeyboardText.text.Length;
			if (length > 0)
			{
				key.kb.KeyboardText.text = key.kb.KeyboardText.text.Remove(length - 1);
			}
		}

		// Token: 0x06000350 RID: 848 RVA: 0x0001039C File Offset: 0x0000E59C
		private void SHIFT(KEYBOARD.KEY key)
		{
			key.kb.Shift = !key.kb.Shift;
			foreach (KEYBOARD.KEY key2 in key.kb.keys)
			{
				string text = (key.kb.Shift ? key2.shifted : key2.value);
				if (key2.shifted != "")
				{
					key2.mybutton.SetButtonText(text);
				}
				if (key2.name == "SHIFT")
				{
					key2.mybutton.GetComponentInChildren<Image>().color = (key.kb.Shift ? Color.green : Color.white);
				}
			}
		}

		// Token: 0x06000351 RID: 849 RVA: 0x00010480 File Offset: 0x0000E680
		private void CAPS(KEYBOARD.KEY key)
		{
			key.kb.Caps = !key.kb.Caps;
			key.mybutton.GetComponentInChildren<Image>().color = (key.kb.Caps ? Color.green : Color.white);
		}

		// Token: 0x06000352 RID: 850 RVA: 0x000104D0 File Offset: 0x0000E6D0
		public void DrawCursor()
		{
			if (!this.EnableInputField)
			{
				return;
			}
			Vector2 preferredValues = this.KeyboardText.GetPreferredValues(this.KeyboardText.text + "|");
			preferredValues.y = 15f;
			preferredValues.x = preferredValues.x / 2f + 30f - 0.5f;
			(this.KeyboardCursor.transform as RectTransform).anchoredPosition = preferredValues;
		}

		// Token: 0x040000E0 RID: 224
		public List<KEYBOARD.KEY> keys = new List<KEYBOARD.KEY>();

		// Token: 0x040000E1 RID: 225
		private KEYBOARD.KEY dummy = new KEYBOARD.KEY();

		// Token: 0x040000E2 RID: 226
		private bool EnableInputField = true;

		// Token: 0x040000E3 RID: 227
		private bool Shift;

		// Token: 0x040000E4 RID: 228
		private bool Caps;

		// Token: 0x040000E5 RID: 229
		private RectTransform container;

		// Token: 0x040000E6 RID: 230
		private Vector2 currentposition;

		// Token: 0x040000E7 RID: 231
		private Vector2 baseposition;

		// Token: 0x040000E8 RID: 232
		private float scale = 0.5f;

		// Token: 0x040000E9 RID: 233
		private float padding = 0.5f;

		// Token: 0x040000EA RID: 234
		private float buttonwidth = 12f;

		// Token: 0x040000EB RID: 235
		public TextMeshProUGUI KeyboardText;

		// Token: 0x040000ED RID: 237
		public TextMeshProUGUI KeyboardCursor;

		// Token: 0x040000EE RID: 238
		public Button BaseButton;

		// Token: 0x040000EF RID: 239
		public const string QWERTY = "[CLEAR]/20\n(`~) (1!) (2@) (3#) (4$) (5%) (6^) (7&) (8*) (9() (0)) (-_) (=+) [<--]/15\n[TAB]/15'\\t' (qQ) (wW) (eE) (rR) (tT) (yY) (uU) (iI) (oO) (pP) ([{) (]}) (\\|)\n[CAPS]/20 (aA) (sS) (dD) (fF) (gG) (hH) (jJ) (kK) (lL) (;:) ('\") [ENTER]/20,22#20A0D0\n[SHIFT]/25 (zZ) (xX) (cC) (vV) (bB) (nN) (mM) (,<) (.>) (/?)  \n/23 (!!) (@@) [SPACE]/40' ' (##) (__)";

		// Token: 0x040000F0 RID: 240
		public const string FKEYROW = "\n[Esc] /2 [F1] [F2] [F3] [F4] /2 [F5] [F6] [F7] [F8] /2 [F9] [F10] [F11] [F12]\n";

		// Token: 0x040000F1 RID: 241
		public const string NUMPAD = "\n[NUM] (//) (**) (--)\n(77) (88) (99) [+]/10,22\n(44) (55) (66)\n(11) (22) (33) [ENTER]/10,22\n[0]/22 [.]\n";

		// Token: 0x040000F2 RID: 242
		public const string DVORAK = "\n(`~) (1!) (2@) (3#) (4$) (5%) (6^) (7&) (8*) (9() (0)) ([{) (]}) [<--]/15\n[TAB]/15 ('\") (,<) (.>) (pP) (yY) (fF) (gG) (cC) (rR) (lL) (/?) (=+) (\\|)\n[CAPS]/20 (aA) (oO) (eE) (uU) (iI) (dD) (hH) (tT) (nN) (sS) (-_) [ENTER]/20\n[SHIFT] (;:) (qQ) (jJ) (kK) (xX) (bB) (mM) (wW) (vV) (zZ) [CLEAR]/28\n/23 (!!) (@@) [SPACE]/40 (##) (__)";

		// Token: 0x02000141 RID: 321
		public class KEY
		{
			// Token: 0x06000671 RID: 1649 RVA: 0x00016AD0 File Offset: 0x00014CD0
			public KEYBOARD.KEY Set(string Value)
			{
				if (Value != "")
				{
					this.value = Value;
				}
				return this;
			}

			// Token: 0x06000672 RID: 1650 RVA: 0x00016AE7 File Offset: 0x00014CE7
			public KEY()
			{
			}

			// Token: 0x06000673 RID: 1651 RVA: 0x00016B10 File Offset: 0x00014D10
			public KEY(KEYBOARD kb, Vector2 position, string text, float width, float height, Color color)
			{
				KEYBOARD.KEY <>4__this = this;
				this.value = text;
				this.kb = kb;
				this.name = text;
				this.mybutton = Object.Instantiate<Button>(kb.BaseButton, kb.container, false);
				(this.mybutton.transform as RectTransform).anchorMin = new Vector2(0.5f, 0.5f);
				(this.mybutton.transform as RectTransform).anchorMax = new Vector2(0.5f, 0.5f);
				TMP_Text componentInChildren = this.mybutton.GetComponentInChildren<TMP_Text>();
				this.mybutton.ToggleWordWrapping(false);
				this.mybutton.transform.localScale = new Vector3(kb.scale, kb.scale, 1f);
				this.mybutton.SetButtonTextSize(5f);
				this.mybutton.SetButtonText(text);
				this.mybutton.GetComponentInChildren<Image>().color = color;
				if (width == 0f)
				{
					Vector2 preferredValues = componentInChildren.GetPreferredValues(text);
					preferredValues.x += 10f;
					preferredValues.y += 2f;
					width = preferredValues.x;
				}
				position.x += kb.scale * width / 2f;
				position.y -= kb.scale * height / 2f;
				(this.mybutton.transform as RectTransform).anchoredPosition = position;
				(this.mybutton.transform as RectTransform).sizeDelta = new Vector2(width, height);
				KEYBOARD keyboard = kb;
				keyboard.currentposition.x = keyboard.currentposition.x + (width * kb.scale + kb.padding);
				this.mybutton.onClick.RemoveAllListeners();
				this.mybutton.onClick.AddListener(delegate
				{
					if (<>4__this.keyaction != null)
					{
						<>4__this.keyaction(<>4__this);
						kb.DrawCursor();
						return;
					}
					if (<>4__this.value.EndsWith("%CR%"))
					{
						TextMeshProUGUI keyboardText = kb.KeyboardText;
						keyboardText.text += <>4__this.value.Substring(0, <>4__this.value.Length - 4);
						kb.Enter(<>4__this);
						kb.DrawCursor();
						return;
					}
					string text2 = (kb.Shift ? <>4__this.shifted : <>4__this.value);
					if (text2 == "")
					{
						text2 = <>4__this.value;
					}
					if (kb.Caps)
					{
						text2 = <>4__this.value.ToUpper();
					}
					TextMeshProUGUI keyboardText2 = kb.KeyboardText;
					keyboardText2.text += text2;
					kb.DrawCursor();
				});
				HoverHint hoverHint = this.mybutton.gameObject.AddComponent<HoverHint>();
				hoverHint.text = this.value;
				hoverHint.SetField("_hoverHintController", Resources.FindObjectsOfTypeAll<HoverHintController>().First<HoverHintController>());
			}

			// Token: 0x040002C3 RID: 707
			public string name = "";

			// Token: 0x040002C4 RID: 708
			public string value = "";

			// Token: 0x040002C5 RID: 709
			public string shifted = "";

			// Token: 0x040002C6 RID: 710
			public Button mybutton;

			// Token: 0x040002C7 RID: 711
			public KEYBOARD kb;

			// Token: 0x040002C8 RID: 712
			public Action<KEYBOARD.KEY> keyaction;
		}
	}
}
