using SQLite;
using suntvaccinat.Models;
using suntvaccinat.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly:Dependency(typeof(EventsDataBase))]
namespace suntvaccinat.Services
{
    public class EventsDataBase : IEventsDataBase
    {
        SQLiteAsyncConnection db;

        async Task Init()
        {
            if (db != null)
                return;

            var databasePath = Path.Combine(FileSystem.AppDataDirectory, "mySubs.db");
            db = new SQLiteAsyncConnection(databasePath);

            await db.CreateTableAsync<EventModel>();
            await db.CreateTableAsync<ParticipantModel>();
            await db.CreateTableAsync<User>();
        }

        public async Task AddUser(User user)
        {
            await Init();
            var users = await db.Table<User>().ToListAsync();

            if (users.Count == 0)
                await db.InsertAsync(user);
        }

        public async Task<User> GetUser()
        {
            await Init();
            var users =await db.Table<User>().ToListAsync();
            return users.Count > 0 ? users[0] : new User();
        }

        public async Task<bool> RemoveUser(User user)
        {
            var number = await db.DeleteAsync<EventModel>(user.Id);
            return number > 0;
        }

        public async Task AddEvent(string name, DateTime date)
        {
            await Init();

            var subs = new EventModel
            {
                Name = name,
                Date = date,
                IsNoEnded = true
            };

            await db.InsertAsync(subs);
        }

        public async Task AddUserToEvent(ParticipantModel part)
        {
            await Init();
            await db.InsertAsync(part);
        }

        public async Task RemoveEvent(int id)
        {
            await Init();
            await db.DeleteAsync<EventModel>(id);
            await db.QueryAsync<EventModel>(Helpers.DataBaseQuerys.DeleteEventQuery(id));
        }

        public async Task<IEnumerable<EventModel>> GetEvents(string querie)
        {
            await Init();
            var events = await db.QueryAsync<EventModel>(querie);
            var events_list = events.ToList();
            return events_list;
        }

        public async Task<IEnumerable<ParticipantModel>> GetPartPerEvent(string querie)
        {
            await Init();
            var parts = await db.QueryAsync<ParticipantModel>(querie);
            var parts_list = parts.ToList();
            return parts_list;
        }

        public async Task<EventModel> GetEvByID(int id)
        {
            await Init();
            var parts = await db.QueryAsync<EventModel>(Helpers.DataBaseQuerys.GetEventsQuery(id));
            return parts[0];
        }

        public async Task<bool> CloseAEvent(int id)
        {
            await Init();
            var parts = await GetEvByID(id);

            parts.IsNoEnded = false;

            if (await db.UpdateAsync(parts) > 0)
                return true;
            else
                return false;
        }

    }
}
