using System;
using System.Collections.Generic;

namespace Xamarin.Utilities.Core.Persistence
{
    public class DatabaseCollection<TObject, TKey> : IEnumerable<TObject> where TObject : IDatabaseItem<TKey>, new()
    {
        private readonly SQLite.SQLiteConnection _sqlConnection;

        public TObject this[TKey id]
        {
            get
            {
                Func<TObject, bool> del = x => x.Id.Equals(id);
                return _sqlConnection.Find<TObject>(del);
            }
        }

        public DatabaseCollection(SQLite.SQLiteConnection sqlConnection)
        {
            _sqlConnection = sqlConnection;
            _sqlConnection.CreateTable<TObject>();
        }

        public void Insert(TObject o)
        {
            _sqlConnection.Insert(o);
        }

        public void Update(TObject o)
        {
            _sqlConnection.Update(o);
        }

        public void Remove(TObject o)
        {
            _sqlConnection.Delete(o);
        }

        public IEnumerator<TObject> GetEnumerator()
        {
            return _sqlConnection.Table<TObject>().GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

}