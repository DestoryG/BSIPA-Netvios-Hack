using System;
using System.Collections;
using System.Security.Permissions;

namespace System.CodeDom.Compiler
{
	// Token: 0x02000676 RID: 1654
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	[Serializable]
	public class CompilerErrorCollection : CollectionBase
	{
		// Token: 0x06003CCE RID: 15566 RVA: 0x000FAC85 File Offset: 0x000F8E85
		public CompilerErrorCollection()
		{
		}

		// Token: 0x06003CCF RID: 15567 RVA: 0x000FAC8D File Offset: 0x000F8E8D
		public CompilerErrorCollection(CompilerErrorCollection value)
		{
			this.AddRange(value);
		}

		// Token: 0x06003CD0 RID: 15568 RVA: 0x000FAC9C File Offset: 0x000F8E9C
		public CompilerErrorCollection(CompilerError[] value)
		{
			this.AddRange(value);
		}

		// Token: 0x17000E75 RID: 3701
		public CompilerError this[int index]
		{
			get
			{
				return (CompilerError)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x06003CD3 RID: 15571 RVA: 0x000FACCD File Offset: 0x000F8ECD
		public int Add(CompilerError value)
		{
			return base.List.Add(value);
		}

		// Token: 0x06003CD4 RID: 15572 RVA: 0x000FACDC File Offset: 0x000F8EDC
		public void AddRange(CompilerError[] value)
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

		// Token: 0x06003CD5 RID: 15573 RVA: 0x000FAD10 File Offset: 0x000F8F10
		public void AddRange(CompilerErrorCollection value)
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

		// Token: 0x06003CD6 RID: 15574 RVA: 0x000FAD4C File Offset: 0x000F8F4C
		public bool Contains(CompilerError value)
		{
			return base.List.Contains(value);
		}

		// Token: 0x06003CD7 RID: 15575 RVA: 0x000FAD5A File Offset: 0x000F8F5A
		public void CopyTo(CompilerError[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x17000E76 RID: 3702
		// (get) Token: 0x06003CD8 RID: 15576 RVA: 0x000FAD6C File Offset: 0x000F8F6C
		public bool HasErrors
		{
			get
			{
				if (base.Count > 0)
				{
					foreach (object obj in this)
					{
						CompilerError compilerError = (CompilerError)obj;
						if (!compilerError.IsWarning)
						{
							return true;
						}
					}
					return false;
				}
				return false;
			}
		}

		// Token: 0x17000E77 RID: 3703
		// (get) Token: 0x06003CD9 RID: 15577 RVA: 0x000FADD4 File Offset: 0x000F8FD4
		public bool HasWarnings
		{
			get
			{
				if (base.Count > 0)
				{
					foreach (object obj in this)
					{
						CompilerError compilerError = (CompilerError)obj;
						if (compilerError.IsWarning)
						{
							return true;
						}
					}
					return false;
				}
				return false;
			}
		}

		// Token: 0x06003CDA RID: 15578 RVA: 0x000FAE3C File Offset: 0x000F903C
		public int IndexOf(CompilerError value)
		{
			return base.List.IndexOf(value);
		}

		// Token: 0x06003CDB RID: 15579 RVA: 0x000FAE4A File Offset: 0x000F904A
		public void Insert(int index, CompilerError value)
		{
			base.List.Insert(index, value);
		}

		// Token: 0x06003CDC RID: 15580 RVA: 0x000FAE59 File Offset: 0x000F9059
		public void Remove(CompilerError value)
		{
			base.List.Remove(value);
		}
	}
}
