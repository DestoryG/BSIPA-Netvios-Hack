using System;

namespace System.Net.WebSockets
{
	// Token: 0x0200023B RID: 571
	public class WebSocketReceiveResult
	{
		// Token: 0x060015A5 RID: 5541 RVA: 0x000706E0 File Offset: 0x0006E8E0
		public WebSocketReceiveResult(int count, WebSocketMessageType messageType, bool endOfMessage)
			: this(count, messageType, endOfMessage, null, null)
		{
		}

		// Token: 0x060015A6 RID: 5542 RVA: 0x00070700 File Offset: 0x0006E900
		public WebSocketReceiveResult(int count, WebSocketMessageType messageType, bool endOfMessage, WebSocketCloseStatus? closeStatus, string closeStatusDescription)
		{
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			this.Count = count;
			this.EndOfMessage = endOfMessage;
			this.MessageType = messageType;
			this.CloseStatus = closeStatus;
			this.CloseStatusDescription = closeStatusDescription;
		}

		// Token: 0x1700048D RID: 1165
		// (get) Token: 0x060015A7 RID: 5543 RVA: 0x0007073C File Offset: 0x0006E93C
		// (set) Token: 0x060015A8 RID: 5544 RVA: 0x00070744 File Offset: 0x0006E944
		public int Count { get; private set; }

		// Token: 0x1700048E RID: 1166
		// (get) Token: 0x060015A9 RID: 5545 RVA: 0x0007074D File Offset: 0x0006E94D
		// (set) Token: 0x060015AA RID: 5546 RVA: 0x00070755 File Offset: 0x0006E955
		public bool EndOfMessage { get; private set; }

		// Token: 0x1700048F RID: 1167
		// (get) Token: 0x060015AB RID: 5547 RVA: 0x0007075E File Offset: 0x0006E95E
		// (set) Token: 0x060015AC RID: 5548 RVA: 0x00070766 File Offset: 0x0006E966
		public WebSocketMessageType MessageType { get; private set; }

		// Token: 0x17000490 RID: 1168
		// (get) Token: 0x060015AD RID: 5549 RVA: 0x0007076F File Offset: 0x0006E96F
		// (set) Token: 0x060015AE RID: 5550 RVA: 0x00070777 File Offset: 0x0006E977
		public WebSocketCloseStatus? CloseStatus { get; private set; }

		// Token: 0x17000491 RID: 1169
		// (get) Token: 0x060015AF RID: 5551 RVA: 0x00070780 File Offset: 0x0006E980
		// (set) Token: 0x060015B0 RID: 5552 RVA: 0x00070788 File Offset: 0x0006E988
		public string CloseStatusDescription { get; private set; }

		// Token: 0x060015B1 RID: 5553 RVA: 0x00070791 File Offset: 0x0006E991
		internal WebSocketReceiveResult Copy(int count)
		{
			this.Count -= count;
			return new WebSocketReceiveResult(count, this.MessageType, this.Count == 0 && this.EndOfMessage, this.CloseStatus, this.CloseStatusDescription);
		}
	}
}
