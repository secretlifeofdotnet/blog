namespace Build.CodeAnalysis.Rules.Samples.Async
{
    using System.Threading;
    using System.Threading.Tasks;

    public class MustBeNamedAsyncPassing
    {
        [Expect(Expectation.None)]
        public Task IncorrectlyNamedReturningTaskAsync()
        {
            return Task.Run(() => { });
        }

        [Expect(Expectation.None)]
        public Task IncorrectlyNamedReturningTaskAsync(CancellationToken cancellationToken)
        {
            return Task.Run(() => { }, cancellationToken);
        }

        [Expect(Expectation.None)]
        public Task<bool> IncorrectlyNamedReturningGenericTaskAsync()
        {
            return Task.Run(() => false);
        }

        [Expect(Expectation.None)]
        public Task<bool> IncorrectlyNamedReturningGenericTaskAsync(CancellationToken cancellationToken)
        {
            return Task.Run(() => false, cancellationToken);
        }

        [Expect(Expectation.None)]
        public Task<bool> IncorrectlyNamedReturningGenericTaskWithAsyncLambdaAsync()
        {
            return Task.Run(async () => await Task.Run(() => false));
        }

        [Expect(Expectation.None)]
        public Task<bool> IncorrectlyNamedReturningGenericTaskWithAsyncLambdaAsync(CancellationToken cancellationToken)
        {
            return Task.Run(async () => await Task.Run(() => false, cancellationToken), cancellationToken);
        }

        [Expect(Expectation.None)]
        public async void EventHandlerShouldNotRequireNameChange(object sender)
        {
            await Task.Run(() => { });
        }
    }
}