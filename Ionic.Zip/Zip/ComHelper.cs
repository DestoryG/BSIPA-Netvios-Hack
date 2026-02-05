using System;
using System.Runtime.InteropServices;

namespace Ionic.Zip
{
	// Token: 0x02000008 RID: 8
	[ComVisible(true)]
	[Guid("ebc25cf6-9120-4283-b972-0e5520d0000F")]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	public class ComHelper
	{
		// Token: 0x06000081 RID: 129 RVA: 0x00003554 File Offset: 0x00001754
		public bool IsZipFile(string filename)
		{
			return ZipFile.IsZipFile(filename);
		}

		// Token: 0x06000082 RID: 130 RVA: 0x0000355C File Offset: 0x0000175C
		public bool IsZipFileWithExtract(string filename)
		{
			return ZipFile.IsZipFile(filename, true);
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00003565 File Offset: 0x00001765
		public bool CheckZip(string filename)
		{
			return ZipFile.CheckZip(filename);
		}

		// Token: 0x06000084 RID: 132 RVA: 0x0000356D File Offset: 0x0000176D
		public bool CheckZipPassword(string filename, string password)
		{
			return ZipFile.CheckZipPassword(filename, password);
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00003576 File Offset: 0x00001776
		public void FixZipDirectory(string filename)
		{
			ZipFile.FixZipDirectory(filename);
		}

		// Token: 0x06000086 RID: 134 RVA: 0x0000357E File Offset: 0x0000177E
		public string GetZipLibraryVersion()
		{
			return ZipFile.LibraryVersion.ToString();
		}
	}
}
