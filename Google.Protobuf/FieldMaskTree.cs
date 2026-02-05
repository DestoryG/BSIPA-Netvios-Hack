using System;
using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.Reflection;
using Google.Protobuf.WellKnownTypes;

namespace Google.Protobuf
{
	// Token: 0x02000011 RID: 17
	internal sealed class FieldMaskTree
	{
		// Token: 0x06000132 RID: 306 RVA: 0x00005CCA File Offset: 0x00003ECA
		public FieldMaskTree()
		{
		}

		// Token: 0x06000133 RID: 307 RVA: 0x00005CDD File Offset: 0x00003EDD
		public FieldMaskTree(FieldMask mask)
		{
			this.MergeFromFieldMask(mask);
		}

		// Token: 0x06000134 RID: 308 RVA: 0x00005CF8 File Offset: 0x00003EF8
		public override string ToString()
		{
			return this.ToFieldMask().ToString();
		}

		// Token: 0x06000135 RID: 309 RVA: 0x00005D08 File Offset: 0x00003F08
		public FieldMaskTree AddFieldPath(string path)
		{
			string[] array = path.Split(new char[] { '.' });
			if (array.Length == 0)
			{
				return this;
			}
			FieldMaskTree.Node node = this.root;
			bool flag = false;
			foreach (string text in array)
			{
				if (!flag && node != this.root && node.Children.Count == 0)
				{
					return this;
				}
				FieldMaskTree.Node node2;
				if (!node.Children.TryGetValue(text, out node2))
				{
					flag = true;
					node2 = new FieldMaskTree.Node();
					node.Children.Add(text, node2);
				}
				node = node2;
			}
			node.Children.Clear();
			return this;
		}

		// Token: 0x06000136 RID: 310 RVA: 0x00005DA4 File Offset: 0x00003FA4
		public FieldMaskTree MergeFromFieldMask(FieldMask mask)
		{
			foreach (string text in mask.Paths)
			{
				this.AddFieldPath(text);
			}
			return this;
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00005DF4 File Offset: 0x00003FF4
		public FieldMask ToFieldMask()
		{
			FieldMask fieldMask = new FieldMask();
			if (this.root.Children.Count != 0)
			{
				List<string> list = new List<string>();
				this.GetFieldPaths(this.root, "", list);
				fieldMask.Paths.AddRange(list);
			}
			return fieldMask;
		}

		// Token: 0x06000138 RID: 312 RVA: 0x00005E40 File Offset: 0x00004040
		private void GetFieldPaths(FieldMaskTree.Node node, string path, List<string> paths)
		{
			if (node.Children.Count == 0)
			{
				paths.Add(path);
				return;
			}
			foreach (KeyValuePair<string, FieldMaskTree.Node> keyValuePair in node.Children)
			{
				string text = ((path.Length == 0) ? keyValuePair.Key : (path + "." + keyValuePair.Key));
				this.GetFieldPaths(keyValuePair.Value, text, paths);
			}
		}

		// Token: 0x06000139 RID: 313 RVA: 0x00005ED4 File Offset: 0x000040D4
		public void IntersectFieldPath(string path, FieldMaskTree output)
		{
			if (this.root.Children.Count == 0)
			{
				return;
			}
			string[] array = path.Split(new char[] { '.' });
			if (array.Length == 0)
			{
				return;
			}
			FieldMaskTree.Node node = this.root;
			foreach (string text in array)
			{
				if (node != this.root && node.Children.Count == 0)
				{
					output.AddFieldPath(path);
					return;
				}
				if (!node.Children.TryGetValue(text, out node))
				{
					return;
				}
			}
			List<string> list = new List<string>();
			this.GetFieldPaths(node, path, list);
			foreach (string text2 in list)
			{
				output.AddFieldPath(text2);
			}
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00005FB4 File Offset: 0x000041B4
		public void Merge(IMessage source, IMessage destination, FieldMask.MergeOptions options)
		{
			if (source.Descriptor != destination.Descriptor)
			{
				throw new InvalidProtocolBufferException("Cannot merge messages of different types.");
			}
			if (this.root.Children.Count == 0)
			{
				return;
			}
			this.Merge(this.root, "", source, destination, options);
		}

		// Token: 0x0600013B RID: 315 RVA: 0x00006004 File Offset: 0x00004204
		private void Merge(FieldMaskTree.Node node, string path, IMessage source, IMessage destination, FieldMask.MergeOptions options)
		{
			if (source.Descriptor != destination.Descriptor)
			{
				throw new InvalidProtocolBufferException(string.Format("source ({0}) and destination ({1}) descriptor must be equal", source.Descriptor, destination.Descriptor));
			}
			MessageDescriptor descriptor = source.Descriptor;
			foreach (KeyValuePair<string, FieldMaskTree.Node> keyValuePair in node.Children)
			{
				FieldDescriptor fieldDescriptor = descriptor.FindFieldByName(keyValuePair.Key);
				if (fieldDescriptor != null)
				{
					if (keyValuePair.Value.Children.Count != 0)
					{
						if (!fieldDescriptor.IsRepeated && fieldDescriptor.FieldType == FieldType.Message)
						{
							object value = fieldDescriptor.Accessor.GetValue(source);
							object obj = fieldDescriptor.Accessor.GetValue(destination);
							if (value != null || obj != null)
							{
								if (obj == null)
								{
									obj = fieldDescriptor.MessageType.Parser.CreateTemplate();
									fieldDescriptor.Accessor.SetValue(destination, obj);
								}
								string text = ((path.Length == 0) ? keyValuePair.Key : (path + "." + keyValuePair.Key));
								this.Merge(keyValuePair.Value, text, (IMessage)value, (IMessage)obj, options);
							}
						}
					}
					else
					{
						if (fieldDescriptor.IsRepeated)
						{
							if (options.ReplaceRepeatedFields)
							{
								fieldDescriptor.Accessor.Clear(destination);
							}
							IEnumerable enumerable = (IList)fieldDescriptor.Accessor.GetValue(source);
							IList list = (IList)fieldDescriptor.Accessor.GetValue(destination);
							using (IEnumerator enumerator2 = enumerable.GetEnumerator())
							{
								while (enumerator2.MoveNext())
								{
									object obj2 = enumerator2.Current;
									list.Add(obj2);
								}
								continue;
							}
						}
						object value2 = fieldDescriptor.Accessor.GetValue(source);
						if (fieldDescriptor.FieldType == FieldType.Message)
						{
							if (options.ReplaceMessageFields)
							{
								if (value2 == null)
								{
									fieldDescriptor.Accessor.Clear(destination);
								}
								else
								{
									fieldDescriptor.Accessor.SetValue(destination, value2);
								}
							}
							else if (value2 != null)
							{
								ByteString byteString = ((IMessage)value2).ToByteString();
								IMessage message = (IMessage)fieldDescriptor.Accessor.GetValue(destination);
								if (message != null)
								{
									message.MergeFrom(byteString);
								}
								else
								{
									fieldDescriptor.Accessor.SetValue(destination, fieldDescriptor.MessageType.Parser.ParseFrom(byteString));
								}
							}
						}
						else if (value2 != null || !options.ReplacePrimitiveFields)
						{
							fieldDescriptor.Accessor.SetValue(destination, value2);
						}
						else
						{
							fieldDescriptor.Accessor.Clear(destination);
						}
					}
				}
			}
		}

		// Token: 0x0400003A RID: 58
		private const char FIELD_PATH_SEPARATOR = '.';

		// Token: 0x0400003B RID: 59
		private readonly FieldMaskTree.Node root = new FieldMaskTree.Node();

		// Token: 0x020000A0 RID: 160
		internal sealed class Node
		{
			// Token: 0x17000260 RID: 608
			// (get) Token: 0x0600091C RID: 2332 RVA: 0x0001F1CC File Offset: 0x0001D3CC
			public Dictionary<string, FieldMaskTree.Node> Children { get; } = new Dictionary<string, FieldMaskTree.Node>();
		}
	}
}
