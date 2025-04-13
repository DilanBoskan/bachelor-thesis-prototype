namespace Domain.Entities.Pages;

public sealed record PageId(int Value) {
    public static PageId Create(int value) => new(value);
    public static PageId New() => new(_counter++);

    private static int _counter = 0;
}


//public sealed record PageId(Guid Value) {
//    public static PageId Create(Guid value) => new PageId(value);
//    public static PageId New() => new(Guid.NewGuid());
//}