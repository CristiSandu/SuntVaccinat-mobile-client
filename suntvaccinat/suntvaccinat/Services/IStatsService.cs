using System.Threading.Tasks;

namespace suntvaccinat.Services
{
    public interface IStatsService
    {
        Task<bool> AddNewUserToStat(string age, int id_event);
    }
}