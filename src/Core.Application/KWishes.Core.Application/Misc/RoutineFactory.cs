namespace KWishes.Core.Application.Misc;

public static class RoutineFactory
{
    public static async void StartInfiniteLoop(
        Func<ValueTask> action,
        TimeSpan delay,
        CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            await action();
            await Task.Delay(delay, cancellationToken);
        }
    }
}