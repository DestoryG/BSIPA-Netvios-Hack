using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BeatSaverSharp
{
	// Token: 0x02000010 RID: 16
	public sealed class Page
	{
		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000A7 RID: 167 RVA: 0x0000376C File Offset: 0x0000196C
		// (set) Token: 0x060000A8 RID: 168 RVA: 0x00003774 File Offset: 0x00001974
		[JsonProperty("docs")]
		public ReadOnlyCollection<Beatmap> Docs { get; private set; }

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000A9 RID: 169 RVA: 0x0000377D File Offset: 0x0000197D
		// (set) Token: 0x060000AA RID: 170 RVA: 0x00003785 File Offset: 0x00001985
		[JsonProperty("totalDocs")]
		public int TotalDocs { get; private set; }

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000AB RID: 171 RVA: 0x0000378E File Offset: 0x0000198E
		// (set) Token: 0x060000AC RID: 172 RVA: 0x00003796 File Offset: 0x00001996
		[JsonProperty("lastPage")]
		public int LastPage { get; private set; }

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000AD RID: 173 RVA: 0x0000379F File Offset: 0x0000199F
		// (set) Token: 0x060000AE RID: 174 RVA: 0x000037A7 File Offset: 0x000019A7
		[JsonProperty("prevPage")]
		public int? PreviousPage { get; private set; }

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000AF RID: 175 RVA: 0x000037B0 File Offset: 0x000019B0
		// (set) Token: 0x060000B0 RID: 176 RVA: 0x000037B8 File Offset: 0x000019B8
		[JsonProperty("nextPage")]
		public int? NextPage { get; private set; }

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060000B1 RID: 177 RVA: 0x000037C1 File Offset: 0x000019C1
		// (set) Token: 0x060000B2 RID: 178 RVA: 0x000037C9 File Offset: 0x000019C9
		[JsonIgnore]
		internal BeatSaver Client { get; set; }

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060000B3 RID: 179 RVA: 0x000037D2 File Offset: 0x000019D2
		// (set) Token: 0x060000B4 RID: 180 RVA: 0x000037DA File Offset: 0x000019DA
		[JsonIgnore]
		internal string PageURI { get; set; }

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060000B5 RID: 181 RVA: 0x000037E3 File Offset: 0x000019E3
		// (set) Token: 0x060000B6 RID: 182 RVA: 0x000037EB File Offset: 0x000019EB
		[JsonIgnore]
		internal string Query { get; set; }

		// Token: 0x060000B7 RID: 183 RVA: 0x000037F4 File Offset: 0x000019F4
		public async Task<Page> FetchPreviousPage(IProgress<double> progress = null)
		{
			return await this.FetchPreviousPage(CancellationToken.None, progress).ConfigureAwait(false);
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00003840 File Offset: 0x00001A40
		public async Task<Page> FetchPreviousPage(CancellationToken token, IProgress<double> progress = null)
		{
			Page page;
			if (this.PreviousPage == null)
			{
				page = null;
			}
			else
			{
				string text = string.Format("{0}/{1}", this.PageURI, this.PreviousPage);
				if (this.Query != null)
				{
					text = text + "?q=" + Uri.EscapeUriString(this.Query);
				}
				object obj = await this.Client.FetchPaged(text, token, progress).ConfigureAwait(false);
				obj.PageURI = this.PageURI;
				obj.Query = this.Query;
				page = obj;
			}
			return page;
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00003894 File Offset: 0x00001A94
		public async Task<Page> FetchNextPage(IProgress<double> progress = null)
		{
			return await this.FetchNextPage(CancellationToken.None, progress).ConfigureAwait(false);
		}

		// Token: 0x060000BA RID: 186 RVA: 0x000038E0 File Offset: 0x00001AE0
		public async Task<Page> FetchNextPage(CancellationToken token, IProgress<double> progress = null)
		{
			Page page;
			if (this.NextPage == null)
			{
				page = null;
			}
			else
			{
				string text = string.Format("{0}/{1}", this.PageURI, this.NextPage);
				if (this.Query != null)
				{
					text = text + "?q=" + Uri.EscapeUriString(this.Query);
				}
				object obj = await this.Client.FetchPaged(text, token, progress).ConfigureAwait(false);
				obj.PageURI = this.PageURI;
				obj.Query = this.Query;
				page = obj;
			}
			return page;
		}
	}
}
