using System;
using System.Runtime.InteropServices;

namespace MonoMod.RuntimeDetour.Platforms
{
	// Token: 0x02000354 RID: 852
	internal class DetourNativeMonoPosixPlatform : IDetourNativePlatform
	{
		// Token: 0x060013DF RID: 5087 RVA: 0x00048013 File Offset: 0x00046213
		public DetourNativeMonoPosixPlatform(IDetourNativePlatform inner)
		{
			this.Inner = inner;
			this._Pagesize = DetourNativeMonoPosixPlatform.sysconf(DetourNativeMonoPosixPlatform.SysconfName._SC_PAGESIZE, (DetourNativeMonoPosixPlatform.Errno)0);
		}

		// Token: 0x060013E0 RID: 5088 RVA: 0x00048030 File Offset: 0x00046230
		private static string GetLastError(string name)
		{
			int num = DetourNativeMonoPosixPlatform._GetLastError();
			DetourNativeMonoPosixPlatform.Errno errno;
			if (DetourNativeMonoPosixPlatform.ToErrno(num, out errno) == 0)
			{
				return string.Format("{0} returned {1}", name, errno);
			}
			return string.Format("{0} returned 0x${1:X8}", name, num);
		}

		// Token: 0x060013E1 RID: 5089 RVA: 0x00048070 File Offset: 0x00046270
		private void SetMemPerms(IntPtr start, ulong len, DetourNativeMonoPosixPlatform.MmapProts prot)
		{
			long pagesize = this._Pagesize;
			long num = (long)start & ~(pagesize - 1L);
			long num2 = ((long)start + (long)len + pagesize - 1L) & ~(pagesize - 1L);
			if (DetourNativeMonoPosixPlatform.mprotect((IntPtr)num, (ulong)(num2 - num), prot) != 0)
			{
				throw new Exception(DetourNativeMonoPosixPlatform.GetLastError("mprotect"));
			}
		}

		// Token: 0x060013E2 RID: 5090 RVA: 0x000480C6 File Offset: 0x000462C6
		public void MakeWritable(IntPtr src, uint size)
		{
			this.SetMemPerms(src, (ulong)size, DetourNativeMonoPosixPlatform.MmapProts.PROT_READ | DetourNativeMonoPosixPlatform.MmapProts.PROT_WRITE | DetourNativeMonoPosixPlatform.MmapProts.PROT_EXEC);
		}

		// Token: 0x060013E3 RID: 5091 RVA: 0x000480C6 File Offset: 0x000462C6
		public void MakeExecutable(IntPtr src, uint size)
		{
			this.SetMemPerms(src, (ulong)size, DetourNativeMonoPosixPlatform.MmapProts.PROT_READ | DetourNativeMonoPosixPlatform.MmapProts.PROT_WRITE | DetourNativeMonoPosixPlatform.MmapProts.PROT_EXEC);
		}

		// Token: 0x060013E4 RID: 5092 RVA: 0x000480D2 File Offset: 0x000462D2
		public void FlushICache(IntPtr src, uint size)
		{
			this.Inner.FlushICache(src, size);
		}

		// Token: 0x060013E5 RID: 5093 RVA: 0x000480E1 File Offset: 0x000462E1
		public NativeDetourData Create(IntPtr from, IntPtr to, byte? type)
		{
			return this.Inner.Create(from, to, type);
		}

		// Token: 0x060013E6 RID: 5094 RVA: 0x000480F1 File Offset: 0x000462F1
		public void Free(NativeDetourData detour)
		{
			this.Inner.Free(detour);
		}

		// Token: 0x060013E7 RID: 5095 RVA: 0x000480FF File Offset: 0x000462FF
		public void Apply(NativeDetourData detour)
		{
			this.Inner.Apply(detour);
		}

		// Token: 0x060013E8 RID: 5096 RVA: 0x0004810D File Offset: 0x0004630D
		public void Copy(IntPtr src, IntPtr dst, byte type)
		{
			this.Inner.Copy(src, dst, type);
		}

		// Token: 0x060013E9 RID: 5097 RVA: 0x0004811D File Offset: 0x0004631D
		public IntPtr MemAlloc(uint size)
		{
			return this.Inner.MemAlloc(size);
		}

		// Token: 0x060013EA RID: 5098 RVA: 0x0004812B File Offset: 0x0004632B
		public void MemFree(IntPtr ptr)
		{
			this.Inner.MemFree(ptr);
		}

		// Token: 0x060013EB RID: 5099
		[DllImport("MonoPosixHelper", EntryPoint = "Mono_Posix_Syscall_sysconf", SetLastError = true)]
		public static extern long sysconf(DetourNativeMonoPosixPlatform.SysconfName name, DetourNativeMonoPosixPlatform.Errno defaultError);

		// Token: 0x060013EC RID: 5100
		[DllImport("MonoPosixHelper", EntryPoint = "Mono_Posix_Syscall_mprotect", SetLastError = true)]
		private static extern int mprotect(IntPtr start, ulong len, DetourNativeMonoPosixPlatform.MmapProts prot);

		// Token: 0x060013ED RID: 5101
		[DllImport("MonoPosixHelper", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Mono_Posix_Stdlib_GetLastError")]
		private static extern int _GetLastError();

		// Token: 0x060013EE RID: 5102
		[DllImport("MonoPosixHelper", EntryPoint = "Mono_Posix_ToErrno")]
		private static extern int ToErrno(int value, out DetourNativeMonoPosixPlatform.Errno rval);

		// Token: 0x04001009 RID: 4105
		private readonly IDetourNativePlatform Inner;

		// Token: 0x0400100A RID: 4106
		private readonly long _Pagesize;

		// Token: 0x02000355 RID: 853
		[Flags]
		private enum MmapProts
		{
			// Token: 0x0400100C RID: 4108
			PROT_READ = 1,
			// Token: 0x0400100D RID: 4109
			PROT_WRITE = 2,
			// Token: 0x0400100E RID: 4110
			PROT_EXEC = 4,
			// Token: 0x0400100F RID: 4111
			PROT_NONE = 0,
			// Token: 0x04001010 RID: 4112
			PROT_GROWSDOWN = 16777216,
			// Token: 0x04001011 RID: 4113
			PROT_GROWSUP = 33554432
		}

		// Token: 0x02000356 RID: 854
		public enum SysconfName
		{
			// Token: 0x04001013 RID: 4115
			_SC_ARG_MAX,
			// Token: 0x04001014 RID: 4116
			_SC_CHILD_MAX,
			// Token: 0x04001015 RID: 4117
			_SC_CLK_TCK,
			// Token: 0x04001016 RID: 4118
			_SC_NGROUPS_MAX,
			// Token: 0x04001017 RID: 4119
			_SC_OPEN_MAX,
			// Token: 0x04001018 RID: 4120
			_SC_STREAM_MAX,
			// Token: 0x04001019 RID: 4121
			_SC_TZNAME_MAX,
			// Token: 0x0400101A RID: 4122
			_SC_JOB_CONTROL,
			// Token: 0x0400101B RID: 4123
			_SC_SAVED_IDS,
			// Token: 0x0400101C RID: 4124
			_SC_REALTIME_SIGNALS,
			// Token: 0x0400101D RID: 4125
			_SC_PRIORITY_SCHEDULING,
			// Token: 0x0400101E RID: 4126
			_SC_TIMERS,
			// Token: 0x0400101F RID: 4127
			_SC_ASYNCHRONOUS_IO,
			// Token: 0x04001020 RID: 4128
			_SC_PRIORITIZED_IO,
			// Token: 0x04001021 RID: 4129
			_SC_SYNCHRONIZED_IO,
			// Token: 0x04001022 RID: 4130
			_SC_FSYNC,
			// Token: 0x04001023 RID: 4131
			_SC_MAPPED_FILES,
			// Token: 0x04001024 RID: 4132
			_SC_MEMLOCK,
			// Token: 0x04001025 RID: 4133
			_SC_MEMLOCK_RANGE,
			// Token: 0x04001026 RID: 4134
			_SC_MEMORY_PROTECTION,
			// Token: 0x04001027 RID: 4135
			_SC_MESSAGE_PASSING,
			// Token: 0x04001028 RID: 4136
			_SC_SEMAPHORES,
			// Token: 0x04001029 RID: 4137
			_SC_SHARED_MEMORY_OBJECTS,
			// Token: 0x0400102A RID: 4138
			_SC_AIO_LISTIO_MAX,
			// Token: 0x0400102B RID: 4139
			_SC_AIO_MAX,
			// Token: 0x0400102C RID: 4140
			_SC_AIO_PRIO_DELTA_MAX,
			// Token: 0x0400102D RID: 4141
			_SC_DELAYTIMER_MAX,
			// Token: 0x0400102E RID: 4142
			_SC_MQ_OPEN_MAX,
			// Token: 0x0400102F RID: 4143
			_SC_MQ_PRIO_MAX,
			// Token: 0x04001030 RID: 4144
			_SC_VERSION,
			// Token: 0x04001031 RID: 4145
			_SC_PAGESIZE,
			// Token: 0x04001032 RID: 4146
			_SC_RTSIG_MAX,
			// Token: 0x04001033 RID: 4147
			_SC_SEM_NSEMS_MAX,
			// Token: 0x04001034 RID: 4148
			_SC_SEM_VALUE_MAX,
			// Token: 0x04001035 RID: 4149
			_SC_SIGQUEUE_MAX,
			// Token: 0x04001036 RID: 4150
			_SC_TIMER_MAX,
			// Token: 0x04001037 RID: 4151
			_SC_BC_BASE_MAX,
			// Token: 0x04001038 RID: 4152
			_SC_BC_DIM_MAX,
			// Token: 0x04001039 RID: 4153
			_SC_BC_SCALE_MAX,
			// Token: 0x0400103A RID: 4154
			_SC_BC_STRING_MAX,
			// Token: 0x0400103B RID: 4155
			_SC_COLL_WEIGHTS_MAX,
			// Token: 0x0400103C RID: 4156
			_SC_EQUIV_CLASS_MAX,
			// Token: 0x0400103D RID: 4157
			_SC_EXPR_NEST_MAX,
			// Token: 0x0400103E RID: 4158
			_SC_LINE_MAX,
			// Token: 0x0400103F RID: 4159
			_SC_RE_DUP_MAX,
			// Token: 0x04001040 RID: 4160
			_SC_CHARCLASS_NAME_MAX,
			// Token: 0x04001041 RID: 4161
			_SC_2_VERSION,
			// Token: 0x04001042 RID: 4162
			_SC_2_C_BIND,
			// Token: 0x04001043 RID: 4163
			_SC_2_C_DEV,
			// Token: 0x04001044 RID: 4164
			_SC_2_FORT_DEV,
			// Token: 0x04001045 RID: 4165
			_SC_2_FORT_RUN,
			// Token: 0x04001046 RID: 4166
			_SC_2_SW_DEV,
			// Token: 0x04001047 RID: 4167
			_SC_2_LOCALEDEF,
			// Token: 0x04001048 RID: 4168
			_SC_PII,
			// Token: 0x04001049 RID: 4169
			_SC_PII_XTI,
			// Token: 0x0400104A RID: 4170
			_SC_PII_SOCKET,
			// Token: 0x0400104B RID: 4171
			_SC_PII_INTERNET,
			// Token: 0x0400104C RID: 4172
			_SC_PII_OSI,
			// Token: 0x0400104D RID: 4173
			_SC_POLL,
			// Token: 0x0400104E RID: 4174
			_SC_SELECT,
			// Token: 0x0400104F RID: 4175
			_SC_UIO_MAXIOV,
			// Token: 0x04001050 RID: 4176
			_SC_IOV_MAX = 60,
			// Token: 0x04001051 RID: 4177
			_SC_PII_INTERNET_STREAM,
			// Token: 0x04001052 RID: 4178
			_SC_PII_INTERNET_DGRAM,
			// Token: 0x04001053 RID: 4179
			_SC_PII_OSI_COTS,
			// Token: 0x04001054 RID: 4180
			_SC_PII_OSI_CLTS,
			// Token: 0x04001055 RID: 4181
			_SC_PII_OSI_M,
			// Token: 0x04001056 RID: 4182
			_SC_T_IOV_MAX,
			// Token: 0x04001057 RID: 4183
			_SC_THREADS,
			// Token: 0x04001058 RID: 4184
			_SC_THREAD_SAFE_FUNCTIONS,
			// Token: 0x04001059 RID: 4185
			_SC_GETGR_R_SIZE_MAX,
			// Token: 0x0400105A RID: 4186
			_SC_GETPW_R_SIZE_MAX,
			// Token: 0x0400105B RID: 4187
			_SC_LOGIN_NAME_MAX,
			// Token: 0x0400105C RID: 4188
			_SC_TTY_NAME_MAX,
			// Token: 0x0400105D RID: 4189
			_SC_THREAD_DESTRUCTOR_ITERATIONS,
			// Token: 0x0400105E RID: 4190
			_SC_THREAD_KEYS_MAX,
			// Token: 0x0400105F RID: 4191
			_SC_THREAD_STACK_MIN,
			// Token: 0x04001060 RID: 4192
			_SC_THREAD_THREADS_MAX,
			// Token: 0x04001061 RID: 4193
			_SC_THREAD_ATTR_STACKADDR,
			// Token: 0x04001062 RID: 4194
			_SC_THREAD_ATTR_STACKSIZE,
			// Token: 0x04001063 RID: 4195
			_SC_THREAD_PRIORITY_SCHEDULING,
			// Token: 0x04001064 RID: 4196
			_SC_THREAD_PRIO_INHERIT,
			// Token: 0x04001065 RID: 4197
			_SC_THREAD_PRIO_PROTECT,
			// Token: 0x04001066 RID: 4198
			_SC_THREAD_PROCESS_SHARED,
			// Token: 0x04001067 RID: 4199
			_SC_NPROCESSORS_CONF,
			// Token: 0x04001068 RID: 4200
			_SC_NPROCESSORS_ONLN,
			// Token: 0x04001069 RID: 4201
			_SC_PHYS_PAGES,
			// Token: 0x0400106A RID: 4202
			_SC_AVPHYS_PAGES,
			// Token: 0x0400106B RID: 4203
			_SC_ATEXIT_MAX,
			// Token: 0x0400106C RID: 4204
			_SC_PASS_MAX,
			// Token: 0x0400106D RID: 4205
			_SC_XOPEN_VERSION,
			// Token: 0x0400106E RID: 4206
			_SC_XOPEN_XCU_VERSION,
			// Token: 0x0400106F RID: 4207
			_SC_XOPEN_UNIX,
			// Token: 0x04001070 RID: 4208
			_SC_XOPEN_CRYPT,
			// Token: 0x04001071 RID: 4209
			_SC_XOPEN_ENH_I18N,
			// Token: 0x04001072 RID: 4210
			_SC_XOPEN_SHM,
			// Token: 0x04001073 RID: 4211
			_SC_2_CHAR_TERM,
			// Token: 0x04001074 RID: 4212
			_SC_2_C_VERSION,
			// Token: 0x04001075 RID: 4213
			_SC_2_UPE,
			// Token: 0x04001076 RID: 4214
			_SC_XOPEN_XPG2,
			// Token: 0x04001077 RID: 4215
			_SC_XOPEN_XPG3,
			// Token: 0x04001078 RID: 4216
			_SC_XOPEN_XPG4,
			// Token: 0x04001079 RID: 4217
			_SC_CHAR_BIT,
			// Token: 0x0400107A RID: 4218
			_SC_CHAR_MAX,
			// Token: 0x0400107B RID: 4219
			_SC_CHAR_MIN,
			// Token: 0x0400107C RID: 4220
			_SC_INT_MAX,
			// Token: 0x0400107D RID: 4221
			_SC_INT_MIN,
			// Token: 0x0400107E RID: 4222
			_SC_LONG_BIT,
			// Token: 0x0400107F RID: 4223
			_SC_WORD_BIT,
			// Token: 0x04001080 RID: 4224
			_SC_MB_LEN_MAX,
			// Token: 0x04001081 RID: 4225
			_SC_NZERO,
			// Token: 0x04001082 RID: 4226
			_SC_SSIZE_MAX,
			// Token: 0x04001083 RID: 4227
			_SC_SCHAR_MAX,
			// Token: 0x04001084 RID: 4228
			_SC_SCHAR_MIN,
			// Token: 0x04001085 RID: 4229
			_SC_SHRT_MAX,
			// Token: 0x04001086 RID: 4230
			_SC_SHRT_MIN,
			// Token: 0x04001087 RID: 4231
			_SC_UCHAR_MAX,
			// Token: 0x04001088 RID: 4232
			_SC_UINT_MAX,
			// Token: 0x04001089 RID: 4233
			_SC_ULONG_MAX,
			// Token: 0x0400108A RID: 4234
			_SC_USHRT_MAX,
			// Token: 0x0400108B RID: 4235
			_SC_NL_ARGMAX,
			// Token: 0x0400108C RID: 4236
			_SC_NL_LANGMAX,
			// Token: 0x0400108D RID: 4237
			_SC_NL_MSGMAX,
			// Token: 0x0400108E RID: 4238
			_SC_NL_NMAX,
			// Token: 0x0400108F RID: 4239
			_SC_NL_SETMAX,
			// Token: 0x04001090 RID: 4240
			_SC_NL_TEXTMAX,
			// Token: 0x04001091 RID: 4241
			_SC_XBS5_ILP32_OFF32,
			// Token: 0x04001092 RID: 4242
			_SC_XBS5_ILP32_OFFBIG,
			// Token: 0x04001093 RID: 4243
			_SC_XBS5_LP64_OFF64,
			// Token: 0x04001094 RID: 4244
			_SC_XBS5_LPBIG_OFFBIG,
			// Token: 0x04001095 RID: 4245
			_SC_XOPEN_LEGACY,
			// Token: 0x04001096 RID: 4246
			_SC_XOPEN_REALTIME,
			// Token: 0x04001097 RID: 4247
			_SC_XOPEN_REALTIME_THREADS,
			// Token: 0x04001098 RID: 4248
			_SC_ADVISORY_INFO,
			// Token: 0x04001099 RID: 4249
			_SC_BARRIERS,
			// Token: 0x0400109A RID: 4250
			_SC_BASE,
			// Token: 0x0400109B RID: 4251
			_SC_C_LANG_SUPPORT,
			// Token: 0x0400109C RID: 4252
			_SC_C_LANG_SUPPORT_R,
			// Token: 0x0400109D RID: 4253
			_SC_CLOCK_SELECTION,
			// Token: 0x0400109E RID: 4254
			_SC_CPUTIME,
			// Token: 0x0400109F RID: 4255
			_SC_THREAD_CPUTIME,
			// Token: 0x040010A0 RID: 4256
			_SC_DEVICE_IO,
			// Token: 0x040010A1 RID: 4257
			_SC_DEVICE_SPECIFIC,
			// Token: 0x040010A2 RID: 4258
			_SC_DEVICE_SPECIFIC_R,
			// Token: 0x040010A3 RID: 4259
			_SC_FD_MGMT,
			// Token: 0x040010A4 RID: 4260
			_SC_FIFO,
			// Token: 0x040010A5 RID: 4261
			_SC_PIPE,
			// Token: 0x040010A6 RID: 4262
			_SC_FILE_ATTRIBUTES,
			// Token: 0x040010A7 RID: 4263
			_SC_FILE_LOCKING,
			// Token: 0x040010A8 RID: 4264
			_SC_FILE_SYSTEM,
			// Token: 0x040010A9 RID: 4265
			_SC_MONOTONIC_CLOCK,
			// Token: 0x040010AA RID: 4266
			_SC_MULTI_PROCESS,
			// Token: 0x040010AB RID: 4267
			_SC_SINGLE_PROCESS,
			// Token: 0x040010AC RID: 4268
			_SC_NETWORKING,
			// Token: 0x040010AD RID: 4269
			_SC_READER_WRITER_LOCKS,
			// Token: 0x040010AE RID: 4270
			_SC_SPIN_LOCKS,
			// Token: 0x040010AF RID: 4271
			_SC_REGEXP,
			// Token: 0x040010B0 RID: 4272
			_SC_REGEX_VERSION,
			// Token: 0x040010B1 RID: 4273
			_SC_SHELL,
			// Token: 0x040010B2 RID: 4274
			_SC_SIGNALS,
			// Token: 0x040010B3 RID: 4275
			_SC_SPAWN,
			// Token: 0x040010B4 RID: 4276
			_SC_SPORADIC_SERVER,
			// Token: 0x040010B5 RID: 4277
			_SC_THREAD_SPORADIC_SERVER,
			// Token: 0x040010B6 RID: 4278
			_SC_SYSTEM_DATABASE,
			// Token: 0x040010B7 RID: 4279
			_SC_SYSTEM_DATABASE_R,
			// Token: 0x040010B8 RID: 4280
			_SC_TIMEOUTS,
			// Token: 0x040010B9 RID: 4281
			_SC_TYPED_MEMORY_OBJECTS,
			// Token: 0x040010BA RID: 4282
			_SC_USER_GROUPS,
			// Token: 0x040010BB RID: 4283
			_SC_USER_GROUPS_R,
			// Token: 0x040010BC RID: 4284
			_SC_2_PBS,
			// Token: 0x040010BD RID: 4285
			_SC_2_PBS_ACCOUNTING,
			// Token: 0x040010BE RID: 4286
			_SC_2_PBS_LOCATE,
			// Token: 0x040010BF RID: 4287
			_SC_2_PBS_MESSAGE,
			// Token: 0x040010C0 RID: 4288
			_SC_2_PBS_TRACK,
			// Token: 0x040010C1 RID: 4289
			_SC_SYMLOOP_MAX,
			// Token: 0x040010C2 RID: 4290
			_SC_STREAMS,
			// Token: 0x040010C3 RID: 4291
			_SC_2_PBS_CHECKPOINT,
			// Token: 0x040010C4 RID: 4292
			_SC_V6_ILP32_OFF32,
			// Token: 0x040010C5 RID: 4293
			_SC_V6_ILP32_OFFBIG,
			// Token: 0x040010C6 RID: 4294
			_SC_V6_LP64_OFF64,
			// Token: 0x040010C7 RID: 4295
			_SC_V6_LPBIG_OFFBIG,
			// Token: 0x040010C8 RID: 4296
			_SC_HOST_NAME_MAX,
			// Token: 0x040010C9 RID: 4297
			_SC_TRACE,
			// Token: 0x040010CA RID: 4298
			_SC_TRACE_EVENT_FILTER,
			// Token: 0x040010CB RID: 4299
			_SC_TRACE_INHERIT,
			// Token: 0x040010CC RID: 4300
			_SC_TRACE_LOG,
			// Token: 0x040010CD RID: 4301
			_SC_LEVEL1_ICACHE_SIZE,
			// Token: 0x040010CE RID: 4302
			_SC_LEVEL1_ICACHE_ASSOC,
			// Token: 0x040010CF RID: 4303
			_SC_LEVEL1_ICACHE_LINESIZE,
			// Token: 0x040010D0 RID: 4304
			_SC_LEVEL1_DCACHE_SIZE,
			// Token: 0x040010D1 RID: 4305
			_SC_LEVEL1_DCACHE_ASSOC,
			// Token: 0x040010D2 RID: 4306
			_SC_LEVEL1_DCACHE_LINESIZE,
			// Token: 0x040010D3 RID: 4307
			_SC_LEVEL2_CACHE_SIZE,
			// Token: 0x040010D4 RID: 4308
			_SC_LEVEL2_CACHE_ASSOC,
			// Token: 0x040010D5 RID: 4309
			_SC_LEVEL2_CACHE_LINESIZE,
			// Token: 0x040010D6 RID: 4310
			_SC_LEVEL3_CACHE_SIZE,
			// Token: 0x040010D7 RID: 4311
			_SC_LEVEL3_CACHE_ASSOC,
			// Token: 0x040010D8 RID: 4312
			_SC_LEVEL3_CACHE_LINESIZE,
			// Token: 0x040010D9 RID: 4313
			_SC_LEVEL4_CACHE_SIZE,
			// Token: 0x040010DA RID: 4314
			_SC_LEVEL4_CACHE_ASSOC,
			// Token: 0x040010DB RID: 4315
			_SC_LEVEL4_CACHE_LINESIZE
		}

		// Token: 0x02000357 RID: 855
		public enum Errno
		{
			// Token: 0x040010DD RID: 4317
			EPERM = 1,
			// Token: 0x040010DE RID: 4318
			ENOENT,
			// Token: 0x040010DF RID: 4319
			ESRCH,
			// Token: 0x040010E0 RID: 4320
			EINTR,
			// Token: 0x040010E1 RID: 4321
			EIO,
			// Token: 0x040010E2 RID: 4322
			ENXIO,
			// Token: 0x040010E3 RID: 4323
			E2BIG,
			// Token: 0x040010E4 RID: 4324
			ENOEXEC,
			// Token: 0x040010E5 RID: 4325
			EBADF,
			// Token: 0x040010E6 RID: 4326
			ECHILD,
			// Token: 0x040010E7 RID: 4327
			EAGAIN,
			// Token: 0x040010E8 RID: 4328
			ENOMEM,
			// Token: 0x040010E9 RID: 4329
			EACCES,
			// Token: 0x040010EA RID: 4330
			EFAULT,
			// Token: 0x040010EB RID: 4331
			ENOTBLK,
			// Token: 0x040010EC RID: 4332
			EBUSY,
			// Token: 0x040010ED RID: 4333
			EEXIST,
			// Token: 0x040010EE RID: 4334
			EXDEV,
			// Token: 0x040010EF RID: 4335
			ENODEV,
			// Token: 0x040010F0 RID: 4336
			ENOTDIR,
			// Token: 0x040010F1 RID: 4337
			EISDIR,
			// Token: 0x040010F2 RID: 4338
			EINVAL,
			// Token: 0x040010F3 RID: 4339
			ENFILE,
			// Token: 0x040010F4 RID: 4340
			EMFILE,
			// Token: 0x040010F5 RID: 4341
			ENOTTY,
			// Token: 0x040010F6 RID: 4342
			ETXTBSY,
			// Token: 0x040010F7 RID: 4343
			EFBIG,
			// Token: 0x040010F8 RID: 4344
			ENOSPC,
			// Token: 0x040010F9 RID: 4345
			ESPIPE,
			// Token: 0x040010FA RID: 4346
			EROFS,
			// Token: 0x040010FB RID: 4347
			EMLINK,
			// Token: 0x040010FC RID: 4348
			EPIPE,
			// Token: 0x040010FD RID: 4349
			EDOM,
			// Token: 0x040010FE RID: 4350
			ERANGE,
			// Token: 0x040010FF RID: 4351
			EDEADLK,
			// Token: 0x04001100 RID: 4352
			ENAMETOOLONG,
			// Token: 0x04001101 RID: 4353
			ENOLCK,
			// Token: 0x04001102 RID: 4354
			ENOSYS,
			// Token: 0x04001103 RID: 4355
			ENOTEMPTY,
			// Token: 0x04001104 RID: 4356
			ELOOP,
			// Token: 0x04001105 RID: 4357
			EWOULDBLOCK = 11,
			// Token: 0x04001106 RID: 4358
			ENOMSG = 42,
			// Token: 0x04001107 RID: 4359
			EIDRM,
			// Token: 0x04001108 RID: 4360
			ECHRNG,
			// Token: 0x04001109 RID: 4361
			EL2NSYNC,
			// Token: 0x0400110A RID: 4362
			EL3HLT,
			// Token: 0x0400110B RID: 4363
			EL3RST,
			// Token: 0x0400110C RID: 4364
			ELNRNG,
			// Token: 0x0400110D RID: 4365
			EUNATCH,
			// Token: 0x0400110E RID: 4366
			ENOCSI,
			// Token: 0x0400110F RID: 4367
			EL2HLT,
			// Token: 0x04001110 RID: 4368
			EBADE,
			// Token: 0x04001111 RID: 4369
			EBADR,
			// Token: 0x04001112 RID: 4370
			EXFULL,
			// Token: 0x04001113 RID: 4371
			ENOANO,
			// Token: 0x04001114 RID: 4372
			EBADRQC,
			// Token: 0x04001115 RID: 4373
			EBADSLT,
			// Token: 0x04001116 RID: 4374
			EDEADLOCK = 35,
			// Token: 0x04001117 RID: 4375
			EBFONT = 59,
			// Token: 0x04001118 RID: 4376
			ENOSTR,
			// Token: 0x04001119 RID: 4377
			ENODATA,
			// Token: 0x0400111A RID: 4378
			ETIME,
			// Token: 0x0400111B RID: 4379
			ENOSR,
			// Token: 0x0400111C RID: 4380
			ENONET,
			// Token: 0x0400111D RID: 4381
			ENOPKG,
			// Token: 0x0400111E RID: 4382
			EREMOTE,
			// Token: 0x0400111F RID: 4383
			ENOLINK,
			// Token: 0x04001120 RID: 4384
			EADV,
			// Token: 0x04001121 RID: 4385
			ESRMNT,
			// Token: 0x04001122 RID: 4386
			ECOMM,
			// Token: 0x04001123 RID: 4387
			EPROTO,
			// Token: 0x04001124 RID: 4388
			EMULTIHOP,
			// Token: 0x04001125 RID: 4389
			EDOTDOT,
			// Token: 0x04001126 RID: 4390
			EBADMSG,
			// Token: 0x04001127 RID: 4391
			EOVERFLOW,
			// Token: 0x04001128 RID: 4392
			ENOTUNIQ,
			// Token: 0x04001129 RID: 4393
			EBADFD,
			// Token: 0x0400112A RID: 4394
			EREMCHG,
			// Token: 0x0400112B RID: 4395
			ELIBACC,
			// Token: 0x0400112C RID: 4396
			ELIBBAD,
			// Token: 0x0400112D RID: 4397
			ELIBSCN,
			// Token: 0x0400112E RID: 4398
			ELIBMAX,
			// Token: 0x0400112F RID: 4399
			ELIBEXEC,
			// Token: 0x04001130 RID: 4400
			EILSEQ,
			// Token: 0x04001131 RID: 4401
			ERESTART,
			// Token: 0x04001132 RID: 4402
			ESTRPIPE,
			// Token: 0x04001133 RID: 4403
			EUSERS,
			// Token: 0x04001134 RID: 4404
			ENOTSOCK,
			// Token: 0x04001135 RID: 4405
			EDESTADDRREQ,
			// Token: 0x04001136 RID: 4406
			EMSGSIZE,
			// Token: 0x04001137 RID: 4407
			EPROTOTYPE,
			// Token: 0x04001138 RID: 4408
			ENOPROTOOPT,
			// Token: 0x04001139 RID: 4409
			EPROTONOSUPPORT,
			// Token: 0x0400113A RID: 4410
			ESOCKTNOSUPPORT,
			// Token: 0x0400113B RID: 4411
			EOPNOTSUPP,
			// Token: 0x0400113C RID: 4412
			EPFNOSUPPORT,
			// Token: 0x0400113D RID: 4413
			EAFNOSUPPORT,
			// Token: 0x0400113E RID: 4414
			EADDRINUSE,
			// Token: 0x0400113F RID: 4415
			EADDRNOTAVAIL,
			// Token: 0x04001140 RID: 4416
			ENETDOWN,
			// Token: 0x04001141 RID: 4417
			ENETUNREACH,
			// Token: 0x04001142 RID: 4418
			ENETRESET,
			// Token: 0x04001143 RID: 4419
			ECONNABORTED,
			// Token: 0x04001144 RID: 4420
			ECONNRESET,
			// Token: 0x04001145 RID: 4421
			ENOBUFS,
			// Token: 0x04001146 RID: 4422
			EISCONN,
			// Token: 0x04001147 RID: 4423
			ENOTCONN,
			// Token: 0x04001148 RID: 4424
			ESHUTDOWN,
			// Token: 0x04001149 RID: 4425
			ETOOMANYREFS,
			// Token: 0x0400114A RID: 4426
			ETIMEDOUT,
			// Token: 0x0400114B RID: 4427
			ECONNREFUSED,
			// Token: 0x0400114C RID: 4428
			EHOSTDOWN,
			// Token: 0x0400114D RID: 4429
			EHOSTUNREACH,
			// Token: 0x0400114E RID: 4430
			EALREADY,
			// Token: 0x0400114F RID: 4431
			EINPROGRESS,
			// Token: 0x04001150 RID: 4432
			ESTALE,
			// Token: 0x04001151 RID: 4433
			EUCLEAN,
			// Token: 0x04001152 RID: 4434
			ENOTNAM,
			// Token: 0x04001153 RID: 4435
			ENAVAIL,
			// Token: 0x04001154 RID: 4436
			EISNAM,
			// Token: 0x04001155 RID: 4437
			EREMOTEIO,
			// Token: 0x04001156 RID: 4438
			EDQUOT,
			// Token: 0x04001157 RID: 4439
			ENOMEDIUM,
			// Token: 0x04001158 RID: 4440
			EMEDIUMTYPE,
			// Token: 0x04001159 RID: 4441
			ECANCELED,
			// Token: 0x0400115A RID: 4442
			ENOKEY,
			// Token: 0x0400115B RID: 4443
			EKEYEXPIRED,
			// Token: 0x0400115C RID: 4444
			EKEYREVOKED,
			// Token: 0x0400115D RID: 4445
			EKEYREJECTED,
			// Token: 0x0400115E RID: 4446
			EOWNERDEAD,
			// Token: 0x0400115F RID: 4447
			ENOTRECOVERABLE,
			// Token: 0x04001160 RID: 4448
			EPROCLIM = 1067,
			// Token: 0x04001161 RID: 4449
			EBADRPC = 1072,
			// Token: 0x04001162 RID: 4450
			ERPCMISMATCH,
			// Token: 0x04001163 RID: 4451
			EPROGUNAVAIL,
			// Token: 0x04001164 RID: 4452
			EPROGMISMATCH,
			// Token: 0x04001165 RID: 4453
			EPROCUNAVAIL,
			// Token: 0x04001166 RID: 4454
			EFTYPE = 1079,
			// Token: 0x04001167 RID: 4455
			EAUTH,
			// Token: 0x04001168 RID: 4456
			ENEEDAUTH,
			// Token: 0x04001169 RID: 4457
			EPWROFF,
			// Token: 0x0400116A RID: 4458
			EDEVERR,
			// Token: 0x0400116B RID: 4459
			EBADEXEC = 1085,
			// Token: 0x0400116C RID: 4460
			EBADARCH,
			// Token: 0x0400116D RID: 4461
			ESHLIBVERS,
			// Token: 0x0400116E RID: 4462
			EBADMACHO,
			// Token: 0x0400116F RID: 4463
			ENOATTR = 1093,
			// Token: 0x04001170 RID: 4464
			ENOPOLICY = 1103
		}
	}
}
