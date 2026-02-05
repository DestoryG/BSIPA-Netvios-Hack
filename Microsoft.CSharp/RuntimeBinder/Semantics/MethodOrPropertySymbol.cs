using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.CSharp.RuntimeBinder.Syntax;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x02000066 RID: 102
	internal abstract class MethodOrPropertySymbol : ParentSymbol
	{
		// Token: 0x17000083 RID: 131
		// (get) Token: 0x0600037E RID: 894 RVA: 0x00016657 File Offset: 0x00014857
		// (set) Token: 0x0600037F RID: 895 RVA: 0x0001665F File Offset: 0x0001485F
		public List<Name> ParameterNames { get; private set; }

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000380 RID: 896 RVA: 0x00016668 File Offset: 0x00014868
		// (set) Token: 0x06000381 RID: 897 RVA: 0x00016670 File Offset: 0x00014870
		public TypeArray Params
		{
			get
			{
				return this._Params;
			}
			set
			{
				this._Params = value;
				this._optionalParameterIndex = new bool[this._Params.Count];
				this._defaultParameterIndex = new bool[this._Params.Count];
				this._defaultParameters = new ConstVal[this._Params.Count];
				this._defaultParameterConstValTypes = new CType[this._Params.Count];
				this._marshalAsIndex = new bool[this._Params.Count];
				this._marshalAsBuffer = new UnmanagedType[this._Params.Count];
			}
		}

		// Token: 0x06000382 RID: 898 RVA: 0x00016708 File Offset: 0x00014908
		public MethodOrPropertySymbol()
		{
			this.ParameterNames = new List<Name>();
		}

		// Token: 0x06000383 RID: 899 RVA: 0x0001671B File Offset: 0x0001491B
		public bool IsParameterOptional(int index)
		{
			return this._optionalParameterIndex != null && this._optionalParameterIndex[index];
		}

		// Token: 0x06000384 RID: 900 RVA: 0x0001672F File Offset: 0x0001492F
		public void SetOptionalParameter(int index)
		{
			this._optionalParameterIndex[index] = true;
		}

		// Token: 0x06000385 RID: 901 RVA: 0x0001673C File Offset: 0x0001493C
		public bool HasOptionalParameters()
		{
			if (this._optionalParameterIndex == null)
			{
				return false;
			}
			bool[] optionalParameterIndex = this._optionalParameterIndex;
			for (int i = 0; i < optionalParameterIndex.Length; i++)
			{
				if (optionalParameterIndex[i])
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000386 RID: 902 RVA: 0x00016770 File Offset: 0x00014970
		public bool HasDefaultParameterValue(int index)
		{
			return this._defaultParameterIndex[index];
		}

		// Token: 0x06000387 RID: 903 RVA: 0x0001677A File Offset: 0x0001497A
		public void SetDefaultParameterValue(int index, CType type, ConstVal cv)
		{
			this._defaultParameterIndex[index] = true;
			this._defaultParameters[index] = cv;
			this._defaultParameterConstValTypes[index] = type;
		}

		// Token: 0x06000388 RID: 904 RVA: 0x0001679B File Offset: 0x0001499B
		public ConstVal GetDefaultParameterValue(int index)
		{
			return this._defaultParameters[index];
		}

		// Token: 0x06000389 RID: 905 RVA: 0x000167A9 File Offset: 0x000149A9
		public CType GetDefaultParameterValueConstValType(int index)
		{
			return this._defaultParameterConstValTypes[index];
		}

		// Token: 0x0600038A RID: 906 RVA: 0x000167B3 File Offset: 0x000149B3
		private bool IsMarshalAsParameter(int index)
		{
			return this._marshalAsIndex[index];
		}

		// Token: 0x0600038B RID: 907 RVA: 0x000167BD File Offset: 0x000149BD
		public void SetMarshalAsParameter(int index, UnmanagedType umt)
		{
			this._marshalAsIndex[index] = true;
			this._marshalAsBuffer[index] = umt;
		}

		// Token: 0x0600038C RID: 908 RVA: 0x000167D1 File Offset: 0x000149D1
		private UnmanagedType GetMarshalAsParameterValue(int index)
		{
			return this._marshalAsBuffer[index];
		}

		// Token: 0x0600038D RID: 909 RVA: 0x000167DC File Offset: 0x000149DC
		public bool MarshalAsObject(int index)
		{
			UnmanagedType unmanagedType = (UnmanagedType)0;
			if (this.IsMarshalAsParameter(index))
			{
				unmanagedType = this.GetMarshalAsParameterValue(index);
			}
			return unmanagedType == UnmanagedType.Interface || unmanagedType == UnmanagedType.IUnknown;
		}

		// Token: 0x0600038E RID: 910 RVA: 0x00016808 File Offset: 0x00014A08
		public AggregateSymbol getClass()
		{
			return this.parent as AggregateSymbol;
		}

		// Token: 0x0600038F RID: 911 RVA: 0x00016815 File Offset: 0x00014A15
		public bool IsExpImpl()
		{
			return this.name == null;
		}

		// Token: 0x040004AB RID: 1195
		public uint modOptCount;

		// Token: 0x040004AC RID: 1196
		public new bool isStatic;

		// Token: 0x040004AD RID: 1197
		public bool isOverride;

		// Token: 0x040004AE RID: 1198
		public bool isOperator;

		// Token: 0x040004AF RID: 1199
		public bool isParamArray;

		// Token: 0x040004B0 RID: 1200
		public bool isHideByName;

		// Token: 0x040004B2 RID: 1202
		private bool[] _optionalParameterIndex;

		// Token: 0x040004B3 RID: 1203
		private bool[] _defaultParameterIndex;

		// Token: 0x040004B4 RID: 1204
		private ConstVal[] _defaultParameters;

		// Token: 0x040004B5 RID: 1205
		private CType[] _defaultParameterConstValTypes;

		// Token: 0x040004B6 RID: 1206
		private bool[] _marshalAsIndex;

		// Token: 0x040004B7 RID: 1207
		private UnmanagedType[] _marshalAsBuffer;

		// Token: 0x040004B8 RID: 1208
		public SymWithType swtSlot;

		// Token: 0x040004B9 RID: 1209
		public ErrorType errExpImpl;

		// Token: 0x040004BA RID: 1210
		public CType RetType;

		// Token: 0x040004BB RID: 1211
		private TypeArray _Params;
	}
}
