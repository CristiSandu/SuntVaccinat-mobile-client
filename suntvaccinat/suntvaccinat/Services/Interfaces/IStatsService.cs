using Microcharts;
using System.Threading.Tasks;

namespace suntvaccinat.Services.Interfaces
{
    public interface IStatsService
    {
        Task<bool> AddNewUserToStat(int age, int id_event);
        Task<Chart> GenerateStatsForEvent(int idEvent);
    }
}