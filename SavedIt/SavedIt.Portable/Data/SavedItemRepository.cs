using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SavedIt.Portable.Models;
using SQLite.Net;

namespace SavedIt.Portable.Data
{
    public class SavedItemRepository
    {
        private SQLiteConnection _connection;

        public SavedItemRepository(SQLiteConnection connection)
        {
            _connection = connection;
            _connection.CreateTable<SavedItem>();
        }

        public void Create(SavedItem item)
        {
            _connection.Insert(item);
            _connection.Commit();
        }

        public void Delete(SavedItem item)
        {
            _connection.Delete(item);
            _connection.Commit();
        }

        public SavedItem Get(int id)
        {
           return _connection.Table<SavedItem>().FirstOrDefault(i => i.Id == id);
        }

        public List<SavedItem> GetAll()
        {
            return _connection.Table<SavedItem>().ToList();
        }

        public void Update(SavedItem item)
        {
            _connection.Update(item);
            _connection.Commit();
        }
    }
}
