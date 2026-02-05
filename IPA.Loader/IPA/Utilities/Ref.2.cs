using System;
using System.Diagnostics;

namespace IPA.Utilities
{
	/// <summary>
	/// A class to store a reference for passing to methods which cannot take ref parameters.
	/// </summary>
	/// <typeparam name="T">the type of the value</typeparam>
	// Token: 0x0200001B RID: 27
	public class Ref<T> : IComparable<T>, IComparable<Ref<T>>
	{
		/// <summary>
		/// The value of the reference
		/// </summary>
		/// <value>the value wrapped by this <see cref="T:IPA.Utilities.Ref`1" /></value>
		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000077 RID: 119 RVA: 0x000036A0 File Offset: 0x000018A0
		// (set) Token: 0x06000078 RID: 120 RVA: 0x000036B7 File Offset: 0x000018B7
		public T Value
		{
			get
			{
				if (this.Error != null)
				{
					throw this.Error;
				}
				return this._value;
			}
			set
			{
				this._value = value;
			}
		}

		/// <summary>
		/// An exception that was generated while creating the value.
		/// </summary>
		/// <value>the error held in this <see cref="T:IPA.Utilities.Ref`1" /></value>
		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000079 RID: 121 RVA: 0x000036C0 File Offset: 0x000018C0
		// (set) Token: 0x0600007A RID: 122 RVA: 0x000036C8 File Offset: 0x000018C8
		public Exception Error
		{
			get
			{
				return this._error;
			}
			set
			{
				value.SetStackTrace(new StackTrace(1));
				this._error = value;
			}
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="reference">the initial value of the reference</param>
		// Token: 0x0600007B RID: 123 RVA: 0x000036DE File Offset: 0x000018DE
		public Ref(T reference)
		{
			this._value = reference;
		}

		/// <summary>
		/// Converts to referenced type, returning the stored reference.
		/// </summary>
		/// <param name="self">the object to be de-referenced</param>
		/// <returns>the value referenced by the object</returns>
		// Token: 0x0600007C RID: 124 RVA: 0x000036ED File Offset: 0x000018ED
		public static implicit operator T(Ref<T> self)
		{
			return self.Value;
		}

		/// <summary>
		/// Converts a value T to a reference to that object. Will overwrite the reference in the left hand expression if there is one.
		/// </summary>
		/// <param name="toConvert">the value to wrap in the Ref</param>
		/// <returns>the Ref wrapping the value</returns>
		// Token: 0x0600007D RID: 125 RVA: 0x000036F5 File Offset: 0x000018F5
		public static implicit operator Ref<T>(T toConvert)
		{
			return new Ref<T>(toConvert);
		}

		/// <summary>
		/// Throws error if one was set.
		/// </summary>
		// Token: 0x0600007E RID: 126 RVA: 0x000036FD File Offset: 0x000018FD
		public void Verify()
		{
			if (this.Error != null)
			{
				throw this.Error;
			}
		}

		/// <summary>
		/// Compares the wrapped object to the other object.
		/// </summary>
		/// <param name="other">the object to compare to</param>
		/// <returns>the value of the comparison</returns>
		// Token: 0x0600007F RID: 127 RVA: 0x00003710 File Offset: 0x00001910
		public int CompareTo(T other)
		{
			IComparable<T> compare = this.Value as IComparable<T>;
			if (compare != null)
			{
				return compare.CompareTo(other);
			}
			if (!object.Equals(this.Value, other))
			{
				return -1;
			}
			return 0;
		}

		/// <summary>
		/// Compares the wrapped object to the other wrapped object.
		/// </summary>
		/// <param name="other">the wrapped object to compare to</param>
		/// <returns>the value of the comparison</returns>
		// Token: 0x06000080 RID: 128 RVA: 0x00003754 File Offset: 0x00001954
		public int CompareTo(Ref<T> other)
		{
			return this.CompareTo(other.Value);
		}

		// Token: 0x04000026 RID: 38
		private T _value;

		// Token: 0x04000027 RID: 39
		private Exception _error;
	}
}
