using System;
using System.Collections.Generic;
using System.Threading;
using Mono.Collections.Generic;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001EE RID: 494
	internal sealed class MethodDebugInformation : DebugInformation
	{
		// Token: 0x17000343 RID: 835
		// (get) Token: 0x06000F21 RID: 3873 RVA: 0x000333E1 File Offset: 0x000315E1
		public MethodDefinition Method
		{
			get
			{
				return this.method;
			}
		}

		// Token: 0x17000344 RID: 836
		// (get) Token: 0x06000F22 RID: 3874 RVA: 0x000333E9 File Offset: 0x000315E9
		public bool HasSequencePoints
		{
			get
			{
				return !this.sequence_points.IsNullOrEmpty<SequencePoint>();
			}
		}

		// Token: 0x17000345 RID: 837
		// (get) Token: 0x06000F23 RID: 3875 RVA: 0x000333F9 File Offset: 0x000315F9
		public Collection<SequencePoint> SequencePoints
		{
			get
			{
				if (this.sequence_points == null)
				{
					Interlocked.CompareExchange<Collection<SequencePoint>>(ref this.sequence_points, new Collection<SequencePoint>(), null);
				}
				return this.sequence_points;
			}
		}

		// Token: 0x17000346 RID: 838
		// (get) Token: 0x06000F24 RID: 3876 RVA: 0x0003341B File Offset: 0x0003161B
		// (set) Token: 0x06000F25 RID: 3877 RVA: 0x00033423 File Offset: 0x00031623
		public ScopeDebugInformation Scope
		{
			get
			{
				return this.scope;
			}
			set
			{
				this.scope = value;
			}
		}

		// Token: 0x17000347 RID: 839
		// (get) Token: 0x06000F26 RID: 3878 RVA: 0x0003342C File Offset: 0x0003162C
		// (set) Token: 0x06000F27 RID: 3879 RVA: 0x00033434 File Offset: 0x00031634
		public MethodDefinition StateMachineKickOffMethod
		{
			get
			{
				return this.kickoff_method;
			}
			set
			{
				this.kickoff_method = value;
			}
		}

		// Token: 0x06000F28 RID: 3880 RVA: 0x00033440 File Offset: 0x00031640
		internal MethodDebugInformation(MethodDefinition method)
		{
			if (method == null)
			{
				throw new ArgumentNullException("method");
			}
			this.method = method;
			this.token = new MetadataToken(TokenType.MethodDebugInformation, method.MetadataToken.RID);
		}

		// Token: 0x06000F29 RID: 3881 RVA: 0x00033488 File Offset: 0x00031688
		public SequencePoint GetSequencePoint(Instruction instruction)
		{
			if (!this.HasSequencePoints)
			{
				return null;
			}
			for (int i = 0; i < this.sequence_points.Count; i++)
			{
				if (this.sequence_points[i].Offset == instruction.Offset)
				{
					return this.sequence_points[i];
				}
			}
			return null;
		}

		// Token: 0x06000F2A RID: 3882 RVA: 0x000334DC File Offset: 0x000316DC
		public IDictionary<Instruction, SequencePoint> GetSequencePointMapping()
		{
			Dictionary<Instruction, SequencePoint> dictionary = new Dictionary<Instruction, SequencePoint>();
			if (!this.HasSequencePoints || !this.method.HasBody)
			{
				return dictionary;
			}
			Dictionary<int, SequencePoint> dictionary2 = new Dictionary<int, SequencePoint>(this.sequence_points.Count);
			for (int i = 0; i < this.sequence_points.Count; i++)
			{
				if (!dictionary2.ContainsKey(this.sequence_points[i].Offset))
				{
					dictionary2.Add(this.sequence_points[i].Offset, this.sequence_points[i]);
				}
			}
			Collection<Instruction> instructions = this.method.Body.Instructions;
			for (int j = 0; j < instructions.Count; j++)
			{
				SequencePoint sequencePoint;
				if (dictionary2.TryGetValue(instructions[j].Offset, out sequencePoint))
				{
					dictionary.Add(instructions[j], sequencePoint);
				}
			}
			return dictionary;
		}

		// Token: 0x06000F2B RID: 3883 RVA: 0x000335B7 File Offset: 0x000317B7
		public IEnumerable<ScopeDebugInformation> GetScopes()
		{
			if (this.scope == null)
			{
				return Empty<ScopeDebugInformation>.Array;
			}
			return MethodDebugInformation.GetScopes(new ScopeDebugInformation[] { this.scope });
		}

		// Token: 0x06000F2C RID: 3884 RVA: 0x000335DB File Offset: 0x000317DB
		private static IEnumerable<ScopeDebugInformation> GetScopes(IList<ScopeDebugInformation> scopes)
		{
			int num;
			for (int i = 0; i < scopes.Count; i = num + 1)
			{
				ScopeDebugInformation scope = scopes[i];
				yield return scope;
				if (scope.HasScopes)
				{
					foreach (ScopeDebugInformation scopeDebugInformation in MethodDebugInformation.GetScopes(scope.Scopes))
					{
						yield return scopeDebugInformation;
					}
					IEnumerator<ScopeDebugInformation> enumerator = null;
					scope = null;
				}
				num = i;
			}
			yield break;
			yield break;
		}

		// Token: 0x06000F2D RID: 3885 RVA: 0x000335EC File Offset: 0x000317EC
		public bool TryGetName(VariableDefinition variable, out string name)
		{
			name = null;
			bool flag = false;
			string text = "";
			using (IEnumerator<ScopeDebugInformation> enumerator = this.GetScopes().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					string text2;
					if (enumerator.Current.TryGetName(variable, out text2))
					{
						if (!flag)
						{
							flag = true;
							text = text2;
						}
						else if (text != text2)
						{
							return false;
						}
					}
				}
			}
			name = text;
			return flag;
		}

		// Token: 0x0400093B RID: 2363
		internal MethodDefinition method;

		// Token: 0x0400093C RID: 2364
		internal Collection<SequencePoint> sequence_points;

		// Token: 0x0400093D RID: 2365
		internal ScopeDebugInformation scope;

		// Token: 0x0400093E RID: 2366
		internal MethodDefinition kickoff_method;

		// Token: 0x0400093F RID: 2367
		internal int code_size;

		// Token: 0x04000940 RID: 2368
		internal MetadataToken local_var_token;
	}
}
