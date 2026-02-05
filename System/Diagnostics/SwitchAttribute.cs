using System;
using System.Collections;
using System.Reflection;

namespace System.Diagnostics
{
	// Token: 0x020004A7 RID: 1191
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Event)]
	public sealed class SwitchAttribute : Attribute
	{
		// Token: 0x06002C19 RID: 11289 RVA: 0x000C71B8 File Offset: 0x000C53B8
		public SwitchAttribute(string switchName, Type switchType)
		{
			this.SwitchName = switchName;
			this.SwitchType = switchType;
		}

		// Token: 0x17000AAD RID: 2733
		// (get) Token: 0x06002C1A RID: 11290 RVA: 0x000C71CE File Offset: 0x000C53CE
		// (set) Token: 0x06002C1B RID: 11291 RVA: 0x000C71D8 File Offset: 0x000C53D8
		public string SwitchName
		{
			get
			{
				return this.name;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (value.Length == 0)
				{
					throw new ArgumentException(SR.GetString("InvalidNullEmptyArgument", new object[] { "value" }), "value");
				}
				this.name = value;
			}
		}

		// Token: 0x17000AAE RID: 2734
		// (get) Token: 0x06002C1C RID: 11292 RVA: 0x000C7225 File Offset: 0x000C5425
		// (set) Token: 0x06002C1D RID: 11293 RVA: 0x000C722D File Offset: 0x000C542D
		public Type SwitchType
		{
			get
			{
				return this.type;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.type = value;
			}
		}

		// Token: 0x17000AAF RID: 2735
		// (get) Token: 0x06002C1E RID: 11294 RVA: 0x000C724A File Offset: 0x000C544A
		// (set) Token: 0x06002C1F RID: 11295 RVA: 0x000C7252 File Offset: 0x000C5452
		public string SwitchDescription
		{
			get
			{
				return this.description;
			}
			set
			{
				this.description = value;
			}
		}

		// Token: 0x06002C20 RID: 11296 RVA: 0x000C725C File Offset: 0x000C545C
		public static SwitchAttribute[] GetAll(Assembly assembly)
		{
			if (assembly == null)
			{
				throw new ArgumentNullException("assembly");
			}
			ArrayList arrayList = new ArrayList();
			object[] customAttributes = assembly.GetCustomAttributes(typeof(SwitchAttribute), false);
			arrayList.AddRange(customAttributes);
			Type[] types = assembly.GetTypes();
			for (int i = 0; i < types.Length; i++)
			{
				SwitchAttribute.GetAllRecursive(types[i], arrayList);
			}
			SwitchAttribute[] array = new SwitchAttribute[arrayList.Count];
			arrayList.CopyTo(array, 0);
			return array;
		}

		// Token: 0x06002C21 RID: 11297 RVA: 0x000C72D4 File Offset: 0x000C54D4
		private static void GetAllRecursive(Type type, ArrayList switchAttribs)
		{
			SwitchAttribute.GetAllRecursive(type, switchAttribs);
			MemberInfo[] members = type.GetMembers(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			for (int i = 0; i < members.Length; i++)
			{
				if (!(members[i] is Type))
				{
					SwitchAttribute.GetAllRecursive(members[i], switchAttribs);
				}
			}
		}

		// Token: 0x06002C22 RID: 11298 RVA: 0x000C7314 File Offset: 0x000C5514
		private static void GetAllRecursive(MemberInfo member, ArrayList switchAttribs)
		{
			object[] customAttributes = member.GetCustomAttributes(typeof(SwitchAttribute), false);
			switchAttribs.AddRange(customAttributes);
		}

		// Token: 0x040026B0 RID: 9904
		private Type type;

		// Token: 0x040026B1 RID: 9905
		private string name;

		// Token: 0x040026B2 RID: 9906
		private string description;
	}
}
