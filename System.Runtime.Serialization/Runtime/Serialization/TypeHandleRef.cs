using System;

namespace System.Runtime.Serialization
{
	// Token: 0x02000073 RID: 115
	internal class TypeHandleRef
	{
		// Token: 0x06000892 RID: 2194 RVA: 0x00028083 File Offset: 0x00026283
		public TypeHandleRef()
		{
		}

		// Token: 0x06000893 RID: 2195 RVA: 0x0002808B File Offset: 0x0002628B
		public TypeHandleRef(RuntimeTypeHandle value)
		{
			this.value = value;
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x06000894 RID: 2196 RVA: 0x0002809A File Offset: 0x0002629A
		// (set) Token: 0x06000895 RID: 2197 RVA: 0x000280A2 File Offset: 0x000262A2
		public RuntimeTypeHandle Value
		{
			get
			{
				return this.value;
			}
			set
			{
				this.value = value;
			}
		}

		// Token: 0x04000321 RID: 801
		private RuntimeTypeHandle value;
	}
}
