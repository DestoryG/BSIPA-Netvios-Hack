using System;
using System.Diagnostics;
using Google.Protobuf.Collections;

namespace Google.Protobuf.Reflection
{
	// Token: 0x0200005A RID: 90
	public sealed class FileOptions : IExtendableMessage<FileOptions>, IMessage<FileOptions>, IMessage, IEquatable<FileOptions>, IDeepCloneable<FileOptions>
	{
		// Token: 0x17000146 RID: 326
		// (get) Token: 0x06000596 RID: 1430 RVA: 0x00015DB1 File Offset: 0x00013FB1
		private ExtensionSet<FileOptions> _Extensions
		{
			get
			{
				return this._extensions;
			}
		}

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x06000597 RID: 1431 RVA: 0x00015DB9 File Offset: 0x00013FB9
		[DebuggerNonUserCode]
		public static MessageParser<FileOptions> Parser
		{
			get
			{
				return FileOptions._parser;
			}
		}

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x06000598 RID: 1432 RVA: 0x00015DC0 File Offset: 0x00013FC0
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return DescriptorReflection.Descriptor.MessageTypes[10];
			}
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x06000599 RID: 1433 RVA: 0x00015DD3 File Offset: 0x00013FD3
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return FileOptions.Descriptor;
			}
		}

		// Token: 0x0600059A RID: 1434 RVA: 0x00015DDA File Offset: 0x00013FDA
		[DebuggerNonUserCode]
		public FileOptions()
		{
		}

		// Token: 0x0600059B RID: 1435 RVA: 0x00015DF0 File Offset: 0x00013FF0
		[DebuggerNonUserCode]
		public FileOptions(FileOptions other)
			: this()
		{
			this._hasBits0 = other._hasBits0;
			this.javaPackage_ = other.javaPackage_;
			this.javaOuterClassname_ = other.javaOuterClassname_;
			this.javaMultipleFiles_ = other.javaMultipleFiles_;
			this.javaGenerateEqualsAndHash_ = other.javaGenerateEqualsAndHash_;
			this.javaStringCheckUtf8_ = other.javaStringCheckUtf8_;
			this.optimizeFor_ = other.optimizeFor_;
			this.goPackage_ = other.goPackage_;
			this.ccGenericServices_ = other.ccGenericServices_;
			this.javaGenericServices_ = other.javaGenericServices_;
			this.pyGenericServices_ = other.pyGenericServices_;
			this.phpGenericServices_ = other.phpGenericServices_;
			this.deprecated_ = other.deprecated_;
			this.ccEnableArenas_ = other.ccEnableArenas_;
			this.objcClassPrefix_ = other.objcClassPrefix_;
			this.csharpNamespace_ = other.csharpNamespace_;
			this.swiftPrefix_ = other.swiftPrefix_;
			this.phpClassPrefix_ = other.phpClassPrefix_;
			this.phpNamespace_ = other.phpNamespace_;
			this.phpMetadataNamespace_ = other.phpMetadataNamespace_;
			this.rubyPackage_ = other.rubyPackage_;
			this.uninterpretedOption_ = other.uninterpretedOption_.Clone();
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
			this._extensions = ExtensionSet.Clone<FileOptions>(other._extensions);
		}

		// Token: 0x0600059C RID: 1436 RVA: 0x00015F32 File Offset: 0x00014132
		[DebuggerNonUserCode]
		public FileOptions Clone()
		{
			return new FileOptions(this);
		}

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x0600059D RID: 1437 RVA: 0x00015F3A File Offset: 0x0001413A
		// (set) Token: 0x0600059E RID: 1438 RVA: 0x00015F4B File Offset: 0x0001414B
		[DebuggerNonUserCode]
		public string JavaPackage
		{
			get
			{
				return this.javaPackage_ ?? FileOptions.JavaPackageDefaultValue;
			}
			set
			{
				this.javaPackage_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x0600059F RID: 1439 RVA: 0x00015F5E File Offset: 0x0001415E
		[DebuggerNonUserCode]
		public bool HasJavaPackage
		{
			get
			{
				return this.javaPackage_ != null;
			}
		}

		// Token: 0x060005A0 RID: 1440 RVA: 0x00015F69 File Offset: 0x00014169
		[DebuggerNonUserCode]
		public void ClearJavaPackage()
		{
			this.javaPackage_ = null;
		}

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x060005A1 RID: 1441 RVA: 0x00015F72 File Offset: 0x00014172
		// (set) Token: 0x060005A2 RID: 1442 RVA: 0x00015F83 File Offset: 0x00014183
		[DebuggerNonUserCode]
		public string JavaOuterClassname
		{
			get
			{
				return this.javaOuterClassname_ ?? FileOptions.JavaOuterClassnameDefaultValue;
			}
			set
			{
				this.javaOuterClassname_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x060005A3 RID: 1443 RVA: 0x00015F96 File Offset: 0x00014196
		[DebuggerNonUserCode]
		public bool HasJavaOuterClassname
		{
			get
			{
				return this.javaOuterClassname_ != null;
			}
		}

		// Token: 0x060005A4 RID: 1444 RVA: 0x00015FA1 File Offset: 0x000141A1
		[DebuggerNonUserCode]
		public void ClearJavaOuterClassname()
		{
			this.javaOuterClassname_ = null;
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x060005A5 RID: 1445 RVA: 0x00015FAA File Offset: 0x000141AA
		// (set) Token: 0x060005A6 RID: 1446 RVA: 0x00015FC2 File Offset: 0x000141C2
		[DebuggerNonUserCode]
		public bool JavaMultipleFiles
		{
			get
			{
				if ((this._hasBits0 & 2) != 0)
				{
					return this.javaMultipleFiles_;
				}
				return FileOptions.JavaMultipleFilesDefaultValue;
			}
			set
			{
				this._hasBits0 |= 2;
				this.javaMultipleFiles_ = value;
			}
		}

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x060005A7 RID: 1447 RVA: 0x00015FD9 File Offset: 0x000141D9
		[DebuggerNonUserCode]
		public bool HasJavaMultipleFiles
		{
			get
			{
				return (this._hasBits0 & 2) != 0;
			}
		}

		// Token: 0x060005A8 RID: 1448 RVA: 0x00015FE6 File Offset: 0x000141E6
		[DebuggerNonUserCode]
		public void ClearJavaMultipleFiles()
		{
			this._hasBits0 &= -3;
		}

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x060005A9 RID: 1449 RVA: 0x00015FF7 File Offset: 0x000141F7
		// (set) Token: 0x060005AA RID: 1450 RVA: 0x00016010 File Offset: 0x00014210
		[Obsolete]
		[DebuggerNonUserCode]
		public bool JavaGenerateEqualsAndHash
		{
			get
			{
				if ((this._hasBits0 & 32) != 0)
				{
					return this.javaGenerateEqualsAndHash_;
				}
				return FileOptions.JavaGenerateEqualsAndHashDefaultValue;
			}
			set
			{
				this._hasBits0 |= 32;
				this.javaGenerateEqualsAndHash_ = value;
			}
		}

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x060005AB RID: 1451 RVA: 0x00016028 File Offset: 0x00014228
		[Obsolete]
		[DebuggerNonUserCode]
		public bool HasJavaGenerateEqualsAndHash
		{
			get
			{
				return (this._hasBits0 & 32) != 0;
			}
		}

		// Token: 0x060005AC RID: 1452 RVA: 0x00016036 File Offset: 0x00014236
		[Obsolete]
		[DebuggerNonUserCode]
		public void ClearJavaGenerateEqualsAndHash()
		{
			this._hasBits0 &= -33;
		}

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x060005AD RID: 1453 RVA: 0x00016047 File Offset: 0x00014247
		// (set) Token: 0x060005AE RID: 1454 RVA: 0x00016063 File Offset: 0x00014263
		[DebuggerNonUserCode]
		public bool JavaStringCheckUtf8
		{
			get
			{
				if ((this._hasBits0 & 128) != 0)
				{
					return this.javaStringCheckUtf8_;
				}
				return FileOptions.JavaStringCheckUtf8DefaultValue;
			}
			set
			{
				this._hasBits0 |= 128;
				this.javaStringCheckUtf8_ = value;
			}
		}

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x060005AF RID: 1455 RVA: 0x0001607E File Offset: 0x0001427E
		[DebuggerNonUserCode]
		public bool HasJavaStringCheckUtf8
		{
			get
			{
				return (this._hasBits0 & 128) != 0;
			}
		}

		// Token: 0x060005B0 RID: 1456 RVA: 0x0001608F File Offset: 0x0001428F
		[DebuggerNonUserCode]
		public void ClearJavaStringCheckUtf8()
		{
			this._hasBits0 &= -129;
		}

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x060005B1 RID: 1457 RVA: 0x000160A3 File Offset: 0x000142A3
		// (set) Token: 0x060005B2 RID: 1458 RVA: 0x000160BB File Offset: 0x000142BB
		[DebuggerNonUserCode]
		public FileOptions.Types.OptimizeMode OptimizeFor
		{
			get
			{
				if ((this._hasBits0 & 1) != 0)
				{
					return this.optimizeFor_;
				}
				return FileOptions.OptimizeForDefaultValue;
			}
			set
			{
				this._hasBits0 |= 1;
				this.optimizeFor_ = value;
			}
		}

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x060005B3 RID: 1459 RVA: 0x000160D2 File Offset: 0x000142D2
		[DebuggerNonUserCode]
		public bool HasOptimizeFor
		{
			get
			{
				return (this._hasBits0 & 1) != 0;
			}
		}

		// Token: 0x060005B4 RID: 1460 RVA: 0x000160DF File Offset: 0x000142DF
		[DebuggerNonUserCode]
		public void ClearOptimizeFor()
		{
			this._hasBits0 &= -2;
		}

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x060005B5 RID: 1461 RVA: 0x000160F0 File Offset: 0x000142F0
		// (set) Token: 0x060005B6 RID: 1462 RVA: 0x00016101 File Offset: 0x00014301
		[DebuggerNonUserCode]
		public string GoPackage
		{
			get
			{
				return this.goPackage_ ?? FileOptions.GoPackageDefaultValue;
			}
			set
			{
				this.goPackage_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x060005B7 RID: 1463 RVA: 0x00016114 File Offset: 0x00014314
		[DebuggerNonUserCode]
		public bool HasGoPackage
		{
			get
			{
				return this.goPackage_ != null;
			}
		}

		// Token: 0x060005B8 RID: 1464 RVA: 0x0001611F File Offset: 0x0001431F
		[DebuggerNonUserCode]
		public void ClearGoPackage()
		{
			this.goPackage_ = null;
		}

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x060005B9 RID: 1465 RVA: 0x00016128 File Offset: 0x00014328
		// (set) Token: 0x060005BA RID: 1466 RVA: 0x00016140 File Offset: 0x00014340
		[DebuggerNonUserCode]
		public bool CcGenericServices
		{
			get
			{
				if ((this._hasBits0 & 4) != 0)
				{
					return this.ccGenericServices_;
				}
				return FileOptions.CcGenericServicesDefaultValue;
			}
			set
			{
				this._hasBits0 |= 4;
				this.ccGenericServices_ = value;
			}
		}

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x060005BB RID: 1467 RVA: 0x00016157 File Offset: 0x00014357
		[DebuggerNonUserCode]
		public bool HasCcGenericServices
		{
			get
			{
				return (this._hasBits0 & 4) != 0;
			}
		}

		// Token: 0x060005BC RID: 1468 RVA: 0x00016164 File Offset: 0x00014364
		[DebuggerNonUserCode]
		public void ClearCcGenericServices()
		{
			this._hasBits0 &= -5;
		}

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x060005BD RID: 1469 RVA: 0x00016175 File Offset: 0x00014375
		// (set) Token: 0x060005BE RID: 1470 RVA: 0x0001618D File Offset: 0x0001438D
		[DebuggerNonUserCode]
		public bool JavaGenericServices
		{
			get
			{
				if ((this._hasBits0 & 8) != 0)
				{
					return this.javaGenericServices_;
				}
				return FileOptions.JavaGenericServicesDefaultValue;
			}
			set
			{
				this._hasBits0 |= 8;
				this.javaGenericServices_ = value;
			}
		}

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x060005BF RID: 1471 RVA: 0x000161A4 File Offset: 0x000143A4
		[DebuggerNonUserCode]
		public bool HasJavaGenericServices
		{
			get
			{
				return (this._hasBits0 & 8) != 0;
			}
		}

		// Token: 0x060005C0 RID: 1472 RVA: 0x000161B1 File Offset: 0x000143B1
		[DebuggerNonUserCode]
		public void ClearJavaGenericServices()
		{
			this._hasBits0 &= -9;
		}

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x060005C1 RID: 1473 RVA: 0x000161C2 File Offset: 0x000143C2
		// (set) Token: 0x060005C2 RID: 1474 RVA: 0x000161DB File Offset: 0x000143DB
		[DebuggerNonUserCode]
		public bool PyGenericServices
		{
			get
			{
				if ((this._hasBits0 & 16) != 0)
				{
					return this.pyGenericServices_;
				}
				return FileOptions.PyGenericServicesDefaultValue;
			}
			set
			{
				this._hasBits0 |= 16;
				this.pyGenericServices_ = value;
			}
		}

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x060005C3 RID: 1475 RVA: 0x000161F3 File Offset: 0x000143F3
		[DebuggerNonUserCode]
		public bool HasPyGenericServices
		{
			get
			{
				return (this._hasBits0 & 16) != 0;
			}
		}

		// Token: 0x060005C4 RID: 1476 RVA: 0x00016201 File Offset: 0x00014401
		[DebuggerNonUserCode]
		public void ClearPyGenericServices()
		{
			this._hasBits0 &= -17;
		}

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x060005C5 RID: 1477 RVA: 0x00016212 File Offset: 0x00014412
		// (set) Token: 0x060005C6 RID: 1478 RVA: 0x0001622E File Offset: 0x0001442E
		[DebuggerNonUserCode]
		public bool PhpGenericServices
		{
			get
			{
				if ((this._hasBits0 & 512) != 0)
				{
					return this.phpGenericServices_;
				}
				return FileOptions.PhpGenericServicesDefaultValue;
			}
			set
			{
				this._hasBits0 |= 512;
				this.phpGenericServices_ = value;
			}
		}

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x060005C7 RID: 1479 RVA: 0x00016249 File Offset: 0x00014449
		[DebuggerNonUserCode]
		public bool HasPhpGenericServices
		{
			get
			{
				return (this._hasBits0 & 512) != 0;
			}
		}

		// Token: 0x060005C8 RID: 1480 RVA: 0x0001625A File Offset: 0x0001445A
		[DebuggerNonUserCode]
		public void ClearPhpGenericServices()
		{
			this._hasBits0 &= -513;
		}

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x060005C9 RID: 1481 RVA: 0x0001626E File Offset: 0x0001446E
		// (set) Token: 0x060005CA RID: 1482 RVA: 0x00016287 File Offset: 0x00014487
		[DebuggerNonUserCode]
		public bool Deprecated
		{
			get
			{
				if ((this._hasBits0 & 64) != 0)
				{
					return this.deprecated_;
				}
				return FileOptions.DeprecatedDefaultValue;
			}
			set
			{
				this._hasBits0 |= 64;
				this.deprecated_ = value;
			}
		}

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x060005CB RID: 1483 RVA: 0x0001629F File Offset: 0x0001449F
		[DebuggerNonUserCode]
		public bool HasDeprecated
		{
			get
			{
				return (this._hasBits0 & 64) != 0;
			}
		}

		// Token: 0x060005CC RID: 1484 RVA: 0x000162AD File Offset: 0x000144AD
		[DebuggerNonUserCode]
		public void ClearDeprecated()
		{
			this._hasBits0 &= -65;
		}

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x060005CD RID: 1485 RVA: 0x000162BE File Offset: 0x000144BE
		// (set) Token: 0x060005CE RID: 1486 RVA: 0x000162DA File Offset: 0x000144DA
		[DebuggerNonUserCode]
		public bool CcEnableArenas
		{
			get
			{
				if ((this._hasBits0 & 256) != 0)
				{
					return this.ccEnableArenas_;
				}
				return FileOptions.CcEnableArenasDefaultValue;
			}
			set
			{
				this._hasBits0 |= 256;
				this.ccEnableArenas_ = value;
			}
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x060005CF RID: 1487 RVA: 0x000162F5 File Offset: 0x000144F5
		[DebuggerNonUserCode]
		public bool HasCcEnableArenas
		{
			get
			{
				return (this._hasBits0 & 256) != 0;
			}
		}

		// Token: 0x060005D0 RID: 1488 RVA: 0x00016306 File Offset: 0x00014506
		[DebuggerNonUserCode]
		public void ClearCcEnableArenas()
		{
			this._hasBits0 &= -257;
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x060005D1 RID: 1489 RVA: 0x0001631A File Offset: 0x0001451A
		// (set) Token: 0x060005D2 RID: 1490 RVA: 0x0001632B File Offset: 0x0001452B
		[DebuggerNonUserCode]
		public string ObjcClassPrefix
		{
			get
			{
				return this.objcClassPrefix_ ?? FileOptions.ObjcClassPrefixDefaultValue;
			}
			set
			{
				this.objcClassPrefix_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x060005D3 RID: 1491 RVA: 0x0001633E File Offset: 0x0001453E
		[DebuggerNonUserCode]
		public bool HasObjcClassPrefix
		{
			get
			{
				return this.objcClassPrefix_ != null;
			}
		}

		// Token: 0x060005D4 RID: 1492 RVA: 0x00016349 File Offset: 0x00014549
		[DebuggerNonUserCode]
		public void ClearObjcClassPrefix()
		{
			this.objcClassPrefix_ = null;
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x060005D5 RID: 1493 RVA: 0x00016352 File Offset: 0x00014552
		// (set) Token: 0x060005D6 RID: 1494 RVA: 0x00016363 File Offset: 0x00014563
		[DebuggerNonUserCode]
		public string CsharpNamespace
		{
			get
			{
				return this.csharpNamespace_ ?? FileOptions.CsharpNamespaceDefaultValue;
			}
			set
			{
				this.csharpNamespace_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x060005D7 RID: 1495 RVA: 0x00016376 File Offset: 0x00014576
		[DebuggerNonUserCode]
		public bool HasCsharpNamespace
		{
			get
			{
				return this.csharpNamespace_ != null;
			}
		}

		// Token: 0x060005D8 RID: 1496 RVA: 0x00016381 File Offset: 0x00014581
		[DebuggerNonUserCode]
		public void ClearCsharpNamespace()
		{
			this.csharpNamespace_ = null;
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x060005D9 RID: 1497 RVA: 0x0001638A File Offset: 0x0001458A
		// (set) Token: 0x060005DA RID: 1498 RVA: 0x0001639B File Offset: 0x0001459B
		[DebuggerNonUserCode]
		public string SwiftPrefix
		{
			get
			{
				return this.swiftPrefix_ ?? FileOptions.SwiftPrefixDefaultValue;
			}
			set
			{
				this.swiftPrefix_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x060005DB RID: 1499 RVA: 0x000163AE File Offset: 0x000145AE
		[DebuggerNonUserCode]
		public bool HasSwiftPrefix
		{
			get
			{
				return this.swiftPrefix_ != null;
			}
		}

		// Token: 0x060005DC RID: 1500 RVA: 0x000163B9 File Offset: 0x000145B9
		[DebuggerNonUserCode]
		public void ClearSwiftPrefix()
		{
			this.swiftPrefix_ = null;
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x060005DD RID: 1501 RVA: 0x000163C2 File Offset: 0x000145C2
		// (set) Token: 0x060005DE RID: 1502 RVA: 0x000163D3 File Offset: 0x000145D3
		[DebuggerNonUserCode]
		public string PhpClassPrefix
		{
			get
			{
				return this.phpClassPrefix_ ?? FileOptions.PhpClassPrefixDefaultValue;
			}
			set
			{
				this.phpClassPrefix_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x060005DF RID: 1503 RVA: 0x000163E6 File Offset: 0x000145E6
		[DebuggerNonUserCode]
		public bool HasPhpClassPrefix
		{
			get
			{
				return this.phpClassPrefix_ != null;
			}
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x000163F1 File Offset: 0x000145F1
		[DebuggerNonUserCode]
		public void ClearPhpClassPrefix()
		{
			this.phpClassPrefix_ = null;
		}

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x060005E1 RID: 1505 RVA: 0x000163FA File Offset: 0x000145FA
		// (set) Token: 0x060005E2 RID: 1506 RVA: 0x0001640B File Offset: 0x0001460B
		[DebuggerNonUserCode]
		public string PhpNamespace
		{
			get
			{
				return this.phpNamespace_ ?? FileOptions.PhpNamespaceDefaultValue;
			}
			set
			{
				this.phpNamespace_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x060005E3 RID: 1507 RVA: 0x0001641E File Offset: 0x0001461E
		[DebuggerNonUserCode]
		public bool HasPhpNamespace
		{
			get
			{
				return this.phpNamespace_ != null;
			}
		}

		// Token: 0x060005E4 RID: 1508 RVA: 0x00016429 File Offset: 0x00014629
		[DebuggerNonUserCode]
		public void ClearPhpNamespace()
		{
			this.phpNamespace_ = null;
		}

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x060005E5 RID: 1509 RVA: 0x00016432 File Offset: 0x00014632
		// (set) Token: 0x060005E6 RID: 1510 RVA: 0x00016443 File Offset: 0x00014643
		[DebuggerNonUserCode]
		public string PhpMetadataNamespace
		{
			get
			{
				return this.phpMetadataNamespace_ ?? FileOptions.PhpMetadataNamespaceDefaultValue;
			}
			set
			{
				this.phpMetadataNamespace_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x060005E7 RID: 1511 RVA: 0x00016456 File Offset: 0x00014656
		[DebuggerNonUserCode]
		public bool HasPhpMetadataNamespace
		{
			get
			{
				return this.phpMetadataNamespace_ != null;
			}
		}

		// Token: 0x060005E8 RID: 1512 RVA: 0x00016461 File Offset: 0x00014661
		[DebuggerNonUserCode]
		public void ClearPhpMetadataNamespace()
		{
			this.phpMetadataNamespace_ = null;
		}

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x060005E9 RID: 1513 RVA: 0x0001646A File Offset: 0x0001466A
		// (set) Token: 0x060005EA RID: 1514 RVA: 0x0001647B File Offset: 0x0001467B
		[DebuggerNonUserCode]
		public string RubyPackage
		{
			get
			{
				return this.rubyPackage_ ?? FileOptions.RubyPackageDefaultValue;
			}
			set
			{
				this.rubyPackage_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x060005EB RID: 1515 RVA: 0x0001648E File Offset: 0x0001468E
		[DebuggerNonUserCode]
		public bool HasRubyPackage
		{
			get
			{
				return this.rubyPackage_ != null;
			}
		}

		// Token: 0x060005EC RID: 1516 RVA: 0x00016499 File Offset: 0x00014699
		[DebuggerNonUserCode]
		public void ClearRubyPackage()
		{
			this.rubyPackage_ = null;
		}

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x060005ED RID: 1517 RVA: 0x000164A2 File Offset: 0x000146A2
		[DebuggerNonUserCode]
		public RepeatedField<UninterpretedOption> UninterpretedOption
		{
			get
			{
				return this.uninterpretedOption_;
			}
		}

		// Token: 0x060005EE RID: 1518 RVA: 0x000164AA File Offset: 0x000146AA
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as FileOptions);
		}

		// Token: 0x060005EF RID: 1519 RVA: 0x000164B8 File Offset: 0x000146B8
		[DebuggerNonUserCode]
		public bool Equals(FileOptions other)
		{
			return other != null && (other == this || (!(this.JavaPackage != other.JavaPackage) && !(this.JavaOuterClassname != other.JavaOuterClassname) && this.JavaMultipleFiles == other.JavaMultipleFiles && this.JavaGenerateEqualsAndHash == other.JavaGenerateEqualsAndHash && this.JavaStringCheckUtf8 == other.JavaStringCheckUtf8 && this.OptimizeFor == other.OptimizeFor && !(this.GoPackage != other.GoPackage) && this.CcGenericServices == other.CcGenericServices && this.JavaGenericServices == other.JavaGenericServices && this.PyGenericServices == other.PyGenericServices && this.PhpGenericServices == other.PhpGenericServices && this.Deprecated == other.Deprecated && this.CcEnableArenas == other.CcEnableArenas && !(this.ObjcClassPrefix != other.ObjcClassPrefix) && !(this.CsharpNamespace != other.CsharpNamespace) && !(this.SwiftPrefix != other.SwiftPrefix) && !(this.PhpClassPrefix != other.PhpClassPrefix) && !(this.PhpNamespace != other.PhpNamespace) && !(this.PhpMetadataNamespace != other.PhpMetadataNamespace) && !(this.RubyPackage != other.RubyPackage) && this.uninterpretedOption_.Equals(other.uninterpretedOption_) && object.Equals(this._extensions, other._extensions) && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x060005F0 RID: 1520 RVA: 0x00016680 File Offset: 0x00014880
		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (this.HasJavaPackage)
			{
				num ^= this.JavaPackage.GetHashCode();
			}
			if (this.HasJavaOuterClassname)
			{
				num ^= this.JavaOuterClassname.GetHashCode();
			}
			if (this.HasJavaMultipleFiles)
			{
				num ^= this.JavaMultipleFiles.GetHashCode();
			}
			if (this.HasJavaGenerateEqualsAndHash)
			{
				num ^= this.JavaGenerateEqualsAndHash.GetHashCode();
			}
			if (this.HasJavaStringCheckUtf8)
			{
				num ^= this.JavaStringCheckUtf8.GetHashCode();
			}
			if (this.HasOptimizeFor)
			{
				num ^= this.OptimizeFor.GetHashCode();
			}
			if (this.HasGoPackage)
			{
				num ^= this.GoPackage.GetHashCode();
			}
			if (this.HasCcGenericServices)
			{
				num ^= this.CcGenericServices.GetHashCode();
			}
			if (this.HasJavaGenericServices)
			{
				num ^= this.JavaGenericServices.GetHashCode();
			}
			if (this.HasPyGenericServices)
			{
				num ^= this.PyGenericServices.GetHashCode();
			}
			if (this.HasPhpGenericServices)
			{
				num ^= this.PhpGenericServices.GetHashCode();
			}
			if (this.HasDeprecated)
			{
				num ^= this.Deprecated.GetHashCode();
			}
			if (this.HasCcEnableArenas)
			{
				num ^= this.CcEnableArenas.GetHashCode();
			}
			if (this.HasObjcClassPrefix)
			{
				num ^= this.ObjcClassPrefix.GetHashCode();
			}
			if (this.HasCsharpNamespace)
			{
				num ^= this.CsharpNamespace.GetHashCode();
			}
			if (this.HasSwiftPrefix)
			{
				num ^= this.SwiftPrefix.GetHashCode();
			}
			if (this.HasPhpClassPrefix)
			{
				num ^= this.PhpClassPrefix.GetHashCode();
			}
			if (this.HasPhpNamespace)
			{
				num ^= this.PhpNamespace.GetHashCode();
			}
			if (this.HasPhpMetadataNamespace)
			{
				num ^= this.PhpMetadataNamespace.GetHashCode();
			}
			if (this.HasRubyPackage)
			{
				num ^= this.RubyPackage.GetHashCode();
			}
			num ^= this.uninterpretedOption_.GetHashCode();
			if (this._extensions != null)
			{
				num ^= this._extensions.GetHashCode();
			}
			if (this._unknownFields != null)
			{
				num ^= this._unknownFields.GetHashCode();
			}
			return num;
		}

		// Token: 0x060005F1 RID: 1521 RVA: 0x000168A6 File Offset: 0x00014AA6
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x060005F2 RID: 1522 RVA: 0x000168B0 File Offset: 0x00014AB0
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.HasJavaPackage)
			{
				output.WriteRawTag(10);
				output.WriteString(this.JavaPackage);
			}
			if (this.HasJavaOuterClassname)
			{
				output.WriteRawTag(66);
				output.WriteString(this.JavaOuterClassname);
			}
			if (this.HasOptimizeFor)
			{
				output.WriteRawTag(72);
				output.WriteEnum((int)this.OptimizeFor);
			}
			if (this.HasJavaMultipleFiles)
			{
				output.WriteRawTag(80);
				output.WriteBool(this.JavaMultipleFiles);
			}
			if (this.HasGoPackage)
			{
				output.WriteRawTag(90);
				output.WriteString(this.GoPackage);
			}
			if (this.HasCcGenericServices)
			{
				output.WriteRawTag(128, 1);
				output.WriteBool(this.CcGenericServices);
			}
			if (this.HasJavaGenericServices)
			{
				output.WriteRawTag(136, 1);
				output.WriteBool(this.JavaGenericServices);
			}
			if (this.HasPyGenericServices)
			{
				output.WriteRawTag(144, 1);
				output.WriteBool(this.PyGenericServices);
			}
			if (this.HasJavaGenerateEqualsAndHash)
			{
				output.WriteRawTag(160, 1);
				output.WriteBool(this.JavaGenerateEqualsAndHash);
			}
			if (this.HasDeprecated)
			{
				output.WriteRawTag(184, 1);
				output.WriteBool(this.Deprecated);
			}
			if (this.HasJavaStringCheckUtf8)
			{
				output.WriteRawTag(216, 1);
				output.WriteBool(this.JavaStringCheckUtf8);
			}
			if (this.HasCcEnableArenas)
			{
				output.WriteRawTag(248, 1);
				output.WriteBool(this.CcEnableArenas);
			}
			if (this.HasObjcClassPrefix)
			{
				output.WriteRawTag(162, 2);
				output.WriteString(this.ObjcClassPrefix);
			}
			if (this.HasCsharpNamespace)
			{
				output.WriteRawTag(170, 2);
				output.WriteString(this.CsharpNamespace);
			}
			if (this.HasSwiftPrefix)
			{
				output.WriteRawTag(186, 2);
				output.WriteString(this.SwiftPrefix);
			}
			if (this.HasPhpClassPrefix)
			{
				output.WriteRawTag(194, 2);
				output.WriteString(this.PhpClassPrefix);
			}
			if (this.HasPhpNamespace)
			{
				output.WriteRawTag(202, 2);
				output.WriteString(this.PhpNamespace);
			}
			if (this.HasPhpGenericServices)
			{
				output.WriteRawTag(208, 2);
				output.WriteBool(this.PhpGenericServices);
			}
			if (this.HasPhpMetadataNamespace)
			{
				output.WriteRawTag(226, 2);
				output.WriteString(this.PhpMetadataNamespace);
			}
			if (this.HasRubyPackage)
			{
				output.WriteRawTag(234, 2);
				output.WriteString(this.RubyPackage);
			}
			this.uninterpretedOption_.WriteTo(output, FileOptions._repeated_uninterpretedOption_codec);
			if (this._extensions != null)
			{
				this._extensions.WriteTo(output);
			}
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x060005F3 RID: 1523 RVA: 0x00016B64 File Offset: 0x00014D64
		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.HasJavaPackage)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.JavaPackage);
			}
			if (this.HasJavaOuterClassname)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.JavaOuterClassname);
			}
			if (this.HasJavaMultipleFiles)
			{
				num += 2;
			}
			if (this.HasJavaGenerateEqualsAndHash)
			{
				num += 3;
			}
			if (this.HasJavaStringCheckUtf8)
			{
				num += 3;
			}
			if (this.HasOptimizeFor)
			{
				num += 1 + CodedOutputStream.ComputeEnumSize((int)this.OptimizeFor);
			}
			if (this.HasGoPackage)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.GoPackage);
			}
			if (this.HasCcGenericServices)
			{
				num += 3;
			}
			if (this.HasJavaGenericServices)
			{
				num += 3;
			}
			if (this.HasPyGenericServices)
			{
				num += 3;
			}
			if (this.HasPhpGenericServices)
			{
				num += 3;
			}
			if (this.HasDeprecated)
			{
				num += 3;
			}
			if (this.HasCcEnableArenas)
			{
				num += 3;
			}
			if (this.HasObjcClassPrefix)
			{
				num += 2 + CodedOutputStream.ComputeStringSize(this.ObjcClassPrefix);
			}
			if (this.HasCsharpNamespace)
			{
				num += 2 + CodedOutputStream.ComputeStringSize(this.CsharpNamespace);
			}
			if (this.HasSwiftPrefix)
			{
				num += 2 + CodedOutputStream.ComputeStringSize(this.SwiftPrefix);
			}
			if (this.HasPhpClassPrefix)
			{
				num += 2 + CodedOutputStream.ComputeStringSize(this.PhpClassPrefix);
			}
			if (this.HasPhpNamespace)
			{
				num += 2 + CodedOutputStream.ComputeStringSize(this.PhpNamespace);
			}
			if (this.HasPhpMetadataNamespace)
			{
				num += 2 + CodedOutputStream.ComputeStringSize(this.PhpMetadataNamespace);
			}
			if (this.HasRubyPackage)
			{
				num += 2 + CodedOutputStream.ComputeStringSize(this.RubyPackage);
			}
			num += this.uninterpretedOption_.CalculateSize(FileOptions._repeated_uninterpretedOption_codec);
			if (this._extensions != null)
			{
				num += this._extensions.CalculateSize();
			}
			if (this._unknownFields != null)
			{
				num += this._unknownFields.CalculateSize();
			}
			return num;
		}

		// Token: 0x060005F4 RID: 1524 RVA: 0x00016D28 File Offset: 0x00014F28
		[DebuggerNonUserCode]
		public void MergeFrom(FileOptions other)
		{
			if (other == null)
			{
				return;
			}
			if (other.HasJavaPackage)
			{
				this.JavaPackage = other.JavaPackage;
			}
			if (other.HasJavaOuterClassname)
			{
				this.JavaOuterClassname = other.JavaOuterClassname;
			}
			if (other.HasJavaMultipleFiles)
			{
				this.JavaMultipleFiles = other.JavaMultipleFiles;
			}
			if (other.HasJavaGenerateEqualsAndHash)
			{
				this.JavaGenerateEqualsAndHash = other.JavaGenerateEqualsAndHash;
			}
			if (other.HasJavaStringCheckUtf8)
			{
				this.JavaStringCheckUtf8 = other.JavaStringCheckUtf8;
			}
			if (other.HasOptimizeFor)
			{
				this.OptimizeFor = other.OptimizeFor;
			}
			if (other.HasGoPackage)
			{
				this.GoPackage = other.GoPackage;
			}
			if (other.HasCcGenericServices)
			{
				this.CcGenericServices = other.CcGenericServices;
			}
			if (other.HasJavaGenericServices)
			{
				this.JavaGenericServices = other.JavaGenericServices;
			}
			if (other.HasPyGenericServices)
			{
				this.PyGenericServices = other.PyGenericServices;
			}
			if (other.HasPhpGenericServices)
			{
				this.PhpGenericServices = other.PhpGenericServices;
			}
			if (other.HasDeprecated)
			{
				this.Deprecated = other.Deprecated;
			}
			if (other.HasCcEnableArenas)
			{
				this.CcEnableArenas = other.CcEnableArenas;
			}
			if (other.HasObjcClassPrefix)
			{
				this.ObjcClassPrefix = other.ObjcClassPrefix;
			}
			if (other.HasCsharpNamespace)
			{
				this.CsharpNamespace = other.CsharpNamespace;
			}
			if (other.HasSwiftPrefix)
			{
				this.SwiftPrefix = other.SwiftPrefix;
			}
			if (other.HasPhpClassPrefix)
			{
				this.PhpClassPrefix = other.PhpClassPrefix;
			}
			if (other.HasPhpNamespace)
			{
				this.PhpNamespace = other.PhpNamespace;
			}
			if (other.HasPhpMetadataNamespace)
			{
				this.PhpMetadataNamespace = other.PhpMetadataNamespace;
			}
			if (other.HasRubyPackage)
			{
				this.RubyPackage = other.RubyPackage;
			}
			this.uninterpretedOption_.Add(other.uninterpretedOption_);
			ExtensionSet.MergeFrom<FileOptions>(ref this._extensions, other._extensions);
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x060005F5 RID: 1525 RVA: 0x00016F04 File Offset: 0x00015104
		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 184U)
				{
					if (num <= 90U)
					{
						if (num <= 66U)
						{
							if (num == 10U)
							{
								this.JavaPackage = input.ReadString();
								continue;
							}
							if (num == 66U)
							{
								this.JavaOuterClassname = input.ReadString();
								continue;
							}
						}
						else
						{
							if (num == 72U)
							{
								this.OptimizeFor = (FileOptions.Types.OptimizeMode)input.ReadEnum();
								continue;
							}
							if (num == 80U)
							{
								this.JavaMultipleFiles = input.ReadBool();
								continue;
							}
							if (num == 90U)
							{
								this.GoPackage = input.ReadString();
								continue;
							}
						}
					}
					else if (num <= 136U)
					{
						if (num == 128U)
						{
							this.CcGenericServices = input.ReadBool();
							continue;
						}
						if (num == 136U)
						{
							this.JavaGenericServices = input.ReadBool();
							continue;
						}
					}
					else
					{
						if (num == 144U)
						{
							this.PyGenericServices = input.ReadBool();
							continue;
						}
						if (num == 160U)
						{
							this.JavaGenerateEqualsAndHash = input.ReadBool();
							continue;
						}
						if (num == 184U)
						{
							this.Deprecated = input.ReadBool();
							continue;
						}
					}
				}
				else if (num <= 314U)
				{
					if (num <= 248U)
					{
						if (num == 216U)
						{
							this.JavaStringCheckUtf8 = input.ReadBool();
							continue;
						}
						if (num == 248U)
						{
							this.CcEnableArenas = input.ReadBool();
							continue;
						}
					}
					else
					{
						if (num == 290U)
						{
							this.ObjcClassPrefix = input.ReadString();
							continue;
						}
						if (num == 298U)
						{
							this.CsharpNamespace = input.ReadString();
							continue;
						}
						if (num == 314U)
						{
							this.SwiftPrefix = input.ReadString();
							continue;
						}
					}
				}
				else if (num <= 336U)
				{
					if (num == 322U)
					{
						this.PhpClassPrefix = input.ReadString();
						continue;
					}
					if (num == 330U)
					{
						this.PhpNamespace = input.ReadString();
						continue;
					}
					if (num == 336U)
					{
						this.PhpGenericServices = input.ReadBool();
						continue;
					}
				}
				else
				{
					if (num == 354U)
					{
						this.PhpMetadataNamespace = input.ReadString();
						continue;
					}
					if (num == 362U)
					{
						this.RubyPackage = input.ReadString();
						continue;
					}
					if (num == 7994U)
					{
						this.uninterpretedOption_.AddEntriesFrom(input, FileOptions._repeated_uninterpretedOption_codec);
						continue;
					}
				}
				if (!ExtensionSet.TryMergeFieldFrom<FileOptions>(ref this._extensions, input))
				{
					this._unknownFields = UnknownFieldSet.MergeFieldFrom(this._unknownFields, input);
				}
			}
		}

		// Token: 0x060005F6 RID: 1526 RVA: 0x000171BF File Offset: 0x000153BF
		public TValue GetExtension<TValue>(Extension<FileOptions, TValue> extension)
		{
			return ExtensionSet.Get<FileOptions, TValue>(ref this._extensions, extension);
		}

		// Token: 0x060005F7 RID: 1527 RVA: 0x000171CD File Offset: 0x000153CD
		public RepeatedField<TValue> GetExtension<TValue>(RepeatedExtension<FileOptions, TValue> extension)
		{
			return ExtensionSet.Get<FileOptions, TValue>(ref this._extensions, extension);
		}

		// Token: 0x060005F8 RID: 1528 RVA: 0x000171DB File Offset: 0x000153DB
		public RepeatedField<TValue> GetOrInitializeExtension<TValue>(RepeatedExtension<FileOptions, TValue> extension)
		{
			return ExtensionSet.GetOrInitialize<FileOptions, TValue>(ref this._extensions, extension);
		}

		// Token: 0x060005F9 RID: 1529 RVA: 0x000171E9 File Offset: 0x000153E9
		public void SetExtension<TValue>(Extension<FileOptions, TValue> extension, TValue value)
		{
			ExtensionSet.Set<FileOptions, TValue>(ref this._extensions, extension, value);
		}

		// Token: 0x060005FA RID: 1530 RVA: 0x000171F8 File Offset: 0x000153F8
		public bool HasExtension<TValue>(Extension<FileOptions, TValue> extension)
		{
			return ExtensionSet.Has<FileOptions, TValue>(ref this._extensions, extension);
		}

		// Token: 0x060005FB RID: 1531 RVA: 0x00017206 File Offset: 0x00015406
		public void ClearExtension<TValue>(Extension<FileOptions, TValue> extension)
		{
			ExtensionSet.Clear<FileOptions, TValue>(ref this._extensions, extension);
		}

		// Token: 0x060005FC RID: 1532 RVA: 0x00017214 File Offset: 0x00015414
		public void ClearExtension<TValue>(RepeatedExtension<FileOptions, TValue> extension)
		{
			ExtensionSet.Clear<FileOptions, TValue>(ref this._extensions, extension);
		}

		// Token: 0x04000204 RID: 516
		private static readonly MessageParser<FileOptions> _parser = new MessageParser<FileOptions>(() => new FileOptions());

		// Token: 0x04000205 RID: 517
		private UnknownFieldSet _unknownFields;

		// Token: 0x04000206 RID: 518
		internal ExtensionSet<FileOptions> _extensions;

		// Token: 0x04000207 RID: 519
		private int _hasBits0;

		// Token: 0x04000208 RID: 520
		public const int JavaPackageFieldNumber = 1;

		// Token: 0x04000209 RID: 521
		private static readonly string JavaPackageDefaultValue = "";

		// Token: 0x0400020A RID: 522
		private string javaPackage_;

		// Token: 0x0400020B RID: 523
		public const int JavaOuterClassnameFieldNumber = 8;

		// Token: 0x0400020C RID: 524
		private static readonly string JavaOuterClassnameDefaultValue = "";

		// Token: 0x0400020D RID: 525
		private string javaOuterClassname_;

		// Token: 0x0400020E RID: 526
		public const int JavaMultipleFilesFieldNumber = 10;

		// Token: 0x0400020F RID: 527
		private static readonly bool JavaMultipleFilesDefaultValue = false;

		// Token: 0x04000210 RID: 528
		private bool javaMultipleFiles_;

		// Token: 0x04000211 RID: 529
		public const int JavaGenerateEqualsAndHashFieldNumber = 20;

		// Token: 0x04000212 RID: 530
		private static readonly bool JavaGenerateEqualsAndHashDefaultValue = false;

		// Token: 0x04000213 RID: 531
		private bool javaGenerateEqualsAndHash_;

		// Token: 0x04000214 RID: 532
		public const int JavaStringCheckUtf8FieldNumber = 27;

		// Token: 0x04000215 RID: 533
		private static readonly bool JavaStringCheckUtf8DefaultValue = false;

		// Token: 0x04000216 RID: 534
		private bool javaStringCheckUtf8_;

		// Token: 0x04000217 RID: 535
		public const int OptimizeForFieldNumber = 9;

		// Token: 0x04000218 RID: 536
		private static readonly FileOptions.Types.OptimizeMode OptimizeForDefaultValue = FileOptions.Types.OptimizeMode.Speed;

		// Token: 0x04000219 RID: 537
		private FileOptions.Types.OptimizeMode optimizeFor_;

		// Token: 0x0400021A RID: 538
		public const int GoPackageFieldNumber = 11;

		// Token: 0x0400021B RID: 539
		private static readonly string GoPackageDefaultValue = "";

		// Token: 0x0400021C RID: 540
		private string goPackage_;

		// Token: 0x0400021D RID: 541
		public const int CcGenericServicesFieldNumber = 16;

		// Token: 0x0400021E RID: 542
		private static readonly bool CcGenericServicesDefaultValue = false;

		// Token: 0x0400021F RID: 543
		private bool ccGenericServices_;

		// Token: 0x04000220 RID: 544
		public const int JavaGenericServicesFieldNumber = 17;

		// Token: 0x04000221 RID: 545
		private static readonly bool JavaGenericServicesDefaultValue = false;

		// Token: 0x04000222 RID: 546
		private bool javaGenericServices_;

		// Token: 0x04000223 RID: 547
		public const int PyGenericServicesFieldNumber = 18;

		// Token: 0x04000224 RID: 548
		private static readonly bool PyGenericServicesDefaultValue = false;

		// Token: 0x04000225 RID: 549
		private bool pyGenericServices_;

		// Token: 0x04000226 RID: 550
		public const int PhpGenericServicesFieldNumber = 42;

		// Token: 0x04000227 RID: 551
		private static readonly bool PhpGenericServicesDefaultValue = false;

		// Token: 0x04000228 RID: 552
		private bool phpGenericServices_;

		// Token: 0x04000229 RID: 553
		public const int DeprecatedFieldNumber = 23;

		// Token: 0x0400022A RID: 554
		private static readonly bool DeprecatedDefaultValue = false;

		// Token: 0x0400022B RID: 555
		private bool deprecated_;

		// Token: 0x0400022C RID: 556
		public const int CcEnableArenasFieldNumber = 31;

		// Token: 0x0400022D RID: 557
		private static readonly bool CcEnableArenasDefaultValue = true;

		// Token: 0x0400022E RID: 558
		private bool ccEnableArenas_;

		// Token: 0x0400022F RID: 559
		public const int ObjcClassPrefixFieldNumber = 36;

		// Token: 0x04000230 RID: 560
		private static readonly string ObjcClassPrefixDefaultValue = "";

		// Token: 0x04000231 RID: 561
		private string objcClassPrefix_;

		// Token: 0x04000232 RID: 562
		public const int CsharpNamespaceFieldNumber = 37;

		// Token: 0x04000233 RID: 563
		private static readonly string CsharpNamespaceDefaultValue = "";

		// Token: 0x04000234 RID: 564
		private string csharpNamespace_;

		// Token: 0x04000235 RID: 565
		public const int SwiftPrefixFieldNumber = 39;

		// Token: 0x04000236 RID: 566
		private static readonly string SwiftPrefixDefaultValue = "";

		// Token: 0x04000237 RID: 567
		private string swiftPrefix_;

		// Token: 0x04000238 RID: 568
		public const int PhpClassPrefixFieldNumber = 40;

		// Token: 0x04000239 RID: 569
		private static readonly string PhpClassPrefixDefaultValue = "";

		// Token: 0x0400023A RID: 570
		private string phpClassPrefix_;

		// Token: 0x0400023B RID: 571
		public const int PhpNamespaceFieldNumber = 41;

		// Token: 0x0400023C RID: 572
		private static readonly string PhpNamespaceDefaultValue = "";

		// Token: 0x0400023D RID: 573
		private string phpNamespace_;

		// Token: 0x0400023E RID: 574
		public const int PhpMetadataNamespaceFieldNumber = 44;

		// Token: 0x0400023F RID: 575
		private static readonly string PhpMetadataNamespaceDefaultValue = "";

		// Token: 0x04000240 RID: 576
		private string phpMetadataNamespace_;

		// Token: 0x04000241 RID: 577
		public const int RubyPackageFieldNumber = 45;

		// Token: 0x04000242 RID: 578
		private static readonly string RubyPackageDefaultValue = "";

		// Token: 0x04000243 RID: 579
		private string rubyPackage_;

		// Token: 0x04000244 RID: 580
		public const int UninterpretedOptionFieldNumber = 999;

		// Token: 0x04000245 RID: 581
		private static readonly FieldCodec<UninterpretedOption> _repeated_uninterpretedOption_codec = FieldCodec.ForMessage<UninterpretedOption>(7994U, Google.Protobuf.Reflection.UninterpretedOption.Parser);

		// Token: 0x04000246 RID: 582
		private readonly RepeatedField<UninterpretedOption> uninterpretedOption_ = new RepeatedField<UninterpretedOption>();

		// Token: 0x020000D6 RID: 214
		[DebuggerNonUserCode]
		public static class Types
		{
			// Token: 0x02000118 RID: 280
			public enum OptimizeMode
			{
				// Token: 0x040004A4 RID: 1188
				[OriginalName("SPEED")]
				Speed = 1,
				// Token: 0x040004A5 RID: 1189
				[OriginalName("CODE_SIZE")]
				CodeSize,
				// Token: 0x040004A6 RID: 1190
				[OriginalName("LITE_RUNTIME")]
				LiteRuntime
			}
		}
	}
}
