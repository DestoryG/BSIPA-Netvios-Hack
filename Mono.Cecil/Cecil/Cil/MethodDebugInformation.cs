using System;
using System.Collections.Generic;
using Mono.Collections.Generic;

namespace Mono.Cecil.Cil
{
	// Token: 0x0200012A RID: 298
	public sealed class MethodDebugInformation : DebugInformation
	{
		// Token: 0x17000323 RID: 803
		// (get) Token: 0x06000B3A RID: 2874 RVA: 0x00024231 File Offset: 0x00022431
		public MethodDefinition Method
		{
			get
			{
				return this.method;
			}
		}

		// Token: 0x17000324 RID: 804
		// (get) Token: 0x06000B3B RID: 2875 RVA: 0x00024239 File Offset: 0x00022439
		public bool HasSequencePoints
		{
			get
			{
				return !this.sequence_points.IsNullOrEmpty<SequencePoint>();
			}
		}

		// Token: 0x17000325 RID: 805
		// (get) Token: 0x06000B3C RID: 2876 RVA: 0x0002424C File Offset: 0x0002244C
		public Collection<SequencePoint> SequencePoints
		{
			get
			{
				Collection<SequencePoint> collection;
				if ((collection = this.sequence_points) == null)
				{
					collection = (this.sequence_points = new Collection<SequencePoint>());
				}
				return collection;
			}
		}

		// Token: 0x17000326 RID: 806
		// (get) Token: 0x06000B3D RID: 2877 RVA: 0x00024271 File Offset: 0x00022471
		// (set) Token: 0x06000B3E RID: 2878 RVA: 0x00024279 File Offset: 0x00022479
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

		// Token: 0x17000327 RID: 807
		// (get) Token: 0x06000B3F RID: 2879 RVA: 0x00024282 File Offset: 0x00022482
		// (set) Token: 0x06000B40 RID: 2880 RVA: 0x0002428A File Offset: 0x0002248A
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

		// Token: 0x06000B41 RID: 2881 RVA: 0x00024294 File Offset: 0x00022494
		internal MethodDebugInformation(MethodDefinition method)
		{
			if (method == null)
			{
				throw new ArgumentNullException("method");
			}
			this.method = method;
			this.token = new MetadataToken(TokenType.MethodDebugInformation, method.MetadataToken.RID);
		}

		// Token: 0x06000B42 RID: 2882 RVA: 0x000242DC File Offset: 0x000224DC
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

		// Token: 0x06000B43 RID: 2883 RVA: 0x00024330 File Offset: 0x00022530
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

		// Token: 0x06000B44 RID: 2884 RVA: 0x0002440B File Offset: 0x0002260B
		public IEnumerable<ScopeDebugInformation> GetScopes()
		{
			if (this.scope == null)
			{
				return Empty<ScopeDebugInformation>.Array;
			}
			return MethodDebugInformation.GetScopes(new ScopeDebugInformation[] { this.scope });
		}

		// Token: 0x06000B45 RID: 2885 RVA: 0x0002442F File Offset: 0x0002262F
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

		// Token: 0x06000B46 RID: 2886 RVA: 0x00024440 File Offset: 0x00022640
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

		// Token: 0x040006DC RID: 1756
		internal MethodDefinition method;

		// Token: 0x040006DD RID: 1757
		internal Collection<SequencePoint> sequence_points;

		// Token: 0x040006DE RID: 1758
		internal ScopeDebugInformation scope;

		// Token: 0x040006DF RID: 1759
		internal MethodDefinition kickoff_method;

		// Token: 0x040006E0 RID: 1760
		internal int code_size;

		// Token: 0x040006E1 RID: 1761
		internal MetadataToken local_var_token;
	}
}
