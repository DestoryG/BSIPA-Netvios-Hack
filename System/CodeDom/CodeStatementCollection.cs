using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000657 RID: 1623
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeStatementCollection : CollectionBase
	{
		// Token: 0x06003ACA RID: 15050 RVA: 0x000F4569 File Offset: 0x000F2769
		public CodeStatementCollection()
		{
		}

		// Token: 0x06003ACB RID: 15051 RVA: 0x000F4571 File Offset: 0x000F2771
		public CodeStatementCollection(CodeStatementCollection value)
		{
			this.AddRange(value);
		}

		// Token: 0x06003ACC RID: 15052 RVA: 0x000F4580 File Offset: 0x000F2780
		public CodeStatementCollection(CodeStatement[] value)
		{
			this.AddRange(value);
		}

		// Token: 0x17000E27 RID: 3623
		public CodeStatement this[int index]
		{
			get
			{
				return (CodeStatement)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x06003ACF RID: 15055 RVA: 0x000F45B1 File Offset: 0x000F27B1
		public int Add(CodeStatement value)
		{
			return base.List.Add(value);
		}

		// Token: 0x06003AD0 RID: 15056 RVA: 0x000F45BF File Offset: 0x000F27BF
		public int Add(CodeExpression value)
		{
			return this.Add(new CodeExpressionStatement(value));
		}

		// Token: 0x06003AD1 RID: 15057 RVA: 0x000F45D0 File Offset: 0x000F27D0
		public void AddRange(CodeStatement[] value)
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

		// Token: 0x06003AD2 RID: 15058 RVA: 0x000F4604 File Offset: 0x000F2804
		public void AddRange(CodeStatementCollection value)
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

		// Token: 0x06003AD3 RID: 15059 RVA: 0x000F4640 File Offset: 0x000F2840
		public bool Contains(CodeStatement value)
		{
			return base.List.Contains(value);
		}

		// Token: 0x06003AD4 RID: 15060 RVA: 0x000F464E File Offset: 0x000F284E
		public void CopyTo(CodeStatement[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x06003AD5 RID: 15061 RVA: 0x000F465D File Offset: 0x000F285D
		public int IndexOf(CodeStatement value)
		{
			return base.List.IndexOf(value);
		}

		// Token: 0x06003AD6 RID: 15062 RVA: 0x000F466B File Offset: 0x000F286B
		public void Insert(int index, CodeStatement value)
		{
			base.List.Insert(index, value);
		}

		// Token: 0x06003AD7 RID: 15063 RVA: 0x000F467A File Offset: 0x000F287A
		public void Remove(CodeStatement value)
		{
			base.List.Remove(value);
		}
	}
}
