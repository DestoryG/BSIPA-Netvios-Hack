using System;
using System.Linq;
using System.Reflection;
using System.Text;
using Valve.VR;

namespace CustomAvatar.Tracking
{
	// Token: 0x02000027 RID: 39
	internal static class OpenVRWrapper
	{
		// Token: 0x0600008D RID: 141 RVA: 0x00004B00 File Offset: 0x00002D00
		internal static string[] GetTrackedDeviceSerialNumbers()
		{
			string[] array = new string[64];
			for (uint num = 0U; num < 64U; num += 1U)
			{
				array[(int)num] = OpenVRWrapper.GetStringTrackedDeviceProperty(num, ETrackedDeviceProperty.Prop_SerialNumber_String);
			}
			return array;
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00004B3C File Offset: 0x00002D3C
		internal static TrackedDeviceRole GetTrackedDeviceRole(uint deviceIndex)
		{
			string name = OpenVRWrapper.GetStringTrackedDeviceProperty(deviceIndex, ETrackedDeviceProperty.Prop_ControllerType_String);
			bool flag = name == null;
			TrackedDeviceRole trackedDeviceRole;
			if (flag)
			{
				trackedDeviceRole = TrackedDeviceRole.Unknown;
			}
			else
			{
				FieldInfo fieldInfo = typeof(TrackedDeviceRole).GetFields().FirstOrDefault(delegate(FieldInfo f)
				{
					TrackedDeviceTypeAttribute customAttribute = f.GetCustomAttribute<TrackedDeviceTypeAttribute>();
					return ((customAttribute != null) ? customAttribute.Name : null) == name;
				});
				bool flag2 = fieldInfo == null;
				if (flag2)
				{
					trackedDeviceRole = TrackedDeviceRole.Unknown;
				}
				else
				{
					trackedDeviceRole = (TrackedDeviceRole)fieldInfo.GetValue(null);
				}
			}
			return trackedDeviceRole;
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00004BB8 File Offset: 0x00002DB8
		internal static string GetStringTrackedDeviceProperty(uint deviceIndex, ETrackedDeviceProperty property)
		{
			ETrackedPropertyError etrackedPropertyError = ETrackedPropertyError.TrackedProp_Success;
			uint stringTrackedDeviceProperty = OpenVR.System.GetStringTrackedDeviceProperty(deviceIndex, property, null, 0U, ref etrackedPropertyError);
			bool flag = stringTrackedDeviceProperty > 0U;
			string text;
			if (flag)
			{
				StringBuilder stringBuilder = new StringBuilder((int)stringTrackedDeviceProperty);
				OpenVR.System.GetStringTrackedDeviceProperty(deviceIndex, property, stringBuilder, stringTrackedDeviceProperty, ref etrackedPropertyError);
				text = stringBuilder.ToString();
			}
			else
			{
				text = null;
			}
			return text;
		}
	}
}
