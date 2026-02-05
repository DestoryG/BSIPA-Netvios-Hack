using System;

namespace System.ComponentModel.Design.Serialization
{
	// Token: 0x02000611 RID: 1553
	public struct MemberRelationship
	{
		// Token: 0x060038D6 RID: 14550 RVA: 0x000F1F52 File Offset: 0x000F0152
		public MemberRelationship(object owner, MemberDescriptor member)
		{
			if (owner == null)
			{
				throw new ArgumentNullException("owner");
			}
			if (member == null)
			{
				throw new ArgumentNullException("member");
			}
			this._owner = owner;
			this._member = member;
		}

		// Token: 0x17000D97 RID: 3479
		// (get) Token: 0x060038D7 RID: 14551 RVA: 0x000F1F7E File Offset: 0x000F017E
		public bool IsEmpty
		{
			get
			{
				return this._owner == null;
			}
		}

		// Token: 0x17000D98 RID: 3480
		// (get) Token: 0x060038D8 RID: 14552 RVA: 0x000F1F89 File Offset: 0x000F0189
		public MemberDescriptor Member
		{
			get
			{
				return this._member;
			}
		}

		// Token: 0x17000D99 RID: 3481
		// (get) Token: 0x060038D9 RID: 14553 RVA: 0x000F1F91 File Offset: 0x000F0191
		public object Owner
		{
			get
			{
				return this._owner;
			}
		}

		// Token: 0x060038DA RID: 14554 RVA: 0x000F1F9C File Offset: 0x000F019C
		public override bool Equals(object obj)
		{
			if (!(obj is MemberRelationship))
			{
				return false;
			}
			MemberRelationship memberRelationship = (MemberRelationship)obj;
			return memberRelationship.Owner == this.Owner && memberRelationship.Member == this.Member;
		}

		// Token: 0x060038DB RID: 14555 RVA: 0x000F1FDA File Offset: 0x000F01DA
		public override int GetHashCode()
		{
			if (this._owner == null)
			{
				return base.GetHashCode();
			}
			return this._owner.GetHashCode() ^ this._member.GetHashCode();
		}

		// Token: 0x060038DC RID: 14556 RVA: 0x000F200C File Offset: 0x000F020C
		public static bool operator ==(MemberRelationship left, MemberRelationship right)
		{
			return left.Owner == right.Owner && left.Member == right.Member;
		}

		// Token: 0x060038DD RID: 14557 RVA: 0x000F2030 File Offset: 0x000F0230
		public static bool operator !=(MemberRelationship left, MemberRelationship right)
		{
			return !(left == right);
		}

		// Token: 0x04002B71 RID: 11121
		private object _owner;

		// Token: 0x04002B72 RID: 11122
		private MemberDescriptor _member;

		// Token: 0x04002B73 RID: 11123
		public static readonly MemberRelationship Empty;
	}
}
