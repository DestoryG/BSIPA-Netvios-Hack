using System;
using System.IO;
using BeatSaberMultiplayer.Data;
using BeatSaberMultiplayer.VOIP;
using UnityEngine;

namespace BeatSaberMultiplayer
{
	// Token: 0x0200004D RID: 77
	public class MultiPlayerController : PlayerController
	{
		// Token: 0x060006B3 RID: 1715 RVA: 0x0001B8DC File Offset: 0x00019ADC
		public void Start()
		{
			this._voipSource = base.gameObject.AddComponent<AudioSource>();
			this._voipSource.clip = null;
			this._voipSource.spatialize = false;
			this._voipSource.loop = true;
			this._voipSource.Play();
			this._voipSource.volume = 0.8f;
		}

		// Token: 0x060006B4 RID: 1716 RVA: 0x0001B939 File Offset: 0x00019B39
		public void SetVoIPVolume(float newVolume)
		{
			if (this._voipSource != null)
			{
				this._voipSource.volume = newVolume;
			}
		}

		// Token: 0x060006B5 RID: 1717 RVA: 0x0001B955 File Offset: 0x00019B55
		public void SetSpatialAudioState(bool spatialAudio)
		{
			if (this._voipSource != null)
			{
				this._voipSource.spatialize = spatialAudio;
			}
		}

		// Token: 0x060006B6 RID: 1718 RVA: 0x0001B974 File Offset: 0x00019B74
		public void Update()
		{
			if (this._voipSource != null)
			{
				if (this._voipFragQueue.Length <= 0L)
				{
					this._voipPlaying = false;
				}
				if (this._voipPlaying)
				{
					this._silentFrames = 0;
					return;
				}
			}
			else
			{
				this._silentFrames = 999;
			}
		}

		// Token: 0x060006B7 RID: 1719 RVA: 0x0001B9C0 File Offset: 0x00019BC0
		public void VoIPUpdate()
		{
			this._silentFrames++;
		}

		// Token: 0x060006B8 RID: 1720 RVA: 0x0001B9D0 File Offset: 0x00019BD0
		public void PlayVoIPFragment(float[] data, int fragIndex)
		{
			if (this._voipSource != null)
			{
				if (this._lastVoipFragIndex + 1 != fragIndex || this._silentFrames > 15)
				{
					this._voipFragQueue.Flush();
					this._voipDelayCounter = 0;
				}
				else
				{
					this._voipDelayCounter++;
					if (!this._voipPlaying && this._voipDelayCounter >= 1)
					{
						this._voipPlaying = true;
					}
				}
				this._lastVoipFragIndex = fragIndex;
				this._voipFragQueue.Write(data, 0, data.Length);
			}
		}

		// Token: 0x060006B9 RID: 1721 RVA: 0x0001BA54 File Offset: 0x00019C54
		private void OnAudioFilterRead(float[] data, int channels)
		{
			if (!this._voipPlaying)
			{
				return;
			}
			int outputSampleRate = AudioSettings.outputSampleRate;
			int num = data.Length / channels;
			if (this._voipBuffer == null || this._voipBuffer.Length < Mathf.CeilToInt((float)num / ((float)outputSampleRate / 16000f)))
			{
				this._voipBuffer = new float[Mathf.CeilToInt((float)num / ((float)AudioSettings.outputSampleRate / 16000f))];
				Logger.log.Debug(string.Format("Created new VoIP player buffer! Size: {0}, Channels: {1}, Resampling rate: {2}x", num, channels, (float)outputSampleRate / 16000f));
			}
			int num2 = this._voipFragQueue.Read(this._voipBuffer, 0, Mathf.CeilToInt((float)num / ((float)outputSampleRate / 16000f)));
			AudioUtils.Resample(this._voipBuffer, data, num2, data.Length, 16000, outputSampleRate, channels);
		}

		// Token: 0x04000321 RID: 801
		public Player player;

		// Token: 0x04000322 RID: 802
		private AudioSource _voipSource;

		// Token: 0x04000323 RID: 803
		private int _lastVoipFragIndex;

		// Token: 0x04000324 RID: 804
		private FifoFloatStream _voipFragQueue = new FifoFloatStream();

		// Token: 0x04000325 RID: 805
		private int _voipDelayCounter;

		// Token: 0x04000326 RID: 806
		private bool _voipPlaying;

		// Token: 0x04000327 RID: 807
		private const int _voipDelay = 1;

		// Token: 0x04000328 RID: 808
		private float[] _voipBuffer;

		// Token: 0x04000329 RID: 809
		private int _silentFrames;
	}
}
