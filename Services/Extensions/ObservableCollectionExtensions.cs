using System.Collections.ObjectModel;

namespace Services.Extensions
{
    /// <summary>
    /// A static class for <see cref="ObservableCollection{T}"/> extension methods.
    /// </summary>
    public static class ObservableCollectionExtensions
    {
        /// <summary>
        /// Remove a range of items from an <see cref="ObservableCollection{T}"/>.
        /// </summary>
        /// <typeparam name="T"> The data type for this collection instance. </typeparam>
        /// <param name="collection"> The <see cref="ObservableCollection{T}"/> to remove the items from. </param>
        /// <param name="itemsToRemove"> An <see cref="IEnumerable{T}"/> collection of items to be removed. </param>
        public static void RemoveRange<T>(this ObservableCollection<T> collection, IEnumerable<T> itemsToRemove)
        {
            foreach (var item in itemsToRemove)
            {
                collection.Remove(item);
            }
        }
    }
}
