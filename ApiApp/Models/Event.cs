using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManager.ApiApp.Models {
    public class Event {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Place { get; set; }
        public EventType Type { get; set; }
        public int MaxGuests { get; set; }
        public bool IsArchived { get; set; }
    }
}
