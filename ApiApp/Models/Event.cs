using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace EventManager.ApiApp.Models {
    public class Event {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Place { get; set; }
        public DateTime EventDate { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public EventType Type { get; set; }
        
        public int MaxGuests { get; set; }
        public bool IsArchived { get; set; }
    }
}
