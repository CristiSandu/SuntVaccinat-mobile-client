using suntvaccinat.Models;
using suntvaccinat.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(StatsService))]
namespace suntvaccinat.Services
{
    public class StatsService : IStatsService
    {
        IEventsDataBase _database;

        public StatsService()
        {
            _database = DependencyService.Get<Services.IEventsDataBase>();
        }
        public async Task<bool> AddNewUserToStat(string age, int id_event)
        {
            StatsModel stat = await _database.GetStatByEventId(id_event);
            if (stat.Id_Event == -1)
            {
                await _database.AddStatForEvent(id_event);
                stat = await _database.GetStatByEventId(id_event);
            }

            DateTime oDate = DateTime.Parse(age);
            int years = DateTime.Now.Year - oDate.Year;

            switch (years)
            {
                case int n when Enumerable.Range(0, 19).Contains(n):
                    stat.PersonsUnder19 += 1;
                    break;
                case int n when Enumerable.Range(20, 29).Contains(n):
                    stat.PersonsBetween2029 += 1;
                    break;
                case int n when Enumerable.Range(30, 39).Contains(n):
                    stat.PersonsBetween3039 += 1;
                    break;
                case int n when Enumerable.Range(40, 49).Contains(n):
                    stat.PersonsBetween4049 += 1;
                    break;
                case int n when Enumerable.Range(50, 59).Contains(n):
                    stat.PersonsBetween5059 += 1;
                    break;
                default:
                    stat.PersonsGreater60 += 1;
                    break;
            }

            await _database.UpdateStatForEvent(stat);
            return true;
        }
    }
}
