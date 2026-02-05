using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000625 RID: 1573
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeChecksumPragma : CodeDirective
	{
		// Token: 0x0600396C RID: 14700 RVA: 0x000F2A50 File Offset: 0x000F0C50
		public CodeChecksumPragma()
		{
		}

		// Token: 0x0600396D RID: 14701 RVA: 0x000F2A58 File Offset: 0x000F0C58
		public CodeChecksumPragma(string fileName, Guid checksumAlgorithmId, byte[] checksumData)
		{
			this.fileName = fileName;
			this.checksumAlgorithmId = checksumAlgorithmId;
			this.checksumData = checksumData;
		}

		// Token: 0x17000DBC RID: 3516
		// (get) Token: 0x0600396E RID: 14702 RVA: 0x000F2A75 File Offset: 0x000F0C75
		// (set) Token: 0x0600396F RID: 14703 RVA: 0x000F2A8B File Offset: 0x000F0C8B
		public string FileName
		{
			get
			{
				if (this.fileName != null)
				{
					return this.fileName;
				}
				return string.Empty;
			}
			set
			{
				this.fileName = value;
			}
		}

		// Token: 0x17000DBD RID: 3517
		// (get) Token: 0x06003970 RID: 14704 RVA: 0x000F2A94 File Offset: 0x000F0C94
		// (set) Token: 0x06003971 RID: 14705 RVA: 0x000F2A9C File Offset: 0x000F0C9C
		public Guid ChecksumAlgorithmId
		{
			get
			{
				return this.checksumAlgorithmId;
			}
			set
			{
				this.checksumAlgorithmId = value;
			}
		}

		// Token: 0x17000DBE RID: 3518
		// (get) Token: 0x06003972 RID: 14706 RVA: 0x000F2AA5 File Offset: 0x000F0CA5
		// (set) Token: 0x06003973 RID: 14707 RVA: 0x000F2AAD File Offset: 0x000F0CAD
		public byte[] ChecksumData
		{
			get
			{
				return this.checksumData;
			}
			set
			{
				this.checksumData = value;
			}
		}

		// Token: 0x04002BA4 RID: 11172
		private string fileName;

		// Token: 0x04002BA5 RID: 11173
		private byte[] checksumData;

		// Token: 0x04002BA6 RID: 11174
		private Guid checksumAlgorithmId;
	}
}
