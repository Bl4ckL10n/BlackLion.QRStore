using BlackLion.QRStore.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlackLion.QRStore.Services
{
    public class SQLiteDataStore : IDataStore<Item>
    {
        readonly SQLiteAsyncConnection database;

        public SQLiteDataStore(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<Item>().Wait();
        }

        public async Task<bool> AddItemAsync(Item item)
        {
            if (item.Id != 0)
            {
                return false;
            }
            else
            {
                var affectedRows = await database.InsertAsync(item);

                return affectedRows >= 1;
            }
        }

        public async Task<bool> UpdateItemAsync(Item item)
        {
            if (item.Id != 0)
            {
                var affectedRows = await database.UpdateAsync(item);

                return affectedRows >= 1;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> DeleteItemAsync(int id)
        {
            var item = await GetItemAsync(id);

            if (item == null)
            {
                return false;
            }

            var affectedRows = await database.DeleteAsync(item);

            return affectedRows >= 1;
        }

        public async Task<Item> GetItemAsync(int id)
        {
            return await database.Table<Item>()
                            .Where(i => i.Id == id)
                            .FirstOrDefaultAsync();
        }

        public async Task<List<Item>> GetItemsAsync(bool forceRefresh = false)
        {
            return await database.Table<Item>().ToListAsync();
        }

        public async Task<List<Item>> FindAllByPredicateAsync(Predicate<Item> predicate)
        {
            var items = await database.Table<Item>().ToListAsync();

            return items.FindAll(predicate);
        }
    }
}