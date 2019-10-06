using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventManager.ApiApp.Models;
using EventManager.ApiApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventManager.ApiApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase {
        private IEventsService _eventsService;

        public EventsController(IEventsService eventsService) {
            _eventsService = eventsService;
        }

        [HttpGet]
        public async Task<Event[]> GetEvents() {
            return await _eventsService.GetEvents();
        }

        [HttpPost]
        public async Task<Event> CreateEvent(Event ev) {
            return await _eventsService.CreateEvent(ev);
        }

        [HttpPut("{eventId}/archive")]
        public async Task SetArchive(int eventId) {
            await _eventsService.SetArchive(eventId);
        }
    }
}