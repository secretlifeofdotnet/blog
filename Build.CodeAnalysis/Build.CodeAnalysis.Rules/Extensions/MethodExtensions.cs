namespace Build.CodeAnalysis.Rules.Extensions
{
    using System.Linq;

    using Microsoft.FxCop.Sdk;

    public static class MethodExtensions
    {
        public static bool IsAsyncTask(this Method method)
        {
            var callers = method.Instructions.Where(i => i.OpCode == OpCode.Call).Select(i => i.Value).OfType<Method>();

            return callers.Any(caller => caller.FullName == "System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Create");
        }

        public static bool IsAsyncVoid(this Method method)
        {
            var callers = method.Instructions.Where(i => i.OpCode == OpCode.Call).Select(i => i.Value).OfType<Method>();

            return callers.Any(caller => caller.FullName == "System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Create");
        }

        public static bool IsEventHandlerLike(this Method method)
        {
            var parameter = method.Parameters.FirstOrDefault();

            return parameter != null && parameter.Name.Name == "sender";
        }

        public static bool IsGenerated(this Method method)
        {
            return method.Name.Name.StartsWith("<");
        }

        public static bool IsVoid(this Method method)
        {
            return method.ReturnType.Name.Name == "Void";
        }

        public static bool ReturnsTask(this Method method)
        {
            return method.ReturnType.Name.Name.StartsWith("Task");
        }
    }
}