using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventManager.ApiApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EventManager.ApiApp.Services {
    public class EventsService : IEventsService {
        private AppDbContext _db;

        public EventsService(AppDbContext db) {
            _db = db;
        }

        public Task<Event[]> GetEvents() {
            return _db.Events.Where(ev => ev.IsArchived == false).ToArrayAsync();
        }

        public Task SetArchive(int eventId) {
            return Task.CompletedTask;
        }
    }
}
