using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Threading;

namespace System.Collections.Generic
{
	// Token: 0x020003C2 RID: 962
	[ComVisible(false)]
	[DebuggerTypeProxy(typeof(System_CollectionDebugView<>))]
	[DebuggerDisplay("Count = {Count}")]
	[global::__DynamicallyInvokable]
	[Serializable]
	public class LinkedList<T> : ICollection<T>, IEnumerable<T>, IEnumerable, ICollection, IReadOnlyCollection<T>, ISerializable, IDeserializationCallback
	{
		// Token: 0x06002419 RID: 9241 RVA: 0x000A9234 File Offset: 0x000A7434
		[global::__DynamicallyInvokable]
		public LinkedList()
		{
		}

		// Token: 0x0600241A RID: 9242 RVA: 0x000A923C File Offset: 0x000A743C
		[global::__DynamicallyInvokable]
		public LinkedList(IEnumerable<T> collection)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			foreach (T t in collection)
			{
				this.AddLast(t);
			}
		}

		// Token: 0x0600241B RID: 9243 RVA: 0x000A929C File Offset: 0x000A749C
		protected LinkedList(SerializationInfo info, StreamingContext context)
		{
			this.siInfo = info;
		}

		// Token: 0x1700091E RID: 2334
		// (get) Token: 0x0600241C RID: 9244 RVA: 0x000A92AB File Offset: 0x000A74AB
		[global::__DynamicallyInvokable]
		public int Count
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.count;
			}
		}

		// Token: 0x1700091F RID: 2335
		// (get) Token: 0x0600241D RID: 9245 RVA: 0x000A92B3 File Offset: 0x000A74B3
		[global::__DynamicallyInvokable]
		public LinkedListNode<T> First
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.head;
			}
		}

		// Token: 0x17000920 RID: 2336
		// (get) Token: 0x0600241E RID: 9246 RVA: 0x000A92BB File Offset: 0x000A74BB
		[global::__DynamicallyInvokable]
		public LinkedListNode<T> Last
		{
			[global::__DynamicallyInvokable]
			get
			{
				if (this.head != null)
				{
					return this.head.prev;
				}
				return null;
			}
		}

		// Token: 0x17000921 RID: 2337
		// (get) Token: 0x0600241F RID: 9247 RVA: 0x000A92D2 File Offset: 0x000A74D2
		[global::__DynamicallyInvokable]
		bool ICollection<T>.IsReadOnly
		{
			[global::__DynamicallyInvokable]
			get
			{
				return false;
			}
		}

		// Token: 0x06002420 RID: 9248 RVA: 0x000A92D5 File Offset: 0x000A74D5
		[global::__DynamicallyInvokable]
		void ICollection<T>.Add(T value)
		{
			this.AddLast(value);
		}

		// Token: 0x06002421 RID: 9249 RVA: 0x000A92E0 File Offset: 0x000A74E0
		[global::__DynamicallyInvokable]
		public LinkedListNode<T> AddAfter(LinkedListNode<T> node, T value)
		{
			this.ValidateNode(node);
			LinkedListNode<T> linkedListNode = new LinkedListNode<T>(node.list, value);
			this.InternalInsertNodeBefore(node.next, linkedListNode);
			return linkedListNode;
		}

		// Token: 0x06002422 RID: 9250 RVA: 0x000A930F File Offset: 0x000A750F
		[global::__DynamicallyInvokable]
		public void AddAfter(LinkedListNode<T> node, LinkedListNode<T> newNode)
		{
			this.ValidateNode(node);
			this.ValidateNewNode(newNode);
			this.InternalInsertNodeBefore(node.next, newNode);
			newNode.list = this;
		}

		// Token: 0x06002423 RID: 9251 RVA: 0x000A9334 File Offset: 0x000A7534
		[global::__DynamicallyInvokable]
		public LinkedListNode<T> AddBefore(LinkedListNode<T> node, T value)
		{
			this.ValidateNode(node);
			LinkedListNode<T> linkedListNode = new LinkedListNode<T>(node.list, value);
			this.InternalInsertNodeBefore(node, linkedListNode);
			if (node == this.head)
			{
				this.head = linkedListNode;
			}
			return linkedListNode;
		}

		// Token: 0x06002424 RID: 9252 RVA: 0x000A936E File Offset: 0x000A756E
		[global::__DynamicallyInvokable]
		public void AddBefore(LinkedListNode<T> node, LinkedListNode<T> newNode)
		{
			this.ValidateNode(node);
			this.ValidateNewNode(newNode);
			this.InternalInsertNodeBefore(node, newNode);
			newNode.list = this;
			if (node == this.head)
			{
				this.head = newNode;
			}
		}

		// Token: 0x06002425 RID: 9253 RVA: 0x000A93A0 File Offset: 0x000A75A0
		[global::__DynamicallyInvokable]
		public LinkedListNode<T> AddFirst(T value)
		{
			LinkedListNode<T> linkedListNode = new LinkedListNode<T>(this, value);
			if (this.head == null)
			{
				this.InternalInsertNodeToEmptyList(linkedListNode);
			}
			else
			{
				this.InternalInsertNodeBefore(this.head, linkedListNode);
				this.head = linkedListNode;
			}
			return linkedListNode;
		}

		// Token: 0x06002426 RID: 9254 RVA: 0x000A93DB File Offset: 0x000A75DB
		[global::__DynamicallyInvokable]
		public void AddFirst(LinkedListNode<T> node)
		{
			this.ValidateNewNode(node);
			if (this.head == null)
			{
				this.InternalInsertNodeToEmptyList(node);
			}
			else
			{
				this.InternalInsertNodeBefore(this.head, node);
				this.head = node;
			}
			node.list = this;
		}

		// Token: 0x06002427 RID: 9255 RVA: 0x000A9410 File Offset: 0x000A7610
		[global::__DynamicallyInvokable]
		public LinkedListNode<T> AddLast(T value)
		{
			LinkedListNode<T> linkedListNode = new LinkedListNode<T>(this, value);
			if (this.head == null)
			{
				this.InternalInsertNodeToEmptyList(linkedListNode);
			}
			else
			{
				this.InternalInsertNodeBefore(this.head, linkedListNode);
			}
			return linkedListNode;
		}

		// Token: 0x06002428 RID: 9256 RVA: 0x000A9444 File Offset: 0x000A7644
		[global::__DynamicallyInvokable]
		public void AddLast(LinkedListNode<T> node)
		{
			this.ValidateNewNode(node);
			if (this.head == null)
			{
				this.InternalInsertNodeToEmptyList(node);
			}
			else
			{
				this.InternalInsertNodeBefore(this.head, node);
			}
			node.list = this;
		}

		// Token: 0x06002429 RID: 9257 RVA: 0x000A9474 File Offset: 0x000A7674
		[global::__DynamicallyInvokable]
		public void Clear()
		{
			LinkedListNode<T> next = this.head;
			while (next != null)
			{
				LinkedListNode<T> linkedListNode = next;
				next = next.Next;
				linkedListNode.Invalidate();
			}
			this.head = null;
			this.count = 0;
			this.version++;
		}

		// Token: 0x0600242A RID: 9258 RVA: 0x000A94B8 File Offset: 0x000A76B8
		[global::__DynamicallyInvokable]
		public bool Contains(T value)
		{
			return this.Find(value) != null;
		}

		// Token: 0x0600242B RID: 9259 RVA: 0x000A94C4 File Offset: 0x000A76C4
		[global::__DynamicallyInvokable]
		public void CopyTo(T[] array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (index < 0 || index > array.Length)
			{
				throw new ArgumentOutOfRangeException("index", SR.GetString("IndexOutOfRange", new object[] { index }));
			}
			if (array.Length - index < this.Count)
			{
				throw new ArgumentException(SR.GetString("Arg_InsufficientSpace"));
			}
			LinkedListNode<T> next = this.head;
			if (next != null)
			{
				do
				{
					array[index++] = next.item;
					next = next.next;
				}
				while (next != this.head);
			}
		}

		// Token: 0x0600242C RID: 9260 RVA: 0x000A9558 File Offset: 0x000A7758
		[global::__DynamicallyInvokable]
		public LinkedListNode<T> Find(T value)
		{
			LinkedListNode<T> linkedListNode = this.head;
			EqualityComparer<T> @default = EqualityComparer<T>.Default;
			if (linkedListNode != null)
			{
				if (value != null)
				{
					while (!@default.Equals(linkedListNode.item, value))
					{
						linkedListNode = linkedListNode.next;
						if (linkedListNode == this.head)
						{
							goto IL_005A;
						}
					}
					return linkedListNode;
				}
				while (linkedListNode.item != null)
				{
					linkedListNode = linkedListNode.next;
					if (linkedListNode == this.head)
					{
						goto IL_005A;
					}
				}
				return linkedListNode;
			}
			IL_005A:
			return null;
		}

		// Token: 0x0600242D RID: 9261 RVA: 0x000A95C0 File Offset: 0x000A77C0
		[global::__DynamicallyInvokable]
		public LinkedListNode<T> FindLast(T value)
		{
			if (this.head == null)
			{
				return null;
			}
			LinkedListNode<T> prev = this.head.prev;
			LinkedListNode<T> linkedListNode = prev;
			EqualityComparer<T> @default = EqualityComparer<T>.Default;
			if (linkedListNode != null)
			{
				if (value != null)
				{
					while (!@default.Equals(linkedListNode.item, value))
					{
						linkedListNode = linkedListNode.prev;
						if (linkedListNode == prev)
						{
							goto IL_0061;
						}
					}
					return linkedListNode;
				}
				while (linkedListNode.item != null)
				{
					linkedListNode = linkedListNode.prev;
					if (linkedListNode == prev)
					{
						goto IL_0061;
					}
				}
				return linkedListNode;
			}
			IL_0061:
			return null;
		}

		// Token: 0x0600242E RID: 9262 RVA: 0x000A962F File Offset: 0x000A782F
		[global::__DynamicallyInvokable]
		public LinkedList<T>.Enumerator GetEnumerator()
		{
			return new LinkedList<T>.Enumerator(this);
		}

		// Token: 0x0600242F RID: 9263 RVA: 0x000A9637 File Offset: 0x000A7837
		[global::__DynamicallyInvokable]
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06002430 RID: 9264 RVA: 0x000A9644 File Offset: 0x000A7844
		[global::__DynamicallyInvokable]
		public bool Remove(T value)
		{
			LinkedListNode<T> linkedListNode = this.Find(value);
			if (linkedListNode != null)
			{
				this.InternalRemoveNode(linkedListNode);
				return true;
			}
			return false;
		}

		// Token: 0x06002431 RID: 9265 RVA: 0x000A9666 File Offset: 0x000A7866
		[global::__DynamicallyInvokable]
		public void Remove(LinkedListNode<T> node)
		{
			this.ValidateNode(node);
			this.InternalRemoveNode(node);
		}

		// Token: 0x06002432 RID: 9266 RVA: 0x000A9676 File Offset: 0x000A7876
		[global::__DynamicallyInvokable]
		public void RemoveFirst()
		{
			if (this.head == null)
			{
				throw new InvalidOperationException(SR.GetString("LinkedListEmpty"));
			}
			this.InternalRemoveNode(this.head);
		}

		// Token: 0x06002433 RID: 9267 RVA: 0x000A969C File Offset: 0x000A789C
		[global::__DynamicallyInvokable]
		public void RemoveLast()
		{
			if (this.head == null)
			{
				throw new InvalidOperationException(SR.GetString("LinkedListEmpty"));
			}
			this.InternalRemoveNode(this.head.prev);
		}

		// Token: 0x06002434 RID: 9268 RVA: 0x000A96C8 File Offset: 0x000A78C8
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			info.AddValue("Version", this.version);
			info.AddValue("Count", this.count);
			if (this.count != 0)
			{
				T[] array = new T[this.Count];
				this.CopyTo(array, 0);
				info.AddValue("Data", array, typeof(T[]));
			}
		}

		// Token: 0x06002435 RID: 9269 RVA: 0x000A9738 File Offset: 0x000A7938
		public virtual void OnDeserialization(object sender)
		{
			if (this.siInfo == null)
			{
				return;
			}
			int @int = this.siInfo.GetInt32("Version");
			int int2 = this.siInfo.GetInt32("Count");
			if (int2 != 0)
			{
				T[] array = (T[])this.siInfo.GetValue("Data", typeof(T[]));
				if (array == null)
				{
					throw new SerializationException(SR.GetString("Serialization_MissingValues"));
				}
				for (int i = 0; i < array.Length; i++)
				{
					this.AddLast(array[i]);
				}
			}
			else
			{
				this.head = null;
			}
			this.version = @int;
			this.siInfo = null;
		}

		// Token: 0x06002436 RID: 9270 RVA: 0x000A97DC File Offset: 0x000A79DC
		private void InternalInsertNodeBefore(LinkedListNode<T> node, LinkedListNode<T> newNode)
		{
			newNode.next = node;
			newNode.prev = node.prev;
			node.prev.next = newNode;
			node.prev = newNode;
			this.version++;
			this.count++;
		}

		// Token: 0x06002437 RID: 9271 RVA: 0x000A982B File Offset: 0x000A7A2B
		private void InternalInsertNodeToEmptyList(LinkedListNode<T> newNode)
		{
			newNode.next = newNode;
			newNode.prev = newNode;
			this.head = newNode;
			this.version++;
			this.count++;
		}

		// Token: 0x06002438 RID: 9272 RVA: 0x000A9860 File Offset: 0x000A7A60
		internal void InternalRemoveNode(LinkedListNode<T> node)
		{
			if (node.next == node)
			{
				this.head = null;
			}
			else
			{
				node.next.prev = node.prev;
				node.prev.next = node.next;
				if (this.head == node)
				{
					this.head = node.next;
				}
			}
			node.Invalidate();
			this.count--;
			this.version++;
		}

		// Token: 0x06002439 RID: 9273 RVA: 0x000A98D8 File Offset: 0x000A7AD8
		internal void ValidateNewNode(LinkedListNode<T> node)
		{
			if (node == null)
			{
				throw new ArgumentNullException("node");
			}
			if (node.list != null)
			{
				throw new InvalidOperationException(SR.GetString("LinkedListNodeIsAttached"));
			}
		}

		// Token: 0x0600243A RID: 9274 RVA: 0x000A9900 File Offset: 0x000A7B00
		internal void ValidateNode(LinkedListNode<T> node)
		{
			if (node == null)
			{
				throw new ArgumentNullException("node");
			}
			if (node.list != this)
			{
				throw new InvalidOperationException(SR.GetString("ExternalLinkedListNode"));
			}
		}

		// Token: 0x17000922 RID: 2338
		// (get) Token: 0x0600243B RID: 9275 RVA: 0x000A9929 File Offset: 0x000A7B29
		[global::__DynamicallyInvokable]
		bool ICollection.IsSynchronized
		{
			[global::__DynamicallyInvokable]
			get
			{
				return false;
			}
		}

		// Token: 0x17000923 RID: 2339
		// (get) Token: 0x0600243C RID: 9276 RVA: 0x000A992C File Offset: 0x000A7B2C
		[global::__DynamicallyInvokable]
		object ICollection.SyncRoot
		{
			[global::__DynamicallyInvokable]
			get
			{
				if (this._syncRoot == null)
				{
					Interlocked.CompareExchange<object>(ref this._syncRoot, new object(), null);
				}
				return this._syncRoot;
			}
		}

		// Token: 0x0600243D RID: 9277 RVA: 0x000A9950 File Offset: 0x000A7B50
		[global::__DynamicallyInvokable]
		void ICollection.CopyTo(Array array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (array.Rank != 1)
			{
				throw new ArgumentException(SR.GetString("Arg_MultiRank"));
			}
			if (array.GetLowerBound(0) != 0)
			{
				throw new ArgumentException(SR.GetString("Arg_NonZeroLowerBound"));
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", SR.GetString("IndexOutOfRange", new object[] { index }));
			}
			if (array.Length - index < this.Count)
			{
				throw new ArgumentException(SR.GetString("Arg_InsufficientSpace"));
			}
			T[] array2 = array as T[];
			if (array2 != null)
			{
				this.CopyTo(array2, index);
				return;
			}
			Type elementType = array.GetType().GetElementType();
			Type typeFromHandle = typeof(T);
			if (!elementType.IsAssignableFrom(typeFromHandle) && !typeFromHandle.IsAssignableFrom(elementType))
			{
				throw new ArgumentException(SR.GetString("Invalid_Array_Type"));
			}
			object[] array3 = array as object[];
			if (array3 == null)
			{
				throw new ArgumentException(SR.GetString("Invalid_Array_Type"));
			}
			LinkedListNode<T> next = this.head;
			try
			{
				if (next != null)
				{
					do
					{
						array3[index++] = next.item;
						next = next.next;
					}
					while (next != this.head);
				}
			}
			catch (ArrayTypeMismatchException)
			{
				throw new ArgumentException(SR.GetString("Invalid_Array_Type"));
			}
		}

		// Token: 0x0600243E RID: 9278 RVA: 0x000A9AA4 File Offset: 0x000A7CA4
		[global::__DynamicallyInvokable]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x04001FF8 RID: 8184
		internal LinkedListNode<T> head;

		// Token: 0x04001FF9 RID: 8185
		internal int count;

		// Token: 0x04001FFA RID: 8186
		internal int version;

		// Token: 0x04001FFB RID: 8187
		private object _syncRoot;

		// Token: 0x04001FFC RID: 8188
		private SerializationInfo siInfo;

		// Token: 0x04001FFD RID: 8189
		private const string VersionName = "Version";

		// Token: 0x04001FFE RID: 8190
		private const string CountName = "Count";

		// Token: 0x04001FFF RID: 8191
		private const string ValuesName = "Data";

		// Token: 0x020007F2 RID: 2034
		[global::__DynamicallyInvokable]
		[Serializable]
		public struct Enumerator : IEnumerator<T>, IDisposable, IEnumerator, ISerializable, IDeserializationCallback
		{
			// Token: 0x0600441F RID: 17439 RVA: 0x0011E223 File Offset: 0x0011C423
			internal Enumerator(LinkedList<T> list)
			{
				this.list = list;
				this.version = list.version;
				this.node = list.head;
				this.current = default(T);
				this.index = 0;
				this.siInfo = null;
			}

			// Token: 0x06004420 RID: 17440 RVA: 0x0011E25E File Offset: 0x0011C45E
			internal Enumerator(SerializationInfo info, StreamingContext context)
			{
				this.siInfo = info;
				this.list = null;
				this.version = 0;
				this.node = null;
				this.current = default(T);
				this.index = 0;
			}

			// Token: 0x17000F71 RID: 3953
			// (get) Token: 0x06004421 RID: 17441 RVA: 0x0011E28F File Offset: 0x0011C48F
			[global::__DynamicallyInvokable]
			public T Current
			{
				[global::__DynamicallyInvokable]
				get
				{
					return this.current;
				}
			}

			// Token: 0x17000F72 RID: 3954
			// (get) Token: 0x06004422 RID: 17442 RVA: 0x0011E297 File Offset: 0x0011C497
			[global::__DynamicallyInvokable]
			object IEnumerator.Current
			{
				[global::__DynamicallyInvokable]
				get
				{
					if (this.index == 0 || this.index == this.list.Count + 1)
					{
						global::System.ThrowHelper.ThrowInvalidOperationException(global::System.ExceptionResource.InvalidOperation_EnumOpCantHappen);
					}
					return this.current;
				}
			}

			// Token: 0x06004423 RID: 17443 RVA: 0x0011E2C8 File Offset: 0x0011C4C8
			[global::__DynamicallyInvokable]
			public bool MoveNext()
			{
				if (this.version != this.list.version)
				{
					throw new InvalidOperationException(SR.GetString("InvalidOperation_EnumFailedVersion"));
				}
				if (this.node == null)
				{
					this.index = this.list.Count + 1;
					return false;
				}
				this.index++;
				this.current = this.node.item;
				this.node = this.node.next;
				if (this.node == this.list.head)
				{
					this.node = null;
				}
				return true;
			}

			// Token: 0x06004424 RID: 17444 RVA: 0x0011E360 File Offset: 0x0011C560
			[global::__DynamicallyInvokable]
			void IEnumerator.Reset()
			{
				if (this.version != this.list.version)
				{
					throw new InvalidOperationException(SR.GetString("InvalidOperation_EnumFailedVersion"));
				}
				this.current = default(T);
				this.node = this.list.head;
				this.index = 0;
			}

			// Token: 0x06004425 RID: 17445 RVA: 0x0011E3B4 File Offset: 0x0011C5B4
			[global::__DynamicallyInvokable]
			public void Dispose()
			{
			}

			// Token: 0x06004426 RID: 17446 RVA: 0x0011E3B8 File Offset: 0x0011C5B8
			void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
			{
				if (info == null)
				{
					throw new ArgumentNullException("info");
				}
				info.AddValue("LinkedList", this.list);
				info.AddValue("Version", this.version);
				info.AddValue("Current", this.current);
				info.AddValue("Index", this.index);
			}

			// Token: 0x06004427 RID: 17447 RVA: 0x0011E41C File Offset: 0x0011C61C
			void IDeserializationCallback.OnDeserialization(object sender)
			{
				if (this.list != null)
				{
					return;
				}
				if (this.siInfo == null)
				{
					throw new SerializationException(SR.GetString("Serialization_InvalidOnDeser"));
				}
				this.list = (LinkedList<T>)this.siInfo.GetValue("LinkedList", typeof(LinkedList<T>));
				this.version = this.siInfo.GetInt32("Version");
				this.current = (T)((object)this.siInfo.GetValue("Current", typeof(T)));
				this.index = this.siInfo.GetInt32("Index");
				if (this.list.siInfo != null)
				{
					this.list.OnDeserialization(sender);
				}
				if (this.index == this.list.Count + 1)
				{
					this.node = null;
				}
				else
				{
					this.node = this.list.First;
					if (this.node != null && this.index != 0)
					{
						for (int i = 0; i < this.index; i++)
						{
							this.node = this.node.next;
						}
						if (this.node == this.list.First)
						{
							this.node = null;
						}
					}
				}
				this.siInfo = null;
			}

			// Token: 0x04003507 RID: 13575
			private LinkedList<T> list;

			// Token: 0x04003508 RID: 13576
			private LinkedListNode<T> node;

			// Token: 0x04003509 RID: 13577
			private int version;

			// Token: 0x0400350A RID: 13578
			private T current;

			// Token: 0x0400350B RID: 13579
			private int index;

			// Token: 0x0400350C RID: 13580
			private SerializationInfo siInfo;

			// Token: 0x0400350D RID: 13581
			private const string LinkedListName = "LinkedList";

			// Token: 0x0400350E RID: 13582
			private const string CurrentValueName = "Current";

			// Token: 0x0400350F RID: 13583
			private const string VersionName = "Version";

			// Token: 0x04003510 RID: 13584
			private const string IndexName = "Index";
		}
	}
}
