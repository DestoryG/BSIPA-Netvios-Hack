using System;

namespace System.Text.RegularExpressions
{
	// Token: 0x0200068C RID: 1676
	[global::__DynamicallyInvokable]
	[Serializable]
	public class Capture
	{
		// Token: 0x06003DE6 RID: 15846 RVA: 0x000FDAF4 File Offset: 0x000FBCF4
		internal Capture(string text, int i, int l)
		{
			this._text = text;
			this._index = i;
			this._length = l;
		}

		// Token: 0x17000EAA RID: 3754
		// (get) Token: 0x06003DE7 RID: 15847 RVA: 0x000FDB11 File Offset: 0x000FBD11
		[global::__DynamicallyInvokable]
		public int Index
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this._index;
			}
		}

		// Token: 0x17000EAB RID: 3755
		// (get) Token: 0x06003DE8 RID: 15848 RVA: 0x000FDB19 File Offset: 0x000FBD19
		[global::__DynamicallyInvokable]
		public int Length
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this._length;
			}
		}

		// Token: 0x17000EAC RID: 3756
		// (get) Token: 0x06003DE9 RID: 15849 RVA: 0x000FDB21 File Offset: 0x000FBD21
		[global::__DynamicallyInvokable]
		public string Value
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this._text.Substring(this._index, this._length);
			}
		}

		// Token: 0x06003DEA RID: 15850 RVA: 0x000FDB3A File Offset: 0x000FBD3A
		[global::__DynamicallyInvokable]
		public override string ToString()
		{
			return this.Value;
		}

		// Token: 0x06003DEB RID: 15851 RVA: 0x000FDB42 File Offset: 0x000FBD42
		internal string GetOriginalString()
		{
			return this._text;
		}

		// Token: 0x06003DEC RID: 15852 RVA: 0x000FDB4A File Offset: 0x000FBD4A
		internal string GetLeftSubstring()
		{
			return this._text.Substring(0, this._index);
		}

		// Token: 0x06003DED RID: 15853 RVA: 0x000FDB5E File Offset: 0x000FBD5E
		internal string GetRightSubstring()
		{
			return this._text.Substring(this._index + this._length, this._text.Length - this._index - this._length);
		}

		// Token: 0x04002CF3 RID: 11507
		internal string _text;

		// Token: 0x04002CF4 RID: 11508
		internal int _index;

		// Token: 0x04002CF5 RID: 11509
		internal int _length;
	}
}
