using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Security;
using System.Threading;

namespace System.Collections.Generic
{
	// Token: 0x020003CB RID: 971
	[DebuggerTypeProxy(typeof(SortedSetDebugView<>))]
	[DebuggerDisplay("Count = {Count}")]
	[global::__DynamicallyInvokable]
	[Serializable]
	public class SortedSet<T> : ISet<T>, ICollection<T>, IEnumerable<T>, IEnumerable, ICollection, ISerializable, IDeserializationCallback, IReadOnlyCollection<T>
	{
		// Token: 0x060024DC RID: 9436 RVA: 0x000AB713 File Offset: 0x000A9913
		[global::__DynamicallyInvokable]
		public SortedSet()
		{
			this.comparer = Comparer<T>.Default;
		}

		// Token: 0x060024DD RID: 9437 RVA: 0x000AB726 File Offset: 0x000A9926
		[global::__DynamicallyInvokable]
		public SortedSet(IComparer<T> comparer)
		{
			if (comparer == null)
			{
				this.comparer = Comparer<T>.Default;
				return;
			}
			this.comparer = comparer;
		}

		// Token: 0x060024DE RID: 9438 RVA: 0x000AB744 File Offset: 0x000A9944
		[global::__DynamicallyInvokable]
		public SortedSet(IEnumerable<T> collection)
			: this(collection, Comparer<T>.Default)
		{
		}

		// Token: 0x060024DF RID: 9439 RVA: 0x000AB754 File Offset: 0x000A9954
		[global::__DynamicallyInvokable]
		public SortedSet(IEnumerable<T> collection, IComparer<T> comparer)
			: this(comparer)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			SortedSet<T> sortedSet = collection as SortedSet<T>;
			SortedSet<T> sortedSet2 = collection as SortedSet<T>.TreeSubSet;
			if (sortedSet == null || sortedSet2 != null || !SortedSet<T>.AreComparersEqual(this, sortedSet))
			{
				List<T> list = new List<T>(collection);
				list.Sort(this.comparer);
				for (int i = 1; i < list.Count; i++)
				{
					if (comparer.Compare(list[i], list[i - 1]) == 0)
					{
						list.RemoveAt(i);
						i--;
					}
				}
				this.root = SortedSet<T>.ConstructRootFromSortedArray(list.ToArray(), 0, list.Count - 1, null);
				this.count = list.Count;
				this.version = 0;
				return;
			}
			if (sortedSet.Count == 0)
			{
				this.count = 0;
				this.version = 0;
				this.root = null;
				return;
			}
			Stack<SortedSet<T>.Node> stack = new Stack<SortedSet<T>.Node>(2 * SortedSet<T>.log2(sortedSet.Count) + 2);
			Stack<SortedSet<T>.Node> stack2 = new Stack<SortedSet<T>.Node>(2 * SortedSet<T>.log2(sortedSet.Count) + 2);
			SortedSet<T>.Node node = sortedSet.root;
			SortedSet<T>.Node node2 = ((node != null) ? new SortedSet<T>.Node(node.Item, node.IsRed) : null);
			this.root = node2;
			while (node != null)
			{
				stack.Push(node);
				stack2.Push(node2);
				node2.Left = ((node.Left != null) ? new SortedSet<T>.Node(node.Left.Item, node.Left.IsRed) : null);
				node = node.Left;
				node2 = node2.Left;
			}
			while (stack.Count != 0)
			{
				node = stack.Pop();
				node2 = stack2.Pop();
				SortedSet<T>.Node node3 = node.Right;
				SortedSet<T>.Node node4 = null;
				if (node3 != null)
				{
					node4 = new SortedSet<T>.Node(node3.Item, node3.IsRed);
				}
				node2.Right = node4;
				while (node3 != null)
				{
					stack.Push(node3);
					stack2.Push(node4);
					node4.Left = ((node3.Left != null) ? new SortedSet<T>.Node(node3.Left.Item, node3.Left.IsRed) : null);
					node3 = node3.Left;
					node4 = node4.Left;
				}
			}
			this.count = sortedSet.count;
			this.version = 0;
		}

		// Token: 0x060024E0 RID: 9440 RVA: 0x000AB9A9 File Offset: 0x000A9BA9
		protected SortedSet(SerializationInfo info, StreamingContext context)
		{
			this.siInfo = info;
		}

		// Token: 0x060024E1 RID: 9441 RVA: 0x000AB9B8 File Offset: 0x000A9BB8
		private void AddAllElements(IEnumerable<T> collection)
		{
			foreach (T t in collection)
			{
				if (!this.Contains(t))
				{
					this.Add(t);
				}
			}
		}

		// Token: 0x060024E2 RID: 9442 RVA: 0x000ABA0C File Offset: 0x000A9C0C
		private void RemoveAllElements(IEnumerable<T> collection)
		{
			T min = this.Min;
			T max = this.Max;
			foreach (T t in collection)
			{
				if (this.comparer.Compare(t, min) >= 0 && this.comparer.Compare(t, max) <= 0 && this.Contains(t))
				{
					this.Remove(t);
				}
			}
		}

		// Token: 0x060024E3 RID: 9443 RVA: 0x000ABA8C File Offset: 0x000A9C8C
		private bool ContainsAllElements(IEnumerable<T> collection)
		{
			foreach (T t in collection)
			{
				if (!this.Contains(t))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060024E4 RID: 9444 RVA: 0x000ABAE0 File Offset: 0x000A9CE0
		internal bool InOrderTreeWalk(TreeWalkPredicate<T> action)
		{
			return this.InOrderTreeWalk(action, false);
		}

		// Token: 0x060024E5 RID: 9445 RVA: 0x000ABAEC File Offset: 0x000A9CEC
		internal virtual bool InOrderTreeWalk(TreeWalkPredicate<T> action, bool reverse)
		{
			if (this.root == null)
			{
				return true;
			}
			Stack<SortedSet<T>.Node> stack = new Stack<SortedSet<T>.Node>(2 * SortedSet<T>.log2(this.Count + 1));
			for (SortedSet<T>.Node node = this.root; node != null; node = (reverse ? node.Right : node.Left))
			{
				stack.Push(node);
			}
			while (stack.Count != 0)
			{
				SortedSet<T>.Node node = stack.Pop();
				if (!action(node))
				{
					return false;
				}
				for (SortedSet<T>.Node node2 = (reverse ? node.Left : node.Right); node2 != null; node2 = (reverse ? node2.Right : node2.Left))
				{
					stack.Push(node2);
				}
			}
			return true;
		}

		// Token: 0x060024E6 RID: 9446 RVA: 0x000ABB8C File Offset: 0x000A9D8C
		internal virtual bool BreadthFirstTreeWalk(TreeWalkPredicate<T> action)
		{
			if (this.root == null)
			{
				return true;
			}
			List<SortedSet<T>.Node> list = new List<SortedSet<T>.Node>();
			list.Add(this.root);
			while (list.Count != 0)
			{
				SortedSet<T>.Node node = list[0];
				list.RemoveAt(0);
				if (!action(node))
				{
					return false;
				}
				if (node.Left != null)
				{
					list.Add(node.Left);
				}
				if (node.Right != null)
				{
					list.Add(node.Right);
				}
			}
			return true;
		}

		// Token: 0x17000951 RID: 2385
		// (get) Token: 0x060024E7 RID: 9447 RVA: 0x000ABC02 File Offset: 0x000A9E02
		[global::__DynamicallyInvokable]
		public int Count
		{
			[global::__DynamicallyInvokable]
			get
			{
				this.VersionCheck();
				return this.count;
			}
		}

		// Token: 0x17000952 RID: 2386
		// (get) Token: 0x060024E8 RID: 9448 RVA: 0x000ABC10 File Offset: 0x000A9E10
		[global::__DynamicallyInvokable]
		public IComparer<T> Comparer
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.comparer;
			}
		}

		// Token: 0x17000953 RID: 2387
		// (get) Token: 0x060024E9 RID: 9449 RVA: 0x000ABC18 File Offset: 0x000A9E18
		[global::__DynamicallyInvokable]
		bool ICollection<T>.IsReadOnly
		{
			[global::__DynamicallyInvokable]
			get
			{
				return false;
			}
		}

		// Token: 0x17000954 RID: 2388
		// (get) Token: 0x060024EA RID: 9450 RVA: 0x000ABC1B File Offset: 0x000A9E1B
		[global::__DynamicallyInvokable]
		bool ICollection.IsSynchronized
		{
			[global::__DynamicallyInvokable]
			get
			{
				return false;
			}
		}

		// Token: 0x17000955 RID: 2389
		// (get) Token: 0x060024EB RID: 9451 RVA: 0x000ABC1E File Offset: 0x000A9E1E
		[global::__DynamicallyInvokable]
		object ICollection.SyncRoot
		{
			[global::__DynamicallyInvokable]
			get
			{
				if (this._syncRoot == null)
				{
					Interlocked.CompareExchange(ref this._syncRoot, new object(), null);
				}
				return this._syncRoot;
			}
		}

		// Token: 0x060024EC RID: 9452 RVA: 0x000ABC40 File Offset: 0x000A9E40
		internal virtual void VersionCheck()
		{
		}

		// Token: 0x060024ED RID: 9453 RVA: 0x000ABC42 File Offset: 0x000A9E42
		internal virtual bool IsWithinRange(T item)
		{
			return true;
		}

		// Token: 0x060024EE RID: 9454 RVA: 0x000ABC45 File Offset: 0x000A9E45
		[global::__DynamicallyInvokable]
		public bool Add(T item)
		{
			return this.AddIfNotPresent(item);
		}

		// Token: 0x060024EF RID: 9455 RVA: 0x000ABC4E File Offset: 0x000A9E4E
		[global::__DynamicallyInvokable]
		void ICollection<T>.Add(T item)
		{
			this.AddIfNotPresent(item);
		}

		// Token: 0x060024F0 RID: 9456 RVA: 0x000ABC58 File Offset: 0x000A9E58
		internal virtual bool AddIfNotPresent(T item)
		{
			if (this.root == null)
			{
				this.root = new SortedSet<T>.Node(item, false);
				this.count = 1;
				this.version++;
				return true;
			}
			SortedSet<T>.Node node = this.root;
			SortedSet<T>.Node node2 = null;
			SortedSet<T>.Node node3 = null;
			SortedSet<T>.Node node4 = null;
			this.version++;
			int num = 0;
			while (node != null)
			{
				num = this.comparer.Compare(item, node.Item);
				if (num == 0)
				{
					this.root.IsRed = false;
					return false;
				}
				if (SortedSet<T>.Is4Node(node))
				{
					SortedSet<T>.Split4Node(node);
					if (SortedSet<T>.IsRed(node2))
					{
						this.InsertionBalance(node, ref node2, node3, node4);
					}
				}
				node4 = node3;
				node3 = node2;
				node2 = node;
				node = ((num < 0) ? node.Left : node.Right);
			}
			SortedSet<T>.Node node5 = new SortedSet<T>.Node(item);
			if (num > 0)
			{
				node2.Right = node5;
			}
			else
			{
				node2.Left = node5;
			}
			if (node2.IsRed)
			{
				this.InsertionBalance(node5, ref node2, node3, node4);
			}
			this.root.IsRed = false;
			this.count++;
			return true;
		}

		// Token: 0x060024F1 RID: 9457 RVA: 0x000ABD63 File Offset: 0x000A9F63
		[global::__DynamicallyInvokable]
		public bool Remove(T item)
		{
			return this.DoRemove(item);
		}

		// Token: 0x060024F2 RID: 9458 RVA: 0x000ABD6C File Offset: 0x000A9F6C
		internal virtual bool DoRemove(T item)
		{
			if (this.root == null)
			{
				return false;
			}
			this.version++;
			SortedSet<T>.Node node = this.root;
			SortedSet<T>.Node node2 = null;
			SortedSet<T>.Node node3 = null;
			SortedSet<T>.Node node4 = null;
			SortedSet<T>.Node node5 = null;
			bool flag = false;
			while (node != null)
			{
				if (SortedSet<T>.Is2Node(node))
				{
					if (node2 == null)
					{
						node.IsRed = true;
					}
					else
					{
						SortedSet<T>.Node node6 = SortedSet<T>.GetSibling(node, node2);
						if (node6.IsRed)
						{
							if (node2.Right == node6)
							{
								SortedSet<T>.RotateLeft(node2);
							}
							else
							{
								SortedSet<T>.RotateRight(node2);
							}
							node2.IsRed = true;
							node6.IsRed = false;
							this.ReplaceChildOfNodeOrRoot(node3, node2, node6);
							node3 = node6;
							if (node2 == node4)
							{
								node5 = node6;
							}
							node6 = ((node2.Left == node) ? node2.Right : node2.Left);
						}
						if (SortedSet<T>.Is2Node(node6))
						{
							SortedSet<T>.Merge2Nodes(node2, node, node6);
						}
						else
						{
							TreeRotation treeRotation = SortedSet<T>.RotationNeeded(node2, node, node6);
							SortedSet<T>.Node node7 = null;
							switch (treeRotation)
							{
							case TreeRotation.LeftRotation:
								node6.Right.IsRed = false;
								node7 = SortedSet<T>.RotateLeft(node2);
								break;
							case TreeRotation.RightRotation:
								node6.Left.IsRed = false;
								node7 = SortedSet<T>.RotateRight(node2);
								break;
							case TreeRotation.RightLeftRotation:
								node7 = SortedSet<T>.RotateRightLeft(node2);
								break;
							case TreeRotation.LeftRightRotation:
								node7 = SortedSet<T>.RotateLeftRight(node2);
								break;
							}
							node7.IsRed = node2.IsRed;
							node2.IsRed = false;
							node.IsRed = true;
							this.ReplaceChildOfNodeOrRoot(node3, node2, node7);
							if (node2 == node4)
							{
								node5 = node7;
							}
						}
					}
				}
				int num = (flag ? (-1) : this.comparer.Compare(item, node.Item));
				if (num == 0)
				{
					flag = true;
					node4 = node;
					node5 = node2;
				}
				node3 = node2;
				node2 = node;
				if (num < 0)
				{
					node = node.Left;
				}
				else
				{
					node = node.Right;
				}
			}
			if (node4 != null)
			{
				this.ReplaceNode(node4, node5, node2, node3);
				this.count--;
			}
			if (this.root != null)
			{
				this.root.IsRed = false;
			}
			return flag;
		}

		// Token: 0x060024F3 RID: 9459 RVA: 0x000ABF54 File Offset: 0x000AA154
		[global::__DynamicallyInvokable]
		public virtual void Clear()
		{
			this.root = null;
			this.count = 0;
			this.version++;
		}

		// Token: 0x060024F4 RID: 9460 RVA: 0x000ABF72 File Offset: 0x000AA172
		[global::__DynamicallyInvokable]
		public virtual bool Contains(T item)
		{
			return this.FindNode(item) != null;
		}

		// Token: 0x060024F5 RID: 9461 RVA: 0x000ABF7E File Offset: 0x000AA17E
		[global::__DynamicallyInvokable]
		public void CopyTo(T[] array)
		{
			this.CopyTo(array, 0, this.Count);
		}

		// Token: 0x060024F6 RID: 9462 RVA: 0x000ABF8E File Offset: 0x000AA18E
		[global::__DynamicallyInvokable]
		public void CopyTo(T[] array, int index)
		{
			this.CopyTo(array, index, this.Count);
		}

		// Token: 0x060024F7 RID: 9463 RVA: 0x000ABFA0 File Offset: 0x000AA1A0
		[global::__DynamicallyInvokable]
		public void CopyTo(T[] array, int index, int count)
		{
			if (array == null)
			{
				global::System.ThrowHelper.ThrowArgumentNullException(global::System.ExceptionArgument.array);
			}
			if (index < 0)
			{
				global::System.ThrowHelper.ThrowArgumentOutOfRangeException(global::System.ExceptionArgument.index);
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", SR.GetString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (index > array.Length || count > array.Length - index)
			{
				throw new ArgumentException(SR.GetString("Arg_ArrayPlusOffTooSmall"));
			}
			count += index;
			this.InOrderTreeWalk(delegate(SortedSet<T>.Node node)
			{
				if (index >= count)
				{
					return false;
				}
				T[] array2 = array;
				int index2 = index;
				index = index2 + 1;
				array2[index2] = node.Item;
				return true;
			});
		}

		// Token: 0x060024F8 RID: 9464 RVA: 0x000AC064 File Offset: 0x000AA264
		[global::__DynamicallyInvokable]
		void ICollection.CopyTo(Array array, int index)
		{
			if (array == null)
			{
				global::System.ThrowHelper.ThrowArgumentNullException(global::System.ExceptionArgument.array);
			}
			if (array.Rank != 1)
			{
				global::System.ThrowHelper.ThrowArgumentException(global::System.ExceptionResource.Arg_RankMultiDimNotSupported);
			}
			if (array.GetLowerBound(0) != 0)
			{
				global::System.ThrowHelper.ThrowArgumentException(global::System.ExceptionResource.Arg_NonZeroLowerBound);
			}
			if (index < 0)
			{
				global::System.ThrowHelper.ThrowArgumentOutOfRangeException(global::System.ExceptionArgument.arrayIndex, global::System.ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
			}
			if (array.Length - index < this.Count)
			{
				global::System.ThrowHelper.ThrowArgumentException(global::System.ExceptionResource.Arg_ArrayPlusOffTooSmall);
			}
			T[] array2 = array as T[];
			if (array2 != null)
			{
				this.CopyTo(array2, index);
				return;
			}
			object[] objects = array as object[];
			if (objects == null)
			{
				global::System.ThrowHelper.ThrowArgumentException(global::System.ExceptionResource.Argument_InvalidArrayType);
			}
			try
			{
				this.InOrderTreeWalk(delegate(SortedSet<T>.Node node)
				{
					object[] objects2 = objects;
					int index2 = index;
					index = index2 + 1;
					objects2[index2] = node.Item;
					return true;
				});
			}
			catch (ArrayTypeMismatchException)
			{
				global::System.ThrowHelper.ThrowArgumentException(global::System.ExceptionResource.Argument_InvalidArrayType);
			}
		}

		// Token: 0x060024F9 RID: 9465 RVA: 0x000AC138 File Offset: 0x000AA338
		[global::__DynamicallyInvokable]
		public SortedSet<T>.Enumerator GetEnumerator()
		{
			return new SortedSet<T>.Enumerator(this);
		}

		// Token: 0x060024FA RID: 9466 RVA: 0x000AC140 File Offset: 0x000AA340
		[global::__DynamicallyInvokable]
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return new SortedSet<T>.Enumerator(this);
		}

		// Token: 0x060024FB RID: 9467 RVA: 0x000AC14D File Offset: 0x000AA34D
		[global::__DynamicallyInvokable]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new SortedSet<T>.Enumerator(this);
		}

		// Token: 0x060024FC RID: 9468 RVA: 0x000AC15A File Offset: 0x000AA35A
		private static SortedSet<T>.Node GetSibling(SortedSet<T>.Node node, SortedSet<T>.Node parent)
		{
			if (parent.Left == node)
			{
				return parent.Right;
			}
			return parent.Left;
		}

		// Token: 0x060024FD RID: 9469 RVA: 0x000AC174 File Offset: 0x000AA374
		private void InsertionBalance(SortedSet<T>.Node current, ref SortedSet<T>.Node parent, SortedSet<T>.Node grandParent, SortedSet<T>.Node greatGrandParent)
		{
			bool flag = grandParent.Right == parent;
			bool flag2 = parent.Right == current;
			SortedSet<T>.Node node;
			if (flag == flag2)
			{
				node = (flag2 ? SortedSet<T>.RotateLeft(grandParent) : SortedSet<T>.RotateRight(grandParent));
			}
			else
			{
				node = (flag2 ? SortedSet<T>.RotateLeftRight(grandParent) : SortedSet<T>.RotateRightLeft(grandParent));
				parent = greatGrandParent;
			}
			grandParent.IsRed = true;
			node.IsRed = false;
			this.ReplaceChildOfNodeOrRoot(greatGrandParent, grandParent, node);
		}

		// Token: 0x060024FE RID: 9470 RVA: 0x000AC1DD File Offset: 0x000AA3DD
		private static bool Is2Node(SortedSet<T>.Node node)
		{
			return SortedSet<T>.IsBlack(node) && SortedSet<T>.IsNullOrBlack(node.Left) && SortedSet<T>.IsNullOrBlack(node.Right);
		}

		// Token: 0x060024FF RID: 9471 RVA: 0x000AC201 File Offset: 0x000AA401
		private static bool Is4Node(SortedSet<T>.Node node)
		{
			return SortedSet<T>.IsRed(node.Left) && SortedSet<T>.IsRed(node.Right);
		}

		// Token: 0x06002500 RID: 9472 RVA: 0x000AC21D File Offset: 0x000AA41D
		private static bool IsBlack(SortedSet<T>.Node node)
		{
			return node != null && !node.IsRed;
		}

		// Token: 0x06002501 RID: 9473 RVA: 0x000AC22D File Offset: 0x000AA42D
		private static bool IsNullOrBlack(SortedSet<T>.Node node)
		{
			return node == null || !node.IsRed;
		}

		// Token: 0x06002502 RID: 9474 RVA: 0x000AC23D File Offset: 0x000AA43D
		private static bool IsRed(SortedSet<T>.Node node)
		{
			return node != null && node.IsRed;
		}

		// Token: 0x06002503 RID: 9475 RVA: 0x000AC24A File Offset: 0x000AA44A
		private static void Merge2Nodes(SortedSet<T>.Node parent, SortedSet<T>.Node child1, SortedSet<T>.Node child2)
		{
			parent.IsRed = false;
			child1.IsRed = true;
			child2.IsRed = true;
		}

		// Token: 0x06002504 RID: 9476 RVA: 0x000AC261 File Offset: 0x000AA461
		private void ReplaceChildOfNodeOrRoot(SortedSet<T>.Node parent, SortedSet<T>.Node child, SortedSet<T>.Node newChild)
		{
			if (parent == null)
			{
				this.root = newChild;
				return;
			}
			if (parent.Left == child)
			{
				parent.Left = newChild;
				return;
			}
			parent.Right = newChild;
		}

		// Token: 0x06002505 RID: 9477 RVA: 0x000AC288 File Offset: 0x000AA488
		private void ReplaceNode(SortedSet<T>.Node match, SortedSet<T>.Node parentOfMatch, SortedSet<T>.Node succesor, SortedSet<T>.Node parentOfSuccesor)
		{
			if (succesor == match)
			{
				succesor = match.Left;
			}
			else
			{
				if (succesor.Right != null)
				{
					succesor.Right.IsRed = false;
				}
				if (parentOfSuccesor != match)
				{
					parentOfSuccesor.Left = succesor.Right;
					succesor.Right = match.Right;
				}
				succesor.Left = match.Left;
			}
			if (succesor != null)
			{
				succesor.IsRed = match.IsRed;
			}
			this.ReplaceChildOfNodeOrRoot(parentOfMatch, match, succesor);
		}

		// Token: 0x06002506 RID: 9478 RVA: 0x000AC2FC File Offset: 0x000AA4FC
		internal virtual SortedSet<T>.Node FindNode(T item)
		{
			int num;
			for (SortedSet<T>.Node node = this.root; node != null; node = ((num < 0) ? node.Left : node.Right))
			{
				num = this.comparer.Compare(item, node.Item);
				if (num == 0)
				{
					return node;
				}
			}
			return null;
		}

		// Token: 0x06002507 RID: 9479 RVA: 0x000AC344 File Offset: 0x000AA544
		internal virtual int InternalIndexOf(T item)
		{
			SortedSet<T>.Node node = this.root;
			int num = 0;
			while (node != null)
			{
				int num2 = this.comparer.Compare(item, node.Item);
				if (num2 == 0)
				{
					return num;
				}
				node = ((num2 < 0) ? node.Left : node.Right);
				num = ((num2 < 0) ? (2 * num + 1) : (2 * num + 2));
			}
			return -1;
		}

		// Token: 0x06002508 RID: 9480 RVA: 0x000AC39C File Offset: 0x000AA59C
		internal SortedSet<T>.Node FindRange(T from, T to)
		{
			return this.FindRange(from, to, true, true);
		}

		// Token: 0x06002509 RID: 9481 RVA: 0x000AC3A8 File Offset: 0x000AA5A8
		internal SortedSet<T>.Node FindRange(T from, T to, bool lowerBoundActive, bool upperBoundActive)
		{
			SortedSet<T>.Node node = this.root;
			while (node != null)
			{
				if (lowerBoundActive && this.comparer.Compare(from, node.Item) > 0)
				{
					node = node.Right;
				}
				else
				{
					if (!upperBoundActive || this.comparer.Compare(to, node.Item) >= 0)
					{
						return node;
					}
					node = node.Left;
				}
			}
			return null;
		}

		// Token: 0x0600250A RID: 9482 RVA: 0x000AC407 File Offset: 0x000AA607
		internal void UpdateVersion()
		{
			this.version++;
		}

		// Token: 0x0600250B RID: 9483 RVA: 0x000AC418 File Offset: 0x000AA618
		private static SortedSet<T>.Node RotateLeft(SortedSet<T>.Node node)
		{
			SortedSet<T>.Node right = node.Right;
			node.Right = right.Left;
			right.Left = node;
			return right;
		}

		// Token: 0x0600250C RID: 9484 RVA: 0x000AC440 File Offset: 0x000AA640
		private static SortedSet<T>.Node RotateLeftRight(SortedSet<T>.Node node)
		{
			SortedSet<T>.Node left = node.Left;
			SortedSet<T>.Node right = left.Right;
			node.Left = right.Right;
			right.Right = node;
			left.Right = right.Left;
			right.Left = left;
			return right;
		}

		// Token: 0x0600250D RID: 9485 RVA: 0x000AC484 File Offset: 0x000AA684
		private static SortedSet<T>.Node RotateRight(SortedSet<T>.Node node)
		{
			SortedSet<T>.Node left = node.Left;
			node.Left = left.Right;
			left.Right = node;
			return left;
		}

		// Token: 0x0600250E RID: 9486 RVA: 0x000AC4AC File Offset: 0x000AA6AC
		private static SortedSet<T>.Node RotateRightLeft(SortedSet<T>.Node node)
		{
			SortedSet<T>.Node right = node.Right;
			SortedSet<T>.Node left = right.Left;
			node.Right = left.Left;
			left.Left = node;
			right.Left = left.Right;
			left.Right = right;
			return left;
		}

		// Token: 0x0600250F RID: 9487 RVA: 0x000AC4EE File Offset: 0x000AA6EE
		private static TreeRotation RotationNeeded(SortedSet<T>.Node parent, SortedSet<T>.Node current, SortedSet<T>.Node sibling)
		{
			if (SortedSet<T>.IsRed(sibling.Left))
			{
				if (parent.Left == current)
				{
					return TreeRotation.RightLeftRotation;
				}
				return TreeRotation.RightRotation;
			}
			else
			{
				if (parent.Left == current)
				{
					return TreeRotation.LeftRotation;
				}
				return TreeRotation.LeftRightRotation;
			}
		}

		// Token: 0x06002510 RID: 9488 RVA: 0x000AC516 File Offset: 0x000AA716
		public static IEqualityComparer<SortedSet<T>> CreateSetComparer()
		{
			return new SortedSetEqualityComparer<T>();
		}

		// Token: 0x06002511 RID: 9489 RVA: 0x000AC51D File Offset: 0x000AA71D
		public static IEqualityComparer<SortedSet<T>> CreateSetComparer(IEqualityComparer<T> memberEqualityComparer)
		{
			return new SortedSetEqualityComparer<T>(memberEqualityComparer);
		}

		// Token: 0x06002512 RID: 9490 RVA: 0x000AC528 File Offset: 0x000AA728
		internal static bool SortedSetEquals(SortedSet<T> set1, SortedSet<T> set2, IComparer<T> comparer)
		{
			if (set1 == null)
			{
				return set2 == null;
			}
			if (set2 == null)
			{
				return false;
			}
			if (SortedSet<T>.AreComparersEqual(set1, set2))
			{
				return set1.Count == set2.Count && set1.SetEquals(set2);
			}
			bool flag = false;
			foreach (T t in set1)
			{
				flag = false;
				foreach (T t2 in set2)
				{
					if (comparer.Compare(t, t2) == 0)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06002513 RID: 9491 RVA: 0x000AC5F4 File Offset: 0x000AA7F4
		private static bool AreComparersEqual(SortedSet<T> set1, SortedSet<T> set2)
		{
			return set1.Comparer.Equals(set2.Comparer);
		}

		// Token: 0x06002514 RID: 9492 RVA: 0x000AC607 File Offset: 0x000AA807
		private static void Split4Node(SortedSet<T>.Node node)
		{
			node.IsRed = true;
			node.Left.IsRed = false;
			node.Right.IsRed = false;
		}

		// Token: 0x06002515 RID: 9493 RVA: 0x000AC628 File Offset: 0x000AA828
		internal T[] ToArray()
		{
			T[] array = new T[this.Count];
			this.CopyTo(array);
			return array;
		}

		// Token: 0x06002516 RID: 9494 RVA: 0x000AC64C File Offset: 0x000AA84C
		[global::__DynamicallyInvokable]
		public void UnionWith(IEnumerable<T> other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other");
			}
			SortedSet<T> sortedSet = other as SortedSet<T>;
			SortedSet<T>.TreeSubSet treeSubSet = this as SortedSet<T>.TreeSubSet;
			if (treeSubSet != null)
			{
				this.VersionCheck();
			}
			if (sortedSet != null && treeSubSet == null && this.count == 0)
			{
				SortedSet<T> sortedSet2 = new SortedSet<T>(sortedSet, this.comparer);
				this.root = sortedSet2.root;
				this.count = sortedSet2.count;
				this.version++;
				return;
			}
			if (sortedSet != null && treeSubSet == null && SortedSet<T>.AreComparersEqual(this, sortedSet) && sortedSet.Count > this.Count / 2)
			{
				T[] array = new T[sortedSet.Count + this.Count];
				int num = 0;
				SortedSet<T>.Enumerator enumerator = this.GetEnumerator();
				SortedSet<T>.Enumerator enumerator2 = sortedSet.GetEnumerator();
				bool flag = !enumerator.MoveNext();
				bool flag2 = !enumerator2.MoveNext();
				while (!flag && !flag2)
				{
					int num2 = this.Comparer.Compare(enumerator.Current, enumerator2.Current);
					if (num2 < 0)
					{
						array[num++] = enumerator.Current;
						flag = !enumerator.MoveNext();
					}
					else if (num2 == 0)
					{
						array[num++] = enumerator2.Current;
						flag = !enumerator.MoveNext();
						flag2 = !enumerator2.MoveNext();
					}
					else
					{
						array[num++] = enumerator2.Current;
						flag2 = !enumerator2.MoveNext();
					}
				}
				if (!flag || !flag2)
				{
					SortedSet<T>.Enumerator enumerator3 = (flag ? enumerator2 : enumerator);
					do
					{
						array[num++] = enumerator3.Current;
					}
					while (enumerator3.MoveNext());
				}
				this.root = null;
				this.root = SortedSet<T>.ConstructRootFromSortedArray(array, 0, num - 1, null);
				this.count = num;
				this.version++;
				return;
			}
			this.AddAllElements(other);
		}

		// Token: 0x06002517 RID: 9495 RVA: 0x000AC838 File Offset: 0x000AAA38
		private static SortedSet<T>.Node ConstructRootFromSortedArray(T[] arr, int startIndex, int endIndex, SortedSet<T>.Node redNode)
		{
			int num = endIndex - startIndex + 1;
			if (num == 0)
			{
				return null;
			}
			SortedSet<T>.Node node;
			if (num == 1)
			{
				node = new SortedSet<T>.Node(arr[startIndex], false);
				if (redNode != null)
				{
					node.Left = redNode;
				}
			}
			else if (num == 2)
			{
				node = new SortedSet<T>.Node(arr[startIndex], false);
				node.Right = new SortedSet<T>.Node(arr[endIndex], false);
				node.Right.IsRed = true;
				if (redNode != null)
				{
					node.Left = redNode;
				}
			}
			else if (num == 3)
			{
				node = new SortedSet<T>.Node(arr[startIndex + 1], false);
				node.Left = new SortedSet<T>.Node(arr[startIndex], false);
				node.Right = new SortedSet<T>.Node(arr[endIndex], false);
				if (redNode != null)
				{
					node.Left.Left = redNode;
				}
			}
			else
			{
				int num2 = (startIndex + endIndex) / 2;
				node = new SortedSet<T>.Node(arr[num2], false);
				node.Left = SortedSet<T>.ConstructRootFromSortedArray(arr, startIndex, num2 - 1, redNode);
				if (num % 2 == 0)
				{
					node.Right = SortedSet<T>.ConstructRootFromSortedArray(arr, num2 + 2, endIndex, new SortedSet<T>.Node(arr[num2 + 1], true));
				}
				else
				{
					node.Right = SortedSet<T>.ConstructRootFromSortedArray(arr, num2 + 1, endIndex, null);
				}
			}
			return node;
		}

		// Token: 0x06002518 RID: 9496 RVA: 0x000AC964 File Offset: 0x000AAB64
		[global::__DynamicallyInvokable]
		public virtual void IntersectWith(IEnumerable<T> other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other");
			}
			if (this.Count == 0)
			{
				return;
			}
			SortedSet<T> sortedSet = other as SortedSet<T>;
			SortedSet<T>.TreeSubSet treeSubSet = this as SortedSet<T>.TreeSubSet;
			if (treeSubSet != null)
			{
				this.VersionCheck();
			}
			if (sortedSet != null && treeSubSet == null && SortedSet<T>.AreComparersEqual(this, sortedSet))
			{
				T[] array = new T[this.Count];
				int num = 0;
				SortedSet<T>.Enumerator enumerator = this.GetEnumerator();
				SortedSet<T>.Enumerator enumerator2 = sortedSet.GetEnumerator();
				bool flag = !enumerator.MoveNext();
				bool flag2 = !enumerator2.MoveNext();
				T max = this.Max;
				T min = this.Min;
				while (!flag && !flag2 && this.Comparer.Compare(enumerator2.Current, max) <= 0)
				{
					int num2 = this.Comparer.Compare(enumerator.Current, enumerator2.Current);
					if (num2 < 0)
					{
						flag = !enumerator.MoveNext();
					}
					else if (num2 == 0)
					{
						array[num++] = enumerator2.Current;
						flag = !enumerator.MoveNext();
						flag2 = !enumerator2.MoveNext();
					}
					else
					{
						flag2 = !enumerator2.MoveNext();
					}
				}
				this.root = null;
				this.root = SortedSet<T>.ConstructRootFromSortedArray(array, 0, num - 1, null);
				this.count = num;
				this.version++;
				return;
			}
			this.IntersectWithEnumerable(other);
		}

		// Token: 0x06002519 RID: 9497 RVA: 0x000ACAC0 File Offset: 0x000AACC0
		internal virtual void IntersectWithEnumerable(IEnumerable<T> other)
		{
			List<T> list = new List<T>(this.Count);
			foreach (T t in other)
			{
				if (this.Contains(t))
				{
					list.Add(t);
					this.Remove(t);
				}
			}
			this.Clear();
			this.AddAllElements(list);
		}

		// Token: 0x0600251A RID: 9498 RVA: 0x000ACB34 File Offset: 0x000AAD34
		[global::__DynamicallyInvokable]
		public void ExceptWith(IEnumerable<T> other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other");
			}
			if (this.count == 0)
			{
				return;
			}
			if (other == this)
			{
				this.Clear();
				return;
			}
			SortedSet<T> sortedSet = other as SortedSet<T>;
			if (sortedSet != null && SortedSet<T>.AreComparersEqual(this, sortedSet))
			{
				if (this.comparer.Compare(sortedSet.Max, this.Min) < 0 || this.comparer.Compare(sortedSet.Min, this.Max) > 0)
				{
					return;
				}
				T min = this.Min;
				T max = this.Max;
				using (IEnumerator<T> enumerator = other.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						T t = enumerator.Current;
						if (this.comparer.Compare(t, min) >= 0)
						{
							if (this.comparer.Compare(t, max) > 0)
							{
								break;
							}
							this.Remove(t);
						}
					}
					return;
				}
			}
			this.RemoveAllElements(other);
		}

		// Token: 0x0600251B RID: 9499 RVA: 0x000ACC2C File Offset: 0x000AAE2C
		[global::__DynamicallyInvokable]
		public void SymmetricExceptWith(IEnumerable<T> other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other");
			}
			if (this.Count == 0)
			{
				this.UnionWith(other);
				return;
			}
			if (other == this)
			{
				this.Clear();
				return;
			}
			SortedSet<T> sortedSet = other as SortedSet<T>;
			if (sortedSet != null && SortedSet<T>.AreComparersEqual(this, sortedSet))
			{
				this.SymmetricExceptWithSameEC(sortedSet);
				return;
			}
			T[] array = new List<T>(other).ToArray();
			Array.Sort<T>(array, this.Comparer);
			this.SymmetricExceptWithSameEC(array);
		}

		// Token: 0x0600251C RID: 9500 RVA: 0x000ACC9C File Offset: 0x000AAE9C
		internal void SymmetricExceptWithSameEC(ISet<T> other)
		{
			foreach (T t in other)
			{
				if (this.Contains(t))
				{
					this.Remove(t);
				}
				else
				{
					this.Add(t);
				}
			}
		}

		// Token: 0x0600251D RID: 9501 RVA: 0x000ACCF8 File Offset: 0x000AAEF8
		internal void SymmetricExceptWithSameEC(T[] other)
		{
			if (other.Length == 0)
			{
				return;
			}
			T t = other[0];
			for (int i = 0; i < other.Length; i++)
			{
				while (i < other.Length && i != 0 && this.comparer.Compare(other[i], t) == 0)
				{
					i++;
				}
				if (i >= other.Length)
				{
					break;
				}
				if (this.Contains(other[i]))
				{
					this.Remove(other[i]);
				}
				else
				{
					this.Add(other[i]);
				}
				t = other[i];
			}
		}

		// Token: 0x0600251E RID: 9502 RVA: 0x000ACD80 File Offset: 0x000AAF80
		[SecuritySafeCritical]
		[global::__DynamicallyInvokable]
		public bool IsSubsetOf(IEnumerable<T> other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other");
			}
			if (this.Count == 0)
			{
				return true;
			}
			SortedSet<T> sortedSet = other as SortedSet<T>;
			if (sortedSet != null && SortedSet<T>.AreComparersEqual(this, sortedSet))
			{
				return this.Count <= sortedSet.Count && this.IsSubsetOfSortedSetWithSameEC(sortedSet);
			}
			SortedSet<T>.ElementCount elementCount = this.CheckUniqueAndUnfoundElements(other, false);
			return elementCount.uniqueCount == this.Count && elementCount.unfoundCount >= 0;
		}

		// Token: 0x0600251F RID: 9503 RVA: 0x000ACDF8 File Offset: 0x000AAFF8
		private bool IsSubsetOfSortedSetWithSameEC(SortedSet<T> asSorted)
		{
			SortedSet<T> viewBetween = asSorted.GetViewBetween(this.Min, this.Max);
			foreach (T t in this)
			{
				if (!viewBetween.Contains(t))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06002520 RID: 9504 RVA: 0x000ACE64 File Offset: 0x000AB064
		[SecuritySafeCritical]
		[global::__DynamicallyInvokable]
		public bool IsProperSubsetOf(IEnumerable<T> other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other");
			}
			if (other is ICollection && this.Count == 0)
			{
				return (other as ICollection).Count > 0;
			}
			SortedSet<T> sortedSet = other as SortedSet<T>;
			if (sortedSet != null && SortedSet<T>.AreComparersEqual(this, sortedSet))
			{
				return this.Count < sortedSet.Count && this.IsSubsetOfSortedSetWithSameEC(sortedSet);
			}
			SortedSet<T>.ElementCount elementCount = this.CheckUniqueAndUnfoundElements(other, false);
			return elementCount.uniqueCount == this.Count && elementCount.unfoundCount > 0;
		}

		// Token: 0x06002521 RID: 9505 RVA: 0x000ACEEC File Offset: 0x000AB0EC
		[global::__DynamicallyInvokable]
		public bool IsSupersetOf(IEnumerable<T> other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other");
			}
			if (other is ICollection && (other as ICollection).Count == 0)
			{
				return true;
			}
			SortedSet<T> sortedSet = other as SortedSet<T>;
			if (sortedSet == null || !SortedSet<T>.AreComparersEqual(this, sortedSet))
			{
				return this.ContainsAllElements(other);
			}
			if (this.Count < sortedSet.Count)
			{
				return false;
			}
			SortedSet<T> viewBetween = this.GetViewBetween(sortedSet.Min, sortedSet.Max);
			foreach (T t in sortedSet)
			{
				if (!viewBetween.Contains(t))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06002522 RID: 9506 RVA: 0x000ACFA8 File Offset: 0x000AB1A8
		[SecuritySafeCritical]
		[global::__DynamicallyInvokable]
		public bool IsProperSupersetOf(IEnumerable<T> other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other");
			}
			if (this.Count == 0)
			{
				return false;
			}
			if (other is ICollection && (other as ICollection).Count == 0)
			{
				return true;
			}
			SortedSet<T> sortedSet = other as SortedSet<T>;
			if (sortedSet == null || !SortedSet<T>.AreComparersEqual(sortedSet, this))
			{
				SortedSet<T>.ElementCount elementCount = this.CheckUniqueAndUnfoundElements(other, true);
				return elementCount.uniqueCount < this.Count && elementCount.unfoundCount == 0;
			}
			if (sortedSet.Count >= this.Count)
			{
				return false;
			}
			SortedSet<T> viewBetween = this.GetViewBetween(sortedSet.Min, sortedSet.Max);
			foreach (T t in sortedSet)
			{
				if (!viewBetween.Contains(t))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06002523 RID: 9507 RVA: 0x000AD08C File Offset: 0x000AB28C
		[SecuritySafeCritical]
		[global::__DynamicallyInvokable]
		public bool SetEquals(IEnumerable<T> other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other");
			}
			SortedSet<T> sortedSet = other as SortedSet<T>;
			if (sortedSet != null && SortedSet<T>.AreComparersEqual(this, sortedSet))
			{
				IEnumerator<T> enumerator = this.GetEnumerator();
				IEnumerator<T> enumerator2 = sortedSet.GetEnumerator();
				bool flag = !enumerator.MoveNext();
				bool flag2 = !enumerator2.MoveNext();
				while (!flag && !flag2)
				{
					if (this.Comparer.Compare(enumerator.Current, enumerator2.Current) != 0)
					{
						return false;
					}
					flag = !enumerator.MoveNext();
					flag2 = !enumerator2.MoveNext();
				}
				return flag && flag2;
			}
			SortedSet<T>.ElementCount elementCount = this.CheckUniqueAndUnfoundElements(other, true);
			return elementCount.uniqueCount == this.Count && elementCount.unfoundCount == 0;
		}

		// Token: 0x06002524 RID: 9508 RVA: 0x000AD14C File Offset: 0x000AB34C
		[global::__DynamicallyInvokable]
		public bool Overlaps(IEnumerable<T> other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other");
			}
			if (this.Count == 0)
			{
				return false;
			}
			if (other is ICollection<T> && (other as ICollection<T>).Count == 0)
			{
				return false;
			}
			SortedSet<T> sortedSet = other as SortedSet<T>;
			if (sortedSet != null && SortedSet<T>.AreComparersEqual(this, sortedSet) && (this.comparer.Compare(this.Min, sortedSet.Max) > 0 || this.comparer.Compare(this.Max, sortedSet.Min) < 0))
			{
				return false;
			}
			foreach (T t in other)
			{
				if (this.Contains(t))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002525 RID: 9509 RVA: 0x000AD218 File Offset: 0x000AB418
		[SecurityCritical]
		private unsafe SortedSet<T>.ElementCount CheckUniqueAndUnfoundElements(IEnumerable<T> other, bool returnIfUnfound)
		{
			SortedSet<T>.ElementCount elementCount;
			if (this.Count == 0)
			{
				int num = 0;
				using (IEnumerator<T> enumerator = other.GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						T t = enumerator.Current;
						num++;
					}
				}
				elementCount.uniqueCount = 0;
				elementCount.unfoundCount = num;
				return elementCount;
			}
			int num2 = this.Count;
			int num3 = BitHelper.ToIntArrayLength(num2);
			BitHelper bitHelper;
			int num4;
			int num5;
			checked
			{
				if (num3 <= 100)
				{
					int* ptr = stackalloc int[unchecked((UIntPtr)num3) * 4];
					bitHelper = new BitHelper(ptr, num3);
				}
				else
				{
					int[] array = new int[num3];
					bitHelper = new BitHelper(array, num3);
				}
				num4 = 0;
				num5 = 0;
			}
			foreach (T t2 in other)
			{
				int num6 = this.InternalIndexOf(t2);
				if (num6 >= 0)
				{
					if (!bitHelper.IsMarked(num6))
					{
						bitHelper.MarkBit(num6);
						num5++;
					}
				}
				else
				{
					num4++;
					if (returnIfUnfound)
					{
						break;
					}
				}
			}
			elementCount.uniqueCount = num5;
			elementCount.unfoundCount = num4;
			return elementCount;
		}

		// Token: 0x06002526 RID: 9510 RVA: 0x000AD340 File Offset: 0x000AB540
		[global::__DynamicallyInvokable]
		public int RemoveWhere(Predicate<T> match)
		{
			if (match == null)
			{
				throw new ArgumentNullException("match");
			}
			List<T> matches = new List<T>(this.Count);
			this.BreadthFirstTreeWalk(delegate(SortedSet<T>.Node n)
			{
				if (match(n.Item))
				{
					matches.Add(n.Item);
				}
				return true;
			});
			int num = 0;
			for (int i = matches.Count - 1; i >= 0; i--)
			{
				if (this.Remove(matches[i]))
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x17000956 RID: 2390
		// (get) Token: 0x06002527 RID: 9511 RVA: 0x000AD3C4 File Offset: 0x000AB5C4
		[global::__DynamicallyInvokable]
		public T Min
		{
			[global::__DynamicallyInvokable]
			get
			{
				T ret = default(T);
				this.InOrderTreeWalk(delegate(SortedSet<T>.Node n)
				{
					ret = n.Item;
					return false;
				});
				return ret;
			}
		}

		// Token: 0x17000957 RID: 2391
		// (get) Token: 0x06002528 RID: 9512 RVA: 0x000AD3FC File Offset: 0x000AB5FC
		[global::__DynamicallyInvokable]
		public T Max
		{
			[global::__DynamicallyInvokable]
			get
			{
				T ret = default(T);
				this.InOrderTreeWalk(delegate(SortedSet<T>.Node n)
				{
					ret = n.Item;
					return false;
				}, true);
				return ret;
			}
		}

		// Token: 0x06002529 RID: 9513 RVA: 0x000AD435 File Offset: 0x000AB635
		[global::__DynamicallyInvokable]
		public IEnumerable<T> Reverse()
		{
			SortedSet<T>.Enumerator e = new SortedSet<T>.Enumerator(this, true);
			while (e.MoveNext())
			{
				T t = e.Current;
				yield return t;
			}
			yield break;
		}

		// Token: 0x0600252A RID: 9514 RVA: 0x000AD445 File Offset: 0x000AB645
		[global::__DynamicallyInvokable]
		public virtual SortedSet<T> GetViewBetween(T lowerValue, T upperValue)
		{
			if (this.Comparer.Compare(lowerValue, upperValue) > 0)
			{
				throw new ArgumentException("lowerBound is greater than upperBound");
			}
			return new SortedSet<T>.TreeSubSet(this, lowerValue, upperValue, true, true);
		}

		// Token: 0x0600252B RID: 9515 RVA: 0x000AD46C File Offset: 0x000AB66C
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			this.GetObjectData(info, context);
		}

		// Token: 0x0600252C RID: 9516 RVA: 0x000AD478 File Offset: 0x000AB678
		protected virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				global::System.ThrowHelper.ThrowArgumentNullException(global::System.ExceptionArgument.info);
			}
			info.AddValue("Count", this.count);
			info.AddValue("Comparer", this.comparer, typeof(IComparer<T>));
			info.AddValue("Version", this.version);
			if (this.root != null)
			{
				T[] array = new T[this.Count];
				this.CopyTo(array, 0);
				info.AddValue("Items", array, typeof(T[]));
			}
		}

		// Token: 0x0600252D RID: 9517 RVA: 0x000AD4FD File Offset: 0x000AB6FD
		void IDeserializationCallback.OnDeserialization(object sender)
		{
			this.OnDeserialization(sender);
		}

		// Token: 0x0600252E RID: 9518 RVA: 0x000AD508 File Offset: 0x000AB708
		protected virtual void OnDeserialization(object sender)
		{
			if (this.comparer != null)
			{
				return;
			}
			if (this.siInfo == null)
			{
				global::System.ThrowHelper.ThrowSerializationException(global::System.ExceptionResource.Serialization_InvalidOnDeser);
			}
			this.comparer = (IComparer<T>)this.siInfo.GetValue("Comparer", typeof(IComparer<T>));
			int @int = this.siInfo.GetInt32("Count");
			if (@int != 0)
			{
				T[] array = (T[])this.siInfo.GetValue("Items", typeof(T[]));
				if (array == null)
				{
					global::System.ThrowHelper.ThrowSerializationException(global::System.ExceptionResource.Serialization_MissingValues);
				}
				for (int i = 0; i < array.Length; i++)
				{
					this.Add(array[i]);
				}
			}
			this.version = this.siInfo.GetInt32("Version");
			if (this.count != @int)
			{
				global::System.ThrowHelper.ThrowSerializationException(global::System.ExceptionResource.Serialization_MismatchedCount);
			}
			this.siInfo = null;
		}

		// Token: 0x0600252F RID: 9519 RVA: 0x000AD5D8 File Offset: 0x000AB7D8
		public bool TryGetValue(T equalValue, out T actualValue)
		{
			SortedSet<T>.Node node = this.FindNode(equalValue);
			if (node != null)
			{
				actualValue = node.Item;
				return true;
			}
			actualValue = default(T);
			return false;
		}

		// Token: 0x06002530 RID: 9520 RVA: 0x000AD608 File Offset: 0x000AB808
		private static int log2(int value)
		{
			int num = 0;
			while (value > 0)
			{
				num++;
				value >>= 1;
			}
			return num;
		}

		// Token: 0x04002029 RID: 8233
		private SortedSet<T>.Node root;

		// Token: 0x0400202A RID: 8234
		private IComparer<T> comparer;

		// Token: 0x0400202B RID: 8235
		private int count;

		// Token: 0x0400202C RID: 8236
		private int version;

		// Token: 0x0400202D RID: 8237
		private object _syncRoot;

		// Token: 0x0400202E RID: 8238
		private const string ComparerName = "Comparer";

		// Token: 0x0400202F RID: 8239
		private const string CountName = "Count";

		// Token: 0x04002030 RID: 8240
		private const string ItemsName = "Items";

		// Token: 0x04002031 RID: 8241
		private const string VersionName = "Version";

		// Token: 0x04002032 RID: 8242
		private const string TreeName = "Tree";

		// Token: 0x04002033 RID: 8243
		private const string NodeValueName = "Item";

		// Token: 0x04002034 RID: 8244
		private const string EnumStartName = "EnumStarted";

		// Token: 0x04002035 RID: 8245
		private const string ReverseName = "Reverse";

		// Token: 0x04002036 RID: 8246
		private const string EnumVersionName = "EnumVersion";

		// Token: 0x04002037 RID: 8247
		private const string minName = "Min";

		// Token: 0x04002038 RID: 8248
		private const string maxName = "Max";

		// Token: 0x04002039 RID: 8249
		private const string lBoundActiveName = "lBoundActive";

		// Token: 0x0400203A RID: 8250
		private const string uBoundActiveName = "uBoundActive";

		// Token: 0x0400203B RID: 8251
		private SerializationInfo siInfo;

		// Token: 0x0400203C RID: 8252
		internal const int StackAllocThreshold = 100;

		// Token: 0x02000800 RID: 2048
		[Serializable]
		internal sealed class TreeSubSet : SortedSet<T>, ISerializable, IDeserializationCallback
		{
			// Token: 0x0600449A RID: 17562 RVA: 0x0011F598 File Offset: 0x0011D798
			public TreeSubSet(SortedSet<T> Underlying, T Min, T Max, bool lowerBoundActive, bool upperBoundActive)
				: base(Underlying.Comparer)
			{
				this.underlying = Underlying;
				this.min = Min;
				this.max = Max;
				this.lBoundActive = lowerBoundActive;
				this.uBoundActive = upperBoundActive;
				this.root = this.underlying.FindRange(this.min, this.max, this.lBoundActive, this.uBoundActive);
				this.count = 0;
				this.version = -1;
				this.VersionCheckImpl();
			}

			// Token: 0x0600449B RID: 17563 RVA: 0x0011F613 File Offset: 0x0011D813
			private TreeSubSet()
			{
				this.comparer = null;
			}

			// Token: 0x0600449C RID: 17564 RVA: 0x0011F622 File Offset: 0x0011D822
			private TreeSubSet(SerializationInfo info, StreamingContext context)
			{
				this.siInfo = info;
				this.OnDeserializationImpl(info);
			}

			// Token: 0x0600449D RID: 17565 RVA: 0x0011F638 File Offset: 0x0011D838
			internal override bool AddIfNotPresent(T item)
			{
				if (!this.IsWithinRange(item))
				{
					global::System.ThrowHelper.ThrowArgumentOutOfRangeException(global::System.ExceptionArgument.collection);
				}
				bool flag = this.underlying.AddIfNotPresent(item);
				this.VersionCheck();
				return flag;
			}

			// Token: 0x0600449E RID: 17566 RVA: 0x0011F668 File Offset: 0x0011D868
			public override bool Contains(T item)
			{
				this.VersionCheck();
				return base.Contains(item);
			}

			// Token: 0x0600449F RID: 17567 RVA: 0x0011F678 File Offset: 0x0011D878
			internal override bool DoRemove(T item)
			{
				if (!this.IsWithinRange(item))
				{
					return false;
				}
				bool flag = this.underlying.Remove(item);
				this.VersionCheck();
				return flag;
			}

			// Token: 0x060044A0 RID: 17568 RVA: 0x0011F6A4 File Offset: 0x0011D8A4
			public override void Clear()
			{
				if (this.count == 0)
				{
					return;
				}
				List<T> toRemove = new List<T>();
				this.BreadthFirstTreeWalk(delegate(SortedSet<T>.Node n)
				{
					toRemove.Add(n.Item);
					return true;
				});
				while (toRemove.Count != 0)
				{
					this.underlying.Remove(toRemove[toRemove.Count - 1]);
					toRemove.RemoveAt(toRemove.Count - 1);
				}
				this.root = null;
				this.count = 0;
				this.version = this.underlying.version;
			}

			// Token: 0x060044A1 RID: 17569 RVA: 0x0011F748 File Offset: 0x0011D948
			internal override bool IsWithinRange(T item)
			{
				int num = (this.lBoundActive ? base.Comparer.Compare(this.min, item) : (-1));
				if (num > 0)
				{
					return false;
				}
				num = (this.uBoundActive ? base.Comparer.Compare(this.max, item) : 1);
				return num >= 0;
			}

			// Token: 0x060044A2 RID: 17570 RVA: 0x0011F7A0 File Offset: 0x0011D9A0
			internal override bool InOrderTreeWalk(TreeWalkPredicate<T> action, bool reverse)
			{
				this.VersionCheck();
				if (this.root == null)
				{
					return true;
				}
				Stack<SortedSet<T>.Node> stack = new Stack<SortedSet<T>.Node>(2 * SortedSet<T>.log2(this.count + 1));
				SortedSet<T>.Node node = this.root;
				while (node != null)
				{
					if (this.IsWithinRange(node.Item))
					{
						stack.Push(node);
						node = (reverse ? node.Right : node.Left);
					}
					else if (this.lBoundActive && base.Comparer.Compare(this.min, node.Item) > 0)
					{
						node = node.Right;
					}
					else
					{
						node = node.Left;
					}
				}
				while (stack.Count != 0)
				{
					node = stack.Pop();
					if (!action(node))
					{
						return false;
					}
					SortedSet<T>.Node node2 = (reverse ? node.Left : node.Right);
					while (node2 != null)
					{
						if (this.IsWithinRange(node2.Item))
						{
							stack.Push(node2);
							node2 = (reverse ? node2.Right : node2.Left);
						}
						else if (this.lBoundActive && base.Comparer.Compare(this.min, node2.Item) > 0)
						{
							node2 = node2.Right;
						}
						else
						{
							node2 = node2.Left;
						}
					}
				}
				return true;
			}

			// Token: 0x060044A3 RID: 17571 RVA: 0x0011F8D0 File Offset: 0x0011DAD0
			internal override bool BreadthFirstTreeWalk(TreeWalkPredicate<T> action)
			{
				this.VersionCheck();
				if (this.root == null)
				{
					return true;
				}
				List<SortedSet<T>.Node> list = new List<SortedSet<T>.Node>();
				list.Add(this.root);
				while (list.Count != 0)
				{
					SortedSet<T>.Node node = list[0];
					list.RemoveAt(0);
					if (this.IsWithinRange(node.Item) && !action(node))
					{
						return false;
					}
					if (node.Left != null && (!this.lBoundActive || base.Comparer.Compare(this.min, node.Item) < 0))
					{
						list.Add(node.Left);
					}
					if (node.Right != null && (!this.uBoundActive || base.Comparer.Compare(this.max, node.Item) > 0))
					{
						list.Add(node.Right);
					}
				}
				return true;
			}

			// Token: 0x060044A4 RID: 17572 RVA: 0x0011F9A4 File Offset: 0x0011DBA4
			internal override SortedSet<T>.Node FindNode(T item)
			{
				if (!this.IsWithinRange(item))
				{
					return null;
				}
				this.VersionCheck();
				return base.FindNode(item);
			}

			// Token: 0x060044A5 RID: 17573 RVA: 0x0011F9C0 File Offset: 0x0011DBC0
			internal override int InternalIndexOf(T item)
			{
				int num = -1;
				foreach (T t in this)
				{
					num++;
					if (base.Comparer.Compare(item, t) == 0)
					{
						return num;
					}
				}
				return -1;
			}

			// Token: 0x060044A6 RID: 17574 RVA: 0x0011FA24 File Offset: 0x0011DC24
			internal override void VersionCheck()
			{
				this.VersionCheckImpl();
			}

			// Token: 0x060044A7 RID: 17575 RVA: 0x0011FA2C File Offset: 0x0011DC2C
			private void VersionCheckImpl()
			{
				if (this.version != this.underlying.version)
				{
					this.root = this.underlying.FindRange(this.min, this.max, this.lBoundActive, this.uBoundActive);
					this.version = this.underlying.version;
					this.count = 0;
					base.InOrderTreeWalk(delegate(SortedSet<T>.Node n)
					{
						this.count++;
						return true;
					});
				}
			}

			// Token: 0x060044A8 RID: 17576 RVA: 0x0011FAA0 File Offset: 0x0011DCA0
			public override SortedSet<T> GetViewBetween(T lowerValue, T upperValue)
			{
				if (this.lBoundActive && base.Comparer.Compare(this.min, lowerValue) > 0)
				{
					throw new ArgumentOutOfRangeException("lowerValue");
				}
				if (this.uBoundActive && base.Comparer.Compare(this.max, upperValue) < 0)
				{
					throw new ArgumentOutOfRangeException("upperValue");
				}
				return (SortedSet<T>.TreeSubSet)this.underlying.GetViewBetween(lowerValue, upperValue);
			}

			// Token: 0x060044A9 RID: 17577 RVA: 0x0011FB14 File Offset: 0x0011DD14
			internal override void IntersectWithEnumerable(IEnumerable<T> other)
			{
				List<T> list = new List<T>(base.Count);
				foreach (T t in other)
				{
					if (this.Contains(t))
					{
						list.Add(t);
						base.Remove(t);
					}
				}
				this.Clear();
				base.AddAllElements(list);
			}

			// Token: 0x060044AA RID: 17578 RVA: 0x0011FB88 File Offset: 0x0011DD88
			void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
			{
				this.GetObjectData(info, context);
			}

			// Token: 0x060044AB RID: 17579 RVA: 0x0011FB94 File Offset: 0x0011DD94
			protected override void GetObjectData(SerializationInfo info, StreamingContext context)
			{
				if (info == null)
				{
					global::System.ThrowHelper.ThrowArgumentNullException(global::System.ExceptionArgument.info);
				}
				info.AddValue("Max", this.max, typeof(T));
				info.AddValue("Min", this.min, typeof(T));
				info.AddValue("lBoundActive", this.lBoundActive);
				info.AddValue("uBoundActive", this.uBoundActive);
				base.GetObjectData(info, context);
			}

			// Token: 0x060044AC RID: 17580 RVA: 0x0011FC14 File Offset: 0x0011DE14
			void IDeserializationCallback.OnDeserialization(object sender)
			{
			}

			// Token: 0x060044AD RID: 17581 RVA: 0x0011FC16 File Offset: 0x0011DE16
			protected override void OnDeserialization(object sender)
			{
				this.OnDeserializationImpl(sender);
			}

			// Token: 0x060044AE RID: 17582 RVA: 0x0011FC20 File Offset: 0x0011DE20
			private void OnDeserializationImpl(object sender)
			{
				if (this.siInfo == null)
				{
					global::System.ThrowHelper.ThrowSerializationException(global::System.ExceptionResource.Serialization_InvalidOnDeser);
				}
				this.comparer = (IComparer<T>)this.siInfo.GetValue("Comparer", typeof(IComparer<T>));
				int @int = this.siInfo.GetInt32("Count");
				this.max = (T)((object)this.siInfo.GetValue("Max", typeof(T)));
				this.min = (T)((object)this.siInfo.GetValue("Min", typeof(T)));
				this.lBoundActive = this.siInfo.GetBoolean("lBoundActive");
				this.uBoundActive = this.siInfo.GetBoolean("uBoundActive");
				this.underlying = new SortedSet<T>();
				if (@int != 0)
				{
					T[] array = (T[])this.siInfo.GetValue("Items", typeof(T[]));
					if (array == null)
					{
						global::System.ThrowHelper.ThrowSerializationException(global::System.ExceptionResource.Serialization_MissingValues);
					}
					for (int i = 0; i < array.Length; i++)
					{
						this.underlying.Add(array[i]);
					}
				}
				this.underlying.version = this.siInfo.GetInt32("Version");
				this.count = this.underlying.count;
				this.version = this.underlying.version - 1;
				this.VersionCheck();
				if (this.count != @int)
				{
					global::System.ThrowHelper.ThrowSerializationException(global::System.ExceptionResource.Serialization_MismatchedCount);
				}
				this.siInfo = null;
			}

			// Token: 0x04003536 RID: 13622
			private SortedSet<T> underlying;

			// Token: 0x04003537 RID: 13623
			private T min;

			// Token: 0x04003538 RID: 13624
			private T max;

			// Token: 0x04003539 RID: 13625
			private bool lBoundActive;

			// Token: 0x0400353A RID: 13626
			private bool uBoundActive;
		}

		// Token: 0x02000801 RID: 2049
		internal class Node
		{
			// Token: 0x060044B0 RID: 17584 RVA: 0x0011FDAB File Offset: 0x0011DFAB
			public Node(T item)
			{
				this.Item = item;
				this.IsRed = true;
			}

			// Token: 0x060044B1 RID: 17585 RVA: 0x0011FDC1 File Offset: 0x0011DFC1
			public Node(T item, bool isRed)
			{
				this.Item = item;
				this.IsRed = isRed;
			}

			// Token: 0x0400353B RID: 13627
			public bool IsRed;

			// Token: 0x0400353C RID: 13628
			public T Item;

			// Token: 0x0400353D RID: 13629
			public SortedSet<T>.Node Left;

			// Token: 0x0400353E RID: 13630
			public SortedSet<T>.Node Right;
		}

		// Token: 0x02000802 RID: 2050
		[global::__DynamicallyInvokable]
		[Serializable]
		public struct Enumerator : IEnumerator<T>, IDisposable, IEnumerator, ISerializable, IDeserializationCallback
		{
			// Token: 0x060044B2 RID: 17586 RVA: 0x0011FDD8 File Offset: 0x0011DFD8
			internal Enumerator(SortedSet<T> set)
			{
				this.tree = set;
				this.tree.VersionCheck();
				this.version = this.tree.version;
				this.stack = new Stack<SortedSet<T>.Node>(2 * SortedSet<T>.log2(set.Count + 1));
				this.current = null;
				this.reverse = false;
				this.siInfo = null;
				this.Intialize();
			}

			// Token: 0x060044B3 RID: 17587 RVA: 0x0011FE40 File Offset: 0x0011E040
			internal Enumerator(SortedSet<T> set, bool reverse)
			{
				this.tree = set;
				this.tree.VersionCheck();
				this.version = this.tree.version;
				this.stack = new Stack<SortedSet<T>.Node>(2 * SortedSet<T>.log2(set.Count + 1));
				this.current = null;
				this.reverse = reverse;
				this.siInfo = null;
				this.Intialize();
			}

			// Token: 0x060044B4 RID: 17588 RVA: 0x0011FEA5 File Offset: 0x0011E0A5
			private Enumerator(SerializationInfo info, StreamingContext context)
			{
				this.tree = null;
				this.version = -1;
				this.current = null;
				this.reverse = false;
				this.stack = null;
				this.siInfo = info;
			}

			// Token: 0x060044B5 RID: 17589 RVA: 0x0011FED1 File Offset: 0x0011E0D1
			void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
			{
				this.GetObjectData(info, context);
			}

			// Token: 0x060044B6 RID: 17590 RVA: 0x0011FEDC File Offset: 0x0011E0DC
			private void GetObjectData(SerializationInfo info, StreamingContext context)
			{
				if (info == null)
				{
					global::System.ThrowHelper.ThrowArgumentNullException(global::System.ExceptionArgument.info);
				}
				info.AddValue("Tree", this.tree, typeof(SortedSet<T>));
				info.AddValue("EnumVersion", this.version);
				info.AddValue("Reverse", this.reverse);
				info.AddValue("EnumStarted", !this.NotStartedOrEnded);
				info.AddValue("Item", (this.current == null) ? SortedSet<T>.Enumerator.dummyNode.Item : this.current.Item, typeof(T));
			}

			// Token: 0x060044B7 RID: 17591 RVA: 0x0011FF7C File Offset: 0x0011E17C
			void IDeserializationCallback.OnDeserialization(object sender)
			{
				this.OnDeserialization(sender);
			}

			// Token: 0x060044B8 RID: 17592 RVA: 0x0011FF88 File Offset: 0x0011E188
			private void OnDeserialization(object sender)
			{
				if (this.siInfo == null)
				{
					global::System.ThrowHelper.ThrowSerializationException(global::System.ExceptionResource.Serialization_InvalidOnDeser);
				}
				this.tree = (SortedSet<T>)this.siInfo.GetValue("Tree", typeof(SortedSet<T>));
				this.version = this.siInfo.GetInt32("EnumVersion");
				this.reverse = this.siInfo.GetBoolean("Reverse");
				bool boolean = this.siInfo.GetBoolean("EnumStarted");
				this.stack = new Stack<SortedSet<T>.Node>(2 * SortedSet<T>.log2(this.tree.Count + 1));
				this.current = null;
				if (boolean)
				{
					T t = (T)((object)this.siInfo.GetValue("Item", typeof(T)));
					this.Intialize();
					while (this.MoveNext() && this.tree.Comparer.Compare(this.Current, t) != 0)
					{
					}
				}
			}

			// Token: 0x060044B9 RID: 17593 RVA: 0x00120078 File Offset: 0x0011E278
			private void Intialize()
			{
				this.current = null;
				SortedSet<T>.Node node = this.tree.root;
				while (node != null)
				{
					SortedSet<T>.Node node2 = (this.reverse ? node.Right : node.Left);
					SortedSet<T>.Node node3 = (this.reverse ? node.Left : node.Right);
					if (this.tree.IsWithinRange(node.Item))
					{
						this.stack.Push(node);
						node = node2;
					}
					else if (node2 == null || !this.tree.IsWithinRange(node2.Item))
					{
						node = node3;
					}
					else
					{
						node = node2;
					}
				}
			}

			// Token: 0x060044BA RID: 17594 RVA: 0x00120110 File Offset: 0x0011E310
			[global::__DynamicallyInvokable]
			public bool MoveNext()
			{
				this.tree.VersionCheck();
				if (this.version != this.tree.version)
				{
					global::System.ThrowHelper.ThrowInvalidOperationException(global::System.ExceptionResource.InvalidOperation_EnumFailedVersion);
				}
				if (this.stack.Count == 0)
				{
					this.current = null;
					return false;
				}
				this.current = this.stack.Pop();
				SortedSet<T>.Node node = (this.reverse ? this.current.Left : this.current.Right);
				while (node != null)
				{
					SortedSet<T>.Node node2 = (this.reverse ? node.Right : node.Left);
					SortedSet<T>.Node node3 = (this.reverse ? node.Left : node.Right);
					if (this.tree.IsWithinRange(node.Item))
					{
						this.stack.Push(node);
						node = node2;
					}
					else if (node3 == null || !this.tree.IsWithinRange(node3.Item))
					{
						node = node2;
					}
					else
					{
						node = node3;
					}
				}
				return true;
			}

			// Token: 0x060044BB RID: 17595 RVA: 0x00120201 File Offset: 0x0011E401
			[global::__DynamicallyInvokable]
			public void Dispose()
			{
			}

			// Token: 0x17000F98 RID: 3992
			// (get) Token: 0x060044BC RID: 17596 RVA: 0x00120204 File Offset: 0x0011E404
			[global::__DynamicallyInvokable]
			public T Current
			{
				[global::__DynamicallyInvokable]
				get
				{
					if (this.current != null)
					{
						return this.current.Item;
					}
					return default(T);
				}
			}

			// Token: 0x17000F99 RID: 3993
			// (get) Token: 0x060044BD RID: 17597 RVA: 0x0012022E File Offset: 0x0011E42E
			[global::__DynamicallyInvokable]
			object IEnumerator.Current
			{
				[global::__DynamicallyInvokable]
				get
				{
					if (this.current == null)
					{
						global::System.ThrowHelper.ThrowInvalidOperationException(global::System.ExceptionResource.InvalidOperation_EnumOpCantHappen);
					}
					return this.current.Item;
				}
			}

			// Token: 0x17000F9A RID: 3994
			// (get) Token: 0x060044BE RID: 17598 RVA: 0x0012024F File Offset: 0x0011E44F
			internal bool NotStartedOrEnded
			{
				get
				{
					return this.current == null;
				}
			}

			// Token: 0x060044BF RID: 17599 RVA: 0x0012025A File Offset: 0x0011E45A
			internal void Reset()
			{
				if (this.version != this.tree.version)
				{
					global::System.ThrowHelper.ThrowInvalidOperationException(global::System.ExceptionResource.InvalidOperation_EnumFailedVersion);
				}
				this.stack.Clear();
				this.Intialize();
			}

			// Token: 0x060044C0 RID: 17600 RVA: 0x00120287 File Offset: 0x0011E487
			[global::__DynamicallyInvokable]
			void IEnumerator.Reset()
			{
				this.Reset();
			}

			// Token: 0x0400353F RID: 13631
			private SortedSet<T> tree;

			// Token: 0x04003540 RID: 13632
			private int version;

			// Token: 0x04003541 RID: 13633
			private Stack<SortedSet<T>.Node> stack;

			// Token: 0x04003542 RID: 13634
			private SortedSet<T>.Node current;

			// Token: 0x04003543 RID: 13635
			private static SortedSet<T>.Node dummyNode = new SortedSet<T>.Node(default(T));

			// Token: 0x04003544 RID: 13636
			private bool reverse;

			// Token: 0x04003545 RID: 13637
			private SerializationInfo siInfo;
		}

		// Token: 0x02000803 RID: 2051
		internal struct ElementCount
		{
			// Token: 0x04003546 RID: 13638
			internal int uniqueCount;

			// Token: 0x04003547 RID: 13639
			internal int unfoundCount;
		}
	}
}
