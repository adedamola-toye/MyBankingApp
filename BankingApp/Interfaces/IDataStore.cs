namespace BankingApp.Interfaces
{
    /// <summary>
    /// Provides data storage and retrieval operations
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the dictionary (must be not null)</typeparam>
    /// <typeparam name="TValue">The type of values in the dictionary</typeparam>
    public interface IDataStore<TKey, TValue> where TKey : notnull
    {
        /// <summary>
        /// Loads all data from storage
        /// </summary>
        /// <returns>Dictionary containing all stored data</returns>
        Dictionary<TKey, TValue> Load();

        /// <summary>
        /// Saves data to storage
        /// </summary>
        /// <param name="data">Dictionary containing data to save</param>
        void Save(Dictionary<TKey, TValue> data);
    }
}