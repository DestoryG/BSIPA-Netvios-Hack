using System;
using System.Collections.Generic;
using SemVer;

namespace IPA.Utilities
{
	/// <summary>
	/// A type that wraps <see cref="T:SemVer.Version" /> so that the string of the version is stored when the string is 
	/// not a valid <see cref="T:SemVer.Version" />.
	/// </summary>
	// Token: 0x02000016 RID: 22
	public class AlmostVersion : IComparable<AlmostVersion>, IComparable<global::SemVer.Version>
	{
		/// <summary>
		/// Creates a new <see cref="T:IPA.Utilities.AlmostVersion" /> with the version string provided in <paramref name="vertext" />.
		/// </summary>
		/// <param name="vertext">the version string to store</param>
		// Token: 0x0600004F RID: 79 RVA: 0x00003134 File Offset: 0x00001334
		public AlmostVersion(string vertext)
		{
			if (!this.TryParseFrom(vertext, AlmostVersion.StoredAs.SemVer))
			{
				this.TryParseFrom(vertext, AlmostVersion.StoredAs.String);
			}
		}

		/// <summary>
		/// Creates an <see cref="T:IPA.Utilities.AlmostVersion" /> from the <see cref="T:SemVer.Version" /> provided in <paramref name="ver" />.
		/// </summary>
		/// <param name="ver">the <see cref="T:SemVer.Version" /> to store</param>
		// Token: 0x06000050 RID: 80 RVA: 0x0000314F File Offset: 0x0000134F
		public AlmostVersion(global::SemVer.Version ver)
		{
			this.SemverValue = ver;
			this.StorageMode = AlmostVersion.StoredAs.SemVer;
		}

		/// <summary>
		/// Creates an <see cref="T:IPA.Utilities.AlmostVersion" /> from the version string in <paramref name="vertext" /> stored using 
		/// the storage mode specified in <paramref name="mode" />.
		/// </summary>
		/// <param name="vertext">the text to parse as an <see cref="T:IPA.Utilities.AlmostVersion" /></param>
		/// <param name="mode">the storage mode to store the version in</param>
		// Token: 0x06000051 RID: 81 RVA: 0x00003165 File Offset: 0x00001365
		public AlmostVersion(string vertext, AlmostVersion.StoredAs mode)
		{
			if (!this.TryParseFrom(vertext, mode))
			{
				throw new ArgumentException(string.Format("{0} could not be stored as {1}!", "vertext", mode));
			}
		}

		/// <summary>
		/// Creates a new <see cref="T:IPA.Utilities.AlmostVersion" /> from the version string in <paramref name="vertext" /> stored the
		/// same way as the <see cref="T:IPA.Utilities.AlmostVersion" /> passed in <paramref name="copyMode" />.
		/// </summary>
		/// <param name="vertext">the text to parse as an <see cref="T:IPA.Utilities.AlmostVersion" /></param>
		/// <param name="copyMode">an <see cref="T:IPA.Utilities.AlmostVersion" /> to copy the storage mode of</param>
		// Token: 0x06000052 RID: 82 RVA: 0x00003192 File Offset: 0x00001392
		public AlmostVersion(string vertext, AlmostVersion copyMode)
		{
			if (copyMode == null)
			{
				throw new ArgumentNullException("copyMode");
			}
			if (!this.TryParseFrom(vertext, copyMode.StorageMode))
			{
				this.TryParseFrom(vertext, AlmostVersion.StoredAs.String);
			}
		}

		// Token: 0x06000053 RID: 83 RVA: 0x000031C8 File Offset: 0x000013C8
		private bool TryParseFrom(string str, AlmostVersion.StoredAs mode)
		{
			if (mode == AlmostVersion.StoredAs.SemVer)
			{
				try
				{
					this.SemverValue = new global::SemVer.Version(str, true);
					this.StorageMode = AlmostVersion.StoredAs.SemVer;
					return true;
				}
				catch
				{
					return false;
				}
			}
			this.StringValue = str;
			this.StorageMode = AlmostVersion.StoredAs.String;
			return true;
		}

		/// <summary>
		/// The value of the <see cref="T:IPA.Utilities.AlmostVersion" /> if it was stored as a <see cref="T:System.String" />.
		/// </summary>
		/// <value>the stored value as a <see cref="T:System.String" />, or <see langword="null" /> if not stored as a string.</value>
		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000054 RID: 84 RVA: 0x00003218 File Offset: 0x00001418
		// (set) Token: 0x06000055 RID: 85 RVA: 0x00003220 File Offset: 0x00001420
		public string StringValue { get; private set; }

		/// <summary>
		/// The value of the <see cref="T:IPA.Utilities.AlmostVersion" /> if it was stored as a <see cref="T:SemVer.Version" />.
		/// </summary>
		/// <value>the stored value as a <see cref="T:SemVer.Version" />, or <see langword="null" /> if not stored as a version.</value>
		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000056 RID: 86 RVA: 0x00003229 File Offset: 0x00001429
		// (set) Token: 0x06000057 RID: 87 RVA: 0x00003231 File Offset: 0x00001431
		public global::SemVer.Version SemverValue { get; private set; }

		/// <summary>
		/// The way the value is stored, whether it be as a <see cref="T:SemVer.Version" /> or a <see cref="T:System.String" />.
		/// </summary>
		/// <value>the storage mode used to store this value</value>
		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000058 RID: 88 RVA: 0x0000323A File Offset: 0x0000143A
		// (set) Token: 0x06000059 RID: 89 RVA: 0x00003242 File Offset: 0x00001442
		public AlmostVersion.StoredAs StorageMode { get; private set; }

		/// <summary>
		/// Gets a string representation of the current version. If the value is stored as a string, this returns it. If it is
		/// stored as a <see cref="T:SemVer.Version" />, it is equivalent to calling <see cref="M:SemVer.Version.ToString" />.
		/// </summary>
		/// <returns>a string representation of the current version</returns>
		/// <seealso cref="M:System.Object.ToString" />
		// Token: 0x0600005A RID: 90 RVA: 0x0000324B File Offset: 0x0000144B
		public override string ToString()
		{
			if (this.StorageMode != AlmostVersion.StoredAs.SemVer)
			{
				return this.StringValue;
			}
			return this.SemverValue.ToString();
		}

		/// <summary>
		/// Compares <see langword="this" /> to the <see cref="T:IPA.Utilities.AlmostVersion" /> in <paramref name="other" /> using <see cref="M:SemVer.Version.CompareTo(SemVer.Version)" />
		/// or <see cref="M:System.String.CompareTo(System.String)" />, depending on the current store.
		/// </summary>
		/// <remarks>
		/// The storage methods of the two objects must be the same, or this will throw an <see cref="T:System.InvalidOperationException" />.
		/// </remarks>
		/// <param name="other">the <see cref="T:IPA.Utilities.AlmostVersion" /> to compare to</param>
		/// <returns>less than 0 if <paramref name="other" /> is considered bigger than <see langword="this" />, 0 if equal, and greater than zero if smaller</returns>
		/// <seealso cref="M:IPA.Utilities.AlmostVersion.CompareTo(SemVer.Version)" />
		// Token: 0x0600005B RID: 91 RVA: 0x00003268 File Offset: 0x00001468
		public int CompareTo(AlmostVersion other)
		{
			if (other == null)
			{
				return -1;
			}
			if (this.StorageMode != other.StorageMode)
			{
				throw new InvalidOperationException("Cannot compare AlmostVersions with different stores!");
			}
			if (this.StorageMode == AlmostVersion.StoredAs.SemVer)
			{
				return this.SemverValue.CompareTo(other.SemverValue);
			}
			return this.StringValue.CompareTo(other.StringValue);
		}

		/// <summary>
		/// Compares <see langword="this" /> to the <see cref="T:SemVer.Version" /> in <paramref name="other" /> using <see cref="M:SemVer.Version.CompareTo(SemVer.Version)" />.
		/// </summary>
		/// <remarks>
		/// The storage method of <see langword="this" /> must be <see cref="F:IPA.Utilities.AlmostVersion.StoredAs.SemVer" />, else an <see cref="T:System.InvalidOperationException" /> will
		/// be thrown.
		/// </remarks>
		/// <param name="other">the <see cref="T:SemVer.Version" /> to compare to</param>
		/// <returns>less than 0 if <paramref name="other" /> is considered bigger than <see langword="this" />, 0 if equal, and greater than zero if smaller</returns>
		/// <seealso cref="M:IPA.Utilities.AlmostVersion.CompareTo(IPA.Utilities.AlmostVersion)" />
		// Token: 0x0600005C RID: 92 RVA: 0x000032C4 File Offset: 0x000014C4
		public int CompareTo(global::SemVer.Version other)
		{
			if (this.StorageMode != AlmostVersion.StoredAs.SemVer)
			{
				throw new InvalidOperationException("Cannot compare a SemVer version with an AlmostVersion stored as a string!");
			}
			return this.SemverValue.CompareTo(other);
		}

		/// <summary>
		/// Performs a strict equality check between <see langword="this" /> and <paramref name="obj" />.
		/// </summary>
		/// <remarks>
		/// This may return <see langword="false" /> where <see cref="M:IPA.Utilities.AlmostVersion.op_Equality(IPA.Utilities.AlmostVersion,IPA.Utilities.AlmostVersion)" /> returns <see langword="true" />
		/// </remarks>
		/// <param name="obj">the object to compare to</param>
		/// <returns><see langword="true" /> if they are equal, <see langword="false" /> otherwise</returns>
		/// <seealso cref="M:System.Object.Equals(System.Object)" />
		// Token: 0x0600005D RID: 93 RVA: 0x000032E8 File Offset: 0x000014E8
		public override bool Equals(object obj)
		{
			AlmostVersion version = obj as AlmostVersion;
			return version != null && this.SemverValue == version.SemverValue && this.StringValue == version.StringValue && this.StorageMode == version.StorageMode;
		}

		/// <summary>
		/// Default generated hash code function generated by VS.
		/// </summary>
		/// <returns>a value unique to each object, except those that are considered equal by <see cref="M:IPA.Utilities.AlmostVersion.Equals(System.Object)" /></returns>
		/// <seealso cref="M:System.Object.GetHashCode" />
		// Token: 0x0600005E RID: 94 RVA: 0x00003338 File Offset: 0x00001538
		public override int GetHashCode()
		{
			return ((-126402897 * -1521134295 + EqualityComparer<global::SemVer.Version>.Default.GetHashCode(this.SemverValue)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.StringValue)) * -1521134295 + this.StorageMode.GetHashCode();
		}

		/// <summary>
		/// Compares two versions, only taking into account the numeric part of the version if they are stored as <see cref="T:SemVer.Version" />s,
		/// or strict equality if they are stored as <see cref="T:System.String" />s.
		/// </summary>
		/// <remarks>
		/// This is a looser equality than <see cref="M:IPA.Utilities.AlmostVersion.Equals(System.Object)" />, meaning that this may return <see langword="true" /> where <see cref="M:IPA.Utilities.AlmostVersion.Equals(System.Object)" />
		/// does not.
		/// </remarks>
		/// <param name="l">the first value to compare</param>
		/// <param name="r">the second value to compare</param>
		/// <returns><see langword="true" /> if they are mostly equal, <see langword="false" /> otherwise</returns>
		/// <seealso cref="M:IPA.Utilities.AlmostVersion.Equals(System.Object)" />
		// Token: 0x0600005F RID: 95 RVA: 0x00003394 File Offset: 0x00001594
		public static bool operator ==(AlmostVersion l, AlmostVersion r)
		{
			if (l == null && r == null)
			{
				return true;
			}
			if (l == null || r == null)
			{
				return false;
			}
			if (l.StorageMode != r.StorageMode)
			{
				return false;
			}
			if (l.StorageMode == AlmostVersion.StoredAs.SemVer)
			{
				return Utils.VersionCompareNoPrerelease(l.SemverValue, r.SemverValue) == 0;
			}
			return l.StringValue == r.StringValue;
		}

		/// <summary>
		/// The opposite of <see cref="M:IPA.Utilities.AlmostVersion.op_Equality(IPA.Utilities.AlmostVersion,IPA.Utilities.AlmostVersion)" />. Equivalent to <c>!(l == r)</c>.
		/// </summary>
		/// <param name="l">the first value to compare</param>
		/// <param name="r">the second value to compare</param>
		/// <returns><see langword="true" /> if they are not mostly equal, <see langword="false" /> otherwise</returns>
		/// <seealso cref="M:IPA.Utilities.AlmostVersion.op_Equality(IPA.Utilities.AlmostVersion,IPA.Utilities.AlmostVersion)" />
		// Token: 0x06000060 RID: 96 RVA: 0x000033EF File Offset: 0x000015EF
		public static bool operator !=(AlmostVersion l, AlmostVersion r)
		{
			return !(l == r);
		}

		/// <summary>
		/// Implicitly converts a <see cref="T:SemVer.Version" /> to <see cref="T:IPA.Utilities.AlmostVersion" /> using <see cref="M:IPA.Utilities.AlmostVersion.#ctor(SemVer.Version)" />.
		/// </summary>
		/// <param name="ver">the <see cref="T:SemVer.Version" /> to convert</param>
		/// <seealso cref="M:IPA.Utilities.AlmostVersion.#ctor(SemVer.Version)" />
		// Token: 0x06000061 RID: 97 RVA: 0x000033FB File Offset: 0x000015FB
		public static implicit operator AlmostVersion(global::SemVer.Version ver)
		{
			return new AlmostVersion(ver);
		}

		/// <summary>
		/// Implicitly converts an <see cref="T:IPA.Utilities.AlmostVersion" /> to <see cref="T:SemVer.Version" />, if applicable, using <see cref="P:IPA.Utilities.AlmostVersion.SemverValue" />.
		/// If not applicable, returns <see langword="null" />
		/// </summary>
		/// <param name="av">the <see cref="T:IPA.Utilities.AlmostVersion" /> to convert to a <see cref="T:SemVer.Version" /></param>
		/// <seealso cref="P:IPA.Utilities.AlmostVersion.SemverValue" />
		// Token: 0x06000062 RID: 98 RVA: 0x00003403 File Offset: 0x00001603
		public static implicit operator global::SemVer.Version(AlmostVersion av)
		{
			if (av == null)
			{
				return null;
			}
			return av.SemverValue;
		}

		/// <summary>
		/// Represents a storage type of either parsed <see cref="T:SemVer.Version" /> object or raw <see cref="F:IPA.Utilities.AlmostVersion.StoredAs.String" />.
		/// </summary>
		// Token: 0x020000BC RID: 188
		public enum StoredAs
		{
			/// <summary>
			/// The version was stored as a <see cref="T:SemVer.Version" />.
			/// </summary>
			// Token: 0x040001A3 RID: 419
			SemVer,
			/// <summary>
			/// The version was stored as a <see cref="F:IPA.Utilities.AlmostVersion.StoredAs.String" />.
			/// </summary>
			// Token: 0x040001A4 RID: 420
			String
		}
	}
}
