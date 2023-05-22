using System.Collections.ObjectModel;

namespace Services.Extensions
{
    public static class ObservableCollectionExtensions
    {
        public static void RemoveRange<T>(this ObservableCollection<T> collection, IEnumerable<T> itemsToRemove)
        {
            foreach (var item in itemsToRemove)
            {
                collection.Remove(item);
            }
        }
    }
}
