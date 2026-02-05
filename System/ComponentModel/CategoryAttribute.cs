using System;

namespace System.ComponentModel
{
	// Token: 0x02000523 RID: 1315
	[AttributeUsage(AttributeTargets.All)]
	public class CategoryAttribute : Attribute
	{
		// Token: 0x17000C31 RID: 3121
		// (get) Token: 0x060031D4 RID: 12756 RVA: 0x000E0096 File Offset: 0x000DE296
		public static CategoryAttribute Action
		{
			get
			{
				if (CategoryAttribute.action == null)
				{
					CategoryAttribute.action = new CategoryAttribute("Action");
				}
				return CategoryAttribute.action;
			}
		}

		// Token: 0x17000C32 RID: 3122
		// (get) Token: 0x060031D5 RID: 12757 RVA: 0x000E00B9 File Offset: 0x000DE2B9
		public static CategoryAttribute Appearance
		{
			get
			{
				if (CategoryAttribute.appearance == null)
				{
					CategoryAttribute.appearance = new CategoryAttribute("Appearance");
				}
				return CategoryAttribute.appearance;
			}
		}

		// Token: 0x17000C33 RID: 3123
		// (get) Token: 0x060031D6 RID: 12758 RVA: 0x000E00DC File Offset: 0x000DE2DC
		public static CategoryAttribute Asynchronous
		{
			get
			{
				if (CategoryAttribute.asynchronous == null)
				{
					CategoryAttribute.asynchronous = new CategoryAttribute("Asynchronous");
				}
				return CategoryAttribute.asynchronous;
			}
		}

		// Token: 0x17000C34 RID: 3124
		// (get) Token: 0x060031D7 RID: 12759 RVA: 0x000E00FF File Offset: 0x000DE2FF
		public static CategoryAttribute Behavior
		{
			get
			{
				if (CategoryAttribute.behavior == null)
				{
					CategoryAttribute.behavior = new CategoryAttribute("Behavior");
				}
				return CategoryAttribute.behavior;
			}
		}

		// Token: 0x17000C35 RID: 3125
		// (get) Token: 0x060031D8 RID: 12760 RVA: 0x000E0122 File Offset: 0x000DE322
		public static CategoryAttribute Data
		{
			get
			{
				if (CategoryAttribute.data == null)
				{
					CategoryAttribute.data = new CategoryAttribute("Data");
				}
				return CategoryAttribute.data;
			}
		}

		// Token: 0x17000C36 RID: 3126
		// (get) Token: 0x060031D9 RID: 12761 RVA: 0x000E0145 File Offset: 0x000DE345
		public static CategoryAttribute Default
		{
			get
			{
				if (CategoryAttribute.defAttr == null)
				{
					CategoryAttribute.defAttr = new CategoryAttribute();
				}
				return CategoryAttribute.defAttr;
			}
		}

		// Token: 0x17000C37 RID: 3127
		// (get) Token: 0x060031DA RID: 12762 RVA: 0x000E0163 File Offset: 0x000DE363
		public static CategoryAttribute Design
		{
			get
			{
				if (CategoryAttribute.design == null)
				{
					CategoryAttribute.design = new CategoryAttribute("Design");
				}
				return CategoryAttribute.design;
			}
		}

		// Token: 0x17000C38 RID: 3128
		// (get) Token: 0x060031DB RID: 12763 RVA: 0x000E0186 File Offset: 0x000DE386
		public static CategoryAttribute DragDrop
		{
			get
			{
				if (CategoryAttribute.dragDrop == null)
				{
					CategoryAttribute.dragDrop = new CategoryAttribute("DragDrop");
				}
				return CategoryAttribute.dragDrop;
			}
		}

		// Token: 0x17000C39 RID: 3129
		// (get) Token: 0x060031DC RID: 12764 RVA: 0x000E01A9 File Offset: 0x000DE3A9
		public static CategoryAttribute Focus
		{
			get
			{
				if (CategoryAttribute.focus == null)
				{
					CategoryAttribute.focus = new CategoryAttribute("Focus");
				}
				return CategoryAttribute.focus;
			}
		}

		// Token: 0x17000C3A RID: 3130
		// (get) Token: 0x060031DD RID: 12765 RVA: 0x000E01CC File Offset: 0x000DE3CC
		public static CategoryAttribute Format
		{
			get
			{
				if (CategoryAttribute.format == null)
				{
					CategoryAttribute.format = new CategoryAttribute("Format");
				}
				return CategoryAttribute.format;
			}
		}

		// Token: 0x17000C3B RID: 3131
		// (get) Token: 0x060031DE RID: 12766 RVA: 0x000E01EF File Offset: 0x000DE3EF
		public static CategoryAttribute Key
		{
			get
			{
				if (CategoryAttribute.key == null)
				{
					CategoryAttribute.key = new CategoryAttribute("Key");
				}
				return CategoryAttribute.key;
			}
		}

		// Token: 0x17000C3C RID: 3132
		// (get) Token: 0x060031DF RID: 12767 RVA: 0x000E0212 File Offset: 0x000DE412
		public static CategoryAttribute Layout
		{
			get
			{
				if (CategoryAttribute.layout == null)
				{
					CategoryAttribute.layout = new CategoryAttribute("Layout");
				}
				return CategoryAttribute.layout;
			}
		}

		// Token: 0x17000C3D RID: 3133
		// (get) Token: 0x060031E0 RID: 12768 RVA: 0x000E0235 File Offset: 0x000DE435
		public static CategoryAttribute Mouse
		{
			get
			{
				if (CategoryAttribute.mouse == null)
				{
					CategoryAttribute.mouse = new CategoryAttribute("Mouse");
				}
				return CategoryAttribute.mouse;
			}
		}

		// Token: 0x17000C3E RID: 3134
		// (get) Token: 0x060031E1 RID: 12769 RVA: 0x000E0258 File Offset: 0x000DE458
		public static CategoryAttribute WindowStyle
		{
			get
			{
				if (CategoryAttribute.windowStyle == null)
				{
					CategoryAttribute.windowStyle = new CategoryAttribute("WindowStyle");
				}
				return CategoryAttribute.windowStyle;
			}
		}

		// Token: 0x060031E2 RID: 12770 RVA: 0x000E027B File Offset: 0x000DE47B
		public CategoryAttribute()
			: this("Default")
		{
		}

		// Token: 0x060031E3 RID: 12771 RVA: 0x000E0288 File Offset: 0x000DE488
		public CategoryAttribute(string category)
		{
			this.categoryValue = category;
			this.localized = false;
		}

		// Token: 0x17000C3F RID: 3135
		// (get) Token: 0x060031E4 RID: 12772 RVA: 0x000E02A0 File Offset: 0x000DE4A0
		public string Category
		{
			get
			{
				if (!this.localized)
				{
					this.localized = true;
					string localizedString = this.GetLocalizedString(this.categoryValue);
					if (localizedString != null)
					{
						this.categoryValue = localizedString;
					}
				}
				return this.categoryValue;
			}
		}

		// Token: 0x060031E5 RID: 12773 RVA: 0x000E02D9 File Offset: 0x000DE4D9
		public override bool Equals(object obj)
		{
			return obj == this || (obj is CategoryAttribute && this.Category.Equals(((CategoryAttribute)obj).Category));
		}

		// Token: 0x060031E6 RID: 12774 RVA: 0x000E0301 File Offset: 0x000DE501
		public override int GetHashCode()
		{
			return this.Category.GetHashCode();
		}

		// Token: 0x060031E7 RID: 12775 RVA: 0x000E030E File Offset: 0x000DE50E
		protected virtual string GetLocalizedString(string value)
		{
			return (string)SR.GetObject("PropertyCategory" + value);
		}

		// Token: 0x060031E8 RID: 12776 RVA: 0x000E0325 File Offset: 0x000DE525
		public override bool IsDefaultAttribute()
		{
			return this.Category.Equals(CategoryAttribute.Default.Category);
		}

		// Token: 0x04002937 RID: 10551
		private static volatile CategoryAttribute appearance;

		// Token: 0x04002938 RID: 10552
		private static volatile CategoryAttribute asynchronous;

		// Token: 0x04002939 RID: 10553
		private static volatile CategoryAttribute behavior;

		// Token: 0x0400293A RID: 10554
		private static volatile CategoryAttribute data;

		// Token: 0x0400293B RID: 10555
		private static volatile CategoryAttribute design;

		// Token: 0x0400293C RID: 10556
		private static volatile CategoryAttribute action;

		// Token: 0x0400293D RID: 10557
		private static volatile CategoryAttribute format;

		// Token: 0x0400293E RID: 10558
		private static volatile CategoryAttribute layout;

		// Token: 0x0400293F RID: 10559
		private static volatile CategoryAttribute mouse;

		// Token: 0x04002940 RID: 10560
		private static volatile CategoryAttribute key;

		// Token: 0x04002941 RID: 10561
		private static volatile CategoryAttribute focus;

		// Token: 0x04002942 RID: 10562
		private static volatile CategoryAttribute windowStyle;

		// Token: 0x04002943 RID: 10563
		private static volatile CategoryAttribute dragDrop;

		// Token: 0x04002944 RID: 10564
		private static volatile CategoryAttribute defAttr;

		// Token: 0x04002945 RID: 10565
		private bool localized;

		// Token: 0x04002946 RID: 10566
		private string categoryValue;
	}
}
