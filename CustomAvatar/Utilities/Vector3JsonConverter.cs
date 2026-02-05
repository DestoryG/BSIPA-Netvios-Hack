using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace CustomAvatar.Utilities
{
	// Token: 0x02000020 RID: 32
	internal class Vector3JsonConverter : JsonConverter<Vector3>
	{
		// Token: 0x0600006C RID: 108 RVA: 0x0000450C File Offset: 0x0000270C
		public override void WriteJson(JsonWriter writer, Vector3 value, JsonSerializer serializer)
		{
			JObject jobject = new JObject
			{
				{ "x", value.x },
				{ "y", value.y },
				{ "z", value.z }
			};
			serializer.Serialize(writer, jobject);
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00004570 File Offset: 0x00002770
		public override Vector3 ReadJson(JsonReader reader, Type objectType, Vector3 existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			JObject jobject = serializer.Deserialize<JObject>(reader);
			bool flag = jobject == null;
			Vector3 vector;
			if (flag)
			{
				vector = Vector3.zero;
			}
			else
			{
				vector = new Vector3(jobject.Value<float>("x"), jobject.Value<float>("y"), jobject.Value<float>("z"));
			}
			return vector;
		}
	}
}
