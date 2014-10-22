namespace Build.CodeAnalysis.Tests
{
    using Xunit;

    public abstract class TestingCodeAnalysis<T> : IUseFixture<CodeAnalysisFixture<T>>
        where T : TestingCodeAnalysis<T>
    {
        /// <summary>
        /// Gets the test fixture.
        /// </summary>
        protected CodeAnalysisFixture<T> Fixture { get; private set; }

        /// <summary>
        /// Sets the test fixture.
        /// </summary>
        /// <param name="data">The data.</param>
        public void SetFixture(CodeAnalysisFixture<T> data)
        {
            data.GenerateResultsForRuleAssembly("Build.CodeAnalysis.Rules");
            this.Fixture = data;
        }
    }
}