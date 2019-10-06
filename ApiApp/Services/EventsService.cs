using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventManager.ApiApp.Models;

namespace EventManager.ApiApp.Services {
    public class EventsService : IEventsService {
        public Task<Event[]> GetEvents() {
            Event[] events = new[] {
                new Event { Id = 1, Name = "Event 1", Place = "Place 1", Type = EventType.Conference, MaxGuests = 10 },
                new Event { Id = 2, Name = "Event 2", Place = "Place 2", Type = EventType.Conference, MaxGuests = 10 },
                new Event { Id = 3, Name = "Event 3", Place = "Place 3", Type = EventType.Conference, MaxGuests = 10 },
            };
            return Task.FromResult(events);
        }

        public Task SetArchive(int eventId) {
            return Task.CompletedTask;
        }
    }
}
