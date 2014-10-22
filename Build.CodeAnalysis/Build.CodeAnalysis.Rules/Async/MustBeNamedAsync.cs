namespace Build.CodeAnalysis.Rules.Async
{
    using Build.CodeAnalysis.Rules.Extensions;

    using Microsoft.FxCop.Sdk;

    public class MustBeNamedAsync : CodeAnalysisRule<MustBeNamedAsync>
    {
        public override ProblemCollection Check(Member member)
        {
            var method = member as Method;

            if (method != null && IsCandidate(method) && !method.Name.Name.EndsWith("Async"))
            {
                this.AddProblemFromMethod(method);
            }

            return this.Problems;
        }

        private static bool IsCandidate(Method method)
        {
            return !method.IsGenerated() && !method.IsSpecialName && method.ReturnsTask();
        }
    }
}