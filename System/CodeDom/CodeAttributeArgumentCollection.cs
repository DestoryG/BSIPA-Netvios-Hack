using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x0200061C RID: 1564
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeAttributeArgumentCollection : CollectionBase
	{
		// Token: 0x06003922 RID: 14626 RVA: 0x000F24AA File Offset: 0x000F06AA
		public CodeAttributeArgumentCollection()
		{
		}

		// Token: 0x06003923 RID: 14627 RVA: 0x000F24B2 File Offset: 0x000F06B2
		public CodeAttributeArgumentCollection(CodeAttributeArgumentCollection value)
		{
			this.AddRange(value);
		}

		// Token: 0x06003924 RID: 14628 RVA: 0x000F24C1 File Offset: 0x000F06C1
		public CodeAttributeArgumentCollection(CodeAttributeArgument[] value)
		{
			this.AddRange(value);
		}

		// Token: 0x17000DAE RID: 3502
		public CodeAttributeArgument this[int index]
		{
			get
			{
				return (CodeAttributeArgument)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x06003927 RID: 14631 RVA: 0x000F24F2 File Offset: 0x000F06F2
		public int Add(CodeAttributeArgument value)
		{
			return base.List.Add(value);
		}

		// Token: 0x06003928 RID: 14632 RVA: 0x000F2500 File Offset: 0x000F0700
		public void AddRange(CodeAttributeArgument[] value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			for (int i = 0; i < value.Length; i++)
			{
				this.Add(value[i]);
			}
		}

		// Token: 0x06003929 RID: 14633 RVA: 0x000F2534 File Offset: 0x000F0734
		public void AddRange(CodeAttributeArgumentCollection value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			int count = value.Count;
			for (int i = 0; i < count; i++)
			{
				this.Add(value[i]);
			}
		}

		// Token: 0x0600392A RID: 14634 RVA: 0x000F2570 File Offset: 0x000F0770
		public bool Contains(CodeAttributeArgument value)
		{
			return base.List.Contains(value);
		}

		// Token: 0x0600392B RID: 14635 RVA: 0x000F257E File Offset: 0x000F077E
		public void CopyTo(CodeAttributeArgument[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x0600392C RID: 14636 RVA: 0x000F258D File Offset: 0x000F078D
		public int IndexOf(CodeAttributeArgument value)
		{
			return base.List.IndexOf(value);
		}

		// Token: 0x0600392D RID: 14637 RVA: 0x000F259B File Offset: 0x000F079B
		public void Insert(int index, CodeAttributeArgument value)
		{
			base.List.Insert(index, value);
		}

		// Token: 0x0600392E RID: 14638 RVA: 0x000F25AA File Offset: 0x000F07AA
		public void Remove(CodeAttributeArgument value)
		{
			base.List.Remove(value);
		}
	}
}
