﻿using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Text;

namespace NetDist.Server
{
    public enum CompileResultType
    {
        Ok,
        AlreadyCompiled,
        Failed,
    }

    public class JobScriptCompileResult
    {
        public CompileResultType ResultType { get; private set; }
        public string OutputAssembly { get; private set; }
        public List<string> Output { get; private set; }
        public string OutputString { get; private set; }
        public List<string> Errors { get; private set; }
        public string ErrorString { get; private set; }

        public JobScriptCompileResult(string assemblyPath)
        {
            ResultType = CompileResultType.AlreadyCompiled;
            OutputAssembly = assemblyPath;
        }

        public JobScriptCompileResult(CompilerResults compilerResults)
        {
            Output = new List<string>();
            var sbOutput = new StringBuilder();
            for (int i = 0; i < compilerResults.Output.Count; i++)
            {
                Output.Add(compilerResults.Output[i]);
                sbOutput.AppendLine(compilerResults.Output[i]);
            }
            OutputString = sbOutput.ToString();

            if (compilerResults.Errors.HasErrors)
            {
                ResultType = CompileResultType.Failed;
                Errors = new List<string>();
                var sbError = new StringBuilder();
                for (int i = 0; i < compilerResults.Errors.Count; i++)
                {
                    Errors.Add(compilerResults.Errors[i].ToString());
                    sbError.AppendFormat("{0}: {1}", i, compilerResults.Errors[i]).AppendLine();
                }
                ErrorString = sbError.ToString();
            }
            else
            {
                ResultType = CompileResultType.Ok;
                OutputAssembly = compilerResults.PathToAssembly;
            }
        }
    }
}
