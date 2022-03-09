using suntvaccinat.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace suntvaccinat.Services
{
    public interface IEventsDataBase
    {
        Task AddUser(User user );
        Task<User> GetUser();
        Task<bool> RemoveUser(User user);
        Task AddEvent(string name, DateTime date);
        Task AddUserToEvent(ParticipantModel part);
        Task<bool> CloseAEvent(int id);
        Task<EventModel> GetEvByID(int id);
        Task<IEnumerable<EventModel>> GetEvents(string querie);
        Task<IEnumerable<ParticipantModel>> GetPartPerEvent(string querie);
        Task RemoveEvent(int id);

    }
}