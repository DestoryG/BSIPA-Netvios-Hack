using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000660 RID: 1632
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeTypeMemberCollection : CollectionBase
	{
		// Token: 0x06003B17 RID: 15127 RVA: 0x000F4D0D File Offset: 0x000F2F0D
		public CodeTypeMemberCollection()
		{
		}

		// Token: 0x06003B18 RID: 15128 RVA: 0x000F4D15 File Offset: 0x000F2F15
		public CodeTypeMemberCollection(CodeTypeMemberCollection value)
		{
			this.AddRange(value);
		}

		// Token: 0x06003B19 RID: 15129 RVA: 0x000F4D24 File Offset: 0x000F2F24
		public CodeTypeMemberCollection(CodeTypeMember[] value)
		{
			this.AddRange(value);
		}

		// Token: 0x17000E3F RID: 3647
		public CodeTypeMember this[int index]
		{
			get
			{
				return (CodeTypeMember)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x06003B1C RID: 15132 RVA: 0x000F4D55 File Offset: 0x000F2F55
		public int Add(CodeTypeMember value)
		{
			return base.List.Add(value);
		}

		// Token: 0x06003B1D RID: 15133 RVA: 0x000F4D64 File Offset: 0x000F2F64
		public void AddRange(CodeTypeMember[] value)
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

		// Token: 0x06003B1E RID: 15134 RVA: 0x000F4D98 File Offset: 0x000F2F98
		public void AddRange(CodeTypeMemberCollection value)
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

		// Token: 0x06003B1F RID: 15135 RVA: 0x000F4DD4 File Offset: 0x000F2FD4
		public bool Contains(CodeTypeMember value)
		{
			return base.List.Contains(value);
		}

		// Token: 0x06003B20 RID: 15136 RVA: 0x000F4DE2 File Offset: 0x000F2FE2
		public void CopyTo(CodeTypeMember[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x06003B21 RID: 15137 RVA: 0x000F4DF1 File Offset: 0x000F2FF1
		public int IndexOf(CodeTypeMember value)
		{
			return base.List.IndexOf(value);
		}

		// Token: 0x06003B22 RID: 15138 RVA: 0x000F4DFF File Offset: 0x000F2FFF
		public void Insert(int index, CodeTypeMember value)
		{
			base.List.Insert(index, value);
		}

		// Token: 0x06003B23 RID: 15139 RVA: 0x000F4E0E File Offset: 0x000F300E
		public void Remove(CodeTypeMember value)
		{
			base.List.Remove(value);
		}
	}
}
