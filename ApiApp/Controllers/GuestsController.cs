using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventManager.ApiApp.Models;
using EventManager.ApiApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiApp.Controllers
{
    [Route("api")]
    [ApiController]
    public class GuestsController : ControllerBase {
        private IGuestsService _guestsService;

        public GuestsController(IGuestsService guestsService) {
            _guestsService = guestsService;
        }

        [HttpGet("event/{eventId}/guests")]
        public Task<Guest[]> GetGuests(int eventId) {
            return _guestsService.GetGuests(eventId);
        }
    }
}