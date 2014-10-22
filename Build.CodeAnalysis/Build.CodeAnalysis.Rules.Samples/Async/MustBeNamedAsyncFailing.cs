namespace Build.CodeAnalysis.Rules.Samples.Async
{
    using System.Threading;
    using System.Threading.Tasks;

    public class MustBeNamedAsyncFailing
    {
        [Expect(Expectation.Error)]
        public Task IncorrectlyNamedReturningTask()
        {
            return Task.Run(() => { });
        }

        [Expect(Expectation.Error)]
        public Task IncorrectlyNamedReturningTask(CancellationToken cancellationToken)
        {
            return Task.Run(() => { }, cancellationToken);
        }

        [Expect(Expectation.Error)]
        public Task<bool> IncorrectlyNamedReturningGenericTask()
        {
            return Task.Run(() => false);
        }

        [Expect(Expectation.Error)]
        public Task<bool> IncorrectlyNamedReturningGenericTask(CancellationToken cancellationToken)
        {
            return Task.Run(() => false, cancellationToken);
        }

        [Expect(Expectation.Error)]
        public Task<bool> IncorrectlyNamedReturningGenericTaskWithAsyncLambda()
        {
            return Task.Run(async () => await Task.Run(() => false));
        }

        [Expect(Expectation.Error)]
        public Task<bool> IncorrectlyNamedReturningGenericTaskWithAsyncLambda(CancellationToken cancellationToken)
        {
            return Task.Run(async () => await Task.Run(() => false, cancellationToken), cancellationToken);
        }
    }
}