using System;
using System.Collections.Generic;
using BeatSaberMarkupLanguage.Parser;
using HMUI;
using UnityEngine;

namespace BeatSaberMarkupLanguage.Components
{
	// Token: 0x0200009D RID: 157
	public class CustomCellTableCell : TableCell
	{
		// Token: 0x0600032A RID: 810 RVA: 0x0000F408 File Offset: 0x0000D608
		internal void SetupPostParse()
		{
			this.selectedTags = this.parserParams.GetObjectsWithTag("selected");
			this.hoveredTags = this.parserParams.GetObjectsWithTag("hovered");
			this.neitherTags = this.parserParams.GetObjectsWithTag("un-selected-un-hovered");
		}

		// Token: 0x0600032B RID: 811 RVA: 0x0000F457 File Offset: 0x0000D657
		protected override void SelectionDidChange(TableCell.TransitionType transitionType)
		{
			this.RefreshVisuals();
		}

		// Token: 0x0600032C RID: 812 RVA: 0x0000F457 File Offset: 0x0000D657
		protected override void HighlightDidChange(TableCell.TransitionType transitionType)
		{
			this.RefreshVisuals();
		}

		// Token: 0x0600032D RID: 813 RVA: 0x0000F460 File Offset: 0x0000D660
		public virtual void RefreshVisuals()
		{
			foreach (GameObject gameObject in this.selectedTags)
			{
				gameObject.SetActive(base.selected);
			}
			foreach (GameObject gameObject2 in this.hoveredTags)
			{
				gameObject2.SetActive(base.highlighted);
			}
			foreach (GameObject gameObject3 in this.neitherTags)
			{
				gameObject3.SetActive(!base.selected && !base.highlighted);
			}
			BSMLAction bsmlaction;
			if (this.parserParams.actions.TryGetValue("refresh-visuals", out bsmlaction))
			{
				bsmlaction.Invoke(new object[] { base.selected, base.highlighted });
			}
		}

		// Token: 0x040000CA RID: 202
		public BSMLParserParams parserParams;

		// Token: 0x040000CB RID: 203
		public List<GameObject> selectedTags;

		// Token: 0x040000CC RID: 204
		public List<GameObject> hoveredTags;

		// Token: 0x040000CD RID: 205
		public List<GameObject> neitherTags;
	}
}
