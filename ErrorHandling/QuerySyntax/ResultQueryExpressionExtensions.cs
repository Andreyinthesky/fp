using System;

namespace ResultOfTask
{
    public static class ResultQueryExpressionExtensions
    {
        public static Result<TOutput> SelectMany<TInput, TOutput>(
            this Result<TInput> input,
            Func<TInput, Result<TOutput>> continuation)
        {
            return input.Then(continuation);
        }

        public static Result<TSelected> SelectMany<TInput, TOutput, TSelected>(
            this Result<TInput> input,
            Func<TInput, Result<TOutput>> continuation,
            Func<TInput, TOutput, TSelected> resultSelector)
        {
            var output = SelectMany(input, continuation);
            return SelectMany(output, x => new Result<TSelected>(null, resultSelector(input.Value, x)));
        }
    }
}