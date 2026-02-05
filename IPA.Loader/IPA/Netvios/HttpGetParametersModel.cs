using System;
using System.Net;

namespace IPA.Netvios
{
	/// <summary>
	/// GET请求参数模型
	/// </summary>
	// Token: 0x0200002A RID: 42
	public class HttpGetParametersModel
	{
		/// <summary>
		/// 请求的URL(GET方式就附加参数)
		/// </summary>
		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000F0 RID: 240 RVA: 0x00004B5C File Offset: 0x00002D5C
		// (set) Token: 0x060000F1 RID: 241 RVA: 0x00004B64 File Offset: 0x00002D64
		public string Url { get; set; }

		/// <summary>
		/// 超时时间
		/// </summary>
		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000F2 RID: 242 RVA: 0x00004B6D File Offset: 0x00002D6D
		// (set) Token: 0x060000F3 RID: 243 RVA: 0x00004B75 File Offset: 0x00002D75
		public int? Timeout { get; set; }

		/// <summary>
		///             浏览器代理类型
		/// </summary>
		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000F4 RID: 244 RVA: 0x00004B7E File Offset: 0x00002D7E
		// (set) Token: 0x060000F5 RID: 245 RVA: 0x00004B86 File Offset: 0x00002D86
		public string UserAgent { get; set; }

		/// <summary>
		/// Web请求代理
		/// </summary>
		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000F6 RID: 246 RVA: 0x00004B8F File Offset: 0x00002D8F
		// (set) Token: 0x060000F7 RID: 247 RVA: 0x00004B97 File Offset: 0x00002D97
		public IWebProxy WebProxy { get; set; }

		/// <summary>
		/// Cookies列表
		/// </summary>
		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000F8 RID: 248 RVA: 0x00004BA0 File Offset: 0x00002DA0
		// (set) Token: 0x060000F9 RID: 249 RVA: 0x00004BA8 File Offset: 0x00002DA8
		public CookieCollection Cookies { get; set; }
	}
}
