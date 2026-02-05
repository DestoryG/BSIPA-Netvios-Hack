using System;
using System.IO;

namespace System.Xml
{
	// Token: 0x0200001B RID: 27
	public interface IFragmentCapableXmlDictionaryWriter
	{
		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600008B RID: 139
		bool CanFragment { get; }

		// Token: 0x0600008C RID: 140
		void StartFragment(Stream stream, bool generateSelfContainedTextFragment);

		// Token: 0x0600008D RID: 141
		void EndFragment();

		// Token: 0x0600008E RID: 142
		void WriteFragment(byte[] buffer, int offset, int count);
	}
}
