using EventManager.ApiApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManager.ApiApp.Services {
    public interface IEventsService {
        Task<Event> GetEvent(int eventId);
        Task<Event[]> GetEvents();
        Task<Event> CreateEvent(Event ev);
        Task SetArchive(int eventId);
    }
}
