using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security;

namespace System.Runtime.Serialization
{
	// Token: 0x0200007A RID: 122
	internal class DataMember
	{
		// Token: 0x06000913 RID: 2323 RVA: 0x0002951C File Offset: 0x0002771C
		[SecuritySafeCritical]
		internal DataMember()
		{
			this.helper = new DataMember.CriticalHelper();
		}

		// Token: 0x06000914 RID: 2324 RVA: 0x0002952F File Offset: 0x0002772F
		[SecuritySafeCritical]
		internal DataMember(MemberInfo memberInfo)
		{
			this.helper = new DataMember.CriticalHelper(memberInfo);
		}

		// Token: 0x06000915 RID: 2325 RVA: 0x00029543 File Offset: 0x00027743
		[SecuritySafeCritical]
		internal DataMember(string name)
		{
			this.helper = new DataMember.CriticalHelper(name);
		}

		// Token: 0x06000916 RID: 2326 RVA: 0x00029557 File Offset: 0x00027757
		[SecuritySafeCritical]
		internal DataMember(DataContract memberTypeContract, string name, bool isNullable, bool isRequired, bool emitDefaultValue, int order)
		{
			this.helper = new DataMember.CriticalHelper(memberTypeContract, name, isNullable, isRequired, emitDefaultValue, order);
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x06000917 RID: 2327 RVA: 0x00029573 File Offset: 0x00027773
		internal MemberInfo MemberInfo
		{
			[SecuritySafeCritical]
			get
			{
				return this.helper.MemberInfo;
			}
		}

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x06000918 RID: 2328 RVA: 0x00029580 File Offset: 0x00027780
		// (set) Token: 0x06000919 RID: 2329 RVA: 0x0002958D File Offset: 0x0002778D
		internal string Name
		{
			[SecuritySafeCritical]
			get
			{
				return this.helper.Name;
			}
			[SecurityCritical]
			set
			{
				this.helper.Name = value;
			}
		}

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x0600091A RID: 2330 RVA: 0x0002959B File Offset: 0x0002779B
		// (set) Token: 0x0600091B RID: 2331 RVA: 0x000295A8 File Offset: 0x000277A8
		internal int Order
		{
			[SecuritySafeCritical]
			get
			{
				return this.helper.Order;
			}
			[SecurityCritical]
			set
			{
				this.helper.Order = value;
			}
		}

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x0600091C RID: 2332 RVA: 0x000295B6 File Offset: 0x000277B6
		// (set) Token: 0x0600091D RID: 2333 RVA: 0x000295C3 File Offset: 0x000277C3
		internal bool IsRequired
		{
			[SecuritySafeCritical]
			get
			{
				return this.helper.IsRequired;
			}
			[SecurityCritical]
			set
			{
				this.helper.IsRequired = value;
			}
		}

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x0600091E RID: 2334 RVA: 0x000295D1 File Offset: 0x000277D1
		// (set) Token: 0x0600091F RID: 2335 RVA: 0x000295DE File Offset: 0x000277DE
		internal bool EmitDefaultValue
		{
			[SecuritySafeCritical]
			get
			{
				return this.helper.EmitDefaultValue;
			}
			[SecurityCritical]
			set
			{
				this.helper.EmitDefaultValue = value;
			}
		}

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x06000920 RID: 2336 RVA: 0x000295EC File Offset: 0x000277EC
		// (set) Token: 0x06000921 RID: 2337 RVA: 0x000295F9 File Offset: 0x000277F9
		internal bool IsNullable
		{
			[SecuritySafeCritical]
			get
			{
				return this.helper.IsNullable;
			}
			[SecurityCritical]
			set
			{
				this.helper.IsNullable = value;
			}
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x06000922 RID: 2338 RVA: 0x00029607 File Offset: 0x00027807
		// (set) Token: 0x06000923 RID: 2339 RVA: 0x00029614 File Offset: 0x00027814
		internal bool IsGetOnlyCollection
		{
			[SecuritySafeCritical]
			get
			{
				return this.helper.IsGetOnlyCollection;
			}
			[SecurityCritical]
			set
			{
				this.helper.IsGetOnlyCollection = value;
			}
		}

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x06000924 RID: 2340 RVA: 0x00029622 File Offset: 0x00027822
		internal Type MemberType
		{
			[SecuritySafeCritical]
			get
			{
				return this.helper.MemberType;
			}
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x06000925 RID: 2341 RVA: 0x0002962F File Offset: 0x0002782F
		// (set) Token: 0x06000926 RID: 2342 RVA: 0x0002963C File Offset: 0x0002783C
		internal DataContract MemberTypeContract
		{
			[SecuritySafeCritical]
			get
			{
				return this.helper.MemberTypeContract;
			}
			[SecurityCritical]
			set
			{
				this.helper.MemberTypeContract = value;
			}
		}

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x06000927 RID: 2343 RVA: 0x0002964A File Offset: 0x0002784A
		// (set) Token: 0x06000928 RID: 2344 RVA: 0x00029657 File Offset: 0x00027857
		internal bool HasConflictingNameAndType
		{
			[SecuritySafeCritical]
			get
			{
				return this.helper.HasConflictingNameAndType;
			}
			[SecurityCritical]
			set
			{
				this.helper.HasConflictingNameAndType = value;
			}
		}

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x06000929 RID: 2345 RVA: 0x00029665 File Offset: 0x00027865
		// (set) Token: 0x0600092A RID: 2346 RVA: 0x00029672 File Offset: 0x00027872
		internal DataMember ConflictingMember
		{
			[SecuritySafeCritical]
			get
			{
				return this.helper.ConflictingMember;
			}
			[SecurityCritical]
			set
			{
				this.helper.ConflictingMember = value;
			}
		}

		// Token: 0x0600092B RID: 2347 RVA: 0x00029680 File Offset: 0x00027880
		internal DataMember BindGenericParameters(DataContract[] paramContracts, Dictionary<DataContract, DataContract> boundContracts)
		{
			DataContract dataContract = this.MemberTypeContract.BindGenericParameters(paramContracts, boundContracts);
			return new DataMember(dataContract, this.Name, !dataContract.IsValueType, this.IsRequired, this.EmitDefaultValue, this.Order);
		}

		// Token: 0x0600092C RID: 2348 RVA: 0x000296C4 File Offset: 0x000278C4
		internal bool Equals(object other, Dictionary<DataContractPairKey, object> checkedContracts)
		{
			if (this == other)
			{
				return true;
			}
			DataMember dataMember = other as DataMember;
			if (dataMember != null)
			{
				bool flag = this.MemberTypeContract != null && !this.MemberTypeContract.IsValueType;
				bool flag2 = dataMember.MemberTypeContract != null && !dataMember.MemberTypeContract.IsValueType;
				return this.Name == dataMember.Name && (this.IsNullable || flag) == (dataMember.IsNullable || flag2) && this.IsRequired == dataMember.IsRequired && this.EmitDefaultValue == dataMember.EmitDefaultValue && this.MemberTypeContract.Equals(dataMember.MemberTypeContract, checkedContracts);
			}
			return false;
		}

		// Token: 0x0600092D RID: 2349 RVA: 0x0002976F File Offset: 0x0002796F
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x04000349 RID: 841
		[SecurityCritical]
		private DataMember.CriticalHelper helper;

		// Token: 0x02000172 RID: 370
		[SecurityCritical(SecurityCriticalScope.Everything)]
		private class CriticalHelper
		{
			// Token: 0x0600148C RID: 5260 RVA: 0x00053AC8 File Offset: 0x00051CC8
			internal CriticalHelper()
			{
				this.emitDefaultValue = true;
			}

			// Token: 0x0600148D RID: 5261 RVA: 0x00053AD7 File Offset: 0x00051CD7
			internal CriticalHelper(MemberInfo memberInfo)
			{
				this.emitDefaultValue = true;
				this.memberInfo = memberInfo;
			}

			// Token: 0x0600148E RID: 5262 RVA: 0x00053AED File Offset: 0x00051CED
			internal CriticalHelper(string name)
			{
				this.Name = name;
			}

			// Token: 0x0600148F RID: 5263 RVA: 0x00053AFC File Offset: 0x00051CFC
			internal CriticalHelper(DataContract memberTypeContract, string name, bool isNullable, bool isRequired, bool emitDefaultValue, int order)
			{
				this.MemberTypeContract = memberTypeContract;
				this.Name = name;
				this.IsNullable = isNullable;
				this.IsRequired = isRequired;
				this.EmitDefaultValue = emitDefaultValue;
				this.Order = order;
			}

			// Token: 0x1700043C RID: 1084
			// (get) Token: 0x06001490 RID: 5264 RVA: 0x00053B31 File Offset: 0x00051D31
			internal MemberInfo MemberInfo
			{
				get
				{
					return this.memberInfo;
				}
			}

			// Token: 0x1700043D RID: 1085
			// (get) Token: 0x06001491 RID: 5265 RVA: 0x00053B39 File Offset: 0x00051D39
			// (set) Token: 0x06001492 RID: 5266 RVA: 0x00053B41 File Offset: 0x00051D41
			internal string Name
			{
				get
				{
					return this.name;
				}
				set
				{
					this.name = value;
				}
			}

			// Token: 0x1700043E RID: 1086
			// (get) Token: 0x06001493 RID: 5267 RVA: 0x00053B4A File Offset: 0x00051D4A
			// (set) Token: 0x06001494 RID: 5268 RVA: 0x00053B52 File Offset: 0x00051D52
			internal int Order
			{
				get
				{
					return this.order;
				}
				set
				{
					this.order = value;
				}
			}

			// Token: 0x1700043F RID: 1087
			// (get) Token: 0x06001495 RID: 5269 RVA: 0x00053B5B File Offset: 0x00051D5B
			// (set) Token: 0x06001496 RID: 5270 RVA: 0x00053B63 File Offset: 0x00051D63
			internal bool IsRequired
			{
				get
				{
					return this.isRequired;
				}
				set
				{
					this.isRequired = value;
				}
			}

			// Token: 0x17000440 RID: 1088
			// (get) Token: 0x06001497 RID: 5271 RVA: 0x00053B6C File Offset: 0x00051D6C
			// (set) Token: 0x06001498 RID: 5272 RVA: 0x00053B74 File Offset: 0x00051D74
			internal bool EmitDefaultValue
			{
				get
				{
					return this.emitDefaultValue;
				}
				set
				{
					this.emitDefaultValue = value;
				}
			}

			// Token: 0x17000441 RID: 1089
			// (get) Token: 0x06001499 RID: 5273 RVA: 0x00053B7D File Offset: 0x00051D7D
			// (set) Token: 0x0600149A RID: 5274 RVA: 0x00053B85 File Offset: 0x00051D85
			internal bool IsNullable
			{
				get
				{
					return this.isNullable;
				}
				set
				{
					this.isNullable = value;
				}
			}

			// Token: 0x17000442 RID: 1090
			// (get) Token: 0x0600149B RID: 5275 RVA: 0x00053B8E File Offset: 0x00051D8E
			// (set) Token: 0x0600149C RID: 5276 RVA: 0x00053B96 File Offset: 0x00051D96
			internal bool IsGetOnlyCollection
			{
				get
				{
					return this.isGetOnlyCollection;
				}
				set
				{
					this.isGetOnlyCollection = value;
				}
			}

			// Token: 0x17000443 RID: 1091
			// (get) Token: 0x0600149D RID: 5277 RVA: 0x00053BA0 File Offset: 0x00051DA0
			internal Type MemberType
			{
				get
				{
					FieldInfo fieldInfo = this.MemberInfo as FieldInfo;
					if (fieldInfo != null)
					{
						return fieldInfo.FieldType;
					}
					return ((PropertyInfo)this.MemberInfo).PropertyType;
				}
			}

			// Token: 0x17000444 RID: 1092
			// (get) Token: 0x0600149E RID: 5278 RVA: 0x00053BDC File Offset: 0x00051DDC
			// (set) Token: 0x0600149F RID: 5279 RVA: 0x00053C4D File Offset: 0x00051E4D
			internal DataContract MemberTypeContract
			{
				get
				{
					if (this.memberTypeContract == null && this.MemberInfo != null)
					{
						if (this.IsGetOnlyCollection)
						{
							this.memberTypeContract = DataContract.GetGetOnlyCollectionDataContract(DataContract.GetId(this.MemberType.TypeHandle), this.MemberType.TypeHandle, this.MemberType, SerializationMode.SharedContract);
						}
						else
						{
							this.memberTypeContract = DataContract.GetDataContract(this.MemberType);
						}
					}
					return this.memberTypeContract;
				}
				set
				{
					this.memberTypeContract = value;
				}
			}

			// Token: 0x17000445 RID: 1093
			// (get) Token: 0x060014A0 RID: 5280 RVA: 0x00053C56 File Offset: 0x00051E56
			// (set) Token: 0x060014A1 RID: 5281 RVA: 0x00053C5E File Offset: 0x00051E5E
			internal bool HasConflictingNameAndType
			{
				get
				{
					return this.hasConflictingNameAndType;
				}
				set
				{
					this.hasConflictingNameAndType = value;
				}
			}

			// Token: 0x17000446 RID: 1094
			// (get) Token: 0x060014A2 RID: 5282 RVA: 0x00053C67 File Offset: 0x00051E67
			// (set) Token: 0x060014A3 RID: 5283 RVA: 0x00053C6F File Offset: 0x00051E6F
			internal DataMember ConflictingMember
			{
				get
				{
					return this.conflictingMember;
				}
				set
				{
					this.conflictingMember = value;
				}
			}

			// Token: 0x04000A05 RID: 2565
			private DataContract memberTypeContract;

			// Token: 0x04000A06 RID: 2566
			private string name;

			// Token: 0x04000A07 RID: 2567
			private int order;

			// Token: 0x04000A08 RID: 2568
			private bool isRequired;

			// Token: 0x04000A09 RID: 2569
			private bool emitDefaultValue;

			// Token: 0x04000A0A RID: 2570
			private bool isNullable;

			// Token: 0x04000A0B RID: 2571
			private bool isGetOnlyCollection;

			// Token: 0x04000A0C RID: 2572
			private MemberInfo memberInfo;

			// Token: 0x04000A0D RID: 2573
			private bool hasConflictingNameAndType;

			// Token: 0x04000A0E RID: 2574
			private DataMember conflictingMember;
		}
	}
}
