namespace Build.CodeAnalysis.Rules.Async
{
    using Build.CodeAnalysis.Rules.Extensions;

    using Microsoft.FxCop.Sdk;

    public class AsyncMustReturnTask : CodeAnalysisRule<AsyncMustReturnTask>
    {
        public override ProblemCollection Check(Member member)
        {
            var method = member as Method;

            if (method != null && IsCandidate(method) && method.IsAsyncVoid())
            {
                this.AddProblemFromMethod(method);
            }

            return this.Problems;
        }

        private static bool IsCandidate(Method method)
        {
            return !method.IsGenerated() && !method.IsSpecialName && !method.IsEventHandlerLike();
        }
    }
}