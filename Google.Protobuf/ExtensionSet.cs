using System;
using System.Collections.Generic;
using Google.Protobuf.Collections;

namespace Google.Protobuf
{
	// Token: 0x0200000A RID: 10
	public static class ExtensionSet
	{
		// Token: 0x060000CE RID: 206 RVA: 0x000048D2 File Offset: 0x00002AD2
		private static bool TryGetValue<TTarget>(ref ExtensionSet<TTarget> set, Extension extension, out IExtensionValue value) where TTarget : IExtendableMessage<TTarget>
		{
			if (set == null)
			{
				value = null;
				return false;
			}
			return set.ValuesByNumber.TryGetValue(extension.FieldNumber, out value);
		}

		// Token: 0x060000CF RID: 207 RVA: 0x000048F0 File Offset: 0x00002AF0
		public static TValue Get<TTarget, TValue>(ref ExtensionSet<TTarget> set, Extension<TTarget, TValue> extension) where TTarget : IExtendableMessage<TTarget>
		{
			IExtensionValue extensionValue;
			if (ExtensionSet.TryGetValue<TTarget>(ref set, extension, out extensionValue))
			{
				return ((ExtensionValue<TValue>)extensionValue).GetValue();
			}
			return extension.DefaultValue;
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x0000491C File Offset: 0x00002B1C
		public static RepeatedField<TValue> Get<TTarget, TValue>(ref ExtensionSet<TTarget> set, RepeatedExtension<TTarget, TValue> extension) where TTarget : IExtendableMessage<TTarget>
		{
			IExtensionValue extensionValue;
			if (ExtensionSet.TryGetValue<TTarget>(ref set, extension, out extensionValue))
			{
				return ((RepeatedExtensionValue<TValue>)extensionValue).GetValue();
			}
			return null;
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00004944 File Offset: 0x00002B44
		public static RepeatedField<TValue> GetOrInitialize<TTarget, TValue>(ref ExtensionSet<TTarget> set, RepeatedExtension<TTarget, TValue> extension) where TTarget : IExtendableMessage<TTarget>
		{
			IExtensionValue extensionValue;
			if (set == null)
			{
				extensionValue = extension.CreateValue();
				set = new ExtensionSet<TTarget>();
				set.ValuesByNumber.Add(extension.FieldNumber, extensionValue);
			}
			else if (!set.ValuesByNumber.TryGetValue(extension.FieldNumber, out extensionValue))
			{
				extensionValue = extension.CreateValue();
				set.ValuesByNumber.Add(extension.FieldNumber, extensionValue);
			}
			return ((RepeatedExtensionValue<TValue>)extensionValue).GetValue();
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x000049B4 File Offset: 0x00002BB4
		public static void Set<TTarget, TValue>(ref ExtensionSet<TTarget> set, Extension<TTarget, TValue> extension, TValue value) where TTarget : IExtendableMessage<TTarget>
		{
			ProtoPreconditions.CheckNotNullUnconstrained<TValue>(value, "value");
			IExtensionValue extensionValue;
			if (set == null)
			{
				extensionValue = extension.CreateValue();
				set = new ExtensionSet<TTarget>();
				set.ValuesByNumber.Add(extension.FieldNumber, extensionValue);
			}
			else if (!set.ValuesByNumber.TryGetValue(extension.FieldNumber, out extensionValue))
			{
				extensionValue = extension.CreateValue();
				set.ValuesByNumber.Add(extension.FieldNumber, extensionValue);
			}
			((ExtensionValue<TValue>)extensionValue).SetValue(value);
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00004A30 File Offset: 0x00002C30
		public static bool Has<TTarget, TValue>(ref ExtensionSet<TTarget> set, Extension<TTarget, TValue> extension) where TTarget : IExtendableMessage<TTarget>
		{
			IExtensionValue extensionValue;
			return ExtensionSet.TryGetValue<TTarget>(ref set, extension, out extensionValue);
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00004A46 File Offset: 0x00002C46
		public static void Clear<TTarget, TValue>(ref ExtensionSet<TTarget> set, Extension<TTarget, TValue> extension) where TTarget : IExtendableMessage<TTarget>
		{
			if (set == null)
			{
				return;
			}
			set.ValuesByNumber.Remove(extension.FieldNumber);
			if (set.ValuesByNumber.Count == 0)
			{
				set = null;
			}
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00004A71 File Offset: 0x00002C71
		public static void Clear<TTarget, TValue>(ref ExtensionSet<TTarget> set, RepeatedExtension<TTarget, TValue> extension) where TTarget : IExtendableMessage<TTarget>
		{
			if (set == null)
			{
				return;
			}
			set.ValuesByNumber.Remove(extension.FieldNumber);
			if (set.ValuesByNumber.Count == 0)
			{
				set = null;
			}
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00004A9C File Offset: 0x00002C9C
		public static bool TryMergeFieldFrom<TTarget>(ref ExtensionSet<TTarget> set, CodedInputStream stream) where TTarget : IExtendableMessage<TTarget>
		{
			int tagFieldNumber = WireFormat.GetTagFieldNumber(stream.LastTag);
			IExtensionValue extensionValue;
			if (set != null && set.ValuesByNumber.TryGetValue(tagFieldNumber, out extensionValue))
			{
				extensionValue.MergeFrom(stream);
				return true;
			}
			Extension extension;
			if (stream.ExtensionRegistry != null && stream.ExtensionRegistry.ContainsInputField(stream, typeof(TTarget), out extension))
			{
				IExtensionValue extensionValue2 = extension.CreateValue();
				extensionValue2.MergeFrom(stream);
				set = set ?? new ExtensionSet<TTarget>();
				set.ValuesByNumber.Add(extension.FieldNumber, extensionValue2);
				return true;
			}
			return false;
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00004B28 File Offset: 0x00002D28
		public static void MergeFrom<TTarget>(ref ExtensionSet<TTarget> first, ExtensionSet<TTarget> second) where TTarget : IExtendableMessage<TTarget>
		{
			if (second == null)
			{
				return;
			}
			if (first == null)
			{
				first = new ExtensionSet<TTarget>();
			}
			foreach (KeyValuePair<int, IExtensionValue> keyValuePair in second.ValuesByNumber)
			{
				IExtensionValue extensionValue;
				if (first.ValuesByNumber.TryGetValue(keyValuePair.Key, out extensionValue))
				{
					extensionValue.MergeFrom(keyValuePair.Value);
				}
				else
				{
					IExtensionValue extensionValue2 = keyValuePair.Value.Clone();
					first.ValuesByNumber[keyValuePair.Key] = extensionValue2;
				}
			}
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00004BCC File Offset: 0x00002DCC
		public static ExtensionSet<TTarget> Clone<TTarget>(ExtensionSet<TTarget> set) where TTarget : IExtendableMessage<TTarget>
		{
			if (set == null)
			{
				return null;
			}
			ExtensionSet<TTarget> extensionSet = new ExtensionSet<TTarget>();
			foreach (KeyValuePair<int, IExtensionValue> keyValuePair in set.ValuesByNumber)
			{
				IExtensionValue extensionValue = keyValuePair.Value.Clone();
				extensionSet.ValuesByNumber[keyValuePair.Key] = extensionValue;
			}
			return extensionSet;
		}
	}
}
