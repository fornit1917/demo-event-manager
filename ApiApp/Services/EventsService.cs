using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EventManager.ApiApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EventManager.ApiApp.Services {
    public class EventsService : IEventsService {
        private static Expression<Func<Event, Event>> Projection = (Event x) => new Event {
            Id = x.Id,
            Name = x.Name,
            Place = x.Place,
            Type = x.Type,
            EventDate = x.EventDate,
            MaxGuests = x.MaxGuests,
            IsArchived = x.IsArchived,
            InvitedGuestsCount = x.Guests.Count,
        };

        private AppDbContext _db;

        public EventsService(AppDbContext db) {
            _db = db;
        }

        public Task<Event[]> GetEvents() {
            return _db.Events
                .Where(x => x.IsArchived == false)
                .Select(Projection)
                .AsNoTracking()
                .ToArrayAsync();
        }

        public async Task<Event> CreateEvent(Event ev) {
            //todo: validation
            ev.IsArchived = false;
            _db.Events.Add(ev);
            await _db.SaveChangesAsync();
            return ev;
        }

        public async Task SetArchive(int eventId) {
            Event ev = await _db.Events.FindAsync(eventId);
            //todo: throw exception if not found
            ev.IsArchived = true;
            await _db.SaveChangesAsync();
        }

        public Task<Event> GetEvent(int eventId) {
            return _db.Events
                .Where(x => x.Id == eventId)
                .Select(Projection)
                .FirstOrDefaultAsync();
        }
    }
}
