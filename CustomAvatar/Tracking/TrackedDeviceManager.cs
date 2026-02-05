using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CustomAvatar.Utilities;
using DynamicOpenVR;
using UnityEngine;
using UnityEngine.XR;

namespace CustomAvatar.Tracking
{
	// Token: 0x02000028 RID: 40
	internal class TrackedDeviceManager : MonoBehaviour
	{
		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000090 RID: 144 RVA: 0x00004C0B File Offset: 0x00002E0B
		public TrackedDeviceState head { get; } = new TrackedDeviceState();

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000091 RID: 145 RVA: 0x00004C13 File Offset: 0x00002E13
		public TrackedDeviceState leftHand { get; } = new TrackedDeviceState();

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000092 RID: 146 RVA: 0x00004C1B File Offset: 0x00002E1B
		public TrackedDeviceState rightHand { get; } = new TrackedDeviceState();

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000093 RID: 147 RVA: 0x00004C23 File Offset: 0x00002E23
		public TrackedDeviceState leftFoot { get; } = new TrackedDeviceState();

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000094 RID: 148 RVA: 0x00004C2B File Offset: 0x00002E2B
		public TrackedDeviceState rightFoot { get; } = new TrackedDeviceState();

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000095 RID: 149 RVA: 0x00004C33 File Offset: 0x00002E33
		public TrackedDeviceState waist { get; } = new TrackedDeviceState();

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06000096 RID: 150 RVA: 0x00004C3C File Offset: 0x00002E3C
		// (remove) Token: 0x06000097 RID: 151 RVA: 0x00004C74 File Offset: 0x00002E74
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<TrackedDeviceState, DeviceUse> deviceAdded;

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x06000098 RID: 152 RVA: 0x00004CAC File Offset: 0x00002EAC
		// (remove) Token: 0x06000099 RID: 153 RVA: 0x00004CE4 File Offset: 0x00002EE4
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<TrackedDeviceState, DeviceUse> deviceRemoved;

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x0600009A RID: 154 RVA: 0x00004D1C File Offset: 0x00002F1C
		// (remove) Token: 0x0600009B RID: 155 RVA: 0x00004D54 File Offset: 0x00002F54
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<TrackedDeviceState, DeviceUse> deviceTrackingAcquired;

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x0600009C RID: 156 RVA: 0x00004D8C File Offset: 0x00002F8C
		// (remove) Token: 0x0600009D RID: 157 RVA: 0x00004DC4 File Offset: 0x00002FC4
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<TrackedDeviceState, DeviceUse> deviceTrackingLost;

		// Token: 0x0600009E RID: 158 RVA: 0x00004DFC File Offset: 0x00002FFC
		public void Start()
		{
			this._isOpenVRRunning = OpenVRUtilities.isInitialized;
			InputDevices.deviceConnected += delegate(InputDevice device)
			{
				this.UpdateInputDevices();
			};
			InputDevices.deviceDisconnected += delegate(InputDevice device)
			{
				this.UpdateInputDevices();
			};
			InputDevices.deviceConfigChanged += delegate(InputDevice device)
			{
				this.UpdateInputDevices();
			};
			this.UpdateInputDevices();
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00004E54 File Offset: 0x00003054
		private void Update()
		{
			List<InputDevice> list = new List<InputDevice>();
			InputDevices.GetDevices(list);
			InputDevice? inputDevice = null;
			InputDevice? inputDevice2 = null;
			InputDevice? inputDevice3 = null;
			InputDevice? inputDevice4 = null;
			InputDevice? inputDevice5 = null;
			InputDevice? inputDevice6 = null;
			foreach (InputDevice inputDevice7 in list)
			{
				bool flag = inputDevice7.name == this.head.name;
				if (flag)
				{
					inputDevice = new InputDevice?(inputDevice7);
				}
				bool flag2 = inputDevice7.name == this.leftHand.name;
				if (flag2)
				{
					inputDevice2 = new InputDevice?(inputDevice7);
				}
				bool flag3 = inputDevice7.name == this.rightHand.name;
				if (flag3)
				{
					inputDevice3 = new InputDevice?(inputDevice7);
				}
				bool flag4 = inputDevice7.name == this.waist.name;
				if (flag4)
				{
					inputDevice4 = new InputDevice?(inputDevice7);
				}
				bool flag5 = inputDevice7.name == this.leftFoot.name;
				if (flag5)
				{
					inputDevice5 = new InputDevice?(inputDevice7);
				}
				bool flag6 = inputDevice7.name == this.rightFoot.name;
				if (flag6)
				{
					inputDevice6 = new InputDevice?(inputDevice7);
				}
			}
			this.UpdateTrackedDevice(this.head, inputDevice, DeviceUse.Head);
			this.UpdateTrackedDevice(this.leftHand, inputDevice2, DeviceUse.LeftHand);
			this.UpdateTrackedDevice(this.rightHand, inputDevice3, DeviceUse.RightHand);
			this.UpdateTrackedDevice(this.waist, inputDevice4, DeviceUse.Waist);
			this.UpdateTrackedDevice(this.leftFoot, inputDevice5, DeviceUse.LeftFoot);
			this.UpdateTrackedDevice(this.rightFoot, inputDevice6, DeviceUse.RightFoot);
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00005024 File Offset: 0x00003224
		private void UpdateInputDevices()
		{
			List<InputDevice> list = new List<InputDevice>();
			Queue<InputDevice> queue = new Queue<InputDevice>();
			Dictionary<string, uint> dictionary = new Dictionary<string, uint>();
			InputDevices.GetDevices(list);
			Dictionary<string, TrackedDeviceRole> dictionary2 = new Dictionary<string, TrackedDeviceRole>(list.Count);
			bool isOpenVRRunning = this._isOpenVRRunning;
			if (isOpenVRRunning)
			{
				string[] trackedDeviceSerialNumbers = OpenVRWrapper.GetTrackedDeviceSerialNumbers();
				uint num = 0U;
				while ((ulong)num < (ulong)((long)trackedDeviceSerialNumbers.Length))
				{
					bool flag = string.IsNullOrEmpty(trackedDeviceSerialNumbers[(int)num]);
					if (!flag)
					{
						Plugin.logger.Debug(string.Format("Got serial number \"{0}\" for device at index {1}", trackedDeviceSerialNumbers[(int)num], num));
						dictionary.Add(trackedDeviceSerialNumbers[(int)num], num);
					}
					num += 1U;
				}
			}
			InputDevice? inputDevice = null;
			InputDevice? inputDevice2 = null;
			InputDevice? inputDevice3 = null;
			InputDevice? inputDevice4 = null;
			InputDevice? inputDevice5 = null;
			InputDevice? inputDevice6 = null;
			int num2 = 0;
			foreach (InputDevice inputDevice7 in list)
			{
				bool flag2 = !inputDevice7.isValid;
				if (!flag2)
				{
					dictionary2.Add(inputDevice7.name, TrackedDeviceRole.Unknown);
					bool flag3 = !this._foundDevices.Contains(inputDevice7.name);
					if (flag3)
					{
						Plugin.logger.Info(string.Concat(new string[] { "Found new input device \"", inputDevice7.name, "\" with serial number \"", inputDevice7.serialNumber, "\"" }));
						this._foundDevices.Add(inputDevice7.name);
					}
					bool flag4 = inputDevice7.HasCharacteristics(1);
					if (flag4)
					{
						inputDevice = new InputDevice?(inputDevice7);
					}
					else
					{
						bool flag5 = inputDevice7.HasCharacteristics(260);
						if (flag5)
						{
							inputDevice2 = new InputDevice?(inputDevice7);
						}
						else
						{
							bool flag6 = inputDevice7.HasCharacteristics(516);
							if (flag6)
							{
								inputDevice3 = new InputDevice?(inputDevice7);
							}
							else
							{
								bool flag7 = inputDevice7.HasCharacteristics(32) && !inputDevice7.HasCharacteristics(128);
								if (flag7)
								{
									uint num3;
									bool flag8 = this._isOpenVRRunning && !string.IsNullOrEmpty(inputDevice7.serialNumber) && dictionary.TryGetValue(inputDevice7.serialNumber, out num3);
									if (flag8)
									{
										TrackedDeviceRole trackedDeviceRole = OpenVRWrapper.GetTrackedDeviceRole(num3);
										dictionary2[inputDevice7.name] = trackedDeviceRole;
										Plugin.logger.Info(string.Format("Tracker \"{0}\" has role {1}", inputDevice7.name, trackedDeviceRole));
										switch (trackedDeviceRole)
										{
										case TrackedDeviceRole.LeftFoot:
											inputDevice5 = new InputDevice?(inputDevice7);
											break;
										case TrackedDeviceRole.RightFoot:
											inputDevice6 = new InputDevice?(inputDevice7);
											break;
										case TrackedDeviceRole.LeftShoulder:
										case TrackedDeviceRole.RightShoulder:
											goto IL_02B9;
										case TrackedDeviceRole.Waist:
											inputDevice4 = new InputDevice?(inputDevice7);
											break;
										default:
											goto IL_02B9;
										}
										goto IL_02D2;
										IL_02B9:
										queue.Enqueue(inputDevice7);
									}
									else
									{
										queue.Enqueue(inputDevice7);
									}
									IL_02D2:
									num2++;
								}
							}
						}
					}
				}
			}
			bool flag9 = inputDevice5 == null && num2 >= 2 && queue.Count > 0;
			if (flag9)
			{
				inputDevice5 = new InputDevice?(queue.Dequeue());
			}
			bool flag10 = inputDevice6 == null && num2 >= 2 && queue.Count > 0;
			if (flag10)
			{
				inputDevice6 = new InputDevice?(queue.Dequeue());
			}
			bool flag11 = inputDevice4 == null && queue.Count > 0;
			if (flag11)
			{
				inputDevice4 = new InputDevice?(queue.Dequeue());
			}
			this.AssignTrackedDevice(this.head, inputDevice, DeviceUse.Head, (inputDevice != null) ? dictionary2[inputDevice.Value.name] : TrackedDeviceRole.Unknown);
			this.AssignTrackedDevice(this.leftHand, inputDevice2, DeviceUse.LeftHand, (inputDevice2 != null) ? dictionary2[inputDevice2.Value.name] : TrackedDeviceRole.Unknown);
			this.AssignTrackedDevice(this.rightHand, inputDevice3, DeviceUse.RightHand, (inputDevice3 != null) ? dictionary2[inputDevice3.Value.name] : TrackedDeviceRole.Unknown);
			this.AssignTrackedDevice(this.waist, inputDevice4, DeviceUse.Waist, (inputDevice4 != null) ? dictionary2[inputDevice4.Value.name] : TrackedDeviceRole.Unknown);
			this.AssignTrackedDevice(this.leftFoot, inputDevice5, DeviceUse.LeftFoot, (inputDevice5 != null) ? dictionary2[inputDevice5.Value.name] : TrackedDeviceRole.Unknown);
			this.AssignTrackedDevice(this.rightFoot, inputDevice6, DeviceUse.RightFoot, (inputDevice6 != null) ? dictionary2[inputDevice6.Value.name] : TrackedDeviceRole.Unknown);
			using (List<string>.Enumerator enumerator2 = this._foundDevices.ToList<string>().GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					string deviceName = enumerator2.Current;
					bool flag12 = !list.Exists((InputDevice d) => d.name == deviceName);
					if (flag12)
					{
						Plugin.logger.Info("Lost device \"" + deviceName + "\"");
						this._foundDevices.Remove(deviceName);
					}
				}
			}
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x000055A8 File Offset: 0x000037A8
		private void AssignTrackedDevice(TrackedDeviceState deviceState, InputDevice? possibleInputDevice, DeviceUse use, TrackedDeviceRole deviceRole)
		{
			bool flag = possibleInputDevice != null && !deviceState.found;
			if (flag)
			{
				InputDevice value = possibleInputDevice.Value;
				Plugin.logger.Info(string.Format("Using device \"{0}\" as {1}", value.name, use));
				deviceState.name = value.name;
				deviceState.serialNumber = value.serialNumber;
				deviceState.found = true;
				deviceState.role = deviceRole;
				Action<TrackedDeviceState, DeviceUse> action = this.deviceAdded;
				if (action != null)
				{
					action(deviceState, use);
				}
			}
			bool flag2 = possibleInputDevice == null && deviceState.found;
			if (flag2)
			{
				Plugin.logger.Info(string.Format("Lost device \"{0}\" that was used as {1}", deviceState.name, use));
				deviceState.name = null;
				deviceState.serialNumber = null;
				deviceState.found = false;
				deviceState.role = TrackedDeviceRole.Unknown;
				Action<TrackedDeviceState, DeviceUse> action2 = this.deviceRemoved;
				if (action2 != null)
				{
					action2(deviceState, use);
				}
			}
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x000056AC File Offset: 0x000038AC
		private void UpdateTrackedDevice(TrackedDeviceState deviceState, InputDevice? possibleInputDevice, DeviceUse use)
		{
			bool flag = possibleInputDevice == null;
			if (!flag)
			{
				InputDevice value = possibleInputDevice.Value;
				bool flag3;
				bool flag2 = !value.TryGetFeatureValue(CommonUsages.isTracked, ref flag3) || !flag3;
				if (flag2)
				{
					bool tracked = deviceState.tracked;
					if (tracked)
					{
						Plugin.logger.Info("Lost tracking of device \"" + deviceState.name + "\"");
						deviceState.tracked = false;
						Action<TrackedDeviceState, DeviceUse> action = this.deviceTrackingLost;
						if (action != null)
						{
							action(deviceState, use);
						}
					}
				}
				else
				{
					bool flag4 = !deviceState.tracked;
					if (flag4)
					{
						Plugin.logger.Info("Acquired tracking of device \"" + deviceState.name + "\"");
						deviceState.tracked = true;
						Action<TrackedDeviceState, DeviceUse> action2 = this.deviceTrackingAcquired;
						if (action2 != null)
						{
							action2(deviceState, use);
						}
					}
					Vector3 roomCenter = BeatSaberUtil.GetRoomCenter();
					Quaternion roomRotation = BeatSaberUtil.GetRoomRotation();
					Vector3 vector;
					bool flag5 = value.TryGetFeatureValue(CommonUsages.devicePosition, ref vector);
					if (flag5)
					{
						deviceState.position = roomCenter + roomRotation * vector;
					}
					Quaternion quaternion;
					bool flag6 = value.TryGetFeatureValue(CommonUsages.deviceRotation, ref quaternion);
					if (flag6)
					{
						deviceState.rotation = roomRotation * quaternion;
						string name = deviceState.name;
						bool flag7 = name != null && name.StartsWith("d4vr_tracker_") && (use == DeviceUse.LeftFoot || use == DeviceUse.RightFoot);
						if (flag7)
						{
							deviceState.rotation *= Quaternion.Euler(-90f, 180f, 0f);
						}
						bool flag8 = deviceState.role == TrackedDeviceRole.KinectToVrTracker;
						if (flag8)
						{
							bool flag9 = use == DeviceUse.Waist;
							if (flag9)
							{
								deviceState.rotation *= Quaternion.Euler(-90f, 180f, 0f);
							}
							bool flag10 = use == DeviceUse.LeftFoot || use == DeviceUse.RightFoot;
							if (flag10)
							{
								deviceState.rotation *= Quaternion.Euler(0f, 180f, 0f);
							}
						}
					}
				}
			}
		}

		// Token: 0x0400013D RID: 317
		private readonly HashSet<string> _foundDevices = new HashSet<string>();

		// Token: 0x0400013E RID: 318
		private bool _isOpenVRRunning;
	}
}
