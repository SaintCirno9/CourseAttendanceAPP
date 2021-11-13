using System;
using System.Threading.Tasks;
using SQLite;

namespace AppShared.Services
{
    public class LoginDatabaseService
    {
        private readonly SQLiteAsyncConnection _database;

        public LoginDatabaseService(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<LoginData>().Wait();
        }

        public async Task<LoginData> GetAsync()
        {
            var loginData = await _database.Table<LoginData>().FirstOrDefaultAsync();

            if (loginData is not null && loginData.CreateTime.AddDays(15) >= DateTime.Now)
            {
                return loginData;
            }

            await DeleteAsync();
            return null;
        }

        public async Task<int> GetDataCountAsync()
        {
            var loginDataCount = await _database.Table<LoginData>().CountAsync();
            return loginDataCount;
        }

        public Task<int> SaveAsync(string id, string password)
        {
            DeleteAsync();
            return _database.InsertAsync(new LoginData()
            {
                Id = id,
                Password = password,
                CreateTime = DateTime.Now
            });
        }

        public Task<int> DeleteAsync()
        {
            return _database.DeleteAllAsync<LoginData>();
        }
    }

    public class LoginData
    {
        public string Id { get; set; }
        public string Password { get; set; }
        public DateTime CreateTime { get; set; }
    }
}