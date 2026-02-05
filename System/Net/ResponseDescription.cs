using System;
using System.Text;

namespace System.Net
{
	// Token: 0x02000199 RID: 409
	internal class ResponseDescription
	{
		// Token: 0x17000377 RID: 887
		// (get) Token: 0x06000FEE RID: 4078 RVA: 0x0005372C File Offset: 0x0005192C
		internal bool PositiveIntermediate
		{
			get
			{
				return this.Status >= 100 && this.Status <= 199;
			}
		}

		// Token: 0x17000378 RID: 888
		// (get) Token: 0x06000FEF RID: 4079 RVA: 0x0005374A File Offset: 0x0005194A
		internal bool PositiveCompletion
		{
			get
			{
				return this.Status >= 200 && this.Status <= 299;
			}
		}

		// Token: 0x17000379 RID: 889
		// (get) Token: 0x06000FF0 RID: 4080 RVA: 0x0005376B File Offset: 0x0005196B
		internal bool TransientFailure
		{
			get
			{
				return this.Status >= 400 && this.Status <= 499;
			}
		}

		// Token: 0x1700037A RID: 890
		// (get) Token: 0x06000FF1 RID: 4081 RVA: 0x0005378C File Offset: 0x0005198C
		internal bool PermanentFailure
		{
			get
			{
				return this.Status >= 500 && this.Status <= 599;
			}
		}

		// Token: 0x1700037B RID: 891
		// (get) Token: 0x06000FF2 RID: 4082 RVA: 0x000537AD File Offset: 0x000519AD
		internal bool InvalidStatusCode
		{
			get
			{
				return this.Status < 100 || this.Status > 599;
			}
		}

		// Token: 0x04001300 RID: 4864
		internal const int NoStatus = -1;

		// Token: 0x04001301 RID: 4865
		internal bool Multiline;

		// Token: 0x04001302 RID: 4866
		internal int Status = -1;

		// Token: 0x04001303 RID: 4867
		internal string StatusDescription;

		// Token: 0x04001304 RID: 4868
		internal StringBuilder StatusBuffer = new StringBuilder();

		// Token: 0x04001305 RID: 4869
		internal string StatusCodeString;
	}
}
