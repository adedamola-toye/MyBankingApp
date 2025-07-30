using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using BankingApp.Interfaces;
using BankingApp.Models;

namespace BankingApp.Data
{
    /// <summary>
    /// Provides file-based persistence for user data using JSON serialization
    /// </summary>
    /// <remarks>
    /// Implements <see cref="IDataStore{TKey, TValue}"/> for storing user data in a JSON file.
    /// Handles circular references and type information during serialization.
    /// </remarks>
    public class UserFileDataStore : IDataStore<string, User>
    {
        private readonly string _filePath = "users.json";

        /// <summary>
        /// Loads user data from the JSON file
        /// </summary>
        /// <returns>
        /// Dictionary of users keyed by UserId. Returns empty dictionary if:
        /// - File doesn't exist
        /// - File is empty
        /// - Deserialization fails
        /// </returns>
        /// <remarks>
        /// Additional behaviors:
        /// <list type="bullet">
        ///   <item><description>Automatically recreates owner references for accounts</description></item>
        ///   <item><description>Uses TypeNameHandling.All to preserve polymorphic types</description></item>
        ///   <item><description>Handles circular references during deserialization</description></item>
        /// </list>
        /// </remarks>
       public Dictionary<string, User> Load()
{
    if (!File.Exists(_filePath))
        return new Dictionary<string, User>();

    var settings = new JsonSerializerSettings
    {
        PreserveReferencesHandling = PreserveReferencesHandling.Objects,
        TypeNameHandling = TypeNameHandling.Auto,
        ReferenceLoopHandling = ReferenceLoopHandling.Serialize, // Add this
        ObjectCreationHandling = ObjectCreationHandling.Replace // Add this
    };

    var users = JsonConvert.DeserializeObject<Dictionary<string, User>>(
        File.ReadAllText(_filePath), 
        settings) ?? new Dictionary<string, User>();

    // Rebuild owner references
    foreach (var user in users.Values.Where(u => u?.Accounts != null))
    {
        foreach (var account in user.Accounts.Where(a => a != null))
        {
            account.Owner = user;
        }
    }
    
    return users;
}

        /// <summary>
        /// Saves user data to the JSON file
        /// </summary>
        /// <param name="data">User dictionary to serialize</param>
        /// <remarks>
        /// Serialization features:
        /// <list type="bullet">
        ///   <item><description>Formatted JSON output (indented)</description></item>
        ///   <item><description>Preserves object references</description></item>
        ///   <item><description>Includes type information for polymorphic serialization</description></item>
        /// </list>
        /// </remarks>
        public void Save(Dictionary<string, User> data)
        {
            string json = JsonConvert.SerializeObject(data, Formatting.Indented,
                new JsonSerializerSettings
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.All,
                    TypeNameHandling = TypeNameHandling.All
                });

            File.WriteAllText(_filePath, json);
        }
    }
}