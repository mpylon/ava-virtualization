namespace Application;

public sealed record Model(int Id, string Name)
{
    public static Model Default => new(-1, "Loading...");
}