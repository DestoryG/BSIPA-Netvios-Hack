using System;
using System.Collections.Generic;
using System.Security.Permissions;

namespace System.ComponentModel.Design.Serialization
{
	// Token: 0x02000610 RID: 1552
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public abstract class MemberRelationshipService
	{
		// Token: 0x17000D95 RID: 3477
		public MemberRelationship this[MemberRelationship source]
		{
			get
			{
				if (source.Owner == null)
				{
					throw new ArgumentNullException("Owner");
				}
				if (source.Member == null)
				{
					throw new ArgumentNullException("Member");
				}
				return this.GetRelationship(source);
			}
			set
			{
				if (source.Owner == null)
				{
					throw new ArgumentNullException("Owner");
				}
				if (source.Member == null)
				{
					throw new ArgumentNullException("Member");
				}
				this.SetRelationship(source, value);
			}
		}

		// Token: 0x17000D96 RID: 3478
		public MemberRelationship this[object sourceOwner, MemberDescriptor sourceMember]
		{
			get
			{
				if (sourceOwner == null)
				{
					throw new ArgumentNullException("sourceOwner");
				}
				if (sourceMember == null)
				{
					throw new ArgumentNullException("sourceMember");
				}
				return this.GetRelationship(new MemberRelationship(sourceOwner, sourceMember));
			}
			set
			{
				if (sourceOwner == null)
				{
					throw new ArgumentNullException("sourceOwner");
				}
				if (sourceMember == null)
				{
					throw new ArgumentNullException("sourceMember");
				}
				this.SetRelationship(new MemberRelationship(sourceOwner, sourceMember), value);
			}
		}

		// Token: 0x060038D2 RID: 14546 RVA: 0x000F1E28 File Offset: 0x000F0028
		protected virtual MemberRelationship GetRelationship(MemberRelationship source)
		{
			MemberRelationshipService.RelationshipEntry relationshipEntry;
			if (this._relationships != null && this._relationships.TryGetValue(new MemberRelationshipService.RelationshipEntry(source), out relationshipEntry) && relationshipEntry.Owner.IsAlive)
			{
				return new MemberRelationship(relationshipEntry.Owner.Target, relationshipEntry.Member);
			}
			return MemberRelationship.Empty;
		}

		// Token: 0x060038D3 RID: 14547 RVA: 0x000F1E7C File Offset: 0x000F007C
		protected virtual void SetRelationship(MemberRelationship source, MemberRelationship relationship)
		{
			if (!relationship.IsEmpty && !this.SupportsRelationship(source, relationship))
			{
				string text = TypeDescriptor.GetComponentName(source.Owner);
				string text2 = TypeDescriptor.GetComponentName(relationship.Owner);
				if (text == null)
				{
					text = source.Owner.ToString();
				}
				if (text2 == null)
				{
					text2 = relationship.Owner.ToString();
				}
				throw new ArgumentException(SR.GetString("MemberRelationshipService_RelationshipNotSupported", new object[]
				{
					text,
					source.Member.Name,
					text2,
					relationship.Member.Name
				}));
			}
			if (this._relationships == null)
			{
				this._relationships = new Dictionary<MemberRelationshipService.RelationshipEntry, MemberRelationshipService.RelationshipEntry>();
			}
			this._relationships[new MemberRelationshipService.RelationshipEntry(source)] = new MemberRelationshipService.RelationshipEntry(relationship);
		}

		// Token: 0x060038D4 RID: 14548
		public abstract bool SupportsRelationship(MemberRelationship source, MemberRelationship relationship);

		// Token: 0x04002B70 RID: 11120
		private Dictionary<MemberRelationshipService.RelationshipEntry, MemberRelationshipService.RelationshipEntry> _relationships = new Dictionary<MemberRelationshipService.RelationshipEntry, MemberRelationshipService.RelationshipEntry>();

		// Token: 0x020008B2 RID: 2226
		private struct RelationshipEntry
		{
			// Token: 0x0600461C RID: 17948 RVA: 0x0012498F File Offset: 0x00122B8F
			internal RelationshipEntry(MemberRelationship rel)
			{
				this.Owner = new WeakReference(rel.Owner);
				this.Member = rel.Member;
				this.hashCode = ((rel.Owner == null) ? 0 : rel.Owner.GetHashCode());
			}

			// Token: 0x0600461D RID: 17949 RVA: 0x001249D0 File Offset: 0x00122BD0
			public override bool Equals(object o)
			{
				if (o is MemberRelationshipService.RelationshipEntry)
				{
					MemberRelationshipService.RelationshipEntry relationshipEntry = (MemberRelationshipService.RelationshipEntry)o;
					return this == relationshipEntry;
				}
				return false;
			}

			// Token: 0x0600461E RID: 17950 RVA: 0x001249FC File Offset: 0x00122BFC
			public static bool operator ==(MemberRelationshipService.RelationshipEntry re1, MemberRelationshipService.RelationshipEntry re2)
			{
				object obj = (re1.Owner.IsAlive ? re1.Owner.Target : null);
				object obj2 = (re2.Owner.IsAlive ? re2.Owner.Target : null);
				return obj == obj2 && re1.Member.Equals(re2.Member);
			}

			// Token: 0x0600461F RID: 17951 RVA: 0x00124A58 File Offset: 0x00122C58
			public static bool operator !=(MemberRelationshipService.RelationshipEntry re1, MemberRelationshipService.RelationshipEntry re2)
			{
				return !(re1 == re2);
			}

			// Token: 0x06004620 RID: 17952 RVA: 0x00124A64 File Offset: 0x00122C64
			public override int GetHashCode()
			{
				return this.hashCode;
			}

			// Token: 0x04003B50 RID: 15184
			internal WeakReference Owner;

			// Token: 0x04003B51 RID: 15185
			internal MemberDescriptor Member;

			// Token: 0x04003B52 RID: 15186
			private int hashCode;
		}
	}
}
