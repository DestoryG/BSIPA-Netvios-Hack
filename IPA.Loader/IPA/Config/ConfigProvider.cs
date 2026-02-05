using System;
using System.IO;
using IPA.Config.Data;

namespace IPA.Config
{
	/// <summary>
	/// A wrapper for an <see cref="T:IPA.Config.IConfigProvider" /> and the <see cref="T:System.IO.FileInfo" /> to use with it.
	/// </summary>
	// Token: 0x0200005E RID: 94
	public class ConfigProvider
	{
		// Token: 0x060002A3 RID: 675 RVA: 0x0000D6D5 File Offset: 0x0000B8D5
		internal ConfigProvider(FileInfo file, IConfigProvider provider)
		{
			this.file = file;
			this.provider = provider;
		}

		/// <summary>
		/// Stores the <see cref="T:IPA.Config.Data.Value" /> given to disk in the format specified.
		/// </summary>
		/// <param name="value">the <see cref="T:IPA.Config.Data.Value" /> to store</param>
		// Token: 0x060002A4 RID: 676 RVA: 0x0000D6EB File Offset: 0x0000B8EB
		public void Store(Value value)
		{
			this.provider.Store(value, this.file);
		}

		/// <summary>
		/// Loads a <see cref="T:IPA.Config.Data.Value" /> from disk in whatever format this provider provides
		/// and returns it.
		/// </summary>
		/// <returns>the <see cref="T:IPA.Config.Data.Value" /> loaded</returns>
		// Token: 0x060002A5 RID: 677 RVA: 0x0000D6FF File Offset: 0x0000B8FF
		public Value Load()
		{
			return this.provider.Load(this.file);
		}

		// Token: 0x040000FB RID: 251
		private readonly FileInfo file;

		// Token: 0x040000FC RID: 252
		private readonly IConfigProvider provider;
	}
}
