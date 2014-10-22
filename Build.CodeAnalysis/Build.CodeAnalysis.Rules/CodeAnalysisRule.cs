namespace Build.CodeAnalysis.Rules
{
    using System.Collections.Generic;

    using Microsoft.FxCop.Sdk;

    public abstract class CodeAnalysisRule<TCodeAnalysisRule> : BaseIntrospectionRule
        where TCodeAnalysisRule : BaseIntrospectionRule
    {
        protected CodeAnalysisRule()
            : base(typeof(TCodeAnalysisRule).Name, GetRulesManifestName(), typeof(TCodeAnalysisRule).Assembly)
        {
        }

        protected void AddProblemFromMethod(Method method, params object[] parameters)
        {
            this.Problems.Add(this.CreateProblemFromMethod(method, parameters));
        }

        protected void AddProblemFromTypeNode(TypeNode type, params object[] parameters)
        {
            this.Problems.Add(this.CreateProblemFromTypeNode(type, parameters));
        }

        protected Problem CreateProblemFromMethod(Method method, params object[] parameters)
        {
            var args = MergeParameters(parameters, method.DeclaringType.Name.Name, method.Name.Name);

            return new Problem(this.GetResolution(args), method.SourceContext);
        }

        protected Problem CreateProblemFromTypeNode(TypeNode type, params object[] parameters)
        {
            var args = MergeParameters(parameters, type.Namespace.Name, type.Name.Name);

            return new Problem(this.GetResolution(args), type.SourceContext);
        }

        private static string GetRulesManifestName()
        {
            return typeof(CodeAnalysisRule<>).Assembly.GetName().Name + ".Rules";
        }

        /// <summary>
        /// Simple merge to join together the parameters defined by the caller (source) and the
        /// parameters used by the CreateProblemFromXXX method.
        /// </summary>
        /// <param name="source">The parameters from the developer.</param>
        /// <param name="target">Pre-defined parameters from the method.</param>
        /// <returns>Returns an array of parameters.</returns>
        private static object[] MergeParameters(IEnumerable<object> source, params object[] target)
        {
            var parameters = new List<object>();
            parameters.AddRange(target);
            parameters.AddRange(source);

            return parameters.ToArray();
        }
    }
}