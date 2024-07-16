using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;

namespace Application;

public sealed class DataSource : IList, INotifyCollectionChanged
{
    private readonly Dictionary<int, Model> _cache = new();
    private readonly BehaviorSubject<Range> _visibleRange = new(Range.Empty);

    public event NotifyCollectionChangedEventHandler? CollectionChanged;
    public int Count { get; }
    public bool IsFixedSize => true;
    public bool IsReadOnly => true;
    public bool IsSynchronized => false;
    public object SyncRoot => throw new NotImplementedException();

    public Range VisibleRange
    {
        get => _visibleRange.Value;
        set => _visibleRange.OnNext(value);
    }

    public DataSource(int count)
    {
        Count = count;

        _visibleRange
            .Throttle(TimeSpan.FromMilliseconds(100))
            .DistinctUntilChanged()
            .Select(r => Observable.FromAsync(t => Request(r, t)))
            .Switch()
            .ObserveOn(SynchronizationContext.Current!)
            .Subscribe(Update);
    }

    public int Add(object? value) => throw new NotImplementedException();
    public void Clear() => throw new NotImplementedException();
    public bool Contains(object? value) => throw new NotImplementedException();
    public void CopyTo(Array array, int index) => throw new NotImplementedException();

    public IEnumerator GetEnumerator()
    {
        yield break;
    }

    public int IndexOf(object? value) => throw new NotImplementedException();
    public void Insert(int index, object? value) => throw new NotImplementedException();
    public void Remove(object? value) => throw new NotImplementedException();
    public void RemoveAt(int index) => throw new NotImplementedException();

    public object? this[int index]
    {
        get
        {
            if (_cache.TryGetValue(index, out var value))
            {
                return value;
            }

            return Model.Default;
        }
        set => throw new NotImplementedException();
    }

    private static async Task<Response> Request(Range range, CancellationToken cancellationToken)
    {
        var response = new Response();

        for (var i = range.Start; i < range.End; ++i)
        {
            response.Items.Add(new Model(i, $"Model {i}"));
        }

        await Task.Delay(100, cancellationToken); // Simulate delay.
        return response;
    }

    private void Update(Response response)
    {
        foreach (var item in response.Items)
        {
            _cache[item.Id] = item;
        }

        CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }
}