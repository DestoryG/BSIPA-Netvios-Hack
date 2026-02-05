using System;
using BeatSaberMarkupLanguage.Parser;
using HMUI;
using UnityEngine;

namespace BeatSaberMarkupLanguage.Components
{
	// Token: 0x020000A1 RID: 161
	public class ModalKeyboard : MonoBehaviour
	{
		// Token: 0x0600033A RID: 826 RVA: 0x0000F819 File Offset: 0x0000DA19
		private void OnEnable()
		{
			if (this.associatedValue != null)
			{
				this.SetText(this.associatedValue.GetValue() as string);
			}
			if (this.clearOnOpen)
			{
				this.SetText("");
			}
		}

		// Token: 0x0600033B RID: 827 RVA: 0x0000F84C File Offset: 0x0000DA4C
		public void OnEnter(string value)
		{
			BSMLValue bsmlvalue = this.associatedValue;
			if (bsmlvalue != null)
			{
				bsmlvalue.SetValue(value);
			}
			BSMLAction bsmlaction = this.onEnter;
			if (bsmlaction != null)
			{
				bsmlaction.Invoke(new object[] { value });
			}
			this.modalView.Hide(true, null);
		}

		// Token: 0x0600033C RID: 828 RVA: 0x0000F889 File Offset: 0x0000DA89
		public void SetText(string text)
		{
			this.keyboard.KeyboardText.text = text;
			this.keyboard.DrawCursor();
		}

		// Token: 0x040000DB RID: 219
		public ModalView modalView;

		// Token: 0x040000DC RID: 220
		public KEYBOARD keyboard;

		// Token: 0x040000DD RID: 221
		public BSMLValue associatedValue;

		// Token: 0x040000DE RID: 222
		public BSMLAction onEnter;

		// Token: 0x040000DF RID: 223
		public bool clearOnOpen;
	}
}
