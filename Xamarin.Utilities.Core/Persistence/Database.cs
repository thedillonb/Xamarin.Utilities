using System;

namespace Xamarin.Utilities.Core.Persistence
{
    public class Database
    {
        private static readonly Lazy<Database> _instance = new Lazy<Database>(() => new Database());

        public static Database Instance { get { return _instance.Value; } }

        public SQLite.SQLiteConnection SqlConnection { get; private set; }

        private Database()
        {
            SqlConnection = new SQLite.SQLiteConnection(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "database.db"));
        }
    }
}

