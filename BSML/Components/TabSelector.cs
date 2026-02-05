using System;
using System.Collections.Generic;
using System.Linq;
using BeatSaberMarkupLanguage.Parser;
using HMUI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BeatSaberMarkupLanguage.Components
{
	// Token: 0x020000A8 RID: 168
	public class TabSelector : MonoBehaviour
	{
		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x0600036C RID: 876 RVA: 0x00010900 File Offset: 0x0000EB00
		// (set) Token: 0x0600036D RID: 877 RVA: 0x00010908 File Offset: 0x0000EB08
		public int PageCount
		{
			get
			{
				return this.pageCount;
			}
			set
			{
				this.pageCount = value;
				if (this.tabs.Count > 0)
				{
					this.Refresh();
				}
			}
		}

		// Token: 0x0600036E RID: 878 RVA: 0x00010928 File Offset: 0x0000EB28
		public void Setup()
		{
			this.tabs.Clear();
			foreach (GameObject gameObject in this.parserParams.GetObjectsWithTag(this.tabTag))
			{
				Tab component = gameObject.GetComponent<Tab>();
				this.tabs.Add(component);
				component.selector = this;
			}
			if (this.leftButtonTag != null)
			{
				this.leftButton = this.parserParams.GetObjectsWithTag(this.leftButtonTag).FirstOrDefault<GameObject>().GetComponent<Button>();
			}
			if (this.leftButton != null)
			{
				this.leftButton.onClick.AddListener(new UnityAction(this.PageLeft));
			}
			if (this.rightButtonTag != null)
			{
				this.rightButton = this.parserParams.GetObjectsWithTag(this.rightButtonTag).FirstOrDefault<GameObject>().GetComponent<Button>();
			}
			if (this.rightButton != null)
			{
				this.rightButton.onClick.AddListener(new UnityAction(this.PageRight));
			}
			this.Refresh();
			this.textSegmentedControl.didSelectCellEvent -= this.TabSelected;
			this.textSegmentedControl.didSelectCellEvent += this.TabSelected;
			this.textSegmentedControl.SelectCellWithNumber(0);
			this.TabSelected(this.textSegmentedControl, 0);
		}

		// Token: 0x0600036F RID: 879 RVA: 0x00010A98 File Offset: 0x0000EC98
		private void TabSelected(SegmentedControl segmentedControl, int index)
		{
			this.lastClickedPage = this.currentPage;
			this.lastClickedIndex = index;
			if (this.PageCount != -1)
			{
				index += this.PageCount * this.currentPage;
			}
			for (int i = 0; i < this.tabs.Count; i++)
			{
				this.tabs[i].gameObject.SetActive(i == index);
			}
		}

		// Token: 0x06000370 RID: 880 RVA: 0x00010B04 File Offset: 0x0000ED04
		public void Refresh()
		{
			if (this.PageCount == -1)
			{
				this.textSegmentedControl.SetTexts(this.tabs.Select((Tab x) => x.TabName).ToArray<string>());
				return;
			}
			if (this.currentPage < 0)
			{
				this.currentPage = 0;
			}
			if (this.currentPage > (this.tabs.Count - 1) / this.pageCount)
			{
				this.currentPage = (this.tabs.Count - 1) / this.pageCount;
			}
			this.textSegmentedControl.SetTexts(this.tabs.Select((Tab x) => x.TabName).Skip(this.PageCount * this.currentPage).Take(this.PageCount)
				.ToArray<string>());
			if (this.leftButton != null)
			{
				this.leftButton.interactable = this.currentPage > 0;
			}
			if (this.rightButton != null)
			{
				this.rightButton.interactable = this.currentPage < (this.tabs.Count - 1) / this.pageCount;
			}
			this.textSegmentedControl.SelectCellWithNumber((this.lastClickedPage == this.currentPage) ? this.lastClickedIndex : (-1));
		}

		// Token: 0x06000371 RID: 881 RVA: 0x00010C6B File Offset: 0x0000EE6B
		private void PageLeft()
		{
			this.currentPage--;
			this.Refresh();
		}

		// Token: 0x06000372 RID: 882 RVA: 0x00010C81 File Offset: 0x0000EE81
		private void PageRight()
		{
			this.currentPage++;
			this.Refresh();
		}

		// Token: 0x040000FA RID: 250
		public TextSegmentedControl textSegmentedControl;

		// Token: 0x040000FB RID: 251
		public BSMLParserParams parserParams;

		// Token: 0x040000FC RID: 252
		public string tabTag;

		// Token: 0x040000FD RID: 253
		public string leftButtonTag;

		// Token: 0x040000FE RID: 254
		public string rightButtonTag;

		// Token: 0x040000FF RID: 255
		private List<Tab> tabs = new List<Tab>();

		// Token: 0x04000100 RID: 256
		private int pageCount = -1;

		// Token: 0x04000101 RID: 257
		private int currentPage;

		// Token: 0x04000102 RID: 258
		private Button leftButton;

		// Token: 0x04000103 RID: 259
		private Button rightButton;

		// Token: 0x04000104 RID: 260
		private int lastClickedPage;

		// Token: 0x04000105 RID: 261
		private int lastClickedIndex;
	}
}
