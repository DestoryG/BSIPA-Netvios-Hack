using System;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x02000089 RID: 137
	public sealed class MethodDefinition : MethodReference, IMemberDefinition, ICustomAttributeProvider, IMetadataTokenProvider, ISecurityDeclarationProvider, ICustomDebugInformationProvider
	{
		// Token: 0x17000121 RID: 289
		// (get) Token: 0x06000557 RID: 1367 RVA: 0x000143A9 File Offset: 0x000125A9
		// (set) Token: 0x06000558 RID: 1368 RVA: 0x000143B1 File Offset: 0x000125B1
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

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x06000559 RID: 1369 RVA: 0x000143D6 File Offset: 0x000125D6
		// (set) Token: 0x0600055A RID: 1370 RVA: 0x000143DE File Offset: 0x000125DE
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

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x0600055B RID: 1371 RVA: 0x000143FE File Offset: 0x000125FE
		// (set) Token: 0x0600055C RID: 1372 RVA: 0x00014406 File Offset: 0x00012606
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

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x0600055D RID: 1373 RVA: 0x00014426 File Offset: 0x00012626
		// (set) Token: 0x0600055E RID: 1374 RVA: 0x00014464 File Offset: 0x00012664
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

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x0600055F RID: 1375 RVA: 0x0001446D File Offset: 0x0001266D
		// (set) Token: 0x06000560 RID: 1376 RVA: 0x000112F3 File Offset: 0x0000F4F3
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

		// Token: 0x06000561 RID: 1377 RVA: 0x0001447C File Offset: 0x0001267C
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
			module.Read<MethodDefinition>(this, delegate(MethodDefinition method, MetadataReader reader)
			{
				reader.ReadAllSemantics(method);
			});
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x06000562 RID: 1378 RVA: 0x000144CE File Offset: 0x000126CE
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

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x06000563 RID: 1379 RVA: 0x000144F3 File Offset: 0x000126F3
		public Collection<SecurityDeclaration> SecurityDeclarations
		{
			get
			{
				return this.security_declarations ?? this.GetSecurityDeclarations(ref this.security_declarations, this.Module);
			}
		}

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x06000564 RID: 1380 RVA: 0x00014511 File Offset: 0x00012711
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

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x06000565 RID: 1381 RVA: 0x00014536 File Offset: 0x00012736
		public Collection<CustomAttribute> CustomAttributes
		{
			get
			{
				return this.custom_attributes ?? this.GetCustomAttributes(ref this.custom_attributes, this.Module);
			}
		}

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x06000566 RID: 1382 RVA: 0x00014554 File Offset: 0x00012754
		public int RVA
		{
			get
			{
				return (int)this.rva;
			}
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x06000567 RID: 1383 RVA: 0x0001455C File Offset: 0x0001275C
		public bool HasBody
		{
			get
			{
				return (this.attributes & 1024) == 0 && (this.attributes & 8192) == 0 && (this.impl_attributes & 4096) == 0 && (this.impl_attributes & 1) == 0 && (this.impl_attributes & 4) == 0 && (this.impl_attributes & 3) == 0;
			}
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x06000568 RID: 1384 RVA: 0x000145B4 File Offset: 0x000127B4
		// (set) Token: 0x06000569 RID: 1385 RVA: 0x00014628 File Offset: 0x00012828
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
				return this.body = new MethodBody(this);
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

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x0600056A RID: 1386 RVA: 0x00014688 File Offset: 0x00012888
		// (set) Token: 0x0600056B RID: 1387 RVA: 0x000146C8 File Offset: 0x000128C8
		public MethodDebugInformation DebugInformation
		{
			get
			{
				Mixin.Read(this.Body);
				if (this.debug_info != null)
				{
					return this.debug_info;
				}
				MethodDebugInformation methodDebugInformation;
				if ((methodDebugInformation = this.debug_info) == null)
				{
					methodDebugInformation = (this.debug_info = new MethodDebugInformation(this));
				}
				return methodDebugInformation;
			}
			set
			{
				this.debug_info = value;
			}
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x0600056C RID: 1388 RVA: 0x000146D1 File Offset: 0x000128D1
		public bool HasPInvokeInfo
		{
			get
			{
				return this.pinvoke != null || this.IsPInvokeImpl;
			}
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x0600056D RID: 1389 RVA: 0x000146E4 File Offset: 0x000128E4
		// (set) Token: 0x0600056E RID: 1390 RVA: 0x00014743 File Offset: 0x00012943
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

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x0600056F RID: 1391 RVA: 0x00014754 File Offset: 0x00012954
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

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x06000570 RID: 1392 RVA: 0x000147B0 File Offset: 0x000129B0
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
				return this.overrides = new Collection<MethodReference>();
			}
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x06000571 RID: 1393 RVA: 0x00014814 File Offset: 0x00012A14
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

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x06000572 RID: 1394 RVA: 0x00014839 File Offset: 0x00012A39
		public override Collection<GenericParameter> GenericParameters
		{
			get
			{
				return this.generic_parameters ?? this.GetGenericParameters(ref this.generic_parameters, this.Module);
			}
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x06000573 RID: 1395 RVA: 0x00014857 File Offset: 0x00012A57
		public bool HasCustomDebugInformations
		{
			get
			{
				Mixin.Read(this.Body);
				return !this.custom_infos.IsNullOrEmpty<CustomDebugInformation>();
			}
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x06000574 RID: 1396 RVA: 0x00014874 File Offset: 0x00012A74
		public Collection<CustomDebugInformation> CustomDebugInformations
		{
			get
			{
				Mixin.Read(this.Body);
				Collection<CustomDebugInformation> collection;
				if ((collection = this.custom_infos) == null)
				{
					collection = (this.custom_infos = new Collection<CustomDebugInformation>());
				}
				return collection;
			}
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x06000575 RID: 1397 RVA: 0x000148A4 File Offset: 0x00012AA4
		// (set) Token: 0x06000576 RID: 1398 RVA: 0x000148B3 File Offset: 0x00012AB3
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

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x06000577 RID: 1399 RVA: 0x000148C9 File Offset: 0x00012AC9
		// (set) Token: 0x06000578 RID: 1400 RVA: 0x000148D8 File Offset: 0x00012AD8
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

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x06000579 RID: 1401 RVA: 0x000148EE File Offset: 0x00012AEE
		// (set) Token: 0x0600057A RID: 1402 RVA: 0x000148FD File Offset: 0x00012AFD
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

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x0600057B RID: 1403 RVA: 0x00014913 File Offset: 0x00012B13
		// (set) Token: 0x0600057C RID: 1404 RVA: 0x00014922 File Offset: 0x00012B22
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

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x0600057D RID: 1405 RVA: 0x00014938 File Offset: 0x00012B38
		// (set) Token: 0x0600057E RID: 1406 RVA: 0x00014947 File Offset: 0x00012B47
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

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x0600057F RID: 1407 RVA: 0x0001495D File Offset: 0x00012B5D
		// (set) Token: 0x06000580 RID: 1408 RVA: 0x0001496C File Offset: 0x00012B6C
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

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x06000581 RID: 1409 RVA: 0x00014982 File Offset: 0x00012B82
		// (set) Token: 0x06000582 RID: 1410 RVA: 0x00014991 File Offset: 0x00012B91
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

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x06000583 RID: 1411 RVA: 0x000149A7 File Offset: 0x00012BA7
		// (set) Token: 0x06000584 RID: 1412 RVA: 0x000149B6 File Offset: 0x00012BB6
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

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x06000585 RID: 1413 RVA: 0x000149CC File Offset: 0x00012BCC
		// (set) Token: 0x06000586 RID: 1414 RVA: 0x000149DB File Offset: 0x00012BDB
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

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x06000587 RID: 1415 RVA: 0x000149F1 File Offset: 0x00012BF1
		// (set) Token: 0x06000588 RID: 1416 RVA: 0x00014A00 File Offset: 0x00012C00
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

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x06000589 RID: 1417 RVA: 0x00014A16 File Offset: 0x00012C16
		// (set) Token: 0x0600058A RID: 1418 RVA: 0x00014A28 File Offset: 0x00012C28
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

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x0600058B RID: 1419 RVA: 0x00014A41 File Offset: 0x00012C41
		// (set) Token: 0x0600058C RID: 1420 RVA: 0x00014A54 File Offset: 0x00012C54
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

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x0600058D RID: 1421 RVA: 0x00014A6E File Offset: 0x00012C6E
		// (set) Token: 0x0600058E RID: 1422 RVA: 0x00014A85 File Offset: 0x00012C85
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

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x0600058F RID: 1423 RVA: 0x00014AA3 File Offset: 0x00012CA3
		// (set) Token: 0x06000590 RID: 1424 RVA: 0x00014AB5 File Offset: 0x00012CB5
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

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x06000591 RID: 1425 RVA: 0x00014ACE File Offset: 0x00012CCE
		// (set) Token: 0x06000592 RID: 1426 RVA: 0x00014AE0 File Offset: 0x00012CE0
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

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x06000593 RID: 1427 RVA: 0x00014AF9 File Offset: 0x00012CF9
		// (set) Token: 0x06000594 RID: 1428 RVA: 0x00014B0B File Offset: 0x00012D0B
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

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x06000595 RID: 1429 RVA: 0x00014B24 File Offset: 0x00012D24
		// (set) Token: 0x06000596 RID: 1430 RVA: 0x00014B36 File Offset: 0x00012D36
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

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x06000597 RID: 1431 RVA: 0x00014B4F File Offset: 0x00012D4F
		// (set) Token: 0x06000598 RID: 1432 RVA: 0x00014B5D File Offset: 0x00012D5D
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

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x06000599 RID: 1433 RVA: 0x00014B72 File Offset: 0x00012D72
		// (set) Token: 0x0600059A RID: 1434 RVA: 0x00014B84 File Offset: 0x00012D84
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

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x0600059B RID: 1435 RVA: 0x00014B9D File Offset: 0x00012D9D
		// (set) Token: 0x0600059C RID: 1436 RVA: 0x00014BAF File Offset: 0x00012DAF
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

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x0600059D RID: 1437 RVA: 0x00014BC8 File Offset: 0x00012DC8
		// (set) Token: 0x0600059E RID: 1438 RVA: 0x00014BD7 File Offset: 0x00012DD7
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

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x0600059F RID: 1439 RVA: 0x00014BED File Offset: 0x00012DED
		// (set) Token: 0x060005A0 RID: 1440 RVA: 0x00014BFC File Offset: 0x00012DFC
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

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x060005A1 RID: 1441 RVA: 0x00014C12 File Offset: 0x00012E12
		// (set) Token: 0x060005A2 RID: 1442 RVA: 0x00014C21 File Offset: 0x00012E21
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

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x060005A3 RID: 1443 RVA: 0x00014C37 File Offset: 0x00012E37
		// (set) Token: 0x060005A4 RID: 1444 RVA: 0x00014C46 File Offset: 0x00012E46
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

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x060005A5 RID: 1445 RVA: 0x00014C5C File Offset: 0x00012E5C
		// (set) Token: 0x060005A6 RID: 1446 RVA: 0x00014C6B File Offset: 0x00012E6B
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

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x060005A7 RID: 1447 RVA: 0x00014C81 File Offset: 0x00012E81
		// (set) Token: 0x060005A8 RID: 1448 RVA: 0x00014C90 File Offset: 0x00012E90
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

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x060005A9 RID: 1449 RVA: 0x00014CA6 File Offset: 0x00012EA6
		// (set) Token: 0x060005AA RID: 1450 RVA: 0x00014CB8 File Offset: 0x00012EB8
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

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x060005AB RID: 1451 RVA: 0x00014CD1 File Offset: 0x00012ED1
		// (set) Token: 0x060005AC RID: 1452 RVA: 0x00014CE3 File Offset: 0x00012EE3
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

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x060005AD RID: 1453 RVA: 0x00014CFC File Offset: 0x00012EFC
		// (set) Token: 0x060005AE RID: 1454 RVA: 0x00014D0B File Offset: 0x00012F0B
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

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x060005AF RID: 1455 RVA: 0x00014D21 File Offset: 0x00012F21
		// (set) Token: 0x060005B0 RID: 1456 RVA: 0x00014D2F File Offset: 0x00012F2F
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

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x060005B1 RID: 1457 RVA: 0x00014D44 File Offset: 0x00012F44
		// (set) Token: 0x060005B2 RID: 1458 RVA: 0x00014D53 File Offset: 0x00012F53
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

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x060005B3 RID: 1459 RVA: 0x00014D69 File Offset: 0x00012F69
		// (set) Token: 0x060005B4 RID: 1460 RVA: 0x00014D7B File Offset: 0x00012F7B
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

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x060005B5 RID: 1461 RVA: 0x00014D94 File Offset: 0x00012F94
		// (set) Token: 0x060005B6 RID: 1462 RVA: 0x00014D9D File Offset: 0x00012F9D
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

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x060005B7 RID: 1463 RVA: 0x00014DA7 File Offset: 0x00012FA7
		// (set) Token: 0x060005B8 RID: 1464 RVA: 0x00014DB0 File Offset: 0x00012FB0
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

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x060005B9 RID: 1465 RVA: 0x00014DBA File Offset: 0x00012FBA
		// (set) Token: 0x060005BA RID: 1466 RVA: 0x00014DC3 File Offset: 0x00012FC3
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

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x060005BB RID: 1467 RVA: 0x00014DCD File Offset: 0x00012FCD
		// (set) Token: 0x060005BC RID: 1468 RVA: 0x00014DD6 File Offset: 0x00012FD6
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

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x060005BD RID: 1469 RVA: 0x00014DE0 File Offset: 0x00012FE0
		// (set) Token: 0x060005BE RID: 1470 RVA: 0x00014DEA File Offset: 0x00012FEA
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

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x060005BF RID: 1471 RVA: 0x00014DF5 File Offset: 0x00012FF5
		// (set) Token: 0x060005C0 RID: 1472 RVA: 0x00014DFF File Offset: 0x00012FFF
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

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x060005C1 RID: 1473 RVA: 0x00010BA8 File Offset: 0x0000EDA8
		// (set) Token: 0x060005C2 RID: 1474 RVA: 0x00010BB5 File Offset: 0x0000EDB5
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

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x060005C3 RID: 1475 RVA: 0x00014E0A File Offset: 0x0001300A
		public bool IsConstructor
		{
			get
			{
				return this.IsRuntimeSpecialName && this.IsSpecialName && (this.Name == ".cctor" || this.Name == ".ctor");
			}
		}

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x060005C4 RID: 1476 RVA: 0x00002BE8 File Offset: 0x00000DE8
		public override bool IsDefinition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060005C5 RID: 1477 RVA: 0x00014E42 File Offset: 0x00013042
		internal MethodDefinition()
		{
			this.token = new MetadataToken(TokenType.Method);
		}

		// Token: 0x060005C6 RID: 1478 RVA: 0x00014E5A File Offset: 0x0001305A
		public MethodDefinition(string name, MethodAttributes attributes, TypeReference returnType)
			: base(name, returnType)
		{
			this.attributes = (ushort)attributes;
			this.HasThis = !this.IsStatic;
			this.token = new MetadataToken(TokenType.Method);
		}

		// Token: 0x060005C7 RID: 1479 RVA: 0x00002740 File Offset: 0x00000940
		public override MethodDefinition Resolve()
		{
			return this;
		}

		// Token: 0x04000140 RID: 320
		private ushort attributes;

		// Token: 0x04000141 RID: 321
		private ushort impl_attributes;

		// Token: 0x04000142 RID: 322
		internal volatile bool sem_attrs_ready;

		// Token: 0x04000143 RID: 323
		internal MethodSemanticsAttributes sem_attrs;

		// Token: 0x04000144 RID: 324
		private Collection<CustomAttribute> custom_attributes;

		// Token: 0x04000145 RID: 325
		private Collection<SecurityDeclaration> security_declarations;

		// Token: 0x04000146 RID: 326
		internal uint rva;

		// Token: 0x04000147 RID: 327
		internal PInvokeInfo pinvoke;

		// Token: 0x04000148 RID: 328
		private Collection<MethodReference> overrides;

		// Token: 0x04000149 RID: 329
		internal MethodBody body;

		// Token: 0x0400014A RID: 330
		internal MethodDebugInformation debug_info;

		// Token: 0x0400014B RID: 331
		internal Collection<CustomDebugInformation> custom_infos;
	}
}
