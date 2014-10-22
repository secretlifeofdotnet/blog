namespace Build.CodeAnalysis.Rules.Samples.Async
{
    using System.Threading.Tasks;

    public class AsyncMustReturnTaskFailing
    {
        [Expect(Expectation.Error)]
        public async void AsyncMethodReturningTaskAsync()
        {
            await Task.Run(() => { });
        }

        [Expect(Expectation.Error)]
        public async void AsyncMethodReturningTaskWithAsyncLambdaAsync()
        {
            await Task.Run(async () => await Task.Run(() => { }));
        }
    }
}