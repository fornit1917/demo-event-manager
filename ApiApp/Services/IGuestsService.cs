using EventManager.ApiApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EventManager.ApiApp.Services {
    public interface IGuestsService {
        Task<Guest[]> GetGuests(int eventId);
        Task ExportToStream(int eventId, StreamWriter sw);
    }
}
