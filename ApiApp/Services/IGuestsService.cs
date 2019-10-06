using EventManager.ApiApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManager.ApiApp.Services {
    public interface IGuestsService {
        Task<Guest[]> GetGuests(int eventId);
    }
}
