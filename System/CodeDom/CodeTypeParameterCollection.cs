using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000663 RID: 1635
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeTypeParameterCollection : CollectionBase
	{
		// Token: 0x06003B32 RID: 15154 RVA: 0x000F4F01 File Offset: 0x000F3101
		public CodeTypeParameterCollection()
		{
		}

		// Token: 0x06003B33 RID: 15155 RVA: 0x000F4F09 File Offset: 0x000F3109
		public CodeTypeParameterCollection(CodeTypeParameterCollection value)
		{
			this.AddRange(value);
		}

		// Token: 0x06003B34 RID: 15156 RVA: 0x000F4F18 File Offset: 0x000F3118
		public CodeTypeParameterCollection(CodeTypeParameter[] value)
		{
			this.AddRange(value);
		}

		// Token: 0x17000E45 RID: 3653
		public CodeTypeParameter this[int index]
		{
			get
			{
				return (CodeTypeParameter)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x06003B37 RID: 15159 RVA: 0x000F4F49 File Offset: 0x000F3149
		public int Add(CodeTypeParameter value)
		{
			return base.List.Add(value);
		}

		// Token: 0x06003B38 RID: 15160 RVA: 0x000F4F57 File Offset: 0x000F3157
		public void Add(string value)
		{
			this.Add(new CodeTypeParameter(value));
		}

		// Token: 0x06003B39 RID: 15161 RVA: 0x000F4F68 File Offset: 0x000F3168
		public void AddRange(CodeTypeParameter[] value)
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

		// Token: 0x06003B3A RID: 15162 RVA: 0x000F4F9C File Offset: 0x000F319C
		public void AddRange(CodeTypeParameterCollection value)
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

		// Token: 0x06003B3B RID: 15163 RVA: 0x000F4FD8 File Offset: 0x000F31D8
		public bool Contains(CodeTypeParameter value)
		{
			return base.List.Contains(value);
		}

		// Token: 0x06003B3C RID: 15164 RVA: 0x000F4FE6 File Offset: 0x000F31E6
		public void CopyTo(CodeTypeParameter[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x06003B3D RID: 15165 RVA: 0x000F4FF5 File Offset: 0x000F31F5
		public int IndexOf(CodeTypeParameter value)
		{
			return base.List.IndexOf(value);
		}

		// Token: 0x06003B3E RID: 15166 RVA: 0x000F5003 File Offset: 0x000F3203
		public void Insert(int index, CodeTypeParameter value)
		{
			base.List.Insert(index, value);
		}

		// Token: 0x06003B3F RID: 15167 RVA: 0x000F5012 File Offset: 0x000F3212
		public void Remove(CodeTypeParameter value)
		{
			base.List.Remove(value);
		}
	}
}
