using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace CustomAvatar.Utilities
{
	// Token: 0x0200001D RID: 29
	internal class QuaternionJsonConverter : JsonConverter<Quaternion>
	{
		// Token: 0x06000062 RID: 98 RVA: 0x00004138 File Offset: 0x00002338
		public override void WriteJson(JsonWriter writer, Quaternion value, JsonSerializer serializer)
		{
			JObject jobject = new JObject
			{
				{ "x", value.x },
				{ "y", value.y },
				{ "z", value.z },
				{ "w", value.w }
			};
			serializer.Serialize(writer, jobject);
		}

		// Token: 0x06000063 RID: 99 RVA: 0x000041B4 File Offset: 0x000023B4
		public override Quaternion ReadJson(JsonReader reader, Type objectType, Quaternion existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			JObject jobject = serializer.Deserialize<JObject>(reader);
			bool flag = jobject == null;
			Quaternion quaternion;
			if (flag)
			{
				quaternion = Quaternion.identity;
			}
			else
			{
				float num = jobject.Value<float>("x");
				float num2 = jobject.Value<float>("y");
				float num3 = jobject.Value<float>("z");
				float num4 = jobject.Value<float>("w");
				bool flag2 = num == 0f && num2 == 0f && num3 == 0f && num4 == 0f;
				if (flag2)
				{
					num4 = 1f;
				}
				quaternion = new Quaternion(num, num2, num3, num4);
			}
			return quaternion;
		}
	}
}
