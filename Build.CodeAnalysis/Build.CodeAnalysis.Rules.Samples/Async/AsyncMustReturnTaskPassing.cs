namespace Build.CodeAnalysis.Rules.Samples.Async
{
    using System.Threading.Tasks;

    public class AsyncMustReturnTaskPassing
    {
        [Expect(Expectation.None)]
        public async Task AsyncMethodReturningTaskAsync()
        {
            await Task.Run(() => { });
        }

        [Expect(Expectation.None)]
        public async Task AsyncMethodReturningTaskWithAsyncLambdaAsync()
        {
            await Task.Run(async () => await Task.Run(() => { }));
        }

        [Expect(Expectation.None)]
        public async Task<bool> AsyncMethodReturningGenericTaskAsync()
        {
            return await Task.Run(() => false);
        }

        [Expect(Expectation.None)]
        public async Task<bool> AsyncMethodReturningGenericTaskWithAsyncLambdaAsync()
        {
            return await Task.Run(async () => await Task.FromResult(false));
        }

        [Expect(Expectation.None)]
        public async void AsyncEventHandlerReturningVoidAsync(object sender)
        {
            await Task.Run(() => { });
        }
    }
}