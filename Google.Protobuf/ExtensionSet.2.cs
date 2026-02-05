using System;
using System.Collections.Generic;
using System.Linq;

namespace Google.Protobuf
{
	// Token: 0x0200000B RID: 11
	public sealed class ExtensionSet<TTarget> where TTarget : IExtendableMessage<TTarget>
	{
		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060000D9 RID: 217 RVA: 0x00004C44 File Offset: 0x00002E44
		internal Dictionary<int, IExtensionValue> ValuesByNumber { get; } = new Dictionary<int, IExtensionValue>();

		// Token: 0x060000DA RID: 218 RVA: 0x00004C4C File Offset: 0x00002E4C
		public override int GetHashCode()
		{
			int num = typeof(TTarget).GetHashCode();
			foreach (KeyValuePair<int, IExtensionValue> keyValuePair in this.ValuesByNumber)
			{
				int num2 = keyValuePair.Key.GetHashCode() ^ keyValuePair.Value.GetHashCode();
				num ^= num2;
			}
			return num;
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00004CCC File Offset: 0x00002ECC
		public override bool Equals(object other)
		{
			if (this == other)
			{
				return true;
			}
			ExtensionSet<TTarget> extensionSet = other as ExtensionSet<TTarget>;
			if (this.ValuesByNumber.Count != extensionSet.ValuesByNumber.Count)
			{
				return false;
			}
			foreach (KeyValuePair<int, IExtensionValue> keyValuePair in this.ValuesByNumber)
			{
				IExtensionValue extensionValue;
				if (!extensionSet.ValuesByNumber.TryGetValue(keyValuePair.Key, out extensionValue))
				{
					return false;
				}
				if (!keyValuePair.Value.Equals(extensionValue))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00004D74 File Offset: 0x00002F74
		public int CalculateSize()
		{
			int num = 0;
			foreach (IExtensionValue extensionValue in this.ValuesByNumber.Values)
			{
				num += extensionValue.CalculateSize();
			}
			return num;
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00004DD4 File Offset: 0x00002FD4
		public void WriteTo(CodedOutputStream stream)
		{
			foreach (IExtensionValue extensionValue in this.ValuesByNumber.Values)
			{
				extensionValue.WriteTo(stream);
			}
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00004E2C File Offset: 0x0000302C
		internal bool IsInitialized()
		{
			return this.ValuesByNumber.Values.All((IExtensionValue v) => v.IsInitialized());
		}
	}
}
