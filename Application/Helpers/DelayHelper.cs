namespace Application.Helpers;
public static class DelayHelper {
    public static Task Short(CancellationToken ct) => Task.Delay(50, ct);
    public static Task Medium(CancellationToken ct) => Task.Delay(100, ct);
    public static Task Long(CancellationToken ct) => Task.Delay(200, ct);
}
