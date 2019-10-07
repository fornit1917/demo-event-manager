using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using EventManager.ApiApp.Models;
using EventManager.ApiApp.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventManager.ApiApp.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase {
        private IEventsService _eventsService;
        private IGuestsService _guestsService;
        private readonly IHostingEnvironment _hostingEnvironment;

        public EventsController(IEventsService eventsService, IGuestsService guestsService, IHostingEnvironment hostingEnvironment) {
            _eventsService = eventsService;
            _guestsService = guestsService;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public async Task<Event[]> GetEvents() {
            //todo: add pagination
            return await _eventsService.GetEvents();
        }

        [HttpGet("{eventId}")]
        public async Task<Event> GetEvent(int eventId) {
            return await _eventsService.GetEvent(eventId);
        }

        [HttpPost]
        public async Task<Event> CreateEvent(Event ev) {
            return await _eventsService.CreateEvent(ev);
        }

        [HttpPut("{eventId}/archive")]
        public async Task SetArchive(int eventId) {
            await _eventsService.SetArchive(eventId);
        }

        [HttpGet("{eventId}/guests")]
        public Task<Guest[]> GetGuests(int eventId) {
            //todo: add pagination
            return _guestsService.GetGuests(eventId);
        }

        [HttpGet("{eventId}/guests-csv")]
        public async Task ExportGuests(int eventId) {
            Response.StatusCode = 200;
            Response.ContentType = "text/csv";
            Response.Headers.Add("Content-Disposition", "Attachment;filename=Guests.csv");
            using (var sw = new StreamWriter(Response.Body)) {
                await _guestsService.ExportToStream(eventId, sw);
            }
        }

        [HttpPut("{eventId}/guests-csv")]
        public async Task<GuestsImportResult> ImportGuests(int eventId, IFormFile file) {
            using (Stream stream = file.OpenReadStream()) {
                return await _guestsService.Import(eventId, stream);
            }
        }
    }
}