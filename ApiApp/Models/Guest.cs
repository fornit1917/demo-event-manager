using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManager.ApiApp.Models {
    public class Guest {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }

        public int EventId { get; set; }
    }
}
