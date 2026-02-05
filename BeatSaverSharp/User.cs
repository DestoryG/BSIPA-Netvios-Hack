using System;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BeatSaverSharp
{
	// Token: 0x02000015 RID: 21
	public sealed class User : IEquatable<User>
	{
		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060000C0 RID: 192 RVA: 0x0000395D File Offset: 0x00001B5D
		// (set) Token: 0x060000C1 RID: 193 RVA: 0x00003965 File Offset: 0x00001B65
		[JsonProperty("_id")]
		public string ID { get; private set; }

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060000C2 RID: 194 RVA: 0x0000396E File Offset: 0x00001B6E
		// (set) Token: 0x060000C3 RID: 195 RVA: 0x00003976 File Offset: 0x00001B76
		[JsonProperty("username")]
		public string Username { get; private set; }

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060000C4 RID: 196 RVA: 0x0000397F File Offset: 0x00001B7F
		// (set) Token: 0x060000C5 RID: 197 RVA: 0x00003987 File Offset: 0x00001B87
		[JsonIgnore]
		internal BeatSaver Client { get; set; }

		// Token: 0x060000C6 RID: 198 RVA: 0x00003990 File Offset: 0x00001B90
		public async Task<Page> Beatmaps(uint page = 0U, IProgress<double> progress = null)
		{
			return await this.Beatmaps(page, CancellationToken.None, progress).ConfigureAwait(false);
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x000039E4 File Offset: 0x00001BE4
		public async Task<Page> Beatmaps(uint page, CancellationToken token, IProgress<double> progress = null)
		{
			string pageURI = "maps/uploader/" + this.ID;
			string text = string.Format("{0}/{1}", pageURI, page);
			object obj = await this.Client.FetchPaged(text, token, progress).ConfigureAwait(false);
			obj.PageURI = pageURI;
			return obj;
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00003A3F File Offset: 0x00001C3F
		public override bool Equals(object obj)
		{
			return this.Equals(obj as User);
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00003A4D File Offset: 0x00001C4D
		public bool Equals(User u)
		{
			return u != null && (this == u || (!(base.GetType() != u.GetType()) && this.ID == u.ID));
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00003A80 File Offset: 0x00001C80
		public override int GetHashCode()
		{
			return this.ID.GetHashCode();
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00003A8D File Offset: 0x00001C8D
		public static bool operator ==(User lhs, User rhs)
		{
			if (lhs == null)
			{
				return rhs == null;
			}
			return lhs.Equals(rhs);
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00003AA0 File Offset: 0x00001CA0
		public static bool operator !=(User lhs, User rhs)
		{
			return !(lhs == rhs);
		}
	}
}
