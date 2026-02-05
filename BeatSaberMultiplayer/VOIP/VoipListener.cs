using System;
using BeatSaberMultiplayer.Configuration;
using BeatSaberMultiplayer.Data;
using NSpeex;
using UnityEngine;

namespace BeatSaberMultiplayer.VOIP
{
	// Token: 0x0200008B RID: 139
	public class VoipListener : MonoBehaviour
	{
		// Token: 0x14000033 RID: 51
		// (add) Token: 0x06000967 RID: 2407 RVA: 0x0002645C File Offset: 0x0002465C
		// (remove) Token: 0x06000968 RID: 2408 RVA: 0x00026494 File Offset: 0x00024694
		public event Action<VoipFragment> OnAudioGenerated;

		// Token: 0x17000284 RID: 644
		// (get) Token: 0x06000969 RID: 2409 RVA: 0x000264C9 File Offset: 0x000246C9
		// (set) Token: 0x0600096A RID: 2410 RVA: 0x000264D4 File Offset: 0x000246D4
		public bool isListening
		{
			get
			{
				return this._isListening;
			}
			set
			{
				if (!this._isListening && value && this.recordingBuffer != null)
				{
					this.index += 3;
					this.lastPos = Math.Max(Microphone.GetPosition(this._usedMicrophone) - this.recordingBuffer.Length, 0);
				}
				this._isListening = value;
			}
		}

		// Token: 0x0600096B RID: 2411 RVA: 0x0002652B File Offset: 0x0002472B
		private void Awake()
		{
			this._usedMicrophone = PluginConfig.Instance.MicphoneName;
		}

		// Token: 0x0600096C RID: 2412 RVA: 0x0002653D File Offset: 0x0002473D
		private void Instance_voiceChatMicrophoneChanged(string newMic)
		{
			if (this.recording != null)
			{
				this.StopRecording();
				this._usedMicrophone = newMic;
				this.StartRecording();
			}
		}

		// Token: 0x0600096D RID: 2413 RVA: 0x00026560 File Offset: 0x00024760
		public void StartRecording()
		{
			if (Microphone.devices.Length == 0)
			{
				return;
			}
			this.inputFreq = AudioUtils.GetFreqForMic(null);
			this.encoder = SpeexCodex.Create(BandMode.Wide);
			int num = (int)((float)this.inputFreq / (float)AudioUtils.GetFrequency(this.encoder.mode) * (float)this.encoder.dataSize);
			this.recordingBuffer = new float[num];
			this.resampleBuffer = new float[this.encoder.dataSize];
			if (AudioUtils.GetFrequency(this.encoder.mode) == this.inputFreq)
			{
				this.recordingBuffer = this.resampleBuffer;
			}
			if (this._usedMicrophone.ToLower() == MicrophoneOpt.Auto.ToString().ToLower())
			{
				this._usedMicrophone = Microphone.devices[0];
			}
			foreach (string text in Microphone.devices)
			{
				if (text.ToLower().Contains(this._usedMicrophone))
				{
					this._usedMicrophone = text;
					break;
				}
			}
			this.recording = Microphone.Start(this._usedMicrophone, true, 20, this.inputFreq);
		}

		// Token: 0x0600096E RID: 2414 RVA: 0x00026680 File Offset: 0x00024880
		public void StopRecording()
		{
			Microphone.End(this._usedMicrophone);
			Object.Destroy(this.recording);
			this.recording = null;
		}

		// Token: 0x0600096F RID: 2415 RVA: 0x000266A0 File Offset: 0x000248A0
		private void Update()
		{
			if (this.recording == null)
			{
				return;
			}
			int position = Microphone.GetPosition(this._usedMicrophone);
			int i = position - this.lastPos;
			if (position < this.lastPos)
			{
				this.lastPos = 0;
				i = position;
			}
			while (i >= this.recordingBuffer.Length)
			{
				if (this._isListening && this.recording.GetData(this.recordingBuffer, this.lastPos))
				{
					this.index++;
					if (this.OnAudioGenerated != null)
					{
						if (this.recordingBuffer != this.resampleBuffer)
						{
							AudioUtils.Resample(this.recordingBuffer, this.resampleBuffer, this.inputFreq, AudioUtils.GetFrequency(this.encoder.mode), 1);
						}
						byte[] array = this.encoder.Encode(this.resampleBuffer);
						VoipFragment voipFragment = new VoipFragment(0L, this.index, array, this.encoder.mode);
						Action<VoipFragment> onAudioGenerated = this.OnAudioGenerated;
						if (onAudioGenerated != null)
						{
							onAudioGenerated(voipFragment);
						}
					}
				}
				i -= this.recordingBuffer.Length;
				this.lastPos += this.recordingBuffer.Length;
			}
		}

		// Token: 0x06000970 RID: 2416 RVA: 0x000267CA File Offset: 0x000249CA
		private void OnDestroy()
		{
			this.StopRecording();
		}

		// Token: 0x04000483 RID: 1155
		private AudioClip recording;

		// Token: 0x04000484 RID: 1156
		private float[] recordingBuffer;

		// Token: 0x04000485 RID: 1157
		private float[] resampleBuffer;

		// Token: 0x04000486 RID: 1158
		private SpeexCodex encoder;

		// Token: 0x04000487 RID: 1159
		private int lastPos;

		// Token: 0x04000488 RID: 1160
		private int index;

		// Token: 0x0400048A RID: 1162
		public BandMode max = BandMode.Wide;

		// Token: 0x0400048B RID: 1163
		public int inputFreq;

		// Token: 0x0400048C RID: 1164
		private bool _isListening;

		// Token: 0x0400048D RID: 1165
		private bool _isRecording;

		// Token: 0x0400048E RID: 1166
		private string _usedMicrophone;
	}
}
