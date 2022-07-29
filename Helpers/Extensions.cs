using System.Collections.Generic;

namespace AlphaPersonel.Helpers;

public static class Extensions
{
    public static T First<T>(this IEnumerable<T> observableCollection)
    {
        var view = CollectionViewSource.GetDefaultView(observableCollection);
        var enumerator = view.GetEnumerator();
        enumerator.MoveNext();
        var firstElement = (T)enumerator.Current;
        return firstElement;
    }
}