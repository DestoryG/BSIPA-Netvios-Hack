using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace HarmonyLib
{
	// Token: 0x0200006E RID: 110
	internal static class PatchInfoSerialization
	{
		// Token: 0x060001EE RID: 494 RVA: 0x0000AB64 File Offset: 0x00008D64
		internal static byte[] Serialize(this PatchInfo patchInfo)
		{
			byte[] buffer;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				new BinaryFormatter().Serialize(memoryStream, patchInfo);
				buffer = memoryStream.GetBuffer();
			}
			return buffer;
		}

		// Token: 0x060001EF RID: 495 RVA: 0x0000ABA8 File Offset: 0x00008DA8
		internal static PatchInfo Deserialize(byte[] bytes)
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			binaryFormatter.Binder = new PatchInfoSerialization.Binder();
			MemoryStream memoryStream = new MemoryStream(bytes);
			return (PatchInfo)binaryFormatter.Deserialize(memoryStream);
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x0000ABD8 File Offset: 0x00008DD8
		internal static int PriorityComparer(object obj, int index, int priority)
		{
			Traverse traverse = Traverse.Create(obj);
			int value = traverse.Field("priority").GetValue<int>();
			int value2 = traverse.Field("index").GetValue<int>();
			if (priority != value)
			{
				return -priority.CompareTo(value);
			}
			return index.CompareTo(value2);
		}

		// Token: 0x0200006F RID: 111
		private class Binder : SerializationBinder
		{
			// Token: 0x060001F1 RID: 497 RVA: 0x0000AC24 File Offset: 0x00008E24
			public override Type BindToType(string assemblyName, string typeName)
			{
				foreach (Type type in new Type[]
				{
					typeof(PatchInfo),
					typeof(Patch[]),
					typeof(Patch)
				})
				{
					if (typeName == type.FullName)
					{
						return type;
					}
				}
				return Type.GetType(string.Format("{0}, {1}", typeName, assemblyName));
			}
		}
	}
}
