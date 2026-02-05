using System;
using BeatSaberMarkupLanguage.Components;
using HMUI;
using UnityEngine;

namespace BeatSaberMarkupLanguage.Tags
{
	// Token: 0x02000051 RID: 81
	public class ModalKeyboardTag : ModalTag
	{
		// Token: 0x17000076 RID: 118
		// (get) Token: 0x0600017A RID: 378 RVA: 0x00009CAC File Offset: 0x00007EAC
		public override string[] Aliases
		{
			get
			{
				return new string[] { "modal-keyboard" };
			}
		}

		// Token: 0x0600017B RID: 379 RVA: 0x00009CBC File Offset: 0x00007EBC
		public override GameObject CreateObject(Transform parent)
		{
			GameObject gameObject = base.CreateObject(parent);
			ExternalComponents component = gameObject.GetComponent<ExternalComponents>();
			RectTransform rectTransform = component.Get<RectTransform>();
			rectTransform.name = "BSMLModalKeyboard";
			rectTransform.sizeDelta = new Vector2(135f, 75f);
			RectTransform rectTransform2 = new GameObject("KeyboardParent").AddComponent<RectTransform>();
			rectTransform2.SetParent(gameObject.transform, false);
			KEYBOARD keyboard = new KEYBOARD(rectTransform2, "[CLEAR]/20\n(`~) (1!) (2@) (3#) (4$) (5%) (6^) (7&) (8*) (9() (0)) (-_) (=+) [<--]/15\n[TAB]/15'\\t' (qQ) (wW) (eE) (rR) (tT) (yY) (uU) (iI) (oO) (pP) ([{) (]}) (\\|)\n[CAPS]/20 (aA) (sS) (dD) (fF) (gG) (hH) (jJ) (kK) (lL) (;:) ('\") [ENTER]/20,22#20A0D0\n[SHIFT]/25 (zZ) (xX) (cC) (vV) (bB) (nN) (mM) (,<) (.>) (/?)  \n/23 (!!) (@@) [SPACE]/40' ' (##) (__)", true, 4f, -12f);
			rectTransform2.localScale *= 1.4f;
			ModalKeyboard modalKeyboard = gameObject.AddComponent<ModalKeyboard>();
			modalKeyboard.keyboard = keyboard;
			modalKeyboard.modalView = component.Get<ModalView>();
			keyboard.EnterPressed += delegate(string value)
			{
				modalKeyboard.OnEnter(value);
			};
			return gameObject;
		}
	}
}
