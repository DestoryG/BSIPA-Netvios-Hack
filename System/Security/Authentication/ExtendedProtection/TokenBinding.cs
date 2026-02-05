using System;

namespace System.Security.Authentication.ExtendedProtection
{
	// Token: 0x02000448 RID: 1096
	public class TokenBinding
	{
		// Token: 0x06002898 RID: 10392 RVA: 0x000BA5EC File Offset: 0x000B87EC
		internal TokenBinding(TokenBindingType bindingType, byte[] rawData)
		{
			this.BindingType = bindingType;
			this._rawTokenBindingId = rawData;
		}

		// Token: 0x06002899 RID: 10393 RVA: 0x000BA602 File Offset: 0x000B8802
		public byte[] GetRawTokenBindingId()
		{
			if (this._rawTokenBindingId == null)
			{
				return null;
			}
			return (byte[])this._rawTokenBindingId.Clone();
		}

		// Token: 0x170009F7 RID: 2551
		// (get) Token: 0x0600289A RID: 10394 RVA: 0x000BA61E File Offset: 0x000B881E
		// (set) Token: 0x0600289B RID: 10395 RVA: 0x000BA626 File Offset: 0x000B8826
		public TokenBindingType BindingType { get; private set; }

		// Token: 0x04002266 RID: 8806
		private byte[] _rawTokenBindingId;
	}
}
