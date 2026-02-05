using System;
using System.Collections.Generic;
using System.Text;

namespace IPA.Netvios
{
	/// <summary>
	/// POST请求参数模型
	/// </summary>
	// Token: 0x0200002B RID: 43
	public class HttpPostParametersModel : HttpGetParametersModel
	{
		/// <summary>
		/// POST参数字典
		/// </summary>
		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000FB RID: 251 RVA: 0x00004BB9 File Offset: 0x00002DB9
		// (set) Token: 0x060000FC RID: 252 RVA: 0x00004BC1 File Offset: 0x00002DC1
		public IDictionary<string, string> DictParameters { get; set; }

		/// <summary>
		/// 发送HTTP请求时所用的编码
		/// </summary>
		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000FD RID: 253 RVA: 0x00004BCA File Offset: 0x00002DCA
		// (set) Token: 0x060000FE RID: 254 RVA: 0x00004BD2 File Offset: 0x00002DD2
		public Encoding RequestEncoding { get; set; }
	}
}
