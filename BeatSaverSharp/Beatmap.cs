using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BeatSaverSharp.Exceptions;
using Newtonsoft.Json;

namespace BeatSaverSharp
{
	// Token: 0x0200000A RID: 10
	public sealed class Beatmap : IEquatable<Beatmap>
	{
		// Token: 0x0600003D RID: 61 RVA: 0x00002EC0 File Offset: 0x000010C0
		[JsonConstructor]
		public Beatmap()
		{
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002EC8 File Offset: 0x000010C8
		public Beatmap(BeatSaver client = null, string key = null, string hash = null, string name = null)
		{
			if (key == null && hash == null)
			{
				throw new ArgumentException("Key and Hash cannot both be null");
			}
			this.Client = client ?? BeatSaver.Client;
			if (key != null)
			{
				this.Key = key;
			}
			if (hash != null)
			{
				this.Hash = hash;
			}
			if (name != null)
			{
				this.Name = name;
			}
			this.Partial = true;
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600003F RID: 63 RVA: 0x00002F23 File Offset: 0x00001123
		// (set) Token: 0x06000040 RID: 64 RVA: 0x00002F2B File Offset: 0x0000112B
		[JsonProperty("_id")]
		public string ID { get; private set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000041 RID: 65 RVA: 0x00002F34 File Offset: 0x00001134
		// (set) Token: 0x06000042 RID: 66 RVA: 0x00002F3C File Offset: 0x0000113C
		[JsonProperty("key")]
		public string Key { get; private set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000043 RID: 67 RVA: 0x00002F45 File Offset: 0x00001145
		// (set) Token: 0x06000044 RID: 68 RVA: 0x00002F4D File Offset: 0x0000114D
		[JsonProperty("name")]
		public string Name { get; private set; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000045 RID: 69 RVA: 0x00002F56 File Offset: 0x00001156
		// (set) Token: 0x06000046 RID: 70 RVA: 0x00002F5E File Offset: 0x0000115E
		[JsonProperty("description")]
		public string Description { get; private set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000047 RID: 71 RVA: 0x00002F67 File Offset: 0x00001167
		// (set) Token: 0x06000048 RID: 72 RVA: 0x00002F6F File Offset: 0x0000116F
		[JsonProperty("uploader")]
		public User Uploader { get; private set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000049 RID: 73 RVA: 0x00002F78 File Offset: 0x00001178
		// (set) Token: 0x0600004A RID: 74 RVA: 0x00002F80 File Offset: 0x00001180
		[JsonProperty("uploaded")]
		public DateTime Uploaded { get; private set; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600004B RID: 75 RVA: 0x00002F89 File Offset: 0x00001189
		// (set) Token: 0x0600004C RID: 76 RVA: 0x00002F91 File Offset: 0x00001191
		[JsonProperty("metadata")]
		public Metadata Metadata { get; private set; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600004D RID: 77 RVA: 0x00002F9A File Offset: 0x0000119A
		// (set) Token: 0x0600004E RID: 78 RVA: 0x00002FA2 File Offset: 0x000011A2
		[JsonProperty("stats")]
		public Stats Stats { get; private set; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600004F RID: 79 RVA: 0x00002FAB File Offset: 0x000011AB
		// (set) Token: 0x06000050 RID: 80 RVA: 0x00002FB3 File Offset: 0x000011B3
		[JsonProperty("directDownload")]
		public string DirectDownload { get; private set; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000051 RID: 81 RVA: 0x00002FBC File Offset: 0x000011BC
		// (set) Token: 0x06000052 RID: 82 RVA: 0x00002FC4 File Offset: 0x000011C4
		[JsonProperty("downloadURL")]
		public string DownloadURL { get; private set; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000053 RID: 83 RVA: 0x00002FCD File Offset: 0x000011CD
		// (set) Token: 0x06000054 RID: 84 RVA: 0x00002FD5 File Offset: 0x000011D5
		[JsonProperty("coverURL")]
		public string CoverURL { get; private set; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000055 RID: 85 RVA: 0x00002FDE File Offset: 0x000011DE
		// (set) Token: 0x06000056 RID: 86 RVA: 0x00002FE6 File Offset: 0x000011E6
		[JsonProperty("hash")]
		public string Hash { get; private set; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000057 RID: 87 RVA: 0x00002FEF File Offset: 0x000011EF
		// (set) Token: 0x06000058 RID: 88 RVA: 0x00002FF7 File Offset: 0x000011F7
		[JsonIgnore]
		internal BeatSaver Client { get; set; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000059 RID: 89 RVA: 0x00003000 File Offset: 0x00001200
		[JsonIgnore]
		public string CoverFilename
		{
			get
			{
				return Path.GetFileName(this.CoverURL);
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600005A RID: 90 RVA: 0x0000300D File Offset: 0x0000120D
		// (set) Token: 0x0600005B RID: 91 RVA: 0x00003015 File Offset: 0x00001215
		[JsonIgnore]
		public bool Partial { get; private set; }

		// Token: 0x0600005C RID: 92 RVA: 0x00003020 File Offset: 0x00001220
		public async Task Populate()
		{
			if (this.Key == null && this.Hash == null)
			{
				throw new InvalidPartialException("Key and Hash cannot both be null");
			}
			if (this.Partial)
			{
				Beatmap beatmap;
				if (this.Hash != null)
				{
					beatmap = await this.Client.Hash(this.Hash, null).ConfigureAwait(false);
				}
				else
				{
					beatmap = await this.Client.Key(this.Key, null).ConfigureAwait(false);
				}
				Beatmap beatmap2 = beatmap;
				if (beatmap2 == null)
				{
					if (this.Hash != null)
					{
						throw new InvalidPartialHashException(this.Hash);
					}
					if (this.Key != null)
					{
						throw new InvalidPartialKeyException(this.Key);
					}
					throw new InvalidPartialException();
				}
				else
				{
					this.ID = beatmap2.ID;
					this.Key = beatmap2.Key;
					this.Name = beatmap2.Name;
					this.Description = beatmap2.Description;
					this.Uploader = beatmap2.Uploader;
					this.Uploaded = beatmap2.Uploaded;
					this.Metadata = beatmap2.Metadata;
					this.Stats = beatmap2.Stats;
					this.DirectDownload = beatmap2.DirectDownload;
					this.DownloadURL = beatmap2.DownloadURL;
					this.CoverURL = beatmap2.CoverURL;
					this.Hash = beatmap2.Hash;
					this.Partial = false;
				}
			}
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00003064 File Offset: 0x00001264
		public async Task Refresh()
		{
			Beatmap beatmap = await this.Client.Hash(this.Hash, null).ConfigureAwait(false);
			this.Name = beatmap.Name;
			this.Description = beatmap.Description;
			this.Stats = beatmap.Stats;
		}

		// Token: 0x0600005E RID: 94 RVA: 0x000030A8 File Offset: 0x000012A8
		public async Task RefreshStats()
		{
			Beatmap beatmap = await this.Client.Hash(this.Hash, null).ConfigureAwait(false);
			this.Stats = beatmap.Stats;
		}

		// Token: 0x0600005F RID: 95 RVA: 0x000030EC File Offset: 0x000012EC
		private async Task<bool> Vote(Beatmap.VoteDirection direction, string steamID, byte[] authTicket)
		{
			Beatmap.VotePayload votePayload = new Beatmap.VotePayload(direction, steamID, authTicket);
			return await this.Vote(votePayload).ConfigureAwait(false);
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00003148 File Offset: 0x00001348
		private async Task<bool> Vote(Beatmap.VoteDirection direction, string steamID, string authTicket)
		{
			Beatmap.VotePayload votePayload = new Beatmap.VotePayload(direction, steamID, authTicket);
			return await this.Vote(votePayload).ConfigureAwait(false);
		}

		// Token: 0x06000061 RID: 97 RVA: 0x000031A4 File Offset: 0x000013A4
		private async Task<bool> Vote(Beatmap.VotePayload payload)
		{
			HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
			HttpResponseMessage httpResponseMessage = await this.Client.HttpClient.PostAsync("vote/steam/" + this.Key, httpContent).ConfigureAwait(false);
			if (httpResponseMessage.IsSuccessStatusCode)
			{
				using (Stream stream = await httpResponseMessage.Content.ReadAsStreamAsync().ConfigureAwait(false))
				{
					using (StreamReader streamReader = new StreamReader(stream))
					{
						using (JsonReader jsonReader = new JsonTextReader(streamReader))
						{
							this.Stats = Http.Serializer.Deserialize<Beatmap>(jsonReader).Stats;
							return true;
						}
					}
				}
			}
			RestError restError;
			using (Stream stream2 = await httpResponseMessage.Content.ReadAsStreamAsync().ConfigureAwait(false))
			{
				using (StreamReader streamReader2 = new StreamReader(stream2))
				{
					using (JsonReader jsonReader2 = new JsonTextReader(streamReader2))
					{
						restError = Http.Serializer.Deserialize<RestError>(jsonReader2);
					}
				}
			}
			if (restError.Identifier == "ERR_INVALID_STEAM_ID")
			{
				throw new InvalidSteamIDException(payload.SteamID);
			}
			if (restError.Identifier == "ERR_STEAM_ID_MISMATCH")
			{
				throw new InvalidSteamIDException(payload.SteamID);
			}
			if (restError.Identifier == "ERR_INVALID_TICKET")
			{
				throw new InvalidTicketException();
			}
			if (restError.Identifier == "ERR_BAD_TICKET")
			{
				throw new InvalidTicketException();
			}
			return false;
		}

		// Token: 0x06000062 RID: 98 RVA: 0x000031F0 File Offset: 0x000013F0
		public async Task<bool> VoteUp(string steamID, byte[] authTicket)
		{
			return await this.Vote(Beatmap.VoteDirection.Up, steamID, authTicket).ConfigureAwait(false);
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00003244 File Offset: 0x00001444
		public async Task<bool> VoteUp(string steamID, string authTicket)
		{
			return await this.Vote(Beatmap.VoteDirection.Up, steamID, authTicket).ConfigureAwait(false);
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00003298 File Offset: 0x00001498
		public async Task<bool> VoteDown(string steamID, byte[] authTicket)
		{
			return await this.Vote(Beatmap.VoteDirection.Down, steamID, authTicket).ConfigureAwait(false);
		}

		// Token: 0x06000065 RID: 101 RVA: 0x000032EC File Offset: 0x000014EC
		public async Task<bool> VoteDown(string steamID, string authTicket)
		{
			return await this.Vote(Beatmap.VoteDirection.Down, steamID, authTicket).ConfigureAwait(false);
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00003340 File Offset: 0x00001540
		public async Task<byte[]> DownloadZip(bool direct = false, IProgress<double> progress = null)
		{
			return await this.DownloadZip(direct, CancellationToken.None, progress).ConfigureAwait(false);
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00003394 File Offset: 0x00001594
		public async Task<byte[]> DownloadZip(bool direct, CancellationToken token, IProgress<double> progress = null)
		{
			string text = (direct ? this.DirectDownload : this.DownloadURL);
			return (await this.Client.HttpInstance.GetAsync(text, token, progress).ConfigureAwait(false)).Bytes();
		}

		// Token: 0x06000068 RID: 104 RVA: 0x000033F0 File Offset: 0x000015F0
		public async Task<byte[]> FetchCoverImage(IProgress<double> progress = null)
		{
			return await this.FetchCoverImage(CancellationToken.None, progress).ConfigureAwait(false);
		}

		// Token: 0x06000069 RID: 105 RVA: 0x0000343C File Offset: 0x0000163C
		public async Task<byte[]> FetchCoverImage(CancellationToken token, IProgress<double> progress = null)
		{
			string text = this.CoverURL ?? "";
			return (await this.Client.HttpInstance.GetAsync(text, token, progress).ConfigureAwait(false)).Bytes();
		}

		// Token: 0x0600006A RID: 106 RVA: 0x0000348F File Offset: 0x0000168F
		public override bool Equals(object obj)
		{
			return this.Equals(obj as Beatmap);
		}

		// Token: 0x0600006B RID: 107 RVA: 0x000034A0 File Offset: 0x000016A0
		public bool Equals(Beatmap b)
		{
			return b != null && (this == b || (!(base.GetType() != b.GetType()) && (this.ID == b.ID && this.Key == b.Key) && this.Hash == b.Hash));
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00003508 File Offset: 0x00001708
		public override int GetHashCode()
		{
			return (((((-2128831035 * 16777619) ^ ((this.ID == null) ? 0 : this.ID.GetHashCode())) * 16777619) ^ ((this.Key == null) ? 0 : this.Key.GetHashCode())) * 16777619) ^ ((this.Hash == null) ? 0 : this.Hash.GetHashCode());
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00003571 File Offset: 0x00001771
		public static bool operator ==(Beatmap lhs, Beatmap rhs)
		{
			if (lhs == null)
			{
				return rhs == null;
			}
			return lhs.Equals(rhs);
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00003584 File Offset: 0x00001784
		public static bool operator !=(Beatmap lhs, Beatmap rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x02000035 RID: 53
		private enum VoteDirection : short
		{
			// Token: 0x04000118 RID: 280
			Up = 1,
			// Token: 0x04000119 RID: 281
			Down = -1
		}

		// Token: 0x02000036 RID: 54
		private struct VotePayload
		{
			// Token: 0x17000046 RID: 70
			// (get) Token: 0x0600010E RID: 270 RVA: 0x000058E6 File Offset: 0x00003AE6
			// (set) Token: 0x0600010F RID: 271 RVA: 0x000058EE File Offset: 0x00003AEE
			[JsonProperty("steamID")]
			public string SteamID { readonly get; set; }

			// Token: 0x17000047 RID: 71
			// (get) Token: 0x06000110 RID: 272 RVA: 0x000058F7 File Offset: 0x00003AF7
			// (set) Token: 0x06000111 RID: 273 RVA: 0x000058FF File Offset: 0x00003AFF
			[JsonProperty("ticket")]
			public string Ticket { readonly get; set; }

			// Token: 0x17000048 RID: 72
			// (get) Token: 0x06000112 RID: 274 RVA: 0x00005908 File Offset: 0x00003B08
			// (set) Token: 0x06000113 RID: 275 RVA: 0x00005910 File Offset: 0x00003B10
			[JsonProperty("direction")]
			public string Direction { readonly get; set; }

			// Token: 0x06000114 RID: 276 RVA: 0x0000591C File Offset: 0x00003B1C
			public VotePayload(Beatmap.VoteDirection direction, string steamID, byte[] authTicket)
			{
				this.SteamID = steamID;
				this.Ticket = string.Concat(Array.ConvertAll<byte, string>(authTicket, (byte x) => x.ToString("X2")));
				short num = (short)direction;
				this.Direction = num.ToString();
			}

			// Token: 0x06000115 RID: 277 RVA: 0x00005970 File Offset: 0x00003B70
			public VotePayload(Beatmap.VoteDirection direction, string steamID, string authTicket)
			{
				this.SteamID = steamID;
				this.Ticket = authTicket;
				short num = (short)direction;
				this.Direction = num.ToString();
			}
		}
	}
}
