using System;
using System.Collections;
using System.Security.Permissions;

namespace System.ComponentModel.Design.Serialization
{
	// Token: 0x02000604 RID: 1540
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public sealed class ContextStack
	{
		// Token: 0x17000D86 RID: 3462
		// (get) Token: 0x06003895 RID: 14485 RVA: 0x000F1859 File Offset: 0x000EFA59
		public object Current
		{
			get
			{
				if (this.contextStack != null && this.contextStack.Count > 0)
				{
					return this.contextStack[this.contextStack.Count - 1];
				}
				return null;
			}
		}

		// Token: 0x17000D87 RID: 3463
		public object this[int level]
		{
			get
			{
				if (level < 0)
				{
					throw new ArgumentOutOfRangeException("level");
				}
				if (this.contextStack != null && level < this.contextStack.Count)
				{
					return this.contextStack[this.contextStack.Count - 1 - level];
				}
				return null;
			}
		}

		// Token: 0x17000D88 RID: 3464
		public object this[Type type]
		{
			get
			{
				if (type == null)
				{
					throw new ArgumentNullException("type");
				}
				if (this.contextStack != null)
				{
					int i = this.contextStack.Count;
					while (i > 0)
					{
						object obj = this.contextStack[--i];
						if (type.IsInstanceOfType(obj))
						{
							return obj;
						}
					}
				}
				return null;
			}
		}

		// Token: 0x06003898 RID: 14488 RVA: 0x000F1934 File Offset: 0x000EFB34
		public void Append(object context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			if (this.contextStack == null)
			{
				this.contextStack = new ArrayList();
			}
			this.contextStack.Insert(0, context);
		}

		// Token: 0x06003899 RID: 14489 RVA: 0x000F1964 File Offset: 0x000EFB64
		public object Pop()
		{
			object obj = null;
			if (this.contextStack != null && this.contextStack.Count > 0)
			{
				int num = this.contextStack.Count - 1;
				obj = this.contextStack[num];
				this.contextStack.RemoveAt(num);
			}
			return obj;
		}

		// Token: 0x0600389A RID: 14490 RVA: 0x000F19B1 File Offset: 0x000EFBB1
		public void Push(object context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			if (this.contextStack == null)
			{
				this.contextStack = new ArrayList();
			}
			this.contextStack.Add(context);
		}

		// Token: 0x04002B68 RID: 11112
		private ArrayList contextStack;
	}
}
