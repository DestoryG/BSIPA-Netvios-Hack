using System;
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;

namespace System.ComponentModel
{
	// Token: 0x02000590 RID: 1424
	[ComVisible(true)]
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public abstract class MemberDescriptor
	{
		// Token: 0x060034D9 RID: 13529 RVA: 0x000E6ABC File Offset: 0x000E4CBC
		protected MemberDescriptor(string name)
			: this(name, null)
		{
		}

		// Token: 0x060034DA RID: 13530 RVA: 0x000E6AC8 File Offset: 0x000E4CC8
		protected MemberDescriptor(string name, Attribute[] attributes)
		{
			this.lockCookie = new object();
			base..ctor();
			try
			{
				if (name == null || name.Length == 0)
				{
					throw new ArgumentException(SR.GetString("InvalidMemberName"));
				}
				this.name = name;
				this.displayName = name;
				this.nameHash = name.GetHashCode();
				if (attributes != null)
				{
					this.attributes = attributes;
					this.attributesFiltered = false;
				}
				this.originalAttributes = this.attributes;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		// Token: 0x060034DB RID: 13531 RVA: 0x000E6B50 File Offset: 0x000E4D50
		protected MemberDescriptor(MemberDescriptor descr)
		{
			this.lockCookie = new object();
			base..ctor();
			this.name = descr.Name;
			this.displayName = this.name;
			this.nameHash = this.name.GetHashCode();
			this.attributes = new Attribute[descr.Attributes.Count];
			descr.Attributes.CopyTo(this.attributes, 0);
			this.attributesFiltered = true;
			this.originalAttributes = this.attributes;
		}

		// Token: 0x060034DC RID: 13532 RVA: 0x000E6BD4 File Offset: 0x000E4DD4
		protected MemberDescriptor(MemberDescriptor oldMemberDescriptor, Attribute[] newAttributes)
		{
			this.lockCookie = new object();
			base..ctor();
			this.name = oldMemberDescriptor.Name;
			this.displayName = oldMemberDescriptor.DisplayName;
			this.nameHash = this.name.GetHashCode();
			ArrayList arrayList = new ArrayList();
			if (oldMemberDescriptor.Attributes.Count != 0)
			{
				foreach (object obj in oldMemberDescriptor.Attributes)
				{
					arrayList.Add(obj);
				}
			}
			if (newAttributes != null)
			{
				foreach (Attribute obj2 in newAttributes)
				{
					arrayList.Add(obj2);
				}
			}
			this.attributes = new Attribute[arrayList.Count];
			arrayList.CopyTo(this.attributes, 0);
			this.attributesFiltered = false;
			this.originalAttributes = this.attributes;
		}

		// Token: 0x17000CE9 RID: 3305
		// (get) Token: 0x060034DD RID: 13533 RVA: 0x000E6CD0 File Offset: 0x000E4ED0
		// (set) Token: 0x060034DE RID: 13534 RVA: 0x000E6CE4 File Offset: 0x000E4EE4
		protected virtual Attribute[] AttributeArray
		{
			get
			{
				this.CheckAttributesValid();
				this.FilterAttributesIfNeeded();
				return this.attributes;
			}
			set
			{
				object obj = this.lockCookie;
				lock (obj)
				{
					this.attributes = value;
					this.originalAttributes = value;
					this.attributesFiltered = false;
					this.attributeCollection = null;
				}
			}
		}

		// Token: 0x17000CEA RID: 3306
		// (get) Token: 0x060034DF RID: 13535 RVA: 0x000E6D3C File Offset: 0x000E4F3C
		public virtual AttributeCollection Attributes
		{
			get
			{
				this.CheckAttributesValid();
				AttributeCollection attributeCollection = this.attributeCollection;
				if (attributeCollection == null)
				{
					object obj = this.lockCookie;
					lock (obj)
					{
						attributeCollection = this.CreateAttributeCollection();
						this.attributeCollection = attributeCollection;
					}
				}
				return attributeCollection;
			}
		}

		// Token: 0x17000CEB RID: 3307
		// (get) Token: 0x060034E0 RID: 13536 RVA: 0x000E6D98 File Offset: 0x000E4F98
		public virtual string Category
		{
			get
			{
				if (this.category == null)
				{
					this.category = ((CategoryAttribute)this.Attributes[typeof(CategoryAttribute)]).Category;
				}
				return this.category;
			}
		}

		// Token: 0x17000CEC RID: 3308
		// (get) Token: 0x060034E1 RID: 13537 RVA: 0x000E6DCD File Offset: 0x000E4FCD
		public virtual string Description
		{
			get
			{
				if (this.description == null)
				{
					this.description = ((DescriptionAttribute)this.Attributes[typeof(DescriptionAttribute)]).Description;
				}
				return this.description;
			}
		}

		// Token: 0x17000CED RID: 3309
		// (get) Token: 0x060034E2 RID: 13538 RVA: 0x000E6E02 File Offset: 0x000E5002
		public virtual bool IsBrowsable
		{
			get
			{
				return ((BrowsableAttribute)this.Attributes[typeof(BrowsableAttribute)]).Browsable;
			}
		}

		// Token: 0x17000CEE RID: 3310
		// (get) Token: 0x060034E3 RID: 13539 RVA: 0x000E6E23 File Offset: 0x000E5023
		public virtual string Name
		{
			get
			{
				if (this.name == null)
				{
					return "";
				}
				return this.name;
			}
		}

		// Token: 0x17000CEF RID: 3311
		// (get) Token: 0x060034E4 RID: 13540 RVA: 0x000E6E39 File Offset: 0x000E5039
		protected virtual int NameHashCode
		{
			get
			{
				return this.nameHash;
			}
		}

		// Token: 0x17000CF0 RID: 3312
		// (get) Token: 0x060034E5 RID: 13541 RVA: 0x000E6E41 File Offset: 0x000E5041
		public virtual bool DesignTimeOnly
		{
			get
			{
				return DesignOnlyAttribute.Yes.Equals(this.Attributes[typeof(DesignOnlyAttribute)]);
			}
		}

		// Token: 0x17000CF1 RID: 3313
		// (get) Token: 0x060034E6 RID: 13542 RVA: 0x000E6E64 File Offset: 0x000E5064
		public virtual string DisplayName
		{
			get
			{
				DisplayNameAttribute displayNameAttribute = this.Attributes[typeof(DisplayNameAttribute)] as DisplayNameAttribute;
				if (displayNameAttribute == null || displayNameAttribute.IsDefaultAttribute())
				{
					return this.displayName;
				}
				return displayNameAttribute.DisplayName;
			}
		}

		// Token: 0x060034E7 RID: 13543 RVA: 0x000E6EA4 File Offset: 0x000E50A4
		private void CheckAttributesValid()
		{
			if (this.attributesFiltered && this.metadataVersion != TypeDescriptor.MetadataVersion)
			{
				this.attributesFilled = false;
				this.attributesFiltered = false;
				this.attributeCollection = null;
			}
		}

		// Token: 0x060034E8 RID: 13544 RVA: 0x000E6ED0 File Offset: 0x000E50D0
		protected virtual AttributeCollection CreateAttributeCollection()
		{
			return new AttributeCollection(this.AttributeArray);
		}

		// Token: 0x060034E9 RID: 13545 RVA: 0x000E6EE0 File Offset: 0x000E50E0
		public override bool Equals(object obj)
		{
			if (this == obj)
			{
				return true;
			}
			if (obj == null)
			{
				return false;
			}
			if (obj.GetType() != base.GetType())
			{
				return false;
			}
			MemberDescriptor memberDescriptor = (MemberDescriptor)obj;
			this.FilterAttributesIfNeeded();
			memberDescriptor.FilterAttributesIfNeeded();
			if (memberDescriptor.nameHash != this.nameHash)
			{
				return false;
			}
			if (memberDescriptor.category == null != (this.category == null) || (this.category != null && !memberDescriptor.category.Equals(this.category)))
			{
				return false;
			}
			if (!LocalAppContextSwitches.MemberDescriptorEqualsReturnsFalseIfEquivalent)
			{
				if (memberDescriptor.description == null != (this.description == null) || (this.description != null && !memberDescriptor.description.Equals(this.description)))
				{
					return false;
				}
			}
			else if (memberDescriptor.description == null != (this.description == null) || (this.description != null && !memberDescriptor.category.Equals(this.description)))
			{
				return false;
			}
			if (memberDescriptor.attributes == null != (this.attributes == null))
			{
				return false;
			}
			bool flag = true;
			if (this.attributes != null)
			{
				if (this.attributes.Length != memberDescriptor.attributes.Length)
				{
					return false;
				}
				for (int i = 0; i < this.attributes.Length; i++)
				{
					if (!this.attributes[i].Equals(memberDescriptor.attributes[i]))
					{
						flag = false;
						break;
					}
				}
			}
			return flag;
		}

		// Token: 0x060034EA RID: 13546 RVA: 0x000E7030 File Offset: 0x000E5230
		protected virtual void FillAttributes(IList attributeList)
		{
			if (this.originalAttributes != null)
			{
				foreach (Attribute attribute in this.originalAttributes)
				{
					attributeList.Add(attribute);
				}
			}
		}

		// Token: 0x060034EB RID: 13547 RVA: 0x000E7068 File Offset: 0x000E5268
		private void FilterAttributesIfNeeded()
		{
			if (!this.attributesFiltered)
			{
				IList list;
				if (!this.attributesFilled)
				{
					list = new ArrayList();
					try
					{
						this.FillAttributes(list);
						goto IL_0034;
					}
					catch (ThreadAbortException)
					{
						throw;
					}
					catch (Exception ex)
					{
						goto IL_0034;
					}
				}
				list = new ArrayList(this.attributes);
				IL_0034:
				Hashtable hashtable = new Hashtable(list.Count);
				foreach (object obj in list)
				{
					Attribute attribute = (Attribute)obj;
					hashtable[attribute.TypeId] = attribute;
				}
				Attribute[] array = new Attribute[hashtable.Values.Count];
				hashtable.Values.CopyTo(array, 0);
				object obj2 = this.lockCookie;
				lock (obj2)
				{
					this.attributes = array;
					this.attributesFiltered = true;
					this.attributesFilled = true;
					this.metadataVersion = TypeDescriptor.MetadataVersion;
				}
			}
		}

		// Token: 0x060034EC RID: 13548 RVA: 0x000E7190 File Offset: 0x000E5390
		protected static MethodInfo FindMethod(Type componentClass, string name, Type[] args, Type returnType)
		{
			return MemberDescriptor.FindMethod(componentClass, name, args, returnType, true);
		}

		// Token: 0x060034ED RID: 13549 RVA: 0x000E719C File Offset: 0x000E539C
		protected static MethodInfo FindMethod(Type componentClass, string name, Type[] args, Type returnType, bool publicOnly)
		{
			MethodInfo methodInfo;
			if (publicOnly)
			{
				methodInfo = componentClass.GetMethod(name, args);
			}
			else
			{
				methodInfo = componentClass.GetMethod(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, args, null);
			}
			if (methodInfo != null && !methodInfo.ReturnType.IsEquivalentTo(returnType))
			{
				methodInfo = null;
			}
			return methodInfo;
		}

		// Token: 0x060034EE RID: 13550 RVA: 0x000E71E1 File Offset: 0x000E53E1
		public override int GetHashCode()
		{
			return this.nameHash;
		}

		// Token: 0x060034EF RID: 13551 RVA: 0x000E71E9 File Offset: 0x000E53E9
		protected virtual object GetInvocationTarget(Type type, object instance)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (instance == null)
			{
				throw new ArgumentNullException("instance");
			}
			return TypeDescriptor.GetAssociation(type, instance);
		}

		// Token: 0x060034F0 RID: 13552 RVA: 0x000E7214 File Offset: 0x000E5414
		protected static ISite GetSite(object component)
		{
			if (!(component is IComponent))
			{
				return null;
			}
			return ((IComponent)component).Site;
		}

		// Token: 0x060034F1 RID: 13553 RVA: 0x000E722B File Offset: 0x000E542B
		[Obsolete("This method has been deprecated. Use GetInvocationTarget instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		protected static object GetInvokee(Type componentClass, object component)
		{
			if (componentClass == null)
			{
				throw new ArgumentNullException("componentClass");
			}
			if (component == null)
			{
				throw new ArgumentNullException("component");
			}
			return TypeDescriptor.GetAssociation(componentClass, component);
		}

		// Token: 0x04002A19 RID: 10777
		private string name;

		// Token: 0x04002A1A RID: 10778
		private string displayName;

		// Token: 0x04002A1B RID: 10779
		private int nameHash;

		// Token: 0x04002A1C RID: 10780
		private AttributeCollection attributeCollection;

		// Token: 0x04002A1D RID: 10781
		private Attribute[] attributes;

		// Token: 0x04002A1E RID: 10782
		private Attribute[] originalAttributes;

		// Token: 0x04002A1F RID: 10783
		private bool attributesFiltered;

		// Token: 0x04002A20 RID: 10784
		private bool attributesFilled;

		// Token: 0x04002A21 RID: 10785
		private int metadataVersion;

		// Token: 0x04002A22 RID: 10786
		private string category;

		// Token: 0x04002A23 RID: 10787
		private string description;

		// Token: 0x04002A24 RID: 10788
		private object lockCookie;
	}
}
