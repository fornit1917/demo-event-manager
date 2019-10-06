using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using EventManager.ApiApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EventManager.ApiApp.Services {
    public class GuestsService : IGuestsService {
        private AppDbContext _db;

        public GuestsService(AppDbContext db) {
            _db = db;
        }

        public Task<Guest[]> GetGuests(int eventId) {
            return _db.Guests.Where(g => g.EventId == eventId).AsNoTracking().ToArrayAsync();
        }

        public async Task ExportToStream(int eventId, StreamWriter sw) {
            Guest[] guests = await GetGuests(eventId);
            var csvWriter = new CsvWriter(sw);
            csvWriter.WriteField("Email");
            csvWriter.WriteField("Name");
            csvWriter.WriteField("Comment");
            await csvWriter.NextRecordAsync();

            foreach (Guest guest in guests) {
                csvWriter.WriteField(guest.Email);
                csvWriter.WriteField(guest.Name);
                csvWriter.WriteField(guest.Comment);
                await csvWriter.NextRecordAsync();
            }
        }
    }
}
