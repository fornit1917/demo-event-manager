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
        private IEventsService _eventsService;

        public GuestsService(AppDbContext db, IEventsService eventsService) {
            _db = db;
            _eventsService = eventsService;
        }

        public Task<Guest[]> GetGuests(int eventId) {
            //todo: add pagination
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

        public async Task<GuestsImportResult> Import(int eventId, Stream stream) {
            using (var reader = new StreamReader(stream))
            using (var tr = _db.Database.BeginTransaction()) {
                Event ev = await _eventsService.GetEvent(eventId);
                int remainingSpaces = ev.MaxGuests - ev.InvitedGuestsCount;

                var csv = new CsvReader(reader);
                csv.Read();
                csv.ReadHeader();
                while (csv.Read()) {
                    string email = csv.GetField<string>(0);
                    string name = csv.GetField<string>(1);
                    string comment = csv.GetField<string>(2);
                    // todo: add validation

                    Guest guest = await _db.Guests.Where(x => x.Email == email && x.EventId == eventId).FirstOrDefaultAsync();
                    if (guest == null) {
                        remainingSpaces--;
                        if (remainingSpaces <= 0) {
                            tr.Rollback();
                            return new GuestsImportResult { IsSuccess = false, Message = "File can not be imported. Not enough free places." };
                        }
                        guest = new Guest { Name = name, Email = email, Comment = comment, EventId = eventId };
                        await _db.Guests.AddAsync(guest);
                    } else {
                        guest.Name = name;
                        guest.Comment = comment;
                    }
                }

                await _db.SaveChangesAsync();
                tr.Commit();

                return new GuestsImportResult { IsSuccess = true };
            }

        }
    }
}
