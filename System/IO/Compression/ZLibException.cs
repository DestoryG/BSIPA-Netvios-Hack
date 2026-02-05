using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.IO.Compression
{
	// Token: 0x02000425 RID: 1061
	[Serializable]
	internal class ZLibException : IOException, ISerializable
	{
		// Token: 0x060027A6 RID: 10150 RVA: 0x000B6699 File Offset: 0x000B4899
		public ZLibException(string message, string zlibErrorContext, int zlibErrorCode, string zlibErrorMessage)
			: base(message)
		{
			this.Init(zlibErrorContext, (ZLibNative.ErrorCode)zlibErrorCode, zlibErrorMessage);
		}

		// Token: 0x060027A7 RID: 10151 RVA: 0x000B66AC File Offset: 0x000B48AC
		public ZLibException()
		{
			this.Init();
		}

		// Token: 0x060027A8 RID: 10152 RVA: 0x000B66BA File Offset: 0x000B48BA
		public ZLibException(string message)
			: base(message)
		{
			this.Init();
		}

		// Token: 0x060027A9 RID: 10153 RVA: 0x000B66C9 File Offset: 0x000B48C9
		public ZLibException(string message, Exception inner)
			: base(message, inner)
		{
			this.Init();
		}

		// Token: 0x060027AA RID: 10154 RVA: 0x000B66DC File Offset: 0x000B48DC
		[SecurityPermission(SecurityAction.LinkDemand, SerializationFormatter = true)]
		protected ZLibException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			string @string = info.GetString("zlibErrorContext");
			ZLibNative.ErrorCode @int = (ZLibNative.ErrorCode)info.GetInt32("zlibErrorCode");
			string string2 = info.GetString("zlibErrorMessage");
			this.Init(@string, @int, string2);
		}

		// Token: 0x060027AB RID: 10155 RVA: 0x000B671E File Offset: 0x000B491E
		[SecurityPermission(SecurityAction.LinkDemand, SerializationFormatter = true)]
		void ISerializable.GetObjectData(SerializationInfo si, StreamingContext context)
		{
			base.GetObjectData(si, context);
			si.AddValue("zlibErrorContext", this.zlibErrorContext);
			si.AddValue("zlibErrorCode", (int)this.zlibErrorCode);
			si.AddValue("zlibErrorMessage", this.zlibErrorMessage);
		}

		// Token: 0x060027AC RID: 10156 RVA: 0x000B675B File Offset: 0x000B495B
		private void Init()
		{
			this.Init("", ZLibNative.ErrorCode.Ok, "");
		}

		// Token: 0x060027AD RID: 10157 RVA: 0x000B676E File Offset: 0x000B496E
		private void Init(string zlibErrorContext, ZLibNative.ErrorCode zlibErrorCode, string zlibErrorMessage)
		{
			this.zlibErrorContext = zlibErrorContext;
			this.zlibErrorCode = zlibErrorCode;
			this.zlibErrorMessage = zlibErrorMessage;
		}

		// Token: 0x170009C9 RID: 2505
		// (get) Token: 0x060027AE RID: 10158 RVA: 0x000B6785 File Offset: 0x000B4985
		public string ZLibContext
		{
			[PermissionSet(SecurityAction.LinkDemand, Unrestricted = true)]
			get
			{
				return this.zlibErrorContext;
			}
		}

		// Token: 0x170009CA RID: 2506
		// (get) Token: 0x060027AF RID: 10159 RVA: 0x000B678D File Offset: 0x000B498D
		public int ZLibErrorCode
		{
			[PermissionSet(SecurityAction.LinkDemand, Unrestricted = true)]
			get
			{
				return (int)this.zlibErrorCode;
			}
		}

		// Token: 0x170009CB RID: 2507
		// (get) Token: 0x060027B0 RID: 10160 RVA: 0x000B6795 File Offset: 0x000B4995
		public string ZLibErrorMessage
		{
			[PermissionSet(SecurityAction.LinkDemand, Unrestricted = true)]
			get
			{
				return this.zlibErrorMessage;
			}
		}

		// Token: 0x04002183 RID: 8579
		private string zlibErrorContext;

		// Token: 0x04002184 RID: 8580
		private string zlibErrorMessage;

		// Token: 0x04002185 RID: 8581
		private ZLibNative.ErrorCode zlibErrorCode;
	}
}
