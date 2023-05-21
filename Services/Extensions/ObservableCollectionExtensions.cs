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

        public static void Replace<T>(this ObservableCollection<T> collection, T oldValue, T newValue)
        {
            bool matchFound = false;
            int index = 0;
            foreach (var item in collection)
            {
                if (item.Equals(oldValue))
                {
                    index = collection.IndexOf(item);
                    matchFound = true;
                    break;
                }
            }
            if(matchFound)
            {
                collection.Remove(oldValue);
                collection.Insert(index, newValue);
            }
        }
    }
}
