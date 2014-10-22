namespace Build.CodeAnalysis.Tests
{
    using Build.CodeAnalysis.Rules.Samples.Async;

    using Xunit;

    public class WhenRunningCodeAnalysis : TestingCodeAnalysis<WhenRunningCodeAnalysis>
    {
        [Fact]
        public void AsyncMustReturnTaskResultsShouldContainFailing()
        {
            this.Fixture.AssertFailingCases<AsyncMustReturnTaskFailing>();
        }

        [Fact]
        public void AsyncMustReturnTaskResultsShouldNotContainPassing()
        {
            this.Fixture.AssertPassingCases<AsyncMustReturnTaskPassing>();
        }

        [Fact]
        public void MustBeNamedAsyncShouldContainFailing()
        {
            this.Fixture.AssertFailingCases<MustBeNamedAsyncFailing>();
        }

        [Fact]
        public void MustBeNamedAsyncShouldNotContainPassing()
        {
            this.Fixture.AssertPassingCases<MustBeNamedAsyncPassing>();
        }
    }
}