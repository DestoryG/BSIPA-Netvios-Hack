using System;
using System.Linq;
using NSpeex;
using UnityEngine;

namespace BeatSaberMultiplayer.VOIP
{
	// Token: 0x02000089 RID: 137
	public static class AudioUtils
	{
		// Token: 0x06000954 RID: 2388 RVA: 0x00026070 File Offset: 0x00024270
		public static void ApplyGain(float[] samples, float gain)
		{
			for (int i = 0; i < samples.Length; i++)
			{
				samples[i] *= gain;
			}
		}

		// Token: 0x06000955 RID: 2389 RVA: 0x00026097 File Offset: 0x00024297
		public static float GetMaxAmplitude(float[] samples)
		{
			return samples.Max();
		}

		// Token: 0x06000956 RID: 2390 RVA: 0x0002609F File Offset: 0x0002429F
		public static int GetFrequency(BandMode mode)
		{
			switch (mode)
			{
			case BandMode.Narrow:
				return 8000;
			case BandMode.Wide:
				return 16000;
			case BandMode.UltraWide:
				return 32000;
			default:
				return 8000;
			}
		}

		// Token: 0x06000957 RID: 2391 RVA: 0x000260CC File Offset: 0x000242CC
		public static void Resample(float[] source, float[] target, int inputSampleRate, int outputSampleRate, int outputChannelsNum = 1)
		{
			AudioUtils.Resample(source, target, source.Length, target.Length, inputSampleRate, outputSampleRate, outputChannelsNum);
		}

		// Token: 0x06000958 RID: 2392 RVA: 0x000260E0 File Offset: 0x000242E0
		public static void Resample(float[] source, float[] target, int inputNum, int outputNum, int inputSampleRate, int outputSampleRate, int outputChannelsNum)
		{
			float num = (float)inputSampleRate / (float)outputSampleRate;
			if (num % 1f <= 1E-45f)
			{
				int num2 = Mathf.RoundToInt(num);
				for (int i = 0; i < outputNum / outputChannelsNum; i++)
				{
					if (i * num2 >= inputNum)
					{
						return;
					}
					for (int j = 0; j < outputChannelsNum; j++)
					{
						target[i * outputChannelsNum + j] = source[i * num2];
					}
				}
			}
			else if (num > 1f)
			{
				for (int k = 0; k < outputNum / outputChannelsNum; k++)
				{
					if (Mathf.CeilToInt((float)k * num) >= inputNum)
					{
						return;
					}
					for (int l = 0; l < outputChannelsNum; l++)
					{
						target[k * outputChannelsNum + l] = Mathf.Lerp(source[Mathf.FloorToInt((float)k * num)], source[Mathf.CeilToInt((float)k * num)], num % 1f);
					}
				}
			}
			else
			{
				int num3 = 0;
				while (num3 < outputNum / outputChannelsNum && Mathf.FloorToInt((float)num3 * num) < inputNum)
				{
					for (int m = 0; m < outputChannelsNum; m++)
					{
						target[num3 * outputChannelsNum + m] = source[Mathf.FloorToInt((float)num3 * num)];
					}
					num3++;
				}
			}
		}

		// Token: 0x06000959 RID: 2393 RVA: 0x000261EC File Offset: 0x000243EC
		public static int GetFreqForMic(string deviceName = null)
		{
			int num;
			int num2;
			Microphone.GetDeviceCaps(deviceName, ref num, ref num2);
			if (num < 16000)
			{
				return num2;
			}
			if (AudioUtils.FindClosestFreq(num, num2) != 0)
			{
				return AudioUtils.FindClosestFreq(num, num2);
			}
			return num;
		}

		// Token: 0x0600095A RID: 2394 RVA: 0x00026220 File Offset: 0x00024420
		public static int FindClosestFreq(int minFreq, int maxFreq)
		{
			foreach (int num in AudioUtils.possibleSampleRates)
			{
				if (num >= minFreq && num <= maxFreq)
				{
					return num;
				}
			}
			return 0;
		}

		// Token: 0x0400047A RID: 1146
		public static int[] possibleSampleRates = new int[] { 16000, 32000, 48000, 96000, 192000, 22050, 44100, 88200, 176400 };
	}
}
