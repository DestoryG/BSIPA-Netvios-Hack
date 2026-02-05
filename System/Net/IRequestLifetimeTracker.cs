using System;

namespace System.Net
{
	// Token: 0x020001B7 RID: 439
	internal interface IRequestLifetimeTracker
	{
		// Token: 0x06001141 RID: 4417
		void TrackRequestLifetime(long requestStartTimestamp);
	}
}
