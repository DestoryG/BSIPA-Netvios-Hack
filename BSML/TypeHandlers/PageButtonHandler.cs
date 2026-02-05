using System;
using System.Collections.Generic;
using BeatSaberMarkupLanguage.Components;
using UnityEngine;
using UnityEngine.UI;

namespace BeatSaberMarkupLanguage.TypeHandlers
{
	// Token: 0x0200002A RID: 42
	[ComponentHandler(typeof(PageButton))]
	public class PageButtonHandler : TypeHandler<PageButton>
	{
		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060000F3 RID: 243 RVA: 0x00006E18 File Offset: 0x00005018
		public override Dictionary<string, string[]> Props
		{
			get
			{
				return new Dictionary<string, string[]> { 
				{
					"direction",
					new string[] { "dir", "direction" }
				} };
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060000F4 RID: 244 RVA: 0x00006E4D File Offset: 0x0000504D
		public override Dictionary<string, Action<PageButton, string>> Setters
		{
			get
			{
				return new Dictionary<string, Action<PageButton, string>> { 
				{
					"direction",
					new Action<PageButton, string>(PageButtonHandler.SetButtonDirection)
				} };
			}
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00006E6C File Offset: 0x0000506C
		public static void SetButtonDirection(PageButton button, string value)
		{
			LayoutElement component = button.gameObject.GetComponent<LayoutElement>();
			RectTransform rectTransform = button.transform.Find("Arrow") as RectTransform;
			bool flag = false;
			object obj = Enum.Parse(typeof(PageButtonHandler.PageButtonDirection), value);
			if (obj is PageButtonHandler.PageButtonDirection)
			{
				switch ((PageButtonHandler.PageButtonDirection)obj)
				{
				case PageButtonHandler.PageButtonDirection.Up:
					flag = true;
					rectTransform.localRotation = Quaternion.Euler(0f, 0f, -180f);
					break;
				case PageButtonHandler.PageButtonDirection.Down:
					flag = true;
					rectTransform.localRotation = Quaternion.Euler(0f, 0f, 0f);
					break;
				case PageButtonHandler.PageButtonDirection.Left:
					flag = false;
					rectTransform.localRotation = Quaternion.Euler(0f, 0f, -90f);
					break;
				case PageButtonHandler.PageButtonDirection.Right:
					flag = false;
					rectTransform.localRotation = Quaternion.Euler(0f, 0f, 90f);
					break;
				}
			}
			if (component.preferredHeight == -1f)
			{
				component.preferredHeight = (float)(flag ? 6 : 40);
			}
			if (component.preferredWidth == -1f)
			{
				component.preferredWidth = (float)(flag ? 40 : 6);
			}
		}

		// Token: 0x02000108 RID: 264
		public enum PageButtonDirection
		{
			// Token: 0x04000216 RID: 534
			Up,
			// Token: 0x04000217 RID: 535
			Down,
			// Token: 0x04000218 RID: 536
			Left,
			// Token: 0x04000219 RID: 537
			Right
		}
	}
}
