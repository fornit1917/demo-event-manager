using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventManager.ApiApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EventManager.ApiApp.Services {
    public class GuestsService : IGuestsService {
        private AppDbContext _db;

        public GuestsService(AppDbContext db) {
            _db = db;
        }

        public Task<Guest[]> GetGuests(int eventId) {
            return _db.Guests.Where(g => g.EventId == eventId).ToArrayAsync();
        }
    }
}
