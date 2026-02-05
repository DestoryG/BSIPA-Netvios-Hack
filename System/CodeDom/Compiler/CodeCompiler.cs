using System;
using System.IO;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;
using System.Security.Principal;
using System.Text;

namespace System.CodeDom.Compiler
{
	// Token: 0x0200066C RID: 1644
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public abstract class CodeCompiler : CodeGenerator, ICodeCompiler
	{
		// Token: 0x06003B7E RID: 15230 RVA: 0x000F59A4 File Offset: 0x000F3BA4
		CompilerResults ICodeCompiler.CompileAssemblyFromDom(CompilerParameters options, CodeCompileUnit e)
		{
			if (options == null)
			{
				throw new ArgumentNullException("options");
			}
			CompilerResults compilerResults;
			try
			{
				compilerResults = this.FromDom(options, e);
			}
			finally
			{
				options.TempFiles.SafeDelete();
			}
			return compilerResults;
		}

		// Token: 0x06003B7F RID: 15231 RVA: 0x000F59E8 File Offset: 0x000F3BE8
		CompilerResults ICodeCompiler.CompileAssemblyFromFile(CompilerParameters options, string fileName)
		{
			if (options == null)
			{
				throw new ArgumentNullException("options");
			}
			CompilerResults compilerResults;
			try
			{
				compilerResults = this.FromFile(options, fileName);
			}
			finally
			{
				options.TempFiles.SafeDelete();
			}
			return compilerResults;
		}

		// Token: 0x06003B80 RID: 15232 RVA: 0x000F5A2C File Offset: 0x000F3C2C
		CompilerResults ICodeCompiler.CompileAssemblyFromSource(CompilerParameters options, string source)
		{
			if (options == null)
			{
				throw new ArgumentNullException("options");
			}
			CompilerResults compilerResults;
			try
			{
				compilerResults = this.FromSource(options, source);
			}
			finally
			{
				options.TempFiles.SafeDelete();
			}
			return compilerResults;
		}

		// Token: 0x06003B81 RID: 15233 RVA: 0x000F5A70 File Offset: 0x000F3C70
		CompilerResults ICodeCompiler.CompileAssemblyFromSourceBatch(CompilerParameters options, string[] sources)
		{
			if (options == null)
			{
				throw new ArgumentNullException("options");
			}
			CompilerResults compilerResults;
			try
			{
				compilerResults = this.FromSourceBatch(options, sources);
			}
			finally
			{
				options.TempFiles.SafeDelete();
			}
			return compilerResults;
		}

		// Token: 0x06003B82 RID: 15234 RVA: 0x000F5AB4 File Offset: 0x000F3CB4
		CompilerResults ICodeCompiler.CompileAssemblyFromFileBatch(CompilerParameters options, string[] fileNames)
		{
			if (options == null)
			{
				throw new ArgumentNullException("options");
			}
			if (fileNames == null)
			{
				throw new ArgumentNullException("fileNames");
			}
			CompilerResults compilerResults;
			try
			{
				foreach (string text in fileNames)
				{
					using (File.OpenRead(text))
					{
					}
				}
				compilerResults = this.FromFileBatch(options, fileNames);
			}
			finally
			{
				options.TempFiles.SafeDelete();
			}
			return compilerResults;
		}

		// Token: 0x06003B83 RID: 15235 RVA: 0x000F5B3C File Offset: 0x000F3D3C
		CompilerResults ICodeCompiler.CompileAssemblyFromDomBatch(CompilerParameters options, CodeCompileUnit[] ea)
		{
			if (options == null)
			{
				throw new ArgumentNullException("options");
			}
			CompilerResults compilerResults;
			try
			{
				compilerResults = this.FromDomBatch(options, ea);
			}
			finally
			{
				options.TempFiles.SafeDelete();
			}
			return compilerResults;
		}

		// Token: 0x17000E53 RID: 3667
		// (get) Token: 0x06003B84 RID: 15236
		protected abstract string FileExtension { get; }

		// Token: 0x17000E54 RID: 3668
		// (get) Token: 0x06003B85 RID: 15237
		protected abstract string CompilerName { get; }

		// Token: 0x06003B86 RID: 15238 RVA: 0x000F5B80 File Offset: 0x000F3D80
		internal void Compile(CompilerParameters options, string compilerDirectory, string compilerExe, string arguments, ref string outputFile, ref int nativeReturnValue, string trueArgs)
		{
			string text = null;
			outputFile = options.TempFiles.AddExtension("out");
			string text2 = Path.Combine(compilerDirectory, compilerExe);
			if (File.Exists(text2))
			{
				string text3 = null;
				if (trueArgs != null)
				{
					text3 = "\"" + text2 + "\" " + trueArgs;
				}
				nativeReturnValue = Executor.ExecWaitWithCapture(options.SafeUserToken, "\"" + text2 + "\" " + arguments, Environment.CurrentDirectory, options.TempFiles, ref outputFile, ref text, text3);
				return;
			}
			throw new InvalidOperationException(SR.GetString("CompilerNotFound", new object[] { text2 }));
		}

		// Token: 0x06003B87 RID: 15239 RVA: 0x000F5C18 File Offset: 0x000F3E18
		protected virtual CompilerResults FromDom(CompilerParameters options, CodeCompileUnit e)
		{
			if (options == null)
			{
				throw new ArgumentNullException("options");
			}
			new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();
			return this.FromDomBatch(options, new CodeCompileUnit[] { e });
		}

		// Token: 0x06003B88 RID: 15240 RVA: 0x000F5C54 File Offset: 0x000F3E54
		protected virtual CompilerResults FromFile(CompilerParameters options, string fileName)
		{
			if (options == null)
			{
				throw new ArgumentNullException("options");
			}
			if (fileName == null)
			{
				throw new ArgumentNullException("fileName");
			}
			new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();
			using (File.OpenRead(fileName))
			{
			}
			return this.FromFileBatch(options, new string[] { fileName });
		}

		// Token: 0x06003B89 RID: 15241 RVA: 0x000F5CC0 File Offset: 0x000F3EC0
		protected virtual CompilerResults FromSource(CompilerParameters options, string source)
		{
			if (options == null)
			{
				throw new ArgumentNullException("options");
			}
			new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();
			return this.FromSourceBatch(options, new string[] { source });
		}

		// Token: 0x06003B8A RID: 15242 RVA: 0x000F5CFC File Offset: 0x000F3EFC
		protected virtual CompilerResults FromDomBatch(CompilerParameters options, CodeCompileUnit[] ea)
		{
			if (options == null)
			{
				throw new ArgumentNullException("options");
			}
			if (ea == null)
			{
				throw new ArgumentNullException("ea");
			}
			new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();
			string[] array = new string[ea.Length];
			CompilerResults compilerResults = null;
			try
			{
				WindowsImpersonationContext windowsImpersonationContext = Executor.RevertImpersonation();
				try
				{
					for (int i = 0; i < ea.Length; i++)
					{
						if (ea[i] != null)
						{
							this.ResolveReferencedAssemblies(options, ea[i]);
							array[i] = options.TempFiles.AddExtension(i.ToString() + this.FileExtension);
							Stream stream = new FileStream(array[i], FileMode.Create, FileAccess.Write, FileShare.Read);
							try
							{
								using (StreamWriter streamWriter = new StreamWriter(stream, Encoding.UTF8))
								{
									((ICodeGenerator)this).GenerateCodeFromCompileUnit(ea[i], streamWriter, base.Options);
									streamWriter.Flush();
								}
							}
							finally
							{
								stream.Close();
							}
						}
					}
					compilerResults = this.FromFileBatch(options, array);
				}
				finally
				{
					Executor.ReImpersonate(windowsImpersonationContext);
				}
			}
			catch
			{
				throw;
			}
			return compilerResults;
		}

		// Token: 0x06003B8B RID: 15243 RVA: 0x000F5E18 File Offset: 0x000F4018
		private void ResolveReferencedAssemblies(CompilerParameters options, CodeCompileUnit e)
		{
			if (e.ReferencedAssemblies.Count > 0)
			{
				foreach (string text in e.ReferencedAssemblies)
				{
					if (!options.ReferencedAssemblies.Contains(text))
					{
						options.ReferencedAssemblies.Add(text);
					}
				}
			}
		}

		// Token: 0x06003B8C RID: 15244 RVA: 0x000F5E90 File Offset: 0x000F4090
		protected virtual CompilerResults FromFileBatch(CompilerParameters options, string[] fileNames)
		{
			if (options == null)
			{
				throw new ArgumentNullException("options");
			}
			if (fileNames == null)
			{
				throw new ArgumentNullException("fileNames");
			}
			new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();
			string text = null;
			int num = 0;
			CompilerResults compilerResults = new CompilerResults(options.TempFiles);
			SecurityPermission securityPermission = new SecurityPermission(SecurityPermissionFlag.ControlEvidence);
			securityPermission.Assert();
			try
			{
				compilerResults.Evidence = options.Evidence;
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			bool flag = false;
			if (options.OutputAssembly == null || options.OutputAssembly.Length == 0)
			{
				string text2 = (options.GenerateExecutable ? "exe" : "dll");
				options.OutputAssembly = compilerResults.TempFiles.AddExtension(text2, !options.GenerateInMemory);
				new FileStream(options.OutputAssembly, FileMode.Create, FileAccess.ReadWrite).Close();
				flag = true;
			}
			compilerResults.TempFiles.AddExtension("pdb");
			string text3 = this.CmdArgsFromParameters(options) + " " + CodeCompiler.JoinStringArray(fileNames, " ");
			string responseFileCmdArgs = this.GetResponseFileCmdArgs(options, text3);
			string text4 = null;
			if (responseFileCmdArgs != null)
			{
				text4 = text3;
				text3 = responseFileCmdArgs;
			}
			this.Compile(options, Executor.GetRuntimeInstallDirectory(), this.CompilerName, text3, ref text, ref num, text4);
			compilerResults.NativeCompilerReturnValue = num;
			if (num != 0 || options.WarningLevel > 0)
			{
				FileStream fileStream = new FileStream(text, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
				try
				{
					if (fileStream.Length > 0L)
					{
						StreamReader streamReader = new StreamReader(fileStream, Encoding.UTF8);
						string text5;
						do
						{
							text5 = streamReader.ReadLine();
							if (text5 != null)
							{
								compilerResults.Output.Add(text5);
								this.ProcessCompilerOutputLine(compilerResults, text5);
							}
						}
						while (text5 != null);
					}
				}
				finally
				{
					fileStream.Close();
				}
				if (num != 0 && flag)
				{
					File.Delete(options.OutputAssembly);
				}
			}
			if (!compilerResults.Errors.HasErrors && options.GenerateInMemory)
			{
				FileStream fileStream2 = new FileStream(options.OutputAssembly, FileMode.Open, FileAccess.Read, FileShare.Read);
				try
				{
					int num2 = (int)fileStream2.Length;
					byte[] array = new byte[num2];
					fileStream2.Read(array, 0, num2);
					SecurityPermission securityPermission2 = new SecurityPermission(SecurityPermissionFlag.ControlEvidence);
					securityPermission2.Assert();
					try
					{
						if (!FileIntegrity.IsEnabled)
						{
							compilerResults.CompiledAssembly = Assembly.Load(array, null, options.Evidence);
							return compilerResults;
						}
						if (!FileIntegrity.IsTrusted(fileStream2.SafeFileHandle))
						{
							throw new IOException(SR.GetString("FileIntegrityCheckFailed", new object[] { options.OutputAssembly }));
						}
						compilerResults.CompiledAssembly = CodeCompiler.LoadImageSkipIntegrityCheck(array, null, options.Evidence);
						return compilerResults;
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
				}
				finally
				{
					fileStream2.Close();
				}
			}
			compilerResults.PathToAssembly = options.OutputAssembly;
			return compilerResults;
		}

		// Token: 0x06003B8D RID: 15245
		protected abstract void ProcessCompilerOutputLine(CompilerResults results, string line);

		// Token: 0x06003B8E RID: 15246
		protected abstract string CmdArgsFromParameters(CompilerParameters options);

		// Token: 0x06003B8F RID: 15247 RVA: 0x000F6140 File Offset: 0x000F4340
		protected virtual string GetResponseFileCmdArgs(CompilerParameters options, string cmdArgs)
		{
			string text = options.TempFiles.AddExtension("cmdline");
			Stream stream = new FileStream(text, FileMode.Create, FileAccess.Write, FileShare.Read);
			try
			{
				using (StreamWriter streamWriter = new StreamWriter(stream, Encoding.UTF8))
				{
					streamWriter.Write(cmdArgs);
					streamWriter.Flush();
				}
			}
			finally
			{
				stream.Close();
			}
			return "@\"" + text + "\"";
		}

		// Token: 0x06003B90 RID: 15248 RVA: 0x000F61C0 File Offset: 0x000F43C0
		protected virtual CompilerResults FromSourceBatch(CompilerParameters options, string[] sources)
		{
			if (options == null)
			{
				throw new ArgumentNullException("options");
			}
			if (sources == null)
			{
				throw new ArgumentNullException("sources");
			}
			new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();
			string[] array = new string[sources.Length];
			FileStream[] array2 = new FileStream[sources.Length];
			CompilerResults compilerResults = null;
			try
			{
				WindowsImpersonationContext windowsImpersonationContext = Executor.RevertImpersonation();
				try
				{
					try
					{
						bool isEnabled = FileIntegrity.IsEnabled;
						for (int i = 0; i < sources.Length; i++)
						{
							string text = options.TempFiles.AddExtension(i.ToString() + this.FileExtension);
							FileStream fileStream = new FileStream(text, FileMode.Create, FileAccess.ReadWrite, FileShare.Read);
							array2[i] = fileStream;
							using (StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.UTF8))
							{
								streamWriter.Write(sources[i]);
								streamWriter.Flush();
								if (isEnabled)
								{
									FileIntegrity.MarkAsTrusted(fileStream.SafeFileHandle);
								}
							}
							array[i] = text;
						}
						compilerResults = this.FromFileBatch(options, array);
					}
					finally
					{
						int num = 0;
						while (num < array2.Length && array2[num] != null)
						{
							array2[num].Close();
							num++;
						}
					}
				}
				finally
				{
					Executor.ReImpersonate(windowsImpersonationContext);
				}
			}
			catch
			{
				throw;
			}
			return compilerResults;
		}

		// Token: 0x06003B91 RID: 15249 RVA: 0x000F6310 File Offset: 0x000F4510
		protected static string JoinStringArray(string[] sa, string separator)
		{
			if (sa == null || sa.Length == 0)
			{
				return string.Empty;
			}
			if (sa.Length == 1)
			{
				return "\"" + sa[0] + "\"";
			}
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < sa.Length - 1; i++)
			{
				stringBuilder.Append("\"");
				stringBuilder.Append(sa[i]);
				stringBuilder.Append("\"");
				stringBuilder.Append(separator);
			}
			stringBuilder.Append("\"");
			stringBuilder.Append(sa[sa.Length - 1]);
			stringBuilder.Append("\"");
			return stringBuilder.ToString();
		}

		// Token: 0x06003B92 RID: 15250 RVA: 0x000F63B0 File Offset: 0x000F45B0
		internal static Assembly LoadImageSkipIntegrityCheck(byte[] rawAssembly, byte[] rawSymbolStore, Evidence securityEvidence)
		{
			MethodInfo method = typeof(Assembly).GetMethod("LoadImageSkipIntegrityCheck", BindingFlags.Static | BindingFlags.NonPublic);
			return (method != null) ? ((Assembly)method.Invoke(null, new object[] { rawAssembly, rawSymbolStore, securityEvidence })) : Assembly.Load(rawAssembly, rawSymbolStore, securityEvidence);
		}
	}
}
