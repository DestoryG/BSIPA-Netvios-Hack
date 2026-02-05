using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace BeatSaverSharp
{
	// Token: 0x02000004 RID: 4
	public sealed class BeatSaver
	{
		// Token: 0x06000003 RID: 3 RVA: 0x00002060 File Offset: 0x00000260
		public BeatSaver(HttpOptions options = default(HttpOptions))
		{
			this.HttpInstance = new Http(options);
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000004 RID: 4 RVA: 0x00002074 File Offset: 0x00000274
		// (set) Token: 0x06000005 RID: 5 RVA: 0x0000207C File Offset: 0x0000027C
		internal Http HttpInstance { get; private set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000006 RID: 6 RVA: 0x00002085 File Offset: 0x00000285
		internal HttpClient HttpClient
		{
			get
			{
				Http httpInstance = this.HttpInstance;
				if (httpInstance == null)
				{
					return null;
				}
				return httpInstance.Client;
			}
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002098 File Offset: 0x00000298
		internal async Task<Page> FetchPaged(string url, CancellationToken token, IProgress<double> progress = null)
		{
			HttpResponse httpResponse = await this.HttpInstance.GetAsync(url, token, progress).ConfigureAwait(false);
			Page page;
			if (httpResponse.StatusCode == HttpStatusCode.NotFound)
			{
				page = null;
			}
			else
			{
				Page page2 = httpResponse.JSON<Page>();
				page2.Client = this;
				foreach (Beatmap beatmap in page2.Docs)
				{
					beatmap.Client = this;
					beatmap.Uploader.Client = this;
				}
				page = page2;
			}
			return page;
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000020F4 File Offset: 0x000002F4
		internal async Task<Beatmap> FetchSingle(string url, CancellationToken token, IProgress<double> progress = null)
		{
			HttpResponse httpResponse = await this.HttpInstance.GetAsync(url, token, progress).ConfigureAwait(false);
			Beatmap beatmap;
			if (httpResponse.StatusCode == HttpStatusCode.NotFound)
			{
				beatmap = null;
			}
			else
			{
				Beatmap beatmap2 = httpResponse.JSON<Beatmap>();
				beatmap2.Client = this;
				beatmap2.Uploader.Client = this;
				beatmap = beatmap2;
			}
			return beatmap;
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002150 File Offset: 0x00000350
		internal async Task<Page> FetchMapsPage(string type, uint page, CancellationToken token, IProgress<double> progress = null)
		{
			object obj = await this.FetchPaged(string.Format("maps/{0}/{1}", type, page), token, progress).ConfigureAwait(false);
			obj.PageURI = "maps/" + type;
			return obj;
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000021B4 File Offset: 0x000003B4
		internal async Task<Page> FetchSearchPage(string searchType, string query, uint page, CancellationToken token, IProgress<double> progress = null)
		{
			if (query == null)
			{
				throw new ArgumentNullException("query", "Query string cannot be null");
			}
			string text = Uri.EscapeUriString(query);
			string pageURI = "search/" + searchType;
			string text2 = string.Format("{0}/{1}?q={2}", pageURI, page, text);
			object obj = await this.FetchPaged(text2, token, progress).ConfigureAwait(false);
			obj.Query = query;
			obj.PageURI = pageURI;
			return obj;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002224 File Offset: 0x00000424
		public async Task<Page> Latest(uint page = 0U, IProgress<double> progress = null)
		{
			return await this.FetchMapsPage("latest", page, CancellationToken.None, progress).ConfigureAwait(false);
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002278 File Offset: 0x00000478
		public async Task<Page> Latest(uint page, CancellationToken token, IProgress<double> progress = null)
		{
			return await this.FetchMapsPage("latest", page, token, progress).ConfigureAwait(false);
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000022D4 File Offset: 0x000004D4
		public async Task<Page> Hot(uint page = 0U, IProgress<double> progress = null)
		{
			return await this.FetchMapsPage("hot", page, CancellationToken.None, progress).ConfigureAwait(false);
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002328 File Offset: 0x00000528
		public async Task<Page> Hot(uint page, CancellationToken token, IProgress<double> progress = null)
		{
			return await this.FetchMapsPage("hot", page, token, progress).ConfigureAwait(false);
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002384 File Offset: 0x00000584
		public async Task<Page> Rating(uint page = 0U, IProgress<double> progress = null)
		{
			return await this.FetchMapsPage("rating", page, CancellationToken.None, progress).ConfigureAwait(false);
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000023D8 File Offset: 0x000005D8
		public async Task<Page> Rating(uint page, CancellationToken token, IProgress<double> progress = null)
		{
			return await this.FetchMapsPage("rating", page, token, progress).ConfigureAwait(false);
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002434 File Offset: 0x00000634
		public async Task<Page> Downloads(uint page = 0U, IProgress<double> progress = null)
		{
			return await this.FetchMapsPage("downloads", page, CancellationToken.None, progress).ConfigureAwait(false);
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002488 File Offset: 0x00000688
		public async Task<Page> Downloads(uint page, CancellationToken token, IProgress<double> progress = null)
		{
			return await this.FetchMapsPage("downloads", page, token, progress).ConfigureAwait(false);
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000024E4 File Offset: 0x000006E4
		public async Task<Page> Plays(uint page = 0U, IProgress<double> progress = null)
		{
			return await this.FetchMapsPage("plays", page, CancellationToken.None, progress).ConfigureAwait(false);
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002538 File Offset: 0x00000738
		public async Task<Page> Plays(uint page, CancellationToken token, IProgress<double> progress = null)
		{
			return await this.FetchMapsPage("plays", page, token, progress).ConfigureAwait(false);
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002594 File Offset: 0x00000794
		public async Task<Beatmap> Key(string key, IProgress<double> progress = null)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			return await this.FetchSingle("maps/detail/" + key, CancellationToken.None, progress).ConfigureAwait(false);
		}

		// Token: 0x06000016 RID: 22 RVA: 0x000025E8 File Offset: 0x000007E8
		public async Task<Beatmap> Key(string key, CancellationToken token, IProgress<double> progress = null)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			return await this.FetchSingle("maps/detail/" + key, token, progress).ConfigureAwait(false);
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002644 File Offset: 0x00000844
		public async Task<Beatmap> Hash(string hash, IProgress<double> progress = null)
		{
			if (hash == null)
			{
				throw new ArgumentNullException("hash");
			}
			return await this.FetchSingle("maps/by-hash/" + hash, CancellationToken.None, progress).ConfigureAwait(false);
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002698 File Offset: 0x00000898
		public async Task<Beatmap> Hash(string hash, CancellationToken token, IProgress<double> progress = null)
		{
			if (hash == null)
			{
				throw new ArgumentNullException("hash");
			}
			return await this.FetchSingle("maps/by-hash/" + hash, token, progress).ConfigureAwait(false);
		}

		// Token: 0x06000019 RID: 25 RVA: 0x000026F4 File Offset: 0x000008F4
		public async Task<Page> Search(string query, uint page = 0U, IProgress<double> progress = null)
		{
			return await this.FetchSearchPage("text", query, page, CancellationToken.None, progress).ConfigureAwait(false);
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002750 File Offset: 0x00000950
		public async Task<Page> Search(string query, uint page, CancellationToken token, IProgress<double> progress = null)
		{
			return await this.FetchSearchPage("text", query, page, token, progress).ConfigureAwait(false);
		}

		// Token: 0x0600001B RID: 27 RVA: 0x000027B4 File Offset: 0x000009B4
		public async Task<Page> SearchAdvanced(string query, uint page = 0U, IProgress<double> progress = null)
		{
			return await this.FetchSearchPage("advanced", query, page, CancellationToken.None, progress).ConfigureAwait(false);
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002810 File Offset: 0x00000A10
		public async Task<Page> SearchAdvanced(string query, uint page, CancellationToken token, IProgress<double> progress = null)
		{
			return await this.FetchSearchPage("advanced", query, page, token, progress).ConfigureAwait(false);
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002874 File Offset: 0x00000A74
		public async Task<User> User(string id, IProgress<double> progress = null)
		{
			return await this.User(id, CancellationToken.None, progress).ConfigureAwait(false);
		}

		// Token: 0x0600001E RID: 30 RVA: 0x000028C8 File Offset: 0x00000AC8
		public async Task<User> User(string id, CancellationToken token, IProgress<double> progress = null)
		{
			if (id == null)
			{
				throw new ArgumentNullException("id");
			}
			HttpResponse httpResponse = await this.HttpInstance.GetAsync("users/find/" + id, token, progress).ConfigureAwait(false);
			User user;
			if (httpResponse.StatusCode == HttpStatusCode.NotFound)
			{
				user = null;
			}
			else
			{
				User user2 = httpResponse.JSON<User>();
				user2.Client = this;
				user = user2;
			}
			return user;
		}

		// Token: 0x04000001 RID: 1
		public const string BaseURL = "https://beatsaberbbs.com";

		// Token: 0x04000002 RID: 2
		public static readonly BeatSaver Client = new BeatSaver(default(HttpOptions));
	}
}
