using Microcharts;
using SkiaSharp;
using suntvaccinat.Models;
using suntvaccinat.Services;
using suntvaccinat.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Entry = Microcharts.ChartEntry;

[assembly: Dependency(typeof(StatsService))]
namespace suntvaccinat.Services
{
    public class StatsService : IStatsService
    {
        IEventsDataBase _database;

        public StatsService()
        {
            _database = DependencyService.Get<IEventsDataBase>();
        }
        public async Task<bool> AddNewUserToStat(int age, int idEvent)
        {
            StatsModel stat = await _database.GetStatByEventId(idEvent);
            if (stat.Id_Event == -1)
            {
                await _database.AddStatForEvent(idEvent);
                stat = await _database.GetStatByEventId(idEvent);
            }

            switch (age)
            {
                case int n when Enumerable.Range(0, 19).Contains(n):
                    stat.PersonsUnder19 += 1;
                    break;
                case int n when Enumerable.Range(20, 10).Contains(n):
                    stat.PersonsBetween2029 += 1;
                    break;
                case int n when Enumerable.Range(30, 10).Contains(n):
                    stat.PersonsBetween3039 += 1;
                    break;
                case int n when Enumerable.Range(40, 10).Contains(n):
                    stat.PersonsBetween4049 += 1;
                    break;
                case int n when Enumerable.Range(50, 10).Contains(n):
                    stat.PersonsBetween5059 += 1;
                    break;
                default:
                    stat.PersonsGreater60 += 1;
                    break;
            }

            await _database.UpdateStatForEvent(stat);
            return true;
        }

        public async Task<Chart> GenerateStatsForEvent(int idEvent)
        {
            Color color = (Color)Application.Current.Resources["OrangePeel"];
            StatsModel stat = await _database.GetStatByEventId(idEvent);

            var entries = new[]
            {
                new Entry(stat.PersonsUnder19)
                {
                    Label = "0-19",
                    ValueLabel = stat.PersonsUnder19.ToString(),
                    TextColor = SKColor.Parse(color.ToHex()),
                    ValueLabelColor = SKColor.Parse(color.ToHex()),
                    Color = SKColor.Parse(color.ToHex())
                },
                new Entry(stat.PersonsBetween2029)
                {
                    Label = "20-29",
                    ValueLabel = stat.PersonsBetween2029.ToString(),
                    TextColor = SKColor.Parse(color.ToHex()),
                    ValueLabelColor = SKColor.Parse(color.ToHex()),

                    Color = SKColor.Parse(color.ToHex())
                },
                new Entry(stat.PersonsBetween3039)
                {
                    Label = "30-39",
                    ValueLabel = stat.PersonsBetween3039.ToString(),
                    TextColor = SKColor.Parse(color.ToHex()),
                    ValueLabelColor = SKColor.Parse(color.ToHex()),

                    Color = SKColor.Parse(color.ToHex())
                },
                new Entry(stat.PersonsBetween4049)
                {
                    Label = "40-49",
                    ValueLabel = stat.PersonsBetween4049.ToString(),
                    TextColor = SKColor.Parse(color.ToHex()),
                    ValueLabelColor = SKColor.Parse(color.ToHex()),

                    Color = SKColor.Parse(color.ToHex())
                },
                new Entry(stat.PersonsBetween5059)
                {
                    Label = "50-59",
                    ValueLabel = stat.PersonsBetween5059.ToString(),
                    TextColor = SKColor.Parse(color.ToHex()),
                    ValueLabelColor = SKColor.Parse(color.ToHex()),

                    Color = SKColor.Parse(color.ToHex())
                },
                  new Entry(stat.PersonsGreater60)
                {
                    Label = "60+",
                    ValueLabel = stat.PersonsGreater60.ToString(),
                    TextColor = SKColor.Parse(color.ToHex()),
                    ValueLabelColor = SKColor.Parse(color.ToHex()),
                    Color = SKColor.Parse(color.ToHex())
                }
            };

            return new BarChart
            {
                Entries = entries,
                LabelTextSize = 40f,
                Margin = 50,
                LabelOrientation = Orientation.Horizontal,
                ValueLabelOrientation = Orientation.Horizontal,
                BackgroundColor = SKColor.Parse(Color.Transparent.ToHex())
            };
        }
    }
}
