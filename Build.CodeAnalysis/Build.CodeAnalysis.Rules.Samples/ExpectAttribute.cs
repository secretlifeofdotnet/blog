namespace Build.CodeAnalysis.Rules.Samples
{
    using System;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class ExpectAttribute : Attribute
    {
        public ExpectAttribute(Expectation expectation)
        {
            this.Expectation = expectation;
        }

        public Expectation Expectation { get; private set; }
    }
}