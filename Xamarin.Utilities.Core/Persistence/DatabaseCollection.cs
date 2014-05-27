using System;
using System.Collections.Generic;

namespace Xamarin.Utilities.Core.Persistence
{
    public class DatabaseCollection<TObject, TKey> : IEnumerable<TObject> where TObject : IDatabaseItem<TKey>, new()
    {
        protected SQLite.SQLiteConnection SqlConnection { get; private set; }

        public TObject this[TKey id]
        {
            get
            {
                Func<TObject, bool> del = x => x.Id.Equals(id);
                return SqlConnection.Find<TObject>(del);
            }
        }

        public DatabaseCollection(SQLite.SQLiteConnection sqlConnection)
        {
            SqlConnection = sqlConnection;
            SqlConnection.CreateTable<TObject>();
        }

        public virtual void Insert(TObject o)
        {
            SqlConnection.Insert(o);
        }

        public virtual void Update(TObject o)
        {
            SqlConnection.Update(o);
        }

        public virtual void Remove(TObject o)
        {
            SqlConnection.Delete(o);
        }

        public IEnumerator<TObject> GetEnumerator()
        {
            return SqlConnection.Table<TObject>().GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

}