namespace Application;

public record struct Range(int Start, int End)
{
    public static Range Empty => default;
}