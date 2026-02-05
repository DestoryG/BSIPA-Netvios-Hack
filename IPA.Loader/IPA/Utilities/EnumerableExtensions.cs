using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace IPA.Utilities
{
	/// <summary>
	/// Extensions for <see cref="T:System.Collections.Generic.IEnumerable`1" /> that don't currently exist in <c>System.Linq</c>.
	/// </summary>
	// Token: 0x02000019 RID: 25
	public static class EnumerableExtensions
	{
		/// <summary>
		/// Adds a value to the beginning of the sequence.
		/// </summary>
		/// <typeparam name="T">the type of the elements of <paramref name="seq" /></typeparam>
		/// <param name="seq">a sequence of values</param>
		/// <param name="prep">the value to prepend to <paramref name="seq" /></param>
		/// <returns>a new sequence beginning with <paramref name="prep" /></returns>
		// Token: 0x06000070 RID: 112 RVA: 0x000035AD File Offset: 0x000017AD
		public static IEnumerable<T> Prepend<T>(this IEnumerable<T> seq, T prep)
		{
			return new EnumerableExtensions.PrependEnumerable<T>(seq, prep);
		}

		/// <summary>
		/// Adds a value to the end of the sequence.
		/// </summary>
		/// <typeparam name="T">the type of the elements of <paramref name="seq" /></typeparam>
		/// <param name="seq">a sequence of values</param>
		/// <param name="app">the value to append to <paramref name="seq" /></param>
		/// <returns>a new sequence ending with <paramref name="app" /></returns>
		// Token: 0x06000071 RID: 113 RVA: 0x000035B6 File Offset: 0x000017B6
		public static IEnumerable<T> Append<T>(this IEnumerable<T> seq, T app)
		{
			return new EnumerableExtensions.AppendEnumerable<T>(seq, app);
		}

		/// <summary>
		/// LINQ-style extension method that filters <see langword="null" /> elements out of an enumeration.
		/// </summary>
		/// <typeparam name="T">the type of the enumeration</typeparam>
		/// <param name="self">the enumeration to filter</param>
		/// <returns>a filtered enumerable</returns>
		// Token: 0x06000072 RID: 114 RVA: 0x000035BF File Offset: 0x000017BF
		public static IEnumerable<T> NonNull<T>(this IEnumerable<T> self) where T : class
		{
			return self.Where((T o) => o != null);
		}

		/// <summary>
		/// LINQ-style extension method that filters <see langword="null" /> elements out of an enumeration based on a converter.
		/// </summary>
		/// <typeparam name="T">the type of the enumeration</typeparam>
		/// <typeparam name="U">the type to compare to null</typeparam>
		/// <param name="self">the enumeration to filter</param>
		/// <param name="pred">the predicate to select for filtering</param>
		/// <returns>a filtered enumerable</returns>
		// Token: 0x06000073 RID: 115 RVA: 0x000035E8 File Offset: 0x000017E8
		public static IEnumerable<T> NonNull<T, U>(this IEnumerable<T> self, Func<T, U> pred) where U : class
		{
			return self.Where((T o) => pred(o) != null);
		}

		/// <summary>
		/// LINQ-style extension method that filters <see langword="null" /> elements from an enumeration of nullable types.
		/// </summary>
		/// <typeparam name="T">the underlying type of the nullable enumeration</typeparam>
		/// <param name="self">the enumeration to filter</param>
		/// <returns>a filtered enumerable</returns>
		// Token: 0x06000074 RID: 116 RVA: 0x00003614 File Offset: 0x00001814
		public static IEnumerable<T> NonNull<T>(this IEnumerable<T?> self) where T : struct
		{
			return from o in self
				where o != null
				select o.Value;
		}

		/// <summary>
		/// LINQ-style extension method that filters <see langword="null" /> elements out of an enumeration based on a converter to a nullable type.
		/// </summary>
		/// <typeparam name="T">the type of the enumeration</typeparam>
		/// <typeparam name="U">the type of the predicate's resulting nullable</typeparam>
		/// <param name="self">the enumeration to filter</param>
		/// <param name="pred">the predicate to select for filtering</param>
		/// <returns>a filtered enumerable</returns>
		// Token: 0x06000075 RID: 117 RVA: 0x0000366C File Offset: 0x0000186C
		public static IEnumerable<T> NonNull<T, U>(this IEnumerable<T> self, Func<T, U?> pred) where U : struct
		{
			return self.Where((T o) => pred(o) != null);
		}

		// Token: 0x020000C0 RID: 192
		private sealed class PrependEnumerable<T> : IEnumerable<T>, IEnumerable
		{
			// Token: 0x06000495 RID: 1173 RVA: 0x00015C8A File Offset: 0x00013E8A
			public PrependEnumerable(IEnumerable<T> rest, T first)
			{
				this.rest = rest;
				this.first = first;
			}

			// Token: 0x06000496 RID: 1174 RVA: 0x00015CA0 File Offset: 0x00013EA0
			public EnumerableExtensions.PrependEnumerable<T>.PrependEnumerator GetEnumerator()
			{
				return new EnumerableExtensions.PrependEnumerable<T>.PrependEnumerator(this);
			}

			// Token: 0x06000497 RID: 1175 RVA: 0x00015CA8 File Offset: 0x00013EA8
			IEnumerator<T> IEnumerable<T>.GetEnumerator()
			{
				return new EnumerableExtensions.PrependEnumerable<T>.PrependEnumerator(this);
			}

			// Token: 0x06000498 RID: 1176 RVA: 0x00015CB5 File Offset: 0x00013EB5
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			// Token: 0x040001A8 RID: 424
			private readonly IEnumerable<T> rest;

			// Token: 0x040001A9 RID: 425
			private readonly T first;

			// Token: 0x0200015E RID: 350
			public struct PrependEnumerator : IEnumerator<T>, IDisposable, IEnumerator
			{
				// Token: 0x060006B6 RID: 1718 RVA: 0x00018860 File Offset: 0x00016A60
				internal PrependEnumerator(EnumerableExtensions.PrependEnumerable<T> enumerable)
				{
					this.enumerable = enumerable;
					this.restEnum = enumerable.rest.GetEnumerator();
					this.state = 0;
					this.Current = default(T);
				}

				// Token: 0x170000F6 RID: 246
				// (get) Token: 0x060006B7 RID: 1719 RVA: 0x0001889B File Offset: 0x00016A9B
				// (set) Token: 0x060006B8 RID: 1720 RVA: 0x000188A3 File Offset: 0x00016AA3
				public T Current { readonly get; private set; }

				// Token: 0x170000F7 RID: 247
				// (get) Token: 0x060006B9 RID: 1721 RVA: 0x000188AC File Offset: 0x00016AAC
				object IEnumerator.Current
				{
					get
					{
						return this.Current;
					}
				}

				// Token: 0x060006BA RID: 1722 RVA: 0x000188B9 File Offset: 0x00016AB9
				public void Dispose()
				{
					this.restEnum.Dispose();
				}

				// Token: 0x060006BB RID: 1723 RVA: 0x000188C8 File Offset: 0x00016AC8
				public bool MoveNext()
				{
					switch (this.state)
					{
					case 0:
						this.Current = this.enumerable.first;
						this.state++;
						return true;
					case 1:
						if (!this.restEnum.MoveNext())
						{
							this.state = 2;
							return false;
						}
						this.Current = this.restEnum.Current;
						return true;
					}
					return false;
				}

				// Token: 0x060006BC RID: 1724 RVA: 0x0001893B File Offset: 0x00016B3B
				public void Reset()
				{
					this.restEnum.Reset();
					this.state = 0;
				}

				// Token: 0x04000466 RID: 1126
				private readonly IEnumerator<T> restEnum;

				// Token: 0x04000467 RID: 1127
				private readonly EnumerableExtensions.PrependEnumerable<T> enumerable;

				// Token: 0x04000468 RID: 1128
				private int state;
			}
		}

		// Token: 0x020000C1 RID: 193
		private sealed class AppendEnumerable<T> : IEnumerable<T>, IEnumerable
		{
			// Token: 0x06000499 RID: 1177 RVA: 0x00015CC2 File Offset: 0x00013EC2
			public AppendEnumerable(IEnumerable<T> rest, T last)
			{
				this.rest = rest;
				this.last = last;
			}

			// Token: 0x0600049A RID: 1178 RVA: 0x00015CD8 File Offset: 0x00013ED8
			public EnumerableExtensions.AppendEnumerable<T>.AppendEnumerator GetEnumerator()
			{
				return new EnumerableExtensions.AppendEnumerable<T>.AppendEnumerator(this);
			}

			// Token: 0x0600049B RID: 1179 RVA: 0x00015CE0 File Offset: 0x00013EE0
			IEnumerator<T> IEnumerable<T>.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			// Token: 0x0600049C RID: 1180 RVA: 0x00015CED File Offset: 0x00013EED
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			// Token: 0x040001AA RID: 426
			private readonly IEnumerable<T> rest;

			// Token: 0x040001AB RID: 427
			private readonly T last;

			// Token: 0x0200015F RID: 351
			public struct AppendEnumerator : IEnumerator<T>, IDisposable, IEnumerator
			{
				// Token: 0x060006BD RID: 1725 RVA: 0x00018950 File Offset: 0x00016B50
				internal AppendEnumerator(EnumerableExtensions.AppendEnumerable<T> enumerable)
				{
					this.enumerable = enumerable;
					this.restEnum = enumerable.rest.GetEnumerator();
					this.state = 0;
					this.Current = default(T);
				}

				// Token: 0x170000F8 RID: 248
				// (get) Token: 0x060006BE RID: 1726 RVA: 0x0001898B File Offset: 0x00016B8B
				// (set) Token: 0x060006BF RID: 1727 RVA: 0x00018993 File Offset: 0x00016B93
				public T Current { readonly get; private set; }

				// Token: 0x170000F9 RID: 249
				// (get) Token: 0x060006C0 RID: 1728 RVA: 0x0001899C File Offset: 0x00016B9C
				object IEnumerator.Current
				{
					get
					{
						return this.Current;
					}
				}

				// Token: 0x060006C1 RID: 1729 RVA: 0x000189A9 File Offset: 0x00016BA9
				public void Dispose()
				{
					this.restEnum.Dispose();
				}

				// Token: 0x060006C2 RID: 1730 RVA: 0x000189B8 File Offset: 0x00016BB8
				public bool MoveNext()
				{
					switch (this.state)
					{
					case 0:
						if (this.restEnum.MoveNext())
						{
							this.Current = this.restEnum.Current;
							return true;
						}
						this.state = 1;
						break;
					case 1:
						break;
					case 2:
						return false;
					default:
						return false;
					}
					this.Current = this.enumerable.last;
					this.state++;
					return true;
				}

				// Token: 0x060006C3 RID: 1731 RVA: 0x00018A2B File Offset: 0x00016C2B
				public void Reset()
				{
					this.restEnum.Reset();
					this.state = 0;
				}

				// Token: 0x0400046A RID: 1130
				private readonly IEnumerator<T> restEnum;

				// Token: 0x0400046B RID: 1131
				private readonly EnumerableExtensions.AppendEnumerable<T> enumerable;

				// Token: 0x0400046C RID: 1132
				private int state;
			}
		}
	}
}
