using System;
using System.Runtime.InteropServices;

namespace System.Collections.Generic
{
	// Token: 0x020003C3 RID: 963
	[ComVisible(false)]
	[global::__DynamicallyInvokable]
	public sealed class LinkedListNode<T>
	{
		// Token: 0x0600243F RID: 9279 RVA: 0x000A9AB1 File Offset: 0x000A7CB1
		[global::__DynamicallyInvokable]
		public LinkedListNode(T value)
		{
			this.item = value;
		}

		// Token: 0x06002440 RID: 9280 RVA: 0x000A9AC0 File Offset: 0x000A7CC0
		internal LinkedListNode(LinkedList<T> list, T value)
		{
			this.list = list;
			this.item = value;
		}

		// Token: 0x17000924 RID: 2340
		// (get) Token: 0x06002441 RID: 9281 RVA: 0x000A9AD6 File Offset: 0x000A7CD6
		[global::__DynamicallyInvokable]
		public LinkedList<T> List
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.list;
			}
		}

		// Token: 0x17000925 RID: 2341
		// (get) Token: 0x06002442 RID: 9282 RVA: 0x000A9ADE File Offset: 0x000A7CDE
		[global::__DynamicallyInvokable]
		public LinkedListNode<T> Next
		{
			[global::__DynamicallyInvokable]
			get
			{
				if (this.next != null && this.next != this.list.head)
				{
					return this.next;
				}
				return null;
			}
		}

		// Token: 0x17000926 RID: 2342
		// (get) Token: 0x06002443 RID: 9283 RVA: 0x000A9B03 File Offset: 0x000A7D03
		[global::__DynamicallyInvokable]
		public LinkedListNode<T> Previous
		{
			[global::__DynamicallyInvokable]
			get
			{
				if (this.prev != null && this != this.list.head)
				{
					return this.prev;
				}
				return null;
			}
		}

		// Token: 0x17000927 RID: 2343
		// (get) Token: 0x06002444 RID: 9284 RVA: 0x000A9B23 File Offset: 0x000A7D23
		// (set) Token: 0x06002445 RID: 9285 RVA: 0x000A9B2B File Offset: 0x000A7D2B
		[global::__DynamicallyInvokable]
		public T Value
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.item;
			}
			[global::__DynamicallyInvokable]
			set
			{
				this.item = value;
			}
		}

		// Token: 0x06002446 RID: 9286 RVA: 0x000A9B34 File Offset: 0x000A7D34
		internal void Invalidate()
		{
			this.list = null;
			this.next = null;
			this.prev = null;
		}

		// Token: 0x04002000 RID: 8192
		internal LinkedList<T> list;

		// Token: 0x04002001 RID: 8193
		internal LinkedListNode<T> next;

		// Token: 0x04002002 RID: 8194
		internal LinkedListNode<T> prev;

		// Token: 0x04002003 RID: 8195
		internal T item;
	}
}
