using System;
using Avalonia;
using Avalonia.Controls;

namespace Application;

public sealed class MyVirtualizingStackPanel : VirtualizingStackPanel
{
    protected override Size MeasureOverride(Size availableSize)
    {
        Console.WriteLine("Measuring VirtualizingStackPanel.");
        return base.MeasureOverride(availableSize);
    }
}