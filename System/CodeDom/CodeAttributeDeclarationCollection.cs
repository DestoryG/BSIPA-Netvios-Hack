using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x0200061E RID: 1566
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeAttributeDeclarationCollection : CollectionBase
	{
		// Token: 0x06003938 RID: 14648 RVA: 0x000F268D File Offset: 0x000F088D
		public CodeAttributeDeclarationCollection()
		{
		}

		// Token: 0x06003939 RID: 14649 RVA: 0x000F2695 File Offset: 0x000F0895
		public CodeAttributeDeclarationCollection(CodeAttributeDeclarationCollection value)
		{
			this.AddRange(value);
		}

		// Token: 0x0600393A RID: 14650 RVA: 0x000F26A4 File Offset: 0x000F08A4
		public CodeAttributeDeclarationCollection(CodeAttributeDeclaration[] value)
		{
			this.AddRange(value);
		}

		// Token: 0x17000DB2 RID: 3506
		public CodeAttributeDeclaration this[int index]
		{
			get
			{
				return (CodeAttributeDeclaration)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x0600393D RID: 14653 RVA: 0x000F26D5 File Offset: 0x000F08D5
		public int Add(CodeAttributeDeclaration value)
		{
			return base.List.Add(value);
		}

		// Token: 0x0600393E RID: 14654 RVA: 0x000F26E4 File Offset: 0x000F08E4
		public void AddRange(CodeAttributeDeclaration[] value)
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

		// Token: 0x0600393F RID: 14655 RVA: 0x000F2718 File Offset: 0x000F0918
		public void AddRange(CodeAttributeDeclarationCollection value)
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

		// Token: 0x06003940 RID: 14656 RVA: 0x000F2754 File Offset: 0x000F0954
		public bool Contains(CodeAttributeDeclaration value)
		{
			return base.List.Contains(value);
		}

		// Token: 0x06003941 RID: 14657 RVA: 0x000F2762 File Offset: 0x000F0962
		public void CopyTo(CodeAttributeDeclaration[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x06003942 RID: 14658 RVA: 0x000F2771 File Offset: 0x000F0971
		public int IndexOf(CodeAttributeDeclaration value)
		{
			return base.List.IndexOf(value);
		}

		// Token: 0x06003943 RID: 14659 RVA: 0x000F277F File Offset: 0x000F097F
		public void Insert(int index, CodeAttributeDeclaration value)
		{
			base.List.Insert(index, value);
		}

		// Token: 0x06003944 RID: 14660 RVA: 0x000F278E File Offset: 0x000F098E
		public void Remove(CodeAttributeDeclaration value)
		{
			base.List.Remove(value);
		}
	}
}
