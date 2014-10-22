namespace Build.CodeAnalysis.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Xml.Linq;

    using Build.CodeAnalysis.Rules.Samples;
    using Build.CodeAnalysis.Tests.Extensions;

    using Xunit;

    public class CodeAnalysisFixture<T>
        where T : TestingCodeAnalysis<T>
    {
        private const string FxCopExe = "FxCopCmd.exe";

        private const string FxCopResults = "analysis.results.xml";

        private readonly string[] locations =
        {
            @"C:\Program Files (x86)\Microsoft Visual Studio 10.0\Team Tools\Static Analysis Tools\FxCop",
            @"C:\Program Files (x86)\Microsoft Visual Studio 11.0\Team Tools\Static Analysis Tools\FxCop",
            @"C:\Program Files (x86)\Microsoft Visual Studio 12.0\Team Tools\Static Analysis Tools\FxCop"
        };

        protected DirectoryInfo CodeAnalysisDirectory { get; private set; }

        protected string CodeAnalysisExecutable { get; private set; }

        protected string CodeAnalysisFileName { get; private set; }

        protected FileInfo RulesAssembly { get; private set; }

        protected FileInfo RulesSamplesAssembly { get; private set; }

        public void AssertFailingCases<TFailing>()
        {
            var methods = typeof(TFailing).GetMethods();
            var results = this.LoadResults();
            var type = results.Descendants().Single(e => e.Name == "Type" && e.Attribute("Name").Value == typeof(TFailing).Name);

            foreach (var method in methods.Where(m => m.CustomAttributes.Any(a => a.AttributeType == typeof(ExpectAttribute))))
            {
                var attribute = method.GetCustomAttribute<ExpectAttribute>();
                var name = string.Format("#{0}()", method.Name);
                var member = type.Descendants().SingleOrDefault(e => e.Name == "Member" && e.Attribute("Name").Value == name);
                Assert.NotNull(member);

                var issues =
                    member.Descendants().Where(e => e.Name == "Issue" && e.Attribute("Level").Value == attribute.Expectation.ToString());

                Assert.NotEmpty(issues);
            }
        }

        public void AssertPassingCases<TPassing>()
        {
            var results = this.LoadResults();
            var type = results.Descendants().SingleOrDefault(e => e.Name == "Type" && e.Attribute("Name").Value == typeof(TPassing).Name);

            Assert.Null(type);
        }

        public bool GenerateResultsForRuleAssembly(string rulesAssemblyName)
        {
            this.SetupTestingEnvironment(rulesAssemblyName);

            return this.RunCodeAnalysis();
        }

        public XElement LoadResults()
        {
            return XDocument.Load(this.CodeAnalysisFileName).Root;
        }

        private static void ProcessOnErrorDataReceived(object sender, DataReceivedEventArgs args)
        {
            Trace.WriteLine(args.Data, "ERROR");
        }

        private static void ProcessOnOutputDataReceived(object sender, DataReceivedEventArgs args)
        {
            Trace.WriteLine(args.Data, "OUTPUT");
        }

        private Process CreateCodeAnalysisProcess()
        {
            var process = new Process();

            try
            {
                var startup = new ProcessStartInfo(this.CodeAnalysisExecutable)
                {
                    Arguments = this.CreateCodeAnalysisProcessArguments(),
                    CreateNoWindow = true,
                    RedirectStandardError = true,
                    RedirectStandardInput = false,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    WorkingDirectory = this.CodeAnalysisDirectory.FullName,
                };

                process.StartInfo = startup;

                return process;
            }
            catch
            {
                process.Dispose();
                throw;
            }
        }

        private string CreateCodeAnalysisProcessArguments()
        {
            var arguments = new List<string>
            {
                "/directory:" + this.CodeAnalysisDirectory.FullName.ToQuoted(),
                "/file:" + this.RulesSamplesAssembly.FullName.ToQuoted(),
                "/out:" + this.CodeAnalysisFileName.ToQuoted(),
                "/rule:" + this.RulesAssembly.FullName.ToQuoted(),
                "/summary",
                "/verbose",
            };

            return string.Join(" ", arguments);
        }

        private string GetFxCopLocation()
        {
            return (from path in this.locations
                    let filename = Path.Combine(path, FxCopExe)
                    orderby filename descending // We want newer versions first
                    where File.Exists(filename)
                    select filename).FirstOrDefault();
        }

        private bool RunCodeAnalysis()
        {
            using (var process = this.CreateCodeAnalysisProcess())
            {
                process.ErrorDataReceived += ProcessOnErrorDataReceived;
                process.OutputDataReceived += ProcessOnOutputDataReceived;

                try
                {
                    if (process.Start())
                    {
                        process.BeginErrorReadLine();
                        process.BeginOutputReadLine();
                        process.WaitForExit(5000);

                        if (process.ExitCode != 0)
                        {
                            throw new InvalidOperationException(string.Format("FxCop returned exit code of {0}.", process.ExitCode));
                        }

                        return true;
                    }

                    throw new InvalidOperationException("Could not successfully start FxCop process.");
                }
                finally
                {
                    process.ErrorDataReceived -= ProcessOnErrorDataReceived;
                    process.OutputDataReceived -= ProcessOnOutputDataReceived;
                }
            }
        }

        private void SetupTestingEnvironment(string rulesAssemblyName)
        {
            this.CodeAnalysisExecutable = this.GetFxCopLocation();

            if (string.IsNullOrWhiteSpace(this.CodeAnalysisExecutable))
            {
                throw new FileNotFoundException("Could not find FxCop executable.", FxCopExe);
            }

            var directory = Path.GetDirectoryName(new Uri(this.GetType().Assembly.CodeBase).LocalPath);
            Debug.Assert(!string.IsNullOrWhiteSpace(directory), "Path to assembly output must not be null.");
            this.CodeAnalysisDirectory = new DirectoryInfo(directory);
            Trace.WriteLine(string.Format("Looking for assemblies in {0}.", this.CodeAnalysisDirectory.FullName));

            this.RulesAssembly = new FileInfo(Path.Combine(directory, rulesAssemblyName + ".dll"));
            Debug.Assert(this.RulesAssembly.Exists, "Could not find rules assembly.");
            Trace.WriteLine(string.Format("Using rules assembly {0}.", this.RulesAssembly.Name));

            this.RulesSamplesAssembly = new FileInfo(Path.Combine(directory, rulesAssemblyName + ".Samples.dll"));
            Debug.Assert(this.RulesSamplesAssembly.Exists, "Could not find rules sample assembly.");
            Trace.WriteLine(string.Format("Using rules sample assembly {0}.", this.RulesSamplesAssembly.Name));

            this.CodeAnalysisFileName = Path.Combine(directory, typeof(T).Name + "." + FxCopResults);
            Trace.WriteLine(string.Format("Rules will output to {0}.", this.CodeAnalysisFileName));
        }
    }
}