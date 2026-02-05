using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x0200065D RID: 1629
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeTypeDeclarationCollection : CollectionBase
	{
		// Token: 0x06003AF9 RID: 15097 RVA: 0x000F4AA1 File Offset: 0x000F2CA1
		public CodeTypeDeclarationCollection()
		{
		}

		// Token: 0x06003AFA RID: 15098 RVA: 0x000F4AA9 File Offset: 0x000F2CA9
		public CodeTypeDeclarationCollection(CodeTypeDeclarationCollection value)
		{
			this.AddRange(value);
		}

		// Token: 0x06003AFB RID: 15099 RVA: 0x000F4AB8 File Offset: 0x000F2CB8
		public CodeTypeDeclarationCollection(CodeTypeDeclaration[] value)
		{
			this.AddRange(value);
		}

		// Token: 0x17000E35 RID: 3637
		public CodeTypeDeclaration this[int index]
		{
			get
			{
				return (CodeTypeDeclaration)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x06003AFE RID: 15102 RVA: 0x000F4AE9 File Offset: 0x000F2CE9
		public int Add(CodeTypeDeclaration value)
		{
			return base.List.Add(value);
		}

		// Token: 0x06003AFF RID: 15103 RVA: 0x000F4AF8 File Offset: 0x000F2CF8
		public void AddRange(CodeTypeDeclaration[] value)
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

		// Token: 0x06003B00 RID: 15104 RVA: 0x000F4B2C File Offset: 0x000F2D2C
		public void AddRange(CodeTypeDeclarationCollection value)
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

		// Token: 0x06003B01 RID: 15105 RVA: 0x000F4B68 File Offset: 0x000F2D68
		public bool Contains(CodeTypeDeclaration value)
		{
			return base.List.Contains(value);
		}

		// Token: 0x06003B02 RID: 15106 RVA: 0x000F4B76 File Offset: 0x000F2D76
		public void CopyTo(CodeTypeDeclaration[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x06003B03 RID: 15107 RVA: 0x000F4B85 File Offset: 0x000F2D85
		public int IndexOf(CodeTypeDeclaration value)
		{
			return base.List.IndexOf(value);
		}

		// Token: 0x06003B04 RID: 15108 RVA: 0x000F4B93 File Offset: 0x000F2D93
		public void Insert(int index, CodeTypeDeclaration value)
		{
			base.List.Insert(index, value);
		}

		// Token: 0x06003B05 RID: 15109 RVA: 0x000F4BA2 File Offset: 0x000F2DA2
		public void Remove(CodeTypeDeclaration value)
		{
			base.List.Remove(value);
		}
	}
}
