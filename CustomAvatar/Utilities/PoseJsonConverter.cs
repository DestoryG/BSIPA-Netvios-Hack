using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace CustomAvatar.Utilities
{
	// Token: 0x0200001E RID: 30
	internal class PoseJsonConverter : JsonConverter<Pose>
	{
		// Token: 0x06000065 RID: 101 RVA: 0x0000425C File Offset: 0x0000245C
		public override void WriteJson(JsonWriter writer, Pose value, JsonSerializer serializer)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>
			{
				{ "position", value.position },
				{ "rotation", value.rotation }
			};
			serializer.Serialize(writer, dictionary);
		}

		// Token: 0x06000066 RID: 102 RVA: 0x000042A8 File Offset: 0x000024A8
		public override Pose ReadJson(JsonReader reader, Type objectType, Pose existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			JObject jobject = serializer.Deserialize<JObject>(reader);
			bool flag = jobject == null;
			Pose pose;
			if (flag)
			{
				pose = Pose.identity;
			}
			else
			{
				JObject jobject2 = jobject.Value<JObject>("position");
				Vector3 vector = ((jobject2 != null) ? jobject2.ToObject<Vector3>(serializer) : default(Vector3));
				JObject jobject3 = jobject.Value<JObject>("rotation");
				pose = new Pose(vector, (jobject3 != null) ? jobject3.ToObject<Quaternion>(serializer) : default(Quaternion));
			}
			return pose;
		}
	}
}
