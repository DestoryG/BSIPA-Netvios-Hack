using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace System.Net
{
	// Token: 0x02000229 RID: 553
	internal class TrackingValidationObjectDictionary : StringDictionary
	{
		// Token: 0x0600145F RID: 5215 RVA: 0x0006BA97 File Offset: 0x00069C97
		internal TrackingValidationObjectDictionary(IDictionary<string, TrackingValidationObjectDictionary.ValidateAndParseValue> validators)
		{
			this.IsChanged = false;
			this.validators = validators;
		}

		// Token: 0x06001460 RID: 5216 RVA: 0x0006BAB0 File Offset: 0x00069CB0
		private void PersistValue(string key, string value, bool addValue)
		{
			key = key.ToLowerInvariant();
			if (!string.IsNullOrEmpty(value))
			{
				if (this.validators != null && this.validators.ContainsKey(key))
				{
					object obj = this.validators[key](value);
					if (this.internalObjects == null)
					{
						this.internalObjects = new Dictionary<string, object>();
					}
					if (addValue)
					{
						this.internalObjects.Add(key, obj);
						base.Add(key, obj.ToString());
					}
					else
					{
						this.internalObjects[key] = obj;
						base[key] = obj.ToString();
					}
				}
				else if (addValue)
				{
					base.Add(key, value);
				}
				else
				{
					base[key] = value;
				}
				this.IsChanged = true;
			}
		}

		// Token: 0x17000442 RID: 1090
		// (get) Token: 0x06001461 RID: 5217 RVA: 0x0006BB63 File Offset: 0x00069D63
		// (set) Token: 0x06001462 RID: 5218 RVA: 0x0006BB6B File Offset: 0x00069D6B
		internal bool IsChanged { get; set; }

		// Token: 0x06001463 RID: 5219 RVA: 0x0006BB74 File Offset: 0x00069D74
		internal object InternalGet(string key)
		{
			if (this.internalObjects != null && this.internalObjects.ContainsKey(key))
			{
				return this.internalObjects[key];
			}
			return base[key];
		}

		// Token: 0x06001464 RID: 5220 RVA: 0x0006BBA0 File Offset: 0x00069DA0
		internal void InternalSet(string key, object value)
		{
			if (this.internalObjects == null)
			{
				this.internalObjects = new Dictionary<string, object>();
			}
			this.internalObjects[key] = value;
			base[key] = value.ToString();
			this.IsChanged = true;
		}

		// Token: 0x17000443 RID: 1091
		public override string this[string key]
		{
			get
			{
				return base[key];
			}
			set
			{
				this.PersistValue(key, value, false);
			}
		}

		// Token: 0x06001467 RID: 5223 RVA: 0x0006BBEA File Offset: 0x00069DEA
		public override void Add(string key, string value)
		{
			this.PersistValue(key, value, true);
		}

		// Token: 0x06001468 RID: 5224 RVA: 0x0006BBF5 File Offset: 0x00069DF5
		public override void Clear()
		{
			if (this.internalObjects != null)
			{
				this.internalObjects.Clear();
			}
			base.Clear();
			this.IsChanged = true;
		}

		// Token: 0x06001469 RID: 5225 RVA: 0x0006BC17 File Offset: 0x00069E17
		public override void Remove(string key)
		{
			if (this.internalObjects != null && this.internalObjects.ContainsKey(key))
			{
				this.internalObjects.Remove(key);
			}
			base.Remove(key);
			this.IsChanged = true;
		}

		// Token: 0x04001624 RID: 5668
		private IDictionary<string, object> internalObjects;

		// Token: 0x04001625 RID: 5669
		private readonly IDictionary<string, TrackingValidationObjectDictionary.ValidateAndParseValue> validators;

		// Token: 0x0200076A RID: 1898
		// (Invoke) Token: 0x0600424F RID: 16975
		internal delegate object ValidateAndParseValue(object valueToValidate);
	}
}
