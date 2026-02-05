using System;
using System.Threading;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x02000140 RID: 320
	internal sealed class MethodDefinition : MethodReference, IMemberDefinition, ICustomAttributeProvider, IMetadataTokenProvider, ISecurityDeclarationProvider, ICustomDebugInformationProvider
	{
		// Token: 0x1700013B RID: 315
		// (get) Token: 0x060008E9 RID: 2281 RVA: 0x00022BE1 File Offset: 0x00020DE1
		// (set) Token: 0x060008EA RID: 2282 RVA: 0x00022BE9 File Offset: 0x00020DE9
		public override string Name
		{
			get
			{
				return base.Name;
			}
			set
			{
				if (base.IsWindowsRuntimeProjection && value != base.Name)
				{
					throw new InvalidOperationException();
				}
				base.Name = value;
			}
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x060008EB RID: 2283 RVA: 0x00022C0E File Offset: 0x00020E0E
		// (set) Token: 0x060008EC RID: 2284 RVA: 0x00022C16 File Offset: 0x00020E16
		public MethodAttributes Attributes
		{
			get
			{
				return (MethodAttributes)this.attributes;
			}
			set
			{
				if (base.IsWindowsRuntimeProjection && value != (MethodAttributes)this.attributes)
				{
					throw new InvalidOperationException();
				}
				this.attributes = (ushort)value;
			}
		}

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x060008ED RID: 2285 RVA: 0x00022C36 File Offset: 0x00020E36
		// (set) Token: 0x060008EE RID: 2286 RVA: 0x00022C3E File Offset: 0x00020E3E
		public MethodImplAttributes ImplAttributes
		{
			get
			{
				return (MethodImplAttributes)this.impl_attributes;
			}
			set
			{
				if (base.IsWindowsRuntimeProjection && value != (MethodImplAttributes)this.impl_attributes)
				{
					throw new InvalidOperationException();
				}
				this.impl_attributes = (ushort)value;
			}
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x060008EF RID: 2287 RVA: 0x00022C5E File Offset: 0x00020E5E
		// (set) Token: 0x060008F0 RID: 2288 RVA: 0x00022C9C File Offset: 0x00020E9C
		public MethodSemanticsAttributes SemanticsAttributes
		{
			get
			{
				if (this.sem_attrs_ready)
				{
					return this.sem_attrs;
				}
				if (base.HasImage)
				{
					this.ReadSemantics();
					return this.sem_attrs;
				}
				this.sem_attrs = MethodSemanticsAttributes.None;
				this.sem_attrs_ready = true;
				return this.sem_attrs;
			}
			set
			{
				this.sem_attrs = value;
			}
		}

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x060008F1 RID: 2289 RVA: 0x00022CA5 File Offset: 0x00020EA5
		// (set) Token: 0x060008F2 RID: 2290 RVA: 0x0001F94A File Offset: 0x0001DB4A
		internal new MethodDefinitionProjection WindowsRuntimeProjection
		{
			get
			{
				return (MethodDefinitionProjection)this.projection;
			}
			set
			{
				this.projection = value;
			}
		}

		// Token: 0x060008F3 RID: 2291 RVA: 0x00022CB4 File Offset: 0x00020EB4
		internal void ReadSemantics()
		{
			if (this.sem_attrs_ready)
			{
				return;
			}
			ModuleDefinition module = this.Module;
			if (module == null)
			{
				return;
			}
			if (!module.HasImage)
			{
				return;
			}
			object syncRoot = module.SyncRoot;
			lock (syncRoot)
			{
				if (!this.sem_attrs_ready)
				{
					module.Read<MethodDefinition>(this, delegate(MethodDefinition method, MetadataReader reader)
					{
						reader.ReadAllSemantics(method);
					});
				}
			}
		}

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x060008F4 RID: 2292 RVA: 0x00022D40 File Offset: 0x00020F40
		public bool HasSecurityDeclarations
		{
			get
			{
				if (this.security_declarations != null)
				{
					return this.security_declarations.Count > 0;
				}
				return this.GetHasSecurityDeclarations(this.Module);
			}
		}

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x060008F5 RID: 2293 RVA: 0x00022D65 File Offset: 0x00020F65
		public Collection<SecurityDeclaration> SecurityDeclarations
		{
			get
			{
				return this.security_declarations ?? this.GetSecurityDeclarations(ref this.security_declarations, this.Module);
			}
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x060008F6 RID: 2294 RVA: 0x00022D83 File Offset: 0x00020F83
		public bool HasCustomAttributes
		{
			get
			{
				if (this.custom_attributes != null)
				{
					return this.custom_attributes.Count > 0;
				}
				return this.GetHasCustomAttributes(this.Module);
			}
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x060008F7 RID: 2295 RVA: 0x00022DA8 File Offset: 0x00020FA8
		public Collection<CustomAttribute> CustomAttributes
		{
			get
			{
				return this.custom_attributes ?? this.GetCustomAttributes(ref this.custom_attributes, this.Module);
			}
		}

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x060008F8 RID: 2296 RVA: 0x00022DC6 File Offset: 0x00020FC6
		public int RVA
		{
			get
			{
				return (int)this.rva;
			}
		}

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x060008F9 RID: 2297 RVA: 0x00022DD0 File Offset: 0x00020FD0
		public bool HasBody
		{
			get
			{
				return (this.attributes & 1024) == 0 && (this.attributes & 8192) == 0 && (this.impl_attributes & 4096) == 0 && (this.impl_attributes & 1) == 0 && (this.impl_attributes & 4) == 0 && (this.impl_attributes & 3) == 0;
			}
		}

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x060008FA RID: 2298 RVA: 0x00022E28 File Offset: 0x00021028
		// (set) Token: 0x060008FB RID: 2299 RVA: 0x00022EA8 File Offset: 0x000210A8
		public MethodBody Body
		{
			get
			{
				MethodBody methodBody = this.body;
				if (methodBody != null)
				{
					return methodBody;
				}
				if (!this.HasBody)
				{
					return null;
				}
				if (base.HasImage && this.rva != 0U)
				{
					return this.Module.Read<MethodDefinition, MethodBody>(ref this.body, this, (MethodDefinition method, MetadataReader reader) => reader.ReadMethodBody(method));
				}
				Interlocked.CompareExchange<MethodBody>(ref this.body, new MethodBody(this), null);
				return this.body;
			}
			set
			{
				ModuleDefinition module = this.Module;
				if (module == null)
				{
					this.body = value;
					return;
				}
				object syncRoot = module.SyncRoot;
				lock (syncRoot)
				{
					this.body = value;
					if (value == null)
					{
						this.debug_info = null;
					}
				}
			}
		}

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x060008FC RID: 2300 RVA: 0x00022F08 File Offset: 0x00021108
		// (set) Token: 0x060008FD RID: 2301 RVA: 0x00022F36 File Offset: 0x00021136
		public MethodDebugInformation DebugInformation
		{
			get
			{
				Mixin.Read(this.Body);
				if (this.debug_info == null)
				{
					Interlocked.CompareExchange<MethodDebugInformation>(ref this.debug_info, new MethodDebugInformation(this), null);
				}
				return this.debug_info;
			}
			set
			{
				this.debug_info = value;
			}
		}

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x060008FE RID: 2302 RVA: 0x00022F3F File Offset: 0x0002113F
		public bool HasPInvokeInfo
		{
			get
			{
				return this.pinvoke != null || this.IsPInvokeImpl;
			}
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x060008FF RID: 2303 RVA: 0x00022F54 File Offset: 0x00021154
		// (set) Token: 0x06000900 RID: 2304 RVA: 0x00022FB3 File Offset: 0x000211B3
		public PInvokeInfo PInvokeInfo
		{
			get
			{
				if (this.pinvoke != null)
				{
					return this.pinvoke;
				}
				if (base.HasImage && this.IsPInvokeImpl)
				{
					return this.Module.Read<MethodDefinition, PInvokeInfo>(ref this.pinvoke, this, (MethodDefinition method, MetadataReader reader) => reader.ReadPInvokeInfo(method));
				}
				return null;
			}
			set
			{
				this.IsPInvokeImpl = true;
				this.pinvoke = value;
			}
		}

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x06000901 RID: 2305 RVA: 0x00022FC4 File Offset: 0x000211C4
		public bool HasOverrides
		{
			get
			{
				if (this.overrides != null)
				{
					return this.overrides.Count > 0;
				}
				if (base.HasImage)
				{
					return this.Module.Read<MethodDefinition, bool>(this, (MethodDefinition method, MetadataReader reader) => reader.HasOverrides(method));
				}
				return false;
			}
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x06000902 RID: 2306 RVA: 0x00023020 File Offset: 0x00021220
		public Collection<MethodReference> Overrides
		{
			get
			{
				if (this.overrides != null)
				{
					return this.overrides;
				}
				if (base.HasImage)
				{
					return this.Module.Read<MethodDefinition, Collection<MethodReference>>(ref this.overrides, this, (MethodDefinition method, MetadataReader reader) => reader.ReadOverrides(method));
				}
				Interlocked.CompareExchange<Collection<MethodReference>>(ref this.overrides, new Collection<MethodReference>(), null);
				return this.overrides;
			}
		}

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x06000903 RID: 2307 RVA: 0x0002308E File Offset: 0x0002128E
		public override bool HasGenericParameters
		{
			get
			{
				if (this.generic_parameters != null)
				{
					return this.generic_parameters.Count > 0;
				}
				return this.GetHasGenericParameters(this.Module);
			}
		}

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x06000904 RID: 2308 RVA: 0x000230B3 File Offset: 0x000212B3
		public override Collection<GenericParameter> GenericParameters
		{
			get
			{
				return this.generic_parameters ?? this.GetGenericParameters(ref this.generic_parameters, this.Module);
			}
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x06000905 RID: 2309 RVA: 0x000230D1 File Offset: 0x000212D1
		public bool HasCustomDebugInformations
		{
			get
			{
				Mixin.Read(this.Body);
				return !this.custom_infos.IsNullOrEmpty<CustomDebugInformation>();
			}
		}

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x06000906 RID: 2310 RVA: 0x000230EC File Offset: 0x000212EC
		public Collection<CustomDebugInformation> CustomDebugInformations
		{
			get
			{
				Mixin.Read(this.Body);
				if (this.custom_infos == null)
				{
					Interlocked.CompareExchange<Collection<CustomDebugInformation>>(ref this.custom_infos, new Collection<CustomDebugInformation>(), null);
				}
				return this.custom_infos;
			}
		}

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x06000907 RID: 2311 RVA: 0x00023119 File Offset: 0x00021319
		// (set) Token: 0x06000908 RID: 2312 RVA: 0x00023128 File Offset: 0x00021328
		public bool IsCompilerControlled
		{
			get
			{
				return this.attributes.GetMaskedAttributes(7, 0U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(7, 0U, value);
			}
		}

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x06000909 RID: 2313 RVA: 0x0002313E File Offset: 0x0002133E
		// (set) Token: 0x0600090A RID: 2314 RVA: 0x0002314D File Offset: 0x0002134D
		public bool IsPrivate
		{
			get
			{
				return this.attributes.GetMaskedAttributes(7, 1U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(7, 1U, value);
			}
		}

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x0600090B RID: 2315 RVA: 0x00023163 File Offset: 0x00021363
		// (set) Token: 0x0600090C RID: 2316 RVA: 0x00023172 File Offset: 0x00021372
		public bool IsFamilyAndAssembly
		{
			get
			{
				return this.attributes.GetMaskedAttributes(7, 2U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(7, 2U, value);
			}
		}

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x0600090D RID: 2317 RVA: 0x00023188 File Offset: 0x00021388
		// (set) Token: 0x0600090E RID: 2318 RVA: 0x00023197 File Offset: 0x00021397
		public bool IsAssembly
		{
			get
			{
				return this.attributes.GetMaskedAttributes(7, 3U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(7, 3U, value);
			}
		}

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x0600090F RID: 2319 RVA: 0x000231AD File Offset: 0x000213AD
		// (set) Token: 0x06000910 RID: 2320 RVA: 0x000231BC File Offset: 0x000213BC
		public bool IsFamily
		{
			get
			{
				return this.attributes.GetMaskedAttributes(7, 4U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(7, 4U, value);
			}
		}

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x06000911 RID: 2321 RVA: 0x000231D2 File Offset: 0x000213D2
		// (set) Token: 0x06000912 RID: 2322 RVA: 0x000231E1 File Offset: 0x000213E1
		public bool IsFamilyOrAssembly
		{
			get
			{
				return this.attributes.GetMaskedAttributes(7, 5U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(7, 5U, value);
			}
		}

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x06000913 RID: 2323 RVA: 0x000231F7 File Offset: 0x000213F7
		// (set) Token: 0x06000914 RID: 2324 RVA: 0x00023206 File Offset: 0x00021406
		public bool IsPublic
		{
			get
			{
				return this.attributes.GetMaskedAttributes(7, 6U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(7, 6U, value);
			}
		}

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x06000915 RID: 2325 RVA: 0x0002321C File Offset: 0x0002141C
		// (set) Token: 0x06000916 RID: 2326 RVA: 0x0002322B File Offset: 0x0002142B
		public bool IsStatic
		{
			get
			{
				return this.attributes.GetAttributes(16);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(16, value);
			}
		}

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x06000917 RID: 2327 RVA: 0x00023241 File Offset: 0x00021441
		// (set) Token: 0x06000918 RID: 2328 RVA: 0x00023250 File Offset: 0x00021450
		public bool IsFinal
		{
			get
			{
				return this.attributes.GetAttributes(32);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(32, value);
			}
		}

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x06000919 RID: 2329 RVA: 0x00023266 File Offset: 0x00021466
		// (set) Token: 0x0600091A RID: 2330 RVA: 0x00023275 File Offset: 0x00021475
		public bool IsVirtual
		{
			get
			{
				return this.attributes.GetAttributes(64);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(64, value);
			}
		}

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x0600091B RID: 2331 RVA: 0x0002328B File Offset: 0x0002148B
		// (set) Token: 0x0600091C RID: 2332 RVA: 0x0002329D File Offset: 0x0002149D
		public bool IsHideBySig
		{
			get
			{
				return this.attributes.GetAttributes(128);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(128, value);
			}
		}

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x0600091D RID: 2333 RVA: 0x000232B6 File Offset: 0x000214B6
		// (set) Token: 0x0600091E RID: 2334 RVA: 0x000232C9 File Offset: 0x000214C9
		public bool IsReuseSlot
		{
			get
			{
				return this.attributes.GetMaskedAttributes(256, 0U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(256, 0U, value);
			}
		}

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x0600091F RID: 2335 RVA: 0x000232E3 File Offset: 0x000214E3
		// (set) Token: 0x06000920 RID: 2336 RVA: 0x000232FA File Offset: 0x000214FA
		public bool IsNewSlot
		{
			get
			{
				return this.attributes.GetMaskedAttributes(256, 256U);
			}
			set
			{
				this.attributes = this.attributes.SetMaskedAttributes(256, 256U, value);
			}
		}

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x06000921 RID: 2337 RVA: 0x00023318 File Offset: 0x00021518
		// (set) Token: 0x06000922 RID: 2338 RVA: 0x0002332A File Offset: 0x0002152A
		public bool IsCheckAccessOnOverride
		{
			get
			{
				return this.attributes.GetAttributes(512);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(512, value);
			}
		}

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x06000923 RID: 2339 RVA: 0x00023343 File Offset: 0x00021543
		// (set) Token: 0x06000924 RID: 2340 RVA: 0x00023355 File Offset: 0x00021555
		public bool IsAbstract
		{
			get
			{
				return this.attributes.GetAttributes(1024);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(1024, value);
			}
		}

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x06000925 RID: 2341 RVA: 0x0002336E File Offset: 0x0002156E
		// (set) Token: 0x06000926 RID: 2342 RVA: 0x00023380 File Offset: 0x00021580
		public bool IsSpecialName
		{
			get
			{
				return this.attributes.GetAttributes(2048);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(2048, value);
			}
		}

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x06000927 RID: 2343 RVA: 0x00023399 File Offset: 0x00021599
		// (set) Token: 0x06000928 RID: 2344 RVA: 0x000233AB File Offset: 0x000215AB
		public bool IsPInvokeImpl
		{
			get
			{
				return this.attributes.GetAttributes(8192);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(8192, value);
			}
		}

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x06000929 RID: 2345 RVA: 0x000233C4 File Offset: 0x000215C4
		// (set) Token: 0x0600092A RID: 2346 RVA: 0x000233D2 File Offset: 0x000215D2
		public bool IsUnmanagedExport
		{
			get
			{
				return this.attributes.GetAttributes(8);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(8, value);
			}
		}

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x0600092B RID: 2347 RVA: 0x000233E7 File Offset: 0x000215E7
		// (set) Token: 0x0600092C RID: 2348 RVA: 0x000233F9 File Offset: 0x000215F9
		public bool IsRuntimeSpecialName
		{
			get
			{
				return this.attributes.GetAttributes(4096);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(4096, value);
			}
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x0600092D RID: 2349 RVA: 0x00023412 File Offset: 0x00021612
		// (set) Token: 0x0600092E RID: 2350 RVA: 0x00023424 File Offset: 0x00021624
		public bool HasSecurity
		{
			get
			{
				return this.attributes.GetAttributes(16384);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(16384, value);
			}
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x0600092F RID: 2351 RVA: 0x0002343D File Offset: 0x0002163D
		// (set) Token: 0x06000930 RID: 2352 RVA: 0x0002344C File Offset: 0x0002164C
		public bool IsIL
		{
			get
			{
				return this.impl_attributes.GetMaskedAttributes(3, 0U);
			}
			set
			{
				this.impl_attributes = this.impl_attributes.SetMaskedAttributes(3, 0U, value);
			}
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x06000931 RID: 2353 RVA: 0x00023462 File Offset: 0x00021662
		// (set) Token: 0x06000932 RID: 2354 RVA: 0x00023471 File Offset: 0x00021671
		public bool IsNative
		{
			get
			{
				return this.impl_attributes.GetMaskedAttributes(3, 1U);
			}
			set
			{
				this.impl_attributes = this.impl_attributes.SetMaskedAttributes(3, 1U, value);
			}
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x06000933 RID: 2355 RVA: 0x00023487 File Offset: 0x00021687
		// (set) Token: 0x06000934 RID: 2356 RVA: 0x00023496 File Offset: 0x00021696
		public bool IsRuntime
		{
			get
			{
				return this.impl_attributes.GetMaskedAttributes(3, 3U);
			}
			set
			{
				this.impl_attributes = this.impl_attributes.SetMaskedAttributes(3, 3U, value);
			}
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x06000935 RID: 2357 RVA: 0x000234AC File Offset: 0x000216AC
		// (set) Token: 0x06000936 RID: 2358 RVA: 0x000234BB File Offset: 0x000216BB
		public bool IsUnmanaged
		{
			get
			{
				return this.impl_attributes.GetMaskedAttributes(4, 4U);
			}
			set
			{
				this.impl_attributes = this.impl_attributes.SetMaskedAttributes(4, 4U, value);
			}
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x06000937 RID: 2359 RVA: 0x000234D1 File Offset: 0x000216D1
		// (set) Token: 0x06000938 RID: 2360 RVA: 0x000234E0 File Offset: 0x000216E0
		public bool IsManaged
		{
			get
			{
				return this.impl_attributes.GetMaskedAttributes(4, 0U);
			}
			set
			{
				this.impl_attributes = this.impl_attributes.SetMaskedAttributes(4, 0U, value);
			}
		}

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x06000939 RID: 2361 RVA: 0x000234F6 File Offset: 0x000216F6
		// (set) Token: 0x0600093A RID: 2362 RVA: 0x00023505 File Offset: 0x00021705
		public bool IsForwardRef
		{
			get
			{
				return this.impl_attributes.GetAttributes(16);
			}
			set
			{
				this.impl_attributes = this.impl_attributes.SetAttributes(16, value);
			}
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x0600093B RID: 2363 RVA: 0x0002351B File Offset: 0x0002171B
		// (set) Token: 0x0600093C RID: 2364 RVA: 0x0002352D File Offset: 0x0002172D
		public bool IsPreserveSig
		{
			get
			{
				return this.impl_attributes.GetAttributes(128);
			}
			set
			{
				this.impl_attributes = this.impl_attributes.SetAttributes(128, value);
			}
		}

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x0600093D RID: 2365 RVA: 0x00023546 File Offset: 0x00021746
		// (set) Token: 0x0600093E RID: 2366 RVA: 0x00023558 File Offset: 0x00021758
		public bool IsInternalCall
		{
			get
			{
				return this.impl_attributes.GetAttributes(4096);
			}
			set
			{
				this.impl_attributes = this.impl_attributes.SetAttributes(4096, value);
			}
		}

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x0600093F RID: 2367 RVA: 0x00023571 File Offset: 0x00021771
		// (set) Token: 0x06000940 RID: 2368 RVA: 0x00023580 File Offset: 0x00021780
		public bool IsSynchronized
		{
			get
			{
				return this.impl_attributes.GetAttributes(32);
			}
			set
			{
				this.impl_attributes = this.impl_attributes.SetAttributes(32, value);
			}
		}

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x06000941 RID: 2369 RVA: 0x00023596 File Offset: 0x00021796
		// (set) Token: 0x06000942 RID: 2370 RVA: 0x000235A4 File Offset: 0x000217A4
		public bool NoInlining
		{
			get
			{
				return this.impl_attributes.GetAttributes(8);
			}
			set
			{
				this.impl_attributes = this.impl_attributes.SetAttributes(8, value);
			}
		}

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x06000943 RID: 2371 RVA: 0x000235B9 File Offset: 0x000217B9
		// (set) Token: 0x06000944 RID: 2372 RVA: 0x000235C8 File Offset: 0x000217C8
		public bool NoOptimization
		{
			get
			{
				return this.impl_attributes.GetAttributes(64);
			}
			set
			{
				this.impl_attributes = this.impl_attributes.SetAttributes(64, value);
			}
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x06000945 RID: 2373 RVA: 0x000235DE File Offset: 0x000217DE
		// (set) Token: 0x06000946 RID: 2374 RVA: 0x000235F0 File Offset: 0x000217F0
		public bool AggressiveInlining
		{
			get
			{
				return this.impl_attributes.GetAttributes(256);
			}
			set
			{
				this.impl_attributes = this.impl_attributes.SetAttributes(256, value);
			}
		}

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x06000947 RID: 2375 RVA: 0x00023609 File Offset: 0x00021809
		// (set) Token: 0x06000948 RID: 2376 RVA: 0x00023612 File Offset: 0x00021812
		public bool IsSetter
		{
			get
			{
				return this.GetSemantics(MethodSemanticsAttributes.Setter);
			}
			set
			{
				this.SetSemantics(MethodSemanticsAttributes.Setter, value);
			}
		}

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x06000949 RID: 2377 RVA: 0x0002361C File Offset: 0x0002181C
		// (set) Token: 0x0600094A RID: 2378 RVA: 0x00023625 File Offset: 0x00021825
		public bool IsGetter
		{
			get
			{
				return this.GetSemantics(MethodSemanticsAttributes.Getter);
			}
			set
			{
				this.SetSemantics(MethodSemanticsAttributes.Getter, value);
			}
		}

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x0600094B RID: 2379 RVA: 0x0002362F File Offset: 0x0002182F
		// (set) Token: 0x0600094C RID: 2380 RVA: 0x00023638 File Offset: 0x00021838
		public bool IsOther
		{
			get
			{
				return this.GetSemantics(MethodSemanticsAttributes.Other);
			}
			set
			{
				this.SetSemantics(MethodSemanticsAttributes.Other, value);
			}
		}

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x0600094D RID: 2381 RVA: 0x00023642 File Offset: 0x00021842
		// (set) Token: 0x0600094E RID: 2382 RVA: 0x0002364B File Offset: 0x0002184B
		public bool IsAddOn
		{
			get
			{
				return this.GetSemantics(MethodSemanticsAttributes.AddOn);
			}
			set
			{
				this.SetSemantics(MethodSemanticsAttributes.AddOn, value);
			}
		}

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x0600094F RID: 2383 RVA: 0x00023655 File Offset: 0x00021855
		// (set) Token: 0x06000950 RID: 2384 RVA: 0x0002365F File Offset: 0x0002185F
		public bool IsRemoveOn
		{
			get
			{
				return this.GetSemantics(MethodSemanticsAttributes.RemoveOn);
			}
			set
			{
				this.SetSemantics(MethodSemanticsAttributes.RemoveOn, value);
			}
		}

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x06000951 RID: 2385 RVA: 0x0002366A File Offset: 0x0002186A
		// (set) Token: 0x06000952 RID: 2386 RVA: 0x00023674 File Offset: 0x00021874
		public bool IsFire
		{
			get
			{
				return this.GetSemantics(MethodSemanticsAttributes.Fire);
			}
			set
			{
				this.SetSemantics(MethodSemanticsAttributes.Fire, value);
			}
		}

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x06000953 RID: 2387 RVA: 0x0001F1AC File Offset: 0x0001D3AC
		// (set) Token: 0x06000954 RID: 2388 RVA: 0x0001F1B9 File Offset: 0x0001D3B9
		public new TypeDefinition DeclaringType
		{
			get
			{
				return (TypeDefinition)base.DeclaringType;
			}
			set
			{
				base.DeclaringType = value;
			}
		}

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x06000955 RID: 2389 RVA: 0x0002367F File Offset: 0x0002187F
		public bool IsConstructor
		{
			get
			{
				return this.IsRuntimeSpecialName && this.IsSpecialName && (this.Name == ".cctor" || this.Name == ".ctor");
			}
		}

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x06000956 RID: 2390 RVA: 0x00010F39 File Offset: 0x0000F139
		public override bool IsDefinition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000957 RID: 2391 RVA: 0x000236B7 File Offset: 0x000218B7
		internal MethodDefinition()
		{
			this.token = new MetadataToken(TokenType.Method);
		}

		// Token: 0x06000958 RID: 2392 RVA: 0x000236CF File Offset: 0x000218CF
		public MethodDefinition(string name, MethodAttributes attributes, TypeReference returnType)
			: base(name, returnType)
		{
			this.attributes = (ushort)attributes;
			this.HasThis = !this.IsStatic;
			this.token = new MetadataToken(TokenType.Method);
		}

		// Token: 0x06000959 RID: 2393 RVA: 0x00010978 File Offset: 0x0000EB78
		public override MethodDefinition Resolve()
		{
			return this;
		}

		// Token: 0x0400035A RID: 858
		private ushort attributes;

		// Token: 0x0400035B RID: 859
		private ushort impl_attributes;

		// Token: 0x0400035C RID: 860
		internal volatile bool sem_attrs_ready;

		// Token: 0x0400035D RID: 861
		internal MethodSemanticsAttributes sem_attrs;

		// Token: 0x0400035E RID: 862
		private Collection<CustomAttribute> custom_attributes;

		// Token: 0x0400035F RID: 863
		private Collection<SecurityDeclaration> security_declarations;

		// Token: 0x04000360 RID: 864
		internal uint rva;

		// Token: 0x04000361 RID: 865
		internal PInvokeInfo pinvoke;

		// Token: 0x04000362 RID: 866
		private Collection<MethodReference> overrides;

		// Token: 0x04000363 RID: 867
		internal MethodBody body;

		// Token: 0x04000364 RID: 868
		internal MethodDebugInformation debug_info;

		// Token: 0x04000365 RID: 869
		internal Collection<CustomDebugInformation> custom_infos;
	}
}
