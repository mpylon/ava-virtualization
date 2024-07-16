using System;
using Avalonia.Controls;

namespace Application;

public partial class MainWindow : Window
{
    public DataSource ItemsSource { get; } = new(10_000);

    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;
    }

    private void ScrollViewer_OnScrollChanged(object? sender, ScrollChangedEventArgs e)
    {
        var panel = (VirtualizingStackPanel)Items.ItemsPanelRoot!;

        ItemsSource.VisibleRange = new Range(
            Math.Max(0, panel.FirstRealizedIndex),
            Math.Max(0, panel.LastRealizedIndex + 1));
    }
}